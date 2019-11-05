using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Spice.Data;
using Spice.Models;
using Spice.Models.Viewmodel;
using Tangy.Models.ViewModels;

namespace Spice.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext db;

        public HomeController(ILogger<HomeController> logger,ApplicationDbContext db)
        {
            _logger = logger;
            this.db = db;
        }

        public async Task<IActionResult> Index(string cat)
        {
            Index_ViewModel IndexVM = new Index_ViewModel()
            {
               // Menuitems = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).ToListAsync(),
                categories = await db.categories.ToListAsync(),
                Copuns = db.Copuns.ToList()
            };
            if (cat == null || cat =="all")
            {
                IndexVM.Menuitems = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).ToListAsync();
            }
            else
            {
                IndexVM.Menuitems = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).Where(x => x.Category.name == cat).ToListAsync();
            }
            if (cat == null || cat == "all")
            {
                ViewData["onclick"] = "all";

            }
            else
            {
                ViewData["onclick"] = cat;

            }


            return View(IndexVM);
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var menuitemdb =await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).FirstOrDefaultAsync(x=>x.id==id);
            ShopingCart cartobj = new ShopingCart()
            {
                Menuitem= menuitemdb,
                menuitemid = menuitemdb.id
            };
            return View(cartobj);
        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(ShopingCart CartObj)
        {
            if (ModelState.IsValid)
            {
                var claimidentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
                CartObj.applicationuserid = claim.Value;
                CartObj.id = 0;
                ShopingCart cartFromdb = await db.shopingCarts.Where(x => x.applicationuserid == CartObj.applicationuserid && x.menuitemid == CartObj.menuitemid).FirstOrDefaultAsync();
                if (cartFromdb == null)
                {
                   await db.shopingCarts.AddAsync(CartObj);
                }
                else
                {
                    cartFromdb.count = cartFromdb.count+ CartObj.count;
                }
                await db.SaveChangesAsync();
                var count = db.shopingCarts.Where(x => x.applicationuserid == CartObj.applicationuserid).ToList().Count();
                HttpContext.Session.SetInt32("sscartcount", count);
                return RedirectToAction("index");

            }
            else
            {
                var menuitemdb = await db.Menuitem.Include(m => m.Category).Include(x => x.subcategory).FirstOrDefaultAsync(x => x.id == CartObj.menuitemid);
                ShopingCart cartobj = new ShopingCart()
                {
                    Menuitem = menuitemdb,
                    menuitemid = menuitemdb.id
                };
                return View(cartobj);
            }
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
