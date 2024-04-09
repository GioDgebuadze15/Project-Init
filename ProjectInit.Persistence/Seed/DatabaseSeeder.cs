using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ProjectInit.Persistence.Seed;

public static class DatabaseSeeder
{
    public static IApplicationBuilder Seed(this IApplicationBuilder @this)
    {
        var ctx = @this.ApplicationServices.GetRequiredService<AppDbContext>();
        ApplyPendingMigrations(ctx);

        return @this;
    }

    private static void ApplyPendingMigrations(AppDbContext ctx)
    {
        if (ctx.Database.IsRelational() && ctx.Database.GetPendingMigrations().Any())
        {
            ctx.Database.Migrate();
        }
    }
}