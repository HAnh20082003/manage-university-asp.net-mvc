use QuanLyDaiHoc

Set dateformat dmy

insert into tb_KhoaVien values('KTCN',N'Viện Kỹ thuật - Công nghệ',N'Viện Kỹ thuật - Công nghệ')
insert into tb_KhoaVien values('KHQL',N'Khoa Khoa Học Quản Lý',N'Khoa Khoa Học Quản Lý')
insert into tb_KhoaVien values('EF',N'Khoa Kinh Tế',N'Khoa Kinh Tế')
insert into tb_KhoaVien values('FLF',N'Khoa Ngoại Ngữ',N'Khoa Ngoại Ngữ')
insert into tb_KhoaVien values('SP',N'Khoa Sư Phạm',N'Khoa Sư Phạm')
insert into tb_KhoaVien values('PTUD',N'Viện Phát Triển Ứng Dụng',N'Viện Phát Triển Ứng Dụng')
insert into tb_KhoaVien values('CNVH',N'Khoa Công Nghiệp Văn Hóa',N'Khoa Công Nghiệp Văn Hóa')
insert into tb_KhoaVien values('KT',N'Khoa Kiến Trúc',N'Khoa Kiến Trúc')
insert into tb_KhoaVien values('YD',N'Khoa Y Dược',N'Khoa Y Dược')

insert into tb_HeDaoTao values ('CQ',N'Chính Quy')
insert into tb_HeDaoTao values ('TX',N'Thường Xuyên')

insert into tb_Nganh values('CNTT',N'Công nghệ Thông tin',1,4.5,N'Ngành Công nghệ Thông tin')
insert into tb_Nganh values('LO',N'Logistic',1,4.5,N'Ngành Kinh tế')

insert into tb_LoaiPhong values (N'Lý thuyết')
insert into tb_LoaiPhong values (N'Thực hành')
insert into tb_LoaiPhong values (N'Trực tuyến')

insert into tb_PhongHoc values('P106',N'Trong trường',1,60,1)

insert into tb_PhongHoc values('TH06',N'Trong trường',2,40,1)

INSERT INTO tb_PhongHoc
VALUES ('P107', N'Trong trường', 1, 30, 1);

INSERT INTO tb_PhongHoc
VALUES ('P108', N'Trong trường', 1, 50, 1);

INSERT INTO tb_PhongHoc
VALUES ('P109', N'Ngoài trường', 1, 40, 1);

INSERT INTO tb_PhongHoc
VALUES ('TH11', N'Ngoài trường', 2, 25, 1);

INSERT INTO tb_PhongHoc
VALUES ('P111', N'Trong trường', 1, 60, 0);

INSERT INTO tb_PhongHoc
VALUES ('TH12', N'Trong trường', 2, 35, 1);

INSERT INTO tb_PhongHoc
VALUES ('P113', N'Ngoài trường', 2, 45, 1);

INSERT INTO tb_PhongHoc
VALUES ('TH14', N'Trong trường', 2, 40, 1);

INSERT INTO tb_PhongHoc
VALUES ('P115', N'Ngoài trường', 2, 55, 1);

INSERT INTO tb_PhongHoc
VALUES ('TH16', N'Trong trường', 2, 30, 0);



