using PicPaySimplificado.Domain;
using PicPaySimplificado.DTOs;

namespace PicPaySimplificado.Application;

public class Notification
{
    public void SendeNotification(Domain.Users user, string message)
    {
        string email = user.GetEmail();

        HttpClient httpClient = new HttpClient();
        NotificationRequestDto notificationRequest = new NotificationRequestDto(email, message);
        
        var response = httpClient.PostAsJsonAsync("https://util.devi.tools/api/v1/notify", notificationRequest);

        if (!response.IsCompleted)
        {
            throw new Exception("Notification service failed");
        }

        
    }
}