using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // Add this namespace for Preferences

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();

            // Hide the ViewAssignmentsButton until the user is logged in
            CheckLoginState();
        }

        // This method checks if the user is logged in and updates the buttons accordingly
        private void CheckLoginState()
        {
            // Use Preferences to check if the user is logged in
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false); // Default to false if not found

            LoginButton.IsVisible = !isLoggedIn;
            ViewAssignmentsButton.IsVisible = isLoggedIn;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            // Button animation (scale up and down)
            await button.ScaleTo(1.1, 100);  // Slightly enlarge button
            await button.ScaleTo(1.0, 100);  // Return to normal size

            await Navigation.PushAsync(new Login());
        }

        private async void OnViewAssignmentsClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            // Button animation (scale up and down)
            await button.ScaleTo(1.1, 100);  // Slightly enlarge button
            await button.ScaleTo(1.0, 100);  // Return to normal size

            await Navigation.PushAsync(new ViewAssignments());
        }
    }
}
