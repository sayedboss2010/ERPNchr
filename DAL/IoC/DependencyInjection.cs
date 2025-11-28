using EF.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.IoC;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services,
     IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DBConnection")));
        return services;
    }
}