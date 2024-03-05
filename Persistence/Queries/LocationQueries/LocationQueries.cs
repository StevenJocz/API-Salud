using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UNAC.AppSalud.Domain.DTOs.LocationDTOs;
using UNAC.AppSalud.Domain.DTOs.MedicineDTOs;
using UNAC.AppSalud.Domain.Entities.LocationE;
using UNAC.AppSalud.Domain.Entities.UserE;
using UNAC.AppSalud.Infrastructure;
using UNAC.AppSalud.Persistence.Commands.LoginCommands;
using UNAC.AppSalud.Persistence.Commands.UserCommands;
using UNAC.AppSalud.Persistence.Utilidades;

namespace UNAC.AppSalud.Persistence.Queries.LocationQueries
{
    public interface ILocationQueries
    {
        Task<List<CountriesDTOs>> listCountries();
        Task<List<StatesDTOs>> listStates(int idCountrie);
        Task<List<CitiesDTOs>> listCities(int idState);
    }
    public class LocationQueries: ILocationQueries, IDisposable
    {
        private readonly SaludDbContext _context = null;
        private readonly ILogger<LocationQueries> _logger;
        private readonly IConfiguration _configuration;

        public LocationQueries(ILogger<LocationQueries> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            string? connectionString = _configuration.GetConnectionString("Connection_Salud");
            _context = new SaludDbContext(connectionString);
        }

        #region implementacion Disponse
        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                disposed = true;
            }
        }

        #endregion


        public async Task<List<CountriesDTOs>> listCountries()
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuario...");
            try
            {
                var countries = await _context.CountriesEs.AsNoTracking().ToListAsync();

                var listsCountries = new  List <CountriesDTOs>();

                if (countries != null && countries.Count > 0)
                {
                    foreach (var item in countries)
                    {
                        var list = new CountriesDTOs
                        {
                            id = item.id,
                            s_country_name = item.s_country_name,
                        };
                        listsCountries.Add(list);
                    }
                    return listsCountries;
                }
                else
                {
                    return listsCountries;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<StatesDTOs>> listStates(int idCountrie)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuario...");
            try
            {
                var states = await _context.StatesEs.AsNoTracking().Where(x => x.fk_tblcountries == idCountrie).ToArrayAsync();

                var listsStates = new List<StatesDTOs>();

                if (states != null )
                {
                    foreach (var item in states)
                    {
                        var list = new StatesDTOs
                        {
                            id = item.id,
                            s_state_name = item.s_state_name,
                        };
                        listsStates.Add(list);
                    }
                    return listsStates;
                }
                else
                {
                    return listsStates;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<List<CitiesDTOs>> listCities(int idState)
        {
            _logger.LogTrace("Iniciando metodo LoginQueries.ConsultarUsuario...");
            try
            {
                var cities = await _context.CitiesEs.AsNoTracking().Where(x => x.fk_tblstates == idState).ToArrayAsync();

                var listsCities = new List<CitiesDTOs>();

                if (cities != null)
                {
                    foreach (var item in cities)
                    {
                        var list = new CitiesDTOs
                        {
                            id = item.id,
                            s_city_name = item.s_city_name,
                        };
                        listsCities.Add(list);
                    }
                    return listsCities;
                }
                else
                {
                    return listsCities;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
