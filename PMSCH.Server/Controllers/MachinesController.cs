using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Healper;
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

        //private string GetToken() => Request.Headers["X-Custom-Token"].FirstOrDefault();

        //private bool IsTokenValid()
        //{
        //    var token = GetToken();
        //    return !string.IsNullOrEmpty(token) && _userRepo.ValidateToken(token);
        //}

        //private bool IsAdmin()
        //{
        //    var token = GetToken();
        //    var user = _userRepo.GetUserByToken(token);
        //    return user != null && user.Role == "Admin";
        //}

        [HttpGet("machines/by-role")]
        public IActionResult GetMachinesByRole([FromQuery] string role, [FromQuery] int userId)
        {
            if (string.IsNullOrWhiteSpace(role))
                return BadRequest("Role is required.");

            List<Machine> machines;

            try
            {
                switch (role.Trim().ToLower())
                {
                    case "technician":
                        machines = _repository.GetMachinesByTechnician(userId);
                        break;

                    case "manager":
                        machines = _repository.GetMachinesByManager(userId);
                        break;

                    case "admin":
                        machines = _repository.GetAll();
                        break;

                    default:
                        return BadRequest("Invalid role. Valid roles are: Technician, Manager, Admin.");
                }

                return Ok(machines);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while fetching machine data: {ex.Message}");
            }
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
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            //if (!IsAdmin())
            //    return Forbid("Admin access required");

            _repository.Add(machine);
            return CreatedAtAction(nameof(GetById), new { id = machine.MachineID }, machine);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Machine machine)
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            //if (!IsAdmin())
            //    return Forbid("Admin access required");

            _repository.Update(id, machine);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(id);
                return NoContent(); // 204 if successful
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // 404 if machine not found
            }
        }


        [HttpPost("assign")]
        public IActionResult AssignMachineToTechnician([FromQuery] int userId, [FromQuery] int machineId)
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            //if (!IsAdmin())
            //    return Forbid("Admin access required");

            _assignmentRepository.AssignMachine(userId, machineId);
            return Ok(new { message = "Machine assigned successfully." });
        }

        [HttpDelete("unassign")]
        public IActionResult UnassignMachineFromTechnician([FromQuery] int userId, [FromQuery] int machineId)
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            //if (!IsAdmin())
            //    return Forbid("Admin access required");




            _assignmentRepository.RemoveAssignment(userId, machineId);
            return Ok(new { message = "Machine unassigned successfully." });
        }

        [HttpGet("assigned/{userId}")]
        public IActionResult GetMachinesAssignedToTechnician(int userId)
        {
            //if (!IsTokenValid())
            //    return Unauthorized("Invalid or missing token");

            var machineIds = _assignmentRepository.GetAssignedMachines(userId);
            var machines = _repository.GetAll().Where(m => machineIds.Contains(m.MachineID)).ToList();
            return Ok(machines);
        }
    }
}
