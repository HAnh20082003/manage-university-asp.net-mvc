using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyTruongDaiHoc.Models
{
    public class TmpNoiDungRL
    {
        public tb_NoiDungRenLuyen NoiDungRL = new tb_NoiDungRenLuyen();
        public List<tb_ChiTietNoiDungRenLuyen> chiTietNoiDungRL = new List<tb_ChiTietNoiDungRenLuyen>();
        public bool daLuuND = false;
        public List<bool> daLuuChiTiet = new List<bool>();
    }
}