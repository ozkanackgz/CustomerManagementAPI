using CustomerManagementAPI.Models;
using CustomerManagementAPI.Services; 
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CustomerManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService; 

        public AuthController(IConfiguration configuration, AuthService authService)
        {
            _configuration = configuration;
            _authService = authService; 
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] User user)
        {
            
            var authenticatedUser = _authService.Authenticate(user.Username, user.Password);

            if (authenticatedUser == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, authenticatedUser.Username),
                new Claim(ClaimTypes.Role, authenticatedUser.Role) 
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(_configuration["Jwt:ExpiryMinutes"])), 
                signingCredentials: creds
            );

            return Ok(new
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo 
            });
        }
    }
}