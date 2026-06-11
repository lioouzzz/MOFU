using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MOFU.Data;
using MOFU.Helper;
using MOFU.Interfaces;
using MOFU.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// 註冊 Swagger 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 註冊 UserService
builder.Services.AddScoped<IUserService, UserService>();

//註冊 AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

//註冊 ProjectService
builder.Services.AddScoped<IProjectService, ProjectService>();
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//DB連線
var connectionString= builder.Configuration.GetConnectionString("MOFU");
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));


//註冊 FileLogger
builder.Services.AddSingleton<FileLogger>();


//註冊 JwtService
builder.Services.AddScoped<JwtService>();

//jwt 驗證
var jwtKey = builder.Configuration["JWT:Key"];

builder.Services.AddAuthentication(options =>
{
    //預設用哪種方式辨認使用者
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        options.TokenValidationParameters=new
        TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

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
