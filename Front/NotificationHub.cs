using Microsoft.AspNetCore.SignalR;

namespace Front
{
    public class NotificationHub : Hub
    {
        public async Task SendUserCreationNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveUserCreation", message);
        }

        public async Task SendNotification(int id, string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}