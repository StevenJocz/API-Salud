using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UNAC.AppSalud.Domain.DTOs.EmailDTOs
{
    public class EmailDTOs
    {
        public string Para { set; get; } = string.Empty;
        public string Asunto { set; get; } = string.Empty;
        public string Contenido { set; get; } = string.Empty;
    }
}
