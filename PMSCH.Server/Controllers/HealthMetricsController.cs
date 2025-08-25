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

        
        [HttpGet("metrics/by-role")]
        public IActionResult GetMetricsByRoleAndId([FromQuery] string role, [FromQuery] int userId)
        {
            if (string.IsNullOrWhiteSpace(role))
                return BadRequest("Role is required.");

            List<HealthMetric> metrics;

            try
            {
                switch (role.Trim().ToLower())
                {
                    case "technician":
                        metrics = _repository.GetHealthMetricsByTechnician(userId);
                        break;

                    case "manager":
                        metrics = _repository.GetHealthMetricsByManager(userId);
                        break;

                    case "admin":
                        metrics = _repository.GetAll();
                        break;

                    default:
                        return BadRequest("Invalid role. Valid roles are: Technician, Manager, Admin.");
                }

                return Ok(metrics);
            }
            catch (Exception ex)
            {
                // Optional: log the exception
                return StatusCode(500, $"An error occurred while fetching metrics: {ex.Message}");
            }
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

        [HttpGet("critical-metrics")]
        public IActionResult GetCriticalMachinesByRole([FromQuery] string role, [FromQuery] int userId, [FromQuery] float threshold = 80.0f, [FromQuery] float Energy=100.0f)
        {
            List<HealthMetric> metrics;

            switch (role.Trim().ToLower())
            {
                case "technician":
                    metrics = _repository.GetCriticalMachinesByTechnician(userId, threshold,Energy);
                    break;
                case "manager":
                    metrics = _repository.GetCriticalMachinesByManager(userId, threshold, Energy);
                    break;
                case "admin":
                    metrics = _repository.GetCriticalMachines(threshold,Energy);
                    break;
                default:
                    return BadRequest("Invalid role. Valid roles are: Technician, Manager, Admin.");
            }

            return Ok(metrics);
        }


    }
}
