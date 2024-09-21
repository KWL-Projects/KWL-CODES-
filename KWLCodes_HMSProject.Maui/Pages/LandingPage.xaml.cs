using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Pages;

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
            await Navigation.PushAsync(new Login());
        }

        private async void OnUserAdminClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UserAdmin());
        }

        private async void OnListAssignmentsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssignmentList());
        }

        private async void OnCreateAssignmentClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AssignmentCreate());
        }
    }
}
