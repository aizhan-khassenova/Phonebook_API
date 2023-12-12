using DapperASPNetCore.Contracts;
using DapperASPNetCore.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperASPNetCore.Controllers
{
	[Route("api/street")]
	[ApiController]
	public class StreetController : ControllerBase
	{
		private readonly IStreetRepository _streetRepo;

		public StreetController(IStreetRepository streetRepo)
		{
            _streetRepo = streetRepo;
		}

        [HttpGet]
        public async Task<IActionResult> GetStreet()
        {
            try
            {
                var streets = await _streetRepo.GetStreet();
                return Ok(streets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{cityId}")]
        public async Task<IActionResult> CreateStreet(int cityId, StreetForCreate street)
        {
            try
            {
                var createdStreet = await _streetRepo.CreateStreet(cityId, street);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{streetId}")]
        public async Task<IActionResult> UpdateStreet(int streetId, StreetForUpdate street)
        {
            try
            {
                var updatedStreet = await _streetRepo.UpdateStreet(streetId, street);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{streetId}")]
        public async Task<IActionResult> DeleteCity(int streetId)
        {
            try
            {
                await _streetRepo.DeleteStreet(streetId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
