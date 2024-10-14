using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Services;
using KWLCodes_HMSProject.Maui.Pages;

namespace KWLCodes_HMSProject.Maui
{
    public partial class App : Application
    {
        public App(LoginService loginService, AssignmentService assignmentService, FilesService filesService, FeedbackService feedbackService) // Updated constructor
        {
            InitializeComponent();

            // Pass all necessary services to LandingPage
            MainPage = new NavigationPage(new LandingPage(loginService, assignmentService, filesService, feedbackService));
        }
    }
}
