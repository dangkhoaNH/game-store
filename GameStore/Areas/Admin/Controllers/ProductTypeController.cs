using GameStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class ProductTypeController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        // GET: Admin/ProductType
        public ActionResult Index()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            return View(db.tblLoaiSanPhams.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection collection, tblLoaiSanPham lsp)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            //Tạo biến loaisanpham và gán giá trị của người dùng nhập vào
            var loaisp = collection["TenLoai"];
            //nếu loaisanpham có giá trị == null (để trống)
            if (string.IsNullOrEmpty(loaisp))
            {
                ViewBag.Error = "Tên loại sản phẩm không được để trống";
            }
            else
            {
                string min = DateTime.Now.ToString("mm");
                string sec = DateTime.Now.ToString("ss");
                string MaLoaiSanPham = "L" + "" + min + "" + sec;
                lsp.MaLoai = MaLoaiSanPham;
                lsp.TenLoai = loaisp;
                db.tblLoaiSanPhams.InsertOnSubmit(lsp);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Create();
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var loaisp = db.tblLoaiSanPhams.First(m => m.MaLoai == id);
            return View(loaisp);
        }

        [HttpPost]
        public ActionResult Edit(string id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var loaisp = db.tblLoaiSanPhams.First(m => m.MaLoai == id);
            var lsp = collection["TenLoai"];
            loaisp.MaLoai = id;
            if (string.IsNullOrEmpty(lsp))
            {
                ViewBag.Error = "Loại sản phẩm  không được để trống";
            }
            else
            {
                loaisp.TenLoai = lsp;
                UpdateModel(lsp);
                db.SubmitChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(id);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var loaisp = db.tblLoaiSanPhams.First(m => m.MaLoai == id);
            return View(loaisp);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            var loaisp = db.tblLoaiSanPhams.Where(m => m.MaLoai == id).First();
            db.tblLoaiSanPhams.DeleteOnSubmit(loaisp);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }
    }
}