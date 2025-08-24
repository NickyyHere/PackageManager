using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using PackageManager.Application.DTO.Request;
using PackageManager.Application.DTO.Request.Commands.User;
using PackageManager.Core.Interfaces;

namespace PackageManager.Application.Handlers.User;

public class EditUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    CurrentUser currentUser,
    Logger<EditUserHandler> logger
) : IRequestHandler<EditUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly CurrentUser _currentUser = currentUser;
    private readonly Logger<EditUserHandler> _logger = logger;
    public async Task<Result> Handle(EditUserCommand request, CancellationToken cancellationToken)
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
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.BirthDate = request.DateOfBirth;
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
        _logger.LogInformation("User {UserID} was edited at {Time}", user.ID, DateTime.UtcNow);
        return Result.Ok();
    }
}
