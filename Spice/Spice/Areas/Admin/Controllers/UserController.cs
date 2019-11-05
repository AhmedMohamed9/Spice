using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Utility;

namespace Spice.Areas.Admin.Controllers
{
    [Authorize(Roles = SD.ManagerUser)]

    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            var claimidentity=(ClaimsIdentity)this.User.Identity;
            var claim = claimidentity.FindFirst(ClaimTypes.NameIdentifier);
            return View(await db.ApplicationUser.Where(x=>x.Id!=claim.Value).ToListAsync());
        }

        public async Task<IActionResult> Lock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationUser = await db.ApplicationUser.Where(m => m.Id == id).FirstOrDefaultAsync();
            if(applicationUser == null)
            {
                return NotFound();
            }
            applicationUser.LockoutEnd = DateTime.Now.AddDays(5);
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> UnLock(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var applicationUser = await db.ApplicationUser.Where(m => m.Id == id).FirstOrDefaultAsync();
            if(applicationUser == null)
            {
                return NotFound();
            }
            applicationUser.LockoutEnd = DateTime.Now;
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }
    }
}