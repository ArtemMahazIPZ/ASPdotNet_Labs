using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Lab05.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _users;
        private readonly IConfiguration _config;

        public AuthController(UserManager<IdentityUser> users, IConfiguration config)
        {
            _users = users;
            _config = config;
        }

        public record LoginDto(string Email, string Password);

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _users.FindByEmailAsync(dto.Email);
            if (user == null) return Unauthorized();

            var ok = await _users.CheckPasswordAsync(user, dto.Password);
            if (!ok) return Unauthorized();

            var token = await GenerateToken(user);
            return Ok(new { token });
        }

        private async Task<string> GenerateToken(IdentityUser user)
        {
            var key = _config["Jwt:Key"] ?? "SUPER_SECRET_KEY_12345";
            var issuer = _config["Jwt:Issuer"] ?? "Lab05.Api";

            var roles = await _users.GetRolesAsync(user);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email ?? user.UserName ?? ""),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: null,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
