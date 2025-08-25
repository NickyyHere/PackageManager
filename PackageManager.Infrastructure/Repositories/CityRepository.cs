using FluentResults;
using Microsoft.EntityFrameworkCore;
using PackageManager.Core.Interfaces;
using PackageManager.Core.Models;
using PackageManager.Infrastructure.Data;

namespace PackageManager.Infrastructure.Repositories;

public class CityRepository(
    AppDbContext context
) : ICityRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Result<List<City>>> GetCitiesAsync()
    {
        return await _context.Cities
            .Include(c => c.Country)
            .ToListAsync();
    }
}