using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.DTOs.LoginDTOs.LoginDTOs;
using UNAC.AppSalud.Domain.Entities.LoginE;

namespace UNAC.AppSalud.Domain.DTOs.LoginDTOs
{
    public class CodigoRestablecimientoDTOs
    {
        public string s_codigo { get; set; }
        public string s_correo { get; set; }

        public static CodigoRestablecimientoDTOs CreateDTO(CodigoRestablecimientoE codigoRestablecimiento)
        {
            CodigoRestablecimientoDTOs CodigoRestablecimientoDTOs = new()
            {
                s_codigo = codigoRestablecimiento.s_codigo,
                s_correo = codigoRestablecimiento.s_correo,
            };
            return CodigoRestablecimientoDTOs;
        }

    }
}
