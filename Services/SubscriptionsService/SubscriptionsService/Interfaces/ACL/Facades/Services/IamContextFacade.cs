using SubscriptionsService.Interfaces.ACL.Facades;

namespace SubscriptionsService.Interfaces.ACL.Facades.Services;

public class IamContextFacade(HttpClient httpClient) : IIamContextFacade
{
    public async Task<bool> UserExists(int userId)
    {
        var endpoint = $"http://localhost:5271/api/v1/users/user-exists/{userId}";
        var response = await httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error checking user existence: {response.StatusCode}, Content: {errorContent}");
        }
        

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return bool.Parse(content);
        }

        // Handle error response
        throw new Exception($"Error checking user existence: {response.StatusCode}");
    }
}