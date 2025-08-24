using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PackageManager.Application.DTO.Request;
using PackageManager.Application.DTO.Request.Commands.User;
using PackageManager.Core.Interfaces;

namespace PackageManager.Application.Handlers.User;

public class DeleteUserHandler(
    IUserRepository userRepository,
    CurrentUser currentUser,
    IUnitOfWork unitOfWork,
    ILogger<DeleteUserHandler> logger
) : IRequestHandler<DeleteUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly CurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<DeleteUserHandler> _logger = logger;
    public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserID == null)
        {
            return Result.Fail("Not logged in");
        }
        var userResult = await _userRepository.GetUserByIdAsync((Guid)_currentUser.UserID);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }
        var user = userResult.Value;
        var transactionResult = await _unitOfWork.BeginTransactionAsync();
        if (transactionResult.IsFailed)
        {
            return Result.Fail(transactionResult.Errors);
        }
        user.IsDeleted = true;
        var commitResult = await _unitOfWork.CommitTransactionAsync();
        if (commitResult.IsFailed)
        {
            var rollbackResult = await _unitOfWork.RollbackTransactionAsync();
            if (rollbackResult.IsFailed)
            {
                _logger.LogCritical("Failed to rollback transaction at {Time}", DateTime.UtcNow);
                return Result.Fail(rollbackResult.Errors);
            }
            return Result.Fail(commitResult.Errors);
        }
        _logger.LogInformation("User {UserID} has been deleted at {Time}", user.ID, DateTime.UtcNow);
        return Result.Ok();
    }
}
