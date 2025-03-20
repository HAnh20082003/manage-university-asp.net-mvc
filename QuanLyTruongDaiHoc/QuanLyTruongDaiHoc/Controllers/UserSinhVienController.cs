using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net;
using System.IO;

namespace QuanLyTruongDaiHoc.Controllers
{
    public class UserSinhVienController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        // GET: UserSinhVien
        public static tb_SinhVien sv;
        public static List<NamHoc> namhoc = new List<NamHoc>();
        public ActionResult TrangChu()
        {
            Session["LoaiTK"] = Init.TaiKhoanSV;
            Session["PreviousURL"] = "~/UserSinhVien/TrangChu";
            return View();
        }

        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanSV"] == null)
            {
                Session["PreviousURL"] = "~/UserSinhVien/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanSV;
                return false;
            }
            if (sv == null)
            {
                tb_TaiKhoan tk = (tb_TaiKhoan)Session["TaikhoanSV"];
                sv = db.tb_SinhVien.SingleOrDefault(n => n.tb_TaiKhoan.ID == tk.ID);
                var HocKi = db.tb_Chuongtrinhdaotao.Where(s => s.tb_Nganh.ID == sv.tb_Nganh.ID).Select(hk => new { hk.Hocki, hk.Nam }).Distinct().OrderBy(hk => hk.Nam).ToList();
                namhoc.Clear();
                foreach (var item in HocKi)
                {
                    namhoc.Add(new NamHoc { Hocki = (int)item.Hocki, Nam = (int)item.Nam });
                }
            }
            return true;
        }

        //Yêu cầu cấp giấy
        public ActionResult XemYeuCauCapGiay()
        {
            if (!checkLogin("XemYeuCauCapGiay"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var giayChungNhan = db.tb_YeuCauGiayChungNhan.Where(n => n.tb_SinhVien.ID == sv.ID);
            return View(giayChungNhan);
        }

        public ActionResult ThemYeuCauCapGiay()
        {
            ViewBag.GiayChungNhan = db.tb_GiayChungNhan.ToList();
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemYeuCauCapGiay(FormCollection collection)
        {
            int id = int.Parse(collection["GiayChungNhan"]);
            string yeucau = collection["Yeucau"];
            tb_YeuCauGiayChungNhan yc = new tb_YeuCauGiayChungNhan();
            yc.ID_GiayChungNhan = id;
            yc.ThongTinYeuCau = yeucau;
            yc.ID_SinhVien = sv.ID;
            yc.NgayYeuCau = DateTime.Now;
            db.tb_YeuCauGiayChungNhan.Add(yc);
            db.SaveChanges();
            Session["SuccessThemYeuCau"] = "Gửi yêu cầu cấp giấy chứng nhận thành công";
            return RedirectToAction("XemYeuCauCapGiay", "UserSinhVien");
        }


        //Đánh giá rèn luyện
        public ActionResult DanhGiaRenLuyen()
        {
            if (!checkLogin("DanhGiaRenLuyen"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            tb_LichDanhGiaRL lich = db.tb_LichDanhGiaRL.SingleOrDefault();
            var hkAndNam = Init.calculateHocKiNam(sv.NgayNhapHoc);
            if (lich != null)
            {
                var dgrl = db.tb_DanhGiaRL.SingleOrDefault(n => n.ID_SinhVien == sv.ID && n.HocKi == hkAndNam.Item1 && n.Nam == hkAndNam.Item2);
                if (dgrl != null)
                {
                    int? id = dgrl.ID;
                    var ctrl = db.tb_ChiTietDanhGiaRL.Where(n => n.ID_DanhGiaRL == id);
                    ViewBag.ChitietDGRL = ctrl.ToList();
                }
            }
            var ndrl = db.tb_DanhGiaRL.Where(n => n.ID_SinhVien == sv.ID);
            ViewBag.NoiDungRL = db.tb_NoiDungRenLuyen.ToList();
            ViewBag.LichDGRL = db.tb_LichDanhGiaRL.SingleOrDefault();
            return View(ndrl);
        }

        public ActionResult XemChiTietRL(int ?id)
        {
            var drl = db.tb_DanhGiaRL.SingleOrDefault(n => n.ID == id);
            ViewBag.NoiDungRL = db.tb_NoiDungRenLuyen.ToList();
            return PartialView(drl);
        }

        [HttpPost]
        public ActionResult ThemDGRL(FormCollection collection)
        {
            tb_LichDanhGiaRL lich = db.tb_LichDanhGiaRL.SingleOrDefault();
            var hkAndNam = Init.calculateHocKiNam(sv.NgayNhapHoc);
            var dgs = db.tb_DanhGiaRL.SingleOrDefault(n => n.ID_SinhVien == sv.ID && n.HocKi == hkAndNam.Item1 && n.Nam == hkAndNam.Item2);
            if (dgs == null)
            {
                dgs = new tb_DanhGiaRL();
                dgs.ID_SinhVien = sv.ID;
                dgs.HocKi = hkAndNam.Item1;
                dgs.Nam = hkAndNam.Item2;
                dgs.TongDiem = 0;
                db.tb_DanhGiaRL.Add(dgs);
            }
            db.SaveChanges();
            foreach (string key in collection.Keys)
            {
                if (key.Contains("Diemsvdanhgia"))
                {
                    int diemsv = int.Parse(collection[key]);
                    int? id = int.Parse(key.Split('-')[1]);
                    var chitietndrl = db.tb_ChiTietNoiDungRenLuyen.SingleOrDefault(n => n.ID == id);
                    tb_ChiTietDanhGiaRL chitiet = db.tb_ChiTietDanhGiaRL.SingleOrDefault(n => n.ID_DanhGiaRL == dgs.ID && n.ID_ChiTietNoiDung == id);
                    if (chitiet == null)
                    {
                        chitiet = new tb_ChiTietDanhGiaRL();
                        chitiet.ID_ChiTietNoiDung = id;
                        chitiet.ID_DanhGiaRL = dgs.ID;
                        chitiet.DiemGVDanhGia = 0;
                        chitiet.DiemQTDanhGia = 0;
                        chitiet.DiemSVDanhGia = diemsv;
                        db.tb_ChiTietDanhGiaRL.Add(chitiet);
                    }
                    else
                    {
                        chitiet.DiemSVDanhGia = diemsv;
                        db.Entry(chitiet).State = EntityState.Modified;
                    }
                }
            }
            db.SaveChanges();
            Session["SuccessThemDG"] = "Thêm đánh giá rèn luyện thành công";
            return RedirectToAction("DanhGiaRenLuyen");
        }

        //Xem thông báo
        public ActionResult XemThongBao(int? page)
        {
            if (!checkLogin("XemThongBao"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            int pageSize = 5;
            int pageNumber = page ?? 1;
            ViewBag.ThongBaoMoiNhat = db.tb_ThongBao.OrderByDescending(n => n.NgayGui).FirstOrDefault(n => n.DoiTuongNhanTB == Init.ThongBaoSV || n.DoiTuongNhanTB == Init.ThongBaoAll);
            var tb = db.tb_ThongBao.Where(n => n.DoiTuongNhanTB == Init.ThongBaoSV).OrderByDescending(n => n.NgayGui);
            return View(tb.ToPagedList(pageNumber, pageSize));
        }

        //Xem chương trình đào tạo
        public ActionResult ChuongTrinhDaoTao()
        {
            if (!checkLogin("ChuongTrinhDaoTao"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            ViewBag.Nganh = sv.tb_Nganh;

            ViewBag.HocKi = namhoc;
            //foreach(var item in HocKi)
            //{
            //    item.Hocki
            //}
            var ctdt = db.tb_Chuongtrinhdaotao.Include(n => n.tb_Monhoc).Include(t => t.tb_Nganh).Where(ct => ct.MaNganh == sv.tb_Nganh.ID);
            return View(ctdt);
        }

        //Xem lịch học
        public ActionResult XemLichHoc()
        {
            if (!checkLogin("XemLichHoc"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            return View();
        }
        public ActionResult LichHoc(DateTime? ngay)
        {
            DateTime day = ngay ?? DateTime.Now;
            List<tb_KhoaHoc> kh = new List<tb_KhoaHoc>();
            foreach (var item in db.tb_KhoaHoc)
            {
                if (item.tb_DangKyMonHoc.SingleOrDefault(n => n.ID_SinhVien == sv.ID) != null)
                {
                    kh.Add(item);
                }
            }
            Tuan tuan = new Tuan(kh, day);
            ViewBag.Tuan = tuan;
            return PartialView();
        }


        //Đăng ký môn
        public ActionResult DangKyMonHoc()
        {
            if (!checkLogin("DangKyMonHoc"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            return View();
        }


        public ActionResult MonDangKy()
        {
            var dsDangKy = db.tb_DangKyMonHoc.Where(s => s.ID_SinhVien == sv.ID).ToList();
            ViewBag.SV = sv;
            return PartialView(dsDangKy);
        }

        public ActionResult DanhSachKhoaHoc()
        {
            var dsKhoaHoc = db.tb_KhoaHoc.ToList();
            ViewBag.SV = sv;
            return PartialView(dsKhoaHoc);
        }

        public ActionResult DangKyMon(int? id)
        {
            var khoahoc = db.tb_KhoaHoc.Find(id);
            if (khoahoc == null)
            {
                return RedirectToAction("MonDangKy", "UserSinhVien");
            }
            if (khoahoc.SoLuongHienTai >= khoahoc.SoLuongYeuCau)
            {
                return RedirectToAction("MonDangKy", "UserSinhVien");
            }
            tb_DangKyMonHoc dkmh = new tb_DangKyMonHoc();
            dkmh.ID_SinhVien = sv.ID;
            dkmh.ID_KhoaHoc = khoahoc.ID;
            dkmh.ID_MonHoc = khoahoc.tb_Monhoc.ID;
            var hockiAndNam = Init.calculateHocKiNam(sv.NgayNhapHoc);
            dkmh.HocKi = hockiAndNam.Item1;
            dkmh.Nam = hockiAndNam.Item2;
            dkmh.diemKTDK = 0;
            dkmh.diemKTHP = 0;
            dkmh.KetQua = false;

            db.tb_DangKyMonHoc.Add(dkmh);
            khoahoc.SoLuongHienTai++;
            db.SaveChanges();
            return RedirectToAction("MonDangKy", "UserSinhVien");

        }

        public ActionResult HuyDangKyMon(int? id)
        {
            var khoahoc = db.tb_KhoaHoc.Find(id);
            if (khoahoc != null)
            {
                tb_DangKyMonHoc dkmh = db.tb_DangKyMonHoc.Where(s => s.ID_SinhVien == sv.ID && s.ID_KhoaHoc == khoahoc.ID).FirstOrDefault();

                if (dkmh != null)
                {
                    db.tb_DangKyMonHoc.Remove(dkmh);
                }
                khoahoc.SoLuongHienTai--;
                Session["SuccessHuyDangKyMon"] = "Huỷ khoá học " + khoahoc.MaKhoaHoc + " - " + khoahoc.tb_Monhoc.TenMonHoc;
            }
            db.SaveChanges();
            return RedirectToAction("MonDangKy", "UserSinhVien");
        }

        public ActionResult XemDiem()
        {
            if (!checkLogin("XemDiem"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var dkmh = db.tb_DangKyMonHoc.Where(n => n.ID_SinhVien == sv.ID);
            var HocKi = db.tb_Chuongtrinhdaotao.Where(s => s.tb_Nganh.ID == sv.tb_Nganh.ID).Select(hk => new { hk.Hocki, hk.Nam }).Distinct().OrderBy(hk => hk.Nam).ToList();
            List<NamHoc> namhoc = new List<NamHoc>();
            ViewBag.Nganh = sv.tb_Nganh;
            foreach (var item in HocKi)
            {
                namhoc.Add(new NamHoc { Hocki = (int)item.Hocki, Nam = (int)item.Nam });
            }
            ViewBag.HocKi = namhoc;
            return View(dkmh);
        }


        public ActionResult XemHocBong()
        {
            if (!checkLogin("XemHocBong"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var dshb = db.tb_CapHocBong.Where(n => n.ID_SinhVien == sv.ID).OrderByDescending(n => n.Nam);
            return View(dshb);
        }

        public ActionResult XemLichThi()
        {
            if (!checkLogin("XemLichThi"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            ViewBag.SV = sv;

            var hocKiAndNam = Init.calculateHocKiNam(sv.NgayNhapHoc);
            var dskhht = db.tb_DangKyMonHoc.Where(n => n.HocKi == hocKiAndNam.Item1 && n.Nam == hocKiAndNam.Item2 && n.tb_KhoaHoc.TrangThai == Init.HoatDong);

            var dsdkmh = db.tb_DangKyMonHoc.Where(n => n.HocKi != hocKiAndNam.Item1 || n.Nam != hocKiAndNam.Item2);
            ViewBag.HocKi = namhoc;
            ViewBag.HocKiHT = hocKiAndNam;
            ViewBag.DKMH = dskhht.ToList();
            return View(dsdkmh);
        }



        public ActionResult ThongTinSinhVien(int? id)
        {



            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_SinhVien sinhvien = db.tb_SinhVien.Find(id);
            if (sinhvien == null)
            {
                return HttpNotFound();
            }


            ViewBag.TongTC = db.tb_Chuongtrinhdaotao.Where(s => s.MaNganh == sinhvien.ID_ChuyenNganh).Sum(s => s.tb_Monhoc.SoTC);
            if (ViewBag.TongTC == null) ViewBag.TongTC = 0;
            double tongtc = double.Parse(ViewBag.TongTC.ToString());
            double tcTichluy = sinhvien.SoTC_TichLuy;
            double progress = (tcTichluy / tongtc) * 100;
            ViewBag.Prg = progress + "%";


            return View(sinhvien);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTinSinhVien(FormCollection fields, HttpPostedFileBase Anh)
        {
            var ID = fields["ID"];
            var TenSV = fields["HoTen"];
            var GioiTinh = fields["GioiTinh"];
            var DanToc = fields["DanToc"];
            var NgaySinh = fields["NgaySinh"];
            var SDT = fields["SDT"];
            var DiaChi = fields["DiaChi"];
            var Emal1 = fields["Email1"];
            var Emal2 = fields["Email2"];
            var SoCCCD = fields["CCCD"];

            var TrangThai = fields["TrangThai"];

            tb_SinhVien sinhvien = db.tb_SinhVien.Find(int.Parse(ID));

            if (sinhvien != null)
            {
                sinhvien.tb_ThongTinCaNhan.Hoten = TenSV;
                sinhvien.tb_ThongTinCaNhan.GioiTinh = bool.Parse(GioiTinh);
                sinhvien.tb_ThongTinCaNhan.DanToc = DanToc;
                sinhvien.tb_ThongTinCaNhan.NgaySinh = DateTime.Parse(NgaySinh);
                sinhvien.tb_ThongTinCaNhan.SDT = SDT;
                sinhvien.tb_ThongTinCaNhan.DiaChi = DiaChi;
                sinhvien.tb_ThongTinCaNhan.Email_1 = Emal1;
                if (Emal2 != null)
                {
                    sinhvien.tb_ThongTinCaNhan.Email_2 = Emal2;

                }
                if (Anh != null)
                {
                    if (Anh.ContentLength > 0)
                    {

                        //System.IO.File.Delete(Path.Combine(Server.MapPath("~/AnhSV"), sinhvien.tb_ThongTinCaNhan.Anh));
                        string fileName = Path.GetFileName(Anh.FileName);
                        string _path = Path.Combine(Server.MapPath("~/AnhSV"), fileName);
                        Anh.SaveAs(_path);
                        sinhvien.tb_ThongTinCaNhan.Anh = fileName;
                    }
                }

                sinhvien.TrangThai = bool.Parse(TrangThai);
                sinhvien.tb_ThongTinCaNhan.So_CCCD = SoCCCD;
                db.Entry(sinhvien).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ThongTinSinhVien", new { id = sinhvien.ID });
            }



            return RedirectToAction("ThongTinSinhVien", new { id = sinhvien.ID });
        }






    }
}