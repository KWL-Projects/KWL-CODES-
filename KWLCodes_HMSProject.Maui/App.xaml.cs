using Microsoft.Maui.Controls;
using KWLCodes_HMSProject.Maui.Pages;

namespace KWLCodes_HMSProject.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
