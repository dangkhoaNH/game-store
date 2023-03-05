using GameStore.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GameStore.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();

        // GET: Admin/Product
        public ActionResult Index(int? page)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            //return View(db.tblSanPhams.ToList());
            return View(db.tblSanPhams.ToList().OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            //Dua du lieu vao dropdownlist
            //Lay ds tu table loai san pham, sap xep tang dan theo ten loai, chon lay gia tri MaLoai,thì hien thị TenLoai
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(tblSanPham sanpham, HttpPostedFileBase fileupload)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            //Đưa du liệu vào dropdownload
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Kiểm tra đường dẫn file
            if(sanpham.TenSP == null)
            {
                ViewBag.ErrorName = "Vui lòng nhập tên sản phẩm";
                return View();
            }
            else if (sanpham.GiaTien == null || sanpham.GiaTien <= 0)
            {
                ViewBag.ErrorPrice = "Giá tiền phải lớn hơn 0";
                return View();
            }
            else if (sanpham.SoLuong <= 0 || sanpham.SoLuong == null)
            {
                ViewBag.ErrorQuantity = "Số lượng phải lớn hơn 0";
                return View();
            }
            else if (sanpham.NgayCapNhat == null)
            {
                ViewBag.ErrorDate = "Chọn ngày cập nhật";
                return View();
            }
            else if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh";
                return View();
            }
            //Thêm vào CSDL
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file, lưu ý bổ sung thư viện using System.IO;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/Content/Hinhsanpham"), fileName);
                    //Kiểm tra hình ảnh tồn tại chưa?
                    string min = DateTime.Now.ToString("mm");
                    string sec = DateTime.Now.ToString("ss");
                    string MaSanPham = "S" + "" + min + "" + sec;
                    sanpham.MaSP = MaSanPham;
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //Lưu hình ảnh vào đường dẫn 
                        fileupload.SaveAs(path);
                    }
                    sanpham.HinhAnh = fileName;
                    //Lưu vào CSDL
                    db.tblSanPhams.InsertOnSubmit(sanpham);
                    db.SubmitChanges();
                }
                return RedirectToAction("Index");
            }
        }

        public ActionResult Detail(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            //Lấy ra đối tượng sản phẩm theo mã 
            tblSanPham sanpham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpGet]
        public ActionResult Delete(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            //Lấy ra đối tượng sản phẩm cần xóa theo mã 
            tblSanPham sanpham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sanpham);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            tblSanPham sanpham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sanpham.MaSP;
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.tblSanPhams.DeleteOnSubmit(sanpham);
            db.SubmitChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");

            //lay ra doi tuong san pham theo ma
            tblSanPham sanpham = db.tblSanPhams.SingleOrDefault(n => n.MaSP == id);
            if (sanpham == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Dua du lieu vao Dropdownlist
            //Lay ds tu table loai san pham, sắp xếp tang dan theo ten loai san pham, chọn lấy giá trị hiện ra tên 
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");

            return View(sanpham);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(tblSanPham sanpham, HttpPostedFileBase fileupload)
        {
            if (Session["Taikhoanadmin"] == null) return RedirectToAction("Login", "Default");
            ViewBag.MaLoai = new SelectList(db.tblLoaiSanPhams.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaTH = new SelectList(db.tblThuongHieus.ToList().OrderBy(n => n.TenTH), "MaTH", "TenTH");
            ViewBag.MaNCC = new SelectList(db.tblNhaCungCaps.ToList().OrderBy(n => n.TenNCC), "MaNCC", "TenNCC");
            //Kiem tra duong dan file
            if (sanpham.TenSP == null)
            {
                ViewBag.ErrorName = "Vui lòng nhập tên sản phẩm";
                return View();
            }
            else if (sanpham.GiaTien == null || sanpham.GiaTien <= 0)
            {
                ViewBag.ErrorPrice = "Giá tiền phải lớn hơn 0";
                return View();
            }
            else if (sanpham.SoLuong <= 0 || sanpham.SoLuong == null)
            {
                ViewBag.ErrorQuantity = "Số lượng phải lớn hơn 0";
                return View();
            }
            else if (sanpham.NgayCapNhat == null)
            {
                ViewBag.ErrorDate = "Chọn ngày cập nhật";
                return View();
            }
            else if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn hình ảnh";
                return View();
            }
            //Them vao csdl
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file, luu y bo sung thu vien using system.io;
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan cua file
                    var path = Path.Combine(Server.MapPath("~/Content/Hinhsanpham"), fileName);
                    //Kiem tra hinh anh ton tai chua
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        //luu hinh anh vao duong dan
                        fileupload.SaveAs(path);
                    }
                    sanpham.HinhAnh = fileName;
                    //luu vao csdl

                    var sanpham1 = db.tblSanPhams.FirstOrDefault(p => p.MaSP.Equals(sanpham.MaSP));
                    sanpham1.GiaTien = sanpham.GiaTien;
                    sanpham1.HinhAnh = fileName;
                    sanpham1.MaLoai = sanpham.MaLoai;
                    sanpham1.MaNCC = sanpham.MaNCC;
                    sanpham1.MaSP = sanpham.MaSP;
                    sanpham1.MaTH = sanpham.MaTH;
                    sanpham.MoTa = sanpham.MoTa;
                    sanpham.NgayCapNhat = sanpham.NgayCapNhat;

                    UpdateModel(sanpham1);
                    db.SubmitChanges();
                }
                return RedirectToAction("Index");
            }
        }
    }
}