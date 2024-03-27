using System.Reflection;
using Marten;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProjectInit.Shared.Constants;
using Weasel.Core;

namespace ProjectInit.Persistence.DatabaseRegistrations;

public static class DatabaseRegistration
{
    public static IServiceCollection AddDatabase(this IServiceCollection @this, IConfiguration configuration,
        ILoggerFactory iLoggerFactory, IHostEnvironment env)
    {
        @this.AddIdentity<IdentityUser, IdentityRole>(IdentitySetupAction(env))
            .AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();

        if (env.IsProduction())
        {
            @this.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString(DatabaseConstants.PostgreConnectionName)!,
                        b => b.MigrationsAssembly(
                            Assembly.GetCallingAssembly().FullName
                        ))
                    .UseLoggerFactory(iLoggerFactory);
            });
        }
        else if (env.IsDevelopment())
        {
            @this.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(DatabaseConstants.InMemoryDatabaseName));
        }

        @this.AddMarten(options =>
        {
            options.Connection(configuration.GetConnectionString(DatabaseConstants.MartenConnectionName)!);

            if (env.IsDevelopment()) options.AutoCreateSchemaObjects = AutoCreate.All;
        });

        return @this;
    }

    private static Action<IdentityOptions>? IdentitySetupAction(IHostEnvironment env)
        =>
            env.IsProduction()
                ? new Action<IdentityOptions>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = false;
                })
                : env.IsDevelopment()
                    ? new Action<IdentityOptions>(options =>
                    {
                        options.Password.RequireDigit = false;
                        options.Password.RequiredLength = 4;
                        options.Password.RequireLowercase = false;
                        options.Password.RequireUppercase = false;
                        options.Password.RequireNonAlphanumeric = false;
                    })
                    : null;
}