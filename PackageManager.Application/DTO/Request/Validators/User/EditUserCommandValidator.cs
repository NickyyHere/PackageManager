using FluentValidation;
using PackageManager.Application.DTO.Request.Commands.User;

namespace PackageManager.Application.DTO.Validators.User;

public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
{
    public EditUserCommandValidator()
    {
        RuleFor(x => x.LastName)
            .MinimumLength(2)
            .MaximumLength(90)
            .NotEmpty();
        RuleFor(x => x.FirstName)
            .MinimumLength(2)
            .MaximumLength(90)
            .NotEmpty();
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        RuleFor(x => x.DateOfBirth)
            .LessThan(DateOnly.FromDateTime(DateTime.UtcNow))
            .NotEmpty();
    }
}
