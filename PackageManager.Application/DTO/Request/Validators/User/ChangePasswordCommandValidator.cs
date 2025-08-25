using FluentValidation;
using PackageManager.Application.DTO.Request.Commands.User;

namespace PackageManager.Application.DTO.Validators.User;

public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(99);
        RuleFor(x => x.NewPassword)
            .MinimumLength(5)
            .MaximumLength(90)
            .NotEmpty();
        RuleFor(x => x.RepeatNewPassword)
            .MinimumLength(5)
            .MaximumLength(90)
            .NotEmpty();
    }
}
