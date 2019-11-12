using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;

namespace Spice.Areas.Admin.Controllers
{
    [Area ("Admin")]
    public class CouponsController : Controller
    {
        private readonly ApplicationDbContext db;
        private readonly IHostingEnvironment hosting;

        public CouponsController(ApplicationDbContext db, IHostingEnvironment Hosting)
        {
            this.db = db;
            hosting = Hosting;
        }
        public async Task<IActionResult> Index()
        {
            return View(await db.Copuns.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Copuns coupons)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files[0] != null && files[0].Length > 0)
                {
                    coupons.Picture = TransimageToByte(files[0]);
                }
                db.Copuns.Add(coupons);
                await db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(coupons);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupon = await db.Copuns.SingleOrDefaultAsync(x => x.id == id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Copuns coupons)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (coupons.Picture != null)
                {
                    if (files[0] != null && files[0].Length > 0)
                    {
                        coupons.Picture = TransimageToByte(files[0]);
                    }
                }
                db.Copuns.Update(coupons);
                await db.SaveChangesAsync();
                return RedirectToAction("index");
            }
            return View(coupons);
        }

        public async Task<IActionResult> delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var coupons = await db.Copuns.SingleOrDefaultAsync(x => x.id == id);
            if (coupons == null)
            {
                return NotFound();
            }
            return View(coupons);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> delete(Copuns copuns)
        {
            if (copuns == null)
            {
                return NotFound();
            }
            db.Copuns.Remove(copuns);
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }







        public byte[] TransimageToByte(IFormFile file)
        {
            byte[] p1 = null;
            using (var f1 = file.OpenReadStream())
            {
                using (var ms1 = new MemoryStream())
                {
                    f1.CopyTo(ms1);
                    p1 = ms1.ToArray();
                }
            }
            return p1;
        }
    }
}