using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongDaiHoc.Models;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class QuanLyTotNghiepController : Controller
    {
        QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        // GET: Admin/QuanLyTotNghiep
        public ActionResult DanhSachTotNghiep()
        {
            var dstn = db.tb_SinhVien.Where(n => n.TotNghiep);
            return View(dstn);
        }
    }
}