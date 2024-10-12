using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http; // Required for HttpClient

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
                client.BaseAddress = new Uri("http://your-api-url/"); // Set your API base URL
            });

            // Register HttpClient and FilesService
            builder.Services.AddHttpClient<LoginService>(client =>
            {
                client.BaseAddress = new Uri("http://your-api-url/"); // Set your API base URL
            });
            // Register HttpClient and FilesService
            builder.Services.AddHttpClient<AssignmentService>(client =>
            {
                client.BaseAddress = new Uri("http://your-api-url/"); // Set your API base URL
            });



#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
