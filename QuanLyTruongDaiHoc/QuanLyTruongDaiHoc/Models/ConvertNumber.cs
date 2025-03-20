using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuanLyTruongDaiHoc.Models
{
    public class ConvertNumber
    {
        public static string getStrDecimal(int? number, int count)
        {
            if (number == null)
            {
                return null;
            }
            string result = number.ToString();
            int lengthNumber = result.Length;
            if (count <= lengthNumber)
            {
                return result;
            }
            int du = count - lengthNumber;
            for (int i  = 0; i < du; i++)
            {
                result = "0" + result;
            }
            return result;
        }
    }
}