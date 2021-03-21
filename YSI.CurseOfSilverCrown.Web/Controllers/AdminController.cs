using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.BL.EndOfTurn;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EndOfTurnService _endOfTurnService;

        public AdminController(ApplicationDbContext context, EndOfTurnService endOfTurnService)
        {
            _context = context;
            _endOfTurnService = endOfTurnService;
        }        

        public async Task<IActionResult> NextTurn()
        {
            var succes = await _endOfTurnService.Execute();

            return RedirectToAction("Index", "Home");
        }
    }
}
