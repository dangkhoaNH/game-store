using GameStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class CustomerController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        // GET: Admin/Customer
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.tblKhachHangs.ToList().OrderBy(n => n.MaKH).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            var khachhang = db.tblKhachHangs.First(m => m.MaKH == id);
            return View(khachhang);
        }
        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var khachhang = db.tblKhachHangs.Where(m => m.MaKH == id).First();
            db.tblKhachHangs.DeleteOnSubmit(khachhang);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}