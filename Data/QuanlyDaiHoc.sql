create database QuanLyDaiHoc1
use QuanLyDaiHoc1

create table tb_HeDaoTao(
	ID int identity(1,1) primary key,
	MaHeDaoTao varchar(10),
	TenHeDaoTao nvarchar(100),
)

create table tb_KhoaVien(
	ID	int identity(1,1) primary key,
	MaKhoaVien	varchar(20),
	TenKhoaVien nvarchar(100),
	Mota ntext,
)

create table tb_Nganh(
	ID	int identity(1,1) primary key,
	MaNganh varchar(20),
	TenNganh	nvarchar(100),
	MaKhoaVien int references tb_KhoaVien(ID),
	NamDaotao	float,
	Mota		ntext,
)

create table tb_Monhoc(
	ID	int identity(1,1) primary key,
	MaMonHoc varchar(10),
	TenMonHoc nvarchar(100),
	ChuyenNganh	int references tb_Nganh(ID),
	LoaiMonHoc	int, --1: lt, 0: th
	SoTC		int,
	SoTiet	int,
)

create table tb_Chuongtrinhdaotao(
	ID	int identity(1,1) primary key,
	MaNganh int references tb_Nganh(ID),
	MaMonHoc int references tb_MonHoc(ID),
	BatBuoc bit,
	Hocki int,
	Nam	int,
)

create table tb_ThongTinCaNhan(
	ID int identity(1,1) primary key not null,
	Hoten nvarchar(200) not null,
	NgaySinh datetime not null,
	SDT varchar(20),
	DiaChi nvarchar(500),
	Email_1 varchar(200) ,
	Email_2 varchar(200) ,
	So_CCCD varchar(20) ,
	DanToc nvarchar(20) not null, --Kinh | ....
	GioiTinh bit not null default 1, --1: nam, 0: nữ
	Anh nvarchar(500),
)


create table tb_TaiKhoan(
	ID int identity(1,1) primary key not null,
	TenDangNhap varchar(200) not null unique,
	MatKhau varchar(200) not null,
	LoaiTaiKhoan nvarchar(50), --Quản trị | Giảng viên | Sinh viên
	Anh ntext,
)

create table tb_GiangVien(
	ID int identity(1,1) primary key not null,
	MaGiangVien varchar(100),
	TrinhDo nvarchar(100),
	ChuongTrinhGiangDay nvarchar(100),
	ID_ThongTinCaNhan int references tb_ThongTinCaNhan(ID),
	ID_TaiKhoan int references tb_TaiKhoan(ID),
	ID_ChuyenNganh int references tb_Nganh(ID),
	TrangThai bit
)

create table tb_MonHocCoTheDay
(
	ID int identity(1,1) primary key,
	ID_GiangVien int references tb_GiangVien(ID),
	ID_MonHoc int references tb_MonHoc(ID),
);


create table tb_LopHoc
(
	ID int not null primary key identity(1,1),
	MaLopHoc varchar(100),
	ID_Nganh int not null references tb_Nganh(ID),
	ID_HeDaoTao int references tb_HeDaoTao(ID),
	ID_CoVan int references tb_GiangVien(ID),
	SoLuong int,
);


create table tb_SinhVien(
	ID int identity(1,1) primary key not null,
	MaSinhVien varchar(100),
	SoTC_TichLuy int not null default 0,
	GiaoDucTheChat_LT bit not null default 0,
	GiaoDucTheChat_TH bit not null default 0,
	GDQPAN bit not null default 0,
	SoMon_F int not null default 0,
	SoCap_NgoaiNgu int not null default 0,
	KyNang bit,
	DiemTichLuy float,
	TotNghiep bit not null default 0,
	TrangThai bit,
	ID_ThongTinCaNhan int references tb_ThongTinCaNhan(ID),
	ID_TaiKhoan int references tb_TaiKhoan(ID),
	ID_ChuyenNganh int references tb_Nganh(ID),
	ID_HeDaoTao int references tb_HeDaoTao(ID),
	ID_LopHoc int references tb_LopHoc(ID),
	NgayNhapHoc datetime,
);

