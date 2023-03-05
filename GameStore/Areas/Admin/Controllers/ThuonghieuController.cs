using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class ThuonghieuController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        // GET: Admin/Thuonghieu
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            return View(db.tblThuongHieus.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, tblThuongHieu th)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var thuonghieu = collection["TenTH"];
            if (string.IsNullOrEmpty(thuonghieu))
            {
                ViewData["Loi"] = "Tên thương hiệu không được để trống";
            }
            else
            {
                string min = DateTime.Now.ToString("mm");
                string sec = DateTime.Now.ToString("ss");
                string MaThuongHieu = "T" + "" + min + "" + sec;
                th.MaTH = MaThuongHieu;
                th.TenTH = thuonghieu;
                db.tblThuongHieus.InsertOnSubmit(th);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var thuonghieu = db.tblThuongHieus.First(m => m.MaTH == id);
            return View(thuonghieu);
        }

        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var thuonghieu = db.tblThuongHieus.First(m => m.MaTH == id);
            var tenth = collection["TenTH"];
            thuonghieu.MaTH = id;
            if (string.IsNullOrEmpty(tenth))
            {
                ViewData["Loi"] = "Tên thương hiệu không được để trống tên";
            }
            else
            {
                thuonghieu.TenTH = tenth;
                UpdateModel(thuonghieu);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var thuonghieu = db.tblThuongHieus.First(m => m.MaTH == id);
            return View(thuonghieu);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var thuonghieu = db.tblThuongHieus.Where(m => m.MaTH == id).First();
            db.tblThuongHieus.DeleteOnSubmit(thuonghieu);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}