namespace UNAC.AppSalud.Domain.DTOs.AnswersBankDTOs
{
    public class AnswersBankDTOs
    {
        public string? Id { get; set; }
        public string? answer_description { get; set; }
        public string? answer_value { get; set; }
        public string? fk_tblquestion_bank { get; set; }
        public string? fk_tblqustion_bank_description { get; set; }
    }
}
