using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.Entities.LoginE
{
    [Table("tbl_codigo_restablecimiento")]
    public class CodigoRestablecimientoE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id_codigo_restablecimiento { get; set; }
        public string s_codigo { get; set; }
        public string s_correo { get; set; }
    }
}
