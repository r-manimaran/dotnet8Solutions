using CompanyApi.Models;
using CompanyApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json;

namespace CompanyApi.Interceptors;

public class AuditTrialInterceptor : SaveChangesInterceptor
{
    private readonly ILogger<AuditTrialInterceptor> _logger;
    private readonly ICurrentUserService _currentUserService;
    public AuditTrialInterceptor(ICurrentUserService currentUserService, ILogger<AuditTrialInterceptor> logger)
    {
        _currentUserService = currentUserService;
        _logger = logger;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, 
                    InterceptionResult<int> result, 
                    CancellationToken cancellationToken = default)
    {
        UpdateAuditFields(eventData.Context);
        return ValueTask.FromResult(result);
    }
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, 
                                                          InterceptionResult<int> result)
    {
        UpdateAuditFields(eventData.Context);
        return result;
    }

    private void UpdateAuditFields(DbContext context)
    {
        if (context == null) return;

        var timestamp = DateTime.UtcNow;
        var userId = _currentUserService.UserId;
        var userName = _currentUserService.UserName;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.Entity is IAuditable &&
                (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var audit = (IAuditable)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                audit.CreatedBy = userId;
                audit.CreatedDate = timestamp;

                _logger.LogInformation(
                    "New entity {EntityType} created by {UserName} at {Timestamp}",
                    entry.Entity.GetType().Name,
                    userName,
                    timestamp);
            }
            else
            {
                // Don't modify the CreatedBy/CreatedDate on updates
                context.Entry(audit).Property(x => x.CreatedBy).IsModified = false;
                context.Entry(audit).Property(x => x.CreatedDate).IsModified = false;

                _logger.LogInformation(
                    "Entity {EntityType} modified by {UserName} at {Timestamp}",
                    entry.Entity.GetType().Name,
                    userName,
                    timestamp);
            }

            audit.ModifiedBy = userId;
            audit.ModifiedDate = timestamp;

            // Log to Audit Table
            TrackChanges(context, entry);
        }
    }

    private void TrackChanges(DbContext context, EntityEntry entry)
    {
        var auditLog = new AuditLog
        {
            UserId = _currentUserService.UserId,
            UserName = _currentUserService.UserName,
            EntityName = entry.Entity.GetType().Name,
            Action = entry.State.ToString(),
            Timestamp = DateTime.UtcNow,
            OldValues = entry.State == EntityState.Modified ?
                JsonSerializer.Serialize(entry.OriginalValues.ToObject()) : null,
            NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject())
        };

        context.Set<AuditLog>().Add(auditLog);
    }

}
