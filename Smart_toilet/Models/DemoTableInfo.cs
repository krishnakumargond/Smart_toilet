using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_toilet.Models
{
    public class DemoTableInfo
    {
        public string ImeiNo { get; set; }
        public string V1 { get; set; }
        public string V2 { get; set; }
    }
    public class DevicerainInfo
    {
        public int RainSensorId { get; set; }
        public string Imei { get; set; }
        public string raifall { get; set; }
        public string light { get; set; }
        public string Tdate { get; set; }
    }
}