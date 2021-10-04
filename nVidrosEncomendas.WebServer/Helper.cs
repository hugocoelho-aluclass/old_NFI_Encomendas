using System;
using System.Globalization;
using System.Security.Cryptography;

namespace NVidrosEncomendas.WebServer
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

            return cal.GetWeekOfYear(date1, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);

        }
    }
}