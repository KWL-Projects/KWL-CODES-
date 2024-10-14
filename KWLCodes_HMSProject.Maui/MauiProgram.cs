using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http; // Required for HttpClient
using KWLCodes_HMSProject.Maui.Services;
using KWLCodes_HMSProject.Maui.Models;
using KWLCodes_HMSProject.Maui.Pages;

namespace KWLCodes_HMSProject.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>() // Using the App class
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register HttpClient for LoginService
            builder.Services.AddHttpClient<LoginService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074/"); // Set your API base URL
            });

            // Register HttpClient for other services if needed
            builder.Services.AddHttpClient<FilesService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074"); // Set your API base URL
            });

            builder.Services.AddHttpClient<AssignmentService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074"); // Set your API base URL
            });

            builder.Services.AddHttpClient<FeedbackService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074"); // Set your API base URL
            });

            builder.Services.AddHttpClient<UserService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074"); // Set your API base URL
            });

            // Register services as singletons
            builder.Services.AddSingleton<FilesService>();
            builder.Services.AddSingleton<LoginService>();
            builder.Services.AddSingleton<AssignmentService>();
            builder.Services.AddSingleton<FeedbackService>();
            builder.Services.AddSingleton<UserService>();

            // Register the App class with LoginService
            builder.Services.AddSingleton<App>();
            builder.Services.AddSingleton<UserService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
