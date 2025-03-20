using QuanLyTruongDaiHoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyTruongDaiHoc.Areas.Admin.Controllers
{
    public class DiemRenLuyenController : Controller
    {
        private QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
        // GET: Admin/DiemRenLuyen
        bool checkLogin(string prevUrl)
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/DiemRenLuyen/" + prevUrl;
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return false;
            }
            return true;
        }

        public ActionResult XemDanhSachRenLuyen() 
        {
            if (!checkLogin("XemDanhSachRenLuyen"))
            {
                return RedirectToAction("DangNhap", "Admin");
            }
            var ds = db.tb_DanhGiaRL;
            return View(ds);
        }


        public ActionResult XemChiTietDGDRL(int? id)
        {
            var dgdrl = db.tb_DanhGiaRL.SingleOrDefault(n => n.ID == id);
            ViewBag.NoiDungRL = db.tb_NoiDungRenLuyen.ToList();
            return PartialView(dgdrl);
        }

        [HttpPost]
        public ActionResult SuaDGDRL(FormCollection collection)
        {
            tb_TaiKhoan tk = (tb_TaiKhoan)Session["TaikhoanQT"];
            tb_QuanTri qt = db.tb_QuanTri.SingleOrDefault(n => n.tb_TaiKhoan.ID == tk.ID);
            int? id = int.Parse(collection["id_dgdrl"]);
            var dgrl = db.tb_DanhGiaRL.SingleOrDefault(n => n.ID == id);
            dgrl.ID_QuanTri = qt.ID;
            db.SaveChanges();
            var dsct = db.tb_ChiTietDanhGiaRL.Where(n => n.ID_DanhGiaRL == id);
            int tongdiem = 0;
            foreach (var item in dsct)
            {
                //int diemsv = int.Parse(collection["diemsvdanhgia-" + item.ID]);
                //int diemgv = int.Parse(collection["diemgvdanhgia-" + item.ID]);
                int diemqt = int.Parse(collection["diemqtdanhgia-" + item.ID]);
                //item.DiemSVDanhGia = diemsv;
                //item.DiemGVDanhGia = diemgv;
                item.DiemQTDanhGia = diemqt;
                db.Entry(item).State = System.Data.Entity.EntityState.Modified;
                tongdiem += diemqt;
            }
            db.SaveChanges();

            var ndrl = db.tb_NoiDungRenLuyen;
            int? tongtoida = 0;
            if (ndrl != null)
            {
                foreach (var item in ndrl)
                {
                    tongtoida += item.TongToiDa;
                }
            }
            dgrl.TongToiDa = tongtoida;
            dgrl.TongDiem = tongdiem;
            dgrl.XepLoai = Init.getStrXepLoaiRL(tongdiem, tongtoida);
            db.Entry(dgrl).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("XemDanhSachRenLuyen");
        }
        public ActionResult XemDanhSachNoiDungRenLuyen()
        {
            if (Session["TaikhoanQT"] == null)
            {
                Session["PreviousURL"] = "~/Admin/DiemRenLuyen/XemDanhSachNoiDungRenLuyen";
                Session["LoaiTK"] = Init.TaiKhoanQT;
                return Redirect("~/Admin/DangNhap");
            }
            var ndrl = db.tb_NoiDungRenLuyen;
            return View(ndrl);
        }

        [HttpPost]
        public ActionResult ThayDoiNoiDungRL(FormCollection collection)
        {
            bool fail_nd = false, fail_ct = false;
            bool fail_delete_nd = false, fail_delete_ct = false;
            var ndrl = db.tb_NoiDungRenLuyen;
            if (ndrl != null)
            {
                List<tb_NoiDungRenLuyen> lstndrl = ndrl.ToList();
                for (int i = 0; i < lstndrl.Count(); i++)
                {
                    int tmpIDND = lstndrl[i].ID;
                    tb_NoiDungRenLuyen nd = db.tb_NoiDungRenLuyen.SingleOrDefault(n => n.ID == tmpIDND);

                    if (string.IsNullOrEmpty(collection["hidden-" + nd.ID]))
                    {
                        if (db.tb_ChiTietDanhGiaRL.SingleOrDefault(n => n.tb_ChiTietNoiDungRenLuyen.tb_NoiDungRenLuyen.ID == tmpIDND) == null)
                        {
                            var chitietnd = db.tb_ChiTietNoiDungRenLuyen.Where(n => n.ID_NoiDungRL == tmpIDND);
                            db.tb_ChiTietNoiDungRenLuyen.RemoveRange(chitietnd);
                            db.SaveChanges();
                            db.tb_NoiDungRenLuyen.Remove(nd);
                        }
                        else
                        {
                            fail_delete_nd = true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(collection["TenND-" + nd.ID]))
                        {
                            fail_nd = true;
                        }
                        else
                        {
                            nd.TenNoiDung = collection["TenND-" + nd.ID];
                        }
                        if (string.IsNullOrEmpty(collection["MaND-" + nd.ID]))
                        {
                            fail_nd = true;
                        }
                        else
                        {
                            nd.MaNoiDung = collection["MaND-" + nd.ID];
                        }
                        db.Entry(nd).State = System.Data.Entity.EntityState.Modified;

                        if (!(string.IsNullOrEmpty(collection["Machitietrl-new-" + nd.ID]) && string.IsNullOrEmpty(collection["Tenchitietrl-new-" + nd.ID]) && (string.IsNullOrEmpty(collection["Diemtoidarl-new-" + nd.ID]) || collection["Diemtoidarl-new-" + nd.ID] == "0")))
                        {
                            if (string.IsNullOrEmpty(collection["Machitietrl-new-" + nd.ID]) || string.IsNullOrEmpty(collection["Tenchitietrl-new-" + nd.ID]) || string.IsNullOrEmpty(collection["Diemtoidarl-new-" + nd.ID]))
                            {
                                fail_ct = true;
                            }
                            else
                            {
                                tb_ChiTietNoiDungRenLuyen chitiet = new tb_ChiTietNoiDungRenLuyen();
                                chitiet.ID_NoiDungRL = nd.ID;
                                chitiet.MaChiTiet = collection["Machitietrl-new-" + nd.ID];
                                chitiet.TenChiTiet = collection["Tenchitietrl-new-" + nd.ID];
                                chitiet.DiemToiDa = int.Parse(collection["Diemtoidarl-new-" + nd.ID]);
                                db.tb_ChiTietNoiDungRenLuyen.Add(chitiet);
                            }
                        }
                    }
                }
                db.SaveChanges();
            }
            
            var chitiets = db.tb_ChiTietNoiDungRenLuyen;
            if (chitiets != null)
            {
                List<tb_ChiTietNoiDungRenLuyen> lstchitiets = chitiets.ToList();
                for (int j = 0; j < lstchitiets.Count(); j++)
                {
                    int? id = lstchitiets[j].ID;
                    tb_ChiTietNoiDungRenLuyen chitiet = db.tb_ChiTietNoiDungRenLuyen.SingleOrDefault(n => n.ID == id);
                    if (string.IsNullOrEmpty(collection["Diemtoidarl-" + lstchitiets[j].ID]))
                    {
                        if (db.tb_ChiTietDanhGiaRL.SingleOrDefault(n => n.ID_ChiTietNoiDung == chitiet.ID) == null)
                        {
                            db.tb_ChiTietNoiDungRenLuyen.Remove(chitiet);
                        }
                        else
                        {
                            fail_delete_ct = true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(collection["Tenchitietrl-" + chitiet.ID]))
                        {
                            fail_ct = true;
                        }
                        else
                        {
                            chitiet.TenChiTiet = collection["Tenchitietrl-" + chitiet.ID];
                        }
                        if (string.IsNullOrEmpty(collection["Machitietrl-" + chitiet.ID]))
                        {
                            fail_ct = true;
                        }
                        else
                        {
                            lstchitiets[j].MaChiTiet = collection["Machitietrl-" + chitiet.ID];
                        }
                        chitiet.DiemToiDa = int.Parse(collection["Diemtoidarl-" + chitiet.ID]);
                        db.Entry(chitiet).State = System.Data.Entity.EntityState.Modified;
                    }
                }
                db.SaveChanges();
            }
            if (ndrl != null)
            {
                foreach (var nd in ndrl)
                {
                    int? tong = 0;
                    foreach (var ct in nd.tb_ChiTietNoiDungRenLuyen)
                    {
                        tong += ct.DiemToiDa;
                    }
                    nd.TongToiDa = tong;
                    db.Entry(nd).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }

            if (fail_nd)
            {
                Session["ErrorNoiDungRL"] =  "Các nội dung đánh giá không hợp lệ sẽ không được lưu";
            }
            else if (fail_ct)
            {
                Session["ErrorNoiDungRL"] = "Các chi tiết nội dung đánh giá không hợp lệ sẽ không được lưu";
            }
            else if (fail_delete_nd)
            {
                Session["ErrorNoiDungRL"] = "Không thể xoá các nội dung đang được dùng cho một số bài đánh giá";
            }
            else if(fail_delete_ct)
            {
                Session["ErrorNoiDungRL"] = "Không thể xoá các chi tiết nội dung đang được dùng cho một số bài đánh giá";
            }
            else
            {
                Session["SuccessThayDoiNoiDung"] = "Thay đổi nội dung đánh giá thành công";
            }
            return RedirectToAction("XemDanhSachNoiDungRenLuyen");
        }

        public ActionResult ThemMoiNoiDungRL()
        {
            ViewBag.MaND = "ND" + ConvertNumber.getStrDecimal(db.tb_NoiDungRenLuyen.Count() + 1, 2);
            return PartialView();
        }

        [HttpPost]
        public ActionResult ThemMoiNoiDungRL(FormCollection collection)
        {
            tb_NoiDungRenLuyen nd = new tb_NoiDungRenLuyen();
            if (string.IsNullOrEmpty(collection["Tennd"]))
            {
                Session["Error_ThemNoiDungRL"] = "Nội dung rèn luyện cần nhập đầy đủ mã chi tiết và tên nội dung";
                return RedirectToAction("XemDanhSachNoiDungRenLuyen");
            }
            nd.MaNoiDung = collection["hidden"];
            nd.TenNoiDung = collection["Tennd"];
            db.tb_NoiDungRenLuyen.Add(nd);
            bool haderrordetail = false;
            int tongdiem = 0;
            foreach (string key in collection.Keys)
            {
                if (key.StartsWith("Tenct")) // hoặc key.Contains("a")
                {
                    int number = int.Parse(key.Split('-')[1]);
                    string tenct = collection[key];
                    string mact = "CT" + ConvertNumber.getStrDecimal(number, 2);
                    int diemtoida = int.Parse(collection["Diemtoida-" + number]);
                    if (string.IsNullOrEmpty(collection["Tenct-" + number]))
                    {
                        haderrordetail = true;
                    }
                    else
                    {
                        tb_ChiTietNoiDungRenLuyen chitiet = new tb_ChiTietNoiDungRenLuyen();
                        chitiet.ID_NoiDungRL = nd.ID;
                        chitiet.MaChiTiet = mact;
                        chitiet.TenChiTiet = tenct;
                        chitiet.DiemToiDa = diemtoida;
                        db.tb_ChiTietNoiDungRenLuyen.Add(chitiet);
                        tongdiem += diemtoida;
                    }
                }
            }
            db.SaveChanges();
            nd.TongToiDa = tongdiem;
            db.SaveChanges();

            if (haderrordetail)
            {
                Session["Error_ThemChiTietRL"] = "Hệ thống sẽ không lưu các chi tiết không nhập đầy đủ";
            }
            else
            {
                Session["Success_ThemNoiDungRL"] = "Thêm nội dung thành công";
            }
            return RedirectToAction("XemDanhSachNoiDungRenLuyen");
        }
    }
}