using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YSI.CurseOfSilverCrown.Web.Data;
using YSI.CurseOfSilverCrown.Web.Models.DbModels;

namespace YSI.CurseOfSilverCrown.Web.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        private string GetTurnName(int number)
        {
            var year = 587 + (number - 1) / 4;
            switch (number % 4)
            {
                case 1:
                    return $"{year} год - Зима";
                case 2:
                    return $"{year} год - Весна";
                case 3:
                    return $"{year} год - Лето";
                case 4:
                default:
                    return $"{year} год - Осень";
            }
        }

        public async Task<IActionResult> NextTurn()
        {
            var oldTurn = _context.Turns.
               Single(t => t.IsActive);
            oldTurn.IsActive = false;
            _context.Update(oldTurn);

            var newTurn = new Turn
            {
                IsActive = true,
                Started = DateTime.UtcNow,
                Name = GetTurnName(oldTurn.Id + 1)
            };
            _context.Add(newTurn);


            var organizations = _context.Organizations;
            foreach (var organization in organizations)
            {
                var command = new Command
                {
                    Id = Guid.NewGuid().ToString(),
                    OrganizationId = organization.Id,
                    Turn = newTurn,
                    Type = Enums.enCommandType.Idleness
                };
                _context.Add(command);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}
