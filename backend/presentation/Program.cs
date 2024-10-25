
using infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Application.Interfaces;
using Application.mapper;
using Serilog;
using infrastructure.Repositories;
using infrastructure.Repositories.SmsService;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;


/*dont forget today deploy
--project infrastructure/infrastructure.csproj --startup-project presentation/presentation.csproj
/* 

implement Message Controller
signalir 
frontend
deploy
*/
/*
using test 
add caller
add story 
use redis
use .net caching methods
use database performance methods INDEX , PRODEDURAL
*/



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(typeof(MapperConfig));




builder.Services.AddScoped<IAuthMangaer, AuthManager>();
builder.Services.AddScoped<IUserManager, UserManager>();
builder.Services.AddScoped<IChatManager, ChatManager>();
builder.Services.AddTransient<ISmsSender, AuthMessageSender>();
builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
builder.Services.Configure<SMSoptions>(builder.Configuration);
builder.Services.AddScoped<IImageKitService, ImageKitService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
});


builder.Services.AddDbContext<ApplicationDbContext>(
        options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddIdentity<ApiUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddTokenProvider<DataProtectorTokenProvider<ApiUser>>("Test")
    .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
options.Password.RequireDigit = true;
options.Password.RequireLowercase = true;
options.Password.RequireNonAlphanumeric = true;
options.Password.RequireUppercase = true;
options.Password.RequiredLength = 6;
options.Password.RequiredUniqueChars = 1;

options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
options.Lockout.MaxFailedAccessAttempts = 5;
options.Lockout.AllowedForNewUsers = true;

options.User.AllowedUserNameCharacters =
"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
options.User.RequireUniqueEmail = false;
});

builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["jwtSettings:Key"] ?? string.Empty)),

            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true
        };
    });


builder.Services.AddAuthorization();

builder.Host.UseSerilog((ctx, lc) => lc.WriteTo.Console().ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
  {
      c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
      c.RoutePrefix = string.Empty;
  });
}

app.UseCors("AllowAll");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();


