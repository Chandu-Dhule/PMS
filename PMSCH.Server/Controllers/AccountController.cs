using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Helpers;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;
using System;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public AccountController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Username and password are required." });

            var user = _userRepo.GetUserByUsername(request.Username);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid credentials" });

            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.UtcNow.AddHours(2);

            _userRepo.SaveToken(user.Id, token, expiry);

            return Ok(new
            {
                token,
                role = user.Role,
                category = user.CategoryID
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout([FromHeader(Name = "X-Custom-Token")] string token)
        {
            //if (string.IsNullOrWhiteSpace(token))
            //    return BadRequest(new { message = "Token is missing" });

            //if (!_userRepo.ValidateToken(token))
            //    return Unauthorized(new { message = "Invalid or expired token" });

            _userRepo.DeleteToken(token);
            return Ok(new { message = "Logged out successfully" });
        }

        [HttpPost("create-user")]
        public IActionResult CreateUser([FromHeader(Name = "X-Custom-Token")] string token, [FromBody] User newUser)
        {
            //if (string.IsNullOrWhiteSpace(token))
            //    return BadRequest(new { message = "Token is missing" });

            //if (!_userRepo.ValidateToken(token))
            //    return Unauthorized(new { message = "Invalid or expired token" });

            var currentUser = _userRepo.GetUserByToken(token);
            if (currentUser == null || currentUser.Role != "Admin")
                return Forbid();

            if (string.IsNullOrWhiteSpace(newUser.Username) || string.IsNullOrWhiteSpace(newUser.PasswordHash))
                return BadRequest(new { message = "Username and password are required." });

            if (_userRepo.GetUserByUsername(newUser.Username) != null)
                return Conflict(new { message = "Username already exists." });

            newUser.PasswordHash = PasswordHelper.HashPassword(newUser.PasswordHash);
            _userRepo.AddUser(newUser);

            return Ok(new { message = "User created successfully." });
        }

        [HttpGet("me")]
        public IActionResult GetCurrentUser([FromHeader(Name = "X-Custom-Token")] string token)
        {
            //if (string.IsNullOrWhiteSpace(token))
            //    return BadRequest(new { message = "Token is missing" });

            //if (!_userRepo.ValidateToken(token))
            //    return Unauthorized(new { message = "Invalid or expired token" });

            var user = _userRepo.GetUserByToken(token);
            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                username = user.Username,
                role = user.Role,
                category = user.CategoryID
            });
        }
    }
}
