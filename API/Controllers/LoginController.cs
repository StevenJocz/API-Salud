using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UNAC.AppSalud.API.Application;
using UNAC.AppSalud.Domain.DTOs;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAutorizacionService _autorizacionService;

        public LoginController(IAutorizacionService autorizacionService)
        {
            _autorizacionService = autorizacionService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Autenticar([FromBody] LoginDTOs autorizacion)
        {
            var resultado_autorizacion = await _autorizacionService.DevolverToken(autorizacion);
            if (resultado_autorizacion == null)
                return Unauthorized();
            return Ok(resultado_autorizacion);
        }



        [HttpPost("ObtenerToken")]
        public async Task<IActionResult> ObtenerToken([FromBody] HistorialrefreshtokenDTOs refreshtoken)
        {
           var tokenHandler = new JwtSecurityTokenHandler();
           var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(refreshtoken.Token);

            if (tokenExpiradoSupuestamente.ValidTo > DateTime.UtcNow)
                return BadRequest(new AutorizacionResponse { Resultado = false, Msg = "Token no ha expirado" });

            string idUsuario = tokenExpiradoSupuestamente.Claims.First(x => 
                x.Type == JwtRegisteredClaimNames.NameId).Value.ToString();

            var autorizacionResponse = await _autorizacionService.DevolverRefreshToken(refreshtoken, int.Parse(idUsuario));

            if (autorizacionResponse.Resultado)
                return Ok(autorizacionResponse);
            else return BadRequest(autorizacionResponse);


        }
    }
}
