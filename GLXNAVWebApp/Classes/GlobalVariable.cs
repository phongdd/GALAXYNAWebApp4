using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace GLXNAVWebApp.Classes
{
    public class GlobalVariable
    {
        public static String STR_USERNAME = "username";
        public static String STR_COMPANY = "API TC";
        public static String STR_FULLNAME = "fullname";

        public static String UserName
        {
            get
            {
                if (HttpContext.Current.Session[STR_USERNAME] != null)
                {
                    return HttpContext.Current.Session[STR_USERNAME].ToString();
                }
                else
                {
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session[STR_USERNAME] = value;
            }
        }

        public static String CompanyName
        {
            get
            {
                return HttpContext.Current.Session[STR_COMPANY].ToString();
            }
            set
            {
                HttpContext.Current.Session[STR_COMPANY] = value;
            }
        }

        public static String FullName
        {
            get
            {
                if (HttpContext.Current.Session[STR_FULLNAME] != null)
                {
                    return HttpContext.Current.Session[STR_FULLNAME].ToString();
                }
                else
                {
                    HttpContext.Current.Response.Redirect(FormsAuthentication.LoginUrl);
                }
                return "";
            }
            set
            {
                HttpContext.Current.Session[STR_FULLNAME] = value;
            }
        }        

    }
}