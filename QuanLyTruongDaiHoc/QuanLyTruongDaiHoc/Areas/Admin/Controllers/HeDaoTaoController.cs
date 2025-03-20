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
    public class HeDaoTaoController : Controller
    {
       
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/HeDaoTao/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        // GET: Admin/HeDaoTao
        public ActionResult XemHeDaoTao()
        {
            if (!checkLogin("XemHeDaoTao"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var dsKhoa = db.tb_KhoaVien.ToList();
            ViewBag.dsKhoa = dsKhoa;
            ViewBag.MaKhoaVien = new SelectList(db.tb_KhoaVien, "ID", "TenKhoaVien");
            var tb_Nganh = db.tb_Nganh.Include(t => t.tb_KhoaVien);
            return View(tb_Nganh.ToList());
        }
    }
}
