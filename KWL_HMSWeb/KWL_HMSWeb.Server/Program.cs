using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Azure.Identity;
using KWL_HMSWeb.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySQL(connectionString);

/*
// Add Azure App Configuration to the container.
var connectionString = builder.Configuration["DefaultConnection"];

if (!string.IsNullOrEmpty(connectionString))
{
    // Use the connection string if it is available.
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(connectionString)
        .ConfigureRefresh(refresh =>
        {
            // All configuration values will be refreshed if the sentinel key changes.
            refresh.Register("TestApp:Settings:Sentinel", refreshAll: true);
        });
    });
}
else if (Uri.TryCreate(builder.Configuration["Endpoints:DefaultConnection"], UriKind.Absolute, out var endpoint))
{
    // Use Azure Active Directory authentication.
    // The identity of this app should be assigned 'App Configuration Data Reader' or 'App Configuration Data Owner' role in App Configuration.
    // For more information, please visit https://aka.ms/vs/azure-app-configuration/concept-enable-rbac
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(endpoint, new VisualStudioCredential())
        .ConfigureRefresh(refresh =>
        {
            // All configuration values will be refreshed if the sentinel key changes.
            refresh.Register("TestApp:Settings:Sentinel", refreshAll: true);
        });
    });
}*/
//builder.Services.AddAzureAppConfiguration();

builder.Services.AddScoped<IVideoService, VideoService>();


/*
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    // Use the connection string from Azure App Configuration
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
*/

builder.Services.AddSingleton<BlobStorageService>();

builder.Services.AddControllers();

/*
builder.Services.AddEndpointsApiExplorer();
*/

var app = builder.Build();

/*
app.UseAzureAppConfiguration();
*/

app.UseAuthorization();

app.MapControllers();

app.Run();