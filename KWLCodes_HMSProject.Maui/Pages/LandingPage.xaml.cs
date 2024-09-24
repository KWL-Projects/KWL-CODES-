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

        private void CheckLoginState()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            LoginButton.IsVisible = !isLoggedIn;
            ViewAssignmentsButton.IsVisible = isLoggedIn;
            LogoutButton.IsVisible = isLoggedIn;
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
