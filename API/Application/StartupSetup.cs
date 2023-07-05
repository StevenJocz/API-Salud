using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UNAC.AppSalud.Infrastructure.EmailServices;
using UNAC.AppSalud.Persistence.Commands.LoginCommands;
using UNAC.AppSalud.Persistence.Queries.LoginQueries;

namespace UNAC.AppSalud.API.Application
{
    public static class StartupSetup
    {
        public static IServiceCollection AddStartupSetup(this IServiceCollection service, IConfiguration configuration)
        {
            // Autorizacion Services
            service.AddTransient<IAutorizacionService, AutorizacionService>();

            // Email Services
            service.AddTransient<IEmailServices, EmailServices>();

            // Queries Persistance Services
            service.AddTransient<ILoginQueries, LoginQueries>();

            // Commands Persistance Services
            service.AddTransient<ILoginCommands, LoginCommands>();


            // Authentication Services
            var key = configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            service.AddAuthentication(config =>
            {
                config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(config =>
            {
                config.RequireHttpsMetadata = false;
                config.SaveToken = true;
                config.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            return service;
        }
    }
}
