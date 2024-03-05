using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.QuestionBankDTOs;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Queries.QuestionsBankQueries
{
    public interface IQuestionBankQueries
    {
        Task<List<QuestionsBankDTOs>> ShowQuestionsBankAsync();
    }
    public class QuestionsBankQueries : IQuestionBankQueries, IDisposable
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<QuestionsBankQueries> _logger;
        private readonly IConfiguration _configuration;

        public QuestionsBankQueries(ILogger<QuestionsBankQueries> logger, IConfiguration configuration)
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

        public async Task<List<QuestionsBankDTOs>> ShowQuestionsBankAsync()
        {
            _logger.LogInformation("Start QuestionsBankQueries.ShowQuestionsBank");
            List<QuestionsBankDTOs> FindAllQuestionBank;
            try
            {
                FindAllQuestionBank = await _context.QuestionsBankE.Select(x => new QuestionsBankDTOs
                {
                    Id = Convert.ToString(x.id),
                    question_description = x.s_question_description
                }).ToListAsync();

                return FindAllQuestionBank;
            }
            catch(Exception ex)
            {
                _logger.LogError("Error en el metodo QuestionsBankQueries.ShowQuestionsBank...");
                throw ex;
            }
        }

    }
}
