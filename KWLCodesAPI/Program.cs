using Microsoft.EntityFrameworkCore;
using KWLCodesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

//DotNetEnv.Env.Load(builder);

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
