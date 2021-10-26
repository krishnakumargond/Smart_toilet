using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using Smart_toilet.Common;

namespace Smart_toilet.Helper
{
    public class EnvHelper
    {
        public void InsertEnvData(EnvironmentInfo info, int TypeId,bool IsRain)
        {
            try
            {

                string Lat = "19.24301";
                string Lng = "73.150787";

                if (TypeId == 0)
                {
                    Lat = "25.6156947";
                    Lng = "85.1400547";
                }
                else
                {
                    KDMCDeviceInfo kdmcdevice = GetKDMCDeviceInfo("2").Where(i => i.DeviceId == info.DeviceId).FirstOrDefault();
                    if (kdmcdevice != null)
                    {
                        Lat = kdmcdevice.Lat;
                        Lng = kdmcdevice.Lng;
                    }
                }

                AirQualityParser AirQualityResult = GetAirQtyLiveData(Lat, Lng);
                EnvParser EnvResult = GetEnvLiveData(Lat, Lng);

                string Result = string.Empty;
                int THour = info.Timstamp.Hour;
                //List<EnvironmentSTInfo> _lst =CommonHelper.GetEnvStData();
                //EnvironmentSTInfo sinfo = _lst.Where(i => THour>= i.SHour  &&  THour<= i.EHour).FirstOrDefault();
                //if(sinfo!=null)
                //{
                //    info.AQI = sinfo.AQI;
                //    info.PM2_5 = sinfo.PM2_5;
                //    info.PM10 = sinfo.PM10;
                //    info.NO2 = sinfo.NO2;
                //    info.CO = sinfo.CO;
                //    info.O3 = sinfo.O3;
                //    info.SO2 = sinfo.SO2;
                //}
                if (AirQualityResult != null)
                {

                    // dynamic AResult = JsonConvert.DeserializeObject(AirQualityResult);
                    info.AQI = AirQualityResult.data[0].aqi;
                    info.PM2_5 = AirQualityResult.data[0].pm25;
                    info.PM10 = AirQualityResult.data[0].pm10;
                    info.NO2 = AirQualityResult.data[0].no2;
                    info.CO = AirQualityResult.data[0].co;
                    info.O3 = AirQualityResult.data[0].o3;
                    info.SO2 = AirQualityResult.data[0].so2;
                }
                if (EnvResult != null)
                {
                    info.Humidity = EnvResult.current.humidity;
                    info.UV = EnvResult.current.uvi.ToString();
                    info.Temperature = (EnvResult.current.temp - 273.15).ToString();
                    info.RainLvl =IsRain?info.RainLvl: EnvResult.daily[0].rain.ToString();
                    info.Alrt_Desc = EnvResult.current.weather[0].description.ToString();
                    info.WindSpeed = EnvResult.current.wind_speed;
                    info.Battery_Level = EnvResult.current.wind_deg;
                    if (TypeId == 0)
                        info.VOC = EnvResult.current.pressure.ToString();

                }
                //info.RainLvl = "0";
                LoginResponseInfo result = PushEnvData(info, TypeId);
            }
            catch (Exception ex)
            {
               // CommonHelper.WriteToFile("UTPL EX Service2 Error on: {0} " + "INSERT FUNC" + ex.Message + ex.StackTrace);
            }
        }
        public EnvParser GetEnvLiveData(string Lat, string Lng)
        {
            EnvParser EnvResult = new EnvParser();
            try
            {

                string ForcastapiUrl = "https://api.openweathermap.org/data/2.5/onecall?lat=" + Lat + "&lon=" + Lng + "&exclude=minutely,hourly&appid=6bcc8a588d46d6949e7ee4de914119a0";
                HttpClientHelper<EnvParser> apiobj3 = new HttpClientHelper<EnvParser>();
                EnvResult = apiobj3.GetSingleItemRequest(ForcastapiUrl);
            }
            catch (Exception ex)
            {
                EnvResult = null;
            }
            return EnvResult;
        }
        public AirQualityParser GetAirQtyLiveData(string Lat, string Lng)
        {
            AirQualityParser Result = new AirQualityParser();
            try
            {
                // string EnvApiUrl = "https://api.waqi.info/feed/geo:" + Lat + ";" + Lng + "/?token=f4dcaeb9575126f5c4e732f225699cfd3710ffa7";
                string EnvApiUrl = "http://api.weatherbit.io/v2.0/current/airquality?lat=" + Lat + "&lon=" + Lng + "&key=31c8a1ebe43249e395e8e22c0edf5a02";
                HttpClientHelper<AirQualityParser> apiobj2 = new HttpClientHelper<AirQualityParser>();
                Result = apiobj2.GetSingleItemRequest(EnvApiUrl);
            }
            catch (Exception ex)
            {
                return null;
            }
            return Result;
        }

