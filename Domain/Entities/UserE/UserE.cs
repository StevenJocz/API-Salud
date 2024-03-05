using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace UNAC.AppSalud.Domain.Entities.UserE
{
    [Table("tbl_users")]

    public class UserE
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        public string s_user_name { get; set; }
        public string s_user_lastname { get; set; }
        public DateTime dt_user_birthdate { get; set; }
        public string s_user_gender { get; set; }
        public int fk_user_address_city { get; set; }
        public string s_user_cellphone { get; set; }
        public string s_user_email { get; set; }

    }
}
