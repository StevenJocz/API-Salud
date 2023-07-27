namespace UNAC.AppSalud.Domain.Entities.User
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("tbl_users")]

    public class UserE
    {
        [Key]
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
