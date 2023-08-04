using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Data;
using UNAC.AppSalud.Domain.DTOs.CommonDTOs;
using UNAC.AppSalud.Domain.DTOs.DiagnosisFormDTOs;
using UNAC.AppSalud.Domain.Entities.DiagnosisFormAnswersE;
using UNAC.AppSalud.Domain.Entities.DiagnosticFormE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Commands.DiagnosisFormCommands
{

    public interface IDiagnosisFormCommands {
        Task<AnswersErrorDTOs> SaveAnswersDiagnosisFormAsync(DiagnosisFormDTOs Diagnosis_form);
    }

    public class DiagnosisFormCommands : IDiagnosisFormCommands
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<DiagnosisFormCommands> _logger;
        private readonly IConfiguration _configuration;

        public DiagnosisFormCommands(ILogger<DiagnosisFormCommands> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        private async Task<int> SaveAnswerDiagnosisFormXUser(int UserId, DiagnosisFormDTOs Diagnosis_form)
        {
            try
            {
                var SaveDiagnosisForm = new DiagnosisFormE
                {
                    i_total_result = Convert.ToInt32(Diagnosis_form.total_result),
                    i_life_style_indicator = Convert.ToInt32(Diagnosis_form.life_style_indicator),
                    i_protective_factors = Convert.ToInt32(Diagnosis_form.protective_factors_indicator),
                    i_risk_behaviors_indicator = Convert.ToInt32(Diagnosis_form.risk_behaviors_indicator),
                    fk_tblusers = UserId,
                    ts_form_registration = Convert.ToDateTime(Diagnosis_form.form_timestamp_registration)
                };

                await _context.DiagnosisFormE.AddAsync(SaveDiagnosisForm);
                await _context.SaveChangesAsync();

                var SelectIdFormCreate=_context.DiagnosisFormE.Select(x => x.id).Max();

                return SelectIdFormCreate;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public async Task<AnswersErrorDTOs> SaveAnswersDiagnosisFormAsync(DiagnosisFormDTOs DiagnosisForm)
        {
            _logger.LogInformation("Start DiagnosisFormCommands.SaveAnswersDiagnosisForm");
            List<AnswersErrorDTOs> DetailAnswerConsult = new List<AnswersErrorDTOs>();
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var UserId =await _context.UserEs.Where(x => x.s_user_email == DiagnosisForm.user_email).Select(x => new { x.id }).FirstOrDefaultAsync() ;

                if (UserId != null && DiagnosisForm.Answers != null)
                {
                    var IdDiagnosisForm= await SaveAnswerDiagnosisFormXUser(UserId.id, DiagnosisForm);

                    if (IdDiagnosisForm != 0)
                    {
                        foreach (var item in DiagnosisForm.Answers)
                        {
                            var SaveAnswersUser = new DiagnosisFormAnswersE
                            {
                                fk_tbldiagnosis_form = IdDiagnosisForm,
                                fk_tblquestion_bank = Convert.ToInt32(item.question),
                                fk_tblanswer_bank = Convert.ToInt32(item.answer)
                            };

                            await _context.DiagnosisFormAnswersE.AddAsync(SaveAnswersUser);
                            await _context.SaveChangesAsync();
                        }

                        DescriptionStateInformation = "Todos los cambios han sido guardados.";
                    }
                    else
                    {
                        StateInformation = false;
                        DescriptionStateInformation = "El formulario de diagnostico no ha sido guardado.";
                    }
                }
                else
                {
                    StateInformation = false;
                    DescriptionStateInformation = "El usuario no ha sido encontrado.";
                }

            }
            catch(Exception ex)
            {
                _logger.LogError("Error to start SaveAnswersDiagnosisForm");
                StateInformation = false;
                DescriptionStateInformation = "Error al iniciar SaveAnswersDiagnosisForm.";
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
