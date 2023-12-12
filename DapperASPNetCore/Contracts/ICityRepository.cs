using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface ICityRepository
	{
        public Task<List<City>> GetCity();
        //public Task<IEnumerable<City>> GetCity();
        public Task<City> CreateCity(CityForCreate city);
        public Task<City> UpdateCity(int cityId, CityForUpdate city);
        public Task DeleteCity(int cityId);
    }
}
