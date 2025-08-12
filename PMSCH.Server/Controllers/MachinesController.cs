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

        public MachinesController(MachineRepository repository, TechnicianMachineAssignmentRepository assignmentRepository)
        {
            _repository = repository;
            _assignmentRepository = assignmentRepository;
        }

        // Get all machines
        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.GetAll());

        // Get machine by ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var machine = _repository.GetById(id);
            return machine == null ? NotFound() : Ok(machine);
        }

        // Create a new machine
        [HttpPost]
        public IActionResult Create([FromBody] Machine machine)
        {
            _repository.Add(machine);
            return CreatedAtAction(nameof(GetById), new { id = machine.MachineID }, machine);
        }

        // Update machine
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Machine machine)
        {
            _repository.Update(id, machine);
            return NoContent();
        }

        // Delete machine
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }

        // Assign machine to technician
        [HttpPost("assign")]
        public IActionResult AssignMachineToTechnician([FromQuery] int userId, [FromQuery] int machineId)
        {
            _assignmentRepository.AssignMachine(userId, machineId);
            return Ok(new { message = "Machine assigned successfully." });
        }

        // Unassign machine from technician
        [HttpDelete("unassign")]
        public IActionResult UnassignMachineFromTechnician([FromQuery] int userId, [FromQuery] int machineId)
        {
            _assignmentRepository.RemoveAssignment(userId, machineId);
            return Ok(new { message = "Machine unassigned successfully." });
        }

        // Get machines assigned to a technician
        [HttpGet("assigned/{userId}")]
        public IActionResult GetMachinesAssignedToTechnician(int userId)
        {
            var machineIds = _assignmentRepository.GetAssignedMachines(userId);
            var machines = _repository.GetAll().Where(m => machineIds.Contains(m.MachineID)).ToList();
            return Ok(machines);
        }
    }
}
