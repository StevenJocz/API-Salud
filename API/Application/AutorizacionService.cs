using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using UNAC.AppSalud.Domain.DTOs;
using UNAC.AppSalud.Domain.Entities;
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
            _logger.LogTrace("Iniciando clase EmpleadoQueries...");
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }


        private string GenerarToken(string IdUsuario)
        {
            var key = _configuration.GetValue<string>("JwtSettings:key");
            var keyBytes = Encoding.ASCII.GetBytes(key);


            var usuario = _context.LoginEs.FirstOrDefault(x =>
                x.IdLogin == int.Parse(IdUsuario)   
            );

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("userId", IdUsuario));
            claims.AddClaim(new Claim("userName", "Hamilton Espinal"));
            claims.AddClaim(new Claim("userEmail", usuario.userEmail));
            claims.AddClaim(new Claim("userPhone", "3043461586"));


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


        private string GenerarResfreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var mg = RandomNumberGenerator.Create())
            {
                mg.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

        private async Task<AutorizacionResponse> GuardarHistorialRefreshToken(int IdUsuario, string Token, string refreshToken)
        {
            var historialRefreshToken = new HistorialrefreshtokenE
            {
                idhistorialtoken = 0,
                idusuario = IdUsuario,
                token = Token,
                refreshtoken = refreshToken,
                fechacreacion = DateTime.UtcNow,
                fechaexpiracion = DateTime.UtcNow.AddMinutes(2)

            };

            await _context.HistorialrefreshtokenEs.AddAsync(historialRefreshToken);
            await _context.SaveChangesAsync();

            return new AutorizacionResponse { 
                Token = Token,
                RefreshToken = refreshToken,
                Resultado = true,
                Msg = "Ok"
            };
        }


        public async  Task<AutorizacionResponse> DevolverToken(LoginDTOs autorizacion) 
        { 
            var usuario_Encontrado = _context.LoginEs.FirstOrDefault(x => 
                x.userEmail == autorizacion.userEmail && 
                x.userPassword == autorizacion.userPassword
            );

            if (usuario_Encontrado ==  null )
            {
                return  new AutorizacionResponse()
                {
                    Resultado = false,
                    Msg = "Correo electrónico o contraseña inválidos. Por favor, verifica la información ingresada."
                };
            }

            string tokenCreado = GenerarToken(usuario_Encontrado.IdLogin.ToString());

            string refreshTokenCreado = GenerarResfreshToken();

            return await GuardarHistorialRefreshToken(usuario_Encontrado.IdLogin, tokenCreado, refreshTokenCreado);

        }

        public async Task<AutorizacionResponse> DevolverRefreshToken(HistorialrefreshtokenDTOs refreshtoken, int IdUsuario)
        {
            var refreshTokenEncontrado = _context.HistorialrefreshtokenEs.FirstOrDefault(x => 
            x.token == refreshtoken.Token &&  
            x.refreshtoken == refreshtoken.RefreshToken &&
            x.idusuario == IdUsuario );

            if (refreshTokenEncontrado == null)
                return new AutorizacionResponse { Resultado = false, Msg = "No existe refreshToken" };

            var refreshTokenCreado = GenerarResfreshToken();
            var tokenCreado = GenerarToken(IdUsuario.ToString());

            return await GuardarHistorialRefreshToken(IdUsuario, tokenCreado, refreshTokenCreado);
        }
    }

    
}
