using Ext.Net;
using GLXNAVWebApp.Classes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace GLXNAVWebApp.Forms.PurchaseOrder
{
    /// <summary>
    /// Summary description for SvcPurchaseOrder
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class SvcPurchaseOrder : System.Web.Services.WebService
    {
        const int fetchSize = 10;
        string bookmarkKey = null;
        Common common = new Common();

        [WebMethod]
        public List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line> GetPOLine(string DocumentType, string DocumentNo, string CurCompany)
        {
            GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service svc = new GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service();
            svc.Url = common.ReBuildUrl(svc.Url, CurCompany); 
            svc.Credentials = common.CheckCredentials();

            List<GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter> ReqFilters = new List<GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter>();

            GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter DocNoFilter = new GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter();
            DocNoFilter.Field = GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Fields.No;
            DocNoFilter.Criteria = DocumentNo;

            ReqFilters.Add(DocNoFilter);
            GLXNAVWebApp.PurchaseOrder.PurchaseOrder[] POList;
            POList = svc.ReadMultiple(ReqFilters.ToArray(), bookmarkKey, 10);

            List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line> POLines = new List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line>();
            foreach (GLXNAVWebApp.PurchaseOrder.PurchaseOrder item in POList)
            {
                POLines = item.PurchLines.ToList();
                //item.PurchLines.Where(x=>x.Finished)
            }
            return POLines;
        }

        [WebMethod]
        public DirectResponse UpdateData(string HdrChangedData, string dData, string LineChangedData, string CurCompany)
        {
            DirectResponse dr = new DirectResponse();
            Dictionary<string, object> jsonData = JSON.Deserialize<Dictionary<string, object>>(HdrChangedData);

            #region Parse data to class hdr
            GLXNAVWebApp.PurchaseOrder.PurchaseOrder g = (GLXNAVWebApp.PurchaseOrder.PurchaseOrder)
                JsonConvert.DeserializeObject(HdrChangedData, typeof(GLXNAVWebApp.PurchaseOrder.PurchaseOrder), 
                                                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Include});

            //workaround
            g.Posting_Date = g.Posting_Date.AddDays(1);

            #endregion Parse data to class

            #region Data of Request Line
            //Get modified data
            List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line> line = new List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line>();
            if (!string.IsNullOrEmpty(dData))
            {
                line = JSON.Deserialize<List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line>>(dData);
            }
            if (line != null)
            {
                g.PurchLines = line.ToArray();

                //Set key null for new line
                foreach (GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line item in g.PurchLines)
                {
                    if (string.IsNullOrEmpty(item.Key))
                    {
                        item.Key = null;
                    }
                }
            }
            #endregion Data of Request Line

            #region Update data
            GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service svc = new GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service();
            svc.Url = common.ReBuildUrl(svc.Url, CurCompany);
            svc.Credentials = common.CheckCredentials();

            try
            {
                if (string.IsNullOrEmpty(g.Key))
                    svc.Create(ref g);
                else
                    svc.Update(ref g);
            }
            catch (Exception e)
            {
                dr.Success = false;
                dr.Result = "{control: 'grid'}";
                dr.ErrorMessage = e.Message;
                return dr;
            }
            #endregion Update data

            string json = JSON.Serialize(g);
            json = json.Replace(@"'", @" ");

            dr.Result = String.Format("{{documentNo:'{0}',record:'{1}'}}", g.No, json);
            return dr;
        }

        [WebMethod]
        public DirectResponse PostReceive(string docNo, string CurCompany)
        {
            DirectResponse dr = new DirectResponse();

            GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service svc = new GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service();
            svc.Url = common.ReBuildUrl(svc.Url, CurCompany);
            svc.Credentials = common.CheckCredentials();

            GLXNAVWebApp.PurchaseOrder.PurchaseOrder g;
            g = svc.Read(docNo);
            try
            {
                svc.PostReceive(g.Key);
                g = svc.Read(docNo);
            }
            catch (Exception e)
            {
                dr.Success = false;
                dr.Result = "{control: 'grid'}";
                dr.ErrorMessage = e.Message;
                return dr;
            }
            string json = JSON.Serialize(g);
            json = json.Replace(@"'", @" ");

            dr.Result = String.Format("{{documentNo:'{0}',record:'{1}'}}", g.No, json);
            return dr;
        }
    }
}