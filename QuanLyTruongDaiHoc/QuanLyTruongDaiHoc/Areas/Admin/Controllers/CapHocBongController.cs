using OfficeOpenXml;
using OfficeOpenXml.Style;
using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class CapHocBongController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/CapHocBong/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        // GET: Admin/CapHocBong
        public ActionResult DanhSachSinhVien()
        {
            if (!checkLogin("DanhSachSinhVien"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            ViewBag.Khoa = db.tb_KhoaVien.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult CapHocBong(FormCollection collection)
        {
            var dssv = db.tb_SinhVien;
            int count = 0;

            foreach (var item in dssv)
            {
                if (!string.IsNullOrEmpty(collection["sv-" + item.ID])) 
                {
                    tb_CapHocBong chb = new tb_CapHocBong();
                    chb.ID_SinhVien = item.ID;
                    chb.ID_HocBong = Init.getIntHB(Init.strHBHSGioi);
                    var hkAndNam = Init.calculateHocKiNam(item.NgayNhapHoc);
                    chb.HocKi = hkAndNam.Item1; //int.Parse(collection["Hocki"]);
                    chb.Nam = hkAndNam.Item2; // int.Parse(collection["Nam"]);
                    db.tb_CapHocBong.Add(chb);
                    count++;
                }
            }
            db.SaveChanges();
            Session["SuccessCapHocBong"] = "Đã cấp học bổng cho " + count + " sinh viên!";
            return RedirectToAction("DanhSachSinhVien");
        }

        public void InDanhSach()
        {
            var hb = db.tb_CapHocBong.ToList();
            foreach (var item in hb)
            {
                var hockiAndNam = Init.calculateHocKiNam(item.tb_SinhVien.NgayNhapHoc);
                if (item.HocKi != hockiAndNam.Item1 || item.Nam != hockiAndNam.Item2)
                {
                    hb.Remove(item);
                }
            }

            ExcelPackage pck = new ExcelPackage();

            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Danh sách");



            ws.Cells["A1"].Value = "Danh sách cấp học bổng";
            ws.Cells["A2"].Value = "Ngày lập danh sách";
            ws.Cells["B2"].Value = string.Format("{0:dd MMMM yyyy} at {0:H : mm tt}", DateTimeOffset.Now);
            ws.Cells["A4"].Value = "SST";
            ws.Cells["B4"].Value = "Mã sinh viên";
            ws.Cells["C4"].Value = "Họ tên";
            ws.Cells["D4"].Value = "Loại học bổng";
            ws.Cells["E4"].Value = "Học kì";
            ws.Cells["F4"].Value = "Năm";
            ws.Cells["G4"].Value = "Ngành";

            ws.Row(4).Height = 20;
            ws.Row(4).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            ws.Row(4).Style.Font.Bold = true;

            int rowStart = 5;
            int i = 1;
            foreach (var item in hb)
            {
                ws.Cells[string.Format("A{0}", rowStart)].Value =i;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.tb_SinhVien.MaSinhVien;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.tb_SinhVien.tb_ThongTinCaNhan.Hoten;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.tb_HocBong.Ten;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.HocKi;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.Nam;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.tb_SinhVien.tb_Nganh.TenNganh;
                i++;
                rowStart++;

            }
            ws.Cells["A:AZ"].AutoFitColumns();
            string excelName = "Danh sách cấp học bổng ngày " + string.Format("{0:dd MMMM yyyy} at {0:H : mm tt}", DateTimeOffset.Now);
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