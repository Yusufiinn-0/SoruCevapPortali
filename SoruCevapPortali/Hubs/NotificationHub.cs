using Microsoft.AspNetCore.SignalR;

namespace SoruCevapPortali.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task SendNotification(string message, string type = "info")
        {
            await Clients.All.SendAsync("ReceiveNotification", message, type);
        }

        public async Task SendToAdmin(string message, string type = "info")
        {
            await Clients.Group("Admins").SendAsync("ReceiveNotification", message, type);
        }

        public override async Task OnConnectedAsync()
        {
            if (Context.User?.IsInRole("Admin") == true)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            if (Context.User?.IsInRole("Admin") == true)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Admins");
            }
            await base.OnDisconnectedAsync(exception);
        }
    }
}


