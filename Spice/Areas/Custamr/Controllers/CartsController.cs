using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViweModel;
using Spice.Utilti;
using Stripe;

namespace Spice.Areas.Custamr.Controllers
{
    [Area("Custamr")]
    [Authorize]
    public class CartsController : Controller
    {
        private readonly ApplicationDbContext db;

        public CartsController(ApplicationDbContext db)
        {
            this.db = db;
        }

        [BindProperty]
        public OrdarDetailsCartViewModel OrdarDetailsCartVM { get; set; }

        public IActionResult Index()
        {
            OrdarDetailsCartVM = new OrdarDetailsCartViewModel()
            {
                OrdarHedar = new Models.OrdarHedar()
            };

            OrdarDetailsCartVM.OrdarHedar.OrdarTotal = 0;

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var shoppingCarts = db.ShoppingCarts.Where(m => m.ApplicationUserId == claim.Value);
            if (shoppingCarts != null)
            {
                OrdarDetailsCartVM.ShoppingCartList = shoppingCarts.ToList();
            }

            foreach (var item in OrdarDetailsCartVM.ShoppingCartList)
            {
                item.MenuItem = db.MenuItems.FirstOrDefault(m => m.Id == item.MenuItemId);
                OrdarDetailsCartVM.OrdarHedar.OrdarTotal += item.MenuItem.Price * item.Count;

                OrdarDetailsCartVM.OrdarHedar.OrdarTotal = Math.Round(OrdarDetailsCartVM.OrdarHedar.OrdarTotal, 2);



            }
            OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal = OrdarDetailsCartVM.OrdarHedar.OrdarTotal;

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                OrdarDetailsCartVM.OrdarHedar.CoupnCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var coponFromDB = db.Coupons.Where(m => m.Name.ToLower() == OrdarDetailsCartVM.OrdarHedar.CoupnCode.ToLower()).FirstOrDefault();
                OrdarDetailsCartVM.OrdarHedar.OrdarTotal = SD.DiscountPrice(coponFromDB, OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal);
            }


            return View(OrdarDetailsCartVM);
        }



       public IActionResult Summary()
        {
            OrdarDetailsCartVM = new OrdarDetailsCartViewModel()
            {
                OrdarHedar = new Models.OrdarHedar()
            };

            OrdarDetailsCartVM.OrdarHedar.OrdarTotal = 0;

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var appUser = db.ApplicationUsers.Find(claim.Value);
            OrdarDetailsCartVM.OrdarHedar.PickUpName = appUser.Name;
            OrdarDetailsCartVM.OrdarHedar.PickUpDate = DateTime.Now;
            OrdarDetailsCartVM.OrdarHedar.PhoneNumber = appUser.PhoneNumber;


            var shoppingCarts = db.ShoppingCarts.Where(m => m.ApplicationUserId == claim.Value);
            if (shoppingCarts != null)
            {
                OrdarDetailsCartVM.ShoppingCartList = shoppingCarts.ToList();
            }
            foreach (var item in OrdarDetailsCartVM.ShoppingCartList)
            {
                item.MenuItem = db.MenuItems.FirstOrDefault(m => m.Id == item.MenuItemId);
                OrdarDetailsCartVM.OrdarHedar.OrdarTotal += item.MenuItem.Price * item.Count;

                OrdarDetailsCartVM.OrdarHedar.OrdarTotal = Math.Round(OrdarDetailsCartVM.OrdarHedar.OrdarTotal, 2);



            }
            OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal = OrdarDetailsCartVM.OrdarHedar.OrdarTotal;

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                OrdarDetailsCartVM.OrdarHedar.CoupnCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var coponFromDB = db.Coupons.Where(m => m.Name.ToLower() == OrdarDetailsCartVM.OrdarHedar.CoupnCode.ToLower()).FirstOrDefault();
                OrdarDetailsCartVM.OrdarHedar.OrdarTotal = SD.DiscountPrice(coponFromDB, OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal);
            }


