

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

        // Insert into Logins table only
        _userRepo.CreateLogin(login);

        return Ok("User registered successfully");
    }


    [HttpPost("login")]
    public IActionResult Login([FromBody] Login login)
    {
        var storedLogin = _userRepo.GetLogin(login.User);
        if (storedLogin == null || !VerifyPassword(login.Pass, storedLogin.Pass))
            return Unauthorized("Invalid credentials");

     

       

        return Ok(new
        {
            Id = storedLogin.Id,
           
            role = storedLogin.Role,
            user = storedLogin.User
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

  
}
