using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GLXNAVWebApp
{
    public partial class login : System.Web.UI.Page
    {
        public string arrCompanies = string.Empty;
        public static string bookmarkKey = null;
        public const int fetchSize = 10;
        Common common = new Common();

        protected void Page_Load(object sender, EventArgs e)
        {
            Companies.Companies_Service svc = new GLXNAVWebApp.Companies.Companies_Service();
            svc.Credentials = common.CheckCredentials();

            Companies.Companies[] results = svc.ReadMultiple(null, "", fetchSize);
            List<Companies.Companies> CompanyList = new List<Companies.Companies>();
            while (results.Length > 0)
            {
                bookmarkKey = results.Last().Key;
                CompanyList.AddRange(results);
                results = svc.ReadMultiple(null, bookmarkKey, fetchSize);
            }

            foreach (var item in CompanyList)
            {
                arrCompanies += item.Name + ",";
            }
        }

        [System.Web.Services.WebMethod]
        public static string DoAuthentication(string username, string password, string company)
        {
            //Using Domain
            bool valid = false;
            string fullname = string.Empty;
            using (PrincipalContext context = new PrincipalContext(ContextType.Domain, "galaxy.local.vn"))
            {
                valid = context.ValidateCredentials(username, password);
                if (valid)
                {
                    //string rCheckInNAV = CheckInNAV(username, company);
                    bool rCheckInNAV = CheckMatchingAccComp(username, company);
                    if (rCheckInNAV)
                    {
                        //fullname = System.DirectoryServices.AccountManagement.UserPrincipal.Current.DisplayName;
                        ResourceManager.AjaxSuccess = true;
                        HttpContext.Current.Session[GlobalVariable.STR_USERNAME] = username;
                        HttpContext.Current.Session[GlobalVariable.STR_FULLNAME] = fullname;
                        HttpContext.Current.Session[GlobalVariable.STR_COMPANY] = company;
                        FormsAuthentication.SetAuthCookie(username, false);
                        //================================================================================//
                        HttpContext.Current.Response.Cookies["user_cookies"]["username"] = username;
                        HttpContext.Current.Response.Cookies["user_cookies"]["company"] = company;
                        HttpContext.Current.Response.Cookies["user_cookies"].Expires = DateTime.Now.AddMinutes(60);
                        //================================================================================//
                        return "";
                    }
                    else {
                        return "You cannot login the selected company.";
                    }
                }
                else
                {
                    return "Your Galaxy account or password was entered incorrectly.";
                }
            }
        }

        public static bool CheckMatchingAccComp(string username, string company)
        {
            using (GLXAccountSVC.BSL_GLXAccountClient client = new GLXAccountSVC.BSL_GLXAccountClient())
            {
                bool r = client.CheckExistsItem(username, company);
                return r;
            }
        }

        public static string CheckInNAV(string username, string company)
        {
            //Common common = new Common();
            //UserCard.UserCard_Service abc = new UserCard.UserCard_Service();
            //abc.Credentials = common.CheckCredentials();
            //try
            //{
            //    UserCard.UserCard[] c = abc.ReadMultiple(null, bookmarkKey, fetchSize);
            //}
            //catch (Exception e)
            //{

            //    throw;
            //}
            
            return "";

            //UserPermissionSets.UserPermissionSets_Service svc = new UserPermissionSets.UserPermissionSets_Service();
            //svc.Credentials = common.CheckCredentials();

            //List<UserPermissionSets.UserPermissionSets_Filter> ReqFilters = new List<UserPermissionSets.UserPermissionSets_Filter>();
            //UserPermissionSets.UserPermissionSets_Filter UsernameFilter = new UserPermissionSets.UserPermissionSets_Filter();
            //UsernameFilter.Field = UserPermissionSets.UserPermissionSets_Fields.User_Name;
            //UsernameFilter.Criteria = @"Galaxy\" + username;
            //ReqFilters.Add(UsernameFilter);
            //UserPermissionSets.UserPermissionSets[] results = svc.ReadMultiple(null, bookmarkKey, fetchSize);
            //if (results.Count() > 0)
            //{
            //    string comp = results[0].Company;
            //    if (comp == "") { return ""; }
            //    else
            //    {
            //        foreach (UserPermissionSets.UserPermissionSets item in results)
            //        {
            //            if (item.Company == company) { return ""; }
            //            else { return "You are not allowed to logon to " + company; }
            //        }
            //    }
            //}
            //else
            //{
            //    return "You do not have permission to login.";
            //}
            //return "";
        }

        private void SecurityApply(string username, string fullname, string wcfUsername, string wcfPassword)
        {
            HttpContext.Current.Session[GlobalVariable.STR_USERNAME] = username;
            HttpContext.Current.Session[GlobalVariable.STR_FULLNAME] = fullname;
        }
    }
}