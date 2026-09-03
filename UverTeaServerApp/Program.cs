using System.Text;
using System.Threading.RateLimiting;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UverTeaServerApp.Shared.Behaviors;
using UverTeaServerApp.Shared.Caching;
using UverTeaServerApp.Shared.Data;
using UverTeaServerApp.Shared.Hubs;
using UverTeaServerApp.Shared.Middlewares;
using UverTeaServerApp.Shared.Security;
using UverTeaServerApp.Shared.Services;
using UverTeaServerApp.src.Feature.EmployeeModule.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddSingleton<AuditableEntityInterceptor>();

// 1. Register Database Context
builder.Services.AddDbContext<UvaTeaDbContext>((sp, options) =>
{
    var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
           .AddInterceptors(interceptor);
});

builder.Services.AddHealthChecks()
    .AddDbContextCheck<UvaTeaDbContext>("Database");

// 2. Register Redis Distributed Cache & CacheService
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});
builder.Services.AddScoped<ICacheService, CacheService>();

// 3. Register Services, MediatR, and Validators 
builder.Services.AddScoped<EmployeeLookupService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// 4. JWT Authentication & Authorization Configuration
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                  ?? throw new InvalidOperationException("JwtSettings is not configured in appsettings.json.");
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
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
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Register MediatR along with Pipeline Behaviors
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
    cfg.AddOpenBehavior(typeof(CachingBehavior<,>));
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
    cfg.AddOpenBehavior(typeof(TransactionBehavior<,>));
});

// Register FluentValidation Validators 
builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddRateLimiter(options =>
{
    // Configure Global Rate Limiter Policy
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 100,            // Max 100 requests per minute
                QueueLimit = 2,
                Window = TimeSpan.FromMinutes(1)
            }));

    // Custom response when Rate Limit is exceeded
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        context.HttpContext.Response.ContentType = "application/json";
        await context.HttpContext.Response.WriteAsync(
            "{\"error\": \"Too many requests. Please try again later.\"}", cancellationToken);
    };
});

// Register Application Services & SignalR before building the container
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddSignalR();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


// --- BUILD APPLICATION ---
var app = builder.Build();

// Configure the HTTP request pipeline
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Rate Limiter Middleware
app.UseRateLimiter();

// Authentication and Authorization Middlewares (Must be placed before MapControllers)
app.UseAuthentication();
app.UseAuthorization();

// Health Check Endpoint Mapping (Excluded from Rate Limiting)
app.MapHealthChecks("/health")
   .DisableRateLimiting();

// Controllers Mapping
app.MapControllers();

// SignalR Hub Mapping
app.MapHub<NotificationHub>("/hubs/notification");

app.Run();