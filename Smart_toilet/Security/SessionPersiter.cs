using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Smart_toilet.Security
{
    public class SessionPersiter
    {
        static string usernameSessionvar = "UserName";
        public static string oUserInfo
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;
                var sessionvar = HttpContext.Current.Session[usernameSessionvar];
                if (sessionvar != null)
                    return sessionvar as string;
                return null;
            }
            set
            {
                HttpContext.Current.Session[usernameSessionvar] = value;
            }
        }
    }
}