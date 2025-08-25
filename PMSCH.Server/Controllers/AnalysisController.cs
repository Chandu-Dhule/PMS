using Microsoft.AspNetCore.Mvc;
using PMSCH.Server.Repositories;
using PMSCH.Server.Models;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace PMSCH.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalysisController : ControllerBase
    {
        private readonly IAnalysisRepository _analysisRepo;
        private readonly IUserRepository _userRepo;

        public AnalysisController(IAnalysisRepository analysisRepo, IUserRepository userRepo)
        {
            _analysisRepo = analysisRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public IActionResult GetAnalysis(int Id,string Role)
        {
            //var user = GetCurrentUser();
            //if (user == null)
            //    return Unauthorized("User not found");

            try
            {
                var data = _analysisRepo.GetAnalysisData(Id,Role);
                return Ok(data);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
        }

        //private User GetCurrentUser()
        //{
        //    var token = Request.Headers["Authorization"].FirstOrDefault()?.Replace("Bearer ", "");
        //    return token != null ? _userRepo.GetUserByToken(token) : null;
        //}
    }
}