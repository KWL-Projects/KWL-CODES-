using System;
using System.Threading.Tasks; // Ensure you have this using directive for Task
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using KWLCodes_HMSProject.Maui.Services;
using KWLCodes_HMSProject.Maui.Models;
using System.Net.Http; // Include HttpClient namespace if not already present

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class SignUp : ContentPage
    {
        private readonly UserService _userService; // Declare the UserService field

        public SignUp()
        {
            InitializeComponent();
            _userService = new UserService(new HttpClient()); // Instantiate UserService with a new HttpClient
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            // Retrieve data from entries
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;
            string firstName = FirstNameEntry.Text;
            string surname = SurnameEntry.Text;
            string userType = (string)UserTypePicker.SelectedItem; // Get the selected user type

            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) ||
                string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(surname) || string.IsNullOrEmpty(userType))
            {
                await DisplayAlert("Error", "Please fill in all fields", "OK");
                return;
            }

            // Create UserRegister object to send to API
            var userRegister = new UserRegister
            {
                username = username,
                password = password,
                user_firstname = firstName,
                user_surname = surname,
                user_type = userType
            };

            try
            {
                // Call the RegisterAsync method from UserService
                string token = await _userService.RegisterAsync(userRegister);

                // Save token securely (if applicable)
                await SecureStorage.SetAsync("token", token);

                await DisplayAlert("Success", "Account created successfully. Please log in.", "OK");

                // Navigate back to Login page
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Registration failed: {ex.Message}", "OK");
            }
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100); // Scale up
            await button.ScaleTo(1.0, 100); // Scale back down
        }
    }
}
