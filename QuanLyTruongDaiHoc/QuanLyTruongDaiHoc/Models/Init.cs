using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyTruongDaiHoc.Models
{
    public class Init
    {
        //Account
        public static string TaiKhoanQT = "Quản Trị", TaiKhoanGV = "Giảng Viên", TaiKhoanSV = "Sinh Viên";

        //Yêu cầu cấp giấy và xin nghỉ phép
        public static int DaDuyet = 1, DoiDuyet = 0, TuChoiDuyet = -1;
        public static string StrDaDuyet = "Đã duyệt", StrDoiDuyet = "Đang chờ", StrTuChoiDuyet = "Từ chối";

        public static string getStrTrangThai(int? trangthai)
        {
            if (trangthai == DaDuyet)
            {
                return StrDaDuyet;
            }
            if (trangthai == DoiDuyet)
            {
                return StrDoiDuyet;
            }
            return StrTuChoiDuyet;
        }

        //Thông báo
        public static int ThongBaoSV = 1, ThongBaoGV = 0, ThongBaoAll = -1;
        public static string StrThongBaoSV = "Sinh viên", StrThongBaoGV = "Giảng viên", StrThongBaoAll = "Tất cả";


        public static string getStrThongBao(int? thongBao)
        {
            if (thongBao == ThongBaoSV)
            {
                return StrThongBaoSV;
            }
            if (thongBao == ThongBaoGV)
            {
                return StrThongBaoGV;
            }
            return StrThongBaoAll;
        }

        //Đánh giá rèn luyện
        public static string LoaiXuatSac = "Xuất sắc", LoaiGioi = "Giỏi", LoaiKha = "Khá", LoaiTB = "Trung bình", LoaiYeu = "Yếu";

        public static string getStrXepLoaiRL(int? tongdiem, int? tongdiemToida)
        {
            float? diem = (tongdiem / (float) tongdiemToida) * 10;
            if (diem < 5)
            {
                return LoaiYeu;
            }
            if (diem < 6.5)
            {
                return LoaiTB;
            }
            if (diem < 8)
            {
                return LoaiKha;
            }
            if (diem < 9)
            {
                return LoaiGioi;
            }
            return LoaiXuatSac;
        }

        //Môn học
        public static int LyThuyet = 1, ThucHanh = 0;
        public static string strLyThuyet = "Lý thuyết", strThucHanh = "Thực hành";

        public static string getStrLoaiMon(int? loaiMon)
        {
            if (loaiMon == LyThuyet)
            {
                return strLyThuyet;
            }
            return strThucHanh;
        }
        public static bool BatBuoc = true, KhongBatBuoc = false;
        public static string strBatBuoc = "Bắt buộc", strKhongBatBuoc = "Không bắt buộc";
        public static string getStrBatBuoc(bool batbuoc)
        {
            if (batbuoc)
            {
                return strBatBuoc;
            }
            return strKhongBatBuoc;
        }

        //Thông tin cá nhân
        public static bool Nam = true, Nu = false;
        public static string strNam = "Nam", strNu = "Nữ";
        public static string getStrGioiTinh(bool gioiTinh)
        {
            if (gioiTinh)
            {
                return strNam;
            }
            return strNu;
        }

        //Khoá học và phòng học
        public static int HoatDong = 1;
        public static string strHoatDong = "Hoạt động";

        //Khoá học
        public static int DaKetThuc = 0;
        public static string strDaKetThuc = "Đã kết thúc";
        public static string getStrTrangThaiKH(int? trangthai)
        {
            if (trangthai ==  HoatDong)
            {
                return strHoatDong;
            }
            return strDaKetThuc;
        }


        //Phòng học
        public static int BaoTri = 0;
        public static string strBaoTri = "Bảo trì";
        public static string getStrTrangThaiPH(int? trangthai)
        {
            if (trangthai == HoatDong)
            {
                return strHoatDong;
            }
            return strBaoTri;
        }

        //Chấm điểm
        public static bool Dau = true, Rot = false;
        public static string strDau = "Hoàn thành", strChuaHoanThanh = "Chưa hoàn thành";
        public static string getStrKetQua(bool? ketqua)
        {
            if (ketqua == true)
            {
                return strDau;
            }
            return strChuaHoanThanh;
        }

        //Tính học kì và năm
        public static (int, int) calculateHocKiNam(DateTime? ngayNhapHoc)
        {
            if (ngayNhapHoc == null)
            {
                return (-1, -1);
            }
            string tmp = ngayNhapHoc.ToString();
            DateTime ngayhientai = DateTime.Now;
            DateTime ngayNH = DateTime.Parse(tmp);
            int namnhaphoc = ngayNH.Year;
            int namhientai = DateTime.Now.Year;
            int nam = namhientai - namnhaphoc + 1;
            TimeSpan duration = ngayhientai - ngayNH;
            int soNgayDaHoc = (int)duration.TotalDays;
            int hocki = (soNgayDaHoc / 120) + 1;
            return (hocki, nam);
        }

        //CHương trình đào tạo
        public static int? calculatSoTCYC(List<tb_Chuongtrinhdaotao> ctdt)
        {
            int? soTCYC = 0;
            foreach (var item in ctdt)
            {
                if (item.BatBuoc == BatBuoc)
                {
                    soTCYC += item.tb_Monhoc.SoTC;
                }
            }
            return soTCYC;
        }

        //Đăng ký môn
        public static int? calculateSoTC(List<tb_DangKyMonHoc> dkmh)
        {
            int? soTC = 0;
            foreach (var item in dkmh)
            {
                soTC += item.tb_KhoaHoc.tb_Monhoc.SoTC;
            }
            return soTC;
        }
        public static double? calcutelateDiem(List<tb_DangKyMonHoc> dkmh)
        {
            int? tongTC = calculateSoTC(dkmh);
            if (tongTC == null || tongTC == 0)
            {
                return 0;
            }
            double? diem = 0;
            QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
            var phanTram = db.tb_QuyTacTinhDiem.SingleOrDefault();

            foreach (var item in dkmh)
            {
                diem += item.tb_KhoaHoc.tb_Monhoc.SoTC * (item.diemKTDK * phanTram.PhanTramKTDK / 100 + item.diemKTHP * phanTram.PhanTramKTHP/100);
            }
            return diem / (double) tongTC;
        }

        //Học bổng
        public static string strHBHSGioi = "Học sinh giỏi";
        public static int getIntHB (string hb)
        {
            QuanLyDaiHocEntities db = new QuanLyDaiHocEntities();
            return db.tb_HocBong.SingleOrDefault(n => n.Ten == hb).ID;
        }

    }
}