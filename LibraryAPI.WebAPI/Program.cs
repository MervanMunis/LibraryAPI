using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using YourNamespace.WebAPI.Extensions;
using LibraryAPI.Repositories.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Identity;
using LibraryAPI.Entities.Models;
using LibraryAPI.Presentation.Auth.Controllers;
using LibraryAPI.Presentation.Middleware;

namespace LibraryAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure services
            builder.Services.ConfigureSqlContext(builder.Configuration); // Configure the DbContext
            builder.Services.ConfigureIdentity(); // Configure Identity services
            builder.Services.ConfigureRepositoryManager(); // Configure the repository manager
            builder.Services.ConfigureServiceManager(); // Configure the service manager
            builder.Services.ConfigureAutoMapper(); // Configure AutoMapper
            builder.Services.ConfigureAuthenticationManager(); // Configure the custom authentication manager

            // Configure JWT authentication
            var jwtSettings = builder.Configuration.GetSection("Authentication:Jwt");
            var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

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
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });

            // Add services to the container
            builder.Services.AddAuthorization(); // Add Authorization
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly); // Add Controllers and Presentation Layer

            // Configure Swagger for API documentation
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryAPI", Version = "v1" });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Build the app
            var app = builder.Build();

            // Automatically apply migrations at startup
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var dbContext = services.GetRequiredService<RepositoryContext>(); // Get the DbContext
                    await dbContext.Database.MigrateAsync(); // Apply migrations automatically

                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    await RoleInitializer.InitializeAsync(userManager, roleManager); // Initialize roles and admin user
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>(); // Get the logger
                    logger.LogError(ex, "An error occurred while migrating the database."); // Log the error
                }
            }

            // Configure the HTTP request pipeline
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection(); // Redirect HTTP requests to HTTPS

            app.UseAuthentication(); // Use authentication middleware
            app.UseAuthorization(); // Use authorization middleware

            app.UseMiddleware<ValidationMiddleware>(); // Use custom validation middleware
            app.UseMiddleware<CustomResponseMiddleware>(); // Use custom response formatting middleware
            app.UseMiddleware<ExceptionMiddleware>(); // Use custom exception handling middleware
            app.UseMiddleware<ExceptionLoggingMiddleware>(); // Use custom exception logging middleware

            app.MapControllers(); // Map controller routes

            await app.RunAsync(); // Run the application
        }
    }
}