        internal List<KDMCDeviceInfo> GetKDMCDeviceInfo(string typeId)
        {
            List<KDMCDeviceInfo> _lst = new List<KDMCDeviceInfo>();

                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AjeeviIotJbConstr"].ConnectionString);
                using (SqlCommand cmd = new SqlCommand("spGetAllKdmcDevice", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@DeviceTypeId", typeId);


                    con.Open();

                    SqlDataReader dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {

                        _lst.Add(new KDMCDeviceInfo
                        {

                            DeviceId = Convert.ToString(dr["DeviceId"]),
                            DeviceType = Convert.ToString(dr["DeviceType"]),
                            Lat = Convert.ToString(dr["Lat"]),
                            Lng = Convert.ToString(dr["Lng"]),
                            Location = Convert.ToString(dr["Location"]),
                            Status = Convert.ToString(dr["Status"]),
                            DateOfInstallation = Convert.ToString(dr["DateOfInstallation"]),

                        });

                    }
                    con.Close();

                }

            return _lst;
        }
        internal LoginResponseInfo PushEnvData(EnvironmentInfo info, int TypeId)
        {
            DateTime TDate = TypeId == 0 ? CommonHelper.IndianStandard(DateTime.UtcNow.AddSeconds(-10)) : CommonHelper.IndianStandard(DateTime.UtcNow);
            LoginResponseInfo Result = new LoginResponseInfo();
            try
            {
                using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["AjeeviIotJbConstr"].ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("spPushEnvData"))
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@DeviceId", info.DeviceId);
                        cmd.Parameters.AddWithValue("@PM10", info.PM10);
                        cmd.Parameters.AddWithValue("@PM2_5", info.PM2_5);
                        cmd.Parameters.AddWithValue("@VOC", !String.IsNullOrEmpty(info.VOC)? info.VOC:"");
                        cmd.Parameters.AddWithValue("@CO2", info.CO2);
                        cmd.Parameters.AddWithValue("@Humidity", info.Humidity);
                        cmd.Parameters.AddWithValue("@Temperature", info.Temperature);
                        cmd.Parameters.AddWithValue("@AQI", info.AQI);
                        cmd.Parameters.AddWithValue("@Timstamp", info.Timstamp);
                        cmd.Parameters.AddWithValue("@SO2", info.SO2);
                        cmd.Parameters.AddWithValue("@NO2", info.NO2);
                        cmd.Parameters.AddWithValue("@O3", info.O3);
                        cmd.Parameters.AddWithValue("@CO", info.CO);
                        cmd.Parameters.AddWithValue("@Noise", info.Noise);
                        cmd.Parameters.AddWithValue("@UV", info.UV);
                        cmd.Parameters.AddWithValue("@SyncOn", TDate);
                        cmd.Parameters.AddWithValue("@Battery_Level", info.Battery_Level);
                        cmd.Parameters.AddWithValue("@Rain", info.RainLvl);
                        cmd.Parameters.AddWithValue("@Alert_Desc", info.Alrt_Desc);
                        cmd.Parameters.AddWithValue("@WindSpeed", info.WindSpeed);
                        cmd.Parameters.AddWithValue("@Light", info.Light);

                        using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);

                            Result = dt.AsEnumerable().Select(sdr => new LoginResponseInfo()
                            {

                                Result = sdr["Result"].ToString(),
                                Msg = sdr["Msg"].ToString()

                            }).FirstOrDefault();
                        }
                    }

                }
                if (TypeId == 1)
                    HostingEnvironment.QueueBackgroundWorkItem(cancellationToken => KDMCApiHelper.GetAuthenticate(info.DeviceId, info, TDate));
            }
            catch (Exception ex)
            {

            }
            return Result;
        }
    }
}