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
	public class StreetRepository : IStreetRepository
	{
		private readonly DapperContext _context;

		public StreetRepository(DapperContext context)
		{
			_context = context;
		}

        public async Task<List<Street>> GetStreet()
        {
            var query = @"
                SELECT Street.*, City_Street.*, House.*
                FROM Street
                FULL JOIN City_Street ON Street.Street_ID = City_Street.Street_ID
                FULL JOIN House ON City_Street.Street_ID = House.Street_ID";

            using (var connection = _context.CreateConnection())
            {
                var streetDict = new Dictionary<int, Street>();

                var streets = await connection.QueryAsync<Street, House, Street>(
                    query, (street, house) =>
                    {
                        if (!streetDict.TryGetValue(street.Street_ID, out var currentStreet))
                        {
                            currentStreet = street;
                            streetDict.Add(currentStreet.Street_ID, currentStreet);
                        }

                        currentStreet.Houses.Add(house);

                        return currentStreet;
                    },

                     splitOn: "City_Street_ID, House_ID"
                );

                return streets.Distinct().ToList();
            }
        }

        public async Task<Street> CreateStreet(int cityId, StreetForCreate street)
        {
            var procedureName = "dbo.Street_Insert";
            var parameters = new DynamicParameters();
            parameters.Add("CityID", cityId, DbType.Int32);
            parameters.Add("StreetName", street.Street_Name, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var createdStreet = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return createdStreet;
            }
        }

        public async Task<Street> UpdateStreet(int streetId, StreetForUpdate street)
        {
            var procedureName = "dbo.Street_Update";
            var parameters = new DynamicParameters();
            parameters.Add("StreetID", streetId, DbType.Int32);
            parameters.Add("NewStreetName", street.Street_Name, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var updatedStreet = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return updatedStreet;
            }
        }

        public async Task DeleteStreet(int streetId)
        {
            var procedureName = "dbo.Street_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("StreetID", streetId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}