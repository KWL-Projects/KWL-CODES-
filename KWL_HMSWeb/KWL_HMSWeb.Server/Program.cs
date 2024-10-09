using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Azure.Identity;
using KWL_HMSWeb.Services;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

// Configure CORS policy to allow your web client to communicate with the API.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", policy =>
        policy.WithOrigins("https://localhost:7074") // Replace with your actual web client URL
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// Configure DatabaseContext with SQL Server.
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Load environment variables from the .env file
Env.Load("info.env");

// Set up configuration to include environment variables
builder.Configuration.AddEnvironmentVariables();

// Retrieve JWT settings from configuration
var jwtSecret = builder.Configuration["KWLCodes_JWT_SECRET"];
var jwtIssuer = builder.Configuration["KWLCodes_JWT_ISSUER"];
var jwtAudience = builder.Configuration["KWLCodes_JWT_AUDIENCE"];

// Retrieve environment variables for JWT configuration.
//var jwtSecret = Env.GetString("KWLCodes_JWT_SECRET");
//var jwtIssuer = Env.GetString("KWLCodes_JWT_ISSUER");
//var jwtAudience = Env.GetString("KWLCodes_JWT_AUDIENCE");

// Log to confirm loading of variables
//Console.WriteLine($"JWT Secret: {jwtSecret}");
//Console.WriteLine($"JWT Issuer: {jwtIssuer}");
//Console.WriteLine($"JWT Audience: {jwtAudience}");

// Configure JWT Authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
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

// Create DefaultAzureCredential instance for Azure services.
var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ExcludeVisualStudioCredential = true
});

// Configure Azure App Configuration.
var azAppConfigConnection = builder.Configuration["DefaultConnection"];
if (!string.IsNullOrEmpty(azAppConfigConnection))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(azAppConfigConnection)
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("TestApp:Settings:Sentinel", refreshAll: true);
            });
    });
}
else if (Uri.TryCreate(builder.Configuration["Endpoints:DefaultConnection"], UriKind.Absolute, out var endpoint))
{
    builder.Configuration.AddAzureAppConfiguration(options =>
    {
        options.Connect(endpoint, credential)
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("TestApp:Settings:Sentinel", refreshAll: true);
            });
    });
}

builder.Services.AddAzureAppConfiguration();
builder.Services.AddHttpClient();
builder.Services.AddScoped<IServices, VideoService>();
builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

// Build the app.
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

// Use Developer Exception Page in Development mode.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Use configured CORS policy.
app.UseCors("AllowWebClient");

app.UseHttpsRedirection();

// Ensure to call UseAuthentication before UseAuthorization.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();


