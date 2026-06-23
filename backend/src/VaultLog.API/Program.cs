using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using VaultLog.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

var pgConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var redisConnectionString = builder.Configuration["Redis:ConnectionString"];

builder.Services.AddDbContext<VaultLogDbContext>(options => options.UseNpgsql(pgConnectionString));

builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(redisConnectionString ?? "localhost"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["Authentik:Authority"];
    options.Audience = builder.Configuration["Authentik:Audience"];
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
    };
    
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        }
    };
});
builder.Services.AddAuthorization();
builder.Services.AddFastEndpoints();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();

app.Run();
