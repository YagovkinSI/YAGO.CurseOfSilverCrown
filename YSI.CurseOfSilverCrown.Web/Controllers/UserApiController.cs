using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Core.Database;
using YSI.CurseOfSilverCrown.Core.Database.Users;
using YSI.CurseOfSilverCrown.Core.Helpers;
using YSI.CurseOfSilverCrown.Web.ApiModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<UserApiController> _logger;

        public UserApiController(ApplicationDbContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<UserApiController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        [Route("getCurrentUser")]
        public async Task<ActionResult<UserPublicData>> GetCurrentUser()
        {
            try
            {
                var user = await UserHelper.GetCurrentUser(_userManager, HttpContext.User, _context);
                if (user == null) 
                {
                    return Ok(null);
                }
                var userPubliCdata = new UserPublicData(user);
                return Ok(userPubliCdata);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<ActionResult<UserPublicData>> Register(RegisterRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await UserHelper.RegisterAsync(_userManager, _signInManager, request.UserName, request.Password);
                    if (response.Success)
                    {
                        _logger.LogInformation($"Created user: id - {response.Result.Id}, userName - {response.Result.UserName}");
                        var userPubliCdata = new UserPublicData(response.Result);
                        return Ok(userPubliCdata);
                    }
                    else
                    {
                        return BadRequest(response.Error);
                    }
                }

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
        public async Task<ActionResult<UserPublicData>> Login(LoginRequest request)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = await UserHelper.LoginAsync(_context, _signInManager, request.UserName, request.Password);
                    if (response.Success)
                    {
                        _logger.LogInformation($"Logined user: id - {response.Result.Id}, userName - {response.Result.UserName}");
                        var userPubliCdata = new UserPublicData(response.Result);
                        return Ok(userPubliCdata);
                    }
                    else
                    {
                        return BadRequest(response.Error);
                    }
                }

                var stateErrors = ModelState.SelectMany(s => s.Value.Errors.Select(e => e.ErrorMessage));
                return BadRequest(string.Join(". ", stateErrors));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                var response = await UserHelper.SignOutAsync(_context, _userManager, _signInManager, HttpContext.User);
                if (response.Success)
                {
                    if (response.Result != null)
                        _logger.LogInformation($"Logout user: id - {response.Result.Id}, userName - {response.Result.UserName}");
                    return Ok();
                }
                else
                {
                    return BadRequest(response.Error);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
