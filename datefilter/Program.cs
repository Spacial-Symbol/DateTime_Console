using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace datefilter
{
    class Program
    {
         HttpWebRequest _httpRequest;
        static void Main(string[] args)
        {
           // SendRequest();
            //string jsonData = JsonHelp.JsonSerialize<T>(t);
            // if (string.IsNullOrEmpty(jsonData))
            //    return;
            //char[] charsToTrim = { '*', ' ', '\'' };
            //string banner = "*** Much Ado About Nothing ***";
            //string result = banner.Trim(charsToTrim);
            //Console.WriteLine("Trimmmed\n   {0}\nto\n   '{1}'", banner, result);
            //Console.ReadLine();

            //MakeDate();
            MakeMonth("2013-12-26");
        }


        public static void MakeMonth(string date)
        {
            //"2013-12-26"
            DateTime passedDate = DateTime.Parse(date);
            int month = DateTime.Parse(date).Month;
            int year = DateTime.Parse(date).Year; 
            var  dtmonth =  new DataTable();
            var  dtMonth = new DataTable();
            int newmonth = month;
            for (int i = 0; i < 6; i++)
            {
                
                if (month > 12)
                {
                    month = 1;
                    year = year + 1;

                    
                    string monthFirstDate = FirstDayOfMonthFromDateTime(year, month).ToString("yyyy-MM-dd");
                    string monthName = DateTime.Parse(monthFirstDate).ToString("MMM");
                    string monthlastdate = LastDayOfMonthFromDateTime(year, month).ToString("yyyy-MM-dd");
                    dtMonth = GetMonthDataTable(dtMonth, monthFirstDate + "--" + monthlastdate ,monthName);
                    //newmonth = 1;
                }
                else
                {
                    string monthFirstDate = FirstDayOfMonthFromDateTime(year, month).ToString("yyyy-MM-dd");
                    string monthName = DateTime.Parse(monthFirstDate).ToString("MMM");
                    string monthlastdate = LastDayOfMonthFromDateTime(year, month).ToString("yyyy-MM-dd");
                    dtMonth = GetMonthDataTable(dtMonth, monthFirstDate + "--" + monthlastdate, monthName);
                }
                month = month + 1;
            }
        }

        #region Send request
        public void SendRequest()
        {
            //string jsonData = "";
            //byte[] data = UnicodeEncoding.UTF8.GetBytes(jsonData);

            //_httpRequest = HttpWebRequest.CreateHttp(string.Format("", "", ""));
            //_httpRequest.Method = method.ToString();
            //_httpRequest.ContentType = "application/json";
            //_httpRequest.ContentLength = data.Length;

            //try
            //{
            //    using (dataStream = _httpRequest.GetRequestStream())
            //    {
            //        dataStream.Write(data, 0, data.Length);
            //    }
            //}
            //catch (WebException we)
            //{
            //    StrMessage = we.Message;
            //}
        }
        #endregion

        #region Make Month
        public static void MakeDate()
        {
            //DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            //DateTime date1 = DateTime.Parse("2013-12-26");
            //Calendar cal = dfi.Calendar;

            //Console.WriteLine("{0:d}: Week {1} ({2})", date1,
            //                  cal.GetWeekOfYear(date1, dfi.CalendarWeekRule,
            //                                    dfi.FirstDayOfWeek),
            //                  cal.ToString().Substring(cal.ToString().LastIndexOf(".", System.StringComparison.Ordinal) + 1));
            //Console.ReadLine();
            CultureInfo myCI = new CultureInfo("en-US");
            //int yearf = int.Parse("2013");
            //int weekf = int.Parse("40");
            int year = DateTime.Parse("2013-12-26").Year;
            var week = DateTime.Parse("2013-12-26").DayOfWeek;
            var month = DateTime.Parse("2013-12-26").Month;
            DateTime addfirstmonth = DateTime.Parse("2013-12-26").AddMonths(1);
            var addmonthnumber = addfirstmonth.DayOfWeek;


            var weekNo = myCI.Calendar.GetWeekOfYear(DateTime.Parse("2013-12-26"), myCI.DateTimeFormat.CalendarWeekRule, myCI.DateTimeFormat.FirstDayOfWeek);
            DataTable dtnowone = new DataTable();
            for (int i = 0; i <= 8; i++)
            {
                int nextweeknouber = weekNo + i;
                if (nextweeknouber == 53)
                {
                    year = year + 1;
                    weekNo = 1;
                    DateTime FromDate = FirstDateOfWeek(year, weekNo);                   
                    string fromdatestring = FromDate.AddDays(1).ToString("yyyy-MM-dd");
                    string FDate = FromDate.Date.ToString("yyyy-MM-dd");
                    DateTime ToDate = FromDate.AddDays(7);
                    string TDate = ToDate.Date.ToString("yyyy-MM-dd");
                    dtnowone = SaveDataSet(dtnowone, fromdatestring + "--" + TDate);
                    weekNo = weekNo - 1;
                }
                else
                {

                    DateTime FromDate = FirstDateOfWeek(year, nextweeknouber);
                    string fromdatestring = FromDate.AddDays(1).ToString("yyyy-MM-dd");
                    string FDate = FromDate.Date.ToString("yyyy-MM-dd");
                    DateTime ToDate = FromDate.AddDays(7);
                    string TDate = ToDate.Date.ToString("yyyy-MM-dd");
                    dtnowone = SaveDataSet(dtnowone, fromdatestring + "--" + TDate );
                }
            }

           
        }
        #endregion

        #region First Week
        public static DateTime FirstDateOfWeek(int year, int weekOfYear)
        {
            CultureInfo myCI = new CultureInfo("en-US");
            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = (int)myCI.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;

            DateTime firstMonday = jan1.AddDays(daysOffset);

            int firstWeek = myCI.Calendar.GetWeekOfYear(jan1, myCI.DateTimeFormat.CalendarWeekRule, myCI.DateTimeFormat.FirstDayOfWeek);

            if (firstWeek <= 1)
            {
                weekOfYear -= 1;
            }

            return firstMonday.AddDays(weekOfYear * 7);
        }
        #endregion

        #region Data Set
        public static DataTable SaveDataSet(DataTable dtdates, string weeks)
        {
            if (!dtdates.Columns.Contains("Dates"))
                dtdates.Columns.Add("Dates");           

            DataRow drow = dtdates.NewRow();
            drow["Dates"] = weeks;           
            dtdates.Rows.Add(drow);
            return dtdates;
        }
        #endregion

        public static DateTime FirstDayOfMonthFromDateTime(int year ,int month)
        {
            return new DateTime(year, month, 1);
        }

        public static DateTime LastDayOfMonthFromDateTime(int year , int month)
        {
            var  firstDayOfTheMonth = new DateTime(year, month, 1);
            return firstDayOfTheMonth.AddMonths(1).AddDays(-1);
        }
    
        public static DataTable GetMonthDataTable(DataTable dtmonth ,string month, string monthname)
        {           
            if (!dtmonth.Columns.Contains("MonthDate"))
                dtmonth.Columns.Add("MonthDate");

            if (!dtmonth.Columns.Contains("MonthName"))
                dtmonth.Columns.Add("MonthName");

            DataRow drow = dtmonth.NewRow();         
            drow["MonthDate"] = month;
            drow["MonthName"] = monthname;
            dtmonth.Rows.Add(drow);
            return dtmonth;
        }
    }
}
