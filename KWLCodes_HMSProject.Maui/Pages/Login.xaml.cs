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
        private readonly AssignmentService _assignmentService; // New service
        private readonly FilesService _filesService; // New service
        private readonly FeedbackService _feedbackService; // New service

        public Login(LoginService loginService, AssignmentService assignmentService, FilesService filesService, FeedbackService feedbackService) // Updated constructor
        {
            InitializeComponent();
            _loginService = loginService; // Store the service
            _assignmentService = assignmentService; // Store the service
            _filesService = filesService; // Store the service
            _feedbackService = feedbackService; // Store the service
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

                // Navigate to landing page with all necessary services
                await Navigation.PushAsync(new LandingPage(_loginService, _assignmentService, _filesService, _feedbackService));
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
            await Navigation.PushAsync(new LandingPage(_loginService, _assignmentService, _filesService, _feedbackService)); // Pass all services
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
