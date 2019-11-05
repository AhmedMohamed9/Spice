using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Utility;
using Tangy.Models.ViewModels;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]
    [Area("Admin")]
    public class MenuitemController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment hostingEnvironment;

        [BindProperty]
        public Menuitem_ViewModel MenuItemVM { get; set; }


        public MenuitemController(ApplicationDbContext db, IHostingEnvironment hostingEnvironment)
        {
            this.db = db;
            this.hostingEnvironment = hostingEnvironment;
            this.MenuItemVM = new Menuitem_ViewModel()
            {
                menuitem = new Menuitem(),
                categorylist = db.categories.ToList(),

            };
        }


        public async Task<IActionResult> Index()
        {
            var menuitems = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).ToListAsync();
            return View(menuitems);
        }


        public IActionResult create()
        {
            return View(MenuItemVM);

        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Createpost()
        {
            MenuItemVM.menuitem.subcategoryid = Convert.ToInt32(Request.Form["subcategoryid"].ToString());
            MenuItemVM.menuitem.categoryid = Convert.ToInt32(Request.Form["categoryid"].ToString());

            if (!ModelState.IsValid)
            {
                return View(MenuItemVM);
            }
            MenuItemVM.menuitem.image = UploadFile(Request.Form.Files[0]);
            db.Menuitem.Add(MenuItemVM.menuitem);
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.menuitem = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).SingleOrDefaultAsync(s => s.id == id);
            if (MenuItemVM.menuitem == null)
            {
                return NotFound();
            }
            MenuItemVM.Subcategorylist = db.subcategory.Where(x => x.categoryid == MenuItemVM.menuitem.categoryid).ToList();

            return View(MenuItemVM);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editpost()
        {
            if (ModelState.IsValid)
            {
                MenuItemVM.menuitem.subcategoryid = Convert.ToInt32(Request.Form["subcategoryid"].ToString());
                MenuItemVM.menuitem.categoryid = Convert.ToInt32(Request.Form["categoryid"].ToString());

                MenuItemVM.menuitem.image = UploadFile(MenuItemVM.File, MenuItemVM.menuitem.image);
                db.Update(MenuItemVM.menuitem);
                await db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(MenuItemVM);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.menuitem = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).SingleOrDefaultAsync(s => s.id == id);

            if (MenuItemVM.menuitem == null)
            {
                return NotFound();
            }

            return View(MenuItemVM);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MenuItemVM.menuitem = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).SingleOrDefaultAsync(s => s.id == id);
            if (MenuItemVM.menuitem == null)
            {
                return NotFound();
            }
            MenuItemVM.Subcategorylist = db.subcategory.Where(x => x.categoryid == MenuItemVM.menuitem.categoryid).ToList();

            return View(MenuItemVM);
        }
        public async Task<IActionResult> deletepost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var men = await db.Menuitem.SingleOrDefaultAsync(x => x.id == id);

            db.Menuitem.Remove(men);
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public JsonResult getsubcategory(int categoryid)
        {
            List<Subcategory> subcat = new List<Subcategory>();
            subcat = (from subCategories in db.subcategory
                      where subCategories.categoryid == categoryid
                      select subCategories).ToList();
            return Json(new SelectList(subcat, "id", "name"));
        }
            string UploadFile(IFormFile file)
        {
            //at Create
            if (file != null)
            {
                string uploads = Path.Combine(hostingEnvironment.WebRootPath, "images");

                string newPath = Path.Combine(uploads, file.FileName);


                file.CopyTo(new FileStream(newPath, FileMode.Create));


                return file.FileName;
            }

            return null;
        }


        string UploadFile(IFormFile file, string imageUrl)
        {
            //at Edit
            if (file != null)
            {
                string uploads = Path.Combine(hostingEnvironment.WebRootPath, "images");

                string newPath = Path.Combine(uploads, file.FileName);
                string oldPath = Path.Combine(uploads, imageUrl);

                if (oldPath != newPath)
                {
                    file.CopyTo(new FileStream(newPath, FileMode.Create));
                }

                return file.FileName;
            }

            return imageUrl;
        }


    }
}