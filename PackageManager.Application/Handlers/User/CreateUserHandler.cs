using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using PackageManager.Application.DTO.Request.Commands.User;
using PackageManager.Core.Interfaces;
using PackageManager.Core.Models;

namespace PackageManager.Application.Handlers.User;

public class CreateUserHandler(
    IUserRepository userRepository
) : IRequestHandler<CreateUserCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
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
        return await _userRepository.AddUserAsync(user);
    }
}
