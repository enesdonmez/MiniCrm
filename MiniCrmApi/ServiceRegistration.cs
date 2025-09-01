using Carter;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MiniCrmApi.Auth;
using MiniCrmApi.Data;
using MiniCrmApi.Entities;
using MiniCrmApi.Mapping;
using MiniCrmApi.Middlewares;
using MiniCrmApi.Services.Abstract;
using MiniCrmApi.Services.Concrete;
using Serilog;
using System.Text;
using System.Threading.RateLimiting;

namespace MiniCrmApi;

public static class ServiceRegistration
{
    public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddTransient<ReqAndResActivityBodyMiddleware>();
        var connectionString = configuration.GetConnectionString("MiniCrmDb");
        services.AddDbContext<CrmDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
                npgsqlOptions.MigrationsAssembly("MiniCrmApi")));
        services.AddCarter();
        services.AddMapster();
        var config = TypeAdapterConfig.GlobalSettings;
        MappingCongfig.Configure();
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        services.AddValidatorsFromAssemblyContaining<Program>();
        services.AddTransient<JwtGenerator>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();
        services.AddScoped<ICustomerNoteRepository, CustomerNoteRepository>();
        services.AddScoped<IDealRepository, DealRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
        services.AddScoped<IEmailService, SmtpEmailService>();
        services.AddResponseCompression();

        var rateLimitConfig = configuration.GetSection("RateLimiting:Fixed");
        services.AddRateLimiter(options =>
        {
            options.AddFixedWindowLimiter("fixed", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimitConfig.GetValue<int>("PermitLimit", 50);
                limiterOptions.QueueLimit = rateLimitConfig.GetValue<int>("QueueLimit", 5);
                limiterOptions.Window = TimeSpan.FromSeconds(rateLimitConfig.GetValue<int>("WindowSeconds", 60));
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
        });
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
            {
                policy.RequireClaim("role", "admin");
            });
        });

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "Bearer";
            options.DefaultChallengeScheme = "Bearer";
        })
           .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
                };
            });

        services.AddSerilog();
    }
}
