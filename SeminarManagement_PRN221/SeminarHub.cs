using Microsoft.AspNetCore.SignalR;

namespace SeminarManagement_PRN221
{
    public class SeminarHub : Hub
    {
        public async Task NotifyTicketUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveTicketUpdate", message);
        }
    }
}
