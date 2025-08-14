using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Healper;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthMetricsController : ControllerBase
    {
        private readonly HealthMetricRepository _repository;
        private readonly IUserRepository _userRepo;

        public HealthMetricsController(HealthMetricRepository repository, IUserRepository userRepo)
        {
            _repository = repository;
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult GetAllMetrics()
        {
            // Optional: enable token validation
            // if (!TokenValidator.IsTokenValid(Request, _userRepo))
            //     return Unauthorized("Invalid or missing token");

            var metrics = _repository.GetAll();
            return Ok(metrics);
        }

        //  Get all health metrics for a specific machine
        [HttpGet("machine/{machineId}")]
        public IActionResult GetByMachine(int machineId)
        {
            //if (!TokenValidator.IsTokenValid(Request, _userRepo))
            //    return Unauthorized("Invalid or missing token");

            var metrics = _repository.GetByMachineId(machineId);
            return Ok(metrics);
        }

        [HttpPost]
        public IActionResult AddMetric([FromBody] HealthMetric metric)
        {
            //if (!TokenValidator.IsTokenValid(Request, _userRepo))
            //    return Unauthorized("Invalid or missing token");

            if (metric == null)
                return BadRequest("Invalid metric data.");

            _repository.Add(metric);
            return Ok(metric);
        }

        [HttpGet("critical")]
        public IActionResult GetCriticalMachines([FromQuery] float threshold = 80.0f)
        {
            //if (!TokenValidator.IsTokenValid(Request, _userRepo))
            //    return Unauthorized("Invalid or missing token");

            var criticalMachines = _repository.GetCriticalMachines(threshold);
            return Ok(criticalMachines);
        }

    }
}
