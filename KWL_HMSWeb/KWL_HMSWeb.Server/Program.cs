using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Azure.Identity;
using KWL_HMSWeb.Services;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//DotNetEnv.Env.Load();

Env.Load("info.env");

// Retrieve environment variables
var jwtSecret = Env.GetString("KWLCodes_JWT_SECRET");
var jwtIssuer = Env.GetString("KWLCodes_JWT_ISSUER");
var jwtAudience = Env.GetString("KWLCodes_JWT_AUDIENCE");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

// Add Azure App Configuration to the container.
var azAppConfigConnection = builder.Configuration["DefaultConnection"];
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
}
builder.Services.AddAzureAppConfiguration();

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    // Use the connection string from Azure App Configuration
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64; // You can adjust this if needed
    });

/*// Retrieve the Blob Storage connection string from configuration
string? blobConnectionString = builder.Configuration.GetConnectionString("ConnectionString");

if (string.IsNullOrEmpty(blobConnectionString))
{
    throw new InvalidOperationException("Azure Blob Storage connection string is missing or empty. Please check your configuration.");
}

// Register BlobServiceClient
builder.Services.AddSingleton(new BlobServiceClient(blobConnectionString));*/

builder.Services.AddControllers();

builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddScoped<IServices, VideoService>();

builder.Services.AddSingleton<BlobStorageService>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

if(app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.UseAzureAppConfiguration();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();