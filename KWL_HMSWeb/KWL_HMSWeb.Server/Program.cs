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

// Create a DefaultAzureCredential instance
var credential = new DefaultAzureCredential(new DefaultAzureCredentialOptions
{
    ExcludeVisualStudioCredential = true
});

// Load environment variables
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

builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Configure services
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.MaxDepth = 64; // You can adjust this if needed
    });

builder.Services.AddHttpClient();

builder.Services.AddScoped<IServices, VideoService>();
builder.Services.AddSingleton<BlobStorageService>();
builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddSwaggerGen();

var app = builder.Build();

/*if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}*/

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Make sure to call UseAuthentication before UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
