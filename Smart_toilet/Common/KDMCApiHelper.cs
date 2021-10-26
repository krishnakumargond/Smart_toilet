using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Smart_toilet.Common
{
    public class KDMCApiHelper
    {
        private const string userService_Post = "https://piothub.smartkdcl.com:6443/HTTPProtAdaptorService/oauth/token";
        private const string datapost_Post = "https://piothub.smartkdcl.com:6443/HTTPProtAdaptorService/rest/telemetry/pushData";

        private const string WUserNane = "trinity-client";
        private const string WPWd = "trinity-secret";

        public static void GetAuthenticate(string DeviceId, EnvironmentInfo info, DateTime TDate)
        {
            string res = "";
            try
            {
                // Create the HttpContent for the form to be posted.
                var requestContent = new FormUrlEncodedContent(new[] {
    new KeyValuePair<string, string>("username", "trinity"),
                    new KeyValuePair<string,string>("password","trinity@123"),
                    new KeyValuePair<string,string>("grant_type","password")
});

                var byteArray = Encoding.ASCII.GetBytes(WUserNane + ":" + WPWd);
                using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    // Get the response.
                    HttpResponseMessage response = client.PostAsync(
                        userService_Post,
                        requestContent).Result;

                    using (HttpContent content1 = response.Content)
                    {

                        Task<string> result = content1.ReadAsStringAsync();
                        res = result.Result;
                    }
                    //var k = response.Content.ReadAsStreamAsync().Result;
                }


                dynamic data = JObject.Parse(res);

                string Token = data.access_token;

                SendDataOnserver(info, Token, DeviceId, TDate);

            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(st.FrameCount - 1);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();


                //CommonHelper.WriteToFile("UTPL EX Service1 Error on: {0} " + ex.Message + ex.StackTrace);
                CommonHelper.WriteToFile("UTPL EX Service1 Error on" + ex.Message + ex.StackTrace + "st-" + st + " frame-" + frame + " line-" + line);
            }
        }

        public static void SendDataOnserver(EnvironmentInfo info, string Token, string DeviceId, DateTime TDate)
        {
            Uri address = new Uri(datapost_Post);
            CookieContainer cookieJar = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = cookieJar
            };

            handler.UseCookies = true;
            handler.UseDefaultCredentials = false;
            HttpClient client = new HttpClient(handler as HttpMessageHandler)
            {
                BaseAddress = address,

            };
            try
            {

                var obj = new
                {
                    ENVIRONMENT = new
                    {

                        HUM = info.Humidity,
                        UV = info.UV,
                        PS = "MP",
                        O3 = info.O3,
                        NS = "NM",
                        CO2 = info.CO2,
                        LIGHT = info.Light,//"9974.96",
                        TIME = TDate.ToString("HHmmss"),
                        NOISE = info.Noise,
                        CO = info.CO,
                        MAC = info.DeviceId,
                        NO2 = info.NO2,
                        DATE = TDate.ToString("ddMMyy"),
                        TEMP = info.Temperature,
                        SO2 = info.SO2,
                        PM2_5 = info.PM2_5,
                        PM10 = info.PM10,
                        WLVL = "",
                        RAIN = info.RainLvl,
                        DATE_TIME = TDate.ToString("yyyy-MM-dd HH:mm:ss"),
                        WS = info.WindSpeed,
                        WD = "NE",
                        WP = "",
                        PM1 = "",
                        AQI = info.AQI,
                        ALRT = info.Alrt_Desc


                    }

                };
                byte[] json = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.Indented));
                string output = JsonConvert.SerializeObject(obj);

                client.DefaultRequestHeaders.Add("Authorization", "bearer " + Token);
                client.DefaultRequestHeaders.Add("deviceId", DeviceId);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var response = client.PostAsync(client.BaseAddress,
                     new StringContent(output.ToString(), Encoding.UTF8, "application/json")).Result;


                string res = "";
                using (HttpContent content1 = response.Content)
                {

                    Task<string> result = content1.ReadAsStringAsync();
                    res = result.Result;
                }
                dynamic data = JObject.Parse(res);

                string Presult = data.message;


                client.Dispose();
                string jsonData = JsonConvert.SerializeObject(obj, Formatting.None);
                CommonHelper.WriteToJsonFile("KDMCENVLOG", jsonData + " Api Response-" + Presult);
            }
            catch (Exception ex)
            {
                client.Dispose();
                CommonHelper.WriteToFile("UTPL EX Service2 Error on: {0} " + ex.Message + ex.StackTrace);
            }
        }

        public static void GetFloodAuthenticate(string DeviceId, string Level, DateTime TDate)
        {
            string res = "";
            try
            {
                // Create the HttpContent for the form to be posted.
                var requestContent = new FormUrlEncodedContent(new[] {
    new KeyValuePair<string, string>("username", "trinity"),
                    new KeyValuePair<string,string>("password","trinity@123"),
                    new KeyValuePair<string,string>("grant_type","password")
});

                var byteArray = Encoding.ASCII.GetBytes(WUserNane + ":" + WPWd);
                using (HttpClient client = new HttpClient())
                {

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
                    // Get the response.
                    HttpResponseMessage response = client.PostAsync(
                        userService_Post,
                        requestContent).Result;

                    using (HttpContent content1 = response.Content)
                    {

                        Task<string> result = content1.ReadAsStringAsync();
                        res = result.Result;
                    }
                    //var k = response.Content.ReadAsStreamAsync().Result;
                }


                dynamic data = JObject.Parse(res);

                string Token = data.access_token;

                SendFloodDataOnserver(Level, Token, DeviceId, TDate);

            }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(st.FrameCount - 1);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();


                //CommonHelper.WriteToFile("UTPL EX Service1 Error on: {0} " + ex.Message + ex.StackTrace);
                CommonHelper.WriteToFile("UTPL EX Service1 Error on" + ex.Message + ex.StackTrace + "st-" + st + " frame-" + frame + " line-" + line);
            }
        }

        public static void SendFloodDataOnserver(string Level, string Token, string DeviceId, DateTime TDate)
        {
            Uri address = new Uri(datapost_Post);
            CookieContainer cookieJar = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler()
            {
                CookieContainer = cookieJar
            };

            handler.UseCookies = true;
            handler.UseDefaultCredentials = false;
            HttpClient client = new HttpClient(handler as HttpMessageHandler)
            {
                BaseAddress = address,

            };
            try
            {

                var obj = new
                {
                    DATA = new
                    {
                        MAC = DeviceId,
                        PS = "MP",
                        DATE = TDate.ToString("ddMMyy"),
                        TIME = TDate.ToString("HHmmss"),
                        STATUS = "ONLINE",
                        Lat = "0.00",
                        Long = "0.00",
                        Level = Level,
                        Default_Value = "9.999"

                    }

                };
                byte[] json = System.Text.Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj, Formatting.Indented));
                string output = JsonConvert.SerializeObject(obj);

                client.DefaultRequestHeaders.Add("Authorization", "bearer " + Token);
                client.DefaultRequestHeaders.Add("deviceId", DeviceId);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                var response = client.PostAsync(client.BaseAddress,
                     new StringContent(output.ToString(), Encoding.UTF8, "application/json")).Result;


                string res = "";
                using (HttpContent content1 = response.Content)
                {

                    Task<string> result = content1.ReadAsStringAsync();
                    res = result.Result;
                }
                dynamic data = JObject.Parse(res);

                string Presult = data.message;


                client.Dispose();
                string jsonData = JsonConvert.SerializeObject(obj, Formatting.None);
                CommonHelper.WriteToJsonFile("KDMCFLOODLOG", jsonData + " Api Response-" + Presult);
            }
            catch (Exception ex)
            {
                client.Dispose();
                CommonHelper.WriteToFile("UTPL EX Service2 Error on: {0} " + ex.Message + ex.StackTrace);
            }
        }
    }
}