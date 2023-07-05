using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UNAC.AppSalud.API.Application;
using UNAC.AppSalud.Domain.DTOs.EmailDTOs;
using UNAC.AppSalud.Domain.DTOs.Login.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.LoginDTOs.LoginDTOs;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Infrastructure.EmailServices;
using UNAC.AppSalud.Persistence.Commands.LoginCommands;
using UNAC.AppSalud.Persistence.Queries.LoginQueries;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;
        private readonly ILoginCommands _loginCommands;
        private readonly ILoginQueries _loginQueries;
        private readonly IEmailServices _emailServices;
        private readonly ILogger<LoginController> _logger;

        public LoginController(IAutorizacionService autorizacionService, ILoginCommands loginCommands, ILoginQueries loginQueries, IEmailServices emailServices, ILogger<LoginController> logger)
        {
            _autorizacionService = autorizacionService;
            _loginCommands = loginCommands;
            _loginQueries = loginQueries;
            _emailServices = emailServices;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Autenticar([FromBody] LoginDTOs autorizacion)
        {
            try
            {
                _logger.LogInformation("Iniciando Autenticar.Controller");
                var resultado_autorizacion = await _autorizacionService.DevolverToken(autorizacion);
                if (resultado_autorizacion == null)
                    return Unauthorized();
                return Ok(resultado_autorizacion);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar Autenticar.Controller");
                throw;
            }
        }

        [HttpPost("ObtenerToken")]
        public async Task<IActionResult> ObtenerToken([FromBody] HistorialrefreshtokenDTOs refreshtoken)
        {
            try
            {
                _logger.LogInformation("Iniciando ObtenerToken.Controller");
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(refreshtoken.Token);

                if (tokenExpiradoSupuestamente.ValidTo > DateTime.UtcNow)
                    return BadRequest(new AutorizacionResponse { Resultado = false, Msg = "Token no ha expirado" });

                string idUsuario = tokenExpiradoSupuestamente.Payload["userId"].ToString();

                var autorizacionResponse = await _autorizacionService.DevolverRefreshToken(refreshtoken, int.Parse(idUsuario));

                if (autorizacionResponse.Resultado)
                    return Ok(autorizacionResponse);
                else return BadRequest(autorizacionResponse);
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar ObtenerToken.Controller");
                throw;
            }
        }

        [HttpPost("EmailRestablecimientoPassword")]
        public async Task<IActionResult> EmailRestablecimientoPassword(EmailDTOs request)
        {
            try
            {
                var respuesta = await _emailServices.EmailRestablecimientoPassword(request);
                if (respuesta.codigo != null)
                {
                    var nuevoCodigo = new CodigoRestablecimientoE
                    {
                        s_correo = request.Para,
                        s_codigo = respuesta.codigo
                    };

                    bool registroRealizado = await _loginCommands.InsertarCodigo(nuevoCodigo);
                    if (registroRealizado)
                    {
                        return Ok(new { respuesta.message, respuesta.resultado });
                    }
                    else
                    {
                        return Ok(new
                        {
                            resultado = false,
                            message = "Tenemos problemas al enviar el correo electrónico. Por favor intentalo más tarde.",
                        });
                    }
                }else
                {
                    return Ok(new
                    {
                        resultado = false,
                        message = respuesta.message,
                    });
                }
                
               
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar EmailRestablecimientoPassword.Controller");
                throw;
            }
        }


        [HttpPost("VerificarCodigo")]
        public async Task<IActionResult> VerificarCodigo(CodigoRestablecimientoDTOs request)
        {
            try
            {
                var nuevoCodigo = new CodigoRestablecimientoE
                {
                    s_correo = request.s_correo,
                    s_codigo = request.s_codigo
                };

                bool codigoCorrecto = await _loginQueries.ConsultarCodigo(nuevoCodigo);
                if (codigoCorrecto)
                {
                    bool eliminarCodigo = await _loginCommands.EliminarCodigo(request.s_correo);
                    return Ok(new
                    {
                        resultado = true,
                        message = "Código correcto, puede continuar",
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = false,
                        message = "Código incorrecto. Por favor verifica el código y vuelve a intentarlo.",
                    });
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar EmailRestablecimientoPassword.Controller");
                throw;
            }
        }

        [HttpPost("ActualizarPassword")]
        public async Task<IActionResult> ActualizarPassword(string userEmail, string nuevoPassword)
        {
            try
            {
                bool Correcto = await _loginCommands.ActualizarPassword(userEmail, nuevoPassword);
                if (Correcto)
                {
                    return Ok(new
                    {
                        resultado = true,
                        message = "¡Contraseña cambiada exitosamente! Inicia sesión nuevamente con tu nueva contraseña.",
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = false,
                        message = "No se pudo cambiar la contraseña. Por favor, inténtalo nuevamente.",
                    });
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar EmailRestablecimientoPassword.Controller");
                throw;
            }
        }


    }
}
