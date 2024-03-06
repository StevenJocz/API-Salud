using Microsoft.AspNetCore.Mvc;
using UNAC.AppSalud.Domain.DTOs.IlnessesDTOs;
using UNAC.AppSalud.Persistence.Commands.IllnessesCommands;
using UNAC.AppSalud.Persistence.Queries.IllnessesQueries;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IllnessesController : ControllerBase
    {
        private readonly IIllnessesComands _illnessesCommands;
        private readonly IIllnessesQueries _illnessesQueries;
        private readonly ILogger<UserController> _logger;

        public IllnessesController(IIllnessesComands IllnessesCommands, IIllnessesQueries IllnessesQueries, ILogger<UserController> logger)
        {
            _illnessesCommands = IllnessesCommands;
            _illnessesQueries = IllnessesQueries;
            _logger = logger;
        }

        [HttpGet("Show_Illnesses")]
        public async Task<List<IllnessesDTOs>> ShowIllnesses()
        {
            List<IllnessesDTOs> ListMedicines;
            try
            {
                //Revisar porque no funciona con el ok
                _logger.LogInformation("Start MedicineController.SaveMedicine...");
                ListMedicines = await _illnessesQueries.ShowIllnessesAsync();
                return ListMedicines;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start MedicineController.SaveMedicine...");
                throw;
            }
        }

        [HttpPost("Save_Illnes")]
        public async Task<IActionResult> SaveIllnes([FromBody] IllnessesDTOs illness)
        {
            try
            {
                _logger.LogInformation("Start IllnessesController.SaveIllnesses...");
                var SaveIllnesses = await _illnessesCommands.SaveIllnessesAsync(illness);
                if (!SaveIllnesses.stateError)
                {
                    return BadRequest(new
                    {
                        resultado = SaveIllnesses.stateError,
                        message = SaveIllnesses.descriptionError,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = SaveIllnesses.stateError,
                        message = SaveIllnesses.descriptionError,
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start IllnessesController.SaveIllnesses...");
                throw;
            }
        }

        [HttpPut("Chage_Illnes")]
        public async Task<IActionResult> ChangeIllnes([FromBody] IllnessesDTOs illness)
        {
            try
            {
                _logger.LogInformation("Start IllnessesController.ChangeIllnes...");
                var SaveIllnesses = await _illnessesCommands.ChangeIllnessesAsync(illness);
                if (!SaveIllnesses.stateError)
                {
                    return BadRequest(new
                    {
                        resultado = SaveIllnesses.stateError,
                        message = SaveIllnesses.descriptionError,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = SaveIllnesses.stateError,
                        message = SaveIllnesses.descriptionError,
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start IllnessesController.ChangeIllnes...");
                throw;
            }
        }

        [HttpDelete("Delete_Illnes")]
        public async Task<IActionResult> DeleteIllnes(int IdIllnes) 
        {
            try
            {
                _logger.LogInformation("Start IllnessesController.DeleteIllnes...");
                var SaveIllnesses = await _illnessesCommands.DeleteIllnessesAsync(IdIllnes);
                if (!SaveIllnesses.stateError)
                {
                    return BadRequest(new
                    {
                        resultado = SaveIllnesses.stateError,
                        message = SaveIllnesses.descriptionError,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = SaveIllnesses.stateError,
                        message = SaveIllnesses.descriptionError,
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start IllnessesController.DeleteIllnes...");
                throw;
            }
        }
    }
}
