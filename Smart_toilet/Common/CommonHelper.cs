using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Hosting;

namespace Smart_toilet.Common
{
    public static class CommonHelper
    {
        private static object lockObject { get; set; }

        public static object LockObject
        {
            get
            {
                if (lockObject == null)
                    lockObject = new object();
                return lockObject;
            }
        }
        public static DateTime IndianStandard(DateTime currentDate)
        {
            TimeZoneInfo mountain = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            DateTime utc = currentDate;
            return TimeZoneInfo.ConvertTimeFromUtc(utc, mountain);
            //return DateTime.Now;
        }
        public static void WriteToFile(string text)
        {
            lock (LockObject)
            {
                string FileName = "WinRT" + DateTime.Now.ToString("dd-MM-yyyy") + ".text";

                //string path = HttpContext.Current.Server.MapPath("~/Files/" + FileName);
                string path = HostingEnvironment.MapPath("/Files/" + FileName);
                if (!File.Exists(path))
                    File.Create(path).Dispose();
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(string.Format(text, DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt")));
                    writer.Close();
                }
            }
        }
        public static void WriteToJsonFile(string FName, string text)
        {
            lock (LockObject)
            {
                string FileName = FName + DateTime.Now.ToString("dd-MM-yyyy") + ".json";

                //string path = HttpContext.Current.Server.MapPath("~/Files/" + FileName);
                string path = HostingEnvironment.MapPath("/Files/" + FileName);
                if (!File.Exists(path))
                    File.Create(path).Dispose();
                using (TextWriter writer = new StreamWriter(path, true))
                {
                    writer.WriteLine(text);
                    writer.Close();
                }
            }
        }
        public static string Get6Digits()
        {
            Random generator = new Random();
            int r = generator.Next(1, 1000000);
            string result = r.ToString().PadLeft(6, '0');

            return result;
        }
        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }
        public static string UnixTimeToDateTime(double unixtime)
        {
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixtime).ToLocalTime();
            return dtDateTime.ToString("dd-MM-yyyy h:mm tt");
        }

        public static string SAHARANPUR_GPS_URL = "http://spurwapi.ecosmartdc.com/api/Transport/PushGpsData";



        public static List<string> GetAllSolapurGpsIme()
        {
            List<string> _lst = new List<string>();
            _lst.Add("868003032952967");
            _lst.Add("868003032953288");
            _lst.Add("868003032953486");
            _lst.Add("868003032968096");
            _lst.Add("868003032953403");
            _lst.Add("868003032951373");
            _lst.Add("868003032950417");
            _lst.Add("868003032950607");

            return _lst;
        }
        public static List<string> GetAllKhargoneGpsIme()
        {
            List<string> _lst = new List<string>();
            _lst.Add("868003032967858");
            _lst.Add("868003032952363");
            _lst.Add("868003032949039");
            _lst.Add("868003032966496");
            _lst.Add("868003032950037");
            _lst.Add("868003032964517");


            return _lst;
        }

        public static DataTable ListToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names  
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
    }
}