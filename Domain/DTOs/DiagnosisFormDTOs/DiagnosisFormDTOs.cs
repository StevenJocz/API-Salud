namespace UNAC.AppSalud.Domain.DTOs.DiagnosisFormDTOs
{
    public class DiagnosisFormDTOs
    {
        public string user_email { get; set; }
        public int total_result { get; set; }
        public int life_style_indicator { get; set; }
        public int protective_factors_indicator { get; set; }
        public int risk_behaviors_indicator { get; set; }
        public DateTime form_timestamp_registration { get; set; }
        public List<DiagnosisFormAnswersDTOs> Answers { get; set; }
    }
}
