namespace UNAC.AppSalud.Domain.Entities.AnswersBankE
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using UNAC.AppSalud.Domain.Entities.QuestionsBankE;

    [Table("tbl_answer_bank")]
    public class AnswersBankE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string s_answer_description { get; set; }
        public int i_answer_value { get; set; }
        public int fk_tblquestion_bank { get; set; }
    }
}
