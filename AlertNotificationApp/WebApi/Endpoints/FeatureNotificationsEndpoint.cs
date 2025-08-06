using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.DTO;
using WebApi.Models;

namespace WebApi.Endpoints;

public static class FeatureNotificationsEndpoint
{
    public static void MapFeatureNotificationEndpoints(this WebApplication app)
    {
        app.MapGet("/api/feature-notifications", async (AppDbContext dbContext, HttpContext httpContext) =>
        {
            var userId = httpContext.User.Identity?.Name ?? "anonymous";
            var dismissedNotificationIds = await dbContext.UserDismissedNotifications
                .Where(udn => udn.UserId == userId)
                .Select(udn => udn.FeatureNotificationId)
                .ToListAsync();

            var notifications = await dbContext.FeatureNotifications
                .Where(fn => !dismissedNotificationIds.Contains(fn.Id))
                .ToListAsync();

            return Results.Ok(notifications);
        })
        .WithName("GetFeatureNotifications")
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
        });

        app.MapPost("/api/feature-notifications/dismiss", async (DismissNotificationDto dto, AppDbContext dbContext, HttpContext httpContext) =>
        {
            var userId = httpContext.User.Identity?.Name ?? "anonymous";
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
        .WithName("DismissFeatureNotification")
        .WithTags("Feature Notifications");
    }
}
