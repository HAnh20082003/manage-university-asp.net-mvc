using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyTruongDaiHoc.Models
{
    public class ConvertDate
    {
        public static string getStrDate(DateTime? date)
        {
            string day = date.Value.Day.ToString();
            if (date.Value.Day < 10)
            {
                day = "0" + day;
            }
            string month = date.Value.Month.ToString();
            if (date.Value.Month < 10)
            {
                month = "0" + month;
            }
            return date.Value.Year + "-" + month + "-" + day;
        }
    }
}