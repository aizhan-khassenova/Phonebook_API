using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Contracts
{
	public interface IApartmentRepository
	{
        public Task<List<Apartment>> GetApartment();
        public Task<Apartment> CreateApartment(int houseId, ApartmentForCreate apartment);
        public Task DeleteApartment(int apartmentId);
    }
}
