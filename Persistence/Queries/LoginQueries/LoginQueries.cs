using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Infrastructure;
using UNAC.AppSalud.Persistence.Commands.LoginCommands;

namespace UNAC.AppSalud.Persistence.Queries.LoginQueries
{
    public interface ILoginQueries
    {
        Task<bool> ConsultarCodigo(CodigoRestablecimientoE nuevoCodigo);
        Task<LoginE> ConsultarUsuarioXId(int idUsuario);
        Task<LoginE> ConsultarUsuarioXCorreo(string userEmail, string userPassword);
    }

    public class LoginQueries : ILoginQueries, IDisposable
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<LoginQueries> _logger;
        private readonly IConfiguration _configuration;

        public LoginQueries(ILogger<LoginQueries> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
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

        public async Task<LoginE> ConsultarUsuarioXId(int idUsuario)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuario...");
            try
            {
                var usuario = await _context.LoginEs.AsNoTracking().FirstOrDefaultAsync(x => x.IdLogin == idUsuario);

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<LoginE> ConsultarUsuarioXCorreo(string userEmail, string userPassword)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuarioAutorizado...");
            try
            {
                var usuario = await _context.LoginEs.AsNoTracking().FirstOrDefaultAsync(x =>
                    x.s_userEmail == userEmail &&
                    x.s_userPassword == userPassword
                );

                return usuario;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<bool> ConsultarCodigo(CodigoRestablecimientoE nuevoCodigo)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarCodigo...");
            try
            {
               var correocorrecto = await _context.CodigoRestablecimientoEs.AsNoTracking().FirstOrDefaultAsync(e => e.s_codigo == nuevoCodigo.s_codigo && e.s_correo == nuevoCodigo.s_correo);

                return (correocorrecto != null) ? true : false;

            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
