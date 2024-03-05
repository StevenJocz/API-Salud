using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.Entities.LocationE
{
    [Table("tbl_states")]
    public class StatesE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string s_state_name { get; set; }
        public int fk_tblcountries { get; set; }
    }
}
