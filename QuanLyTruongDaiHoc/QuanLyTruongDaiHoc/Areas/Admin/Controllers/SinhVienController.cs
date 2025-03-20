using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using QuanLyTruongDaiHoc.Models;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class SinhVienController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/SinhVien/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        // GET: Admin/SinhVien
        public ActionResult DanhSachSinhVien()
        {
            if (!checkLogin("DanhSachSinhVien"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            ViewBag.countsv = db.tb_SinhVien.Count();
            var tb_SinhVien = db.tb_SinhVien.Include(t => t.tb_HeDaoTao).Include(t => t.tb_TaiKhoan).Include(t => t.tb_ThongTinCaNhan);
            return View(tb_SinhVien.ToList());
        }

        public ActionResult ThemSinhVien()
        {
            ViewBag.ID_Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");
            ViewBag.ID_HeDaoTao = new SelectList(db.tb_HeDaoTao, "ID", "TenHeDaoTao");
            ViewBag.ID_TaiKhoan = new SelectList(db.tb_TaiKhoan, "ID", "TenDangNhap");
            ViewBag.ID_ThongTinCaNhan = new SelectList(db.tb_ThongTinCaNhan, "ID", "Hoten");
            ViewBag.ID_Lop = new SelectList(db.tb_LopHoc, "ID", "MaLopHoc");
            return PartialView();
        }



        private tb_ThongTinCaNhan ThemThongTinCaNhan(string HoTen,string GioiTinh,string DanToc,string NgaySinh, string SDT,string DiaChi, string Email1, string CCCD,string Anh="")
        {
            tb_ThongTinCaNhan tt = new tb_ThongTinCaNhan();
            tt.Hoten = HoTen;
            tt.GioiTinh = bool.Parse(GioiTinh);
            tt.DanToc = DanToc;
            tt.NgaySinh = DateTime.Parse(NgaySinh);
            tt.SDT = SDT;
            tt.DiaChi = DiaChi;
            tt.Email_1 = Email1;
            tt.So_CCCD = CCCD;
            tt.Anh = Anh;
            db.tb_ThongTinCaNhan.Add(tt);
            db.SaveChanges();
            return tt;
        }
        private tb_TaiKhoan ThemTaiKhoan(string MaSV)
        {
            tb_TaiKhoan taiKhoan = new tb_TaiKhoan();
            taiKhoan.TenDangNhap = MaSV;
            taiKhoan.MatKhau = MaSV;
            taiKhoan.LoaiTaiKhoan = "Sinh Viên";
            db.tb_TaiKhoan.Add(taiKhoan);
            db.SaveChanges();
            return taiKhoan;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemSinhVien(FormCollection fields, HttpPostedFileBase Anh)
        {
            //var MaSV = fields["MaSV"];
            var TenSV = fields["TenSV"];
            var GioiTinh = fields["GioiTinh"];
            var DanToc = fields["DanToc"];
            var NgaySinh = fields["NgaySinh"];
            var SDT = fields["SDT"];
            var DiaChi = fields["DiaChi"];
            var Emal1 = fields["Email1"];
            var SoCCCD = fields["SoCCCD"];
            var ChuyenNganh = fields["ID_Nganh"];
            var LopHoc = fields["ID_LopHoc"];
            var HeDaoTao = fields["ID_HeDaoTao"];
         



            tb_ThongTinCaNhan thongTinCaNhan = new tb_ThongTinCaNhan();
            if (Anh != null)
            {
                if (Anh.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Anh.FileName);
                    string _path = Path.Combine(Server.MapPath("~/AnhSV"), fileName);
                    Anh.SaveAs(_path);
                    thongTinCaNhan = ThemThongTinCaNhan(TenSV, GioiTinh, DanToc, NgaySinh, SDT, DiaChi, Emal1, SoCCCD, fileName);
                }
               

            }
            else
            {
                thongTinCaNhan = ThemThongTinCaNhan(TenSV, GioiTinh, DanToc, NgaySinh, SDT, DiaChi, Emal1, SoCCCD);
            }
            tb_SinhVien sinhvien = new tb_SinhVien();
            sinhvien.ID_ThongTinCaNhan = thongTinCaNhan.ID;
           

            sinhvien.ID_HeDaoTao = int.Parse(HeDaoTao);
            sinhvien.ID_ChuyenNganh = int.Parse(ChuyenNganh);

            tb_Nganh nganh = db.tb_Nganh.Find(int.Parse(ChuyenNganh));
            int nienkhoa = (int)Math.Round((double)nganh.NamDaotao);
            sinhvien.NgayNhapHoc = DateTime.Now;
            sinhvien.KyNang = false;
            sinhvien.DiemTichLuy = 0;
            sinhvien.GDQPAN = false;
            sinhvien.GiaoDucTheChat_LT = false;
            sinhvien.GiaoDucTheChat_TH = false;
            sinhvien.SoCap_NgoaiNgu = 0;
            sinhvien.SoMon_F = 0;
            sinhvien.SoTC_TichLuy = 0;
            sinhvien.TrangThai = true;
            sinhvien.DiemTichLuy = 0;

            int maxSinhVien = 1000000;
            int id = db.tb_SinhVien.OrderByDescending(s => s.ID).Select(s=> s.ID).FirstOrDefault();
            sinhvien.MaSinhVien = ((DateTime.Now.Year) % 1000).ToString() + ((DateTime.Now.AddYears(nienkhoa).Year) % 1000).ToString() + (maxSinhVien + id+1);

            thongTinCaNhan.Email_2 = sinhvien.MaSinhVien + "@student.edu.vn";


            tb_TaiKhoan taiKhoan = ThemTaiKhoan(sinhvien.MaSinhVien);

            sinhvien.ID_TaiKhoan = taiKhoan.ID;

            //sinhvien.MaSinhVien = MaSV;
            if (LopHoc != null)
            {
                sinhvien.ID_LopHoc = int.Parse(LopHoc);
            }


            db.tb_SinhVien.Add(sinhvien);

            db.SaveChanges();


            return RedirectToAction("DanhSachSinhVien");
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
            double progress = (tcTichluy / tongtc)*100;
            ViewBag.Prg = progress + "%";





            return View(sinhvien);

        }


        public ActionResult XoaSinhVien(int? id)
        {
            var sinhvien = db.tb_SinhVien.Find(id);
            return PartialView(sinhvien);
        }
        
        public ActionResult XacNhanXoaSinhVien(int id)
        {
            tb_SinhVien sv = db.tb_SinhVien.Find(id);
            db.tb_SinhVien.Remove(sv);
            try
            {

                db.SaveChanges();
                Session["XoaSVThanhCong"] = "Đã xóa sinh viên";
            }
            catch (Exception) { Session["LoiXoaSV"] = "Không thể xóa Sinh viên"; }
            return RedirectToAction("DanhSachSinhVien");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTinSinhVien(FormCollection fields,HttpPostedFileBase Anh)
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


       

        public ActionResult XemDSSV()
        {
            List<DSSV> danhsachSV = Session["dssv"] as List<DSSV>;
            ViewBag.c = danhsachSV.Count;
            return View(danhsachSV);
        }

        [HttpPost]
        public ActionResult XemDSSV(HttpPostedFileBase DSSV)
        {
            List<DSSV> danhsachSV = new List<DSSV>();
            if (DSSV != null)
            {
                if (DSSV.ContentLength > 0)
                {
                    if(DSSV.FileName.EndsWith("xlsx") || DSSV.FileName.EndsWith("xls"))
                    {

                        
                        //string fileName = Path.GetFileName(DSSV.FileName);
                        //string _path = Path.Combine(Server.MapPath("~/DSSV"), fileName);
                        //DSSV.SaveAs(_path);
                        ExcelPackage excel = new ExcelPackage(DSSV.InputStream);
                        ExcelWorksheet ws = excel.Workbook.Worksheets["DSSV"];
                        int i = ws.Dimension.Rows;
                        for (int row = 2; row <= ws.Dimension.Rows; row++)
                        {

                            DSSV sinhvien = new DSSV();
                            sinhvien.HoTen = ws.Cells[row, 1].Text.ToString();
                            sinhvien.GioiTinh = ws.Cells[row, 2].Text.ToString();
                            sinhvien.NgaySinh = /*DateTime.ParseExact(ws.Cells[row, 3].Text.ToString(), "yyyy-MM-dd", null);*/  ws.Cells[row, 3].Text.ToString();
                            sinhvien.DanToc = ws.Cells[row, 4].Text.ToString();
                            sinhvien.SDT = ws.Cells[row, 5].Text.ToString();
                            sinhvien.DiaChi = ws.Cells[row, 6].Text.ToString();
                            sinhvien.Email = ws.Cells[row, 7].Text.ToString();
                            sinhvien.CCCD = ws.Cells[row, 8].Text.ToString();
                            sinhvien.Nganh = ws.Cells[row, 9].Text.ToString();
                            sinhvien.HeDaoTao = ws.Cells[row, 10].Text.ToString();

                            danhsachSV.Add(sinhvien);
                        }
                        Session["dssv"] = danhsachSV;
                        return View(danhsachSV);

                    }
                }

            }

            
            return RedirectToAction("DanhSachSinhVien");
        }

        public ActionResult XoaSinhVienDS(string cccd)
        {
            List<DSSV> danhsach = Session["dssv"] as List<DSSV>;
            DSSV sinhvien = danhsach.Find(s => s.CCCD == cccd);
            if (sinhvien != null)
            {
                danhsach.Remove(sinhvien);
            }
            return RedirectToAction("XemDSSV");
        }

        public ActionResult LuuDanhSach()
        {
            List<DSSV> danhsach = Session["dssv"] as List<DSSV>;
            
            foreach(DSSV item in danhsach)
            {
                tb_ThongTinCaNhan thongTinCaNhan = new tb_ThongTinCaNhan();
                string ngaysinh = DateTime.ParseExact(item.NgaySinh, "yyyy-MM-dd", null).ToString();
                string GioiTinh;
                if (item.GioiTinh.Equals("Nam"))
                {
                    GioiTinh = "true";
                }
                else
                {
                    GioiTinh = "false";
                }

                thongTinCaNhan = ThemThongTinCaNhan(item.HoTen, GioiTinh, item.DanToc, ngaysinh, item.SDT, item.DiaChi, item.Email, item.CCCD);
               
                tb_SinhVien sinhvien = new tb_SinhVien();
                sinhvien.ID_ThongTinCaNhan = thongTinCaNhan.ID;

                int ID_hedaotao = db.tb_HeDaoTao.Where(s => s.MaHeDaoTao == item.HeDaoTao).Select(s => s.ID).FirstOrDefault();
                int ID_chuyenNganh = db.tb_Nganh.Where(s => s.MaNganh == item.Nganh).Select(s => s.ID).FirstOrDefault();
                sinhvien.ID_HeDaoTao = ID_hedaotao;
                sinhvien.ID_ChuyenNganh = ID_chuyenNganh;

                tb_Nganh nganh = db.tb_Nganh.Find(ID_chuyenNganh);
                int nienkhoa = (int)Math.Round((double)nganh.NamDaotao);
                sinhvien.NgayNhapHoc = DateTime.Now;
                sinhvien.KyNang = false;
                sinhvien.GDQPAN = false;
                sinhvien.GiaoDucTheChat_LT = false;
                sinhvien.GiaoDucTheChat_TH = false;
                sinhvien.SoCap_NgoaiNgu = 0;
                sinhvien.SoMon_F = 0;
                sinhvien.SoTC_TichLuy = 0;

                int maxSinhVien = 1000000;
                int id = db.tb_SinhVien.OrderByDescending(s => s.ID).Select(s => s.ID).FirstOrDefault();
                sinhvien.MaSinhVien = ((DateTime.Now.Year) % 1000).ToString() + ((DateTime.Now.AddYears(nienkhoa).Year) % 1000).ToString() + (maxSinhVien + id + 1);

                thongTinCaNhan.Email_2 = sinhvien.MaSinhVien + "@student.edu.vn";


                tb_TaiKhoan taiKhoan = ThemTaiKhoan(sinhvien.MaSinhVien);

                sinhvien.ID_TaiKhoan = taiKhoan.ID;

                sinhvien.TrangThai = true;
                sinhvien.DiemTichLuy = 0;


                db.tb_SinhVien.Add(sinhvien);
            }
            db.SaveChanges();
            Session["dssv"] = null; 

            return RedirectToAction("DanhSachSinhVien");
        }

        public ActionResult NhapDanhSach()
        {
            return PartialView();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
