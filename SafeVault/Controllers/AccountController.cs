using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SafeVault.Data;
using SafeVault.Helpers;
using SafeVault.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SafeVault.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            string allowedSpecialCharacters = "!@#$%^&*?";
            if (!ValidationHelpers.IsValidInput(username) ||
                !ValidationHelpers.IsValidInput(password, allowedSpecialCharacters))
            {
                ModelState.AddModelError("", "Ogiltig inmatning.");
                return View();
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            if (user != null)
            {
                var token = GenerateJwtToken(user.Username);
                return Ok(new { token }); // Du kan också returnera till en vy med token
            }

            ModelState.AddModelError("", "Felaktigt användarnamn eller lösenord.");
            return View();
        }

        private string GenerateJwtToken(string username)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("YourSuperSecretKey12345");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "User") // Du kan lägga till roller här
            }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
