using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UNAC.AppSalud.Domain.DTOs.CommonDTOs;
using UNAC.AppSalud.Domain.DTOs.MedicineFormDTOs;
using UNAC.AppSalud.Domain.Entities.MedicinesRegistrationE;
using UNAC.AppSalud.Infrastructure;

namespace UNAC.AppSalud.Persistence.Commands.MedicineFormCommands
{
    public interface IMedicineFormCommands
    {
        Task<AnswersErrorDTOs> SaveAnswersMedicineFormAsync(MedicineFormDTOs Medicine_fsorm);
    }
    public class MedicineFormCommands : IMedicineFormCommands, IDisposable
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<MedicineFormCommands> _logger;
        private readonly IConfiguration _configuration;

        public MedicineFormCommands(ILogger<MedicineFormCommands> logger, IConfiguration configuration)
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

        private async Task<bool> SaveAnswersMedicineFormXUser(int UserId, MedicineFormDTOs Medicine_form)
        {
            try
            {
                var SaveMedicineForm = new MedicinesRegistrationE
                {
                    fk_tblmedicines = Convert.ToInt32(Medicine_form.medicine_id),
                    fk_tblusers = UserId,
                    s_medicine_type = Medicine_form.medicine_type == null ? "0" : Convert.ToString(Medicine_form.medicine_type),
                    s_medicine_presentation = Medicine_form.medicine_presentation == null ? "0" : Convert.ToString(Medicine_form.medicine_presentation),
                    s_medicine_administration_way = Medicine_form.medicine_administration_way == null ? "0" : Convert.ToString(Medicine_form.medicine_administration_way),
                    i_medicine_daily_times = Medicine_form.medicine_daily_times == null ? 0 : Convert.ToInt32(Medicine_form.medicine_daily_times),
                    ts_medicine_hour = Convert.ToDateTime(Medicine_form.medicine_hour),
                    ts_medicine_registration =  Convert.ToDateTime(Medicine_form.medicine_registration)
                };

                await _context.MedicinesRegistrationE.AddAsync(SaveMedicineForm);
                await _context.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public async Task<AnswersErrorDTOs> SaveAnswersMedicineFormAsync(MedicineFormDTOs Medicine_form) 
        {
            _logger.LogInformation("Start SaveAnswersMedicineFormAsync");
            List<AnswersErrorDTOs> DetailAnswerConsult = new List<AnswersErrorDTOs>();
            bool StateInformation = true;
            string DescriptionStateInformation = "";
            try
            {
                var UserId = await _context.UserEs.Where(x => x.s_user_email == Medicine_form.user_email).Select(x => new { x.id }).FirstOrDefaultAsync();
                var ValidMedicine = await _context.MedicinesE.Where(x => x.id.Equals(Convert.ToInt32(Medicine_form.medicine_id))).FirstOrDefaultAsync();
                if (UserId != null && ValidMedicine != null) {
                    var SaveAnswerMedicineForm= await SaveAnswersMedicineFormXUser(UserId.id, Medicine_form);
                    StateInformation = SaveAnswerMedicineForm;
                    DescriptionStateInformation = SaveAnswerMedicineForm == true ? "Los cambios se han guardado exitosamente." : "Hay problemas al guardar los datos, intentalo nuevamente.";
                }
                else
                {
                    StateInformation = false;
                    DescriptionStateInformation = "La información del usuario no es correcta o el medicamento no existe, intentelo nuevamente.";
                }
            }
            catch (Exception ex) {
                _logger.LogError("Error en el metodo MedicineFormCommands.SaveAnswersMedicineFormAsync...");
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
