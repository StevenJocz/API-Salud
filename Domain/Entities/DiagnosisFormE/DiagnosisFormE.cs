namespace UNAC.AppSalud.Domain.Entities.DiagnosticFormE
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbl_diagnosis_form")]

    public class DiagnosisFormE {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int i_total_result { get; set; }
        public int i_life_style_indicator { get; set; }
        public int i_protective_factors { get; set; }
        public int i_risk_behaviors_indicator { get; set; }
        public int fk_tblusers { get; set; }
        public DateTime ts_form_registration { get; set; }
    }

}
