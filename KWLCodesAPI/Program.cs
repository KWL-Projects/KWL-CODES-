using Microsoft.EntityFrameworkCore;
using KWLCodesAPI.Models;
using Azure.Identity;
using KWLCodesAPI;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine(connectionString + "Is the connection string------------------------------------------------------------------------------------------------------------------------");

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();