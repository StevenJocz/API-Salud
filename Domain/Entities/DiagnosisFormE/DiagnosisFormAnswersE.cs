namespace UNAC.AppSalud.Domain.Entities.DiagnosisFormAnswersE
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ti_diagnosis_form_answers")]

    public class DiagnosisFormAnswersE
    {
        [Key]
        public int id { get; set; }
        public int fk_tbldiagnosis_form { get; set; }
        public int fk_tblquestion_bank { get; set; }
        public int fk_tblanswer_bank { get; set; }
    }
}
