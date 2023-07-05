using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.Entities.LoginE
{
    [Table("login")]
    public class LoginE
    {
        [Key]
        public int IdLogin { get; set; }
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public string user_name { get; set; }

    }
}
