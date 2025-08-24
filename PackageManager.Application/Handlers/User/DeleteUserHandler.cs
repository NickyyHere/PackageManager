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
        user.IsDeleted = true;
        var savingResult = await _unitOfWork.SaveChangesAsync();
        if (savingResult.IsFailed)
        {
            return Result.Fail(savingResult.Errors);
        }
        _logger.LogInformation("User {UserID} has been deleted at {Time}", user.ID, DateTime.UtcNow);
        return Result.Ok();
    }
}
