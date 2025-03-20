using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongDaiHoc.Models;
using PagedList;
using System.Net;
using System.IO;

namespace QuanLyTruongDaiHoc.Controllers
{
    public class UserGiangVienController : Controller
    {
        public static tb_GiangVien gv;
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        public ActionResult TrangChu()
        {
            Session["LoaiTK"] = Init.TaiKhoanGV;
            Session["PreviousURL"] = "~/UserGiangVien/TrangChu";
            return View();
        }
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanGV"] == null)
            {
                Session["PreviousURL"] = "~/UserGiangVien/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanGV;
                return false;
            }
            if (gv == null)
            {
                tb_TaiKhoan tk = (tb_TaiKhoan)Session["TaikhoanGV"];
                gv = db.tb_GiangVien.SingleOrDefault(n => n.tb_TaiKhoan.ID == tk.ID);
            }
            return true;
        }
        // GET: UserGiangVien
        public ActionResult DanhGiaRenLuyen()
        {
            if (!checkLogin("DanhGiaRenLuyen"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var lh = db.tb_LopHoc.Where(n => n.tb_GiangVien.ID == gv.ID);
            ViewBag.Lich = db.tb_LichDanhGiaRL.SingleOrDefault();
            return View(lh);
        }

        public ActionResult XemChiTiet(int? id)
        {
            var sv = db.tb_SinhVien.SingleOrDefault(n => n.ID == id);
            var hocKiAndNam = Init.calculateHocKiNam(sv.NgayNhapHoc);
            var dsht = db.tb_DanhGiaRL.SingleOrDefault(n => n.HocKi == hocKiAndNam.Item1 && n.Nam == hocKiAndNam.Item2 && n.ID_SinhVien==sv.ID);
            ViewBag.DGHT = dsht;
            int? iddght =null;
            if (dsht != null)
            {
                iddght = dsht.ID;
            }
            var dsdg = db.tb_DanhGiaRL.Where(n => n.ID != iddght && n.ID_SinhVien == sv.ID).OrderByDescending(n => n.Nam);
            ViewBag.DSDG = dsdg.ToList();

            var ndrl = db.tb_NoiDungRenLuyen;
            if (ndrl != null)
            {
                ViewBag.NoiDungRL = ndrl.ToList();
            }

            return PartialView(sv);
        }

        public ActionResult XemChiTietDGCu(int? id)
        {
            var dg = db.tb_DanhGiaRL.SingleOrDefault(n => n.ID == id);
            var dsdg = db.tb_NoiDungRenLuyen;
            if (dsdg != null)
            {
                ViewBag.NoiDungRL = dsdg.ToList();
            }
            return PartialView(dg);
        }

        [HttpPost]
        public ActionResult ThayDoiDanhGia(FormCollection collection)
        {
            string ghichu = collection["Ghichu"];
            int? id = int.Parse(collection["hidden"]);
            var dg = db.tb_DanhGiaRL.SingleOrDefault(n => n.ID == id);
            dg.ID_GiangVien = gv.ID;
            dg.GhiChu = ghichu;
            db.SaveChanges();

            var dsct = db.tb_ChiTietDanhGiaRL.Where(n => n.ID_DanhGiaRL == id);
            foreach (var ct in dsct)
            {
                int diemgv = int.Parse(collection["Diemgvdanhgia-" + ct.ID]);
                ct.DiemGVDanhGia = diemgv;
                db.Entry(ct).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("DanhGiaRenLuyen");
        } 

        public ActionResult DanhSachNghiPhep()
        {
            if (!checkLogin("DanhSachNghiPhep"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var dsnp = db.tb_DangKyNghiPhep.Where(n => n.tb_GiangVien.ID == gv.ID);
            return View(dsnp);
        }

        public ActionResult ChinhSuaDonNghiPhep(int? id)
        {
            var don = db.tb_DangKyNghiPhep.SingleOrDefault(n => n.ID == id);
            return PartialView(don);
        }

        [HttpPost]
        public ActionResult ChinhSuaDonNghiPhep(FormCollection collection)
        {
            int? id = int.Parse(collection["id_don"]);
            var don = db.tb_DangKyNghiPhep.SingleOrDefault(n => n.ID == id);
            don.NgayNghi = DateTime.Parse(collection["Ngaynghi"]);
            don.LyDo = collection["Lydo"];
            don.NgayGui = DateTime.Now;
            db.Entry(don).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("DanhSachNghiPhep");
        }

        public ActionResult VietDonNghiPhep()
        {

            return PartialView();
        }

        [HttpPost]
        public ActionResult VietDonNghiPhep(FormCollection collection)
        {
            tb_DangKyNghiPhep np = new tb_DangKyNghiPhep();
            np.ID_GiangVien = gv.ID;
            np.LyDo = collection["Lydo"];
            np.NgayNghi = DateTime.Parse(collection["Ngaynghi"]);
            np.NgayGui = DateTime.Now;
            np.TrangThai = Init.DoiDuyet;
            db.tb_DangKyNghiPhep.Add(np);
            db.SaveChanges();
            Session["SuccessGuiDon"] = "Gửi đơn thành công";
            return RedirectToAction("DanhSachNghiPhep");
        }

        public ActionResult XemThongBao(int? page)
        {
            if (!checkLogin("XemThongBao"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            int pageSize = 5;
            int pageNumber = page ?? 1;
            ViewBag.ThongBaoMoiNhat = db.tb_ThongBao.OrderByDescending(n => n.NgayGui).FirstOrDefault(n => n.DoiTuongNhanTB == Init.ThongBaoGV || n.DoiTuongNhanTB == Init.ThongBaoAll);
            var tb = db.tb_ThongBao.Where(n => n.DoiTuongNhanTB == Init.ThongBaoGV).Where(n => n.DoiTuongNhanTB == Init.ThongBaoGV).OrderByDescending(n=>n.NgayGui);

            return View(tb.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult MonHocCoTheDay()
        {
            if (!checkLogin("MonHocCoTheDay"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            ViewBag.ChuyenNganh = gv.tb_Nganh.TenNganh;
            var dsmh = db.tb_MonHocCoTheDay.Where(n => n.tb_GiangVien.ID == gv.ID);
            return View(dsmh);
        }

        [HttpPost]
        public ActionResult LuuThayDoiMonHocCoTheDay(FormCollection collection)
        {
            var dsmh = db.tb_MonHocCoTheDay.Where(n => n.tb_GiangVien.ID == gv.ID);
            if (dsmh != null)
            {
                foreach (var item in dsmh)
                {
                    if (string.IsNullOrEmpty(collection["hidden-" + item.ID]))
                    {
                        db.tb_MonHocCoTheDay.Remove(item);
                    }
                }
                db.SaveChanges();
            }
            Session["SuccessThayDoiMonHocCoTheDay"] = "Thay đổi môn học có thể dạy thành công";
            return RedirectToAction("MonHocCoTheDay");
        }

        public ActionResult ThemMonCoTheDay()
        {
            ViewBag.ChuyenNganh = gv.tb_Nganh.TenNganh;
            var ctdt = gv.tb_Nganh.tb_Chuongtrinhdaotao;
            var dsmh = db.tb_MonHocCoTheDay.Where(n => n.tb_GiangVien.ID == gv.ID);
            if (ctdt != null)
            {
                List<tb_Monhoc> mh = new List<tb_Monhoc>();
                foreach (var item in ctdt)
                {
                    bool ok = true;
                    foreach (var item2 in dsmh)
                    {
                        if (item.MaMonHoc == item2.ID_MonHoc)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        mh.Add(db.tb_Monhoc.SingleOrDefault(n => n.ID == item.MaMonHoc));
                    }
                }
                ViewBag.DSMH = mh;
            }
            return PartialView();
        }

        [HttpPost] 
        public ActionResult ThemMonCoTheDay(FormCollection collection)
        {
            var ctdt = gv.tb_Nganh.tb_Chuongtrinhdaotao;
            var dsmh = db.tb_MonHocCoTheDay.Where(n => n.tb_GiangVien.ID == gv.ID);
            if (ctdt != null)
            {
                foreach (var item in ctdt)
                {
                    bool ok = true;
                    foreach (var item2 in dsmh)
                    {
                        if (item.MaMonHoc == item2.ID_MonHoc)
                        {
                            ok = false;
                            break;
                        }
                    }
                    if (ok)
                    {
                        if (collection["Monhoc-" + item.tb_Monhoc.ID] == "on")
                        {
                            tb_MonHocCoTheDay mhctd = new tb_MonHocCoTheDay();
                            mhctd.ID_GiangVien = gv.ID;
                            mhctd.ID_MonHoc = item.tb_Monhoc.ID;
                            db.tb_MonHocCoTheDay.Add(mhctd);
                            db.SaveChanges();
                        }
                    }
                }
            }
            Session["SuccessThemMonHocCoTheDay"] = "Thêm môn học có thể dạy thành công";
            return RedirectToAction("MonHocCoTheDay");
        }

        public ActionResult ChuongTrinhDaoTao()
        {
            if (!checkLogin("ChuongTrinhDaoTao"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            ViewBag.Nganh = gv.tb_Nganh;


            var HocKi = db.tb_Chuongtrinhdaotao.Where(s => s.tb_Nganh.ID == gv.tb_Nganh.ID).Select(hk => new { hk.Hocki, hk.Nam }).Distinct().OrderBy(hk => hk.Nam).ToList();
            List<NamHoc> namhoc = new List<NamHoc>();

            foreach (var item in HocKi)
            {
                namhoc.Add(new NamHoc { Hocki = (int)item.Hocki, Nam = (int)item.Nam });
            }
            ViewBag.HocKi = namhoc;
            //foreach(var item in HocKi)
            //{
            //    item.Hocki
            //}
            var ctdt = db.tb_Chuongtrinhdaotao.Include(n => n.tb_Monhoc).Include(t => t.tb_Nganh).Where(ct => ct.MaNganh == gv.tb_Nganh.ID);

            return View(ctdt);
        }

        public ActionResult DanhSachKhoaHoc()
        {
            if (!checkLogin("DanhSachKhoaHoc"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var dskh = gv.tb_KhoaHoc.OrderBy(n => n.NgayMo);
            return View(dskh);
        }

        public ActionResult ChamDiem(int? id)
        {
            var kh = db.tb_KhoaHoc.SingleOrDefault(n => n.ID == id);
            ViewBag.KH = kh;
            var dsdkm = kh.tb_DangKyMonHoc.OrderBy(n => n.tb_SinhVien.tb_ThongTinCaNhan.Hoten);
            return View(dsdkm);
        }

        [HttpPost]
        public ActionResult ChamDiem(FormCollection collection)
        {
            int? idkh = int.Parse(collection["hidden"]);
            var kh = db.tb_KhoaHoc.SingleOrDefault(n => n.ID == idkh);
            var dsdkm = kh.tb_DangKyMonHoc;

            var phantram = db.tb_QuyTacTinhDiem.SingleOrDefault();

            foreach (var item in dsdkm)
            {
                item.diemKTDK = int.Parse(collection["Diemktdk-" + item.ID]);
                item.diemKTHP = int.Parse(collection["Diemkthp-" + item.ID]);
                double? diem = item.diemKTDK * phantram.PhanTramKTDK / 100 + item.diemKTHP * phantram.PhanTramKTHP / 100;
                if (diem >= 5)
                {
                    item.KetQua = true;
                    item.tb_SinhVien.SoTC_TichLuy = (int)(item.tb_SinhVien.SoTC_TichLuy + kh.tb_Monhoc.SoTC);
                }
                else
                {
                    item.KetQua = false;
                    item.tb_SinhVien.SoTC_TichLuy = (int)(item.tb_SinhVien.SoTC_TichLuy - kh.tb_Monhoc.SoTC);
                }
                db.Entry(item).State = EntityState.Modified;

                bool totnghiep = true;

                var sv = item.tb_SinhVien;

                if (diem < 5)
                {
                    sv.SoMon_F++;
                    totnghiep = false;
                }
                else
                {
                    int count = 0;
                    foreach (var dkm in sv.tb_DangKyMonHoc)
                    {
                        double? tmpdiem = dkm.diemKTDK * phantram.PhanTramKTDK / 100 + dkm.diemKTHP * phantram.PhanTramKTHP / 100;
                        if (tmpdiem < 5)
                        {
                            count++;
                        }
                    }
                    sv.SoMon_F = count;
                    if (sv.SoMon_F != 0)
                    {
                        totnghiep = false;
                    }
                }
                if (totnghiep)
                {
                    var gdtclt = db.tb_GiaoDucTheChatLT.SingleOrDefault();
                    if (gdtclt != null)
                    {
                        var dkmh = sv.tb_DangKyMonHoc.Where(n => n.tb_Monhoc.ID == gdtclt.ID_MonHoc);
                        bool ok = false;
                        foreach (var dk in dkmh)
                        {
                            double? tmpdiem = dk.diemKTDK * phantram.PhanTramKTDK / 100 + dk.diemKTHP * phantram.PhanTramKTHP / 100;
                            if (tmpdiem >= 5)
                            {
                                ok = true;
                            }
                        }
                        if (ok)
                        {
                            sv.GiaoDucTheChat_LT = false;
                            totnghiep = false;
                        }
                        else
                        {
                            sv.GiaoDucTheChat_LT = true;
                        }
                    }
                }
                if (totnghiep)
                {
                    var gdtcth = db.tb_GiaoDucTheChatTH.SingleOrDefault();
                    if (gdtcth != null)
                    {
                        var dkmh = sv.tb_DangKyMonHoc.Where(n => n.tb_Monhoc.ID == gdtcth.ID_MonHoc);
                        bool ok = false;
                        foreach (var dk in dkmh)
                        {
                            double? tmpdiem = dk.diemKTDK * phantram.PhanTramKTDK / 100 + dk.diemKTHP * phantram.PhanTramKTHP / 100;
                            if (tmpdiem >= 5)
                            {
                                ok = true;
                            }
                        }
                        if (ok)
                        {
                            sv.GiaoDucTheChat_TH = false;
                            totnghiep = false;
                        }
                        else
                        {
                            sv.GiaoDucTheChat_TH = true;
                        }
                    }
                }
                if (totnghiep)
                {
                    var gdanqp = db.tb_GDQPAN.SingleOrDefault();
                    if (gdanqp != null)
                    {
                        var dkmh = sv.tb_DangKyMonHoc.Where(n => n.tb_Monhoc.ID == gdanqp.ID_MonHoc);
                        bool ok = false;
                        foreach (var dk in dkmh)
                        {
                            double? tmpdiem = dk.diemKTDK * phantram.PhanTramKTDK / 100 + dk.diemKTHP * phantram.PhanTramKTHP / 100;
                            if (tmpdiem >= 5)
                            {
                                ok = true;
                            }
                        }
                        if (ok)
                        {
                            sv.GDQPAN = false;
                            totnghiep = false;
                        }
                        else
                        {
                            sv.GDQPAN = true;
                        }
                    }
                }
                if (totnghiep)
                {
                    var nnkc = db.tb_NgoaiNguKhongChuyen;
                    if (nnkc != null)
                    {
                        foreach (var nn in nnkc)
                        {
                            var dkmh = sv.tb_DangKyMonHoc.SingleOrDefault(n => n.tb_Monhoc.ID == nn.ID_MonHoc);
                            if (dkmh != null)
                            {
                                double? tmpdiem = dkmh.diemKTDK * phantram.PhanTramKTDK / 100 + dkmh.diemKTHP * phantram.PhanTramKTHP / 100;
                                if (tmpdiem >= 5)
                                {
                                    if (sv.SoCap_NgoaiNgu < nn.Cap)
                                    {
                                        sv.SoCap_NgoaiNgu = (int)nn.Cap;
                                    }
                                }

                            }
                        }
                        if (sv.SoCap_NgoaiNgu != db.tb_NgoaiNguKhongChuyen.Max(n => n.Cap))
                        {
                            totnghiep = false;
                        }
                    }
                }
                sv.TotNghiep = totnghiep;
                db.Entry(sv).State = EntityState.Modified;
            }
            db.SaveChanges();

            Session["SuccessChamDiem"] = "Đã cập nhật điểm cho các sinh viên thuộc khoá " + kh.MaKhoaHoc + " - " + kh.tb_Monhoc.TenMonHoc;
            return RedirectToAction("ChamDiem", new { id = idkh });
        }

        public ActionResult XemLichDay()
        {
            if (!checkLogin("XemLichDay"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            return View();
        }
        public ActionResult LichDay(DateTime? ngay)
        {
            DateTime day = ngay ?? DateTime.Now;
            List<tb_KhoaHoc> kh = gv.tb_KhoaHoc.ToList();
            Tuan tuan = new Tuan(kh, day);
            ViewBag.Tuan = tuan;
            return PartialView();
        }

        public ActionResult QuanLyLopCoVan()
        {
            if (!checkLogin("QuanLyLopCoVan"))
            {
                return RedirectToAction("DangNhap", "Account");
            }
            var dslop = gv.tb_LopHoc;
            return View(dslop);
        }

        public ActionResult XemThongTinSV(int? id)
        {
            var sv = db.tb_SinhVien.SingleOrDefault(n => n.ID == id);
            ViewBag.TongTC = db.tb_Chuongtrinhdaotao.Where(s => s.MaNganh == sv.ID_ChuyenNganh).Sum(s => s.tb_Monhoc.SoTC);
            return PartialView(sv);
        }



        public ActionResult ThongTinGiangVien(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            tb_GiangVien gv = db.tb_GiangVien.Find(id);
            if (gv == null)
            {
                return HttpNotFound();
            }
            List<string> LopCoVan = db.tb_LopHoc.Where(s => s.ID_CoVan == gv.ID).Select(s => s.MaLopHoc).ToList();

            ViewBag.LopCoVan = LopCoVan;
            return View(gv);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTinGiangVien(FormCollection fields, HttpPostedFileBase Anh)
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

            var TrinhDo = fields["TrinhDo"];
            var CTGD = fields["CTGD"];
            var TrangThai = fields["TrangThai"];
            tb_GiangVien gv = db.tb_GiangVien.Find(int.Parse(ID));

            if (gv != null)
            {
                gv.tb_ThongTinCaNhan.Hoten = TenSV;
                gv.tb_ThongTinCaNhan.GioiTinh = bool.Parse(GioiTinh);
                gv.tb_ThongTinCaNhan.DanToc = DanToc;
                gv.tb_ThongTinCaNhan.NgaySinh = DateTime.Parse(NgaySinh);
                gv.tb_ThongTinCaNhan.SDT = SDT;
                gv.tb_ThongTinCaNhan.DiaChi = DiaChi;
                gv.tb_ThongTinCaNhan.Email_1 = Emal1;

                if (Emal2 != null)
                {
                    gv.tb_ThongTinCaNhan.Email_2 = Emal2;

                }
                if (Anh != null)
                {
                    if (Anh.ContentLength > 0)
                    {

                        //System.IO.File.Delete(Path.Combine(Server.MapPath("~/AnhSV"), GiangVien.tb_ThongTinCaNhan.Anh));
                        string fileName = Path.GetFileName(Anh.FileName);
                        string _path = Path.Combine(Server.MapPath("~/AnhGV"), fileName);
                        Anh.SaveAs(_path);
                        gv.tb_ThongTinCaNhan.Anh = fileName;
                    }
                }
                gv.TrangThai = bool.Parse(TrangThai);
                gv.TrinhDo = TrinhDo;
                gv.ChuongTrinhGiangDay = CTGD;
                gv.tb_ThongTinCaNhan.So_CCCD = SoCCCD;
                db.Entry(gv).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ThongTinGiangVien", new { id = gv.ID });
            }



            return RedirectToAction("ThongTinGiangVien", new { id = gv.ID });
        }





    }

}