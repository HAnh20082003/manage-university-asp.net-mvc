﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuanLyTruongDaiHoc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public partial class tb_QuanTri
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public tb_QuanTri()
        {
            this.tb_DangKyNghiPhep = new HashSet<tb_DangKyNghiPhep>();
            this.tb_DanhGiaRL = new HashSet<tb_DanhGiaRL>();
            this.tb_ThongBao = new HashSet<tb_ThongBao>();
            this.tb_YeuCauGiayChungNhan = new HashSet<tb_YeuCauGiayChungNhan>();
        }

        [DisplayName("ID")]
        public int ID { get; set; }

        [DisplayName("Mã Quản trị")]
        public string MaQuanTri { get; set; }

        [DisplayName("ID Thông tin Cá nhân")]
        public Nullable<int> ID_ThongTinCaNhan { get; set; }

        [DisplayName("ID Tài khoản")]
        public Nullable<int> ID_TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_DangKyNghiPhep> tb_DangKyNghiPhep { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_DanhGiaRL> tb_DanhGiaRL { get; set; }
        public virtual tb_TaiKhoan tb_TaiKhoan { get; set; }
        public virtual tb_ThongTinCaNhan tb_ThongTinCaNhan { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_ThongBao> tb_ThongBao { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<tb_YeuCauGiayChungNhan> tb_YeuCauGiayChungNhan { get; set; }
    }
}
