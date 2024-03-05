using Microsoft.AspNetCore.Mvc;
using UNAC.AppSalud.Infrastructure.EmailServices;
using UNAC.AppSalud.Persistence.Commands.UserCommands;
using UNAC.AppSalud.Persistence.Queries.LocationQueries;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly ILocationQueries _locationQueries;
        private readonly ILogger<UserController> _logger;

        public LocationController(ILocationQueries locationQueries, ILogger<UserController> logger)
        {
            _locationQueries = locationQueries;
            _logger = logger;
        }

        [HttpGet("listCountries")]
        public async Task<IActionResult> listCountries()
        {
            try
            {
                _logger.LogInformation("Iniciando LocationController.listCountries");
                var lists = await _locationQueries.listCountries();
                if (lists.Count > 0)
                {
                    return Ok(lists);
                }
                else
                {
                    return BadRequest(new
                    {
                        resultado = false,
                        message = "No se pudo cargar los paises, por favor intentelo mas tarde",
                    });
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar LocationController.listCountries");
                throw;
            }
        }

        [HttpGet("listStates")]
        public async Task<IActionResult> listStates(int idCountrie)
        {
            try
            {
                _logger.LogInformation("Iniciando LocationController.listStates");
                var lists = await _locationQueries.listStates(idCountrie);
                if (lists.Count > 0)
                {
                    return Ok(lists);
                }
                else
                {
                    return BadRequest(new
                    {
                        resultado = false,
                        message = "No se pudo cargar los estados, por favor intentelo mas tarde",
                    });
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar LocationController.listStates");
                throw;
            }
        }

        [HttpGet("listCities")]
        public async Task<IActionResult> listCities(int idState)
        {
            try
            {
                _logger.LogInformation("Iniciando LocationController.listCities");
                var lists = await _locationQueries.listCities(idState);
                if (lists.Count > 0)
                {
                    return Ok(lists);
                }
                else
                {
                    return BadRequest(new
                    {
                        resultado = false,
                        message = "No se pudo cargar las ciudades, por favor intentelo mas tarde",
                    });
                }
            }
            catch (Exception)
            {
                _logger.LogError("Error al iniciar LocationController.listCities");
                throw;
            }
        }
    }
}
