using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.CommonDTOs;
using UNAC.AppSalud.Domain.DTOs.IlnessesDTOs;
using UNAC.AppSalud.Domain.Entities.IllnessesE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Commands.IllnessesCommands
{
    public interface IIllnessesComands
    {
        Task<AnswersErrorDTOs> SaveIllnessesAsync(IllnessesDTOs Illness);
        Task<AnswersErrorDTOs> ChangeIllnessesAsync(IllnessesDTOs Illness);
        Task<AnswersErrorDTOs> DeleteIllnessesAsync(int IdIllness);
    }
    public class IllnessessCommands : IIllnessesComands
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<IllnessessCommands> _logger;
        private readonly IConfiguration _configuration;

        public IllnessessCommands(ILogger<IllnessessCommands> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        private async Task<bool> SaveIllnesses(IllnessesDTOs Illness)
        {
            try
            {
                var SaveIllness = new IllnessesE
                {
                    s_illness_name=Illness.illness_name
                };

                await _context.IllnessesEs.AddAsync(SaveIllness);
                await _context.SaveChangesAsync();
                return true;
            }catch(Exception ex) {
                return false;
            }
        }

        private async Task<bool> ChangeIlllness(IllnessesDTOs Illness)
        {
            try
            {
                var SearchIllness = await _context.IllnessesEs.FirstOrDefaultAsync(x => x.id == Convert.ToInt32(Illness.Id));
                if (SearchIllness != null) {
                    SearchIllness.s_illness_name = Illness.illness_name;
                    await _context.SaveChangesAsync();
                }
                return true;
            }catch(Exception ex)
            {
                return false;
            }
        }

        private async Task<bool> DeleteIllness(int IdIllness)
        {
            try 
            {
                var DeleteIllness = await _context.IllnessesEs.FirstOrDefaultAsync(x => x.id == IdIllness);
                if (DeleteIllness != null)
                {
                    _context.IllnessesEs.Remove(DeleteIllness);
                    await _context.SaveChangesAsync();
                }
                return true;
            }catch(Exception ex)
            {
                return false;   
            }
        }

        public async Task<AnswersErrorDTOs> SaveIllnessesAsync(IllnessesDTOs Illnesses)
        {
            _logger.LogInformation("Start IllnessessCommands.SaveIllnessesAsync");
            List<AnswersErrorDTOs> DetailAnswerConsult = new List<AnswersErrorDTOs>();
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var SaveIllness = await SaveIllnesses(Illnesses);
                StateInformation = SaveIllness;
                DescriptionStateInformation = SaveIllness == true ? "Los cambios se han guardado exitosamente." : "Hay problemas al guardar los datos, intentalo nuevamente.";
            }
            catch(Exception ex)
            {
                _logger.LogError("Error to start IllnessessCommands.SaveIllnessesAsync");
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
        public async Task<AnswersErrorDTOs> ChangeIllnessesAsync(IllnessesDTOs Illnesses)
        {
            _logger.LogInformation("Start IllnessessCommands.ChangeIllnessesAsync");
            List<AnswersErrorDTOs> DetailAnswerConsult = new List<AnswersErrorDTOs>();
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var ChangeIllness=await ChangeIlllness(Illnesses);
                StateInformation = ChangeIllness;
                DescriptionStateInformation = ChangeIllness == true ? "Los cambios se han guardado exitosamente." : "Hay problemas al guardar los datos, intentalo nuevamente.";
            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start IllnessessCommands.ChangeIllnessesAsync");
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

        public async Task<AnswersErrorDTOs> DeleteIllnessesAsync(int IdIllnesses)
        {
            _logger.LogInformation("Start IllnessessCommands.DeleteIllnessesAsync");
            List<AnswersErrorDTOs> DetailAnswerConsult = new List<AnswersErrorDTOs>();
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var CheckIlnness = await _context.MedicinesE.Where(x => x.fk_tblillnesses.Equals(IdIllnesses)).FirstOrDefaultAsync();
                if (CheckIlnness == null)
                {
                    var DeleteIllnessBD = await DeleteIllness(IdIllnesses);
                    StateInformation = DeleteIllnessBD;
                    DescriptionStateInformation = DeleteIllnessBD == true ? "Los cambios se han guardado exitosamente." : "Hay problemas al guardar los datos, intentalo nuevamente.";
                }
                else
                {
                    StateInformation = false;
                    DescriptionStateInformation = "La enfermedad es usada por un medicamento";
                }
                

            }
            catch (Exception ex)
            {
                _logger.LogError("Error to start IllnessessCommands.DeleteIllnessesAsync");
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
