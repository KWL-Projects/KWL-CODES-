using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using KWLCodes_HMSProject.Maui.Models; // Ensure this namespace includes your LoginRequest and LoginResponse models
using KWLCodes_HMSProject.Maui.Services; // Ensure you include the namespace for your LoginService

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class Login : ContentPage
    {
        private readonly LoginService _loginService;

        public Login(LoginService loginService) // Constructor with LoginService parameter
        {
            InitializeComponent();
            _loginService = loginService; // Store the service
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            // Create a LoginRequest object
            var loginRequest = new LoginRequest
            {
                Username = username,
                Password = password
            };

            // Authenticate using the LoginService
            var loginResponse = await _loginService.AuthenticateAsync(loginRequest);

            if (loginResponse.Token != null) // Check if the login was successful
            {
                Preferences.Set("IsLoggedIn", true);
                Preferences.Set("Token", loginResponse.Token); // Store the token if necessary
                await Navigation.PushAsync(new LandingPage(_loginService)); // Navigate to landing page
            }
            else
            {
                await DisplayAlert("Error", loginResponse.TokenError ?? "Invalid username or password", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            await Navigation.PushAsync(new LandingPage(_loginService)); // Pass the LoginService
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            await Navigation.PushAsync(new SignUp());
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
