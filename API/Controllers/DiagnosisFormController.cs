namespace UNAC.AppSalud.API.Controllers
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Org.BouncyCastle.Asn1.Ocsp;
    using UNAC.AppSalud.Domain.DTOs.CommonDTOs;
    using UNAC.AppSalud.Domain.DTOs.DiagnosisFormDTOs;
    using UNAC.AppSalud.Persistence.Commands.DiagnosisFormCommands;
    using UNAC.AppSalud.Persistence.Commands.LoginCommands;
    using UNAC.AppSalud.Persistence.Queries.LoginQueries;

    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosisFormController : ControllerBase
    {
        private readonly IDiagnosisFormCommands _diagnosisFormServiceService;
        private readonly ILoginCommands _loginCommands;
        private readonly ILoginQueries _loginQueries;
        private readonly ILogger<LoginController> _logger;

        public DiagnosisFormController(IDiagnosisFormCommands diagnosisService, ILoginCommands loginCommands, ILoginQueries loginQueries, ILogger<LoginController> logger)
        {
            _diagnosisFormServiceService = diagnosisService;
            _loginCommands = loginCommands;
            _loginQueries = loginQueries;
            _logger = logger;
        }

        [HttpPost("SaveAnswersDiagnosisForms")]
        public async Task<IActionResult> SaveAnswersDiagnosisForm([FromBody] DiagnosisFormDTOs DiagnosisForm)
        {
            try
            {
                _logger.LogInformation("Start SaveAnswersDiagnosisForm.Controller");
                AnswersErrorDTOs SaveAnswersDiagnosisForm = await _diagnosisFormServiceService.SaveAnswersDiagnosisForm(DiagnosisForm);
                return Ok(new
                {
                    resultado = SaveAnswersDiagnosisForm.stateError,
                    message = SaveAnswersDiagnosisForm.descriptionError,
                });
            }
            catch (Exception ex) {
                _logger.LogError("Error to start SaveAnswersDiagnosisForm.Controller");
                throw;
            }
        }
    }
}