create table tb_GiaoDucTheChatLT
(
	ID_MonHoc int primary key references tb_MonHoc(ID),
);
create table tb_GiaoDucTheChatTH
(
	ID_MonHoc int primary key references tb_MonHoc(ID),
);
create table tb_GDQPAN
(
	ID_MonHoc int primary key references tb_MonHoc(ID),
);
create table tb_NgoaiNguKhongChuyen
(
	ID_MonHoc int primary key references tb_MonHoc(ID),
	Cap int,
);
create table tb_KyNang
(
	ID_MonHoc int primary key references tb_MonHoc(ID),
	Cap int,
);

create table tb_QuanTri(
	ID int identity(1,1) primary key not null,
	MaQuanTri varchar(100),
	ID_ThongTinCaNhan int references tb_ThongTinCaNhan(ID),
	ID_TaiKhoan int references tb_TaiKhoan(ID),
)

create table tb_LoaiPhong
(
	ID int not null primary key identity,
	Ten nvarchar(200), --Phòng thường | Phòng máy thường | Phòng máy vip | ....
);


create table tb_PhongHoc
(
	ID int not null primary key identity,
	MaPhongHoc varchar(100) unique,
	ViTri ntext not null,
	ID_LoaiPhong int references tb_LoaiPhong(ID),
	SucChua int not null default 0,
	TrangThai bit not null default N'Cho phép sử dụng', --Cho phép sử dụng | Đang bảo trì | ....
);

create table tb_KhoaHoc
(
	ID int not null primary key identity,
	MaKhoaHoc varchar(200) not null unique,
	ID_MonHoc int not null references tb_MonHoc(ID),
	ID_HeDaoTao int references tb_HeDaoTao(ID),
	ID_GiangVien int not null references tb_GiangVien(ID),
	SoLuongYeuCau int not null default 0,
	SoLuongHienTai int not null default 0,
	
	NgayMo datetime,
	TrangThai int, --1: Hoạt động, --0: Đã kết thúc
);

create table tb_ChiTietKhoaHoc(
	ID int identity(1,1) primary key,
	ID_KhoaHoc int references tb_KhoaHoc(ID),
	ID_PhongHoc int references tb_PhongHoc(ID),
	Ngay date,
	TietBatDau int,
	TietKetThuc int,
	soTiet int,
)

create table tb_DangKyMonHoc
(
	ID int identity(1,1)  primary key,
	ID_KhoaHoc int not null references tb_KhoaHoc(ID),
	ID_MonHoc int references tb_MonHoc(ID),
	ID_SinhVien int not null references tb_SinhVien(ID),
	HocKi int,
	Nam int,
	diemKTDK float,
	diemKTHP float,
	KetQua bit,
);

create table tb_QuyTacTinhDiem
(
	ID int primary key identity(1,1),
	PhanTramKTDK int,
	PhanTramKTHP int,
);

create table tb_ThongTinNgayThi
(
	ID int identity (1,1) primary key,
	ID_KhoaHoc int references tb_KhoaHoc(ID),
	ID_GiamThi1 int references tb_GiangVien(ID),
	ID_GiamThi2 int references tb_GiangVien(ID),
	PhongThi int references tb_PhongHoc(ID),
	NgayThi date,
	GioBatDau nvarchar(20),
	ThoiLuong int,
);

create table tb_ChiTietLichThi(
	ID int identity(1,1) primary key,
	ID_ThongTinNgayThi int references tb_ThongTinNgayThi(ID),
	ID_SinhVien int references tb_SinhVien(ID),
)

create table tb_LichDangKyMon(
	ID int identity(1,1) primary key,
	TGBatDau datetime,
	TGKetThuc datetime,
	HocKi int,
	Nam int,
)

create table tb_HocPhiTC
(
	ID_HeDaoTao int not null references tb_HeDaoTao(ID),
	ID_Nganh int  references tb_Nganh(ID),
	Gia money not null default 0,
	Primary key (ID_Nganh, ID_HeDaoTao),
);

create table tb_ThongBao
(	
	ID int not null primary key identity(1,1),
	ID_QuanTri int not null references tb_QuanTri(ID),
	TieuDe nvarchar(200) not null,
	NoiDung ntext,
	DoiTuongNhanTB int not null, -- 1: Sinh viên | 0: Giảng viên | -1: Toàn bộ 
	NgayGui datetime,
);

create table tb_NoiDungKhaoSat(
	ID int identity(1,1) primary key,
	MaNoiDung varchar(20),
	TenNoiDung nvarchar(200),
)

create table tb_ChiTietNoiDungKhaoSat(
	ID int identity(1,1) primary key,
	MaNoiDung int references tb_NoiDungKhaoSat(ID),
	MaChiTiet varchar(20),
	TenChiTiet nvarchar(200),	
)

