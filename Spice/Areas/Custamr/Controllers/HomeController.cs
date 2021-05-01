using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViweModel;
using Spice.Utilti;

namespace Spice.Areas.Custamr.Controllers
{
    [Area("Custamr")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            IndexViweModel indexViweModel = new IndexViweModel()
            {
                Categorys = await context.Categories.ToListAsync(),
                Coupons = await context.Coupons.ToListAsync(),
                MenuItems=await context.MenuItems.Include(m=>m.Category).Include(m=>m.SubCategory).ToListAsync()

            };
            return View(indexViweModel);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Details(int itemid)
        {
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim!=null)
            {


                List<ShoppingCart> shoppingCarts = await context.ShoppingCarts.Where(m => m.ApplicationUserId == claim.Value).ToListAsync();
                HttpContext.Session.SetInt32(SD.ShoppingCartCount, shoppingCarts.Count);
            }
            var menuItem = await context.MenuItems.Include(m => m.Category)
                .Include(m => m.SubCategory).Where(m=>m.Id== itemid).FirstOrDefaultAsync();

            ShoppingCart shoppingCart = new ShoppingCart()
            {
                MenuItem = menuItem,
                MenuItemId = menuItem.Id

            };

            return View(shoppingCart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
               
                var claimsIdentity = (ClaimsIdentity)User.Identity;

                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                shoppingCart.ApplicationUserId = claim.Value;

               
               var shoppingCartFormDB =await context.ShoppingCarts.Where(m => m.ApplicationUserId == shoppingCart.ApplicationUserId && m.MenuItemId == shoppingCart.MenuItemId).FirstOrDefaultAsync();
               
                if (shoppingCartFormDB == null)
                {
                    context.ShoppingCarts.Add(shoppingCart);
                }
                else
                {
                    shoppingCartFormDB.Count += shoppingCart.Count;
                }
                 await context.SaveChangesAsync();

                var count = context.ShoppingCarts
                    .Where(m => m.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count;
                     HttpContext.Session.SetInt32(SD.ShoppingCartCount, count);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                var menuItem = await context.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).Where(m => m.Id == shoppingCart.MenuItemId).FirstOrDefaultAsync();

                ShoppingCart shoppingCartobj = new ShoppingCart()
                {
                    MenuItem = menuItem,
                    MenuItemId = menuItem.Id

                };

                return View(shoppingCartobj);
            }

        }
    }
}
