using Microsoft.AspNetCore.Mvc;
using UNAC.AppSalud.Domain.DTOs.MedicineDTOs;
using UNAC.AppSalud.Persistence.Commands.MedicineCommands;
using UNAC.AppSalud.Persistence.Queries.MedicineQueries;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicineController : ControllerBase
    {
        private readonly IMedicineCommands _medicineCommands;
        private readonly IMedicineQueries _medicineQueries;
        private readonly ILogger<UserController> _logger;

        public MedicineController(IMedicineCommands medicineCommands, IMedicineQueries medicineQueries, ILogger<UserController> logger)
        {
            _medicineCommands = medicineCommands;
            _medicineQueries = medicineQueries;
            _logger = logger;
        }

        [HttpGet("Show_Medicines")]
        public async Task<List<MedicineListDTOs>> ShowMedicines()
        {
            List<MedicineListDTOs> ListMedicines;
            try
            {
                //Revisar porque no funciona con el ok
                _logger.LogInformation("Start MedicineController.SaveMedicine...");
                ListMedicines = await _medicineQueries.ShowMedicinesAsync();
                return ListMedicines;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start MedicineController.SaveMedicine...");
                throw;
            }
        }

        [HttpPost("Save_Medicine")]
        public async Task<IActionResult> SaveMedicine([FromBody] MedicineDTOs medicine)
        {
            try
            {
                _logger.LogInformation("Start MedicineController.SaveMedicine...");
                var SaveMedicine = await _medicineCommands.SaveNewMedicineAsync(medicine);
                if (!SaveMedicine.stateError)
                {
                    return BadRequest(new
                    {
                        resultado = SaveMedicine.stateError,
                        message = SaveMedicine.descriptionError,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = SaveMedicine.stateError,
                        message = SaveMedicine.descriptionError,
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start MedicineController.SaveMedicine...");
                throw;
            }
        }

        [HttpPut("chage_Medicine")]
        public async Task<IActionResult> ChangeMedicine([FromBody] MedicineDTOs medicine)
        {
            try
            {
                _logger.LogInformation("Start MedicineController.ChangeMedicine...");
                var ChangeMedicine = await _medicineCommands.ChangeXMedicineAsync(medicine);
                if (!ChangeMedicine.stateError)
                {
                    return BadRequest(new
                    {
                        resultado = ChangeMedicine.stateError,
                        message = ChangeMedicine.descriptionError,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = ChangeMedicine.stateError,
                        message = ChangeMedicine.descriptionError,
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start MedicineController.ChangeMedicine...");
                throw;
            }
        }

        [HttpDelete("Delete_Medicine")]
        public async Task<IActionResult> DeleteMedicine(int IdMedicine)
        {
            try
            {
                _logger.LogInformation("Start MedicineController.DeleteMedicine...");
                var DeleteMedicine = await _medicineCommands.DeleteMedicineAsync(IdMedicine);
                if (!DeleteMedicine.stateError)
                {
                    return BadRequest(new
                    {
                        resultado = DeleteMedicine.stateError,
                        message = DeleteMedicine.descriptionError,
                    });
                }
                else
                {
                    return Ok(new
                    {
                        resultado = DeleteMedicine.stateError,
                        message = DeleteMedicine.descriptionError,
                    }
                    );
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start MedicineController.DeleteMedicine...");
                throw;
            }
        }

    }
}
