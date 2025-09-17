using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace SharedKernel;
public class UnitOfWorkInterceptor(IHttpContextAccessor _httpContextAccessor) : SaveChangesInterceptor
{
    private static bool _skipInterceptor;
    public static void SkipInterceptor() => _skipInterceptor = true;
    public static void ResumeInterceptor() => _skipInterceptor = false;

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
         DbContextEventData eventData,
         InterceptionResult<int> result,
         CancellationToken cancellationToken = default)
    {
        if (eventData.Context == null || _skipInterceptor)
            return await base.SavingChangesAsync(eventData, result, cancellationToken);


        var modifiedEntries = eventData.Context.ChangeTracker.Entries()
            .Where(e => e.State is EntityState.Modified && e.Entity is ITrackableEntity)
            .ToList();

        List<EntityChangeLog> changeLogs = [];
        List<PropertiesChanges> propsChangeLogs = [];
        string userId = GetUserId() ?? string.Empty;
        string ipAddress = GetUserIPAddress() ?? string.Empty;
        string userName = GetUserName() ?? string.Empty;

        foreach (var entry in modifiedEntries)
        {
            var entityName = entry.Entity.GetType().Name;

            var trackableProperties = ((ITrackableEntity)entry.Entity).GetTrackableProperties();

            foreach (var propName in trackableProperties)
            {
                var originalValue = entry.OriginalValues[propName];
                var currentValue = entry.CurrentValues[propName];

                if (Equals(originalValue, currentValue))
                    continue;

                propsChangeLogs.Add(new PropertiesChanges
                {
                    PropertyName = propName,
                    OldValue = originalValue?.ToString() ?? string.Empty,
                    NewValue = currentValue?.ToString() ?? string.Empty,
                });
            }

            if (propsChangeLogs.Count > 0)
            {
                changeLogs.Add(new EntityChangeLog
                {
                    EntityId = long.TryParse(entry.CurrentValues["Id"]?.ToString(), out var id) ? id : 0,
                    EntityName = entityName,
                    PropertiesChanges = propsChangeLogs.SerializeJson(),
                    ChangeTime = DateTimeOffset.UtcNow,
                    ChangeType = entry.State.ToString(),
                    UserId = userId ?? string.Empty,
                    IPAddress = ipAddress ?? string.Empty,
                    UserName = userName ?? string.Empty
                });
                propsChangeLogs = [];
            }
        }

        if (changeLogs.Count > 0)
            await eventData.Context.Set<EntityChangeLog>().AddRangeAsync(changeLogs, cancellationToken);

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private string? GetUserId() => _httpContextAccessor.HttpContext?.User?.FindFirst(PropertyConstants.Id)?.Value;
    private string? GetUserIPAddress() => _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
    private string? GetUserName() => _httpContextAccessor.HttpContext?.User?.FindFirst(PropertyConstants.UserName)?.Value;

}