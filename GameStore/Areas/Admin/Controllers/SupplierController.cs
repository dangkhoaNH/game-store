using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class SupplierController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        // GET: Admin/Supplier
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            return View(db.tblNhaCungCaps.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, tblNhaCungCap ncc)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var tencungcap = collection["TenNCC"];
            var diachi = collection["DiaChi"];
            var sdt = collection["SDT"];

            if (string.IsNullOrEmpty(tencungcap))
            {
                ViewBag.ErrorName = "Tên nhà cung cấp không được để trống";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewBag.ErrorAddress = "Phải nhập địa chỉ nhà cung cấp";
            }
            else if (string.IsNullOrEmpty(sdt))
            {
                ViewBag.ErrorPhone = "Số điện thoại không được để trống";
            }
            else
            {
                string min = DateTime.Now.ToString("mm");
                string sec = DateTime.Now.ToString("ss");
                string MaNhaCungCap = "N" + "" + min + "" + sec;
                ncc.MaNCC = MaNhaCungCap;
                ncc.TenNCC = tencungcap;
                ncc.DiaChi = diachi;
                ncc.SDT = sdt;
                db.tblNhaCungCaps.InsertOnSubmit(ncc);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var nhacungcap = db.tblNhaCungCaps.First(m => m.MaNCC == id);
            return View(nhacungcap);
        }
        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var ncc = db.tblNhaCungCaps.First(m => m.MaNCC == id);
            var tenncc = collection["TenNCC"];
            var diachi = collection["DiaChi"];
            var sdt = collection["SDT"];
            ncc.MaNCC = id;
            if (string.IsNullOrEmpty(tenncc))
            {
                ViewData["Loi"] = "Tên nhà cung cấp không được để trống";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi"] = "Phải nhập địa chỉ nhà cung cấp";
            }
            else if (string.IsNullOrEmpty(sdt))
            {
                ViewData["Loi"] = "Số điện thoại không được để trống";
            }
            else
            {
                ncc.TenNCC = tenncc;
                ncc.DiaChi = diachi;
                ncc.SDT = sdt;
                UpdateModel(ncc);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var ncc = db.tblNhaCungCaps.First(m => m.MaNCC == id);
            return View(ncc);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var ncc = db.tblNhaCungCaps.Where(m => m.MaNCC == id).First();
            db.tblNhaCungCaps.DeleteOnSubmit(ncc);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}