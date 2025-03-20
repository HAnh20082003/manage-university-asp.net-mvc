using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongDaiHoc.Models;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class PhongHocController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/PhongHoc/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        // GET: Admin/PhongHoc
        public ActionResult DanhSachPhongHoc()
        {
            if (!checkLogin("DanhSachPhongHoc"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var loaiphong = db.tb_LoaiPhong;
            if (loaiphong != null)
            {
                ViewBag.LoaiPhong = loaiphong.ToList();
            }
            return View(db.tb_PhongHoc.ToList());
        }

        public ActionResult ChiTietPhongHoc(int id)
        {
            var phong = db.tb_PhongHoc.Find(id);
            return PartialView(phong);
        }


        [HttpPost]
        public ActionResult ChiTietPhongHoc(FormCollection collection)
        {
            var maPhongHoc = collection["MaPhongHoc"];
            var viTri = collection["ViTri"];
            var loaiPhonghoc = int.Parse(collection["LoaiPhongHoc"]);
            var sucChua = collection["SucChua"];
            var trangThai = collection["TrangThai"];
            int id = int.Parse(collection["ID"]);

            tb_PhongHoc phong = db.tb_PhongHoc.Find(id);
            if (phong != null)
            {
                phong.MaPhongHoc = maPhongHoc;
                phong.ViTri = viTri;
                phong.ID_LoaiPhong = loaiPhonghoc;
                phong.TrangThai = bool.Parse(trangThai);
                phong.SucChua = int.Parse(sucChua);

                db.Entry(phong).State = EntityState.Modified;
                db.SaveChanges();
                Session["SuaPhong"] = "Sửa phòng "+phong.MaPhongHoc+" thành công";

            }
            return RedirectToAction("DanhSachPhongHoc");
        }


        public ActionResult ThemPhong()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ThemPhong(FormCollection collection)
        {
            var maPhongHoc = collection["MaPhongHoc"];
            var viTri = collection["ViTri"];
            var loaiPhonghoc = int.Parse(collection["LoaiPhongHoc"]);
            var sucChua = collection["SucChua"];
            var trangThai = collection["TrangThai"];

            tb_PhongHoc phong = new tb_PhongHoc();
            phong.MaPhongHoc = maPhongHoc;
            phong.ViTri = viTri;
            phong.ID_LoaiPhong = loaiPhonghoc;
            phong.TrangThai = bool.Parse(trangThai);
            phong.SucChua = int.Parse(sucChua);

            db.tb_PhongHoc.Add(phong);

            try
            {
                db.SaveChanges();
                Session["ThemPhong"] = "Thêm phòng " + phong.MaPhongHoc + " thành công";
            }
            catch (Exception)
            {
                Session["LoiThemPhong"] = "Không thể thêm phòng";
            }


            return RedirectToAction("DanhSachPhongHoc");
        }

        public ActionResult XoaPhong(int id)
        {
            var phong = db.tb_PhongHoc.Find(id);
            return PartialView(phong);
        }


        public ActionResult XacNhanXoaPhong(int id)
        {
            tb_PhongHoc phong = db.tb_PhongHoc.Find(id);
            string maPhong = phong.MaPhongHoc;
            db.tb_PhongHoc.Remove(phong);
            try
            {
                db.SaveChanges();
                Session["XoaPhong"] = "Xóa phòng " + maPhong + " thành công";
            }
            catch
            {
                Session["LoiXoaPhong"] = "Không thể xóa phòng " + maPhong;
            }

            return RedirectToAction("DanhSachPhongHoc");
        }

        public ActionResult QuanLyLoaiPhong()
        {
            var loaiPhong = db.tb_LoaiPhong;
            if (loaiPhong != null)
            {
                return View(loaiPhong.ToList());
            }
            return View(new List<tb_LoaiPhong>());
        }

        public ActionResult ThemLoaiPhong()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ThemLoaiPhong(FormCollection collection)
        {
            var tenloai = collection["Tenloai"];
            tb_LoaiPhong lp = new tb_LoaiPhong();
            lp.Ten = tenloai;
            db.tb_LoaiPhong.Add(lp);
            db.SaveChanges();
            Session["SuccessThemLoaiPhong"] = "Thêm loại phòng " + tenloai + " thành công";
            return RedirectToAction("QuanLyLoaiPhong");
        }
        public ActionResult XoaLoaiPhong(int id)
        {
            var lp = db.tb_LoaiPhong.Find(id);
            return PartialView(lp);
        }


        public ActionResult XacNhanXoaLoaiPhong(int id)
        {
            tb_LoaiPhong lp = db.tb_LoaiPhong.Find(id);
            db.tb_LoaiPhong.Remove(lp);
            try
            {
                db.SaveChanges();
                Session["SuccessXoaLoaiPhong"] = "Xóa loại phòng " + lp.Ten.ToLower() + " thành công";
            }
            catch
            {
                Session["ErrorXoaLoaiPhong"] = "Không thể xóa loại phòng " + lp.Ten.ToLower() + " vì đang được sử dụng";
            }
            return RedirectToAction("QuanLyLoaiPhong");
        }


    }
}
