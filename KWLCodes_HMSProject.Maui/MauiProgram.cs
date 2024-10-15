using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http; // Required for HttpClient
using KWLCodes_HMSProject.Maui.Services;
using KWLCodes_HMSProject.Maui.Models;
using KWLCodes_HMSProject.Maui.Pages;
using Serilog; // Import Serilog namespace

namespace KWLCodes_HMSProject.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            // Configure Serilog with more detailed settings
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug() // Set minimum log level
                .WriteTo.File(
                    "log/log-.txt", // Log file path
                    rollingInterval: RollingInterval.Day, // Create a new log file each day
                    retainedFileCountLimit: 7, // Retain up to 7 log files
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] {Message}{NewLine}{Exception}", // Custom output format
                    fileSizeLimitBytes: 10 * 1024 * 1024, // 10MB file size limit before rolling
                    rollOnFileSizeLimit: true // Enable rolling on file size limit
                )
                .CreateLogger();

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

            // Add Serilog to the logging system
            builder.Logging.AddSerilog();

            return builder.Build();
        }
    }
}
