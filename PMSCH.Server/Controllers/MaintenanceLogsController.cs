using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;
using System.Linq;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceLogsController : ControllerBase
    {
        private readonly MaintenanceLogRepository _repository;
        private readonly IUserRepository _userRepo;

        public MaintenanceLogsController(MaintenanceLogRepository repository, IUserRepository userRepo)
        {
            _repository = repository;
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

        // ✅ Get all maintenance logs
        [HttpGet]
        public IActionResult GetAllLogs()
        {
            // Optional: enable token validation
            // if (!IsTokenValid()) return Unauthorized("Invalid or missing token");

            var logs = _repository.GetAll();
            return Ok(logs);
        }

        // ✅ Get logs by machine ID
        [HttpGet("machine/{machineId}")]
        public IActionResult GetLogsByMachine(int machineId)
        {
            // if (!IsTokenValid()) return Unauthorized("Invalid or missing token");

            var logs = _repository.GetByMachineId(machineId);
            return Ok(logs);
        }

        // ✅ Add a new maintenance log
        [HttpPost]
        public IActionResult LogMaintenance([FromBody] MaintenanceLog log)
        {
            // if (!IsTokenValid()) return Unauthorized("Invalid or missing token");
            // if (!IsAdmin()) return Forbid("Admin access required");

            if (log == null)
                return BadRequest("Invalid log data.");

            _repository.Add(log);
            return Ok(log);
        }

        // ✅ Get machines due for maintenance
        [HttpGet("due")]
        public IActionResult GetMachinesDueForMaintenance()
        {
            // if (!IsTokenValid()) return Unauthorized("Invalid or missing token");
            // if (!IsAdmin()) return Forbid("Admin access required");

            var dueMachines = _repository.GetMachinesDue();
            return Ok(dueMachines);
        }
    }
}
