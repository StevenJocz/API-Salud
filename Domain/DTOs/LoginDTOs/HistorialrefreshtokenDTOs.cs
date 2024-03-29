﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.Entities.LoginE;

namespace UNAC.AppSalud.Domain.DTOs.LoginDTOs.LoginDTOs
{
    public class HistorialrefreshtokenDTOs
    {

        public int IdHistorialToken { get; set; }
        public int IdUsuario { get; set; }
        public string Token { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }

        public static HistorialrefreshtokenDTOs CreateDTO(HistorialrefreshtokenE historialrefreshtoken)
        {
            HistorialrefreshtokenDTOs historialrefreshtokenDTOs = new()
            {
                IdHistorialToken = historialrefreshtoken.idhistorialtoken,
                IdUsuario = historialrefreshtoken.idusuario,
                Token = historialrefreshtoken.s_token,
                FechaCreacion = historialrefreshtoken.ts_fechacreacion,
                FechaExpiracion = historialrefreshtoken.ts_fechaexpiracion,
            };
            return historialrefreshtokenDTOs;
        }
    }

}
