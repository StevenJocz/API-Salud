using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.Entities.LoginE
{
    [Table("tbl_login")]
    public class LoginE
    {
        [Key]
        public int IdLogin { get; set; }
        public string s_userEmail { get; set; }
        public string s_userPassword { get; set; }
        public int fk_tblusers { get; set; }

    }
}
