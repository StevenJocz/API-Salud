using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.Entities.LocationE;

namespace UNAC.AppSalud.Domain.DTOs.LocationDTOs
{

    public class StatesDTOs
    {
        public int id { get; set; }
        public string s_state_name { get; set; }
        public int fk_tblcountries { get; set; }

        public static StatesDTOs CreateDTO(StatesE statesE)
        {
            StatesDTOs statesDTOs = new()
            {
                id = statesE.id,
                s_state_name = statesE.s_state_name,
                fk_tblcountries = statesE.fk_tblcountries,
            };
            return statesDTOs;
        }


        public static StatesE CreateE(StatesDTOs statesDTOs)
        {
            StatesE statesE = new()
            {
                id = statesDTOs.id,
                s_state_name = statesDTOs.s_state_name,
                fk_tblcountries = statesDTOs.fk_tblcountries,
            };
            return statesE;
        }
    }
}
