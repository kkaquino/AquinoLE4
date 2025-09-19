using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using BlogDataLibrary.Models;
using BlogDataLibrary.Data;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BlogAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SqlData _db;

        public LoginController(IConfiguration config, SqlData db)
        {
            _config = config;
            _db = db;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] UserLogin login)
        {
            var user = _db.Authenticate(login.UserName, login.Password).Result;
            if (user != null)
            {
                var token = GenerateToken(user);
                return Ok(new { Token = token });
            }
            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ActionResult Register([FromBody] UserModel user)
        {
            _db.Register(user.UserName, user.FirstName, user.LastName, user.Password);
            return Ok("User registered.");
        }

        private string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
