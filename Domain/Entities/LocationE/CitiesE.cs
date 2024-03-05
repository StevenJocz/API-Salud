using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.Entities.LocationE
{
    [Table("tbl_cities")]
    public class CitiesE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string s_city_name { get; set; }
        public int fk_tblstates { get; set; }
    }
}
