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
    public class GiangVienController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();

        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/GiangVien/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }

        // GET: Admin/GiangVien
        public ActionResult DanhSachGiangVien()
        {
            if (!checkLogin("DanhSachGiangVien"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var tb_GiangVien = db.tb_GiangVien.Include(t => t.tb_Nganh).Include(t => t.tb_TaiKhoan).Include(t => t.tb_ThongTinCaNhan);
            return View(tb_GiangVien.ToList());
        }

     
        public ActionResult ThemGiangVien()
        {
            ViewBag.ID_Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");
            //ViewBag.ID_HeDaoTao = new SelectList(db.tb_HeDaoTao, "ID", "TenHeDaoTao");
            //ViewBag.ID_TaiKhoan = new SelectList(db.tb_TaiKhoan, "ID", "TenDangNhap");
            //ViewBag.ID_ThongTinCaNhan = new SelectList(db.tb_ThongTinCaNhan, "ID", "Hoten");
            ViewBag.ID_Lop = new SelectList(db.tb_LopHoc.Where(s=>s.ID_CoVan ==null), "ID", "MaLopHoc");
            return PartialView();
        }




        private tb_ThongTinCaNhan ThemThongTinCaNhan(string HoTen, string GioiTinh, string DanToc, string NgaySinh, string SDT, string DiaChi, string Email1, string CCCD, string Anh = "")
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
        private tb_TaiKhoan ThemTaiKhoan(string MaGV)
        {
            tb_TaiKhoan taiKhoan = new tb_TaiKhoan();
            taiKhoan.TenDangNhap = MaGV;
            taiKhoan.MatKhau = MaGV;
            taiKhoan.LoaiTaiKhoan = "Giảng Viên";
            db.tb_TaiKhoan.Add(taiKhoan);
            db.SaveChanges();
            return taiKhoan;
        }

        [HttpPost]
        public ActionResult ThemGiangVien(FormCollection fields, HttpPostedFileBase Anh)
        {
            //var MaSV = fields["MaSV"];
            var TenGV = fields["TenGV"];
            var GioiTinh = fields["GioiTinh"];
            var DanToc = fields["DanToc"];
            var NgaySinh = fields["NgaySinh"];
            var SDT = fields["SDT"];
            var DiaChi = fields["DiaChi"];
            var Emal1 = fields["Email1"];
            var SoCCCD = fields["SoCCCD"];
            var ChuyenNganh = fields["ID_Nganh"];
            var LopHoc = fields["ID_LopHoc"];
            var TrinhDo = fields["TrinhDo"];
            var CTGD = fields["CTGD"];



            tb_ThongTinCaNhan thongTinCaNhan = new tb_ThongTinCaNhan();
            if (Anh != null)
            {
                if (Anh.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(Anh.FileName);
                    string _path = Path.Combine(Server.MapPath("~/AnhGV"), fileName);
                    Anh.SaveAs(_path);
                    thongTinCaNhan = ThemThongTinCaNhan(TenGV, GioiTinh, DanToc, NgaySinh, SDT, DiaChi, Emal1, SoCCCD, fileName);
                }
            }
           
            else
            {
                thongTinCaNhan = ThemThongTinCaNhan(TenGV, GioiTinh, DanToc, NgaySinh, SDT, DiaChi, Emal1, SoCCCD);
            }

            tb_GiangVien giangvien = new tb_GiangVien();
            giangvien.ID_ThongTinCaNhan = thongTinCaNhan.ID;


            
            giangvien.ID_ChuyenNganh = int.Parse(ChuyenNganh);
            int idNganh = int.Parse(ChuyenNganh);

            string nganh = db.tb_Nganh.Where(s => s.ID == idNganh).Select(s => s.MaNganh).SingleOrDefault();

            int id = db.tb_GiangVien.OrderByDescending(s => s.ID).Select(s => s.ID).FirstOrDefault();
            giangvien.MaGiangVien = nganh + ((id+1).ToString("0000"));

            thongTinCaNhan.Email_2 = giangvien.MaGiangVien + "@edu.vn";


            tb_TaiKhoan taiKhoan = ThemTaiKhoan(giangvien.MaGiangVien);

            giangvien.ID_TaiKhoan = taiKhoan.ID;
            giangvien.TrinhDo = TrinhDo;
            giangvien.ChuongTrinhGiangDay = CTGD;

            //giangvien.Magiangvien = MaSV;

            //if (LopHoc != null)
            //{
            //    giangvien.ID_LopHoc = int.Parse(LopHoc);
            //}

            giangvien.TrangThai = true;


            db.tb_GiangVien.Add(giangvien);

            db.SaveChanges();



            return RedirectToAction("DanhSachGiangVien");
        }



        public ActionResult XoaGiangVien(int? id)
        {
            var gv = db.tb_GiangVien.Find(id);
            return PartialView(gv);
        }

        public ActionResult XacNhanXoaGiangVien(int id)
        {
            tb_GiangVien gv = db.tb_GiangVien.Find(id);
            db.tb_GiangVien.Remove(gv);
            try
            {

                db.SaveChanges();
                Session["XoaGVThanhCong"] = "Đã xóa Giảng viên";
            }
            catch (Exception) { Session["LoiXoaGV"] = "Không thể xóa Giảng viên"; }
            return RedirectToAction("DanhSachGiangVien");
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
            List<string> LopCoVan = db.tb_LopHoc.Where(s => s.ID_CoVan == gv.ID).Select(s=>s.MaLopHoc).ToList();

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



        public ActionResult NhapDanhSach()
        {
            return PartialView();
        }



        public ActionResult XemDSGV()
        {
            List<DSGV> danhsachGV = Session["dsgv"] as List<DSGV>;
            //ViewBag.c = danhsachGV.Count;
            return View(danhsachGV);
        }


        [HttpPost]
        public ActionResult XemDSGV(HttpPostedFileBase DSGV)
        {
            List<DSGV> danhsachGV = new List<DSGV>();
            if (DSGV != null)
            {
                if (DSGV.ContentLength > 0)
                {
                    if (DSGV.FileName.EndsWith("xlsx") || DSGV.FileName.EndsWith("xls"))
                    {


                        //string fileName = Path.GetFileName(DSSV.FileName);
                        //string _path = Path.Combine(Server.MapPath("~/DSSV"), fileName);
                        //DSSV.SaveAs(_path);
                        ExcelPackage excel = new ExcelPackage(DSGV.InputStream);
                        ExcelWorksheet ws = excel.Workbook.Worksheets[0];
                        int i = ws.Dimension.Rows;
                        for (int row = 2; row <= ws.Dimension.Rows; row++)
                        {

                            DSGV GiangVien = new DSGV();
                            GiangVien.HoTen = ws.Cells[row, 1].Text.ToString();
                            GiangVien.GioiTinh = ws.Cells[row, 2].Text.ToString();
                            GiangVien.NgaySinh = DateTime.ParseExact(ws.Cells[row, 3].Text.ToString(), "yyyy-MM-dd", null); /* ws.Cells[row, 3].Text.ToString();*/
                            GiangVien.DanToc = ws.Cells[row, 4].Text.ToString();
                            GiangVien.SDT = ws.Cells[row, 5].Text.ToString();
                            GiangVien.DiaChi = ws.Cells[row, 6].Text.ToString();
                            GiangVien.Email = ws.Cells[row, 7].Text.ToString();
                            GiangVien.CCCD = ws.Cells[row, 8].Text.ToString();
                            GiangVien.Nganh = ws.Cells[row, 9].Text.ToString();
                            GiangVien.TrinhDo = ws.Cells[row, 11].Text.ToString();
                            GiangVien.CTGD = ws.Cells[row, 10].Text.ToString();
                            danhsachGV.Add(GiangVien);
                        }
                        Session["dsgv"] = danhsachGV;
                        return View(danhsachGV);

                    }
                }

            }


            return RedirectToAction("DanhSachGiangVien");
        }


        public ActionResult XoaGiangVienDS(string cccd)
        {
            List<DSGV> danhsach = Session["dsgv"] as List<DSGV>;
            DSGV gv = danhsach.Find(s => s.CCCD == cccd);
            if (gv != null)
            {
                danhsach.Remove(gv);
                return RedirectToAction("XemDSGV");
            }
            return RedirectToAction("XemDSGV");
        }

        public ActionResult LuuDanhSach()
        {
            List<DSGV> danhsach = Session["dsgv"] as List<DSGV>;

            foreach (DSGV item in danhsach)
            {
                tb_ThongTinCaNhan thongTinCaNhan = new tb_ThongTinCaNhan();
                string ngaysinh = item.NgaySinh.ToString(); //DateTime.ParseExact(item.NgaySinh, "yyyy-MM-dd", null).ToString();
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

                tb_GiangVien gv = new tb_GiangVien();
                gv.ID_ThongTinCaNhan = thongTinCaNhan.ID;

                //int ID_hedaotao = db.tb_HeDaoTao.Where(s => s.MaHeDaoTao == item.HeDaoTao).Select(s => s.ID).FirstOrDefault();
                int ID_chuyenNganh = db.tb_Nganh.Where(s => s.MaNganh == item.Nganh).Select(s => s.ID).FirstOrDefault();
                //g.ID_HeDaoTao = ID_hedaotao;
                gv.ID_ChuyenNganh = ID_chuyenNganh;

                gv.TrinhDo = item.TrinhDo;
                gv.ChuongTrinhGiangDay = item.CTGD;
                tb_Nganh nganh = db.tb_Nganh.Find(ID_chuyenNganh);
                //int nienkhoa = (int)Math.Round((double)nganh.NamDaotao);
                //sinhvien.NienKhoa = DateTime.Now.Year.ToString() + " - " + DateTime.Now.AddYears(nienkhoa).Year.ToString();

                //int maxSinhVien = 1000000;
                int id = db.tb_GiangVien.OrderByDescending(s => s.ID).Select(s => s.ID).FirstOrDefault();
                gv.MaGiangVien = nganh.MaNganh.ToString() + (id + 1).ToString("0000");

                thongTinCaNhan.Email_2 = gv.MaGiangVien + "@edu.vn";


                tb_TaiKhoan taiKhoan = ThemTaiKhoan(gv.MaGiangVien);

                gv.ID_TaiKhoan = taiKhoan.ID;

                //sinhvien.MaSinhVien = MaSV;


                gv.TrangThai = true;


                db.tb_GiangVien.Add(gv);
            }
            db.SaveChanges();
            Session["dsgv"] = null;

            return RedirectToAction("DanhSachGiangVien");
        }
    }
}
