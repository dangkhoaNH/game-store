using GameStore.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace GameStore.Controllers
{
    public class NguoidungController : Controller
    {
        dbQLBanGameDataContext db = new dbQLBanGameDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Dangky()
        {
            return View();
        }
        //POST: Hàm Dangky(post) nhận dữ liệu từ trang Dangky và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, tblKhachHang kh)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var hoten = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var nhaplaimatkhau = collection["Nhaplaimatkhau"];
            var email = collection["Email"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            if (string.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Họ tên khách hàng không được để trống";
            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["Loi2"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi3"] = "Mật khẩu không được để trống";
            }
            else if (string.IsNullOrEmpty(nhaplaimatkhau))
            {
                ViewData["Loi4"] = "Phải nhập lại mật khẩu";
            }
            else if (string.IsNullOrEmpty(email))
            {
                ViewData["loi5"] = "Phải nhập email";
            }
            else if (string.IsNullOrEmpty(diachi))
            {
                ViewData["Loi6"] = "Địa chỉ không được bỏ trống";
            }
            else if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi7"] = "Phải nhập số điện thoại";
            }
            else
            {
                //gán giá trị cho đối tượng được tạo mới (kh)
                string min = DateTime.Now.ToString("mm");
                string sec = DateTime.Now.ToString("ss");
                string MaKhachHang = "K" + "" + min + "" + sec;
                kh.MaKH = MaKhachHang;
                kh.TenKH = hoten;
                kh.Username = tendn;
                kh.Password = matkhau;
                kh.Email = email;
                kh.DiaChi = diachi;
                kh.SDT = dienthoai;
                db.tblKhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        public ActionResult Dangnhap(FormCollection collection)
        {
            //Gán các giá trị người dùng nhập liệu cho các biến
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (string.IsNullOrEmpty(tendn))
            {
                ViewData["errorUsername"] = "Phải nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["errorPassword"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đôi tượng được tạo mới (kh)
                tblKhachHang kh = db.tblKhachHangs.SingleOrDefault(n => n.Username == tendn && n.Password == matkhau);
                if (kh == null) {
                    ViewData["errorAccount"] = "Sai tên đăng nhập hoặc mật khẩu";
                }
                else if (kh != null)
                {
                    Session["Username"] = kh;
                    return RedirectToAction("Index", "GameProduct");
                }
            }
            return View();
        }
        public ActionResult Dangxuat()
        {
            Session.Remove("Username");
            return RedirectToAction("Index", "GameProduct");
        }
    }
}