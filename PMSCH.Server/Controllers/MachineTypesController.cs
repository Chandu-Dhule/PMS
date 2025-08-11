using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineTypesController : ControllerBase
    {
        private readonly MachineTypeRepository _repository;

        public MachineTypesController(MachineTypeRepository repository)
        {
            _repository = repository;
        }

        //  Get all machine types
        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.GetAll());

        // Get machine type by ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var type = _repository.GetById(id);
            return type == null ? NotFound() : Ok(type);
        }

        // Create a new machine type
        [HttpPost]
        public IActionResult Create([FromBody] MachineType type)
        {
            _repository.Add(type);
            return CreatedAtAction(nameof(GetById), new { id = type.TypeID }, type);
        }

        // Delete a machine type
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
