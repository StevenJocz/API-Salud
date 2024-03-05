using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.DTOs.Login.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.UserDTOs;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Infrastructure;
using UNAC.AppSalud.Persistence.Utilidades;

namespace UNAC.AppSalud.Persistence.Commands.LoginCommands
{
    public interface ILoginCommands
    {
        Task<bool> InsertarCodigo(CodigoRestablecimientoE nuevoCodigo);
        Task<bool> EliminarCodigo(string correo);
        Task<bool> ActualizarPassword(string userEmail, string nuevoPassword);
        Task<bool> InsertarLogin(LoginDTOs LoginDTOs);
    }

    public class LoginCommands : ILoginCommands, IDisposable
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<LoginCommands> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUtilidades _utilidades;

        public LoginCommands(ILogger<LoginCommands> logger, IConfiguration configuration, IUtilidades utilidades)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
            _utilidades = utilidades;
        }

        #region implementacion Disponse
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        #endregion

        public async Task<bool> InsertarCodigo(CodigoRestablecimientoE nuevoCodigo)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.InsertarCodigo...");
            try
            {
                await _context.CodigoRestablecimientoEs.AddAsync(nuevoCodigo);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo LoginCommands.InsertarCodigo...");
                throw;
            }
        }

        public async Task<bool> EliminarCodigo(string correo)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.EliminarCodigo...");
            try
            {
                var codigo = await _context.CodigoRestablecimientoEs.FirstOrDefaultAsync(c => c.s_correo == correo);
                if (codigo != null)
                {
                    _context.CodigoRestablecimientoEs.Remove(codigo);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el LoginCommands.EliminarCodigo...");
                throw;
            }
        }

        public async Task<bool> ActualizarPassword(string userEmail, string nuevoPassword)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.ActualizarCodigo...");
            try
            {
                var codigo = await _context.LoginEs.FirstOrDefaultAsync(c => c.s_userEmail == userEmail);
                if (codigo != null)
                {
                    var hashedPassword = await _utilidades.HashPassword(nuevoPassword.Trim());
                    codigo.s_userPassword = hashedPassword; 
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false; 
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error en el LoginCommands.ActualizarCodigo...");
                throw;
            }
        }


        public async Task<bool> InsertarLogin(LoginDTOs LoginDTOs)
        {
            _logger.LogTrace("Iniciando metodo LoginCommands.InsertarLogin...");
            try
            {
                var LoginE = LoginDTOs.CreateE(LoginDTOs);
                await _context.LoginEs.AddAsync(LoginE);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                _logger.LogError("Error en el metodo LoginCommands.InsertarLogin...");
                throw;
            }
        }



    }
}
