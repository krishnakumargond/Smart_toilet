using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Smart_toilet.Models;

namespace Smart_toilet.Controllers
{
    [RoutePrefix("api/Device")]
    public class HomeApiController : ApiController
    {
        [HttpGet]
        [Route("Getdeviceinfo")]
        public List<DeviceInfo> Get()
        {
            List<DeviceInfo> lst = new List<DeviceInfo>();
            SqlDataReader reader = null;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Server=ecosmartdc.com;Database=dbSoubhagya;User ID=sa;Password=india@123;";

            SqlCommand sqlCmd = new SqlCommand("spGetDeviceInfo", con);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            reader = sqlCmd.ExecuteReader();
            DeviceInfo emp = null;
            while (reader.Read())
            {
                emp = new DeviceInfo();
                emp.Imei = reader["Imei"].ToString();
                emp.V1 = reader["V1"].ToString();
                emp.V2 = reader["V2"].ToString();
                emp.V3 = reader["V3"].ToString();
                emp.V4 = reader["V4"].ToString();
                emp.V5 = reader["V5"].ToString();
                emp.Tdate = reader["TDate"].ToString();
                lst.Add(emp);
            }
            con.Close();
            return lst;


        }
        [HttpGet]
        [Route("AddDevice")]
        public string AddDevice(string ImeiNo, string V1, string V2)
        {
            string msg = "";

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Server=ecosmartdc.com;Database=dbSoubhagya;User ID=sa;Password=india@123;";

                SqlCommand sqlCmd = new SqlCommand("spInsertDemotableData", con);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@ImeiNo", ImeiNo);
                sqlCmd.Parameters.AddWithValue("@V1", V1);
                sqlCmd.Parameters.AddWithValue("@V2", V2);
                con.Open();
                sqlCmd.ExecuteNonQuery();
                con.Close();
                msg = "save";
            }

            catch (Exception ex)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                msg = "Somthing Worng";
            }
            return msg;


        }
        [HttpGet]
        [Route("GetTestDeviceinfo")]
        public List<DemoTableInfo> GetTestDeviceinfo()
        {
            List<DemoTableInfo> lst = new List<DemoTableInfo>();
            SqlDataReader reader = null;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Server=ecosmartdc.com;Database=dbSoubhagya;User ID=sa;Password=india@123;";

            SqlCommand sqlCmd = new SqlCommand("spGetDemotableData", con);
            sqlCmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            reader = sqlCmd.ExecuteReader();
            DemoTableInfo emp = null;
            while (reader.Read())
            {
                emp = new DemoTableInfo();
                emp.ImeiNo = reader["ImeiNo"].ToString();
                emp.V1 = reader["V1"].ToString();
                emp.V2 = reader["V2"].ToString();

                lst.Add(emp);
            }
            con.Close();
            return lst;


        }
        [HttpGet]
        [Route("GetRaindeviceinfo")]
        public List<DevicerainInfo> Getrain()
        {
            List<DevicerainInfo> lst = new List<DevicerainInfo>();
            SqlDataReader reader = null;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Server=ecosmartdc.com;Database=dbSoubhagya;User ID=sa;Password=india@123;";

            SqlCommand sqlCmd = new SqlCommand("sp_GetAllRainSensor", con);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@IsType", 2);
            con.Open();

            reader = sqlCmd.ExecuteReader();
            DevicerainInfo emp = null;
            while (reader.Read())
            {
                emp = new DevicerainInfo();
                emp.RainSensorId = Convert.ToInt32(reader["RainSensorId"].ToString());
                emp.Imei = reader["IMEINO"].ToString();
                emp.raifall = reader["RAIFALL"].ToString();
                emp.light = reader["LIGHT"].ToString();
                emp.Tdate = reader["Tdate"].ToString();
                lst.Add(emp);
            }
            con.Close();
            return lst;


        }

        [HttpGet]
        [Route("GetRaindeviceBackendinfo")]
        public List<DevicerainInfo> GetRaindeviceBackendinfo()
        {
            List<DevicerainInfo> lst = new List<DevicerainInfo>();
            SqlDataReader reader = null;
            SqlConnection con = new SqlConnection();
            con.ConnectionString = @"Server=ecosmartdc.com;Database=dbSoubhagya;User ID=sa;Password=india@123;";

            SqlCommand sqlCmd = new SqlCommand("sp_GetAllRainSensor", con);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.Parameters.AddWithValue("@IsType", 1);

            con.Open();

            reader = sqlCmd.ExecuteReader();
            DevicerainInfo emp = null;
            while (reader.Read())
            {
                emp = new DevicerainInfo();
                emp.RainSensorId = Convert.ToInt32(reader["RainSensorId"].ToString());
                emp.Imei = reader["IMEINO"].ToString();
                emp.raifall = reader["RAIFALL"].ToString();
                emp.light = reader["LIGHT"].ToString();
                emp.Tdate = reader["Tdate"].ToString();
                lst.Add(emp);
            }
            con.Close();
            return lst;


        }
        [HttpGet]
        [Route("AddRainDevice")]
        public HttpResponseMessage AddRainDevice(string ImeiNo, float raifall, float light)
        {
            string msg = "FACK" + Environment.NewLine;

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Server=ecosmartdc.com;Database=dbSoubhagya;User ID=sa;Password=india@123;";

                SqlCommand sqlCmd = new SqlCommand("sp_InsertRainSensor", con);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@IMEINO", ImeiNo);
                sqlCmd.Parameters.AddWithValue("@RAIFALL", raifall);
                sqlCmd.Parameters.AddWithValue("@LIGHT", light);
                sqlCmd.Parameters.AddWithValue("@IsType", 1);
                con.Open();
                sqlCmd.ExecuteNonQuery();
                con.Close();
                msg = "PACK" + Environment.NewLine;
            }

            catch (Exception ex)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                msg = "FACK" + Environment.NewLine;
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(msg, System.Text.Encoding.UTF8, "text/plain");
            return resp;


        }
        [HttpGet]
        [Route("AddRainDeviceBackend")]
        public HttpResponseMessage AddRainDeviceBackend(string ImeiNo, float raifall, float light)
        {
            string msg = "FACK" + Environment.NewLine;

            try
            {
                SqlConnection con = new SqlConnection();
                con.ConnectionString = @"Server=ecosmartdc.com;Database=dbSoubhagya;User ID=sa;Password=india@123;";

                SqlCommand sqlCmd = new SqlCommand("sp_InsertRainSensor", con);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                sqlCmd.Parameters.AddWithValue("@IMEINO", ImeiNo);
                sqlCmd.Parameters.AddWithValue("@RAIFALL", raifall);
                sqlCmd.Parameters.AddWithValue("@LIGHT", light);
                sqlCmd.Parameters.AddWithValue("@IsType", 2);
                con.Open();
                sqlCmd.ExecuteNonQuery();
                con.Close();
                msg = "PACK" + Environment.NewLine;
            }

            catch (Exception ex)
            {
                Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                msg = "FACK" + Environment.NewLine;
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(msg, System.Text.Encoding.UTF8, "text/plain");
            return resp;


        }
    }
}
