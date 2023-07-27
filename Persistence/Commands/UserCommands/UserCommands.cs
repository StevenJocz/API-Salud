using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.DTOs.EmailDTOs;
using UNAC.AppSalud.Domain.DTOs.Login.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.UserDTOs;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Domain.Entities.UserE;
using UNAC.AppSalud.Infrastructure;
using UNAC.AppSalud.Persistence.Commands.LoginCommands;

namespace UNAC.AppSalud.Persistence.Commands.UserCommands
{
    public interface IUserCommands
    {
        Task<int> InsertarUser(UserDTOs userDTOs);
    }

    public class UserCommands : IUserCommands, IDisposable
    {

        private readonly SaludDbContext _context = null;
        private readonly ILogger<UserCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginCommands _loginCommands;

        public UserCommands(ILogger<UserCommands> logger, IConfiguration configuration, ILoginCommands loginCommands)
        {
            _logger = logger;
            _configuration = configuration;
            _loginCommands = loginCommands;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }


        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        public async Task<int> InsertarUser(UserDTOs userDTOs)
        {
            _logger.LogTrace("Iniciando metodo UserCommands.InsertarUser...");

            try
            {
                var Email = _context.UserEs.FirstOrDefault(x => x.s_user_email == userDTOs.s_user_email);

                if (Email == null)
                {
                    var userE = UserDTOs.CreateE(userDTOs);
                    await _context.UserEs.AddAsync(userE);
                    await _context.SaveChangesAsync();

                    if (userE.id != 0)
                    {
                        var nuevoLogin = new LoginDTOs
                        {
                            userEmail = userDTOs.s_user_email,
                            userPassword = userDTOs.Password,
                            fk_tblusers = userE.id,
                        };

                        await _loginCommands.InsertarLogin(nuevoLogin);

                        return userE.id;
                    }
                    else
                    {
                        return 0;
                    }

                }
                else
                {
                    return -1;
                }
               
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo UserCommands.InsertarUser...");
                throw;
            }
        }
    }
}
