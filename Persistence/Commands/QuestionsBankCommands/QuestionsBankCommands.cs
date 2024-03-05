using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.CommonDTOs;
using UNAC.AppSalud.Domain.DTOs.QuestionBankDTOs;
using UNAC.AppSalud.Domain.Entities.QuestionsBankE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Commands.QuestionsBankCommands
{
    public interface IQuestionBankCommands
    {
        Task<AnswersErrorDTOs> SaveQuestionBankAsync(QuestionsBankDTOs question_bank);
        Task<AnswersErrorDTOs> ChangeQuestionBankAsync(QuestionsBankDTOs question_bank);
        Task<AnswersErrorDTOs> DeleteQuestionBankAsync(int IdQuestion);
    }
    public class QuestionsBankCommands : IQuestionBankCommands, IDisposable
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<QuestionsBankCommands> _logger;
        private readonly IConfiguration _configuration;

        public QuestionsBankCommands(ILogger<QuestionsBankCommands> logger, IConfiguration configuration)
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

        private async Task<bool> SaveQuestion(QuestionsBankDTOs question_bank)
        {
            try
            {
                var SaveQuestion = new QuestionsBankE
                {
                    s_question_description = question_bank.question_description
                };
                await _context.QuestionsBankE.AddAsync(SaveQuestion);
                await _context.SaveChangesAsync();
                return true;
            }
            catch(Exception ex) 
            {
                return false;
            }
        }

        private async Task<bool> ChangeQuestion(QuestionsBankDTOs question_bank)
        {
            try
            {
                var ChangeQuestion = await _context.QuestionsBankE.FirstOrDefaultAsync(x => x.id == Convert.ToInt32(question_bank.Id));
                if (ChangeQuestion != null)
                {
                    ChangeQuestion.s_question_description = question_bank.question_description;
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> DeleteQuestion(int IdQuestion)
        {
            try
            {
                var DeleteQuestion = await _context.QuestionsBankE.FirstOrDefaultAsync(x => x.id == IdQuestion);
                if(DeleteQuestion != null)
                {
                    _context.QuestionsBankE.Remove(DeleteQuestion);
                    await _context.SaveChangesAsync();
                }
                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<AnswersErrorDTOs> SaveQuestionBankAsync(QuestionsBankDTOs question_bank)
        {
            _logger.LogInformation("Start QuestionsBankCommands.SaveQuestionBankAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var SaveQuestionBank = await SaveQuestion(question_bank);
                StateInformation = SaveQuestionBank;
                DescriptionStateInformation = SaveQuestionBank == true ? "Se guardo la información satisfactoriamente" : "No se guardo la medicina, intentélo nuevamentee";
            }
            catch(Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.SaveMedicine...");
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
        public async Task<AnswersErrorDTOs> ChangeQuestionBankAsync(QuestionsBankDTOs question_bank)
        {
            _logger.LogInformation("Start QuestionsBankCommands.SaveQuestionBankAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var ChangeQuestionBank = await ChangeQuestion(question_bank);
                StateInformation = ChangeQuestionBank;
                DescriptionStateInformation = ChangeQuestionBank == true ? "Se guardo la información satisfactoriamente" : "No se guardo los cambios, intentélo nuevamentee";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.SaveMedicine...");
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
        public async Task<AnswersErrorDTOs> DeleteQuestionBankAsync(int IdQuestion)
        {
            _logger.LogInformation("Start QuestionsBankCommands.SaveQuestionBankAsync");
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var SearchAnswersUse = await _context.AnswersBankE.Where(x => x.fk_tblquestion_bank.Equals(IdQuestion)).FirstOrDefaultAsync();
                var SearchAnswersUser = await _context.DiagnosisFormAnswersE.Where(x => x.fk_tblquestion_bank.Equals(IdQuestion)).FirstOrDefaultAsync();
                if (SearchAnswersUse == null && SearchAnswersUser == null)
                {
                    var DeleteAnswerBank = await DeleteQuestion(IdQuestion);
                    StateInformation = DeleteAnswerBank;
                    DescriptionStateInformation = DeleteAnswerBank == true ? "Se guardo la información satisfactoriamente" : "No se elimino la medicina, intentélo nuevamentee";
                }
                else
                {
                    StateInformation = false;
                    DescriptionStateInformation = "La respuesta ya ha sido usada por una respuesta o usuario.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error en el metodo MedicineCommands.SaveMedicine...");
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
