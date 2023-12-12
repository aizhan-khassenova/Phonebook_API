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
	[Route("api/apartment")]
	[ApiController]
	public class ApartmentController : ControllerBase
	{
		private readonly IApartmentRepository _apartmentRepo;

		public ApartmentController(IApartmentRepository apartmentRepo)
		{
            _apartmentRepo = apartmentRepo;
		}

        [HttpGet]
        public async Task<IActionResult> GetApartment()
        {
            try
            {
                var apartments = await _apartmentRepo.GetApartment();
                return Ok(apartments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{houseId}")]
        public async Task<IActionResult> CreateApartment(int houseId, ApartmentForCreate apartment)
        {
            try
            {
                var createdApartment = await _apartmentRepo.CreateApartment(houseId, apartment);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{apartmentId}")]
        public async Task<IActionResult> DeleteApartment(int apartmentId)
        {
            try
            {
                await _apartmentRepo.DeleteApartment(apartmentId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
