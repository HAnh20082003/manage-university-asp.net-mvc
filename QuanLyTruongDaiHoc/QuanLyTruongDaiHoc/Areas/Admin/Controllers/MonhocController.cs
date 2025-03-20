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
    public class MonhocController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/Monhoc/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        // GET: Admin/Monhoc
        public ActionResult DanhSachMonHoc()
        {
            if (!checkLogin("DanhSachMonHoc"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var tb_Monhoc = db.tb_Monhoc.Include(t => t.tb_Nganh);
            return View(tb_Monhoc.ToList());
        }

        // GET: Admin/Monhoc/Details/5
        public ActionResult ThemMonHoc()
        {
            var nganh = db.tb_Nganh;
            if (nganh != null)
            {
                ViewBag.ChuyenNganh = nganh.ToList();
            }
            return PartialView(new tb_Monhoc());
        }

        [HttpPost]
        public ActionResult ThemMonHoc(FormCollection collection)
        {
            tb_Monhoc mh = new tb_Monhoc();
            mh.ChuyenNganh = int.Parse(collection["Chuyennganh"]);
            mh.LoaiMonHoc = int.Parse(collection["Loaimon"]);
            mh.MaMonHoc = collection["Mamh"];
            mh.SoTC = int.Parse(collection["Sotc"]);
            mh.SoTiet = int.Parse(collection["Sotiet"]);
            mh.TenMonHoc = collection["Tenmh"];
            db.tb_Monhoc.Add(mh);
            db.SaveChanges();
            return RedirectToAction("DanhSachMonHoc");
        }



        public ActionResult SuaThongTinMonHoc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Monhoc tb_Monhoc = db.tb_Monhoc.Find(id);
            if (tb_Monhoc == null)
            {
                return HttpNotFound();
            }
            var nganh = db.tb_Nganh;
            if (nganh != null)
            {
                ViewBag.ChuyenNganh = nganh.ToList();
            }
            return PartialView(tb_Monhoc);
        }



        [HttpPost]
        public ActionResult SuaThongTinMonHoc(FormCollection collection)
        {
            int? id = int.Parse(collection["hidden"]);
            tb_Monhoc mh = db.tb_Monhoc.SingleOrDefault(n => n.ID == id);
            mh.MaMonHoc = collection["Mamh"];
            mh.TenMonHoc = collection["Tenmh"];
            mh.ChuyenNganh = int.Parse(collection["Chuyennganh"]);
            mh.LoaiMonHoc = int.Parse(collection["Loaimon"]);
            mh.SoTC = int.Parse(collection["Sotc"]);
            mh.SoTiet = int.Parse(collection["Sotiet"]);
            db.Entry(mh).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachMonHoc");
        }


        public ActionResult XoaMonHoc(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_Monhoc tb_Monhoc = db.tb_Monhoc.Find(id);
            if (tb_Monhoc == null)
            {
                return HttpNotFound();
            }
            return PartialView(tb_Monhoc);
        }

        public ActionResult XacNhanXoaMon(int id)
        {
            tb_Monhoc tb_Monhoc = db.tb_Monhoc.Find(id);
            db.tb_Monhoc.Remove(tb_Monhoc);
            db.SaveChanges();
            return RedirectToAction("DanhSachMonHoc");
        }


    }
}
