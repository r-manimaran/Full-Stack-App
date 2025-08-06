namespace WebApi.Models;

public class FeatureNotifications
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime ReleaseDate { get; set; }
}

public class UserDismissedNotifications
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int FeatureNotificationId { get; set; }
    public DateTime DismissedAt { get; set; }
}

