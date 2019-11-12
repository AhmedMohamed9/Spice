using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spice.Data;
using Spice.Models;
using Spice.Models.Viewmodel;
using Spice.Utility;

namespace Spice.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db;
        [BindProperty]
        public OrderDetails_ViewModel detailcard { get; set; }

        public CartController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public async Task<IActionResult> Index()
        {
            detailcard = new OrderDetails_ViewModel()
            {
                OrderHeaders = new OrderHeaders(),
            };
            detailcard.OrderHeaders.ordertotal = 0;
            var clamidentity = (ClaimsIdentity)User.Identity;
            var claim = clamidentity.FindFirst(ClaimTypes.NameIdentifier);
            var cart = db.shopingCarts.Where(x => x.applicationuserid == claim.Value);
            if (cart != null)
            {
                detailcard.Listcart = await cart.ToListAsync();
            }
            foreach (var list in detailcard.Listcart)
            {
                list.Menuitem = await db.Menuitem.FirstOrDefaultAsync(m => m.id == list.menuitemid);
                detailcard.OrderHeaders.ordertotal = detailcard.OrderHeaders.ordertotal + (list.Menuitem.Price * list.count);
                if (list.Menuitem.description.Length > 100)
                {
                    list.Menuitem.description = list.Menuitem.description.Substring(0, 99) + "...";
                }
            }

            detailcard.OrderHeaders.OrdartotalOriginal = detailcard.OrderHeaders.ordertotal;
            detailcard.OrderHeaders.copunslist = await db.Copuns.ToListAsync();
            if (HttpContext.Session.GetInt32(SD.sscouponcode) != null && HttpContext.Session.GetInt32(SD.sscouponcode) != 0)
            {
                Copuns cop = await db.Copuns.FirstOrDefaultAsync(x => x.id == HttpContext.Session.GetInt32(SD.sscouponcode));
                detailcard.OrderHeaders.ordertotal = SD.discount(cop, detailcard.OrderHeaders.OrdartotalOriginal);
            }
            if (HttpContext.Session.GetInt32(SD.sscouponcode) == 0)
            {
                detailcard.OrderHeaders.ordertotal = detailcard.OrderHeaders.OrdartotalOriginal;
            }
            return View(detailcard);
        }
        public async Task<IActionResult> Summary()
        {
            detailcard = new OrderDetails_ViewModel()
            {
                OrderHeaders = new OrderHeaders(),
            };
            detailcard.OrderHeaders.ordertotal = 0;
            var clamidentity = (ClaimsIdentity)User.Identity;
            var claim = clamidentity.FindFirst(ClaimTypes.NameIdentifier);
            ApplicationUser applicationuser = await db.ApplicationUser.FirstOrDefaultAsync(x => x.Id == claim.Value);
            var cart = db.shopingCarts.Where(x => x.applicationuserid == claim.Value);
            if (cart != null)
            {
                detailcard.Listcart = await cart.ToListAsync();
            }
            foreach (var list in detailcard.Listcart)
            {
                list.Menuitem = await db.Menuitem.FirstOrDefaultAsync(m => m.id == list.menuitemid);
                detailcard.OrderHeaders.ordertotal = detailcard.OrderHeaders.ordertotal + (list.Menuitem.Price * list.count);
            }
            detailcard.OrderHeaders.OrdartotalOriginal = detailcard.OrderHeaders.ordertotal;
            detailcard.OrderHeaders.Picupname = applicationuser.Name;
            detailcard.OrderHeaders.Picktime = DateTime.Now;
            detailcard.OrderHeaders.Phonenumber = applicationuser.PhoneNumber;
            detailcard.OrderHeaders.copunslist = await db.Copuns.ToListAsync();
            if (HttpContext.Session.GetInt32(SD.sscouponcode) != null && HttpContext.Session.GetInt32(SD.sscouponcode) != 0)
            {
                Copuns cop = await db.Copuns.FirstOrDefaultAsync(x => x.id == HttpContext.Session.GetInt32(SD.sscouponcode));
                detailcard.OrderHeaders.ordertotal = SD.discount(cop, detailcard.OrderHeaders.OrdartotalOriginal);
                detailcard.OrderHeaders.CouponCode = cop.Name;

            }
            if (HttpContext.Session.GetInt32(SD.sscouponcode) == 0)
            {
                detailcard.OrderHeaders.ordertotal = detailcard.OrderHeaders.OrdartotalOriginal;
            }
            return View(detailcard);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Summary")]
        public async Task<IActionResult> Summarypost()
        {
            var clamidentity = (ClaimsIdentity)User.Identity;
            var claim = clamidentity.FindFirst(ClaimTypes.NameIdentifier);
            detailcard.Listcart = await db.shopingCarts.Where(c => c.applicationuserid == claim.Value).ToListAsync();

            detailcard.OrderHeaders.PaymentStatus = SD.PaymentStatuspending;
            detailcard.OrderHeaders.orderdate = DateTime.Now;
            detailcard.OrderHeaders.userid = claim.Value;
            detailcard.OrderHeaders.Status = SD.PaymentStatuspending;
            detailcard.OrderHeaders.Picktime = Convert.ToDateTime(detailcard.OrderHeaders.PickDate.ToShortDateString()
                                              + " " + detailcard.OrderHeaders.Picktime.ToShortTimeString());
            List<OrderDatails> orderdetailslist = new List<OrderDatails>();
            db.OrderHeaders.Add(detailcard.OrderHeaders);
            await db.SaveChangesAsync();
            detailcard.OrderHeaders.OrdartotalOriginal = 0;

            foreach (var item in detailcard.Listcart)
            {
                item.Menuitem = await db.Menuitem.FirstOrDefaultAsync(m => m.id == item.menuitemid);
                OrderDatails orderDatails = new OrderDatails()
                {
                    Menuitemid = item.menuitemid,
                    orderid = detailcard.OrderHeaders.id,
                    Description = item.Menuitem.description,
                    name = item.Menuitem.name,
                    price = item.Menuitem.Price,
                    count = item.count
                };
                detailcard.OrderHeaders.OrdartotalOriginal += orderDatails.count * orderDatails.price;
                db.OrderDatails.Add(orderDatails);
            }
            detailcard.OrderHeaders.copunslist = await db.Copuns.ToListAsync();
            if (HttpContext.Session.GetInt32(SD.sscouponcode) != null && HttpContext.Session.GetInt32(SD.sscouponcode) != 0)
            {
                Copuns cop = await db.Copuns.FirstOrDefaultAsync(x => x.id == HttpContext.Session.GetInt32(SD.sscouponcode));
                detailcard.OrderHeaders.ordertotal = SD.discount(cop, detailcard.OrderHeaders.OrdartotalOriginal);
                detailcard.OrderHeaders.CouponCode = cop.Name;

            }
            else
            {
                detailcard.OrderHeaders.ordertotal = detailcard.OrderHeaders.OrdartotalOriginal;
            }
            detailcard.OrderHeaders.CouponCodeDiscount = detailcard.OrderHeaders.OrdartotalOriginal - detailcard.OrderHeaders.ordertotal;
            db.RemoveRange(detailcard.Listcart);
            
            HttpContext.Session.SetInt32("sscartcount", 0);
            await db.SaveChangesAsync();
            return RedirectToAction("index", "Home");
        }
        public IActionResult Addcoupon(int id, double totalorignal)
        {
            if (id != 0)
            {
                HttpContext.Session.SetInt32(SD.sscouponcode, id);
            }
            return RedirectToAction("index");
        }
        public IActionResult removecoupon()
        {
            HttpContext.Session.SetInt32(SD.sscouponcode, 0);

            return RedirectToAction("index");
        }
        public async Task<IActionResult> plus(int cartid)
        {
            var cart = await db.shopingCarts.FirstOrDefaultAsync(x => x.id == cartid);
            cart.count += 1;
            await db.SaveChangesAsync();
            return RedirectToAction("index");
        }
        public async Task<IActionResult> minus(int cartid)
        {
            var cart = await db.shopingCarts.FirstOrDefaultAsync(x => x.id == cartid);
            if (cart.count > 1)
            {
                cart.count -= 1;
                await db.SaveChangesAsync();
            }

            return RedirectToAction("index");
        }
        public async Task<IActionResult> remove(int cartid)
        {
            var cart = await db.shopingCarts.FirstOrDefaultAsync(x => x.id == cartid);
            db.Remove(cart);
            await db.SaveChangesAsync();
            var cnt = db.shopingCarts.Where(u => u.applicationuserid == cart.applicationuserid).ToList().Count;
            HttpContext.Session.SetInt32("sscartcount", cnt);

            return RedirectToAction("index");
        }


    }
}