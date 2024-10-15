using Microsoft.EntityFrameworkCore;
using KWL_HMSWeb.Server.Models;
using Azure.Identity;
using KWL_HMSWeb.Services;
using System.Text.Json.Serialization;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DotNetEnv;
using KWL_HMSWeb.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64;
    });

// CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowWebClient", policy =>
        policy.WithOrigins("http://localhost:4200", "https://kwlcodes-ave7bddvd0bvg4f2.southafricanorth-01.azurewebsites.net") // Both URLs
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// Add BlobStorageService for dependency injection
builder.Services.AddScoped<BlobStorageService>();

// Configure DatabaseContext with SQL Server
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<NotificationService>();


// Load environment variables from the .env file
Env.Load("info.env");
builder.Configuration.AddEnvironmentVariables();

// Retrieve JWT settings from configuration
var jwtSecret = builder.Configuration["KWLCodes_JWT_SECRET"];
var jwtIssuer = builder.Configuration["KWLCodes_JWT_ISSUER"];
var jwtAudience = builder.Configuration["KWLCodes_JWT_AUDIENCE"];

// Configure JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = 401; // Unauthorized
                context.Response.ContentType = "application/json";
                return context.Response.WriteAsync("{\"error\":\"Unauthorized access\"}");
            }
        };
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

// Configure Azure App Configuration
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
        options.Connect(endpoint, new DefaultAzureCredential())
            .ConfigureRefresh(refresh =>
            {
                refresh.Register("TestApp:Settings:Sentinel", refreshAll: true);
            });
    });
}

builder.Services.AddAzureAppConfiguration();
builder.Services.AddHttpClient();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Build the app
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

// CORS policy
app.UseCors("AllowWebClient");

app.Use(async (context, next) =>
{
    // Log the requested URL
    Console.WriteLine($"Request Path: {context.Request.Path}");
    await next();
});

// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();

// Ensure to call UseAuthentication before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers to routes
app.MapControllers();

// Run the application
app.Run();
