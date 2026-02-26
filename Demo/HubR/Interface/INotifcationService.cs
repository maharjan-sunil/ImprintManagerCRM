namespace Demo.HubR.Interface
{
    public interface INotificationService
    {
        Task SendOrderCreatedAsync(string message);
       // Task SendOrderCreatedAsync(string userId, string orderId);
    }
}
