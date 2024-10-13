using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Services;
using KWLCodes_HMSProject.Maui.Models;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class ViewAssignments : ContentPage
    {
        private readonly AssignmentService _assignmentService; // Declare AssignmentService
        private readonly FilesService _filesService; // Declare FilesService
        private readonly FeedbackService _feedbackService; // Declare FeedbackService

        public ViewAssignments(AssignmentService assignmentService, FilesService filesService, FeedbackService feedbackService) // Inject services
        {
            InitializeComponent();
            _assignmentService = assignmentService; // Assign the AssignmentService
            _filesService = filesService; // Assign the FilesService
            _feedbackService = feedbackService; // Assign the FeedbackService
            LoadAssignments();
        }

        private async void LoadAssignments()
        {
            var assignments = await _assignmentService.GetAllAssignmentsAsync(); // Fetch assignments
            AssignmentsListView.ItemsSource = assignments; // Bind assignments to the ListView
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

            // Get the selected assignment from the ListView
            var selectedAssignment = AssignmentsListView.SelectedItem as Assignment; // Ensure Assignment model is used
            if (selectedAssignment != null)
            {
                int assignmentId = selectedAssignment.assignment_id; // Assuming this is how you get the submission ID
                await Navigation.PushAsync(new ViewVideoFeedback(assignmentId, _feedbackService)); // Pass submissionId and FeedbackService
            }
            else
            {
                await DisplayAlert("Error", "Please select an assignment first.", "OK"); // Handle no selection case
            }
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
