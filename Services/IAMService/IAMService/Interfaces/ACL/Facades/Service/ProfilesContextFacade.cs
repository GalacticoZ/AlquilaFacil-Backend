using IAMService.Interfaces.ACL.Facades;

namespace IAMService.Interfaces.ACL.Facades.Service;

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
        var endpoint = "http://localhost:5274/api/v1/profiles";

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
            var content = await response.Content.ReadAsStringAsync();
                
            return int.Parse(content);
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
        var endpoint = $"http://localhost:5274/api/v1/profiles/{userId}";

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
            var content = await response.Content.ReadAsStringAsync();
                
            return int.Parse(content);
        }


        throw new Exception($"Error creating profile: {response.StatusCode}");
    }
}