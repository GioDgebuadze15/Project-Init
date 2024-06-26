﻿using System.Collections.Immutable;
using System.Security.Claims;
using System.Threading.Channels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectInit.Domain.Aggregates.Common;
using ProjectInit.Domain.Entities;
using ProjectInit.Domain.Entities.Common;
using ProjectInit.Domain.Handlers.NotificationHandler;
using Wolverine;

namespace ProjectInit.Persistence;

public class AppDbContext(
    DbContextOptions<AppDbContext> options,
    IMessageBus bus,
    IHttpContextAccessor httpContextAccessor,
    Channel<INotification> channel
)
    : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<FileEntity> Files { get; init; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
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
                modifiedEntity.CreateOrUpdate(userId);
            }

            modifiedEntity.Update();
        }

        await ProduceDomainEvents();

        var response = await base.SaveChangesAsync(cancellationToken);
        entities.ForEach(entity => entity.State = EntityState.Detached);

        if (response <= 0) throw new DbUpdateConcurrencyException();

        return response;
    }

    private async Task ProduceDomainEvents()
    {
        var domainEntities = ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(x => x.Entity.DomainEvents.Count is not 0)
            .Select(x => x.Entity)
            .ToImmutableList();

        var result = domainEntities
            .SelectMany(x => x.DomainEvents)
            .ToImmutableArray();

        domainEntities.ForEach(entity => entity.ClearDomainEvents());

        foreach (var notification in result)
        {
            await channel.Writer.WriteAsync(notification);
        }
    }
}