using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using UNAC.AppSalud.API.Application;
using UNAC.AppSalud.Domain.DTOs.Login.LoginDTOs;
using UNAC.AppSalud.Domain.DTOs.UserDTOs;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Infrastructure.EmailServices;
using UNAC.AppSalud.Persistence.Commands.LoginCommands;
using UNAC.AppSalud.Persistence.Commands.UserCommands;
using UNAC.AppSalud.Persistence.Queries.LoginQueries;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserCommands _userCommands;
        private readonly IEmailServices _emailServices;
        private readonly ILogger<UserController> _logger;

        public UserController( IUserCommands userCommands, IEmailServices emailServices, ILogger<UserController> logger)
        {
            _userCommands = userCommands;
            _emailServices = emailServices;
            _logger = logger;
        }



        [HttpPost("Create_User")]
        public async Task<IActionResult> Create_User([FromBody] UserDTOs user)
        {
            try
            {
                _logger.LogInformation("Iniciando UserController.Create_User...");
                int respuesta = await _userCommands.InsertarUser(user);
                if (respuesta == 0) {

                    return BadRequest(new
                    {
                        resultado = false,
                        message = "No se logro crear el usuario correctamente. Por favor, inténtalo nuevamente.",
                    });
                   
                }
                else if (respuesta == -1) 
                {
                    return BadRequest(new
                    {
                        resultado = false,
                        message = "Ya existe un usuario registrado con el correo " + user.s_user_email + ". Por favor, intenta registrarte con otra dirección de correo electrónico.",
                    });

                }
                else
                {
                    var CorreoEnviado = await _emailServices.EmailCreateUser(user.s_user_email);

                    return Ok(new
                    {
                        resultado = true,
                        message = "¡Has completado el registro satisfactoriamente! Para continuar, inicia sesión.",
                    }
                    );
                }

            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar UserController.Create_User...");
                throw;
            }
        }
    }
}