create table tb_KhaoSatKhoaHoc(
	ID int identity(1,1) primary key,
	ID_DangKyMonHoc int references tb_DangKyMonHoc(ID),
	ID_SinhVien int references tb_SinhVien(ID),
	TrangThai bit,
)

create table tb_GiayChungNhan
(
	ID int not null primary key identity(1,1),
	Ten nvarchar(200) not null unique,
);

create table tb_YeuCauGiayChungNhan
(
	ID int not null primary key identity(1,1),
	ID_GiayChungNhan int not null references tb_GiayChungNhan(ID),
	ID_SinhVien int not null references tb_SinhVien(ID),
	ThongTinYeuCau ntext,
	TrangThai int not null default 0, --'Đợi duyệt | Đã duyệt | Từ chối
	PhanHoi ntext,
	ID_QuanTri int references tb_QuanTri(ID),
	NgayYeuCau datetime,
	NgayDuyet datetime,
);



create table tb_LichDanhGiaRL
(
	ID int identity(1,1) primary key,
	NgayMo datetime,
	NgayDong datetime,
);


create table tb_NoiDungRenLuyen(
	ID int identity(1,1) primary key,
	MaNoiDung varchar(20),
	TenNoiDung nvarchar(200),
	TongToiDa int,
)

create table tb_ChiTietNoiDungRenLuyen(
	ID int identity(1,1) primary key,
	ID_NoiDungRL int not null references tb_NoiDungRenLuyen(ID),
	MaChiTiet varchar(20),
	TenChiTiet nvarchar(200),
	DiemToiDa int,
)

create table tb_DanhGiaRL
(
	ID int primary key identity(1,1),
	ID_SinhVien int references tb_SinhVien(ID),
	ID_GiangVien int references tb_GiangVien(ID),
	ID_QuanTri int references tb_QuanTri(ID),
	TongDiem int default 0,
	TongToiDa int,
	XepLoai nvarchar(20),
	GhiChu ntext,
	HocKi int,
	Nam int,
)

create table tb_MinhChungDGRL(
	ID int identity(1,1) primary key,
	ID_DanhGiaRL int references tb_DanhGiaRL(ID),
	HinhAnh nvarchar(1000),
)

create table tb_ChiTietDanhGiaRL(
	ID int identity(1,1) primary key,
	ID_DanhGiaRL int references tb_DanhGiaRL(ID),
	ID_ChiTietNoiDung int references tb_ChiTietNoiDungRenLuyen(ID),
	DiemSVDanhGia int,
	DiemGVDanhGia int,
	DiemQTDanhGia int,
)


create table tb_HocBong
(
	ID int not null primary key identity(1,1),
	Ten nvarchar(200) not null,
	Tien money,
);



create table tb_CapHocBong
(
	ID int identity(1,1) primary key,
	ID_HocBong int references tb_HocBong(ID),
	ID_SinhVien int not null references tb_SinhVien(ID),
	HocKi int,
	Nam int,
);




create table tb_DangKyNghiPhep
(
	ID int primary key identity(1,1),
	ID_GiangVien int  references tb_GiangVien(ID),
	NgayNghi date,
	LyDo ntext,
	TrangThai int,
	NgayGui datetime,
	ID_QuanTri int  references tb_QuanTri(ID),
	PhanHoi ntext,
	NgayDuyet datetime,
	
);

create table tb_DanhSachTotNghiep(
	ID int identity(1,1) primary key,
	MaDanhSach varchar(20),
	TenDanhSach nvarchar(200),
	Dot int,
	ThoiGian date,

)

create table tb_ChiTietTotNghiep(
	ID int identity(1,1) primary key,
	ID_SinhVien int references tb_SinhVien(ID),
	Diem float,
	XepLoai nvarchar(20),
)

create table tb_HocKi
(
	ID int identity(1, 1) primary key,
	ThoiGian int, --tháng
	HocKiToiDa int
);

Update tb_SinhVien set ID_LopHoc = 1 where ID = 1;
select * from tb_GiangVien
select * from tb_TaiKhoan where ID = (Select ID_TaiKhoan from tb_GiangVien where ID = 1)
select * from tb_LopHoc
select * from tb_HeDaoTao
select * from tb_SinhVien
SELECT * FROM tb_DanhGiaRL
select * from tb_ChiTietLichThi