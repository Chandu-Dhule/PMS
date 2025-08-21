using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PMSCH.Server.Repositories;
using System.Collections.Generic;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TechnicianMachineAssignmentController : ControllerBase
    {
        private readonly TechnicianMachineAssignmentRepository _repository;

        public TechnicianMachineAssignmentController(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("DefaultConnection");
            _repository = new TechnicianMachineAssignmentRepository(connectionString);
        }

        [HttpGet("{username}")]
        public ActionResult<List<dynamic>> GetAssignmentsByUsername(string username)
        {
            var assignments = _repository.GetDetailedAssignmentsByUsername(username);
            if (assignments == null || assignments.Count == 0)
                return NotFound("No assignments found for the technician.");

            return Ok(assignments);
        }
    }
}
