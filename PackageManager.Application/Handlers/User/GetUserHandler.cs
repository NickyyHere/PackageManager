using AutoMapper;
using FluentResults;
using MediatR;
using PackageManager.Application.DTO.Request;
using PackageManager.Application.DTO.Request.Queries.User;
using PackageManager.Application.DTO.Response;
using PackageManager.Core.Enums;
using PackageManager.Core.Interfaces;

namespace PackageManager.Application.Handlers.User;

public class GetUserHandler(
    IUserRepository userRepository,
    CurrentUser currentUser,
    IMapper mapper
) : IRequestHandler<GetUserQuery, Result<UserDTO>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly CurrentUser _currentUser = currentUser;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<UserDTO>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        if (_currentUser.Role == null)
        {
            return Result.Fail("Unable to read role");
        }
        if ((Role)_currentUser.Role != Role.ADMIN)
        {
            return Result.Fail("Unauthorized");
        }
        var userResult = await _userRepository.GetUserByIdAsync(request.UserID);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }
        var user = userResult.Value;
        var userDTO = _mapper.Map<UserDTO>(user);
        return userDTO;
    }
}
