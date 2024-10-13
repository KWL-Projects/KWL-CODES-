using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
//using KWL_HMSWeb.Server.Models; // Update this namespace according to your project structure
using Newtonsoft.Json; // Install-Package Newtonsoft.Json

namespace KWLCodes_HMSProject.Maui.Services
{
    public class LoginService
    {
        private readonly HttpClient _httpClient;

        public LoginService()
        {
            // Initialize HttpClient (you can use dependency injection or instantiate it here)
            _httpClient = new HttpClient
            {
                // Set the base address of your API
                BaseAddress = new Uri("https://your-api-url.com/api/login/")
            };
        }

        // Login request model
        public class LoginRequest
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        // Login response model
        public class LoginResponse
        {
            public string message { get; set; }
            public string token { get; set; }
            public string tokenError { get; set; }
        }

        // Method to send login request to API
        public async Task<LoginResponse> AuthenticateAsync(string username, string password)
        {
            // Create the login request object
            var loginRequest = new LoginRequest
            {
                username = username,
                password = password
            };

            // Convert the request object to JSON
            var jsonContent = new StringContent(JsonConvert.SerializeObject(loginRequest), Encoding.UTF8, "application/json");

            // Send the POST request to the API
            HttpResponseMessage response = await _httpClient.PostAsync("authenticate", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                // Read the response and deserialize it into the LoginResponse model
                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                // Return the deserialized response
                return loginResponse;
            }
            else
            {
                // Handle the error (e.g., return an error response)
                return new LoginResponse
                {
                    message = "Login failed",
                    token = null,
                    tokenError = "Invalid username or password"
                };
            }
        }
    }
}