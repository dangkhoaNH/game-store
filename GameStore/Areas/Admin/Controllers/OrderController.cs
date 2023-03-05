using GameStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        // GET: Admin/Order
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.tblDonHangs.ToList().OrderByDescending(n => n.NgayLap).ToPagedList(pageNumber, pageSize));
        }
    }
}