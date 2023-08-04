using Microsoft.AspNetCore.Mvc;
using UNAC.AppSalud.Domain.DTOs.MedicineFormDTOs;
using UNAC.AppSalud.Persistence.Commands.MedicineFormCommands;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineFormController : ControllerBase
    {
        private readonly IMedicineFormCommands _medicineFormCommands;
        private readonly ILogger<UserController> _logger;

        public MedicineFormController(IMedicineFormCommands medicineFormCommands, ILogger<UserController> logger)
        {
            _medicineFormCommands = medicineFormCommands;
            _logger = logger;
        }

        [HttpPost("Save_MedicineForm")]
        public async Task<IActionResult> SaveMedicineForm([FromBody] MedicineFormDTOs AnswerMedicineForm)
        {
            try
            {
                _logger.LogInformation("Start MedicineFormController.MedicineFormDTOs...");
                var SaveAnswersMedicineForm = await _medicineFormCommands.SaveAnswersMedicineFormAsync(AnswerMedicineForm);
                if (!SaveAnswersMedicineForm.stateError)
                {
                    return BadRequest(new
                    {
                        resultado = SaveAnswersMedicineForm.stateError,
                        message = SaveAnswersMedicineForm.descriptionError,
                    });
                }
                else
                {
                    return Ok(new
                        {
                            resultado = SaveAnswersMedicineForm.stateError,
                            message = SaveAnswersMedicineForm.descriptionError,
                        }
                    );
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Error to start MedicineFormController.MedicineFormDTOs...");
                throw;
            }
        }

    }
}
