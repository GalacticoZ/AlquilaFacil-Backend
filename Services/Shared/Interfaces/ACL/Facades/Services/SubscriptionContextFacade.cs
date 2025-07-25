using System.Text.Json;
using Shared.Interfaces.ACL.DTOs;
using Shared.Interfaces.ACL.Facades;

namespace Shared.Interfaces.ACL.Facades.Services;

public class SubscriptionContextFacade(HttpClient httpClient) : ISubscriptionContextFacade
{
    public async Task<IEnumerable<SubscriptionDTO>> GetSubscriptionByUserIdsList(List<int> usersId)
    {
        var endpoint = $"http://api-gateway:80/subscriptions/api/v1/subscriptions/subscriptions/by-users";
        // put the query param as array with usersId=
        var query = string.Join("&", usersId.Select(id => $"usersId={id}"));
        endpoint += $"?{query}";
        var response = await httpClient.GetAsync(endpoint);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching subscriptions: {response.StatusCode}");
        }

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<SubscriptionDTO>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }
}