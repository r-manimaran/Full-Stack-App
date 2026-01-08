using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Endpoints;

public static class FeatureNotificationsEndpoint
{
    private static string GetUserId(HttpContext httpContext)
    {
        // Try to get user ID from Keycloak JWT claims
        // First try 'sub' claim (standard OIDC subject identifier)
        var subClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
            ?? httpContext.User.FindFirst("sub")?.Value;
        
        // Fallback to preferred_username or name
        if (string.IsNullOrEmpty(subClaim))
        {
            subClaim = httpContext.User.FindFirst(ClaimTypes.Name)?.Value
                ?? httpContext.User.FindFirst("preferred_username")?.Value
                ?? httpContext.User.Identity?.Name;
        }
        
        return subClaim ?? "anonymous";
    }

    public static void MapFeatureNotificationEndpoints(this WebApplication app)
    {
        app.MapGet("/api/feature-notifications", async (AppDbContext dbContext, HttpContext httpContext) =>
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            var userId = GetUserId(httpContext);
            var dismissedNotificationIds = await dbContext.UserDismissedNotifications
                .Where(udn => udn.UserId == userId)
                .Select(udn => udn.FeatureNotificationId)
                .ToListAsync();

            var notifications = await dbContext.FeatureNotifications
                .Where(fn => fn.ReleaseDate >= sevenDaysAgo && !dismissedNotificationIds.Contains(fn.Id))
                .OrderByDescending(fn => fn.ReleaseDate)
                .ToListAsync();

            return Results.Ok(notifications);
        })
        .RequireAuthorization()
        .WithName("GetFeatureNotifications")
        .WithTags("Feature Notifications");

        app.MapGet("/api/feature-notifications/unread-count", async (AppDbContext dbContext, HttpContext httpContext) =>
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);
            var userId = GetUserId(httpContext);
            var dismissedNotificationIds = await dbContext.UserDismissedNotifications
                .Where(udn => udn.UserId == userId)
                .Select(udn => udn.FeatureNotificationId)
                .ToListAsync();

            var count = await dbContext.FeatureNotifications
                .Where(fn => fn.ReleaseDate >= sevenDaysAgo && !dismissedNotificationIds.Contains(fn.Id))
                .CountAsync();

            return Results.Ok(new { count });
        })
        .RequireAuthorization()
        .WithName("GetUnreadNotificationCount")
        .WithTags("Feature Notifications");

        app.MapPost("/api/feature-notifications", async (FeatureNotificationDto request, AppDbContext dbContext) =>
        {
            FeatureNotifications newNotification = new FeatureNotifications()
            {
                Title = request.Title,
                Description = request.Description,
                ReleaseDate = request.ReleaseDate,
            };
            dbContext.FeatureNotifications.Add(newNotification);
            await dbContext.SaveChangesAsync();

            return Results.Ok(newNotification);
        })
        .RequireAuthorization()
        .WithName("CreateFeatureNotification")
        .WithTags("Feature Notifications");

        app.MapPost("/api/feature-notifications/dismiss", async (DismissNotificationDto dto, AppDbContext dbContext, HttpContext httpContext) =>
        {
            var userId = GetUserId(httpContext);
            var dismissedNotification = new UserDismissedNotifications
            {
                UserId = userId,
                FeatureNotificationId = dto.FeatureNotificationId,
                DismissedAt = DateTime.UtcNow
            };
            dbContext.UserDismissedNotifications.Add(dismissedNotification);
            await dbContext.SaveChangesAsync();
            return Results.Ok();
        })
        .RequireAuthorization()
        .WithName("DismissFeatureNotification")
        .WithTags("Feature Notifications");
    }
}
