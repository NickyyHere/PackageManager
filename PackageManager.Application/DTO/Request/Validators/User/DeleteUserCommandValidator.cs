using FluentValidation;
using PackageManager.Application.DTO.Request.Commands.User;

namespace PackageManager.Application.DTO.Validators.User;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(99);
        RuleFor(x => x.RepeatPassword)
            .MinimumLength(5)
            .MaximumLength(90)
            .NotEmpty();
    }
}
