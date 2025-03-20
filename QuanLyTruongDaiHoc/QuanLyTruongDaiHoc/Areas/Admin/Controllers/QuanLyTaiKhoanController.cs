using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class QuanLyTaiKhoanController : Controller
    {
        // GET: Admin/QuanLyTaiKhoan
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        public static tb_QuanTri qt;
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/QuanLyTaiKhoan/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            if (qt == null)
            {
                tb_TaiKhoan tk = (tb_TaiKhoan)Session["TaikhoanQT"];
                qt = db.tb_QuanTri.SingleOrDefault(n => n.tb_TaiKhoan.ID == tk.ID);
            }
            return true;
        }
        public ActionResult DanhSachTaiKhoan()
        {
            if (!checkLogin("DanhSachTaiKhoan"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            return View();
        }

        public ActionResult TaiKhoanQT()
        {
            return PartialView(db.tb_TaiKhoan.Where(n => n.LoaiTaiKhoan == Init.TaiKhoanQT).ToList());
        }
        public ActionResult TaiKhoanGV()
        {
            return PartialView(db.tb_TaiKhoan.Where(n => n.LoaiTaiKhoan == Init.TaiKhoanGV).ToList());
        }
        public ActionResult TaiKhoanSV()
        {
            return PartialView(db.tb_TaiKhoan.Where(n => n.LoaiTaiKhoan == Init.TaiKhoanSV).ToList());
        }

        public ActionResult ChinhSuaTaiKhoan(int? id)
        {
            var tk = db.tb_TaiKhoan.SingleOrDefault(n => n.ID == id);
            return PartialView(tk);
        }
        
        [HttpPost]
        public ActionResult ChinhSuaTaiKhoan(FormCollection collection)
        {
            int? id = int.Parse(collection["hidden"]);
            string tendn = collection["Tendn"];
            string mk = collection["Matkhau"];
            var tk = db.tb_TaiKhoan.SingleOrDefault(n => n.ID == id);
            tk.TenDangNhap = tendn;
            tk.MatKhau = mk;
            db.Entry(tk).State = System.Data.Entity.EntityState.Modified;
            try
            {
                db.SaveChanges();
                Session["Success_ChinhSuaTaiKhoan"] = "Chỉnh sửa tài khoản thành công";
            }
            catch
            {
                Session["Error_ChinhSuaTaiKhoan"] = "Chỉnh sửa tài khoản thất bại";
            }
            return RedirectToAction("DanhSachTaiKhoan");
        }

        public ActionResult XoaTaiKhoan(int? id)
        {
            var tk = db.tb_TaiKhoan.SingleOrDefault(n => n.ID == id);
            return PartialView(tk);
        }

        public ActionResult XacNhanXoaTaiKhoa(int? id)
        {
            if (id == qt.tb_TaiKhoan.ID)
            {
                Session["Error_XoaTaiKhoan"] = "Không thể xoá tài khoản bạn đang sử dụng";
            }
            else
            {
                db.tb_TaiKhoan.Remove(db.tb_TaiKhoan.SingleOrDefault(n => n.ID == id));
                try
                {
                    db.SaveChanges();
                    Session["Success_XoaTaiKhoan"] = "Xoá tài khoản thành công";
                }
                catch
                {
                    Session["Error_XoaTaiKhoan"] = "Tài khoản đang được sử dụng không thể xoá";
                }
            }
            return RedirectToAction("DanhSachTaiKhoan");
        }
    }
}