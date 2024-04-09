using Microsoft.EntityFrameworkCore;
using ProjectInit.Persistence;
using ProjectInit.Shared.Constants;

namespace ProjectInit.API.Middlewares;

public class DatabaseMiddleware(
    RequestDelegate next,
    IServiceScopeFactory factory
)
{
    public async Task Invoke(HttpContext context)
    {
        var endpoint = context.GetEndpoint();
        if (endpoint is null)
        {
            await next(context);
            return;
        }

        foreach (var metadata in endpoint.Metadata)
        {
            if (metadata is not HttpMethodMetadata httpMethodMetadata) continue;

            var isGetMethod = httpMethodMetadata.HttpMethods
                .Any(x =>
                    string.Equals(
                        x,
                        ApiConstants.GetMethod,
                        StringComparison.OrdinalIgnoreCase
                    )
                );

            if (!isGetMethod) continue;

            using var scope = factory.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTrackingWithIdentityResolution;
        }

        await next(context);
    }
}