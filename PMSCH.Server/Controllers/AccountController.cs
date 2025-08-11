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
            var user = _userRepo.GetUserByUsername(request.Username);
            if (user == null || !PasswordHelper.VerifyPassword(request.Password, user.PasswordHash))
                return Unauthorized("Invalid credentials");

            var token = Guid.NewGuid().ToString();
            var expiry = DateTime.Now.AddHours(2);

            _userRepo.SaveToken(user.Id, token, expiry);

            return Ok(new
            {
                token,
                role = user.Role,
                category = user.Category
            });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            var token = Request.Headers["X-Custom-Token"].FirstOrDefault();
            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is missing");

            _userRepo.DeleteToken(token);
            return Ok("Logged out successfully");
        }
    }
}
