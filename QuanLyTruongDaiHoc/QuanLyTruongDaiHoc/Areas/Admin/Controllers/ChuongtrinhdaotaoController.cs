using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyTruongDaiHoc.Models;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class ChuongtrinhdaotaoController : Controller
    {

        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/Chuongtrinhdaotao/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }
        // GET: Admin/Chuongtrinhdaotao
        public ActionResult DanhSachChuongTrinhDaoTao()
        {
            if (!checkLogin("DanhSachChuongTrinhDaoTao"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var nganhdaotao = db.tb_Nganh.Include(t=>t.tb_KhoaVien);
            var tb_Chuongtrinhdaotao = db.tb_Chuongtrinhdaotao.Include(t => t.tb_Monhoc).Include(t => t.tb_Nganh);
            

            return View(nganhdaotao.ToList());
        }

        public ActionResult XemCTDT(int? id)
        {
            var HocKi = db.tb_Chuongtrinhdaotao.Where(s => s.tb_Nganh.ID == id).Select(hk => new {hk.Hocki, hk.Nam }).Distinct().OrderBy(hk=>hk.Nam).ToList();
            List<NamHoc> namhoc = new List<NamHoc>();
           
            foreach(var item in HocKi)
            {
                namhoc.Add(new NamHoc { Hocki = (int)item.Hocki, Nam = (int)item.Nam });
            }
            ViewBag.HocKi = namhoc;
            ViewBag.Nganh = db.tb_Nganh.SingleOrDefault(n => n.ID == id);
            //foreach(var item in HocKi)
            //{
            //    item.Hocki
            //}
            var ctdt = db.tb_Chuongtrinhdaotao.Include(t => t.tb_Monhoc).Include(t => t.tb_Nganh).Where(ct=>ct.MaNganh == id);
            return View(ctdt.ToList());
        }

        [HttpPost]
        public ActionResult LuuThayDoiCTDT(FormCollection collection)
        {
            int? idnganh = int.Parse(collection["Manganh"]);
            var ctdt = db.tb_Chuongtrinhdaotao.Where(n => n.MaNganh == idnganh);

            if (ctdt != null)
            {
                foreach (var item in ctdt)
                {
                    if (string.IsNullOrEmpty(collection["hidden-" + item.ID]))
                    {
                        db.tb_Chuongtrinhdaotao.Remove(item);
                    }
                }
                try
                {
                    db.SaveChanges();
                    Session["SuccessLuuThayDoi"] = "Lưu thay đổi thành công";
                }
                catch
                {
                    Session["ErrorLuuThayDoiCTDT"] = "Các chương trình đào tạo đang được sử dụng sẽ không bị xoá";
                }
            }
            return RedirectToAction("XemCTDT", "Chuongtrinhdaotao", new { id = idnganh });
        }


        public ActionResult ThemChuongtrinhDaotao(int? MaNganh)
        {
            var hocki = db.tb_HocKi.FirstOrDefault();
            if (hocki != null)
            {
                ViewBag.HocKiToiDa = hocki.HocKiToiDa;
            }
            var dsmh = db.tb_Monhoc.Where(n => n.ChuyenNganh == MaNganh);

            if (dsmh != null)
            {
                var dsctdt = db.tb_Chuongtrinhdaotao.Where(n => n.MaNganh == MaNganh);
                var mh = new List<tb_Monhoc>();
                
                foreach (var item in dsmh)
                {
                    if (dsctdt.SingleOrDefault(n => n.MaMonHoc == item.ID) == null)
                    {
                        mh.Add(item);
                    }
                }

                ViewBag.MonHoc = mh;
            }

            ViewBag.Nganh = db.tb_Nganh.SingleOrDefault(n => n.ID == MaNganh);
            return PartialView(new tb_Chuongtrinhdaotao());
        }


        [HttpPost]
        public ActionResult ThemChuongtrinhDaotao(FormCollection collection)
        {
            int manganh = int.Parse(collection["Manganh"]);
            int hocki = int.Parse(collection["Hocki"]);
            int nam = int.Parse(collection["Nam"]);
            bool batbuoc = collection["Batbuoc"] == "1";

            var dsmh = db.tb_Monhoc.Where(n => n.ChuyenNganh == manganh);
            if (dsmh != null)
            {
                foreach (var item in dsmh)
                {
                    if (string.IsNullOrEmpty(collection["Monhoc-" + item.ID]))
                    {
                        continue;
                    }
                    bool check = collection["Monhoc-" + item.ID] == "on";
                    if (check)
                    {
                        tb_Chuongtrinhdaotao ctdt = new tb_Chuongtrinhdaotao();
                        ctdt.MaMonHoc = item.ID;
                        ctdt.MaNganh = manganh;
                        ctdt.Hocki = hocki;
                        ctdt.Nam = nam;
                        ctdt.BatBuoc = batbuoc;
                        db.tb_Chuongtrinhdaotao.Add(ctdt);
                    }
                }
                db.SaveChanges();
            }
            return RedirectToAction("XemCTDT", "Chuongtrinhdaotao", new { id = manganh });
        }
    }
}
