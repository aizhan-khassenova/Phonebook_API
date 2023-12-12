using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface IPhoneRepository
	{
        public Task<IEnumerable<Phone>> GetPhone();
        public Task<Phone> CreatePhone(int apartmentId, PhoneForCreate phone);
        public Task<Phone> UpdatePhone(int phoneId, PhoneForUpdate phone);
        public Task DeletePhone(int phoneId);
    }
}
