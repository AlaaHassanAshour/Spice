using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utilti;

namespace Spice.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize (Roles =SD.ManegerUser)]
    public class Users : Controller
    {
        private readonly ApplicationDbContext _context;

        public Users(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task <IActionResult> Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var clims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            string userId = clims.Value;
            return View(await _context.ApplicationUsers.Where(m => m.Id != userId).ToListAsync());
        }
        public async Task<IActionResult> LoukUnLock(string? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user==null)
            {
                return NotFound();
            }
            if (user.LockoutEnd==null||user.LockoutEnd<DateTime.Now)
            {
                user.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            else
            {
                user.LockoutEnd = DateTime.Now;
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
   
}
