using System;
using KWLCodes_HMSProject.Maui.Pages;
using Microsoft.Maui.Controls;

namespace KWLCodes_HMSProject.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Navigate to the Login page
            await Navigation.PushAsync(new Login());
        }

        private async void OnSignUpClicked(object sender, EventArgs e)
        {
            // Navigate to the Sign Up page
            await Navigation.PushAsync(new SignUp());
        }
    }
}
