using GameStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class OrderDetailController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        // GET: Admin/OrderDetail
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.tblChiTietDonHangs.ToList().OrderBy(n => n.MaDH).ToPagedList(pageNumber, pageSize));
        }
    }
}