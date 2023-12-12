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
	[Route("api/house")]
	[ApiController]
	public class HouseController : ControllerBase
	{
		private readonly IHouseRepository _houseRepo;

		public HouseController(IHouseRepository houseRepo)
		{
            _houseRepo = houseRepo;
		}

        [HttpGet]
        public async Task<IActionResult> GetHouse()
        {
            try
            {
                var houses = await _houseRepo.GetHouse();
                return Ok(houses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{streetId}")]
        public async Task<IActionResult> CreateHouse(int streetId, HouseForCreate house)
        {
            try
            {
                var createdHouse = await _houseRepo.CreateHouse(streetId, house);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{houseId}")]
        public async Task<IActionResult> DeleteHouse(int houseId)
        {
            try
            {
                await _houseRepo.DeleteHouse(houseId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
