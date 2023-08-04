using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UNAC.AppSalud.Infrastructure.EmailServices;
using UNAC.AppSalud.Persistence.Commands.LoginCommands;
using UNAC.AppSalud.Persistence.Commands.DiagnosisFormCommands;
using UNAC.AppSalud.Persistence.Commands.UserCommands;
using UNAC.AppSalud.Persistence.Queries.LoginQueries;
using UNAC.AppSalud.Persistence.Commands.AnswersBankCommands;
using UNAC.AppSalud.Persistence.Queries.AnswersBankQueries;
using UNAC.AppSalud.Persistence.Commands.IllnessesCommands;
using UNAC.AppSalud.Persistence.Queries.IllnessesQueries;
using UNAC.AppSalud.Persistence.Commands.MedicineCommands;
using UNAC.AppSalud.Persistence.Queries.MedicineQueries;
using UNAC.AppSalud.Persistence.Commands.QuestionsBankCommands;
using UNAC.AppSalud.Persistence.Queries.QuestionsBankQueries;
using UNAC.AppSalud.Persistence.Commands.MedicineFormCommands;

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

            //Command user
            service.AddTransient<IUserCommands, UserCommands>();

            //Add service DiagnosisFor
            service.AddTransient<IDiagnosisFormCommands, DiagnosisFormCommands>();

            service.AddTransient<IAnswersBankCommands, AnswersBankCommands>();

            service.AddTransient<IAnswersBankQueries, AnswersBankQueries>();

            service.AddTransient<IIllnessesComands, IllnessessCommands>();

            service.AddTransient<IIllnessesQueries, IllnessesQueries>();

            service.AddTransient<IMedicineCommands, MedicineCommands>();

            service.AddTransient<IMedicineQueries, MedicineQueries>();

            service.AddTransient<IQuestionBankCommands, QuestionsBankCommands>();

            service.AddTransient<IQuestionBankQueries, QuestionsBankQueries>();

            service.AddTransient<IMedicineFormCommands, MedicineFormCommands>();

            service.AddTransient<IDiagnosisFormCommands, DiagnosisFormCommands>();


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
