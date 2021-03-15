using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Test
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            insertDataToDataReview();
        }

        public void insertDataToDataReview()
        {
            Test.DataReviewWorksheet.DataReviewWorksheet_Service svc = new DataReviewWorksheet.DataReviewWorksheet_Service();
            svc.UseDefaultCredentials = true;

            DataReviewWorksheet.DataReviewWorksheet RecordData = new DataReviewWorksheet.DataReviewWorksheet();

            RecordData.Cinema_Code = "HO2DCHCHEE";

            RecordData.Entry_NoSpecified = true;
            RecordData.Entry_No = 80;

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

            RecordData.Remark = "Chi Chi Em Em (2D-VIET.SUB)";

            RecordData.Transaction_TypeSpecified = true;
            RecordData.Transaction_Type = DataReviewWorksheet.Transaction_Type.Box_Office;

            RecordData.Import_TimeSpecified = true;
            RecordData.Import_Time = DateTime.Now;

            RecordData.Import_User = "GALAXY\\PHONGDD";

            RecordData.StatusSpecified = true;
            RecordData.Status = DataReviewWorksheet.Status.Open;

            RecordData.Location_Code = "BMT-CO";

            RecordData.TypeSpecified = true;
            RecordData.Type = DataReviewWorksheet.Type.G_L_Account;
            
            try
            {
                svc.Create(ref RecordData);
            }
            catch (Exception e)
            {
                string msg = e.Message;
                throw;
            }
        }

    }
}