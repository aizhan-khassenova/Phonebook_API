using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using DapperASPNetCore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface IPhonebookRepository
    {
        public Task<IEnumerable<PhonebookForGet>> GetPhonebook();
        public Task<List<City>> GetPhonebookByCity();
        public Task<IEnumerable<PhonebookForFilter>> GetPhoneOwnersByCityAndStreet(int cityId);
        public Task<string> GetStreetNameByHouseID(int houseId);
    }
}
