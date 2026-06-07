using Microsoft.EntityFrameworkCore;
using MOFU.Data;
using MOFU.Helper;
using MOFU.Interfaces;
using MOFU.Services;

var builder = WebApplication.CreateBuilder(args);


// 註冊 Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊 UserService
builder.Services.AddScoped<IUserService, UserService>();



// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//DB連線
var connectionString= builder.Configuration.GetConnectionString("MOFU");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


//註冊 FileLogger
builder.Services.AddSingleton<FileLogger>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
