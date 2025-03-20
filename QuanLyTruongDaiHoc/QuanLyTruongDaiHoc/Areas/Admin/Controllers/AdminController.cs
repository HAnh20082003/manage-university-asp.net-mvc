using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        private static tb_QuanTri qt;
        // GET: Admin/Admin
        public ActionResult TrangChu()
        {
            Session["LoaiTK"] = Init.TaiKhoanQT;
            Session["PreviousURL"] = "~/Admin/Admin/TrangChu";
            return View();
        }

        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            Session["SuccessAccount"] = null;
            string tendn = collection["tendn"];
            string mk = collection["mk"];
            bool remember = collection["remember"] == "on";
            Session["tendn"] = tendn;
            Session["mk"] = mk;
            if (string.IsNullOrEmpty(tendn))
            {
                Session["Error_tendn"] = "Vui lòng nhập tên đăng nhập";
                return RedirectToAction("DangNhap");
            }
            if (string.IsNullOrEmpty(mk))
            {
                Session["Error_mk"] = "Vui lòng nhập mật khẩu";
                return RedirectToAction("DangNhap");
            }
            tb_TaiKhoan tk = db.tb_TaiKhoan.SingleOrDefault(n => n.TenDangNhap == tendn && n.MatKhau == mk);

            Session["TaikhoanQT"] = Session["TaikhoanGV"] = Session["TaikhoanSV"] = null;
            if (tk == null)
            {
                Session["Error_Account"] = "Tên đăng nhập hoặc mật khẩu không khớp";
            }
            else
            {
                if (tk.LoaiTaiKhoan != Init.TaiKhoanQT)
                {
                    Session["Error_Account"] = "Tên đăng nhập hoặc mật khẩu không khớp";
                }
                else
                {
                    qt = tk.tb_QuanTri.SingleOrDefault(n => n.ID_TaiKhoan == tk.ID);
                    Session["TaikhoanQT"] = tk;
                    return Redirect((string)Session["PreviousURL"]);
                }
            }
            return RedirectToAction("DangNhap");

        }
        public ActionResult QuenMatKhau()
        {
            return View();
        }

        [HttpPost]
        public ActionResult QuenMatKhau(string tenDN)
        {
            return RedirectToAction("DangNhap");
        }
        public ActionResult DoiMatKhau(int? id)
        {
            var doimk = db.tb_TaiKhoan.SingleOrDefault(n => n.ID == id);
            return View(doimk);
        }

        [HttpPost]
        public ActionResult DoiMatKhau(FormCollection collection)
        {
            int id = int.Parse(collection["id"]);
            string mkcu = collection["mkcu"];
            string mkmoi = collection["mkmoi"];
            var tk = db.tb_TaiKhoan.SingleOrDefault(n => n.ID == id);
            if (tk.MatKhau != mkcu)
            {
                Session["Error_DoiMatKhau"] = "Mật khẩu cũ không đúng";
            }
            else if (tk.MatKhau == mkmoi)
            {
                Session["Error_DoiMatKhau"] = "Mật khẩu mới của bạn chính là mật khẩu cũ";
            }
            else
            {
                tk.MatKhau = mkmoi;
                db.Entry(tk).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Session["Success_DoiMatKhau"] = "Đổi mật khẩu thành công";
                return Redirect((string)Session["PreviousURL"]);
            }
            Session["MKCu"] = mkcu;
            return RedirectToAction("DoiMatKhau", new { id = id });
        }




        public ActionResult DangXuat()
        {
            Session["TaikhoanQT"] = null;
            return Redirect((string)Session["PreviousURL"]);
        }

        public ActionResult ThongTinCaNhan()
        {
            return View();
        }
       
        public ActionResult AccountManager()
        {
            return View();
        }
        public ActionResult StudentsManager()
        {
            return View();
        }
        public ActionResult LecturersManager()
        {
            return View();
        }


    }
}