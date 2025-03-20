using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyTruongDaiHoc.Models
{
    public class Tuan
    {
        public static int countDay = 7;
        public static int soTiet = 14;
        public DateTime startDate;
        public DateTime[] day = new DateTime[countDay];
        public int[] tiet = new int[soTiet];
        public List<tb_KhoaHoc> khoahoc;

        public Tuan(List<tb_KhoaHoc> kh, DateTime ngay)
        {
            while (ngay.DayOfWeek != DayOfWeek.Monday)
            {
                ngay = prevDate(ngay);
            }
            day[0] = ngay;
            for (int i = 1; i < countDay; i++)
            {
                day[i] = nextDate(day[i - 1]);
            }
            khoahoc = new List<tb_KhoaHoc>();
            for (int i = 0; i < kh.Count; i++)
            {
                for (int j = 0; j < countDay; j++)
                {
                    var chitiet = kh[i].tb_ChiTietKhoaHoc;
                    foreach (var item in chitiet)
                    {
                        if (item.Ngay == day[j])
                        {
                            khoahoc.Add(kh[i]);
                        }
                    }
                    
                }
            }

        }

        public DateTime prevDate(DateTime currentDate)
        {
            int day = currentDate.Day, month = currentDate.Month, year = currentDate.Year;
            if (currentDate.Day > 1)
            {
                day--;
            }
            else
            {
                if (month == 1)
                {
                    month = 12;
                    day = 31;
                    year--;
                }
                else
                {
                    month--;
                    if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
                    {
                        day = 31;
                    }
                    else if (month == 4 || month == 6 || month == 9 || month == 11)
                    {
                        day = 30;
                    }
                    else
                    {
                        if ((year % 100 == 0 && year % 400 == 0) || (year % 4 == 0))
                        {
                            day = 29;
                        }
                        else
                        {
                            day = 28;
                        }
                    }
                }
            }
            return new DateTime(year, month, day);
        }
        public DateTime nextDate(DateTime currentDate)
        {
            int day = currentDate.Day, month = currentDate.Month, year = currentDate.Year;
            if (month == 2)
            {
                if ((year % 100 == 0 && year % 400 == 0) || (year % 4 == 0))
                {
                    if (day == 29)
                    {
                        day = 1;
                        if (month < 12)
                        {
                            month++;
                        }
                        else
                        {
                            day = 1;
                            month = 1;
                            year++;
                        }
                    }
                    else
                    {
                        day++;
                    }
                }
            }
            else if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
            {
                if (day == 31)
                {
                    day = 1;
                    if (month < 12)
                    {
                        month++;
                    }
                    else
                    {
                        day = 1;
                        month = 1;
                        year++;
                    }
                }
                else
                {
                    day++;
                }
            }
            else
            {
                if (day == 30)
                {
                    day = 1;
                    if (month < 12)
                    {
                        month++;
                    }
                    else
                    {
                        day = 1;
                        month = 1;
                        year++;
                    }
                }
                else
                {
                    day++;
                }
            }
            return new DateTime(year, month, day);
        }


    }
}