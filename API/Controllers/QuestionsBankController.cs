using Microsoft.AspNetCore.Mvc;
using UNAC.AppSalud.Domain.DTOs.QuestionBankDTOs;
using UNAC.AppSalud.Persistence.Commands.QuestionsBankCommands;
using UNAC.AppSalud.Persistence.Queries.QuestionsBankQueries;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsBankController : ControllerBase
    {
        private readonly IQuestionBankCommands _questionBankCommands;
        private readonly IQuestionBankQueries _questionBankQueries;
        private readonly ILogger<UserController> _logger;

        public QuestionsBankController(IQuestionBankCommands QuestionBankCommands, IQuestionBankQueries QuestionBankQueries, ILogger<UserController> logger)
        {
            _questionBankCommands = QuestionBankCommands;
            _questionBankQueries = QuestionBankQueries;
            _logger = logger;
        }


        [HttpGet("Show_QuestionsBank")]
        public async Task<List<QuestionsBankDTOs>> ShowIllnesses()
        {
            List<QuestionsBankDTOs> ListMedicines;
            try
            {
                //Revisar porque no funciona con el ok
                _logger.LogInformation("Start MedicineController.SaveMedicine...");
                ListMedicines = await _questionBankQueries.ShowQuestionsBankAsync();
                return ListMedicines;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start MedicineController.SaveMedicine...");
                throw;
            }
        }

        [HttpPost("Save_QuestionBank")]
        public async Task<IActionResult> SaveQuestionBank([FromBody] QuestionsBankDTOs saveQuestion)
        {
            try {
                _logger.LogInformation("Start QuestionsBankController.SaveQuestionBank...");
                var SaveIllnesses = await _questionBankCommands.SaveQuestionBankAsync(saveQuestion);
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
                _logger.LogError("Error to start QuestionsBankController.SaveQuestionBank...");
                throw;
            }
        }

        [HttpPut("Change_QuestionBank")]
        public async Task<IActionResult> ChangeQuestionBank([FromBody] QuestionsBankDTOs changeQuestion)
        {
            try
            {
                _logger.LogInformation("Start QuestionsBankController.ChangeQuestionBank...");
                var SaveIllnesses = await _questionBankCommands.ChangeQuestionBankAsync(changeQuestion);
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
                _logger.LogError("Error to start QuestionsBankController.ChangeQuestionBank...");
                throw;
            }
        }

        [HttpDelete("Delete_QuestionBank")]
        public async Task<IActionResult> DeleteQuestionBank(int IdQuestionBank)
        {
            try
            {
                _logger.LogInformation("Start QuestionsBankController.DeleteQuestionBank...");
                var SaveIllnesses = await _questionBankCommands.DeleteQuestionBankAsync(IdQuestionBank);
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
                _logger.LogError("Error to start QuestionsBankController.DeleteQuestionBank...");
                throw;
            }
        }
    }
}
