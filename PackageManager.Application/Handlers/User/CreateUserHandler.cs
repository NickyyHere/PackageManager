using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PackageManager.Application.DTO.Request.Commands.User;
using PackageManager.Core.Interfaces;
using PackageManager.Core.Models;

namespace PackageManager.Application.Handlers.User;

public class CreateUserHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateUserHandler> logger
) : IRequestHandler<CreateUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<CreateUserHandler> _logger = logger;
    public async Task<Result> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var hasher = new PasswordHasher<Core.Models.User>();
        var user = new Core.Models.User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            BirthDate = request.DateOfBirth
        };
        user.Password = hasher.HashPassword(user, request.Password);
        user.ID = Guid.NewGuid();
        var addUserResult = await _userRepository.AddUserAsync(user);
        if (addUserResult.IsFailed)
        {
            return Result.Fail(addUserResult.Errors);
        }
        var savingResult = await _unitOfWork.SaveChangesAsync();
        if (savingResult.IsFailed)
        {
            return Result.Fail(savingResult.Errors);
        }
        _logger.LogInformation("New user {userID} has been created at {Time}", user.ID, DateTime.UtcNow);
        return Result.Ok();
    }
}
