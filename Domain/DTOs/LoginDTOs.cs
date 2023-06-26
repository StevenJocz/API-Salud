using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.DTOs
{
    public class LoginDTOs
    {
        public string userEmail { get; set; }
        public string userPassword { get; set; }

        public static LoginDTOs CreateDTO(LoginE LoginE)
        {
            LoginDTOs LoginDTOs = new()
            {
                userEmail = LoginE.userEmail,
                userPassword = LoginE.userPassword,
            };
            return LoginDTOs;
        }
    }
}
