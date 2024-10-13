using System;
using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Services; // Add this to reference FilesService

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class ViewAssignments : ContentPage
    {
        private readonly FilesService _filesService; // Declare FilesService

        public ViewAssignments(FilesService filesService) // Inject FilesService
        {
            InitializeComponent();
            _filesService = filesService; // Assign the FilesService
            LoadAssignments();
        }

        private void LoadAssignments()
        {
            // Load Assignments Here
        }

        private async void OnUploadVideoClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            // Pass the FilesService when navigating to UploadVideo
            await Navigation.PushAsync(new UploadVideo(_filesService));
        }

        private async void OnViewVideoFeedbackClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            await Navigation.PushAsync(new ViewVideoFeedback());
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
