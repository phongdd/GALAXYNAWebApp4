using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace GLXNAVWebApp.Classes
{
    public class Common
    {
        #region Get Values web.config
        string AuthorizedUser = WebConfigurationManager.AppSettings["authorizeduser"];
        string AuthorizedUserPsw = WebConfigurationManager.AppSettings["authorizeduserpsw"];

        string Server = WebConfigurationManager.AppSettings["Server"];
        string Port = WebConfigurationManager.AppSettings["Port"];
        string Instance = WebConfigurationManager.AppSettings["Instance"];
        string WebService = WebConfigurationManager.AppSettings["WebService"];      
        string Company = WebConfigurationManager.AppSettings["Company"];
        string PageType = WebConfigurationManager.AppSettings["PageType"];
        string ServiceName = WebConfigurationManager.AppSettings["ServiceName"];
        #endregion Get Values web.config
        #region Variable
        const int fetchSize = 10;
        string bookmarkKey = null;
        #endregion
        public ICredentials CheckCredentials()
        {
            ICredentials IC;
            IC = new NetworkCredential(AuthorizedUser, AuthorizedUserPsw);
            return IC;
        }
        public string ReBuildUrl(string Url, string CurCompany)
        {
            string NewUrl = string.Empty;
            if (Url.Contains("Page"))
            {
                PageType = "Page";
            }
            if (Url.Contains("Codeunit"))
            {
                PageType = "Codeunit";
            }
            if (Url.Contains("Query"))
            {
                PageType = "Query";
            }
            string[] Lines = Regex.Split(Url, "/");
            ServiceName = Lines[Lines.Length - 1];
            
            if (!string.IsNullOrEmpty(CurCompany))
                Company = CurCompany;
            else
                Company = GlobalVariable.CompanyName;

            NewUrl = string.Format("{0}{1}/{2}/{3}/{4}/{5}/{6}", Server, Port, Instance, WebService, Company, PageType, ServiceName);
            return NewUrl;
        }

        public class OptionString
        {
            public string value;
            public string text;
            public string displaytext;
        }
    }
}