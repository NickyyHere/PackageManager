using FluentValidation;
using PackageManager.Application.DTO.Request.Queries.User;

namespace PackageManager.Application.DTO.Validators.User;

public class GetUserDetailsQueryValidator : AbstractValidator<GetUserDetailsQuery>
{
    public GetUserDetailsQueryValidator(){}
}
