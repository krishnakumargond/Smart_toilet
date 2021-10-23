using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_toilet.DAL
{
    public class MasterContext
    {
        internal List<rpt_GetAllTransition_Result> GetLocationDataInfoWithPaging(int PageSize, string Search, int PageNumber)
        {
            List<rpt_GetAllTransition_Result> _lst = new List<rpt_GetAllTransition_Result>();
            using (db_smart_toiletEntities dbContext = new db_smart_toiletEntities())
            {
                _lst = dbContext.rpt_GetAllTransition(PageSize, Search, PageNumber).ToList();
            }
            return _lst;
        }
        internal List<rpt_GetAllTransitionByProdectId_Result> GetSmartLightByIdInfo(string DeviceId)
        {
            List<rpt_GetAllTransitionByProdectId_Result> _lst = new List<rpt_GetAllTransitionByProdectId_Result>();
            using (db_smart_toiletEntities dbContext = new db_smart_toiletEntities())
            {
                _lst = dbContext.rpt_GetAllTransitionByProdectId(DeviceId).ToList();
            }
            return _lst;
        }
    }
}