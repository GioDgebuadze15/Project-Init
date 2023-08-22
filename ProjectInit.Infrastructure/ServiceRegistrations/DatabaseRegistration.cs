using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectInit.Domain;

namespace ProjectInit.Infrastructure.ServiceRegistrations;

public static class DatabaseRegistration
{
    //Todo:move this into constants file
    private const string PostgreConnectionName = "DefaultDb";
    private const string InMemoryDatabaseName = "DefaultDb";

    public static IServiceCollection AddDatabase(this IServiceCollection @this, IConfiguration configuration,
        ILoggerFactory iLoggerFactory, IHostEnvironment env)
    {
        // @this.AddIdentity<IdentityUser, IdentityRole>(options =>
        //     {
        //         options.Password.RequireNonAlphanumeric = false;
        //         options.Password.RequiredLength = 8;
        //     })
        //     .AddEntityFrameworkStores<AppDbContext>()
        //     .AddDefaultTokenProviders();

        if (env.IsProduction())
        {
            @this.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(PostgreConnectionName)!,
                        b => b.MigrationsAssembly(
                            Assembly.GetCallingAssembly().FullName
                        ))
                    .UseLoggerFactory(iLoggerFactory);
            });
        }
        else if (env.IsDevelopment())
        {
            @this.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(InMemoryDatabaseName));
        }

        return @this;
    }
}