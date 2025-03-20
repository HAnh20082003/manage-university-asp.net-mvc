using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTruongDaiHoc.Controllers
{
    public class AccountController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        // GET: Account
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
                if(tk.LoaiTaiKhoan != Session["LoaiTK"].ToString())
                {
                    Session["Error_Account"] = "Tên đăng nhập hoặc mật khẩu không khớp";
                }
                else
                {
                    if (tk.LoaiTaiKhoan == Init.TaiKhoanGV)
                    {
                        Session["TaikhoanGV"] = tk;
                    }
                    else
                    {
                        Session["TaikhoanSV"] = tk;
                    }
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
            if (Session["LoaiTK"].ToString() == Init.TaiKhoanGV)
            {
                Session["TaikhoanGV"] = null;
            }
            else
            {
                Session["TaikhoanSV"] = null;
            }
            return Redirect((string)Session["PreviousURL"]);
        }
    }
}