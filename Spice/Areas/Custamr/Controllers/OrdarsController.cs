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
using Spice.Models.ViweModel;

namespace Spice.Areas.Custamr.Controllers
{
    [Area("Custamr")]
    public class OrdarsController : Controller
    {
        private readonly ApplicationDbContext db;

        public OrdarsController(ApplicationDbContext db)
        {
            this.db = db;
        }
        [Authorize]
        public async Task<IActionResult> Confirm(int id)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrdarDetailsViewModel ordarDetailsVM = new OrdarDetailsViewModel()
            {
                OrdarHedar = await db.OrdarHedars.Include(m => m.ApplicationUser).FirstOrDefaultAsync(m => m.UserId == claim.Value && m.Id == id),
                OrdarDetails = await db.OrdarDetail.Where(m => m.OrdarId == id).ToListAsync()
            };
            return View(ordarDetailsVM);
        }
        private int pageSize = 2;

        [Authorize]
        public async Task<IActionResult> OrdarHistory(int pageNumbar = 1)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            //List<OrdarDetailsViewModel> ordarDetailsVMList = new List<OrdarDetailsViewModel>();

            OrdarListViewModel ordarListVM = new OrdarListViewModel()
            {
                ordars = new List<OrdarDetailsViewModel>()
            };

            List<OrdarHedar> ordarHedarsList = await db.OrdarHedars.Include(m => m.ApplicationUser).Where(m => m.UserId == claim.Value).ToListAsync();
            foreach (var ordarHedars in ordarHedarsList)
            {
                OrdarDetailsViewModel ordarDetailsVM = new OrdarDetailsViewModel()
                {
                    OrdarHedar = ordarHedars,
                    OrdarDetails = await db.OrdarDetail.Where(m => m.OrdarId == m.OrdarHedar.Id).ToListAsync()

                };
                var count = ordarListVM.ordars.Count;

                ordarListVM.ordars = ordarListVM.ordars
                    .OrderByDescending(o => o.OrdarHedar.Id)
                    .Skip((pageNumbar - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                ordarListVM.PagingInfo = new PagingInfo()
                {
                    CurrentPage = pageNumbar,
                    RecordPerPage = pageSize,
                    TotalRecords = count,
                    urlParam = "/Custamr/Ordars/OrdarHistory?pageNumbar=:"
                };

                ordarListVM.ordars.Add(ordarDetailsVM);
            }


            return View(ordarListVM);
        }
        public async Task<IActionResult> GetOrdarDetails(int id)
        {
            OrdarDetailsViewModel ordarDetaisVM = new OrdarDetailsViewModel()
            {
                OrdarHedar = await db.OrdarHedars.Include(m => m.ApplicationUser).FirstOrDefaultAsync(),
                OrdarDetails = await db.OrdarDetail.Where(m => m.OrdarId == id).ToListAsync()
            };
            return PartialView("_indivisualOrdarDetails", ordarDetaisVM);
        }

    }
}
