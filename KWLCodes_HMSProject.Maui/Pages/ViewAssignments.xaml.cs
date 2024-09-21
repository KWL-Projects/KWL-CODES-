using System;
using Microsoft.Maui.Controls;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class ViewAssignments : ContentPage
    {
        public ViewAssignments()
        {
            InitializeComponent();
            LoadAssignments(); // Method to load assignments if you have one
        }

        private void LoadAssignments()
        {
            // Load your assignments into AssignmentsListView here
        }

        private async void OnUploadVideoClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new UploadVideo());
        }

        private async void OnViewVideoFeedbackClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewVideoFeedback());
        }
    }
}
