using FluentValidation;
using PackageManager.Application.DTO.Request.Queries.User;

namespace PackageManager.Application.DTO.Validators.User;

public class AuthorizeUserQueryValidator : AbstractValidator<AuthorizeUserQuery>
{
    public AuthorizeUserQueryValidator()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty();
        RuleFor(x => x.Password)
            .MinimumLength(5)
            .MaximumLength(90)
            .NotEmpty();
    }
}
