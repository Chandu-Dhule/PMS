
/*using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;
using System.Linq;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserRepository _userRepo;

        public LoginController(UserRepository userRepo)
        {
            _userRepo = userRepo;
        }
        [HttpGet]
        public IActionResult GetLogin()
        {

            return Ok(_userRepo.GetAll());
        }




        [HttpPost]
        public IActionResult CreateLogin([FromBody] Login login)
        {
            if (_userRepo.GetLogin(login.User) != null)
                return Conflict("User already exists");

            _userRepo.CreateLogin(login);
            return CreatedAtAction(nameof(GetLogin), new { user = login.User }, login);
        }
    }
}
*/

using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;
using System.Security.Cryptography;
using System.Text;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly UserRepository _userRepo;

    public LoginController(UserRepository userRepo)
    {
        _userRepo = userRepo;
    }

    [HttpPost("register")]
    public IActionResult Register([FromBody] Login login)
    {
        if (_userRepo.GetLogin(login.User) != null)
            return Conflict("User already exists");

        // Hash password
        login.Pass = HashPassword(login.Pass);

        // Insert into Logins table
        _userRepo.CreateLogin(login);

        // Insert into Users table
        var user = new User
        {
            Username = login.User,
            PasswordHash = login.Pass,
            Role = login.Role,
            CategoryID = null
        };
        _userRepo.AddUser(user);

        return Ok("User registered successfully");
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] Login login)
    {
        var user = _userRepo.GetLogin(login.User);
        if (user == null || !VerifyPassword(login.Pass, user.Pass))
            return Unauthorized("Invalid credentials");

        var token = GenerateToken();
        var expiry = DateTime.UtcNow.AddHours(1);

        var userEntity = _userRepo.GetUserByUsername(login.User);
        if (userEntity == null)
            return BadRequest("User not found in Users table.");

        //_userRepo.SaveToken(userEntity.Id, token, expiry);

        return Ok(new
        {   
            Id= userEntity.Id,
            token = token,
            expiry = expiry,
            role = user.Role,
            user = user.User
        });
    }

    [HttpPost("logout")]
    public IActionResult Logout([FromBody] string token)
    {
        _userRepo.DeleteToken(token);
        return Ok("Logged out successfully");
    }

    private string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(bytes);
    }

    private bool VerifyPassword(string inputPassword, string storedHash)
    {
        var inputHash = HashPassword(inputPassword);
        return inputHash == storedHash;
    }

    private string GenerateToken()
    {
        var bytes = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }
}
