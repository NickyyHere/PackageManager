using FluentResults;
using MediatR;
using PackageManager.Application.DTO.Response;

namespace PackageManager.Application.DTO.Request.Queries.User;

public record GetUserDetailsQuery : IRequest<Result<UserDTO>>{}