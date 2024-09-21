using System;
using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Pages;

namespace KWLCodes_HMSProject.Maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        private async void OnUserAdminButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserAdmin());
        }

        private async void OnListAssignmentsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssignmentList());
        }

        private async void OnCreateAssignmentButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssignmentCreate());
        }
    }
}
