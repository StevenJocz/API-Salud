namespace UNAC.AppSalud.Domain.Entities.DiagnosisFormAnswersE
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tidiagnosis_form_answers")]

    public class DiagnosisFormAnswersE
    {
        public int fk_tbldiagnosis_form { get; set; }
        public int fk_tblquestion_bank { get; set; }
        public int fk_tblanswer_bank { get; set; }
    }
}
