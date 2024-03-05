using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.AnswersBankDTOs;
using UNAC.AppSalud.Domain.DTOs.CommonDTOs;
using UNAC.AppSalud.Domain.Entities.AnswersBankE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Commands.AnswersBankCommands
{
    public interface IAnswersBankCommands
    {
        Task<AnswersErrorDTOs> SaveAnswersBankAsync(AnswersBankDTOs Answers_bank);
        Task<AnswersErrorDTOs> ChangeAnswersBankAsync(AnswersBankDTOs Answers_bank);
        Task<AnswersErrorDTOs> DeleteAnswersBankAsync(int IdAnswer);
    }
    public class AnswersBankCommands : IAnswersBankCommands, IDisposable
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<AnswersBankCommands> _logger;
        private readonly IConfiguration _configuration;

        public AnswersBankCommands(ILogger<AnswersBankCommands> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        #region implementacion Disponse
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        #endregion

        private async Task<bool> SaveAnswer(AnswersBankDTOs answer_bank)
        {
            try
            {
                var SaveAnswerBank = new AnswersBankE
                {
                    s_answer_description = answer_bank.answer_description,
                    i_answer_value = Convert.ToInt32(answer_bank.answer_value),
                    fk_tblquestion_bank = Convert.ToInt32(answer_bank.fk_tblquestion_bank)
                };
                await _context.AnswersBankE.AddAsync(SaveAnswerBank);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }
        private async Task<bool> ChangeAnswer(AnswersBankDTOs answer_bank)
        {
            try
            {
                var ChangeAnswer = await _context.AnswersBankE.FirstOrDefaultAsync(x => x.id == Convert.ToInt32(answer_bank.Id));
                if (ChangeAnswer != null)
                {
                    ChangeAnswer.i_answer_value = Convert.ToInt32(answer_bank.answer_value);
                    ChangeAnswer.s_answer_description = answer_bank.answer_description;
                    ChangeAnswer.fk_tblquestion_bank = Convert.ToInt32(answer_bank.fk_tblquestion_bank);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        private async Task<bool> DeleteAnswer(int IdAnswer)
        {
            try
            {
                var DeleteAnswer = await _context.AnswersBankE.FirstOrDefaultAsync(x => x.id == IdAnswer);
                if(DeleteAnswer != null)
                {
                    _context.AnswersBankE.Remove(DeleteAnswer);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<AnswersErrorDTOs> SaveAnswersBankAsync(AnswersBankDTOs answer_bank)
        {
            _logger.LogInformation("Start QuestionsBankCommands.SaveQuestionBankAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var QuestionExist = _context.QuestionsBankE.FirstOrDefaultAsync(x => x.id.Equals(Convert.ToInt32(answer_bank.fk_tblquestion_bank)));
                if (QuestionExist != null)
                {
                    var SaveAnswersBank = await SaveAnswer(answer_bank);
                    StateInformation = SaveAnswersBank;
                    DescriptionStateInformation = SaveAnswersBank == true ? "Se guardo la información satisfactoriamente." : "No se guardo la respuesta, intentélo nuevamente.";
                }
                else {
                    StateInformation = false;
                    DescriptionStateInformation = "La pregunta que ha ingresado no existe. Crealo e intentalo más tarde.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el método QuestionsBankCommands.SaveQuestionBankAsync...");
                StateInformation = false;
                DescriptionStateInformation = "Tenemos problemas al guardar la información, intentélo nuevamente.";
            }
            AnswersErrorDTOs ReponseConsult = new AnswersErrorDTOs
            {
                stateError = StateInformation,
                descriptionError = DescriptionStateInformation
            };

            return ReponseConsult;
        }

        public async Task<AnswersErrorDTOs> ChangeAnswersBankAsync(AnswersBankDTOs answer_bank)
        {
            _logger.LogInformation("Start QuestionsBankCommands.ChangeAnswersBankAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var QuestionExist = _context.QuestionsBankE.FirstOrDefaultAsync(x => x.id.Equals(Convert.ToInt32(answer_bank.fk_tblquestion_bank)));
                if (QuestionExist != null)
                {
                    var ChangeAnswersBank = await ChangeAnswer(answer_bank);
                    StateInformation = ChangeAnswersBank;
                    DescriptionStateInformation = ChangeAnswersBank == true ? "Se guardo la información satisfactoriamente." : "No se guardo la respuesta, intentélo nuevamente.";
                }
                else
                {
                    StateInformation = false;
                    DescriptionStateInformation = "La pregunta que ha ingresado no existe. Crealo e intentalo más tarde.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.ChangeAnswersBankAsync...");
                StateInformation = false;
                DescriptionStateInformation = "Tenemos problemas al guardar la información, intentélo nuevamente.";
            }
            AnswersErrorDTOs ReponseConsult = new AnswersErrorDTOs
            {
                stateError = StateInformation,
                descriptionError = DescriptionStateInformation
            };

            return ReponseConsult;
        }

        public async Task<AnswersErrorDTOs> DeleteAnswersBankAsync(int IdAnswer)
        {
            _logger.LogInformation("Start QuestionsBankCommands.DeleteAnswersBankAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var SearchAnswer = await _context.DiagnosisFormAnswersE.Where(x => x.fk_tblanswer_bank.Equals(IdAnswer)).FirstOrDefaultAsync();
                if (SearchAnswer == null)
                {
                    var DeleteAnswerBank = await DeleteAnswer(IdAnswer);
                    StateInformation = DeleteAnswerBank;
                    DescriptionStateInformation = DeleteAnswerBank == true ? "Se guardo la información satisfactoriamente" : "No se guardo la respuesta, intentélo nuevamente.";
                }
                else
                {
                    StateInformation = false;
                    DescriptionStateInformation = "No se ha eliminado tu respuesta.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.DeleteAnswersBankAsync...");
                StateInformation = false;
                DescriptionStateInformation = "Tenemos problemas al guardar la información, intentélo nuevamente.";
            }
            AnswersErrorDTOs ReponseConsult = new AnswersErrorDTOs
            {
                stateError = StateInformation,
                descriptionError = DescriptionStateInformation
            };

            return ReponseConsult;
        }
    }
}
