using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PackageManager.Application.DTO.Request;
using PackageManager.Application.DTO.Request.Commands.User;
using PackageManager.Core.Interfaces;

namespace PackageManager.Application.Handlers.User;

public class ChangePasswordHandler(
    IUserRepository userRepository,
    CurrentUser currentUser,
    IUnitOfWork unitOfWork,
    ILogger<ChangePasswordHandler> logger
) : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly CurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<ChangePasswordHandler> _logger = logger;
    public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserID == null)
        {
            return Result.Fail("Not logged in");
        }
        if (request.NewPassword != request.RepeatNewPassword)
        {
            return Result.Fail("Passwords do not match");
        }
        var userResult = await _userRepository.GetUserByIdAsync((Guid)_currentUser.UserID);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }
        var user = userResult.Value;

        var hasher = new PasswordHasher<Core.Models.User>();
        var verificationResult = hasher.VerifyHashedPassword(user, user.Password, request.CurrentPassword);
        if (verificationResult == PasswordVerificationResult.Failed)
        {
            return Result.Fail("Incorrect current password");
        }
        var newPassword = hasher.HashPassword(user, request.NewPassword);
        user.Password = newPassword;
        var savingResult = await _unitOfWork.SaveChangesAsync();
        if (savingResult.IsFailed)
        {
            return Result.Fail(savingResult.Errors);
        }
        _logger.LogInformation("User {UserID} changed password at {Time}", user.ID, DateTime.UtcNow);
        return Result.Ok();
    }
}
