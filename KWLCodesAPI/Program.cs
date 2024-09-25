using Microsoft.EntityFrameworkCore;
using KWLCodesAPI.Models;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);

/*var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

//var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("The connection string 'DefaultConnection' is missing or empty");
}

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySQL(connectionString));*/

// Add Azure App Configuration to the container.
var azAppConfigConnection = builder.Configuration["DefaultConnectionString"];
if (!string.IsNullOrEmpty(azAppConfigConnection))
{
    // Use the connection string if it is available.
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(azAppConfigConnection)
        .ConfigureRefresh(refresh =>
        {
            // All configuration values will be refreshed if the sentinel key changes.
            refresh.Register("TestApp:Settings:Sentinel", refreshAll: true);
        });
    });
}
else if (Uri.TryCreate(builder.Configuration["Endpoints:DefaultConnectionString"], UriKind.Absolute, out var endpoint))
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
}
builder.Services.AddAzureAppConfiguration();

//DotNetEnv.Env.Load(builder);

//builder.Services.AddDbContext<DatabaseContext>(options =>
//options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

/*builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));
*/

//builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers();

var app = builder.Build();
app.UseAzureAppConfiguration();

app.UseAuthorization();

app.MapControllers();

app.Run();
