using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.Entities.LoginE;
using UNAC.AppSalud.Domain.Entities.UserE;

namespace UNAC.AppSalud.Domain.DTOs.UserDTOs
{
    public class UserDTOs
    {
        public int id { get; set; }
        public string s_user_name { get; set; }
        public string s_user_lastname { get; set; }
        public DateTime dt_user_birthdate { get; set; }
        public string s_user_gender { get; set; }
        public int fk_user_address_city { get; set; }
        public string s_user_cellphone { get; set; }
        public string s_user_email { get; set; }
        public string Password { get; set; }

        public static UserDTOs CreateDTO(UserE UserE)
        {
            UserDTOs UserDTOs = new()
            {
                id = UserE.id,
                s_user_name = UserE.s_user_name,
                s_user_lastname = UserE.s_user_lastname,
                dt_user_birthdate = UserE.dt_user_birthdate,
                s_user_gender = UserE.s_user_gender,
                fk_user_address_city = UserE.fk_user_address_city,
                s_user_cellphone = UserE.s_user_cellphone,
                s_user_email = UserE.s_user_email   

            };
            return UserDTOs;
        }


        public static UserE CreateE(UserDTOs UserDTOs)
        {
            UserE UserE = new()
            {
                id = UserDTOs.id,
                s_user_name = UserDTOs.s_user_name,
                s_user_lastname = UserDTOs.s_user_lastname,
                dt_user_birthdate = UserDTOs.dt_user_birthdate,
                s_user_gender = UserDTOs.s_user_gender,
                fk_user_address_city = UserDTOs.fk_user_address_city,
                s_user_cellphone = UserDTOs.s_user_cellphone,
                s_user_email = UserDTOs.s_user_email

            };
            return UserE;
        }
    }
}
