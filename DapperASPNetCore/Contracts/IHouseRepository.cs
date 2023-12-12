using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface IHouseRepository
	{
        public Task<List<House>> GetHouse();
        public Task<House> CreateHouse(int streetId, HouseForCreate house);
        public Task DeleteHouse(int houseId);
    }
}
