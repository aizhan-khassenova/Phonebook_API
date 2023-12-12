using Dapper;
using DapperASPNetCore.Context;
using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using DapperASPNetCore.Entities;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Repository
{
	public class HouseRepository : IHouseRepository
	{
		private readonly DapperContext _context;

		public HouseRepository(DapperContext context)
		{
			_context = context;
		}

        public async Task<List<House>> GetHouse()
        {
            var query = "SELECT * FROM House FULL JOIN Apartment ON House.House_ID = Apartment.House_ID";

            using (var connection = _context.CreateConnection())
            {
                var houseDict = new Dictionary<int, House>();

                var houses = await connection.QueryAsync<House, Apartment, House>(
                    query, (house, apartment) =>
                    {
                        if (!houseDict.TryGetValue(house.House_ID, out var currentHouse))
                        {
                            currentHouse = house;
                            houseDict.Add(currentHouse.House_ID, currentHouse);
                        }

                        currentHouse.Apartments.Add(apartment);

                        return currentHouse;
                    },

                    splitOn: "Apartment_ID"
                );

                return houses.Distinct().ToList();
            }
        }

        public async Task<House> CreateHouse(int streetId, HouseForCreate house)
        {
            var procedureName = "dbo.House_Insert";
            var parameters = new DynamicParameters();
            parameters.Add("StreetID", streetId, DbType.Int32);
            parameters.Add("HouseNumber", house.House_Number, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var createdHouse = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return createdHouse;
            }
        }

        public async Task DeleteHouse(int houseId)
        {
            var procedureName = "dbo.House_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("HouseID", houseId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}