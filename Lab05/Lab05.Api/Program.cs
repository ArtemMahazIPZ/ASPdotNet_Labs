using System.Text;
using Lab05.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 1. EF + твоя БД
builder.Services.AddDbContext<NewsDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"))
);

// 2. Identity (щоб через API теж можна було реєструвати / логінити)
builder.Services
    .AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<NewsDbContext>()
    .AddDefaultTokenProviders();

// 3. Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();      // опис ендпойнтів
builder.Services.AddSwaggerGen();                // сам Swagger

// 4. JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "SUPER_SECRET_KEY_12345";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "Lab05.Api";

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
