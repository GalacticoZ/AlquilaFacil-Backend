using Shared.Interfaces.ACL.Facades;

namespace Shared.Interfaces.ACL.Facades.Services;

public class ProfilesContextFacade(HttpClient httpClient) : IProfilesContextFacade
{
    public async Task<int> CreateProfile(
        string name,
        string fatherName,
        string motherName,
        string dateOfBirth,
        string documentNumber,
        string phone,
        int userId
    )
    {
        var endpoint = "http://api-gateway:80/profiles/api/v1/profiles";

        var profileData = new
        {
            name,
            fatherName,
            motherName,
            dateOfBirth,
            documentNumber,
            phone,
            userId
        };
            
        var response = await httpClient.PostAsJsonAsync(endpoint, profileData);

        if (response.IsSuccessStatusCode)
        {
            var profileResource = await response.Content.ReadFromJsonAsync<ProfileResource>();
            return profileResource.Id;
        }


        throw new Exception($"Error creating profile: {response.StatusCode}");
    }

    public async Task<int> UpdateProfile(
        string name,
        string fatherName,
        string motherName,
        string dateOfBirth,
        string documentNumber,
        string phone,
        string bankAccountNumber,
        string interbankAccountNumber,
        int userId
    )
    {
        var endpoint = $"http://api-gateway:80/profiles/api/v1/profiles/{userId}";

        var profileData = new
        {
            name,
            fatherName,
            motherName,
            dateOfBirth,
            documentNumber,
            phone,
            bankAccountNumber,
            interbankAccountNumber,
            userId
        };
            
        var response = await httpClient.PutAsJsonAsync(endpoint, profileData);

        if (response.IsSuccessStatusCode)
        {
            var profileResource = await response.Content.ReadFromJsonAsync<ProfileResource>();
                        return profileResource.Id;
        }


        throw new Exception($"Error creating profile: {response.StatusCode}");
    }
}

public record ProfileResource(
    int Id, 
    string FullName, 
    string Phone, 
    string DocumentNumber, 
    string DateOfBirth
);