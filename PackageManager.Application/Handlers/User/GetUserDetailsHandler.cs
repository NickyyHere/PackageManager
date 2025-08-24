using AutoMapper;
using FluentResults;
using MediatR;
using PackageManager.Application.DTO.Request;
using PackageManager.Application.DTO.Request.Queries.User;
using PackageManager.Application.DTO.Response;
using PackageManager.Core.Interfaces;

namespace PackageManager.Application.Handlers.User;

public class GetUserDetailsHandler(
    IUserRepository userRepository,
    CurrentUser currentUser,
    IMapper mapper
) : IRequestHandler<GetUserDetailsQuery, Result<UserDTO>>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly CurrentUser _currentUser = currentUser;
    private readonly IMapper _mapper = mapper;
    public async Task<Result<UserDTO>> Handle(GetUserDetailsQuery request, CancellationToken cancellationToken)
    {
        if (_currentUser.UserID == null)
        {
            return Result.Fail("Not logged in");
        }
        var userResult = await _userRepository.GetUserByIdAsync((Guid)_currentUser.UserID);
        if (userResult.IsFailed)
        {
            return Result.Fail(userResult.Errors);
        }
        var user = userResult.Value;
        var userDTO = _mapper.Map<UserDTO>(user);
        return userDTO;
    }
}
