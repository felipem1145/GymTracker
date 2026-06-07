using GymTracker.Application.Common.Interfaces;
using GymTracker.Application.Common.Behaviors;
using GymTracker.Infrastructure.Persistence;
using GymTracker.WebApi.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);
var supabaseAuthUrl = builder.Configuration["Supabase:AuthUrl"]
    ?? throw new InvalidOperationException("Missing configuration key 'Supabase:AuthUrl'.");

var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
if (allowedOrigins is null || allowedOrigins.Length == 0)
{
    var allowedOriginsCsv = builder.Configuration["AllowedOrigins"];
    if (!string.IsNullOrWhiteSpace(allowedOriginsCsv))
    {
        allowedOrigins = allowedOriginsCsv
            .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    }
}

supabaseAuthUrl = supabaseAuthUrl.Trim().TrimEnd('/');

if (supabaseAuthUrl.EndsWith("/rest/v1", StringComparison.OrdinalIgnoreCase))
{
    supabaseAuthUrl = $"{supabaseAuthUrl[..^"/rest/v1".Length]}/auth/v1";
}
else if (!supabaseAuthUrl.EndsWith("/auth/v1", StringComparison.OrdinalIgnoreCase))
{
    supabaseAuthUrl = $"{supabaseAuthUrl}/auth/v1";
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddHealthChecks();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = supabaseAuthUrl;
        options.MapInboundClaims = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidAudience = "authenticated",
            NameClaimType = ClaimTypes.NameIdentifier
        };
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");

                logger.LogWarning(
                    context.Exception,
                    "JWT authentication failed for {Method} {Path}.",
                    context.Request.Method,
                    context.Request.Path);

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");

                logger.LogWarning(
                    "JWT challenge for {Method} {Path}. Error: {Error}. Description: {Description}.",
                    context.Request.Method,
                    context.Request.Path,
                    context.Error,
                    context.ErrorDescription);

                return Task.CompletedTask;
            }
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<IApplicationDbContext>();
    cfg.AddOpenBehavior(typeof(UserSyncBehavior<,>));
});

// Register EF Core with PostgreSQL (Supabase)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IApplicationDbContext>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (allowedOrigins is { Length: > 0 })
        {
            var origins = allowedOrigins;
            policy.WithOrigins(origins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
            return;
        }

        if (builder.Environment.IsDevelopment())
        {
            policy.WithOrigins("http://localhost:5173", "http://localhost:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
            return;
        }

        throw new InvalidOperationException(
            "CORS configuration is missing. Set AllowedOrigins in configuration for non-development environments.");
    });
});

// ... abajo antes de app.Run()

var app = builder.Build();

app.UseRouting();

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();
