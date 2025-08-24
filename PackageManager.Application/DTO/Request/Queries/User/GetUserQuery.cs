using FluentResults;
using MediatR;
using PackageManager.Application.DTO.Response;

namespace PackageManager.Application.DTO.Request.Queries.User;

public record GetUserQuery : IRequest<Result<UserDTO>>
{
    public Guid UserID { get; set; }
}