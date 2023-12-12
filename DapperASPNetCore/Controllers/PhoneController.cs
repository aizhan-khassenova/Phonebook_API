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
	[Route("api/phone")]
	[ApiController]
	public class PhoneController : ControllerBase
	{
		private readonly IPhoneRepository _phoneRepo;

		public PhoneController(IPhoneRepository phoneRepo)
		{
            _phoneRepo = phoneRepo;
		}

        [HttpGet]
        public async Task<IActionResult> GetPhone()
        {
            try
            {
                var phones = await _phoneRepo.GetPhone();
                return Ok(phones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("{apartmentId}")]
        public async Task<IActionResult> CreatePhone(int apartmentId, PhoneForCreate phone)
        {
            try
            {
                var createdPhone = await _phoneRepo.CreatePhone(apartmentId, phone);
                return StatusCode(201);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{phoneId}")]
        public async Task<IActionResult> UpdatePhone(int phoneId, PhoneForUpdate phone)
        {
            try
            {
                var updatedPhone = await _phoneRepo.UpdatePhone(phoneId, phone);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{phoneId}")]
        public async Task<IActionResult> DeletePhone(int phoneId)
        {
            try
            {
                await _phoneRepo.DeletePhone(phoneId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
