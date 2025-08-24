using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PackageManager.Core.Interfaces;
using PackageManager.Infrastructure.Data;
using PackageManager.Infrastructure.Repositories;
using PackageManager.Infrastructure.Services;

namespace PackageManager.Infrastructure;

public static class InfrastructureDependencyInjection
{
    public static WebApplicationBuilder AddInfrastructure(this WebApplicationBuilder builder, IConfiguration config)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(config.GetConnectionString("DefaultConnection")));

        builder.Services
            .AddScoped<IUnitOfWork, EfUnitOfWork>()
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IPackageRepository, PackageRepository>()
            .AddScoped<IMachineRepository, MachineRepository>()
            .AddScoped<ILockerSizeRepository, LockerSizeRepository>()
            .AddScoped<ILockerRepository, LockerRepository>()
            .AddScoped<ILockerCodeRepository, LockerCodeRepository>()
            .AddScoped<ICountryRepository, CountryRepository>()
            .AddScoped<ICityRepository, CityRepository>()
        ;
        return builder;
    }
}