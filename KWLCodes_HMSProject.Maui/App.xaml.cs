﻿using Microsoft.Maui.Controls;

namespace KWLCodes_HMSProject.Maui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
        }
    }
}
