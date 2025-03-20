using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongDaiHoc.Models;
using PagedList;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class ThongBaoController : Controller
    {
        // GET: Admin/ThongBao

        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/ThongBao/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }

        public ActionResult ThongBaoDoiTuong(int? page, int? doiTuong)
        {
            int pageSize = 3;
            int pageNumber = page ?? 1;
            var notification = db.tb_ThongBao.Where(n=> n.DoiTuongNhanTB == doiTuong).OrderByDescending(n => n.NgayGui).Take(pageSize);
            ViewBag.StrDoiTuong = Init.getStrThongBao(doiTuong).ToLower();
            ViewBag.DoiTuong = doiTuong;
            return PartialView(notification.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult ThongBao()
        {
            if (!checkLogin("ThongBao"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var newest = db.tb_ThongBao.OrderByDescending(n => n.NgayGui).FirstOrDefault();
            return View(newest);
        }

        public ActionResult ThemThongBao()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ThemThongBao(FormCollection collection)
        {
            int doiTuong = int.Parse(collection["Doituong"]);
            string tieuDe = collection["Tieude"];
            string noiDung = collection["Noidung"];

            tb_ThongBao tb = new tb_ThongBao();
            tb.ID_QuanTri = ((tb_TaiKhoan)Session["TaikhoanQT"]).tb_QuanTri.SingleOrDefault().ID;
            tb.NgayGui = DateTime.Now;
            tb.TieuDe = tieuDe;
            tb.NoiDung = noiDung;
            tb.DoiTuongNhanTB = doiTuong;
            db.tb_ThongBao.Add(tb);
            db.SaveChanges();
            Session["SuccessThemThongBao"] = "Đã gửi thông báo đến " + Init.getStrThongBao(doiTuong).ToLower();
            return RedirectToAction("ThongBao");
        }

        public ActionResult ChinhSuaThongBao(int? id)
        {
            var tb = db.tb_ThongBao.SingleOrDefault(n => n.ID == id);
            return PartialView(tb);
        }

        [HttpPost]
        public ActionResult ChinhSuaThongBao(FormCollection collection)
        {
            if (string.IsNullOrEmpty(collection["Tieude"]) || string.IsNullOrEmpty(collection["Noidung"])) {
                Session["ErrorCapNhatThongBao"] = "Thông báo không được cập nhật vì không đầy đủ";
            }
            else
            {
                int? id = int.Parse(collection["hidden"]);
                var tb = db.tb_ThongBao.SingleOrDefault(n => n.ID == id);
                tb.ID_QuanTri = ((tb_TaiKhoan)Session["TaikhoanQT"]).tb_QuanTri.SingleOrDefault().ID;
                tb.NgayGui = DateTime.Now;
                tb.TieuDe = collection["Tieude"];
                tb.NoiDung = collection["Noidung"];
                tb.DoiTuongNhanTB = int.Parse(collection["Doituong"]);
                db.Entry(tb).State = System.Data.Entity.EntityState.Modified;

                db.SaveChanges();
                Session["SuccessCapNhatThongBao"] = "Đã cập nhật thông báo " + id;
            }
            return RedirectToAction("ThongBao", "ThongBao");
        }

        [HttpPost]
        public ActionResult XoaThongBao(FormCollection collection)
        {
            var tb = db.tb_ThongBao;
            if (tb != null)
            {
                foreach (var item in tb)
                {
                    if (string.IsNullOrEmpty(collection["hidden-" + item.ID]))
                    {
                        db.tb_ThongBao.Remove(item);
                    }
                }
                Session["SuccesssXoaThongBao"] = "Lưu thay đổi thành công";
                db.SaveChanges();
            }
            return RedirectToAction("ThongBao");
        }
    }
}