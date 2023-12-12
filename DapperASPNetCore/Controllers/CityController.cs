using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperASPNetCore.Controllers
{
	[Route("api/city")]
	[ApiController]
	public class CityController : ControllerBase
	{
		private readonly ICityRepository _cityRepo;

        public CityController(ICityRepository cityRepo)
		{
			_cityRepo = cityRepo;
		}

        [HttpGet]
        public async Task<IActionResult> GetCity()
        {
            try
            {
                var cities = await _cityRepo.GetCity();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateCity(CityForCreate city)
        {
            try
            {
                var createdCity = await _cityRepo.CreateCity(city);
                return StatusCode(201);
            }
            catch (Exception ex) 
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{cityId}")]
        public async Task<IActionResult> UpdateCity(int cityId, CityForUpdate city)
        {
            try
            {
                var updatedCity = await _cityRepo.UpdateCity(cityId, city);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCity(int cityId)
        {
            try
            {
                await _cityRepo.DeleteCity(cityId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
