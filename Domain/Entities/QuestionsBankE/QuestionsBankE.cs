namespace UNAC.AppSalud.Domain.Entities.QuestionsBankE
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbl_question_bank")]
    public class QuestionsBankE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string s_question_description { get; set; }
    }
}
