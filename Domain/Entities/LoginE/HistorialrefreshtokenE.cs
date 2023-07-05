using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.Entities.LoginE
{
    [Table("historialrefreshtoken")]
    public class HistorialrefreshtokenE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int idhistorialtoken { get; set; }
        public int idusuario { get; set; }
        public string token { get; set; }
        public DateTime fechacreacion { get; set; }
        public DateTime fechaexpiracion { get; set; }
    }
}
