using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.Entities.LocationE;

namespace UNAC.AppSalud.Domain.DTOs.LocationDTOs
{
    public class CitiesDTOs
    {
        public int id { get; set; }
        public string s_city_name { get; set; }
        public int fk_tblstates { get; set; }

        public static CitiesDTOs CreateDTO(CitiesE citiesE)
        {
            CitiesDTOs citiesDTOs = new()
            {
                id = citiesE.id,
                s_city_name = citiesE.s_city_name,
                fk_tblstates = citiesE.fk_tblstates,
            };
            return citiesDTOs;
        }


        public static CitiesE CreateE(CitiesDTOs citiesDTOs)
        {
            CitiesE citiesE = new()
            {
                id = citiesDTOs.id,
                s_city_name = citiesDTOs.s_city_name,
                fk_tblstates = citiesDTOs.fk_tblstates,
            };
            return citiesE;
        }
    }
}
