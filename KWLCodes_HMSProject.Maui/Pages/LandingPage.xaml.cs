using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using KWLCodes_HMSProject.Maui.Services;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class LandingPage : ContentPage
    {
        private readonly LoginService _loginService;

        public LandingPage(LoginService loginService) // Accept LoginService in the constructor
        {
            InitializeComponent();
            _loginService = loginService; // Store the injected service
            CheckLoginState();
        }

        private async void CheckLoginState()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            if (isLoggedIn)
            {
                string username = await SecureStorage.GetAsync("username");
                string password = await SecureStorage.GetAsync("password");

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    LoginButton.IsVisible = false;
                    ViewAssignmentsButton.IsVisible = true;
                    LogoutButton.IsVisible = true;
                }
                else
                {
                    LoginButton.IsVisible = true;
                    ViewAssignmentsButton.IsVisible = false;
                    LogoutButton.IsVisible = false;
                }
            }
            else
            {
                LoginButton.IsVisible = true;
                ViewAssignmentsButton.IsVisible = false;
                LogoutButton.IsVisible = false;
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            await Navigation.PushAsync(new Login(_loginService)); // Pass the LoginService to Login
        }

        private async void OnViewAssignmentsClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            await Navigation.PushAsync(new ViewAssignments());
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            Preferences.Set("IsLoggedIn", false);
            SecureStorage.Remove("username");
            SecureStorage.Remove("password");

            CheckLoginState();
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
