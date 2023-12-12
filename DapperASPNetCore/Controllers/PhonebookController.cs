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
	[Route("api/phonebook")]
	[ApiController]
	public class PhonebookController : ControllerBase
	{
		private readonly IPhonebookRepository _phonebookRepo;

		public PhonebookController(IPhonebookRepository phonebookRepo)
		{
            _phonebookRepo = phonebookRepo;
		}

        [HttpGet("list")]
        public async Task<IActionResult> GetPhonebook()
        {
            try
            {
                var phonebook = await _phonebookRepo.GetPhonebook();
                return Ok(phonebook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("listByCity")]
        public async Task<IActionResult> GetPhonebookByCity()
        {
            try
            {
                var phonebook = await _phonebookRepo.GetPhonebookByCity();
                return Ok(phonebook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("streetOwners/{cityId}")]
        public async Task<IActionResult> GetPhoneOwnersByCityAndStreet(int cityId)
        {
            try
            {
                var phonebook = await _phonebookRepo.GetPhoneOwnersByCityAndStreet(cityId);
                return Ok(phonebook);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("streetName/{houseId}")]
        public async Task<IActionResult> GetStreetNameByHouseID(int houseId)
        {
            try
            {
                var streetName = await _phonebookRepo.GetStreetNameByHouseID(houseId);
                return Ok(streetName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
