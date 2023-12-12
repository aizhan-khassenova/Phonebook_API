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
    public class PhonebookRepository : IPhonebookRepository
    {
        private readonly DapperContext _context;

        public PhonebookRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PhonebookForGet>> GetPhonebook()
        {
            var query = "SELECT * FROM dbo.Phonebook_Select()";

            using (var connection = _context.CreateConnection())
            {
                var phonebook = await connection.QueryAsync<PhonebookForGet>(query);
                return phonebook.ToList();
            }
        }

        public async Task<List<City>> GetPhonebookByCity()
        {
            var query = @"
                SELECT City.*, City_Street.*, Street.*, House.*, Apartment.*, Phone.* 
                FROM City
                FULL JOIN City_Street ON City.City_ID = City_Street.City_ID
                FULL JOIN Street ON City_Street.Street_ID = Street.Street_ID 
                FULL JOIN House ON City_Street.Street_ID = House.Street_ID 
                FULL JOIN Apartment ON House.House_ID = Apartment.House_ID 
                FULL JOIN Phone ON Apartment.Apartment_ID = Phone.Apartment_ID
                ORDER BY City.City_Name, Street.Street_Name, House.House_Number, Apartment.Apartment_Number, Phone.Owner_Name";

            using (var connection = _context.CreateConnection())
            {
                var cityDict = new Dictionary<int, City>();

                var cities = await connection.QueryAsync<City, Street, House, Apartment, Phone, City>(
                    query, (city, street, house, apartment, phone) =>
                    {
                        city ??= new City();
                        street ??= new Street();
                        house ??= new House();
                        apartment ??= new Apartment();
                        phone ??= new Phone();
                        
                        if (!cityDict.TryGetValue(city.City_ID, out var currentCity))
                        {
                            currentCity = city;
                            currentCity.Streets = new List<Street>();
                            cityDict.Add(currentCity.City_ID, currentCity);
                        }
                        
                        var currentStreet = currentCity.Streets.FirstOrDefault(s => s.Street_ID == street.Street_ID);
                        if (currentStreet == null)
                        {
                            currentStreet = street;
                            currentStreet.Houses = new List<House>();
                            currentCity.Streets.Add(currentStreet);
                        }

                        var currentHouse = currentStreet.Houses.FirstOrDefault(h => h.House_ID == house.House_ID);
                        if (currentHouse == null)
                        {
                            currentHouse = house;
                            currentHouse.Apartments = new List<Apartment>();
                            currentStreet.Houses.Add(currentHouse);
                        }

                        var currentApartment = currentHouse.Apartments.FirstOrDefault(a => a.Apartment_ID == apartment.Apartment_ID);
                        if (currentApartment == null)
                        {
                            currentApartment = apartment;
                            currentApartment.Phones = new List<Phone>();
                            currentHouse.Apartments.Add(currentApartment);
                        }

                        currentApartment.Phones.Add(phone);

                        return currentCity;
                    },

                    splitOn: "City_ID, Street_ID, House_ID, Apartment_ID, Phone_ID"
                );

                return cities.Distinct().ToList();
            }
        }

        public async Task<IEnumerable<PhonebookForFilter>> GetPhoneOwnersByCityAndStreet(int cityId)
        {
            var query = "SELECT * FROM dbo.GetPhoneOwnersByCityAndStreet(@CityID)";
            using (var connection = _context.CreateConnection())
            {
                var phonebook = await connection.QueryAsync<PhonebookForFilter>(query, new { CityID = cityId });

                return phonebook.ToList();
            }
        }

        public async Task<string> GetStreetNameByHouseID(int houseId)
        {
            var query = "SELECT dbo.GetStreetNameByHouseID(@HouseID)";

            using (var connection = _context.CreateConnection())
            {
                var streetName = await connection.QueryFirstAsync<string>(query, new { HouseID = houseId });
                return streetName;
            }
        }
    }
}