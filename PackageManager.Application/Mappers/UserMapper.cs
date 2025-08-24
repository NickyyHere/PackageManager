using AutoMapper;
using PackageManager.Application.DTO.Response;
using PackageManager.Core.Models;

namespace PackageManager.Application.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserDTO>();
        // No DTO -> Entity since it needs custom logic (password hashing) and I rather to do it in handler
    }
}