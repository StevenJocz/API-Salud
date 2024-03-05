using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.Entities.LocationE;
using UNAC.AppSalud.Domain.Entities.UserE;

namespace UNAC.AppSalud.Domain.DTOs.LocationDTOs
{
    public class CountriesDTOs
    {
        public int id { get; set; }
        public string s_country_name { get; set; }

        public static CountriesDTOs CreateDTO(CountriesE countriesE)
        {
            CountriesDTOs countriesDTOs = new()
            {
                id = countriesE.id,
                s_country_name = countriesE.s_country_name,
            };
            return countriesDTOs;
        }


        public static CountriesE CreateE(CountriesDTOs countriesDTOs)
        {
            CountriesE countriesE = new()
            {
                id = countriesDTOs.id,
                s_country_name = countriesDTOs.s_country_name,
            };
            return countriesE;
        }
    }
}