INSERT INTO tb_MonHoc
VALUES ('LING022', N'Cơ sở lập trình', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('LING175', N'Nhập môn nhóm ngành Công Nghệ Thông Tin', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING266', N'Thực hành Cơ sở lập trình', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING295', N'Thực hành Nhập môn nhóm ngành Công Nghệ Thông Tin', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING387', N'Vật lý đại cương A1', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('KTCH001', N'Phương pháp nghiên cứu khoa học', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('KTCH002', N'Giáo dục thể chất (Lý thuyết)', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING105', N'Kỹ thuật lập trình', 1, 1, 2, 30)


INSERT INTO tb_MonHoc
VALUES ('LING256', N'Thiết kế Web', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING283', N'Thực hành Kỹ thuật lập trình', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING310', N'Thực hành thiết kế Web', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING320', N'Thực hành Vật lý đại cương A1', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING344', N'Toán cao cấp A1', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('KTCH003', N'Giáo dục quốc phòng an ninh', 1, 1, 5, 75)

INSERT INTO tb_MonHoc
VALUES ('KTCH004', N'Thực hành Giáo dục quốc phòng an ninh', 1, 0, 3, 90)


INSERT INTO tb_MonHoc
VALUES ('LING020', N'Cơ sở dữ liệu', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING265', N'Thực hành Cơ sở dữ liệu', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING345', N'Toán cao cấp A2', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('KTCH005', N'Tư duy biện luận ứng dụng', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING010', N'Cấu trúc dữ liệu và giải thuật', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('LING068', N'Hệ Quản trị cơ sở dữ liệu', 1, 1, 2, 45)

INSERT INTO tb_MonHoc
VALUES ('LING219', N'Quản trị doanh nghiệp', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING261', N'Thực hành Cấu trúc dữ liệu và giải thuật', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING276', N'Thực hành Hệ Quản trị cơ sở dữ liệu', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING396', N'Xác suất thống kê', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('KTCH006', N'Triết học Mác - Lênin', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('KTCH007', N'Giáo dục thể chất (Thực hành trong Trường)', 1, 0, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('LING053', N'Đổi mới, sáng tạo và khởi nghiệp', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('LING196', N'Phương pháp lập trình hướng đối tượng', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('LING304', N'Thực hành Phương pháp lập trình hướng đối tượng', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('KTCH008', N'Kinh tế chính trị Mác - Lênin', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('KTPM013', N'Thực hành Phân tích, thiết kế hướng đối tượng', 1, 1, 2, 60)

INSERT INTO tb_MonHoc
VALUES ('KTPM031', N'Phân tích, thiết kế hướng đối tượng', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING110', N'Lập trình trên Windows', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('LING185', N'Pháp luật', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING286', N'Thực hành lập trình trên Windows', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING349', N'Toán rời rạc', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('CNTT044', N'Đồ án cơ sở ngành', 1, 1, 2, 60)

INSERT INTO tb_MonHoc
VALUES ('KTCH009', N'Những vấn đề kinh tế - xã hội Đông Nam bộ', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('KTCH010', N'Chủ nghĩa xã hội khoa học', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('KTCH011', N'Tư tưởng Hồ Chí Minh', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING109', N'Lập trình Web', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING135', N'Lý thuyết đồ thị', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING137', N'Mạng máy tính', 1, 1, 2, 30)


INSERT INTO tb_MonHoc
VALUES ('LING285', N'Thực hành lập trình Web', 1, 0, 2, 60)

INSERT INTO tb_MonHoc
VALUES ('LING287', N'Thực hành Lý thuyết đồ thị', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING288', N'Thực hành Mạng máy tính', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('CNTT018', N'Thực hành Kỹ thuật lập trình trong phân tích dữ liệu', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('CNTT024', N'Kỹ thuật lập trình trong phân tích dữ liệu', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('CNTT043', N'Hệ điều hành', 1, 1, 2, 60)

INSERT INTO tb_MonHoc
VALUES ('KTCH012', N'Lịch sử Đảng Cộng sản Việt Nam', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING005', N'An toàn và bảo mật thông tin', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING031', N'Công nghệ phần mềm', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING093', N'Kiến trúc máy tính', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING210', N'Quản lý dự án công nghệ thông tin', 1, 1, 3, 45)

INSERT INTO tb_MonHoc
VALUES ('LING224', N'Quản trị Marketing', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING260', N'Thực hành An toàn và bảo mật thông tin', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING267', N'Thực hành Công nghệ phần mềm', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING314', N'Thực hành Trí tuệ nhân tạo', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING358', N'Trí tuệ nhân tạo', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING042', N'Điện toán đám mây', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING100', N'Quản trị hệ thống', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING189', N'Phát triển ứng dụng di động', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING270', N'Thực hành Điện toán đám mây', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING301', N'Thực hành Phát triển ứng dụng di động', 1, 0, 2, 60)

INSERT INTO tb_MonHoc
VALUES ('LING307', N'Thực hành Quản trị hệ thống', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('CNTT014', N'Đồ án chuyên ngành', 1, 1, 2, 60)

INSERT INTO tb_MonHoc
VALUES ('KTPM005', N'Chuyên đề An toàn ứng dụng', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('KTPM028', N'Thực hành Chuyên đề An toàn ứng dụng', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('LING191', N'Phát triển ứng dụng trên điện toán đám mây', 1, 1, 2, 30)

INSERT INTO tb_MonHoc
VALUES ('LING303', N'Thực hành Phát triển ứng dụng trên điện toán đám mây', 1, 0, 1, 30)

INSERT INTO tb_MonHoc
VALUES ('CNTT006', N'Thực tập doanh nghiệp', 1, 1, 5, 150)

INSERT INTO tb_MonHoc
VALUES ('CNTT003', N'Thực tập tốt nghiệp', 1, 1, 5, 150)

INSERT INTO tb_MonHoc
VALUES ('CNTT004', N'Báo cáo/Đồ án tốt nghiệp', 1, 1, 10, 300)

Insert into tb_chuongtrinhdaotao values (1, 1, 1, 1, 1)
Insert into tb_chuongtrinhdaotao values (1, 4, 0, 1, 1)
Insert into tb_chuongtrinhdaotao values (2, 2, 1, 1, 1)


INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Tạm hoãn nghĩa vụ quân sự')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Xin việc làm bán thời gian')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Xin học bằng tài trợ')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Nhận trợ cấp/Học bổng tại địa phương')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Xin đăng ký tạm trú')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ cấp Thẻ sinh viên liên kết BIDV')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Làm thư viện ngoài trường')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Phỏng vấn du học')
INSERT INTO tb_GiayChungNhan VALUES (N'Đăng ký cấp bằng điểm rèn luyện học kỳ')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Vay vốn ngân hàng')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Hưởng trợ cấp (con TB, BB, LS,...)')
INSERT INTO tb_GiayChungNhan VALUES (N'Bổ sung hồ sơ Chứng minh người phụ thuộc')




Insert into tb_ThongTinCaNhan values (N'Nguyễn Hoàng Anh', '20/08/2003', '0967657011', N'abcadsfs', null, null, '1234', N'Kinh', 1, null)
Insert into tb_TaiKhoan values ('a', '1', N'Quản Trị', null)
Insert into tb_QuanTri values ('QTA', 1, 1)


Insert into tb_HocKi values (4, 3)



Insert into tb_QuyTacTinhDiem values (50, 50);


Insert into tb_LichDanhGiaRL values ('20/10/2023', '30/12/2023');

Insert into tb_LopHoc values ('D21CNTT07', 1, 1, 1);
Insert into tb_YeuCauGiayChungNhan values (1, 1, N'Gửi đi', 0, null, null, GETDATE(), null)
Insert into tb_YeuCauGiayChungNhan values (20, 1, N'ABCD', 0, null, null, GETDATE(), null)

drop table tb_ChiTietDanhGiaRL
drop table tb_MinhChungDGRL
drop table tb_DanhGiaRL
drop table tb_ChiTietNoiDungRenLuyen
drop table tb_NoiDungRenLuyen

Select* from tb_sinhvien
Select * from tb_Monhoc
Select * from tb_DangKyMonHoc
select * from tb_ChiTietLichThi

select * from tb_Chuongtrinhdaotao


insert into tb_HocBong values(N'Học sinh giỏi',100000)