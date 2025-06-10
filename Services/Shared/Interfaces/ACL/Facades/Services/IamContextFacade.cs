using System.Text.Json;
using Shared.Interfaces.ACL.DTOs;
using Shared.Interfaces.ACL.Facades;

namespace Shared.Interfaces.ACL.Facades.Services;

public class IamContextFacade(HttpClient httpClient) : IIamContextFacade
{
    public async Task<bool> UserExists(int userId)
    {
        var endpoint = $"http://iam-service:8011/api/v1/users/user-exists/{userId}";
        var response = await httpClient.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return bool.Parse(content);
        }

        // Handle error response
        throw new Exception($"Error checking user existence: {response.StatusCode}");
    }

    public async Task<UserDTO> FetchUser(int userId)
    {
        var endpoint = $"http://iam-service:8011/api/v1/users/{userId}";
        var response = await httpClient.GetAsync(endpoint);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDTO>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        }

        // Handle error response
        throw new Exception($"Error checking user: {response.StatusCode}");
    }
}