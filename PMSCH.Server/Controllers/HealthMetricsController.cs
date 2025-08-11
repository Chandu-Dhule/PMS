using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthMetricsController : ControllerBase
    {
        private readonly HealthMetricRepository _repository;

        public HealthMetricsController(HealthMetricRepository repository)
        {
            _repository = repository;
        }

        //  Get all health metrics for a specific machine
        [HttpGet("machine/{machineId}")]
        public IActionResult GetByMachine(int machineId)
        {
            var metrics = _repository.GetByMachineId(machineId);
            return Ok(metrics);
        }

        //  Adding  a new health metric
        [HttpPost]
        public IActionResult AddMetric([FromBody] HealthMetric metric)
        {
            if (metric == null)
                return BadRequest("Invalid metric data.");

            _repository.Add(metric);
            return Ok(metric);
        }

        //  machines with critical temperature
        [HttpGet("critical")]
        public IActionResult GetCriticalMachines([FromQuery] float threshold = 80.0f)
        {
            var criticalMachines = _repository.GetCriticalMachines(threshold);
            return Ok(criticalMachines);
        }
    }
}
