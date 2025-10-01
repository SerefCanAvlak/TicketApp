using DefaultCorsPolicyNugetPackage;
using GenericRepository;
using log4net;
using log4net.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Ticketing.Application;
using Ticketing.Application.Services;
using Ticketing.Domain.Entities;
using Ticketing.Infrastructure;
using Ticketing.Infrastructure.Context;
using Ticketing.Infrastructure.Services;
using Ticketing.WebAPI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// ==================== Logger ====================
builder.Services.AddSingleton<ILogService, LogService>();
var entryAssembly = Assembly.GetEntryAssembly();
if (entryAssembly == null)
    throw new InvalidOperationException("Entry assembly could not be determined.");

var logRepository = LogManager.GetRepository(entryAssembly);
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
var logger = LogManager.GetLogger(typeof(Program));
logger.Info("Sunucu baþlatýldý");

// ==================== Services ====================
builder.Services.AddDefaultCors();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddExceptionHandler<ExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddScoped<IRepository<Notification>, Repository<Notification, ApplicationDbContext>>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// ==================== JWT Authentication ====================
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = builder.Configuration["Keycloak:Authority"];
        options.RequireHttpsMetadata = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Keycloak:Authority"],
            ValidateAudience = true,
            ValidAudiences = new[] { "ticketingapi", "account" },
            NameClaimType = "name",
            RoleClaimType = "role", // We'll map this below
        };

        // Map Keycloak roles to ASP.NET Core roles
        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                var claimsIdentity = context.Principal?.Identity as System.Security.Claims.ClaimsIdentity;
                if (claimsIdentity != null)
                {
                    var resourceAccessClaim = context.Principal?.FindFirst("resource_access");
                    var resourceAccess = resourceAccessClaim?.Value;
                    if (!string.IsNullOrEmpty(resourceAccess))
                    {
                        var parsed = System.Text.Json.JsonDocument.Parse(resourceAccess);
                        if (parsed.RootElement.TryGetProperty("ticketingapi", out var ticketingApi))
                        {
                            if (ticketingApi.TryGetProperty("roles", out var roles))
                            {
                                // With this safer version:
                                foreach (var role in roles.EnumerateArray())
                                {
                                    var roleValue = role.GetString();
                                    if (!string.IsNullOrEmpty(roleValue))
                                    {
                                        claimsIdentity.AddClaim(new System.Security.Claims.Claim("role", roleValue));
                                    }
                                }
                            }
                        }
                    }
                }
                return System.Threading.Tasks.Task.CompletedTask;           
            }
        };
    });


// ==================== Authorization + AdminOnly Policy ====================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("admin")); // token’daki RoleClaimType’e göre admin rolünü kontrol eder
});

// ==================== Swagger + JWT ====================
builder.Services.AddSwaggerGen(setup =>
{
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token in textbox below!",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });
});

// ==================== Build App ====================
var app = builder.Build();

// Dev ortamý
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// ==================== Middleware ====================
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();   // JWT middleware
app.UseAuthorization();    // [Authorize] çalýþmasý için
app.UseExceptionHandler();

await ExtensionsMiddleware.CreateFirstUserAsync(app);

app.MapControllers();
app.Run();
