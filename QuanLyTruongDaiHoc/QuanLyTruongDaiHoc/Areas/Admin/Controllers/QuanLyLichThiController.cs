using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class QuanLyLichThiController : Controller
    {
        QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        // GET: Admin/QuanLyLichThi
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/QuanLyLichThi/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        public ActionResult DanhSachKhoaHocLichThi()
        {
            if (!checkLogin("DanhSachKhoaHocLichThi"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var kh = db.tb_KhoaHoc.ToList();
            return View(kh);
        }

        public ActionResult XemLichThi(int? id)
        {
            var kh = db.tb_KhoaHoc.SingleOrDefault(n => n.ID == id);
            ViewBag.KH = kh;
            var lichThi = kh.tb_ThongTinNgayThi;
            ViewBag.DSSV = kh.tb_DangKyMonHoc.ToList();
            return View(lichThi);
        }

        [HttpPost]
        public ActionResult LuuThayDoi(FormCollection collection)
        {
            int id = int.Parse(collection["hidden"]);

            var kh = db.tb_KhoaHoc.SingleOrDefault(n => n.ID == id);

            var lt = kh.tb_ThongTinNgayThi.ToList();

            foreach (var item in lt)
            {
                if (string.IsNullOrEmpty(collection["lt-" + item.ID]))
                {
                    db.tb_ThongTinNgayThi.Remove(item);
                }
            }
            db.SaveChanges();
            Session["SuccessLuuThayDoi"] = "Đã lưu thay đổi thành công";

            return RedirectToAction("XemLichThi", new { id = id });
        }

        public ActionResult ThemLichKhoaHoc(int? id)
        {
            var kh = db.tb_KhoaHoc.SingleOrDefault(n => n.ID == id);
            ViewBag.ID_KhoaHoc = kh.ID;
            var dsgv = db.tb_GiangVien.Where(n => n.tb_KhoaHoc.FirstOrDefault(s => s.ID == kh.ID) == null);
            ViewBag.DSPH = db.tb_PhongHoc.Where(n => n.TrangThai == true).ToList();
            return PartialView(dsgv);
        }

        [HttpPost]
        public ActionResult ThemLichKhoaHoc(FormCollection collection)
        {
            int id = int.Parse(collection["hidden"]);
            DateTime ngaythi = DateTime.Parse(collection["Ngaythi"]);
            int idph = int.Parse(collection["Phongthi"]);
            string giobatdau = collection["Giobatdau"];
            int thoiluong = int.Parse(collection["Thoiluong"]);
            int id_gt1 = int.Parse(collection["Giamthi1"]);
            int id_gt2 = int.Parse(collection["Giamthi2"]);

            tb_ThongTinNgayThi ngt = new tb_ThongTinNgayThi();
            ngt.GioBatDau = giobatdau;
            ngt.ID_GiamThi1 = id_gt1;
            ngt.ID_GiamThi2 = id_gt2;
            ngt.ID_KhoaHoc = id;
            ngt.NgayThi = ngaythi;
            ngt.ThoiLuong = thoiluong;
            ngt.PhongThi = idph;
            db.tb_ThongTinNgayThi.Add(ngt);
            db.SaveChanges();

            Session["SuccessThemLichKhoaHoc"] = "Đã thêm lịch thành công";
            return RedirectToAction("XemLichThi", new { id = id });
        }

        public ActionResult XoaLichThi(int? id)
        {
            var lt = db.tb_ThongTinNgayThi.SingleOrDefault(n => n.ID == id);
            return PartialView(lt);
        }


        public ActionResult XacNhanXoaLichThi(int? id)
        {
            var lt = db.tb_ThongTinNgayThi.SingleOrDefault(n => n.ID == id);
            int? id_kh = lt.ID_KhoaHoc;
            db.tb_ThongTinNgayThi.Remove(lt);
            try
            {
                db.SaveChanges();
                Session["SuccessXoaLichThi"] = "Xoá lịch thi thất bại vì lịch đang được sử dụng";
            }
            catch
            {
                Session["ErrorXoaLichThi"] = "Xoá lịch thi thất bại vì lịch đang được sử dụng";
            }

            return RedirectToAction("XemLichThi", new { id = id_kh });
        }


        public ActionResult XemPhongThi(int? id)
        {
            var lt = db.tb_ThongTinNgayThi.SingleOrDefault(n => n.ID == id);
            var dkmh = lt.tb_KhoaHoc.tb_DangKyMonHoc.ToList();
            foreach (var item in dkmh)
            {
                if (db.tb_ChiTietLichThi.SingleOrDefault(n => n.ID_SinhVien == item.ID_SinhVien && n.ID_ThongTinNgayThi != id) != null)
                {
                    dkmh.Remove(item);
                }
            }
            ViewBag.LT = lt;
            ViewBag.CTLT = lt.tb_ChiTietLichThi.ToList();
            ViewBag.PT = db.tb_PhongHoc;
            return View(dkmh);
        }

        public ActionResult ChinhSuaLichThi(int? id)
        {
            var lt = db.tb_ThongTinNgayThi.SingleOrDefault(n => n.ID == id);
            ViewBag.GV = db.tb_GiangVien.ToList();
            ViewBag.DSPH = db.tb_PhongHoc.ToList();
            return PartialView(lt);
        }

        [HttpPost]
        public ActionResult ChinhSuaLichThi(FormCollection collection)
        {
            var id = int.Parse(collection["hidden"]);
            var lt = db.tb_ThongTinNgayThi.SingleOrDefault(n => n.ID == id);

            lt.ID_GiamThi1 = int.Parse(collection["GiamThi1"]);
            lt.ID_GiamThi2 = int.Parse(collection["GiamThi2"]);
            lt.NgayThi = DateTime.Parse(collection["Ngaythi"]);
            lt.PhongThi = int.Parse(collection["Phongthi"]);
            lt.GioBatDau = collection["Giobatdau"];
            lt.ThoiLuong = int.Parse(collection["Thoiluong"]);
            db.Entry(lt).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            Session["SuccessChinhSuaLichThi"] = "Đã cập nhật lịch thi";
            return RedirectToAction("XemPhongThi", new { id = id });
        }

        public ActionResult DanhSachSinhVienDangKy(int? id)
        {
            ViewBag.ID_PT = id;
            var lt = db.tb_ThongTinNgayThi.SingleOrDefault(n => n.ID == id);
            var dssvdk = db.tb_DangKyMonHoc.Where(n => n.ID_KhoaHoc == lt.ID_KhoaHoc);
            ViewBag.CTLT = lt.tb_ChiTietLichThi.ToList();
            return PartialView(dssvdk);
        }

        public ActionResult SinhVienPhongThi(int? id)
        {
            var dssvdk = db.tb_ChiTietLichThi.Where(n => n.ID_ThongTinNgayThi == id);
            return PartialView(dssvdk);
        }

        public ActionResult ThemSVPhongThi(int? id, int? idlt)
        {
            var dkm = db.tb_DangKyMonHoc.SingleOrDefault(n => n.ID == id);
            var lt = db.tb_ThongTinNgayThi.SingleOrDefault(n => n.ID == idlt);
            var ctlt = db.tb_ChiTietLichThi.SingleOrDefault(n => n.ID_ThongTinNgayThi != idlt && n.ID_SinhVien == dkm.ID_SinhVien && n.tb_ThongTinNgayThi.ID_KhoaHoc == lt.ID_KhoaHoc);

            if (ctlt != null)
            {
                db.tb_ChiTietLichThi.Remove(ctlt);
                db.SaveChanges();
            }
            tb_ChiTietLichThi ct = new tb_ChiTietLichThi();
            ct.ID_SinhVien = dkm.ID_SinhVien;
            ct.ID_ThongTinNgayThi = lt.ID;
            db.tb_ChiTietLichThi.Add(ct);
            db.SaveChanges();
            return RedirectToAction("SinhVienPhongThi", new { id = idlt });

        }
        public ActionResult LoaiBoSvPhongThi2(int? id)
        {
            var dkm = db.tb_DangKyMonHoc.SingleOrDefault(n => n.ID == id);

            var ttnt = dkm.tb_KhoaHoc.tb_ThongTinNgayThi;
            int? id_lt = null;
            foreach (var tt in ttnt)
            {
                var ct = tt.tb_ChiTietLichThi.SingleOrDefault(n => n.ID_SinhVien == dkm.ID_SinhVien);
                if (ct != null)
                {
                    id_lt = ct.ID_ThongTinNgayThi;
                    db.tb_ChiTietLichThi.Remove(ct);
                    db.SaveChanges();
                    break;
                }
            }
            return RedirectToAction("SinhVienPhongThi", new { id = id_lt });
        }

        public ActionResult LoaiBoSvPhongThi(int? id)
        {
            var dkm = db.tb_DangKyMonHoc.SingleOrDefault(n => n.ID == id);

            var ttnt = dkm.tb_KhoaHoc.tb_ThongTinNgayThi;
            int? id_lt = null;
            foreach (var tt in ttnt)
            {
                var ct = tt.tb_ChiTietLichThi.SingleOrDefault(n => n.ID_SinhVien == dkm.ID_SinhVien);
                if (ct != null)
                {
                    id_lt = ct.ID_ThongTinNgayThi;
                    db.tb_ChiTietLichThi.Remove(ct);
                    db.SaveChanges();
                    break;
                }
            }
            return RedirectToAction("DanhSachSinhVienDangKy", new { id = id_lt });
        }
    }
}