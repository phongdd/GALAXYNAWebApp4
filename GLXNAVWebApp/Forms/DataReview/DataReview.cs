using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Forms.DataReview
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public class DataReview: Viewport
    {
        Common common = new Common();
        public void TestServiceFFunction()
        {
            DataReviewWorksheet.DataReviewWorksheet_Service svc = new DataReviewWorksheet.DataReviewWorksheet_Service();
            svc.Url = common.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
            svc.Credentials = common.CheckCredentials();

            DataReviewWorksheet.DataReviewWorksheet RecordData = new DataReviewWorksheet.DataReviewWorksheet();

            //RecordData.Key = Guid.NewGuid().ToString();

            //RecordData.Entry_NoSpecified = true;
            //RecordData.Entry_No = 80;

            RecordData.Entity = "CIN-DAK-BMT";

            RecordData.Posting_DateSpecified = true;
            RecordData.Posting_Date = DateTime.Now;

            RecordData.No = "5111";

            RecordData.QuantitySpecified = true;
            RecordData.Quantity = 9;

            RecordData.Unit_Price = 45;
            RecordData.Unit_PriceSpecified = true;

            RecordData.AmountSpecified = true;
            RecordData.Amount = 405;

            RecordData.Remark = "21 Bridges(2D - VIET.SUB)";

            RecordData.Transaction_TypeSpecified = true;
            RecordData.Transaction_Type = DataReviewWorksheet.Transaction_Type.Box_Office;

            RecordData.Import_TimeSpecified = true;
            RecordData.Import_Time = DateTime.Now;

            RecordData.Import_User = "GALAXY\\NAVPRD";

            RecordData.StatusSpecified = true;
            RecordData.Status = DataReviewWorksheet.Status.Open;

            RecordData.Location_Code = "BMT-CO";

            RecordData.TypeSpecified = true;
            RecordData.Type = DataReviewWorksheet.Type.G_L_Account;

            RecordData.Cinema_Code = "HO2D21BRID";
            try
            {
                svc.Update(ref RecordData);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!Ext.Net.X.IsAjaxRequest)
            {
                this.ResourceManager.AddDirectMethodControl(this);
            }

            TestServiceFFunction();
        }
    }
}