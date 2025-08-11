using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachinesController : ControllerBase
    {
        private readonly MachineRepository _repository;

        public MachinesController(MachineRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.GetAll());

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var machine = _repository.GetById(id);
            return machine == null ? NotFound() : Ok(machine);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Machine machine)
        {
            _repository.Add(machine);
            return CreatedAtAction(nameof(GetById), new { id = machine.MachineID }, machine);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Machine machine)
        {
            _repository.Update(id, machine);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _repository.Delete(id);
            return NoContent();
        }
    }
}
