using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using KWLCodes_HMSProject.Maui.Models; // Adjust the namespace for project structure

namespace KWLCodes_HMSProject.Maui.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        // Inject HttpClient through the constructor (dependency injection)
        public LoginService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Method to send login request to API
        public async Task<LoginResponse> AuthenticateAsync(LoginRequest loginRequest)
        {
            try
            {
                // Send the POST request to the API
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("authenticate", loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response and deserialize it into the LoginResponse model
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    return loginResponse;
                }
                else
                {
                    // Return a failure response
                    return new LoginResponse
                    {
                        Message = "Login failed",
                        Token = null,
                        TokenError = "Invalid username or password"
                    };
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an error response
                Console.WriteLine($"Error during authentication: {ex.Message}");
                return new LoginResponse
                {
                    Message = "Login failed due to an error",
                    Token = null,
                    TokenError = ex.Message
                };
            }
        }
    }
}
