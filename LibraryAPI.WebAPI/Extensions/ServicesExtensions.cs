using Microsoft.EntityFrameworkCore;
using LibraryAPI.Repositories.Manager;
using LibraryAPI.Services.Manager;
using LibraryAPI.Repositories.Data;
using LibraryAPI.Entities.Models;
using Microsoft.AspNetCore.Identity;
using LibraryAPI.Presentation.Auth.Services;

namespace YourNamespace.WebAPI.Extensions
{
    public static class ServicesExtensions
    {
        // Configures the DbContext with the specified connection string.
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration) =>
            services.AddDbContext<RepositoryContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("LibraryAPIContext"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(RepositoryContext).Assembly.FullName)));

        // Configures the identity services with custom options.
        public static void ConfigureIdentity(this IServiceCollection services) =>
            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.Password.RequireDigit = true;
                opts.Password.RequireLowercase = true;
                opts.Password.RequireUppercase = true;
                opts.Password.RequiredLength = 5;
                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<RepositoryContext>()
            .AddSignInManager<SignInManager<ApplicationUser>>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddDefaultTokenProviders();

        // Configures the repository manager to be added to the IServiceCollection scope.
        public static void ConfigureRepositoryManager(this IServiceCollection services) =>
            services.AddScoped<IRepositoryManager, RepositoryManager>();

        // Configures the service manager to be added to the IServiceCollection scope.
        public static void ConfigureServiceManager(this IServiceCollection services) =>
            services.AddScoped<IServiceManager, ServiceManager>();

        // Configures the Auth manager to be added to the IServiceCollection scope.
        public static void ConfigureAuthenticationManager(this IServiceCollection services) =>
            services.AddScoped<IAuthenticationService, AuthenticationManager>();

        // Configures AutoMapper to automatically map between DTOs and entities.
        public static void ConfigureAutoMapper(this IServiceCollection services) =>
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
