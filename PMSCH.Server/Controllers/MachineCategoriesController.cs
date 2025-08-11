using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineCategoriesController : ControllerBase
    {
        private readonly MachineCategoryRepository _repository;

        public MachineCategoriesController(MachineCategoryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_repository.GetAll());

        [HttpPost]
        public IActionResult Create([FromBody] MachineCategory category)
        {
            _repository.Add(category);
            return Ok(category);
        }
    }

}
