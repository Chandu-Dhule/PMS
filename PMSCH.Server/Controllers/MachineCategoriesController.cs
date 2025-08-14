using Microsoft.AspNetCore.Mvc;

using PMSCH.Server.Healper;
using PMSCH.Server.Models;
using PMSCH.Server.Repositories;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MachineCategoriesController : ControllerBase
    {
        private readonly MachineCategoryRepository _repository;
        private readonly IUserRepository _userRepo;

        public MachineCategoriesController(MachineCategoryRepository repository, IUserRepository userRepo)
        {
            _repository = repository;
            _userRepo = userRepo;
        }


        [HttpGet]
        public IActionResult GetAll()
        {
            if (!TokenValidator.IsTokenValid(Request, _userRepo))
                return Unauthorized("Invalid or missing token");

            return Ok(_repository.GetAll());
        }

        [HttpPost]
        public IActionResult Create([FromBody] MachineCategory category)
        {
            if (!TokenValidator.IsTokenValid(Request, _userRepo))
                return Unauthorized("Invalid or missing token");

            _repository.Add(category);
            return Ok(category);
        }

    }

}
