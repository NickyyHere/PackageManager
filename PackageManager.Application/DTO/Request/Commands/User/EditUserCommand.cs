using FluentResults;
using MediatR;

namespace PackageManager.Application.DTO.Request.Commands.User;

public record EditUserCommand : IRequest<Result>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
}