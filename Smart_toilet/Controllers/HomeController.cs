using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smart_toilet.DAL;
using Smart_toilet.Models;
using Smart_toilet.Security;

namespace Smart_toilet.Controllers
{
    [CustomAuthorize()]
    public class HomeController : Controller
    {
        // GET: Home
        //#region Properties
        //private MasterContext _MasterContext;
        //private MasterContext oMasterContext
        //{
        //    get
        //    {
        //        if (_MasterContext == null)
        //            _MasterContext = new MasterContext();
        //        return _MasterContext;
        //    }
        //}
        //#endregion

        db_smart_toiletEntities db = new db_smart_toiletEntities();
        // GET: Master
        public ActionResult AddDevice(int DeviceId)
        {
            tbl_Toilet d = new tbl_Toilet();

            if (DeviceId > 0)
            {


                d = db.tbl_Toilet.Where(i => i.Id == DeviceId).FirstOrDefault();

            }
            return View(d);
        }
        [HttpPost]
        public ActionResult AddDevice(tbl_Toilet d)
        {
            tbl_Toilet de = new tbl_Toilet();
            if (d.Id > 0)
            {
                de.ToiletCode = d.ToiletCode;
                de.Address = d.Address;
                de.IsActive = d.IsActive;
                //db.usp_UpdateDevice(d.DeviceId, de.ProductId, de.ProductName, de.IsActive);
                TempData["msg"] = "<Script> alert('Update Device.') </Script>";
            }

            else
            {
                if (db.tbl_Toilet.Any(ac => ac.ToiletCode.Equals(d.ToiletCode)))
                {
                    TempData["msg"] = "<Script> alert('Device Name already exists.') </Script>";

                }
                else
                {
                    //tbl_Device de = new tbl_Device();
                    de.ToiletCode = d.ToiletCode;
                    de.Address = d.Address;
                    de.IsActive = d.IsActive;
                    de.Lat = d.Lat;
                    de.Lng = d.Lng;
                    db.usp_InerstDevice(de.ToiletCode,de.Address,de.IsActive,de.Lat,de.Lng);
                    TempData["msg"] = "<Script> alert('Save Device.') </Script>";
                }


            }
            return RedirectToAction("Device");
        }
        public ActionResult Device()
        {

            var data = db.tbl_Toilet;
            return View(data.ToList());
        }
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult LoadGrid(DataTableAjaxPostModel requestModel)
        {

            // Initialization.  
            Order _order = new Order();

            string SearchTXT = requestModel.search.value;
            int draw = requestModel.draw;
            int start = requestModel.start;
            int length = requestModel.length;
            MasterContext oMasterContextNew = new MasterContext();
            string str = string.Empty;
            if (SearchTXT != null)
                str = SearchTXT.Trim();


            List<rpt_GetAllTransition_Result> _lst = oMasterContextNew.GetLocationDataInfoWithPaging(length, str, start);
            int tt = 0;
            if (_lst.Count > 0)
            {
                tt = _lst.Count;
            }
            var response = new { data = _lst, recordsFiltered = tt, recordsTotal = tt };
            return Json(response);
        }
        public JsonResult LoadChildLightData(string DeviceId)
        {
            MasterContext oMasterContextNew = new MasterContext();
            List<rpt_GetAllTransitionByProdectId_Result> Result = oMasterContextNew.GetSmartLightByIdInfo(DeviceId);
            var response = new { data = Result };
            return Json(response);
        }
        public ActionResult dashboard()
        {

            //MasterContext oMasterContextNew = new MasterContext();
            //List<rpt_Desbord_Result> Result = oMasterContextNew.GetDesbord();
            return View();
        }
        [HttpPost]
        public JsonResult Getloction()
        {
            List<rpt_GetLoction_Result> _lst = new List<rpt_GetLoction_Result>();
            _lst = db.rpt_GetLoction().ToList();
            return Json(_lst, JsonRequestBehavior.AllowGet);
        }
        //[HttpPost]
        //public JsonResult GetloctionById(string objId)
        //{
        //    List<rp> _lst = new List<rpt_GetLoctionbyid_Result>();
        //    _lst = db.rpt_GetLoctionbyid(objId).ToList();
        //    return Json(_lst, JsonRequestBehavior.AllowGet);
        //}

    }
}