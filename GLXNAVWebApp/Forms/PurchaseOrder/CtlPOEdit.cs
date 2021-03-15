using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Forms.PurchaseOrder
{
    public class CtlPOEdit:Window
    {
        #region Declare Controls
        Ext.Net.Toolbar topBar;
        Ext.Net.Button btnSave;
        Ext.Net.Button btnPost;

        #region Fields
        Hidden txtKey;
        Ext.Net.TextField txtNo;
        Ext.Net.TextField txtBuy_from_Vendor_No;
        Ext.Net.TextField txtBuy_from_Vendor_Name;
        Ext.Net.DateField txtOrder_Date;
        Ext.Net.DateField txtDocument_Date;
        Ext.Net.DateField txtPosting_Date;
        Ext.Net.TextField txtVietnamese_Desc;
        Ext.Net.TextField txtVAT_Desc;
        Ext.Net.Checkbox cboReceive;
        #endregion Fields

        public GridPanelEditBase grdLine;
        public List<LookupFormatData> dataTemplateLine;

        public Store stoHeader;
        public FormPanel frmHeader;
        public TabPanel tabHeader;
        public FormPanel frmGeneral;
        public Panel pnlLine;
        public Panel pnlService;
        #endregion Declare Controls

        Common common = new Common();
        public string SCOPE = "GLX.FORMS.POEdit";
        public string DocumentNo;
        public string CurCompany = string.Empty;

        public CtlPOEdit(string Key, string DocNo, string Mode)
        {
            CurCompany = GlobalVariable.CompanyName;
            this.ID = "winCard";
            DocumentNo = DocNo;
            InitComponents();
            GetDataComboBox();
            InitLogis();
            BindingData();
        }

        private void GetDataComboBox()
        {
            
        }

        private void BindingData()
        {
            GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service svc = new GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Service();
            svc.Url = common.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
            svc.Credentials = common.CheckCredentials();

            List<GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter> ReqFilters = new List<GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter>();

            GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter DocNoFilter = new GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Filter();
            DocNoFilter.Field = GLXNAVWebApp.PurchaseOrder.PurchaseOrder_Fields.No;
            DocNoFilter.Criteria = DocumentNo;

            ReqFilters.Add(DocNoFilter);
            GLXNAVWebApp.PurchaseOrder.PurchaseOrder[] POList;
            POList = svc.ReadMultiple(ReqFilters.ToArray(), "", 10);
            
            foreach (GLXNAVWebApp.PurchaseOrder.PurchaseOrder item in POList)
            {
                frmHeader.SetValues(item);
            }
        }

        private List<LookupFormatData> BuilDataTemplateLine()
        {
            return new List<LookupFormatData>
            {
                new LookupFormatData { ID = "lfdKey", FieldName = "Key", FieldTitle = "key", Type = ModelFieldType.String, ColWidth = 0 },
                new LookupFormatData { ID = "Shortcut_Dimension_1_Code", FieldName = "Shortcut_Dimension_1_Code", FieldTitle = "Entity Code", Type = ModelFieldType.String, ColWidth = 120, ReadOnly = true,  },
                new LookupFormatData { ID = "Expected_Receipt_Date", FieldName = "Expected_Receipt_Date", FieldTitle = "Expected Receipt Date", Type = ModelFieldType.Date, ColWidth = 150, ReadOnly = true },
                new LookupFormatData { ID = "Order_Date", FieldName = "Order_Date", FieldTitle = "Order Date", Type = ModelFieldType.Date, ColWidth = 120, ReadOnly = true },
                new LookupFormatData { ID = "Finished", FieldName = "Finished", FieldTitle = "Finished", Type = ModelFieldType.Boolean, ColWidth = 120, ReadOnly = false },
                new LookupFormatData { ID = "Location_Code", FieldName = "Location_Code", FieldTitle = "Location Code", Type = ModelFieldType.String, ColWidth = 120, ReadOnly = true },
                new LookupFormatData { ID = "No", FieldName = "No", FieldTitle = "No", Type = ModelFieldType.String, ColWidth = 120, ReadOnly = true },
                new LookupFormatData { ID = "Description", FieldName = "Description", FieldTitle = "Description", Type = ModelFieldType.String, ColWidth = 180, ReadOnly = true },
                new LookupFormatData { ID = "Vietnamese_Description", FieldName = "Vietnamese_Description", FieldTitle = "Vietnamese Description", Type = ModelFieldType.String, ColWidth = 180, ReadOnly = true },
                new LookupFormatData { ID = "Unit_of_Measure", FieldName = "Unit_of_Measure", FieldTitle = "UOM", Type = ModelFieldType.String, ColWidth = 100, ReadOnly = true },
                new LookupFormatData { ID = "Qty_to_Receive", FieldName = "Qty_to_Receive", FieldTitle = "Qty.to Receive", Type = ModelFieldType.Float, Format = "0.00/i", ColWidth = 130 },
                new LookupFormatData { ID = "Quantity", FieldName = "Quantity", FieldTitle = "Quantity Order", Type = ModelFieldType.Float, Format = "0.00/i", ColWidth = 130, ReadOnly = true },
                new LookupFormatData { ID = "Unit_Cost_LCY", FieldName = "Unit_Cost_LCY", FieldTitle = "Unit Cost LCY", Type = ModelFieldType.Float, Format = "0.00/i", ColWidth = 130, ReadOnly = true },
                new LookupFormatData { ID = "Quantity_Received", FieldName = "Quantity_Received", FieldTitle = "Quantity Received", Type = ModelFieldType.Float, Format = "0.00/i", ColWidth = 130, ReadOnly = true },
                //new LookupFormatData { ID = "Type", FieldName = "Type", FieldTitle = "Type", Type = ModelFieldType.String, ColWidth = 120, ReadOnly = true },
                
                /*
                    Shortcut_Dimension_1_Code
                    Location_Code
                    Type
                    No
                    Description
                    Vietnamese_Description
                    Unit_of_Measure
                    Quantity
                    Unit_Cost_LCY
                    Quantity_Received
                    Qty_to_Receive
                 */
            };
        }

        private void InitLogis()
        {
            btnSave.Listeners.Click.Handler = SCOPE + ".Save();";
            btnPost.Listeners.Click.Handler = SCOPE + ".PostReceive();";
        }

        private void InitComponents()
        {
            #region Header
            #region Button
            btnSave = new Button { ID = "btnSave", Icon = Icon.Disk, Text = "Save", ToolTip = "Save" };
            btnPost = new Button { ID = "btnPost", Icon = Icon.PackageAdd, Text = "Post Receive", ToolTip = "Post Receive" };
            topBar = new Toolbar { ID = "topBar", Items = { btnSave, btnPost } };
            #endregion Button

            #region Fields
            txtKey = new Hidden { DataIndex = "Key", Name = "Key", LabelWidth = 150, Anchor = "100%", FieldLabel = "Key", ID = "Key" };
            txtNo = new TextField { LabelWidth = 150, Anchor = "100%", FieldLabel = "No.", DataIndex = "No", Name = "No", ID = "txtNo", ReadOnly = true };
            txtBuy_from_Vendor_No = new TextField { LabelWidth = 150, Anchor = "100%", FieldLabel = "Buy-from Vendor No.", DataIndex = "Buy_from_Vendor_No", Name = "Buy_from_Vendor_No", ID = "txtBuy_from_Vendor_No", ReadOnly = true};
            txtBuy_from_Vendor_Name = new TextField { LabelWidth = 150, Anchor = "100%", FieldLabel = "Buy-from Vendor Name", DataIndex = "Buy_from_Vendor_Name", Name = "Buy_from_Vendor_Name", ID = "txtBuy_from_Vendor_Name", ReadOnly = true };
            txtOrder_Date = new DateField { LabelWidth = 150, FieldLabel = "Order Date", Anchor = "100%", Format = "dd/MM/yyyy", SubmitFormat = "dd/MM/yyyy", DataIndex = "Order_Date", Name = " Order_Date", SelectOnFocus = true, ID = "txtOrder_Date", ReadOnly = true };
            txtDocument_Date = new DateField { LabelWidth = 150, FieldLabel = "Document Date", Anchor = "100%", Format = "dd/MM/yyyy", SubmitFormat = "dd/MM/yyyy", DataIndex = "Document_Date", Name = " Document_Date", SelectOnFocus = true, ID = "txtDocument_Date", ReadOnly = true };
            txtPosting_Date = new DateField { LabelWidth = 150, FieldLabel = "Posting Date", Anchor = "100%", Format = "dd/MM/yyyy", SubmitFormat = "dd/MM/yyyy", DataIndex = "Posting_Date", Name = " Posting_Date", SelectOnFocus = true, ID = "txtPosting_Date", AllowBlank = false, MsgTarget = MessageTarget.Side };
            txtVietnamese_Desc = new TextField { LabelWidth = 150, FieldLabel = "Vietnamese Description", Anchor = "100%", DataIndex = "Vietnamese_Description", Name = " Vietnamese_Description", SelectOnFocus = true, ID = "txtVietnamese_Desc", AllowBlank = false, MsgTarget = MessageTarget.Side };
            txtVAT_Desc = new TextField { LabelWidth = 150, FieldLabel = "VAT Description", Anchor = "100%", DataIndex = "VAT_Description", Name = " VAT_Description", SelectOnFocus = true, ID = "txtVAT_Desc", AllowBlank = false, MsgTarget = MessageTarget.Side };
            cboReceive = new Checkbox { LabelWidth = 150, FieldLabel = "Receive", Anchor = "100%", DataIndex = "Receive", Name = " Receive", ID = "cboReceive" };
            #endregion Fields

            #region FormPanel
            frmHeader = new FormPanel
            {
                //Margins = "10 10 0 10",
                Region = Region.North,
                Icon = Ext.Net.Icon.ApplicationForm,
                Title = "General",
                Border = true,
                CollapseMode = CollapseMode.Header,
                Collapsible = true,
                ID = "frmHeader",
                TrackResetOnLoad = true,
                Layout = "Hbox",
                Items =
                {
                    new Panel {
                        Layout = "Anchor",
                        Flex = 1,
                        BodyPaddingSummary = "10 10 0 10",
                        Border = false,
                        Items = { txtKey, txtNo, txtBuy_from_Vendor_No, txtBuy_from_Vendor_Name, txtOrder_Date }
                    },
                    new Panel {
                        Layout = "Anchor",
                        Flex = 1,
                        BodyPaddingSummary = "10 10 0 10",
                        Border = false,
                        Items = { txtPosting_Date, txtVietnamese_Desc, txtVAT_Desc, cboReceive }
                    }
                }
            };
            #endregion FormPanel
            #endregion Header

            #region Line
            #region GridLine
            dataTemplateLine = BuilDataTemplateLine();
            grdLine = new GridPanelEditBase
            {
                ID = "grdLine",
                Border = false,
                TitleAlign = Ext.Net.TitleAlign.Center,
                ProxyUrl = @"..//..//..//..//PurchaseOrder//SvcPurchaseOrder.asmx/GetPOLine",
                DocumentType = "",
                DocumentNo = DocumentNo,
                CurCompany = GlobalVariable.CompanyName,
                SCOPE = this.SCOPE,
                DataTemplate = dataTemplateLine,
            };
            #endregion GridLine

            #region pnlLine
            pnlLine = new Panel
            {
                ID = "pnlLine",
                Region = Ext.Net.Region.Center,
                Layout = "Fit",
                Icon = Ext.Net.Icon.Group,
                Title = "Lines",
                Border = true,
                //Margins = "10 10 5 10",
                Split = true,
                Items = { grdLine }
            };
            #endregion pnlLine
            #endregion Line

            #region Windows            
            this.ID = "winCard";
            this.Maximizable = false;
            this.Minimizable = false;
            this.CloseAction = CloseAction.Destroy;
            this.Icon = Icon.ApplicationEdit;
            this.TopBar.Add(topBar);
            this.Layout = "Border";
            this.Items.AddRange(new ItemsCollection<Ext.Net.AbstractComponent> { this.frmHeader, this.pnlLine });
            #endregion Windows
        }
    }
}