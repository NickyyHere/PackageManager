using FluentResults;
using Microsoft.EntityFrameworkCore;
using PackageManager.Core.Interfaces;
using PackageManager.Core.Models;
using PackageManager.Infrastructure.Data;

namespace PackageManager.Infrastructure.Repositories;

public class UserRepository(
    AppDbContext context
    ) : IUserRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Result> AddUserAsync(User user)
    {
        try
        {
            await _context.AddAsync(user);
        }
        catch (Exception ex)
        {
            return Result.Fail(new Error("Failed to add tracking to entity").CausedBy(ex));
        }
        return Result.Ok();
    }

    public async Task<Result> DeleteUserByIdAsync(Guid userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.ID == userId && !u.IsDeleted);
        if (user == null)
        {
            return Result.Fail($"User with ID {userId} does not exist");
        }
        user.IsDeleted = true;
        return Result.Ok();
    }

    public async Task<Result<User>> GetUserByEmail(string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        if (user == null)
        {
            return Result.Fail($"User with email {email} does not exist");
        }
        return user;
    }
}
