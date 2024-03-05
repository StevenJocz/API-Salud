namespace UNAC.AppSalud.Domain.Entities.MedicinesRegistrationE
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Globalization;

    [Table("ti_medicines_registration")]
    public class MedicinesRegistrationE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public int fk_tblmedicines { get; set; }
        public int fk_tblusers { get; set; }
        public string s_medicine_type { get; set; }
        public string s_medicine_presentation { get; set; }
        public string s_medicine_administration_way { get; set; }
        public int i_medicine_daily_times { get; set; }
        public DateTime ts_medicine_hour { get; set; }
        public DateTime ts_medicine_registration { get; set; }
    }
}
