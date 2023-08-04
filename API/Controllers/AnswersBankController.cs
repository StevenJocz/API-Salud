using Microsoft.AspNetCore.Mvc;
using UNAC.AppSalud.Domain.DTOs.AnswersBankDTOs;
using UNAC.AppSalud.Persistence.Commands.AnswersBankCommands;
using UNAC.AppSalud.Persistence.Queries.AnswersBankQueries;

namespace UNAC.AppSalud.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersBankController : ControllerBase
    {
        private readonly IAnswersBankCommands _answerBankCommands;
        private readonly IAnswersBankQueries _answerBankQueries;
        private readonly ILogger<UserController> _logger;

        public AnswersBankController(IAnswersBankCommands AnswerBankCommands, IAnswersBankQueries AnswerBankQueries, ILogger<UserController> logger)
        {
            _answerBankCommands = AnswerBankCommands;
            _answerBankQueries = AnswerBankQueries;
            _logger = logger;
        }

        [HttpGet("Show_QuestionsBank")]
        public async Task<List<AnswersBankDTOs>> ShowIllnesses()
        {
            List<AnswersBankDTOs> ListMedicines;
            try
            {
                //Revisar porque no funciona con el ok
                _logger.LogInformation("Start MedicineController.SaveMedicine...");
                ListMedicines = await _answerBankQueries.ShowAnswersBankAsync();
                return ListMedicines;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start MedicineController.SaveMedicine...");
                throw;
            }
        }

        [HttpPost("Save_AnswersBank")]
        public async Task<IActionResult> SaveAnswerBank([FromBody] AnswersBankDTOs saveAnswer)
        {
            try
            {
                _logger.LogInformation("Start AnswersBankController.SaveAnswerBank...");
                var SaveIllnesses = await _answerBankCommands.SaveAnswersBankAsync(saveAnswer);
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

        [HttpPut("Change_AnswerBank")]
        public async Task<IActionResult> ChangeAnswerBank([FromBody] AnswersBankDTOs changeAnswer)
        {
            try
            {
                _logger.LogInformation("Start AnswersBankController.SaveAnswerBank...");
                var SaveIllnesses = await _answerBankCommands.ChangeAnswersBankAsync(changeAnswer);
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
        [HttpPost("Delete_AnswerBank")]
        public async Task<IActionResult> DeleteAnswerBank(int IdAnswerBank)
        {
            try
            {
                _logger.LogInformation("Start AnswersBankController.SaveAnswerBank...");
                var SaveIllnesses = await _answerBankCommands.DeleteAnswersBankAsync(IdAnswerBank);
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
    }
}
