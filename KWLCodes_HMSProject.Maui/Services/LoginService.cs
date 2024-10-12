using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
//using KWL_HMSWeb.Server.Models; // Update this namespace according to your project structure
using Newtonsoft.Json; // Install-Package Newtonsoft.Json

public class LoginService
{
    private readonly HttpClient _httpClient;

    public LoginService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Login method
    public async Task<LoginResponse> AuthenticateAsync(LoginService login)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync("api/login/authenticate", login);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonConvert.DeserializeObject<LoginResponse>(jsonResponse);
                return loginResponse; // Successful login response
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonConvert.DeserializeObject<ErrorResponse>(errorResponse);
                throw new Exception(error.Message); // Throw an error with the server's response
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions (network issues, etc.)
            throw new Exception("An error occurred while authenticating. " + ex.Message);
        }
    }
}

// Response models
public class LoginResponse
{
    public string Message { get; set; }
    public string Token { get; set; }
    public string TokenError { get; set; } // Optional
}

public class ErrorResponse
{
    public string Message { get; set; }
}