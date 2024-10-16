using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using KWLCodes_HMSProject.Maui.Services;

namespace KWLCodes_HMSProject.Maui.Pages
{
    public partial class LandingPage : ContentPage
    {
        private readonly LoginService _loginService;
        private readonly AssignmentService _assignmentService; // New service
        private readonly FilesService _filesService; // New service
        private readonly FeedbackService _feedbackService; // New service

        public LandingPage(LoginService loginService, AssignmentService assignmentService, FilesService filesService, FeedbackService feedbackService) // Accept required services in the constructor
        {
            InitializeComponent();
            _loginService = loginService; // Store the injected services
            _assignmentService = assignmentService; // Store the injected services
            _filesService = filesService; // Store the injected services
            _feedbackService = feedbackService; // Store the injected services
            CheckLoginState();
        }

        private async void CheckLoginState()
        {
            bool isLoggedIn = Preferences.Get("IsLoggedIn", false);
            if (isLoggedIn)
            {
                string username = await SecureStorage.GetAsync("username");
                string password = await SecureStorage.GetAsync("password");

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    LoginButton.IsVisible = false;
                    ViewAssignmentsButton.IsVisible = true;
                    LogoutButton.IsVisible = true;
                }
                else
                {
                    LoginButton.IsVisible = true;
                    ViewAssignmentsButton.IsVisible = false;
                    LogoutButton.IsVisible = false;
                }
            }
            else
            {
                LoginButton.IsVisible = true;
                ViewAssignmentsButton.IsVisible = false;
                LogoutButton.IsVisible = false;
            }
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            // Pass all the necessary services to the Login page
            await Navigation.PushAsync(new Login(_loginService, _assignmentService, _filesService, _feedbackService));
        }

        private async void OnViewAssignmentsClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);
            // Pass the necessary services to ViewAssignments
            await Navigation.PushAsync(new ViewAssignments(_assignmentService, _filesService, _feedbackService));
        }

        private async void OnLogoutClicked(object sender, EventArgs e)
        {
            var button = (Button)sender;
            await AnimateButton(button);

            Preferences.Set("IsLoggedIn", false);
            SecureStorage.Remove("username");
            SecureStorage.Remove("password");

            CheckLoginState();
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.1, 100);
            await button.ScaleTo(1.0, 100);
        }
    }
}
