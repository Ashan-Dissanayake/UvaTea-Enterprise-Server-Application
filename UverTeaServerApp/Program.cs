using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.Shared.Caching;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Hubs;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.Shared.Security;
using UverTeaServerApp.Shared.Services;
using UverTeaServerApp.src.Feature.EmployeeModule.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "UvaTea Enterprise Platform API";
        document.Info.Version = "v1";
        document.Info.Description =
            "Production RESTful API for Uva Tea Factory Operations " +
            "(Manufacturing, Harvesting, Agronomy, and Distribution).";

        return Task.CompletedTask;
    });
});

builder.Services.AddSingleton<AuditableEntityInterceptor>();

// ============================================================
// DATABASE
// ============================================================

builder.Services.AddDbContext<UvaTeaDbContext>((sp, options) =>
{
    var interceptor =
        sp.GetRequiredService<AuditableEntityInterceptor>();

    options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"))
        .AddInterceptors(interceptor);
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<UvaTeaDbContext>("Database");

// ============================================================
// REDIS CACHE
// ============================================================

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");
});

builder.Services.AddScoped<ICacheService, CacheService>();

// ============================================================
// APPLICATION SERVICES
// ============================================================

builder.Services.AddScoped<EmployeeLookupService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ============================================================
// HTTP CONTEXT / CURRENT USER
// ============================================================

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICurrentUser, CurrentUser>();

// ============================================================
// JWT CONFIGURATION
// ============================================================

var jwtSettings =
    builder.Configuration
        .GetSection(JwtSettings.SectionName)
        .Get<JwtSettings>()
    ?? throw new InvalidOperationException(
        "JwtSettings is not configured in appsettings.json.");

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// ============================================================
// AUTHENTICATION & AUTHORIZATION
// ============================================================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;

    options.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,

        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Secret)),

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ============================================================
// MEDIATR & PIPELINE BEHAVIORS
// ============================================================

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);

    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

// ============================================================
// FLUENT VALIDATION
// ============================================================

builder.Services.AddValidatorsFromAssembly(
    typeof(Program).Assembly);

// ============================================================
// MVC / CONTROLLERS
// ============================================================

builder.Services.AddControllers();

// ============================================================
// EXCEPTION HANDLING
// ============================================================

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

// ============================================================
// RATE LIMITING
// ============================================================

builder.Services.AddRateLimiter(options =>
{
    options.GlobalLimiter =
        PartitionedRateLimiter.Create<HttpContext, string>(
            httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey:
                        httpContext.User.Identity?.Name
                        ?? httpContext.Request.Headers.Host.ToString(),

                    factory: partition =>
                        new FixedWindowRateLimiterOptions
                        {
                            AutoReplenishment = true,
                            PermitLimit = 100,
                            QueueLimit = 2,
                            Window = TimeSpan.FromMinutes(1)
                        }));

    options.OnRejected = async (
        context,
        cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode =
            StatusCodes.Status429TooManyRequests;

        context.HttpContext.Response.ContentType =
            "application/json";

        await context.HttpContext.Response.WriteAsync(
            "{\"error\": \"Too many requests. Please try again later.\"}",
            cancellationToken);
    };
});

// ============================================================
// SIGNALR
// ============================================================

builder.Services.AddSignalR();

// ============================================================
// BUILD APPLICATION
// ============================================================

var app = builder.Build();

// ============================================================
// HTTP REQUEST PIPELINE
// ============================================================

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.MapScalarApiReference(options =>
    {
        options.Title = "UvaTea Enterprise API Reference";
        options.Theme =
            Scalar.AspNetCore.ScalarTheme.DeepSpace;
    });
}

app.UseHttpsRedirection();

// Rate Limiter
app.UseRateLimiter();

// Authentication MUST come before Authorization
app.UseAuthentication();

app.UseAuthorization();

// ============================================================
// ENDPOINTS
// ============================================================

app.MapHealthChecks("/health")
    .DisableRateLimiting();

app.MapControllers();

app.MapHub<NotificationHub>(
    "/hubs/notification");

app.Run();