using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UNAC.AppSalud.Domain.DTOs.Login;
using UNAC.AppSalud.Domain.DTOs.Login.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.LoginDTOs.LoginDTOs;
using UNAC.AppSalud.Domain.Entities;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.API.Application
{
    public interface IAutorizacionService
    {
        Task<AutorizacionResponse> DevolverToken(LoginDTOs autorizacion);
        Task<AutorizacionResponse> DevolverRefreshToken(HistorialrefreshtokenDTOs refreshtoken, int IdUsuario);
    }

    public class AutorizacionService : IAutorizacionService
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<AutorizacionService> _logger;
        private readonly IConfiguration _configuration;

        public AutorizacionService(ILogger<AutorizacionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        private string GenerarToken(string IdUsuario)
        {
            try
            {
                _logger.LogInformation("Iniciando GenerarToken");
                var key = _configuration.GetValue<string>("JwtSettings:key");
                var keyBytes = Encoding.ASCII.GetBytes(key);

                var usuario = _context.LoginEs.FirstOrDefault(x =>
                    x.IdLogin == int.Parse(IdUsuario)
                );

                var claims = new ClaimsIdentity();
                claims.AddClaim(new Claim("userId", IdUsuario));
                claims.AddClaim(new Claim("userName", usuario.user_name));
                claims.AddClaim(new Claim("userEmail", usuario.userEmail));

                var credencialesToken = new SigningCredentials
                (
                   new SymmetricSecurityKey(keyBytes),
                   SecurityAlgorithms.HmacSha256Signature
                );

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(1),
                    SigningCredentials = credencialesToken
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);

                string tokenCreado = tokenHandler.WriteToken(tokenConfig);

                return tokenCreado;
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar GenerarToken");
                throw;
            }
        }


        private async Task<AutorizacionResponse> GuardarHistorialRefreshToken(int IdUsuario, string Token)
        {
            try
            {
                _logger.LogInformation("Iniciando GuardarHistorialRefreshToken");
                var historialRefreshToken = new HistorialrefreshtokenE
                {
                    idhistorialtoken = 0,
                    idusuario = IdUsuario,
                    token = Token,
                    fechacreacion = DateTime.UtcNow,
                    fechaexpiracion = DateTime.UtcNow.AddMinutes(2)
                };

                await _context.HistorialrefreshtokenEs.AddAsync(historialRefreshToken);
                await _context.SaveChangesAsync();

                return new AutorizacionResponse
                {
                    Token = Token,
                    Resultado = true,
                    Msg = "Ok"
                };
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar GuardarHistorialRefreshToken");
                throw;
            }
        }


        public async  Task<AutorizacionResponse> DevolverToken(LoginDTOs autorizacion) 
        {
            try
            {
                _logger.LogInformation("Iniciando DevolverToken");
                var usuario_Encontrado = _context.LoginEs.FirstOrDefault(x =>
                x.userEmail == autorizacion.userEmail &&
                x.userPassword == autorizacion.userPassword
            );

                if (usuario_Encontrado == null)
                {
                    return new AutorizacionResponse()
                    {
                        Resultado = false,
                        Msg = "Correo electrónico o contraseña inválidos. Por favor, verifica la información ingresada."
                    };
                }

                string tokenCreado = GenerarToken(usuario_Encontrado.IdLogin.ToString());

                return await GuardarHistorialRefreshToken(usuario_Encontrado.IdLogin, tokenCreado);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DevolverToken");
                throw;
            }
        }

        public async Task<AutorizacionResponse> DevolverRefreshToken(HistorialrefreshtokenDTOs refreshtoken, int IdUsuario)
        {
            try
            {
                _logger.LogInformation("Iniciando DevolverRefreshToken");
                var refreshTokenEncontrado = _context.HistorialrefreshtokenEs.FirstOrDefault(x =>
                    x.token == refreshtoken.Token &&
                    x.idusuario == IdUsuario);

                if (refreshTokenEncontrado == null)
                    return new AutorizacionResponse { Resultado = false, Msg = "No existe Token" };

                var tokenCreado = GenerarToken(IdUsuario.ToString());
                return await GuardarHistorialRefreshToken(IdUsuario, tokenCreado);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar DevolverRefreshToken");
                throw;
            }
        }
    }
}
