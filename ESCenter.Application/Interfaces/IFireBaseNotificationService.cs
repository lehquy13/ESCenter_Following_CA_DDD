namespace ESCenter.Application.Interfaces;

public interface IFireBaseNotificationService
{
    Task SendNotificationAsync(string title, string body, string token);
}