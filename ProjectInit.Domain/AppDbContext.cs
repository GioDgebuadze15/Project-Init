using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectInit.Domain.Entities.Common;

namespace ProjectInit.Domain;

public class AppDbContext
    : IdentityDbContext<IdentityUser>
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
        : base(options)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added
                        && e.Entity is BaseEntity);

        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (!string.IsNullOrWhiteSpace(userId))
        {
            foreach (var entity in entities)
            {
                if (entity.Entity is not BaseEntity updatedEntity) continue;

                updatedEntity.CreatedBy ??= userId;

                updatedEntity.UpdatedBy = userId;
                updatedEntity.UpdatedAt = DateTime.Now;
            }
        }

        var response = await base.SaveChangesAsync(cancellationToken);
        if (response <= 0) throw new DbUpdateConcurrencyException("No changes were saved to the database.");

        return response;
    }
}