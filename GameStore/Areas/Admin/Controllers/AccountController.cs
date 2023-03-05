using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        // GET: Admin/Account

        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Default");
            else
                return View(db.tblTaiKhoans.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            ViewBag.MaLoaiTK = new SelectList(db.tblLoaiTaiKhoans.ToList().OrderBy(n => n.ChucVu), "MaLoaiTK", "ChucVu");
            return View();
        }
        [HttpPost]
        public ActionResult Create(tblTaiKhoan tk)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            ViewBag.MaLoaiTK = new SelectList(db.tblLoaiTaiKhoans.ToList().OrderBy(n => n.ChucVu), "MaLoaiTK", "ChucVu");
            int count = db.tblTaiKhoans.Where(u => u.Username == tk.Username).ToList().Count();
            if (tk.Username == null || count > 0)
            {
                ViewBag.ErrorName = "Vui lòng nhập username hoặc username đã tồn tại";
                return View();
            }else if (tk.Username == null)
            {
                ViewBag.ErrorPass = "Vui lòng nhập username";
                return View();
            }
            else
            {
                db.tblTaiKhoans.InsertOnSubmit(tk);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            var taikhoan = db.tblTaiKhoans.SingleOrDefault(n => n.Username == id);
            if (taikhoan == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaLoaiTK = new SelectList(db.tblLoaiTaiKhoans.ToList().OrderBy(n => n.ChucVu), "MaLoaiTK", "ChucVu");

            return View(taikhoan);
        }

        [HttpPost]
        public ActionResult Edit(string id, tblTaiKhoan tk)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            ViewBag.MaLoaiTK = new SelectList(db.tblLoaiTaiKhoans.ToList().OrderBy(n => n.ChucVu), "MaLoaiTK", "ChucVu");
            var taikhoan = db.tblTaiKhoans.FirstOrDefault(p => p.Username.Equals(tk.Username));
            UpdateModel(taikhoan);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}