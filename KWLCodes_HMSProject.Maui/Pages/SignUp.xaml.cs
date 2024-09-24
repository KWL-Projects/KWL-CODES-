using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class SignUp : ContentPage
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
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

            // Save credentials securely
            await SecureStorage.SetAsync("username", username);
            await SecureStorage.SetAsync("password", password);

            // Debugging: Display saved credentials
            Console.WriteLine($"Saved Username: {username}");
            Console.WriteLine($"Saved Password: {password}");

            await DisplayAlert("Success", "Account created successfully. Please log in.", "OK");

            // Navigate back to Login page
            await Navigation.PopAsync();
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
