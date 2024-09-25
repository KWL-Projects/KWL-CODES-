using Microsoft.EntityFrameworkCore;
using KWLCodesAPI.Models;

var builder = WebApplication.CreateBuilder(args);

//DotNetEnv.Env.Load(builder);

builder.Services.AddControllers();

//builder.Services.AddDbContext<DatabaseContext>(options =>
    //options.UseMySQL(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));


builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();
