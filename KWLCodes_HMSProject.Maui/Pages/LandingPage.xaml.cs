using System;
using Microsoft.Maui.Controls; 

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class LandingPage : ContentPage
    {
        public LandingPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Navigate to the Login page
            await Navigation.PushAsync(new Login());
        }

        private async void OnViewAssignmentsClicked(object sender, EventArgs e)
        {
            // Navigate to the View Assignments page
            await Navigation.PushAsync(new ViewAssignments());
        }
    }
}
