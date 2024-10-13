using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using KWLCodes_HMSProject.Maui.Models; // Adjust the namespace for your project structure

namespace KWLCodes_HMSProject.Maui.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> RegisterAsync(UserRegister userRegisterDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/register", userRegisterDto);
            response.EnsureSuccessStatusCode();

            // Adjust this according to the actual response structure
            var result = await response.Content.ReadFromJsonAsync<RegisterResponse>();
            return result.Token; // Assuming the response contains a Token field
        }

        public async Task<User> CreateUserAsync(User user)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user/create", user);
            response.EnsureSuccessStatusCode();

            // Adjust this according to the actual response structure
            var createdUser = await response.Content.ReadFromJsonAsync<User>();
            return createdUser; // Return the created User object directly
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("api/user/all");
            response.EnsureSuccessStatusCode();

            var users = await response.Content.ReadFromJsonAsync<List<User>>();
            return users; // Return the list of users directly
        }

        public async Task<User> GetUserAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/user/view/{id}");
            response.EnsureSuccessStatusCode();

            var user = await response.Content.ReadFromJsonAsync<User>();
            return user; // Return the User object directly
        }

        public async Task<bool> UpdateUserAsync(int id, User updatedUser)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/update/{id}", updatedUser);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/user/delete/{id}");
            return response.IsSuccessStatusCode;
        }
    }

    // If you need a specific response model for registration, you can create it like this:
    public class RegisterResponse
    {
        public string Token { get; set; }
    }
}
