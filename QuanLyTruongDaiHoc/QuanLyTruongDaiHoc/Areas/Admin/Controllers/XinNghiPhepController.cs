using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class XinNghiPhepController : Controller
    {
        // GET: Admin/XinNghiPhep
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/XinNghiPhep/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }

        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        public ActionResult XemDanhSachNghiPhep()
        {
            if (!checkLogin("XemDanhSachNghiPhep"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var ds = db.tb_DangKyNghiPhep.OrderBy(n => n.NgayGui);
            return View(ds);
        }

        public ActionResult XuLyDon(int? id)
        {
            var don = db.tb_DangKyNghiPhep.SingleOrDefault(n => n.ID == id);
            return PartialView(don);
        }
        
        [HttpPost]
        public ActionResult XuLyDon(FormCollection collection)
        {
            int? id = int.Parse(collection["id_don"]);
            var don = db.tb_DangKyNghiPhep.SingleOrDefault(n => n.ID == id);

            don.PhanHoi = collection["PhanHoi"];
            don.NgayDuyet = DateTime.Now;
            don.TrangThai = int.Parse(collection["TrangThai"]);

            db.Entry(don).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("XemDanhSachNghiPhep");
        }
    }
}