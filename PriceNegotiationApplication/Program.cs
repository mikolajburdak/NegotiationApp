using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PriceNegotiationApp.Data;
using PriceNegotiationApp.Models;
using PriceNegotiationApp.Repositories.Impl;
using PriceNegotiationApp.Repositories.Interfaces;
using PriceNegotiationApp.Services.Impl;
using PriceNegotiationApp.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// ===== Database Configuration =====
// Configure SQLite as the database provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// ===== Identity Configuration =====
// Set up ASP.NET Core Identity for user authentication and management
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Authorization"); 
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Authorization"); 
    });
});

builder.Services.AddControllers()    
    .AddJsonOptions(options =>
{
    // Setting ReferenceHandler.Preserve prevents reference cycle errors in object graphs
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    // Ignore null values in JSON response
    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    // Use camelCase for property names
    options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

// ===== Dependency Injection =====
// Register configuration as a service
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

builder.Services.AddScoped<IAuthService, AuthServiceImpl>();
builder.Services.AddScoped<INegotiationService, NegotiationService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<INegotiationRepository, NegotiationRepositoryImpl>();
builder.Services.AddScoped<IProductRepository, ProductRepositoryImpl>();

// ===== Authentication Configuration =====
// Configure JWT Bearer authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.IncludeErrorDetails = true;
        
        // Important for CORS - do not require authentication for OPTIONS (preflight request)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                return Task.CompletedTask;
            },
            OnTokenValidated = context => 
            {                
                // Handle claims identity and ensure NameIdentifier claim is present
                if (context.Principal?.Identity is ClaimsIdentity identity)
                {                    
                    var nameIdClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                    if (nameIdClaim == null)
                    {
                        var altClaim = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier");
                        if (altClaim != null)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, altClaim.Value));
                        }
                    }
                }
                
                return Task.CompletedTask; 
            },
            OnAuthenticationFailed = context => 
            {
                return Task.CompletedTask;
            },
            OnChallenge = context => 
            {
                return Task.CompletedTask;
            }
        };
        
        // Configure JWT token validation parameters
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? 
                throw new InvalidOperationException("JWT key is missing"))),
            ClockSkew = TimeSpan.Zero,
            NameClaimType = ClaimTypes.NameIdentifier
        };
    });

// ===== Authorization Configuration =====
builder.Services.AddAuthorization();

builder.Services.AddLogging(logging =>
{
    logging.AddConsole();
    logging.AddDebug();
});

// ===== Swagger Configuration =====
// Configure Swagger for API documentation
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ContactsApp API", Version = "v1" });
    c.EnableAnnotations();

    // Configure Swagger to support JWT authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter your JWT with Bearer in the format 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Enable Swagger in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

// ===== Middleware Pipeline Configuration =====
// Authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Configure routing for controllers
app.MapControllers();

app.Run();

public partial class Program { }