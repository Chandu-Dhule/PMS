using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaintenanceLogsController : ControllerBase
    {
        private readonly MaintenanceLogRepository _repository;

        public MaintenanceLogsController(MaintenanceLogRepository repository)
        {
            _repository = repository;
        }

        //  Get logs by machine ID
        [HttpGet("machine/{machineId}")]
        public IActionResult GetLogsByMachine(int machineId)
        {
            var logs = _repository.GetByMachineId(machineId);
            return Ok(logs);
        }

        //  Add a new maintenance log
        [HttpPost]
        public IActionResult LogMaintenance([FromBody] MaintenanceLog log)
        {
            if (log == null)
                return BadRequest("Invalid log data.");

            _repository.Add(log);
            return Ok(log);
        }

        
        [HttpGet("due")]
        public IActionResult GetMachinesDueForMaintenance()
        {
            var dueMachines = _repository.GetMachinesDue();
            return Ok(dueMachines);
        }
        
    }
}
