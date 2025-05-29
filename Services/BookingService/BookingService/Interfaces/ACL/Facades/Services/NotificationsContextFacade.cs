using System.Net.Http.Json;

namespace BookingService.Interfaces.ACL.Facades.Services
{
    public class NotificationsContextFacade(HttpClient httpClient) : INotificationsContextFacade
    {
        public async Task<int> CreateNotification(
            string title,
            string description,
            int userId
        )
        {
            var endpoint = "http://localhost:5277/api/v1/notifications";

            var notificationData = new
            {
                title,
                description,
                userId
            };

            var response = await httpClient.PostAsJsonAsync(endpoint, notificationData);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                
                return int.Parse(content);
            }

            // Manejo de error
            throw new Exception($"Error creating notification: {response.StatusCode}");
        }
    }
}