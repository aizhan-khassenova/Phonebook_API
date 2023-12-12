using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface IStreetRepository
	{
        public Task<List<Street>> GetStreet();
        public Task<Street> CreateStreet(int cityId, StreetForCreate street);
        public Task<Street> UpdateStreet(int streetId, StreetForUpdate street);
        public Task DeleteStreet(int streetId);
    }
}
