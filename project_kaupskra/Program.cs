using Microsoft.EntityFrameworkCore;
using project_kaupskra.Models;
using System.Configuration;
using System;
using project_kaupskra.Interfaces;
using project_kaupskra.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using project_kaupskra.Data;
using project_kaupskra.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
    
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

//builder.Services.AddDbContext<FasteignakaupContext>(opt => opt.UseInMemoryDatabase("FasteignakaupList"));
builder.Services.AddDbContext<KaupsamningurDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;   
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 4;
}).AddEntityFrameworkStores<KaupsamningurDbContext>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    options.DefaultChallengeScheme =
    options.DefaultForbidScheme =
    options.DefaultScheme =
    options.DefaultSignInScheme =
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SigningKey"]))
    };
});


builder.Services.AddScoped<IAmountOfDealsRepository, AmountOfDealsRepository>();
builder.Services.AddScoped<IAveragePriceRepository, AveragePriceRepository>();
builder.Services.AddScoped<IAverageSquareMeterRepository, AverageSquareMeterRepository>();
builder.Services.AddScoped<IAveragePriceOfSquareMeterRepository, AveragePriceOfSquareMeterRepository>();
builder.Services.AddScoped<IKaupsamningarRepository, KaupsamningarRepository>();

builder.Services.AddScoped<ITokenService, TokenService>();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
