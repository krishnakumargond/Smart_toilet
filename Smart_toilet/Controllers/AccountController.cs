using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Smart_toilet.Security;
using Smart_toilet.Models;
using Smart_toilet.DAL;

namespace Smart_toilet.Controllers
{
    public class AccountController : Controller
    {
        db_smart_toiletEntities db = new db_smart_toiletEntities();
        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(UserInfo model)
        {


            var s = db.GetLoginInfo(model.UserName, model.Password);

            var item = s.FirstOrDefault();
            if (item == "Success")
            {
                SessionPersiter.oUserInfo = model.UserName.ToString();

                return RedirectToAction("dashboard", "Home");
            }
            else if (item == "User Does not Exists")

            {
                TempData["msg2"] = "<script>alert('User Does not Exists');</script>";

                ViewBag.NotValidUser = item;


            }
            else
            {
                ViewBag.Failedcount = item;
                TempData["msg"] = "<script>alert('Incorrect password');</script>";
            }

            return View();
        }
        public ActionResult LogOut()
        {

            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            return RedirectToAction("Login", "Account");
        }
    }
}