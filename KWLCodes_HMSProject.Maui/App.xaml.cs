using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Services;
using KWLCodes_HMSProject.Maui.Pages;

namespace KWLCodes_HMSProject.Maui
{
    public partial class App : Application
    {
        public App(LoginService loginService) // Accept LoginService in the constructor
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LandingPage(loginService)); // Pass the LoginService
        }
    }
}
