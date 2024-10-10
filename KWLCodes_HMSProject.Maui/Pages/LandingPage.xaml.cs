using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
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
                    // Automatically log the user in
                    LoginButton.IsVisible = false;
                    ViewAssignmentsButton.IsVisible = true;
                    LogoutButton.IsVisible = true;
                }
                else
                {
                    // Show login button if credentials are not found
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
            await Navigation.PushAsync(new Login());
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

            // Clear login state
            Preferences.Set("IsLoggedIn", false);
            SecureStorage.Remove("username");
            SecureStorage.Remove("password");

            // Update UI
            CheckLoginState();
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
