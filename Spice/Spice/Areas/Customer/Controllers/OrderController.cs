using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Spice.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Spice.Models.Viewmodel;
using Microsoft.EntityFrameworkCore;
using Spice.Models;
using ReflectionIT.Mvc.Paging;
using Spice.Utility;

namespace Spice.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext db;

        public OrderController(ApplicationDbContext db)
        {
            this.db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> confirm(int id)
        {
            var clamidentity = (ClaimsIdentity)User.Identity;
            var claim = clamidentity.FindFirst(ClaimTypes.NameIdentifier);
            OrderDetailsConfirm_ViewModel orderdetails = new OrderDetailsConfirm_ViewModel()
            {
                orderHeaders = await db.OrderHeaders.Include(o => o.ApplicationUser).FirstOrDefaultAsync(x => x.id == id && x.userid == claim.Value),
                orderDatails = await db.OrderDatails.Where(x => x.orderid == id).ToListAsync()
            };
            return View(orderdetails);
        }
        [Authorize]
        public async Task<IActionResult> orderhistory(int page = 0)
        {
            var clamidentity = (ClaimsIdentity)User.Identity;
            var claim = clamidentity.FindFirst(ClaimTypes.NameIdentifier);

            List<OrderDetailsConfirm_ViewModel> orderlist = new List<OrderDetailsConfirm_ViewModel>();
            List<OrderHeaders> orderHeaderlist = await db.OrderHeaders.Include(o => o.ApplicationUser).Where(u => u.ApplicationUser.Id == claim.Value).ToListAsync();
            int pagesize = 5;
            //foreach (OrderHeaders item in orderHeaderlist)
            for (int i = page * pagesize; i < orderHeaderlist.Count && i < (page * pagesize) + pagesize; i++)
            {

                OrderDetailsConfirm_ViewModel individual = new OrderDetailsConfirm_ViewModel
                {
                    orderHeaders = orderHeaderlist[i],
                    orderDatails = await db.OrderDatails.Where(x => x.orderid == orderHeaderlist[i].id).ToListAsync()
                };
                orderlist.Add(individual);
            }
            ViewBag.pagingsize = (int)Math.Ceiling((decimal)orderHeaderlist.Count / pagesize);
            ViewBag.page = page;
            return View(orderlist);
        }
        [Authorize(Roles = SD.KitchenUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> ManageOrder()
        {
            List<OrderDetailsConfirm_ViewModel> orderlist = new List<OrderDetailsConfirm_ViewModel>();

            List<OrderHeaders> orderHeaderlist = await db.OrderHeaders.Where(x => x.Status == SD.StatusSubmitted || x.Status == SD.StatusInProcess)
                                                    .OrderByDescending(x => x.Picktime)
                                                    .ToListAsync();
            foreach (OrderHeaders item in orderHeaderlist)
            {

                OrderDetailsConfirm_ViewModel individual = new OrderDetailsConfirm_ViewModel
                {
                    orderHeaders = item,
                    orderDatails = await db.OrderDatails.Where(x => x.orderid == item.id).ToListAsync()
                };
                orderlist.Add(individual);
            }

            return View(orderlist.OrderBy(b => b.orderHeaders.Picktime).ToList());
        }
        [Authorize]
        public async Task<IActionResult> GetorderDetails(int id)
        {
            OrderDetailsConfirm_ViewModel orderdetails = new OrderDetailsConfirm_ViewModel()
            {
                orderHeaders = await db.OrderHeaders.FirstOrDefaultAsync(x => x.id == id),
                orderDatails = await db.OrderDatails.Where(x => x.orderid == id).ToListAsync()

            };
            orderdetails.orderHeaders.ApplicationUser = await db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == orderdetails.orderHeaders.userid);
            return PartialView("_individualorderdetails", orderdetails);
        }

        [Authorize(Roles = SD.KitchenUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> orderprepare(int orderid)
        {
            OrderHeaders orderheder = await db.OrderHeaders.FirstOrDefaultAsync(x => x.id == orderid);
            orderheder.Status = SD.StatusInProcess;
            await db.SaveChangesAsync();
            return RedirectToAction("ManageOrder");
        }
        [Authorize(Roles = SD.KitchenUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> orderReady(int orderid)
        {
            OrderHeaders orderheder = await db.OrderHeaders.FirstOrDefaultAsync(x => x.id == orderid);
            orderheder.Status = SD.StatusReady;
            await db.SaveChangesAsync();
            return RedirectToAction("ManageOrder");
        }
        [Authorize(Roles = SD.KitchenUser + "," + SD.ManagerUser)]
        public async Task<IActionResult> orderCancel(int orderid)
        {
            OrderHeaders orderheder = await db.OrderHeaders.FirstOrDefaultAsync(x => x.id == orderid);
            orderheder.Status = SD.StatusCancelled;
            await db.SaveChangesAsync();
            return RedirectToAction("ManageOrder");
        }

        [Authorize]
        public async Task<IActionResult> orderPickUP(int page = 0, string SearchName = null, string SearchPhone = null, string SearchEmail = null)
        {

            List<OrderDetailsConfirm_ViewModel> orderlist = new List<OrderDetailsConfirm_ViewModel>();
            List<OrderHeaders> orderHeaderlist = await db.OrderHeaders.Include(o => o.ApplicationUser).Where(u => u.Status == SD.StatusReady).ToListAsync();
            int pagesize = 5;
            /*|| SearchPhone != null || SearchEmail != null)*/
            if (SearchName != null)
            {
                orderHeaderlist = orderHeaderlist.Where(x => x.Picupname.Contains(SearchName)).ToList();
            }
            if (SearchPhone != null)
            {
                orderHeaderlist = orderHeaderlist.Where(x => x.Phonenumber.Contains(SearchPhone)).ToList();
            }
            if (SearchEmail != null)
            {
                orderHeaderlist = orderHeaderlist.Where(x => x.ApplicationUser.Email.Contains(SearchEmail)).ToList();
            }

            //foreach (OrderHeaders item in orderHeaderlist)
            for (int i = page * pagesize; i < orderHeaderlist.Count && i < (page * pagesize) + pagesize; i++)
            {
                OrderDetailsConfirm_ViewModel individual = new OrderDetailsConfirm_ViewModel();
                individual.orderHeaders = orderHeaderlist[i];
                individual.orderDatails = await db.OrderDatails.Where(x => x.orderid == orderHeaderlist[i].id).ToListAsync();
                orderlist.Add(individual);
            }

            ViewBag.pagingsize = (int)Math.Ceiling((decimal)orderHeaderlist.Count / pagesize);
            ViewBag.page = page;
            return View(orderlist);
        }
        
        [Authorize]
        public async Task<IActionResult> pickup(int id)
        {
            OrderHeaders orderheder = await db.OrderHeaders.FirstOrDefaultAsync(x => x.id == id);
            orderheder.Status = SD.StatusCompleted;
            await db.SaveChangesAsync();
            return RedirectToAction("orderPickUP");
        }
    }

}