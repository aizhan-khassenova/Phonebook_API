using Dapper;
using DapperASPNetCore.Context;
using DapperASPNetCore.Entities;
using DapperASPNetCore.IdentityAuth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
//using System.Web.Http;

namespace DapperASPNetCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DapperContext _context;

        public AuthController(IConfiguration configuration, DapperContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginData login)
        {
            // Проверка логина и пароля. Замените на свою логику.
            if (IsValidUser(login.Username, login.Password))
            {
                var token = GenerateJwtToken(login.Username);
                return Ok(new { Token = token, Message = "Успешный вход" });
            }

            return Unauthorized(new { Message = "Неверный логин или пароль" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { Message = "Успешный выход" });
        }

        private bool IsValidUser(string username, string password)
        {
            using (var connection = _context.CreateConnection())
            {
                connection.Open();

                // Предположим, у вас есть таблица LoginData с колонками Username и Password
                var query = "SELECT COUNT(*) FROM LoginData WHERE Username = @Username AND Password = @Password";
                var parameters = new { Username = username, Password = password };

                var result = connection.QuerySingleOrDefault<int>(query, parameters);

                return result > 0;
            }
        }

        private string GenerateJwtToken(string username)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey256"])); // Используйте ключ размером 256 бит
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                new Claim[] { new Claim(ClaimTypes.Name, username) },
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
