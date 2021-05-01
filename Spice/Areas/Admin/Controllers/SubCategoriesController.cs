using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        [TempData]
        public string stutasMessage { get; set; }

        public SubCategoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/SubCategories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.SubCategories.Include(s => s.Category);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Admin/SubCategories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory = _context.SubCategories.Include(m => m.Category).Where(m => m.Id == id).SingleOrDefault();

            if (subCategory == null)
            {
                return NotFound();
            }
            return View(subCategory);
        }

        // GET: Admin/SubCategories/Create
        public async Task < IActionResult> Create()
        {
            SubCategoryandCategoryViewModel model = new SubCategoryandCategoryViewModel()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = new SubCategory()
            };
       //  ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(model);
        }

        // POST: Admin/SubCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SubCategoryandCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {

            
            var dosCategoreEqule =await _context.SubCategories.Include(m => m.Category)
                .Where(m => m.CategoryId == model.SubCategory.CategoryId && m.Name == model.SubCategory.Name).ToListAsync();

            if (dosCategoreEqule.Count() > 0)
            {
                stutasMessage = "Error :This is Sub Categoris Exisist undor " + dosCategoreEqule.FirstOrDefault().Category.Name + "category";
            }
            else { 
            
            
                _context.SubCategories.Add(model.SubCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
           } 
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", model.SubCategory);
            }
            SubCategoryandCategoryViewModel vmModel = new SubCategoryandCategoryViewModel()
            {
                CategoryList = await _context.Categories.ToListAsync(),
               SubCategory=model.SubCategory,
                StutasMessage= stutasMessage
            };
            return View(vmModel);
        }

        // GET: Admin/SubCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subCategory = await _context.SubCategories.FindAsync(id);
            if (subCategory==null)
            {
                return NotFound();
            }
            SubCategoryandCategoryViewModel model = new SubCategoryandCategoryViewModel()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = subCategory
            };
         //   ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> GetSubCategory(int id)
        {
            List<SubCategory> subCategories = new List<SubCategory>();
            subCategories = await _context.SubCategories.Where(m => m.CategoryId == id).ToListAsync();
            return Json(new SelectList(subCategories, "Id", "Name"));
        }
        // POST: Admin/SubCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SubCategoryandCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {

          
            var dosCategoreEqule = await _context.SubCategories.Include(m => m.Category)
                .Where(m => m.CategoryId == model.SubCategory.CategoryId && m.Name == model.SubCategory.Name&& m.Id != model.SubCategory.Id).ToListAsync();

            if (dosCategoreEqule.Count() > 0)
            {
                stutasMessage = "Error :This is Sub Categoris Exisist undor " + dosCategoreEqule.FirstOrDefault().Category.Name + "category";
            }
            else
            {
                
                    _context.SubCategories.Update(model.SubCategory);

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
                }
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", model.SubCategory);

            SubCategoryandCategoryViewModel vmModel = new SubCategoryandCategoryViewModel()
            {
                CategoryList = await _context.Categories.ToListAsync(),
                SubCategory = model.SubCategory,
                StutasMessage = stutasMessage
            };
            return View(vmModel);
        }
    

        // GET: Admin/SubCategories/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subCategory =  _context.SubCategories.Include(m=>m.Category).Where(m=>m.Id==id).SingleOrDefault();

            if (subCategory == null)
            {
                return NotFound();
            }

          
            return View(subCategory);
        }

        // POST: Admin/SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var subCategory = await _context.SubCategories.FindAsync(id);
            _context.SubCategories.Remove(subCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubCategoryExists(int id)
        {
            return _context.SubCategories.Any(e => e.Id == id);
        }
    }
}
