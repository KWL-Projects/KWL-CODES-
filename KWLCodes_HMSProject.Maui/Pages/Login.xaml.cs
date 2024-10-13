using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using Microsoft.Extensions.Logging; // Ensure this is included
using KWLCodes_HMSProject.Maui.Services; // Reference your FilesService
using System.Net.Http; // Reference for HttpClient

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class Login : ContentPage
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<FilesService> _logger;

        public Login(HttpClient httpClient, ILogger<FilesService> logger)
        {
            InitializeComponent();
            _httpClient = httpClient;
            _logger = logger;
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

            // Retrieve stored credentials
            string storedUsername = await SecureStorage.GetAsync("username");
            string storedPassword = await SecureStorage.GetAsync("password");

            // Validate credentials
            if (username == storedUsername && password == storedPassword)
            {
                Preferences.Set("IsLoggedIn", true);

                // Create FilesService instance
                var filesService = new FilesService(_httpClient, _logger);

                // Pass the FilesService to LandingPage
                await Navigation.PushAsync(new LandingPage(filesService));
            }
            else
            {
                await DisplayAlert("Error", "Invalid username or password", "OK");
            }
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            // Create FilesService instance
            var filesService = new FilesService(_httpClient, _logger);

            // Pass the FilesService to LandingPage
            await Navigation.PushAsync(new LandingPage(filesService));
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
