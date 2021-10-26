using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_toilet.Common
{
    public class KDMCDeviceInfo
    {
        public string DeviceId { get; set; }
        public string DeviceType { get; set; }
        public string Location { get; set; }
        public string Lat { get; set; }
        public string Lng { get; set; }
        public string Status { get; set; }
        public string DateOfInstallation { get; set; }
    }
    public class LoginResponseInfo
    {
        public string Msg { get; set; }
        public string Result { get; set; }
    }
    public class EnvironmentInfo
    {
        public string DeviceId { get; set; }
        public string PM10 { get; set; }
        public string PM2_5 { get; set; }
        public string VOC { get; set; }
        public string CO2 { get; set; }
        public string Humidity { get; set; }
        public string Temperature { get; set; }
        public string AQI { get; set; }
        public DateTime Timstamp { get; set; }
        public string SO2 { get; set; }
        public string NO2 { get; set; }
        public string O3 { get; set; }
        public string CO { get; set; }
        public string Noise { get; set; }
        public string Battery_Level { get; set; }
        public string UV { get; set; }
        public string RainLvl { get; set; }
        public string Alrt_Desc { get; set; }
        public string WindSpeed { get; set; }
        public string Light { get; set; }
    }
    public class EnvParser
    {
        public EnvInfoCurrent current { get; set; }
        public List<EnvInfoDaily> daily { get; set; }
    }
    public class EnvInfoDaily
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public int moonrise { get; set; }
        public int moonset { get; set; }
        public string moon_phase { get; set; }
        public List<EnvInfoWeather> weather { get; set; }
        public double rain { get; set; }
    }
    public class EnvInfoWeather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
    }
    public class EnvInfoCurrent
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public string humidity { get; set; }
        public double dew_point { get; set; }
        public double uvi { get; set; }
        public string clouds { get; set; }
        public string visibility { get; set; }
        public string wind_speed { get; set; }
        public string wind_deg { get; set; }
        public string wind_gust { get; set; }
        public List<EnvInfoWeather> weather { get; set; }
    }
    public class AirQualityParser
    {
        public List<AirQuality1> data { get; set; }
    }
    public class AirQuality1
    {
        public string mold_level { get; set; }
        public string aqi { get; set; }
        public string pm10 { get; set; }
        public string co { get; set; }
        public string o3 { get; set; }
        public string predominant_pollen_type { get; set; }
        public string so2 { get; set; }
        public string pollen_level_tree { get; set; }
        public string pollen_level_weed { get; set; }
        public string no2 { get; set; }
        public string pm25 { get; set; }
        public string pollen_level_grass { get; set; }
    }
}