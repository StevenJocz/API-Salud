namespace UNAC.AppSalud.Domain.Entities.DiagnosticFormE
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbldiagnosis_form")]

    public class DiagnosisFormE {
        public int i_total_result { get; set; }
        public int i_life_style_indicator { get; set; }
        public int i_protective_factors { get; set; }
        public int i_risk_behavior_indicator { get; set; }
        public int fk_tblusers { get; set; }
        public DateTime ts_form_registration { get; set; }
    }

}
