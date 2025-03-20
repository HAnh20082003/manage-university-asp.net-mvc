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
    public class NganhController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/Nganh/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        // GET: Admin/Nganh
        public ActionResult DanhSachNganhHoc()
        {
            if (!checkLogin("DanhSachNganhHoc"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var tb_Nganh = db.tb_Nganh.Include(t => t.tb_KhoaVien);
            return View(tb_Nganh.ToList());
        }

        public ActionResult ThemNganh()
        {
            ViewBag.MaKhoaVien = new SelectList(db.tb_KhoaVien, "ID", "TenKhoaVien");
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult ThemNganh([Bind(Include = "ID,MaNganh,TenNganh,MaKhoaVien,NamDaotao,Mota")] tb_Nganh tb_Nganh)
        {
            if (ModelState.IsValid)
            {
                db.tb_Nganh.Add(tb_Nganh);
                db.SaveChanges();
                Session["SuccessThemNganh"] = "Thêm ngành thành công";
                return RedirectToAction("DanhSachNganhHoc");
            }

            Session["ErrorThemNganh"] = "Thêm ngành thất bại";
            ViewBag.MaKhoaVien = new SelectList(db.tb_KhoaVien, "ID", "TenKhoaVien", tb_Nganh.MaKhoaVien);
            return RedirectToAction("DanhSachNganhHoc");
        }



        public ActionResult XoaNganh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Nganh tb_Nganh = db.tb_Nganh.Find(id);
            if (tb_Nganh == null)
            {
                return HttpNotFound();
            }
            return PartialView(tb_Nganh);
        }

  
        public ActionResult XacNhanXoaNganh(int id)
        {
            tb_Nganh tb_Nganh = db.tb_Nganh.Find(id);
            db.tb_Nganh.Remove(tb_Nganh);
            try
            {

                db.SaveChanges();
            }
            catch(Exception) { Session["Loi"] = "Không thể xóa ngành này"; }
            return RedirectToAction("DanhSachNganhHoc");
        }


        public ActionResult SuaThongTinNganh(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Nganh tb_Nganh = db.tb_Nganh.Find(id);
            if (tb_Nganh == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKhoaVien = new SelectList(db.tb_KhoaVien, "ID", "TenKhoaVien", tb_Nganh.MaKhoaVien);
           
            return PartialView(tb_Nganh);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaThongTinNganh([Bind(Include = "ID,MaNganh,TenNganh,MaKhoaVien,NamDaotao,Mota")] tb_Nganh tb_Nganh)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tb_Nganh).State = EntityState.Modified;
                db.SaveChanges();
                Session["SuccessSuaNganh"] = "Đã cập nhật thông tin ngành thành công";
                return RedirectToAction("DanhSachNganhHoc");
            }
            Session["ErrorSuaNganh"] = "Cập nhật thông tin ngành thất bại";
            ViewBag.MaKhoaVien = new SelectList(db.tb_KhoaVien, "ID", "MaKhoaVien", tb_Nganh.MaKhoaVien);
            return RedirectToAction("DanhSachNganhHoc");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
