using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;
using System.Linq;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachinesController : ControllerBase
    {
        private readonly MachineRepository _repository;
        private readonly TechnicianMachineAssignmentRepository _assignmentRepository;
        private readonly IUserRepository _userRepo;

        public MachinesController(
            MachineRepository repository,
            TechnicianMachineAssignmentRepository assignmentRepository,
            IUserRepository userRepo)
        {
            _repository = repository;
            _assignmentRepository = assignmentRepository;
            _userRepo = userRepo;
        }

        private string GetToken() => Request.Headers["X-Custom-Token"].FirstOrDefault();

        private bool IsTokenValid()
        {
            var token = GetToken();
            return !string.IsNullOrEmpty(token) && _userRepo.ValidateToken(token);
        }

        private bool IsAdmin()
        {
            var token = GetToken();
            var user = _userRepo.GetUserByToken(token);
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

            var machine = _repository.GetById(id);
            return machine == null ? NotFound() : Ok(machine);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Machine machine)
        {
            if (!IsTokenValid())
                return Unauthorized("Invalid or missing token");

            if (!IsAdmin())
                return Forbid("Admin access required");

            _repository.Add(machine);
            return CreatedAtAction(nameof(GetById), new { id = machine.MachineID }, machine);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Machine machine)
        {
            if (!IsTokenValid())
                return Unauthorized("Invalid or missing token");

            if (!IsAdmin())
                return Forbid("Admin access required");

            _repository.Update(id, machine);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            if (!IsTokenValid())
                return Unauthorized("Invalid or missing token");

            if (!IsAdmin())
                return Forbid("Admin access required");

            _repository.Delete(id);
            return NoContent();
        }

        [HttpPost("assign")]
        public IActionResult AssignMachineToTechnician([FromQuery] int userId, [FromQuery] int machineId)
        {
            if (!IsTokenValid())
                return Unauthorized("Invalid or missing token");

            if (!IsAdmin())
                return Forbid("Admin access required");

            _assignmentRepository.AssignMachine(userId, machineId);
            return Ok(new { message = "Machine assigned successfully." });
        }

        [HttpDelete("unassign")]
        public IActionResult UnassignMachineFromTechnician([FromQuery] int userId, [FromQuery] int machineId)
        {
            if (!IsTokenValid())
                return Unauthorized("Invalid or missing token");

            if (!IsAdmin())
                return Forbid("Admin access required");

            _assignmentRepository.RemoveAssignment(userId, machineId);
            return Ok(new { message = "Machine unassigned successfully." });
        }

        [HttpGet("assigned/{userId}")]
        public IActionResult GetMachinesAssignedToTechnician(int userId)
        {
            if (!IsTokenValid())
                return Unauthorized("Invalid or missing token");

            var machineIds = _assignmentRepository.GetAssignedMachines(userId);
            var machines = _repository.GetAll().Where(m => machineIds.Contains(m.MachineID)).ToList();
            return Ok(machines);
        }
    }
}
