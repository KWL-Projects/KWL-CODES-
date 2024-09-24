using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
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

            Preferences.Set("IsLoggedIn", true);

            await Navigation.PushAsync(new LandingPage());
        }

        private async void OnCancelClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            await Navigation.PushAsync(new LandingPage());
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
