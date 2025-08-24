using FluentResults;
using MediatR;

namespace PackageManager.Application.DTO.Request.Commands.User;

public record ChangePasswordCommand : IRequest<Result>
{
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
    public string RepeatNewPassword { get; set; } = string.Empty;
}