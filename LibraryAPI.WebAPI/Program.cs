using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using YourNamespace.WebAPI.Extensions;
using LibraryAPI.Repositories.Data;

namespace LibraryAPI
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure services
            builder.Services.ConfigureSqlContext(builder.Configuration);
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureRepositoryManager();
            builder.Services.ConfigureServiceManager();
            builder.Services.ConfigureAutoMapper();

            // Add services to the container
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(Presentation.AssemblyReference).Assembly);

            // Configure Swagger for API documentation
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryAPI", Version = "v1" });
                // Uncomment this if you want to include JWT Bearer Authentication in Swagger in the future
                /*
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
                */
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

                    // Uncomment this block if you want to ensure roles are created in the future
                    /*
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    await RoleManager.InitializeAsync(userManager, roleManager);
                    */
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

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}
