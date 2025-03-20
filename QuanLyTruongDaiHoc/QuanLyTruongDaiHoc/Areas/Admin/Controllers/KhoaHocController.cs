using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuanLyTruongDaiHoc.Models;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class KhoaHocController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        // GET: Admin/KhoaHoc
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/KhoaHoc/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        public ActionResult DanhSachKhoaHoc()
        {
            if (!checkLogin("DanhSachKhoaHoc"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var tb_KhoaHoc = db.tb_KhoaHoc.Include(t => t.tb_GiangVien).Include(t => t.tb_HeDaoTao).Include(t => t.tb_Monhoc);
            return View(tb_KhoaHoc.ToList());
        }
        public ActionResult ThemKhoaHoc()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult ThemKhoaHoc(FormCollection fields)
        {
            var HeDaoTao = int.Parse(fields["HeDaoTao"]);
            var Nganh = int.Parse(fields["Nganh"]);
            var MonHoc = int.Parse(fields["MonHoc"]);
            var GiangVien = int.Parse(fields["GiangVien"]);

            var SoLuong = int.Parse(fields["SoLuong"]);
            var trangthai = int.Parse(fields["TrangThai"]);


            tb_KhoaHoc kh = new tb_KhoaHoc();

            tb_HeDaoTao hdt = db.tb_HeDaoTao.Find(HeDaoTao);
            tb_Monhoc mh = db.tb_Monhoc.Find(MonHoc);

            int stt = db.tb_KhoaHoc.Where(s => s.ID_MonHoc == MonHoc).Count();

            if (mh.ChuyenNganh != null)
            {
                if (mh.LoaiMonHoc == Init.LyThuyet)
                {
                    kh.MaKhoaHoc = mh.tb_Nganh.tb_KhoaVien.MaKhoaVien + "." + hdt.MaHeDaoTao + "." + "LT." + (stt + 1).ToString("00");
                }
                else
                {
                    kh.MaKhoaHoc = mh.tb_Nganh.tb_KhoaVien.MaKhoaVien + "." + hdt.MaHeDaoTao + "." + "TH." + (stt + 1).ToString("00");
                }
            }
            else
            {
                kh.MaKhoaHoc = "HPC." + hdt.MaHeDaoTao + "." + (stt + 1).ToString("00");
            }
            kh.ID_MonHoc = MonHoc;
            kh.ID_GiangVien = GiangVien;
            kh.ID_HeDaoTao = HeDaoTao;
            kh.SoLuongYeuCau = SoLuong;
            kh.NgayMo = DateTime.Now;
            kh.TrangThai = trangthai;

            db.tb_KhoaHoc.Add(kh);

            try
            {
                db.SaveChanges();
                Session["SuccessThemKhoaHoc"] = "Thêm khoá học thành công";
            }
            catch
            {
                Session["ErrorThemKhoaHoc"] = "Thêm khoá học thất bại";
            }
            return RedirectToAction("DanhSachKhoaHoc");
        }


        public ActionResult ThemKhoaHocTheoNganh(int? Nganh, int? MonHoc)
        {
            ///Thêm chọn môn học sẽ hiện dropdownlist giảng viên có thể dạy
            ViewBag.HeDaoTao = new SelectList(db.tb_HeDaoTao, "ID", "TenHeDaoTao", db.tb_HeDaoTao);
            if (Nganh != null)
            {
                List<tb_GiangVien> dsGiangVien = new List<tb_GiangVien>();
                //ko hiển thị khi môn học ko có ai dạy
                if (MonHoc != null)
                {

                    var dsMonHocGV = db.tb_MonHocCoTheDay.Where(s => s.ID_MonHoc == MonHoc).ToList();
                    ViewBag.GiangVien = new SelectList(dsMonHocGV, "ID_GiangVien", "tb_GiangVien.tb_ThongTinCaNhan.HoTen", dsMonHocGV);

                    //var dsMonHocGV = from ds in db.tb_MonHocCoTheDay where ds.ID_MonHoc == MonHoc select ds;
                    //dsGiangVien = db.tb_GiangVien.Where( t=> t.ID_ChuyenNganh == Nganh && dsMonHocGV.Any(s=>s.ID_GiangVien == t.ID )).ToList();
                    //var dsgv = db.tb_GiangVien.Where(s => s.ID_ChuyenNganh == Nganh).ToList();
                    //var dsgv = from ds in db.tb_GiangVien where ds.ID_ChuyenNganh == Nganh select ds;
                    //dsGiangVien = (List<tb_GiangVien>)(from gv in dsgv where dsMonHocGV.Any(d => d.ID_GiangVien == gv.ID) select gv);

                }

                else
                {
                    ViewBag.GiangVien = new SelectList(dsGiangVien, "ID", "tb_ThongTinCaNhan.HoTen");
                }
                //var dsGiangVien = db.tb_GiangVien.Where(s => s.ID_ChuyenNganh == Nganh).ToList();
                var dsMonHoc = db.tb_Monhoc.Where(s => s.ChuyenNganh == Nganh);
                ViewBag.Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh", Nganh);
                ViewBag.MonHoc = new SelectList(dsMonHoc, "ID", "TenMonHoc", dsMonHoc);
            }
            else
            {
                var dsnganh = db.tb_Nganh;
                ViewBag.Nganh = new SelectList(dsnganh, "ID", "TenNganh");
                var nganhDau = dsnganh.FirstOrDefault();
                if (nganhDau != null)
                {
                    var dsmh = db.tb_Monhoc.Where(n => n.tb_Nganh.ID == nganhDau.ID);
                    ViewBag.MonHoc = new SelectList(dsmh, "ID", "TenMonHoc");
                    var mhDau = dsmh.FirstOrDefault();
                    if (mhDau != null)
                    {
                        var tmpdsgv = db.tb_GiangVien.Where(n => n.tb_Nganh.ID == nganhDau.ID);
                        var dsgv = new List<tb_GiangVien>();
                        foreach (var gv in tmpdsgv)
                        {
                            var dsmhctd = gv.tb_MonHocCoTheDay;
                            foreach (var mhctd in dsmhctd)
                            {
                                if (mhctd.tb_Monhoc.ID == mhDau.ID)
                                {
                                    dsgv.Add(gv);
                                    break;
                                }
                            }
                        }
                        ViewBag.GiangVien = new SelectList(dsgv, "ID", "tb_ThongTinCaNhan.HoTen");
                    }
                    else
                    {
                        ViewBag.MonHoc = new SelectList(new List<tb_Monhoc>(), "ID", "TenMonHoc");
                        ViewBag.GiangVien = new SelectList(new List<tb_GiangVien>(), "ID", "tb_ThongTinCaNhan.HoTen");
                    }
                }
                else
                {
                    ViewBag.MonHoc = new SelectList(new List<tb_Monhoc>(), "ID", "TenMonHoc");
                    ViewBag.GiangVien = new SelectList(new List<tb_GiangVien>(), "ID", "tb_ThongTinCaNhan.HoTen");
                }
            }
            return PartialView();
        }


        public ActionResult SuaThongTinKhoaHoc(int? id)
        {
            var KhoaHoc = db.tb_KhoaHoc.Find(id);
            var dsGiangVien = db.tb_GiangVien.Where(s => s.ID_ChuyenNganh == KhoaHoc.tb_Monhoc.ChuyenNganh).ToList();
            ViewBag.GiangVien = new SelectList(dsGiangVien, "ID", "tb_ThongTinCaNhan.HoTen", dsGiangVien);
            return View(KhoaHoc);
        }


        [HttpPost]
        public ActionResult SuaThongTinKhoaHoc(FormCollection fields)
        {
            int id = int.Parse(fields["ID"]);
            int IdGiangVien = int.Parse(fields["GiangVien"]);

            var khoahoc = db.tb_KhoaHoc.Find(id);

            if (khoahoc != null)
            {
                khoahoc.ID_GiangVien = IdGiangVien;
                db.Entry(khoahoc).State = EntityState.Modified;
            }
            try
            {
                db.SaveChanges();
                Session["SuccessSuaKhoaHoc"] = "Cập nhật khoá học thành công";
            }
            catch
            {
                Session["ErrorSuaKhoaHoc"] = "Cập nhật khoá học thất bại";
            }
            return RedirectToAction("SuaThongTinKhoaHoc", new { id = id });
        }



        public ActionResult ThemLichHoc(int? id)
        {
            var khoahoc = db.tb_KhoaHoc.Find(id);
            return PartialView(khoahoc);
        }

        [HttpPost]
        public ActionResult ThemLichHoc(FormCollection fields)
        {
            int id = int.Parse(fields["ID"]);
            int batdau = int.Parse(fields["batdau"]);
            int ketthuc = int.Parse(fields["ketthuc"]);
            tb_ChiTietKhoaHoc ctkh = new tb_ChiTietKhoaHoc();

            ctkh.ID_KhoaHoc = id;
            ctkh.ID_PhongHoc = int.Parse(fields["Phong"]);
            ctkh.TietBatDau = batdau;
            ctkh.TietKetThuc = ketthuc;
            ctkh.soTiet = ketthuc - batdau + 1;
            ctkh.Ngay = DateTime.Parse(fields["date"]);
            db.tb_ChiTietKhoaHoc.Add(ctkh);
            db.SaveChanges();
            return RedirectToAction("SuaThongTinKhoaHoc", new { id = id });
        }



        public ActionResult LichHoc(int? id)
        {
            var lichHoc = db.tb_ChiTietKhoaHoc.Where(s => s.ID_KhoaHoc == id).ToList();
            return PartialView(lichHoc);
        }


        public ActionResult FormLichHoc(DateTime? Ngay, int? tietbatdau, int? tietketthuc, int? id)
        {

            //Console.WriteLine(Ngay.ToString());
            //var chitietkhoahoc = from ct in db.tb_ChiTietKhoaHoc where ct.Ngay == Ngay /*&& ct.TietBatDau<=tietbatdau && ct.TietKetThuc>=tietketthuc*/ select ct;
            //var phong = from ph in db.tb_PhongHoc where !chitietkhoahoc.Any(s=>s.ID_PhongHoc == ph.ID) select ph;

            var phong = db.tb_PhongHoc.Where(ph => !db.tb_ChiTietKhoaHoc.Where(s => s.Ngay == Ngay && s.TietBatDau <= tietketthuc && s.TietKetThuc >= tietbatdau).Select(ckh => ckh.ID_PhongHoc).Contains(ph.ID)).ToList();
            ViewBag.bd = tietbatdau;
            ViewBag.kt = tietketthuc;
            ViewBag.Phong = new SelectList(phong, "ID", "MaPhongHoc", phong);
            ViewBag.ID = id;
            return PartialView();
        }


        public ActionResult XoaLichHoc(int id)
        {

            var lichhoc = db.tb_ChiTietKhoaHoc.Find(id);

            return PartialView(lichhoc);
        }



        public ActionResult XacNhanXoaLichHoc(int? id)
        {
            var chitietKH = db.tb_ChiTietKhoaHoc.Find(id);

            if (chitietKH != null)
            {
                int idKhoaHoc = (int)chitietKH.ID_KhoaHoc;

                db.tb_ChiTietKhoaHoc.Remove(chitietKH);
                try
                {
                    db.SaveChanges();
                    Session["XoaLichHoc"] = "Xóa thành công";
                    return RedirectToAction("SuaThongTinKhoaHoc", "KhoaHoc", new { id = idKhoaHoc });
                }
                catch
                {
                    Session["LoiXoaLichHoc"] = "Không thể xóa lịch học này";
                }
            }

            return RedirectToAction("SuaThongTinKhoaHoc");
        }



        public ActionResult DanhSachSinhVienKhoaHoc(int id)
        {
            var dsSinhVien = db.tb_DangKyMonHoc.Where(s => s.ID_KhoaHoc == id).ToList();
            return PartialView(dsSinhVien);
        }

        public void InDanhSachSinhVien(int id)
        {
            var DSSV = db.tb_DangKyMonHoc.Where(s => s.ID_KhoaHoc == id).ToList();

            string maKhoaHoc = db.tb_KhoaHoc.Where(s => s.ID == id).Select(s => s.MaKhoaHoc).FirstOrDefault();

            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách");



            ws.Cells["A1"].Value = "Danh sách sinh viên lớp " + maKhoaHoc;
            ws.Cells["A2"].Value = "Ngày lập danh sách";
            ws.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} at {0:H : mm tt}", DateTimeOffset.Now);
            ws.Cells["A4"].Value = "Mã sinh viên";
            ws.Cells["B4"].Value = "Họ tên";
            ws.Cells["C4"].Value = "Ngày sinh";
            ws.Cells["D4"].Value = "Mã lớp";
            ws.Cells["E4"].Value = "Chuyên ngành";
            ws.Cells["F4"].Value = "Số điện thoại";
            ws.Cells["G4"].Value = "Email";


            ws.Row(4).Height = 20;
            ws.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(4).Style.Font.Bold = true;

            int rowStart = 5;
            foreach (var item in DSSV)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.tb_SinhVien.MaSinhVien;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.tb_SinhVien.tb_ThongTinCaNhan.Hoten;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.tb_SinhVien.tb_ThongTinCaNhan.NgaySinh.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.tb_SinhVien.tb_LopHoc.MaLopHoc;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.tb_SinhVien.tb_Nganh.TenNganh;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.tb_SinhVien.tb_ThongTinCaNhan.SDT;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.tb_SinhVien.tb_ThongTinCaNhan.Email_2;


                rowStart++;

            }
            ws.Cells["A:AZ"].AutoFitColumns();
            string excelName = "Danh sách sinh viên lớp " + maKhoaHoc;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                pck.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }
        }

    }
}