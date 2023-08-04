namespace UNAC.AppSalud.Domain.Entities.MedicinesE
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using UNAC.AppSalud.Domain.Entities.IllnessesE;

    [Table("tbl_medicines")]
    public class MedicinesE
    {
        [Key]
        public int id { get; set; }
        public string s_medicine_name { get; set; }
        public string s_medicine_description { get; set; }
        public string s_medicine_indications { get; set; }
        public int fk_tblillnesses { get; set; }
    }
}
