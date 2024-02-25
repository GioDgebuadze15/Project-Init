using System.Collections.Immutable;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectInit.Domain.Entities;
using ProjectInit.Domain.Entities.Common;

namespace ProjectInit.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor)
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<FileEntity> Files { get; init; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified or EntityState.Added
                        && e.Entity is BaseEntity)
            .ToImmutableList();

        var userId = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        foreach (var entity in entities)
        {
            if (entity.Entity is not BaseEntity modifiedEntity) continue;

            if (!string.IsNullOrWhiteSpace(userId))
            {
                modifiedEntity.CreatedBy ??= userId;
                modifiedEntity.UpdatedBy = userId;
            }

            modifiedEntity.UpdatedAt = DateTimeOffset.Now;
        }

        var response = await base.SaveChangesAsync(cancellationToken);
        entities.ForEach(entity => entity.State = EntityState.Detached);

        if (response <= 0) throw new DbUpdateConcurrencyException();

        return response;
    }
}