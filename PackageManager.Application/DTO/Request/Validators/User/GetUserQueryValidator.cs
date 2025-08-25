using FluentValidation;
using PackageManager.Application.DTO.Request.Queries.User;

namespace PackageManager.Application.DTO.Validators.User;

public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
{
    public GetUserQueryValidator()
    {
        RuleFor(x => x.UserID)
            .NotEmpty();
    }
}
