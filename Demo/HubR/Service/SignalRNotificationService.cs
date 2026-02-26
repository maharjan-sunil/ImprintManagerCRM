using Demo.HubR.Interface;
using Microsoft.AspNetCore.SignalR;

namespace Demo.HubR.Service
{
    public class SignalRNotificationService : INotificationService
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendOrderCreatedAsync(string message)
        {
            // "ReceiveMessage" matches the JS listener
            await _hubContext
                .Clients
                .All
                .SendAsync("ReceiveMessage", message);
        }



        //public class SignalRNotificationService : BackgroundService
        //{
        //    private readonly IHubContext<NotificationHub> _hubContext;

        //    public SignalRNotificationService(IHubContext<NotificationHub> hubContext)
        //    {
        //        _hubContext = hubContext;
        //    }

        //    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //    {
        //        while (!stoppingToken.IsCancellationRequested)
        //        {
        //            await _hubContext.Clients.All.SendAsync(
        //                "ReceiveMessage",
        //                $"Server time: {DateTime.Now}");

        //            await Task.Delay(5000, stoppingToken);
        //        }
        //    }
        //}

    }
}
