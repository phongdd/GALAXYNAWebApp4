using Ext.Net;
using GLXNAVWebApp.Classes;
using GLXNAVWebApp.POWaitingForApprove;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace GLXNAVWebApp.Forms.ReqWorksheet
{
    /// <summary>
    /// Summary description for SvcReqWorksheet
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SvcReqWorksheet : System.Web.Services.WebService
    {
        Common Clsc = new Common();

        [WebMethod]
        public DirectResponse DeleteData(string key, string CurCompany)
        {
            DirectResponse dr = new DirectResponse();
            GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service svc = new GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service();
            svc.Url = Clsc.ReBuildUrl(svc.Url, CurCompany);
            svc.Credentials = Clsc.CheckCredentials();

            if (!string.IsNullOrEmpty(key))
            {
                try
                {
                    svc.Delete("DEFAULT", key);
                }
                catch (Exception e)
                {
                    dr.Success = false;
                    dr.Result = "Completed with error";
                    dr.ErrorMessage = e.Message;
                    return dr;
                }
            }
            dr.Result = "Completed with success";
            return dr;
        }

        [WebMethod]
        public DirectResponse UpdateData(string Batch, string HdrChangedData, string CurCompany)
        {
            DirectResponse dr = new DirectResponse();
            Dictionary<string, object> jsonData = JSON.Deserialize<Dictionary<string, object>>(HdrChangedData);

            #region Parse data to class hdr
            //Nếu Action_Message có giá trị bằng null => Action_Message = ""
            HdrChangedData = HdrChangedData.Replace(String.Format(@"""{0}"":{1}", "Action_Message", "null"), String.Format(@"""{0}"":""{1}""", "Action_Message", ""));

            //Nếu Replenishment_System có giá trị bằng null => Replenishment_System = ""
            HdrChangedData = HdrChangedData.Replace(String.Format(@"""{0}"":{1}", "Replenishment_System", "null"), String.Format(@"""{0}"":""{1}""", "Replenishment_System", ""));

            //Set value of Option string to integer
            //int i = 0;
            //foreach (string item in GLXNAVWebApp.ReqWorksheet.Action_Message.GetNames(typeof(GLXNAVWebApp.ReqWorksheet.Action_Message)))
            //{
            //    string t = item.ToString().Replace("_", " ");
            //    HdrChangedData = HdrChangedData.Replace(String.Format(@"""{0}"":""{1}""", "Action_Message", t), String.Format(@"""{0}"":""{1}""", "Action_Message", i.ToString()));
            //    i += 1;
            //}

            //i = 0;
            //foreach (string item in GLXNAVWebApp.ReqWorksheet.Replenishment_System.GetNames(typeof(GLXNAVWebApp.ReqWorksheet.Replenishment_System)))
            //{
            //    string t = item.ToString().Replace("_", " ");
            //    HdrChangedData = HdrChangedData.Replace(String.Format(@"""{0}"":""{1}""", "Replenishment_System", t), String.Format(@"""{0}"":""{1}""", "Replenishment_System", i.ToString()));
            //    i += 1;
            //}

            GLXNAVWebApp.ReqWorksheet.ReqWorksheet g = (GLXNAVWebApp.ReqWorksheet.ReqWorksheet)
                JsonConvert.DeserializeObject(HdrChangedData, typeof(GLXNAVWebApp.ReqWorksheet.ReqWorksheet),
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include });
            #endregion Parse data to class

            #region
            //GLXNAVWebApp.ReqWorksheet_CodeUnit.ReqWorksheet_CodeUnit. = new GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service();
            GLXNAVWebApp.GLXWebService.GLXWebService_Service svc1 = new GLXWebService.GLXWebService_Service();
            #endregion

            #region Update data
            GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service svc = new GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service();
            svc.Url = Clsc.ReBuildUrl(svc.Url, CurCompany);
            svc.Credentials = Clsc.CheckCredentials();

            try
            {
                if (string.IsNullOrEmpty(g.Key))
                    svc.Create(Batch, ref g);
                else
                    svc.Update(Batch, ref g);
            }
            catch (Exception e)
            {
                dr.Success = false;
                dr.Result = "{control: 'grid'}";
                dr.ErrorMessage = e.Message;
            }
            #endregion Update data

            string json = JSON.Serialize(g);
            json = json.Replace(@"'", @" ");

            dr.Result = String.Format("{{Key:'{0}',No:'{1}',record:'{2}'}}", g.Key, g.No, json);

            return dr;
        }

    }
}
