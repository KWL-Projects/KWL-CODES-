using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http; // Required for HttpClient
using KWLCodes_HMSProject.Maui.Services;

namespace KWLCodes_HMSProject.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register HttpClient and FilesService
            builder.Services.AddHttpClient<FilesService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074"); // Set your API base URL
            });

            // Register HttpClient and FilesService
            builder.Services.AddHttpClient<LoginService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074/"); // Set your API base URL
            });
            // Register HttpClient and FilesService
            builder.Services.AddHttpClient<AssignmentService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074"); // Set your API base URL
            });

            builder.Services.AddHttpClient<FeedbackService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:7074"); // Set your API base URL
            });


            
            builder.Services.AddSingleton<FilesService>();
            builder.Services.AddSingleton<LoginService>();
            builder.Services.AddSingleton<AssignmentService>();
            builder.Services.AddSingleton<FeedbackService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
