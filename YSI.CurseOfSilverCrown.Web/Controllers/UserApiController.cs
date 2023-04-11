using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;
using YSI.CurseOfSilverCrown.Web.ApiModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<bool>> Register(RegisterRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                    return StatusCode(500, "Регистрация ещё не реализована");

                var stateErrors = ModelState.SelectMany(s => s.Value.Errors.Select(e => e.ErrorMessage));
                return BadRequest(string.Join(". ", stateErrors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<bool>> Login(LoginRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                    return StatusCode(500, "Вход ещё не реализован");

                var stateErrors = ModelState.SelectMany(s => s.Value.Errors.Select(e => e.ErrorMessage));
                return BadRequest(string.Join(". ", stateErrors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
