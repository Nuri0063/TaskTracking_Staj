using Microsoft.AspNetCore.SignalR;
namespace TaskTracking.Staj.Hubs
{
    public class NotificationHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var connectionId = Context.ConnectionId;

            // Bağlanan kişi hariç herkese mesaj gönder
            await Clients.Others.SendAsync("userConnected", connectionId);

            // İstersen bağlı olana da mesaj yollayabilirsin: await Clients.Caller.SendAsync("welcome", ...);

            await base.OnConnectedAsync(); // baz metodun çalışmasını da sağla
        }

        //Görev üzerinde bir işlem yapıldığında
        public async Task NotifyTaskUpdate()
        {
            await Clients.Others.SendAsync("TasksUpdated");
        }
    }
}
