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
    public class CityRepository : ICityRepository
	{
		private readonly DapperContext _context;

        public CityRepository(DapperContext context)
        {
			_context = context;
        }

        public async Task<List<City>> GetCity()
        {
            var query = @"
                SELECT City.*, City_Street.*, Street.*
                FROM City
                FULL JOIN City_Street ON City.City_ID = City_Street.City_ID
                FULL JOIN Street ON City_Street.Street_ID = Street.Street_ID";

            using (var connection = _context.CreateConnection())
            {
                var cityDict = new Dictionary<int, City>();

                var cities = await connection.QueryAsync<City, Street, City>(
                    query, (city, street) =>
                    {
                        if (!cityDict.TryGetValue(city.City_ID, out var currentCity))
                        {
                            currentCity = city;
                            cityDict.Add(currentCity.City_ID, currentCity);
                        }

                        currentCity.Streets.Add(street);

                        return currentCity;
                    },

                     splitOn: "City_Street_ID, Street_ID"
                );

                return cities.Distinct().ToList();
            }
        }

        //public async Task<IEnumerable<City>> GetCity()
        //{
        //    var query = "SELECT * FROM City";
        //    using (var connection = _context.CreateConnection())
        //    {
        //        var cities = await connection.QueryAsync<City>(query);
        //        return cities.ToList();
        //    }
        //}

        public async Task<City> CreateCity(CityForCreate city)
        {
            var procedureName = "dbo.City_Insert";
            var parameters = new DynamicParameters();
            parameters.Add("cityName", city.City_Name, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection()) 
            {
                var createdCity = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure); 
                return createdCity;
            }
        }

        public async Task<City> UpdateCity(int cityId, CityForUpdate city)
        {
            var procedureName = "dbo.City_Update";
            var parameters = new DynamicParameters();
            parameters.Add("CityID", cityId, DbType.Int32);
            parameters.Add("NewCityName", city.City_Name, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var updatedCity = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return updatedCity;
            }
        }

        public async Task DeleteCity(int cityId)
        {
            var procedureName = "dbo.City_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("CityID", cityId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}