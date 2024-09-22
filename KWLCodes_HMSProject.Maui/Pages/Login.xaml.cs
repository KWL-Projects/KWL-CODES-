using System;
using Microsoft.Maui.Controls;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        // Handler for the Login Button Click
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            // Simple validation - Replace this with real authentication logic later
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            // Navigate to LandingPage (assuming login is successful)
            await Navigation.PushAsync(new LandingPage());
        }

        // Handler for the Cancel Button Click (goes back to Landing Page)
        private async void OnCancelClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LandingPage());
        }
    }
}
