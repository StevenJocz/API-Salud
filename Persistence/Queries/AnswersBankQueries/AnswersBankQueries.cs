using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.AnswersBankDTOs;
using UNAC.AppSalud.Domain.Entities.AnswersBankE;
using UNAC.AppSalud.Domain.Entities.QuestionsBankE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Queries.AnswersBankQueries
{
    public interface IAnswersBankQueries
    {
        Task<List<AnswersBankDTOs>> ShowAnswersBankAsync();
    }
    public class AnswersBankQueries : IAnswersBankQueries
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<AnswersBankQueries> _logger;
        private readonly IConfiguration _configuration;

        public AnswersBankQueries(ILogger<AnswersBankQueries> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        public async Task<List<AnswersBankDTOs>> ShowAnswersBankAsync()
        {
            _logger.LogInformation("Start AnswersBankQueries.ShowAnswersBank");
            List<AnswersBankDTOs> ShowAnswersBank;
            try
            {
                ShowAnswersBank = await _context.AnswersBankE.Join(
                    _context.QuestionsBankE,
                    Answer => Answer.fk_tblquestion_bank,
                    Question => Question.id,
                    (Answer, Question) => new { AnswersBankE = Answer, QuestionsBankE = Question }
                ).Select(Result => new AnswersBankDTOs
                {
                    Id = Convert.ToString(Result.AnswersBankE.id),
                    answer_description = Result.AnswersBankE.s_answer_description,
                    answer_value = Convert.ToString(Result.AnswersBankE.i_answer_value),
                    fk_tblquestion_bank = Convert.ToString(Result.AnswersBankE.fk_tblquestion_bank),
                    fk_tblqustion_bank_description = Result.QuestionsBankE.s_question_description
                }).ToListAsync();
                return ShowAnswersBank;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error en el metodo AnswersBankQueries.ShowAnswersBank...");
                throw ex;
            }
        }
    }
}
