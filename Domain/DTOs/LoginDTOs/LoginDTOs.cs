using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.Entities.LoginE;

namespace UNAC.AppSalud.Domain.DTOs.Login.LoginDTOs
{
    public class LoginDTOs
    {
        public string userEmail { get; set; }
        public string userPassword { get; set; }
        public string user_name { get; set; }

        public static LoginDTOs CreateDTO(LoginE LoginE)
        {
            LoginDTOs LoginDTOs = new()
            {
                userEmail = LoginE.userEmail,
                userPassword = LoginE.userPassword,
                user_name = LoginE.user_name,
            };
            return LoginDTOs;
        }
    }
}
