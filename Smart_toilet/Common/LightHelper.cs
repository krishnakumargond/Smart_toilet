using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using Excel;


namespace Smart_toilet.Common
{
    public static class LightHelper
    {
        // Instantiate random number generator.  
        private static readonly Random _random = new Random();
        // Generates a random number within a range.      
        public static int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }
        public static DataTable GetLightCustomDataFromExcel()
        {
            string DestinationFile = System.Web.HttpContext.Current.Server.MapPath("~/Files/") + "Envlight.xlsx";
            byte[] FileContent = File.ReadAllBytes(DestinationFile);
            string Extension = ".xlsx";
            byte[] buffer = new byte[FileContent.Length];

            IExcelDataReader reader = null;
            using (FileStream stream = File.Open(DestinationFile, FileMode.Open, FileAccess.Read))
            {

                if (Extension == ".xls")
                {
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                }
                else if (Extension == ".xlsx")
                {
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                }
                reader.IsFirstRowAsColumnNames = true;


            }
            DataSet result = reader.AsDataSet();
            reader.Close();

            DataTable dt = result.Tables[0];


            // CacheForFlowMeterCustomInfo = dt;

            return dt;
        }

        public static string GetLightValue(DateTime TDate)
        {
            string Result = "0";
            DataTable dt = GetLightCustomDataFromExcel();
            TimeSpan cTime = new TimeSpan(TDate.Hour, TDate.Minute, 0);
            List<LightIData> _lst = new List<LightIData>();
            try
            {
                int Count = dt.Rows.Count - 1;
                string[] columnNames = dt.Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();
                foreach (DataRow row in dt.Rows)
                {
                    foreach (string item in columnNames)
                    {
                        if (!string.IsNullOrEmpty(row[item].ToString()))
                        {
                            string first = item.Split('-')[0];
                            string last = item.Split('-')[1];
                            _lst.Add(new LightIData
                            {
                                StartTime = new TimeSpan(Convert.ToInt32(first.Split(':')[0]), Convert.ToInt32(first.Split(':')[1]), 0),
                                EndTime = new TimeSpan(Convert.ToInt32(last.Split(':')[0]), Convert.ToInt32(last.Split(':')[1]), 0),
                                value = row[item].ToString()
                            });
                        }
                    }
                    //object cell13Data = row["21:01-5:00"];
                    //Console.WriteLine(cell13Data);


                }
                LightIData info = _lst.Where(i => cTime >= i.StartTime && cTime <= i.EndTime).FirstOrDefault();
                if (info != null)
                {
                    int RandomVal = 0;
                    int val1 = Convert.ToInt32(info.value.Split('-')[0]);
                    int val2 = Convert.ToInt32(info.value.Split('-')[1]);
                    if (val1 > val2)
                        RandomVal = RandomNumber(val2, val1);
                    else
                        RandomVal = RandomNumber(val1, val2);
                    Result = RandomVal.ToString();
                }

            }
            catch (Exception ex)
            {
                return "0";
            }
            return Result;
        }
    }
    public class LightIData
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public string value { get; set; }
    }
}