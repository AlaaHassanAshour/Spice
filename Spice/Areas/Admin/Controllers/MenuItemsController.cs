using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.ViweModel;
using Spice.Utilti;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManegerUser)]

    [Area("Admin")]
    public class MenuItemsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment webHost;

        [BindProperty]
        public MeunItemViewModel MeunItemVM { get; set; }

        public MenuItemsController(ApplicationDbContext context, IWebHostEnvironment webHost)
        {
            _context = context;
            this.webHost = webHost;
            MeunItemVM = new MeunItemViewModel()
            {
                MenuItem = new MenuItem(),
                CategoriesList = _context.Categories.ToList()

            };
        }

        // GET: Admin/MenuItems
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.MenuItems.Include(m => m.Category).Include(m => m.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/MenuItems/Details/5
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuItem = await _context.MenuItems
        //        .Include(m => m.Category)
        //        .Include(m => m.SubCategory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (menuItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuItem);
        //}

        // GET: Admin/MenuItems/Create
        [HttpGet]
        public IActionResult Create()
        {
            //  ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            // ViewData["SubCategoryId"] = new SelectList(_context.SubCategories, "Id", "Name");
            return View(MeunItemVM);
        }

        // POST: Admin/MenuItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost()
        {
            if (ModelState.IsValid)
            {
                string imegPath = @"\Images\default.jpg";
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {   
                    string webRootPath = webHost.WebRootPath;
                    string ImegName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Images", ImegName), FileMode.Create);
                    files[0].CopyTo(fileStream);
                    imegPath = @"/Images/" + ImegName;
                }

                MeunItemVM.MenuItem.Imeg = imegPath;
                _context.MenuItems.Add(MeunItemVM.MenuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
           
            return View(MeunItemVM);

        }
      //  GET: Admin/MenuItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem =  _context.MenuItems.Include(m=>m.Category).Include(m=>m.SubCategory).SingleOrDefault(m=>m.Id==id);
            if (menuItem == null)
            {
                return NotFound();
            }
            MeunItemVM.MenuItem = menuItem;
            MeunItemVM.SubCategoriesList = await _context.SubCategories.Where(m => m.CategoryId == MeunItemVM.MenuItem.CategoryId).ToListAsync();
            return View(MeunItemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Edit")]
        public async Task<IActionResult> EditPost()
        {
            if (ModelState.IsValid)
            {
                
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string webRootPath = webHost.WebRootPath;
                    string ImegName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);
                    FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Images", ImegName), FileMode.Create);
                    files[0].CopyTo(fileStream);
                    string imegPath = @"/Images/" + ImegName;
                    MeunItemVM.MenuItem.Imeg = imegPath;
                }

                
                        
                _context.MenuItems.Update(MeunItemVM.MenuItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(MeunItemVM);

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = _context.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefault(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }
            MeunItemVM.MenuItem = menuItem;
            MeunItemVM.SubCategoriesList = await _context.SubCategories.Where(m => m.CategoryId == MeunItemVM.MenuItem.CategoryId).ToListAsync();
            return View(MeunItemVM);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuItem = _context.MenuItems.Include(m => m.Category).Include(m => m.SubCategory).SingleOrDefault(m => m.Id == id);
            if (menuItem == null)
            {
                return NotFound();
            }
            MeunItemVM.MenuItem = menuItem;
            MeunItemVM.SubCategoriesList = await _context.SubCategories.Where(m => m.CategoryId == MeunItemVM.MenuItem.CategoryId).ToListAsync();
            return View(MeunItemVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public async Task<IActionResult> DeletePost()
        {
            /*
                 var files = HttpContext.Request.Form.Files;
                 if (files.Count > 0)
                 {
                     string webRootPath = webHost.WebRootPath;
                     string ImegName = DateTime.Now.ToFileTime().ToString() + Path.GetExtension(files[0].FileName);
                     FileStream fileStream = new FileStream(Path.Combine(webRootPath, "Images", ImegName), FileMode.Create);
                     files[0].CopyTo(fileStream);
                     string imegPath = @"/Images/" + ImegName;
                     MeunItemVM.MenuItem.Imeg = imegPath;
                 }

             */

            _context.MenuItems.Remove(MeunItemVM.MenuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

     
        }


        // GET: Admin/MenuItems/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var menuItem = await _context.MenuItems
        //        .Include(m => m.Category)
        //        .Include(m => m.SubCategory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (menuItem == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(menuItem);
        //}

        // POST: Admin/MenuItems/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var menuItem = await _context.MenuItems.FindAsync(id);
        //    _context.MenuItems.Remove(menuItem);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MenuItemExists(int id)
        //{
        //    return _context.MenuItems.Any(e => e.Id == id);
        //}
    }
}

