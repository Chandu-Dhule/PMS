using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;
using System.Linq;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineTypesController : ControllerBase
    {
        private readonly MachineTypeRepository _repository;
        private readonly IUserRepository _userRepo;

        public MachineTypesController(MachineTypeRepository repository, IUserRepository userRepo)
        {
            _repository = repository;
            _userRepo = userRepo;
        }

        private bool IsTokenValid()
        {
            var token = Request.Headers["X-Custom-Token"].FirstOrDefault();
            return !string.IsNullOrEmpty(token) && _userRepo.ValidateToken(token);
        }

        private bool IsAdmin()
        {
            var token = Request.Headers["X-Custom-Token"].FirstOrDefault();
            var user = _userRepo.GetUserByToken(token); // Ensure this method exists in IUserRepository
            return user != null && user.Role == "Admin";
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            return Ok(_repository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            var type = _repository.GetById(id);
            return type == null ? NotFound() : Ok(type);
        }

        [HttpPost]
        public IActionResult Create([FromBody] MachineType type)
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            if (!IsAdmin())
                return Forbid("Admin access required");

            _repository.Add(type);
            return CreatedAtAction(nameof(GetById), new { id = type.TypeID }, type);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            if (!IsAdmin())
                return Forbid("Admin access required");

            _repository.Delete(id);
            return NoContent();
        }
    }
}
