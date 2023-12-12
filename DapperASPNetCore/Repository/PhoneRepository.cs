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
	public class PhoneRepository : IPhoneRepository
	{
		private readonly DapperContext _context;

		public PhoneRepository(DapperContext context)
		{
			_context = context;
		}

        public async Task<IEnumerable<Phone>> GetPhone()
        {
            //var query = "SELECT Phone.Phone_ID, Phone.Phone_Number, Phone.Owner_Name, Apartment.Apartment_ID " +
            //    "FROM Phone " +
            //    "FULL JOIN Apartment ON Phone.Apartment_ID = Apartment.Apartment_ID";

            var query = "SELECT Phone_ID, Phone_Number, Owner_Name FROM Phone";

            using (var connection = _context.CreateConnection())
            {
                var phones = await connection.QueryAsync<Phone>(query);
                return phones.ToList();
            }
        }

        public async Task<Phone> CreatePhone(int apartmentId, PhoneForCreate phone)
        {
            var procedureName = "dbo.Phone_Insert";
            var parameters = new DynamicParameters();
            parameters.Add("ApartmentID", apartmentId, DbType.Int32);
            parameters.Add("PhoneNumber", phone.Phone_Number, DbType.String, ParameterDirection.Input);
            parameters.Add("OwnerName", phone.Owner_Name, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var createdPhone = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return createdPhone;
            }
        }

        public async Task<Phone> UpdatePhone(int phoneId, PhoneForUpdate phone)
        {
            var procedureName = "dbo.Phone_Update";
            var parameters = new DynamicParameters();
            parameters.Add("PhoneID", phoneId, DbType.Int32);
            parameters.Add("NewPhoneNumber", phone.Phone_Number, DbType.String, ParameterDirection.Input);
            parameters.Add("NewOwnerName", phone.Owner_Name, DbType.String, ParameterDirection.Input);

            using (var connection = _context.CreateConnection())
            {
                var updatedPhone = await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
                return updatedPhone;
            }
        }

        public async Task DeletePhone(int phoneId)
        {
            var procedureName = "dbo.Phone_Delete";
            var parameters = new DynamicParameters();
            parameters.Add("PhoneID", phoneId, DbType.Int32);

            using (var connection = _context.CreateConnection())
            {
                await connection.QueryFirstOrDefaultAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
            }
        }
    }
}