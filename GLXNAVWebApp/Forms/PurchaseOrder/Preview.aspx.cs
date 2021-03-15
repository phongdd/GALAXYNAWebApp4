using GLXNAVWebApp.Classes;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GLXNAVWebApp.Forms.PurchaseOrder
{
    public partial class Preview : System.Web.UI.Page
    {
        Common common = new Common();
        string UID = string.Empty;
        string No = string.Empty;
        string Location = string.Empty;
        string VendorNo = string.Empty;
        string VendorName = string.Empty;
        List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line> line;
        protected void Page_Load(object sender, EventArgs e)
        {
            UID = HttpContext.Current.Request.QueryString["UID"].ToString();
        }

        private List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line> GetData()
        {
            List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line> line = new List<GLXNAVWebApp.PurchaseOrder.Purchase_Order_Line>();

            GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service svc = new GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service();
            svc.Url = common.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
            svc.Credentials = common.CheckCredentials();

            GLXNAVWebApp.PurchaseOrder.PurchaseOrder PO = svc.Read(UID);

            this.No = PO.No;
            this.Location = PO.Location_Code;
            this.VendorNo = PO.Buy_from_Vendor_No;
            this.VendorName = PO.Buy_from_Vendor_Name;
            line = PO.PurchLines.ToList();

            return line;
        }

        private void ShowReport()
        {
            RptViewer.ProcessingMode = ProcessingMode.Local;
            RptViewer.LocalReport.ReportPath = Server.MapPath("RptPO.rdlc");

            line = GetData();

            ReportDataSource rds = new ReportDataSource("DataSet1", line);
            ReportParameter[] rptParams = new ReportParameter[] {
                new ReportParameter("No", No),
                new ReportParameter("Location", Location),
                new ReportParameter("VendorNo", VendorNo),
                new ReportParameter("VendorName", VendorName),
            };
            RptViewer.LocalReport.SetParameters(rptParams);

            RptViewer.LocalReport.DataSources.Clear();
            RptViewer.LocalReport.DataSources.Add(rds);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ShowReport();
        }
    }
}