            return View(OrdarDetailsCartVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task< IActionResult> SummaryPost(string stripeToken)
        {
           
            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);



            OrdarDetailsCartVM.ShoppingCartList =await db.ShoppingCarts.Where(m => m.ApplicationUserId == claim.Value).ToListAsync();
        
            OrdarDetailsCartVM.OrdarHedar.PymentState = SD.PaymentStatusPending;
            OrdarDetailsCartVM.OrdarHedar.OrdarDate = DateTime.Now;
            OrdarDetailsCartVM.OrdarHedar.UserId= claim.Value;
            OrdarDetailsCartVM.OrdarHedar.State= SD.PaymentStatusPending;
            OrdarDetailsCartVM.OrdarHedar.PickUpTime= Convert.ToDateTime(OrdarDetailsCartVM.OrdarHedar.PickUpDate.ToShortDateString() + " " +
                OrdarDetailsCartVM.OrdarHedar.PickUpTime.ToShortTimeString());
          
            OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal = 0;
            
            db.OrdarHedars.Add(OrdarDetailsCartVM.OrdarHedar);
            await db.SaveChangesAsync();




            foreach (var item in OrdarDetailsCartVM.ShoppingCartList)
            {
                item.MenuItem = db.MenuItems.FirstOrDefault(m => m.Id == item.MenuItemId);

                OrdarDetails ordarDetails = new OrdarDetails()
                {
                    MenuItemId = item.MenuItemId,
                    OrdarId=OrdarDetailsCartVM.OrdarHedar.Id,
                    Discrption=item.MenuItem.Decrption,
                    Name=item.MenuItem.Name,
                    Pric=item.MenuItem.Price,
                    Count=item.Count
                };

                OrdarDetailsCartVM.OrdarHedar.OrdarTotal += item.MenuItem.Price * item.Count;
                db.OrdarDetail.Add(ordarDetails);


            }

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
            
                OrdarDetailsCartVM.OrdarHedar.CoupnCode = HttpContext.Session.GetString(SD.ssCouponCode);
               
                var coponFromDB = db.Coupons.Where(m => m.Name.ToLower() == OrdarDetailsCartVM.OrdarHedar.CoupnCode.ToLower()).FirstOrDefault();
                
                OrdarDetailsCartVM.OrdarHedar.OrdarTotal = SD.DiscountPrice(coponFromDB, OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal);
            }
            else
            {
                OrdarDetailsCartVM.OrdarHedar.OrdarTotal = Math.Round(OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal, 2);
            }
            OrdarDetailsCartVM.OrdarHedar.CoupnCodeDiscount = OrdarDetailsCartVM.OrdarHedar.OrdarTotalOregnal - OrdarDetailsCartVM.OrdarHedar.OrdarTotal;
            db.ShoppingCarts.RemoveRange(OrdarDetailsCartVM.ShoppingCartList);
            HttpContext.Session.SetInt32(SD.ShoppingCartCount, 0);
            await db.SaveChangesAsync();

            var option = new Stripe.ChargeCreateOptions
            {
                Amount=Convert.ToInt32(OrdarDetailsCartVM.OrdarHedar.OrdarTotal*100),
                Currency="usd",
                Description="Ordar ID :" + OrdarDetailsCartVM.OrdarHedar.Id,
                Source= stripeToken
            };

            //var service = new ChargeService();
            //Charge charge = service.Create(option);

            //if (charge.BalanceTransactionId == null)
            //{
            //    OrdarDetailsCartVM.OrdarHedar.PymentState = SD.PaymentStatusRejected;
            //}
            //else
            //{
            //    OrdarDetailsCartVM.OrdarHedar.TrasactionId = charge.BalanceTransactionId;
            //}
            //if (charge.Status.ToLower() == "succeeded")
            //{
            //    OrdarDetailsCartVM.OrdarHedar.PymentState = SD.PaymentStatusApproved;
            //    OrdarDetailsCartVM.OrdarHedar.State = SD.StatusSubitmed;
            //}
            //else
            //{
            //    OrdarDetailsCartVM.OrdarHedar.PymentState = SD.PaymentStatusRejected;
            //}

            //await db.SaveChangesAsync();

         //   return RedirectToAction("Index","Home");
            return RedirectToAction("Confirm","Ordars",new { id=OrdarDetailsCartVM.OrdarHedar.Id});
        }
            

        public IActionResult ApplyCoupon()
        {

            if (OrdarDetailsCartVM.OrdarHedar.CoupnCode == null)
            {
                OrdarDetailsCartVM.OrdarHedar.CoupnCode = "";
            }
            HttpContext.Session.SetString(SD.ssCouponCode, OrdarDetailsCartVM.OrdarHedar.CoupnCode);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult RemoveCoupon()
        {


            HttpContext.Session.SetString(SD.ssCouponCode, string.Empty);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var shoppingCart = await db.ShoppingCarts.FindAsync(cartId);
            shoppingCart.Count += 1;
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Minus(int cartId)
        {
            var shoppingCart = await db.ShoppingCarts.FindAsync(cartId);
            if (shoppingCart.Count > 1)
            {
                shoppingCart.Count -= 1;
                await db.SaveChangesAsync();
            }


            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Remove(int cartId)
        {
            var shoppingCart = await db.ShoppingCarts.FindAsync(cartId);

            db.ShoppingCarts.Remove(shoppingCart);

            await db.SaveChangesAsync();

            var count = db.ShoppingCarts.Where(m => m.ApplicationUserId == shoppingCart.ApplicationUserId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ShoppingCartCount, count);

            return RedirectToAction(nameof(Index));

        }
    }
    }
