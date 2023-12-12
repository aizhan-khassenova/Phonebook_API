using Dapper;
using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Repository
{
	public class ApartmentRepository : IApartmentRepository
	{
		private readonly DapperContext _context;

		public ApartmentRepository(DapperContext context)
		{
			_context = context;
		}

        public async Task<List<Apartment>> GetApartment()
        {
            var query = "SELECT * FROM Apartment FULL JOIN Phone ON Apartment.Apartment_ID = Phone.Apartment_ID";

            using (var connection = _context.CreateConnection())
            {
                var apartmentDict = new Dictionary<int, Apartment>();

                var apartments = await connection.QueryAsync<Apartment, Phone, Apartment>(
                    query, (apartment, phone) =>
                    {
                        if (!apartmentDict.TryGetValue(apartment.Apartment_ID, out var currentApartment))
                        {
                            currentApartment = apartment;
                            apartmentDict.Add(currentApartment.Apartment_ID, currentApartment);
                        }

                        currentApartment.Phones.Add(phone);

                        return currentApartment;
                    },

                    splitOn: "Phone_ID"
                );

                return apartments.Distinct().ToList();
            }
        }

        public async Task<Apartment> CreateApartment(int houseId, ApartmentForCreate apartment)
        {
            var procedureName = "dbo.Apartment_Insert";
            var parameters = new DynamicParameters();
            parameters.Add("HouseID", houseId, DbType.Int32);
            parameters.Add("ApartmentNumber", apartment.Apartment_Number, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var createdApartment = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return createdApartment;
            }
        }

        public async Task DeleteApartment(int apartmentId)
        {
            var procedureName = "dbo.Apartment_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("ApartmentID", apartmentId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}