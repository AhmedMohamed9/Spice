using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models.Viewmodel;

namespace Spice.Areas.Admin.Controllers
{

    [Area("Admin")]

    public class SubcategoryController : Controller
    {
        [TempData]
        public string statusMessage { get; set; }
        private readonly ApplicationDbContext db;

        public SubcategoryController(ApplicationDbContext db)
        {
            this.db = db;
        }



        public async Task<IActionResult> Index()
        {
            var subcategory = db.subcategory.Include(x => x.category);
            return View(await subcategory.ToListAsync());
        }




        //Get Action for Create
        public IActionResult create()
        {
            SubcategoryViewModel model = new SubcategoryViewModel()
            {
                subCategory = new Models.Subcategory(),
                categorieslist = db.categories.ToList(),
                subcategorylist = db.subcategory.OrderBy(p => p.name).Select(x => x.name).Distinct().ToList()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(SubcategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doessubcatisexist = db.subcategory.Where(x => x.name == model.subCategory.name).Count();
                var doessubcatandcatexist = db.subcategory.Where(x => x.name == model.subCategory.name && x.categoryid == model.subCategory.categoryid).Count();

                if (doessubcatisexist > 0 )
                {
                    //error
                    statusMessage = "error : Subcategory Already exist";
                }
                else
                {
                        if (doessubcatandcatexist > 0)
                        {
                            //error
                            statusMessage = "error : Subcategory and category Already exist";

                        }
                        else
                        {
                            db.Add(model.subCategory);
                            await db.SaveChangesAsync();
                            return RedirectToAction("index");
                        }
                    
                }
            }
            SubcategoryViewModel modelvm = new SubcategoryViewModel()
            {
                subCategory = new Models.Subcategory(),
                categorieslist = db.categories.ToList(),
                subcategorylist = db.subcategory.OrderBy(p => p.name).Select(x => x.name).ToList(),
                StatusMessage = statusMessage
            };
            return View(modelvm);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var osubcategory = await db.subcategory.SingleOrDefaultAsync(x => x.id == id);
            if (osubcategory == null)
            {
                return NotFound();
            }
            SubcategoryViewModel model = new SubcategoryViewModel()
            {
                categorieslist = db.categories.ToList(),
                subCategory = osubcategory,
                subcategorylist = db.subcategory.Select(x => x.name).Distinct().ToList()
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SubcategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var doessubcatisexist = db.subcategory.Where(x => x.name == model.subCategory.name).Count();
                var doessubcatandcatexist = db.subcategory.Where(x => x.name == model.subCategory.name && x.categoryid == model.subCategory.categoryid).Count();

                if (doessubcatisexist == 0)
                {
                    statusMessage = "Error : sub Category Doesn't exist.you cann't add a new subcategory here";
                }
                else
                {
                    if (doessubcatandcatexist > 0)
                    {
                        statusMessage = "Error : sub Category and category already here";
                    }
                    else
                    {
                        var subcat = db.subcategory.Find(id);
                        subcat.categoryid = model.subCategory.categoryid;
                        subcat.name = model.subCategory.name;
                        await db.SaveChangesAsync();
                        return RedirectToAction("index");

                    }
                }
            }
            SubcategoryViewModel modelvm = new SubcategoryViewModel()
            {
                subCategory = new Models.Subcategory(),
                categorieslist = db.categories.ToList(),
                subcategorylist = db.subcategory.OrderBy(p => p.name).Select(x => x.name).ToList(),
                StatusMessage = statusMessage
            };
            return View(modelvm);

        }


        public async Task<IActionResult> details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subcat = await db.subcategory.Include(x => x.category).SingleOrDefaultAsync(x => x.id == id);
            if (subcat == null)
            {
                return NotFound();
            }
            return View(subcat);
        }
        public async Task<IActionResult> delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var subcat = await db.subcategory.Include(x => x.category).SingleOrDefaultAsync(x => x.id == id);
            if (subcat == null)
            {
                return NotFound();
            }
            return View(subcat);
        }
        [HttpPost, ActionName("delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> confirmdelte(int id)
        {
            var subcat = await db.subcategory.SingleOrDefaultAsync(m => m.id == id);
            db.Remove(subcat);
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}