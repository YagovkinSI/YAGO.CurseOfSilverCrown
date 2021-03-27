using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly EndOfTurnService _endOfTurnService;

        public AdminController(EndOfTurnService endOfTurnService)
        {
            _endOfTurnService = endOfTurnService;
        }        

        public async Task<IActionResult> NextTurn()
        {
            await _endOfTurnService.Execute();

            return RedirectToAction("Index", "Home");
        }
    }
}
