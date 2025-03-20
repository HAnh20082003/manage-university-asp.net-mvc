using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuanLyTruongDaiHoc.Models;
using System.IO;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class LopHocController : Controller
    {
        QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        // GET: Admin/LopHoc
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/LopHoc/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        public ActionResult DanhSachLopHoc()
        {
            if (!checkLogin("DanhSachLopHoc"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var tb_LopHoc = db.tb_LopHoc.Include(t => t.tb_GiangVien).Include(t => t.tb_HeDaoTao).Include(t => t.tb_Nganh);
            return View(tb_LopHoc.ToList());
        }
        public ActionResult ThemDanhSachLop()
        {
            ViewBag.Nganh = new SelectList(db.tb_Nganh, "ID", "TenNganh");
            ViewBag.HeDaoTao = new SelectList(db.tb_HeDaoTao, "ID", "TenHeDaoTao");

            return PartialView();
        }




        private void themGVCV(int idNganh, int idHedaotao)
        {
            var DanhSachGV = db.tb_GiangVien.Where(s => s.ID_ChuyenNganh == idNganh).ToList();

            var DanhSachLop = db.tb_LopHoc.Where(s => s.ID_Nganh == idNganh && s.ID_HeDaoTao == idHedaotao && s.ID_CoVan == null).ToList();

            int i = 0;
            if (DanhSachLop != null)
            {
                foreach (var item in DanhSachLop)
                {
                    if (i < DanhSachGV.Count)
                    {
                        item.ID_CoVan = DanhSachGV.ElementAt(i).ID;
                        db.Entry(item).State = EntityState.Modified;
                        i++;
                    }
                    else
                    {
                        i = 0;
                    }

                }
                db.SaveChanges();
            }


        }



        private List<tb_SinhVien> listStudentNotInClass(int idNganh)
        {
            var danhsachSV = db.tb_SinhVien.Where(s => s.ID_ChuyenNganh == idNganh && s.ID_LopHoc == null).ToList();
            return danhsachSV;
        }


        private void themSV(int idNganh, List<tb_LopHoc> danhsachLop)
        {



            var danhsachSV = listStudentNotInClass(idNganh);

            if (danhsachLop != null)
            {
                while (danhsachSV.Count != 0)
                {
                    foreach (var item in danhsachLop)
                    {

                        if (item.SoLuong == db.tb_SinhVien.Where(s => s.ID_LopHoc == item.ID).Count())
                        {
                            danhsachLop.Remove(item);
                            continue;
                        }

                        if (danhsachSV.Count != 0)
                        {
                            tb_SinhVien sv = danhsachSV.FirstOrDefault();
                            sv.ID_LopHoc = item.ID;
                            danhsachSV.Remove(sv);
                            db.Entry(sv).State = EntityState.Modified;

                        }
                    }
                }
            }
            db.SaveChanges();

        }


        [HttpPost]
        public ActionResult ThemDanhSachLop(FormCollection fields)
        {

            int IdNganh = int.Parse(fields["Nganh"]);
            int IdHedaotao = int.Parse(fields["HeDaoTao"]);
            int soluongLop = int.Parse(fields["SLLop"]);
            int soluongSV = int.Parse(fields["SLSV"]);
            var themSinhvien = fields["themSV"];
            var themCoVan = fields["themGVCV"];


            string maNganh = db.tb_Nganh.Where(s => s.ID == IdNganh).Select(s => s.MaNganh).FirstOrDefault();

            int malop = db.tb_LopHoc.Where(s => s.ID_Nganh == IdNganh).Count();

            List<tb_LopHoc> danhsachLop = new List<tb_LopHoc>();

            for (int i = 0; i < soluongLop; i++)
            {
                tb_LopHoc lop = new tb_LopHoc();
                lop.MaLopHoc = "L" + ((DateTime.Now.Year) % 1000).ToString() + maNganh + (malop + 1).ToString("00");
                lop.ID_Nganh = IdNganh;
                lop.ID_HeDaoTao = IdHedaotao;
                lop.SoLuong = soluongSV;
                db.tb_LopHoc.Add(lop);
                malop++;
                danhsachLop.Add(lop);
            }
            try
            {
                db.SaveChanges();
            }
            catch
            {
                danhsachLop = null;
            }

            if (themCoVan != null)
            {
                themGVCV(IdNganh, IdHedaotao);
            }
            if (themSinhvien != null)
            {
                themSV(IdNganh, danhsachLop);
            }

            return RedirectToAction("DanhSachLopHoc");
        }


        public ActionResult XemDanhSachSinhVienLopHoc(int id)
        {
            var dsSinhVien = db.tb_SinhVien.Where(s => s.ID_LopHoc == id).ToList();
            ViewBag.idLop = id;
            return PartialView(dsSinhVien);
        }

        public ActionResult XemThongTinLop(int id)
        {
            var lop = db.tb_LopHoc.Find(id);
            ViewBag.SLSV = db.tb_SinhVien.Where(s => s.ID_LopHoc == lop.ID).Count();
            return View(lop);
        }



        public ActionResult SuaThongTinLop(int id)
        {
            var lophoc = db.tb_LopHoc.Find(id);
            ViewBag.GiangVien = new SelectList(db.tb_GiangVien, "ID", "tb_ThongTinCaNhan.HoTen", lophoc.ID_CoVan);
            //ViewBag.GiangVien = db.tb_GiangVien.Where(s => s.ID == lophoc.ID_Nganh).ToList();
            return PartialView(lophoc);

        }


        [HttpPost]
        public ActionResult SuaThongTinLop(FormCollection fields)
        {
            int ID = int.Parse(fields["ID"]);

            var lophoc = db.tb_LopHoc.Find(ID);

            if (lophoc != null)
            {
                lophoc.ID_CoVan = int.Parse(fields["GiangVien"]);

                db.Entry(lophoc).State = EntityState.Modified;

            }

            try
            {
                db.SaveChanges();
                Session["SuaThongTinLop"] = "Đã sửa thông tin Lớp " + lophoc.MaLopHoc;
            }
            catch
            {
                Session["LoiSuaThongTinLop"] = "Không thể sửa thông tin Lớp " + lophoc.MaLopHoc;
            }


            return RedirectToAction("DanhSachLopHoc");

        }




        public ActionResult XoaLop(int id)
        {
            var lop = db.tb_LopHoc.Find(id);

            return PartialView(lop);

        }


        [HttpPost]
        public ActionResult XoaLop(FormCollection fields)
        {
            int ID = int.Parse(fields["ID"]);
            var lop = db.tb_LopHoc.Find(ID);

            if (lop != null)
            {
                string malop = lop.MaLopHoc;
                db.tb_LopHoc.Remove(lop);

                try
                {
                    db.SaveChanges();
                    Session["XoaLop"] = "Đã xóa thành công lớp " + malop;
                }
                catch
                {
                    Session["LoiXoaLop"] = "Không thể xóa lớp " + malop;
                }
            }

            return RedirectToAction("DanhSachLopHoc");

        }

        public ActionResult ThemSinhVienLopHoc(int id, int idHeDaoTao)
        {
            ViewBag.IdLop = id;
            var sinhvien = db.tb_SinhVien.Where(s => s.ID_LopHoc == null && s.ID_HeDaoTao == idHeDaoTao && s.TrangThai == true).ToList();
            return PartialView(sinhvien);
        }


        [HttpPost]
        public ActionResult ThemSinhVienLopHoc(FormCollection fields, int[] masinhvien)
        {
            int idLop = int.Parse(fields["IdLop"]);
            var lop = db.tb_LopHoc.Where(s => s.ID == idLop).FirstOrDefault();

            if (lop != null)
            {
                if (masinhvien != null)
                {
                    foreach (var item in masinhvien)
                    {
                        var sinhvien = db.tb_SinhVien.Find(item);
                        sinhvien.ID_LopHoc = lop.ID;
                        db.Entry(sinhvien).State = EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }

            return RedirectToAction("XemThongTinLop", new { id = lop.ID });
        }

        public ActionResult XoaSinhVienLopHoc(int? id)
        {
            var sinhvien = db.tb_SinhVien.Find(id);
            ViewBag.idLop = sinhvien.ID_LopHoc;

            return PartialView(sinhvien);
        }
        [HttpPost]
        public ActionResult XoaSinhVienLopHoc(FormCollection fields)
        {
            int ID = int.Parse(fields["ID"]);
            int IDLop = int.Parse(fields["idLop"]);
            var sv = db.tb_SinhVien.Find(ID);

            if (sv != null)
            {
                sv.ID_LopHoc = null;
                db.Entry(sv).State = EntityState.Modified;
            }

            try
            {
                db.SaveChanges();
                Session["XoaSinhVienLopHoc"] = "Đã xóa sinh viên " + sv.tb_ThongTinCaNhan.Hoten + " mssv: " + sv.MaSinhVien;
            }
            catch
            {
                Session["LoiXoaSinhVienLopHoc"] = "Không thể xóa sinh viên " + sv.tb_ThongTinCaNhan.Hoten;
            }


            return RedirectToAction("XemThongTinLop", new { id = IDLop });
        }


        public void DanhSachSinhVien(int id)
        {
            var DSSV = db.tb_SinhVien.Where(s => s.ID_LopHoc == id).ToList();

            string malop = db.tb_LopHoc.Where(s => s.ID == id).Select(s => s.MaLopHoc).FirstOrDefault();

            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách");



            ws.Cells["A1"].Value = "Danh sách sinh viên lớp " + malop;
            ws.Cells["A2"].Value = "Ngày lập danh sách";
            ws.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} at {0:H : mm tt}", DateTimeOffset.Now);
            ws.Cells["A4"].Value = "Mã sinh viên";
            ws.Cells["B4"].Value = "Họ tên";
            ws.Cells["C4"].Value = "Giới tính";
            ws.Cells["D4"].Value = "Ngày sinh";
            ws.Cells["E4"].Value = "Số điện thoại";
            ws.Cells["F4"].Value = "Email";
            ws.Cells["G4"].Value = "Ngành";
            ws.Cells["H4"].Value = "Hệ đào tạo";

            ws.Row(4).Height = 20;
            ws.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(4).Style.Font.Bold = true;

            int rowStart = 5;
            foreach (var item in DSSV)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.MaSinhVien;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.tb_ThongTinCaNhan.Hoten;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.tb_ThongTinCaNhan.GioiTinh == true ? "Nam" : "Nữ";
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.tb_ThongTinCaNhan.NgaySinh.ToString("dd/MM/yyyy");
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.tb_ThongTinCaNhan.SDT;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.tb_ThongTinCaNhan.Email_2;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.tb_Nganh.TenNganh;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.tb_HeDaoTao.TenHeDaoTao;

                rowStart++;

            }
            ws.Cells["A:AZ"].AutoFitColumns();
            string excelName = "Danh sách sinh viên lớp " + malop;
            using (var memoryStream = new MemoryStream())
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment; filename=" + excelName + ".xlsx");
                pck.SaveAs(memoryStream);
                memoryStream.WriteTo(Response.OutputStream);
                Response.Flush();
                Response.End();
            }

            //Response.Clear();
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("Content-Dispostion", "attachment; filename=DanhSachSinhVien.xlsx");
            //Response.BinaryWrite(pck.GetAsByteArray());
            //Response.End();

        }

    }
}