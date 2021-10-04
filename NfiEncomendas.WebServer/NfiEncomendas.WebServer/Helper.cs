using System;
using System.Globalization;
using System.Security.Cryptography;

namespace NfiEncomendas.WebServer
{
    public class Helper
    {
        public static string GetHash(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public int GetWeekOfYear(DateTime date1)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;

            Calendar cal = dfi.Calendar;

            //Console.WriteLine("{0:d}: Week {1} ({2})", date1,
            //                  cal.GetWeekOfYear(date1, dfi.CalendarWeekRule,
            //                                    dfi.FirstDayOfWeek),
            //                  cal.ToString().Substring(cal.ToString().LastIndexOf(".") + 1));

            return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

        }
    }
}