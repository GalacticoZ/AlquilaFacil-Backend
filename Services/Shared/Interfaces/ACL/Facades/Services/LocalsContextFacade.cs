using System.Text.Json;
using Shared.Interfaces.ACL.DTOs;
using Shared.Interfaces.ACL.Facades;

namespace Shared.Interfaces.ACL.Facades.Services;

public class LocalsContextFacade(HttpClient httpClient) : ILocalsContextFacade
{

    public async Task<bool> LocalExists(int localId)
    {
        var endpoint = $"http://api-gateway:80/locals/api/v1/locals/{localId}";
        var response = await httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error checking local existence: {response.StatusCode}");
        }
        var content = await response.Content.ReadAsStringAsync();
        if(string.IsNullOrEmpty(content))
        {
            return false;
        }
        return true;
    }

    public async Task<IEnumerable<LocalDTO>> GetLocalsByUserId(int userId)
    {
        var endpoint = $"http://api-gateway:80/iam/api/v1/locals/get-user-locals/{userId}";
        var response = await httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching locals: {response.StatusCode}");
        }
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<LocalDTO>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
    }

    public async Task<bool> IsLocalOwner(int userId, int localId)
    {
        var endpoint = $"http://api-gateway:80/locals/api/v1/locals/get-user-locals/{userId}";
        var response = await httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error checking local ownership: {response.StatusCode}");
        }
        var content = await response.Content.ReadAsStringAsync();
        var locals = JsonSerializer.Deserialize<IEnumerable<LocalDTO>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        if (locals == null || !locals.Any())
        {
            return false;
        }
        return true;
    }
    
    public async Task<int> GetLocalOwnerIdByLocalId(int localId)
    {
        var endpoint = $"http://api-gateway:80/locals/api/v1/locals/owner/{localId}";
        var response = await httpClient.GetAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Error fetching local owner ID: {response.StatusCode}");
        }
        var content = await response.Content.ReadAsStringAsync();
        return Convert.ToInt32(content);
    }
}