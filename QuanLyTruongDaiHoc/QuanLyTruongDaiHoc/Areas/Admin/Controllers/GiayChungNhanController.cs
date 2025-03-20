
using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class GiayChungNhanController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        // GET: Admin/GiayChungNhan
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/GiayChungNhan/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        public ActionResult XemDanhSachYeuCau()
        {
            if (!checkLogin("XemDanhSachYeuCau"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var giaychungnhan = db.tb_YeuCauGiayChungNhan.Where(n => n.TrangThai == Init.DoiDuyet).OrderBy(n => n.NgayYeuCau);

            return View(giaychungnhan);
        }
        public ActionResult XuLyDon(int? id)
        {
            var giaychungnhan = db.tb_YeuCauGiayChungNhan.SingleOrDefault(n => n.ID == id);
            return View(giaychungnhan);
        }

        [HttpPost]
        public ActionResult XuLyDon(FormCollection collection)
        {
            
            int trangThai = int.Parse(collection["TrangThai"]);
            int id = int.Parse(collection["id_giaychungnhan"]);
            var giaychungnhan = db.tb_YeuCauGiayChungNhan.SingleOrDefault(n => n.ID == id);

            giaychungnhan.TrangThai = trangThai;

            tb_TaiKhoan tk = (tb_TaiKhoan)Session["TaikhoanQT"];
            giaychungnhan.ID_QuanTri = tk.tb_QuanTri.SingleOrDefault().ID;
            giaychungnhan.NgayDuyet = DateTime.Now;
            giaychungnhan.PhanHoi = collection["Phanhoi"];

            db.Entry(giaychungnhan).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("XemDanhSachYeuCau");
        }


        public ActionResult XemYeuCauDaDuyet()
        {
            var giaychungnhan = db.tb_YeuCauGiayChungNhan.Where(n => n.TrangThai == Init.DaDuyet).OrderByDescending(n => n.NgayDuyet);
            return View(giaychungnhan);
        }

        public ActionResult XemYeuCauTuChoi()
        {
            var giaychungnhan = db.tb_YeuCauGiayChungNhan.Where(n => n.TrangThai == Init.TuChoiDuyet).OrderByDescending(n => n.NgayDuyet);
            return View(giaychungnhan);
        }
    }
}