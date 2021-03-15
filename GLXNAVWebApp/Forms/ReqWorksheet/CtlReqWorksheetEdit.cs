using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using static GLXNAVWebApp.Classes.Common;

namespace GLXNAVWebApp.Forms.ReqWorksheet
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public class CtlReqWorksheetEdit : Window
    {
        #region Variable
        Common common = new Common();
        Toolbar topBar;
        Ext.Net.Button btnSave;
        Ext.Net.Button btnDeleteCard;
        Ext.Net.Hidden txtKey;
        Ext.Net.Hidden txtBatch;
        ComboBoxLookup cboNo;
        Ext.Net.ComboBox cboType;
        Ext.Net.ComboBox cboAction_Message;
        Ext.Net.Checkbox cboAccept_Action_Message;
        Ext.Net.TextField txtDescription;
        Ext.Net.TextField txtRemark;
        ComboBoxLookup cboLocation_Code;
        Ext.Net.NumberField txtOriginal_Quantity;
        Ext.Net.TextField txtQuantity;
        ComboBoxLookup cboUnit_of_Measure_Code;
        Ext.Net.TextField txtDirect_Unit_Cost;
        Ext.Net.DateField txtDue_Date;
        ComboBoxLookup cboVendor_No;
        Ext.Net.TextField txtVendor_Item_No;
        Ext.Net.ComboBox cboReplenishment_System;
        Ext.Net.Hidden hiddenConverted;
        Ext.Net.Hidden hiddenStatus;
        Ext.Net.TextArea TextAreaChangeLogComment;

        Ext.Net.FormPanel frmHeader;
        Ext.Net.FormPanel frmComment;
        List<OptionString> ActionMessage = new List<OptionString>();
        List<OptionString> ReplenishmentSystem = new List<OptionString>();
        public Ext.Net.Store strUnit_of_Measure;
        public string SCOPE = "GLX.FORMS.ReqWorksheetEdit";
        public string title;
        public string strKey;
        public string JournalBatchName;
        public int LineNo;
        public string Mode;
        public string Status;
        public string Converted;
        #endregion Variablei
        public CtlReqWorksheetEdit(string mode, string Key, string converted, string batch, string status)
        {
            strKey = Key;
            Mode = mode;
            Status = status;
            Converted = converted;
            JournalBatchName = batch;

            this.ID = "winCard";
            InitComponents();
            GetDataComboBox();
            InitLogis();
            BindingData();
        }
        public CtlReqWorksheetEdit() { InitComponents(); }
        private void GetDataComboBox()
        {
            int i = 0;
            string t = string.Empty;
            string displayStr = string.Empty;
            #region ActionMessage
            foreach (string item in GLXNAVWebApp.ReqWorksheet.Action_Message.GetNames(typeof(GLXNAVWebApp.ReqWorksheet.Action_Message)))
            {
                t = item.ToString().Replace("_blank_", "\u00a0");
                t = t.Replace("_", " ");

                //  ,New,Change Qty.,Reschedule,Resched. & Chg. Qty.,Cancel
                switch (t)
                {
                    case "\u00a0":
                        displayStr = " ";
                        break;
                }
                ActionMessage.Add(new OptionString() { value = i.ToString(), text = t, displaytext = displayStr });
                i += 1;
            }
            #endregion ActionMessage
            #region ReplenishmentSystem
            i = 0;
            t = string.Empty;
            displayStr = string.Empty;
            foreach (string item in GLXNAVWebApp.ReqWorksheet.Replenishment_System.GetNames(typeof(GLXNAVWebApp.ReqWorksheet.Replenishment_System)))
            {
                t = item.ToString().Replace("_blank_", "\u00a0");
                t = t.Replace("_", " ");

                //Purchase,Prod. Order,Transfer,Assembly, 
                switch (t)
                {
                    case "\u00a0":
                        displayStr = " ";
                        break;
                }
                ReplenishmentSystem.Add(new OptionString() { value = i.ToString(), text = t, displaytext = displayStr });
                i += 1;
            }
            #endregion ReplenishmentSystem
        }

        private void InitComponents()
        {
            #region Button and Toolbar
            bool display = (Status == "Open" || Status == "") ? true : false;
            btnSave = new Button { ID = "btnSave", Icon = Icon.Disk, Text = "Lưu", ToolTip = "Save", Visible = display };
            btnDeleteCard = new Button { ID = "btnDeleteCard", Icon = Icon.Delete, Text = "Xóa", ToolTip = "Delete", Visible = display };
            topBar = new Toolbar { ID = "topBar", Items = { btnSave, btnDeleteCard }, Visible = display };
            #endregion Button and Toolbar
            #region Batch and Key
            txtBatch = new Ext.Net.Hidden { Name = "Batch", LabelWidth = 150, Anchor = "100%", FieldLabel = "Batch", ID = "txtBatch" };
            txtKey = new Ext.Net.Hidden { DataIndex = "UID", Name = "UID", LabelWidth = 150, Anchor = "100%", FieldLabel = "UID", ID = "txtKey" };
            #endregion Batch and Key
            #region cboNo
            cboNo = new ComboBoxLookup
            {
                DataIndex = "No",
                Name = "No",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "No.",
                ID = "cboNo",
                SelectOnFocus = true,
                AllowBlank = false,
                MsgTarget = MessageTarget.Side,
                ProxyUrl = "../../../Handler/ItemListHandler.ashx",
                DataTemplete = {
                    new LookupFormatData{FieldName = "No", FieldTitle = "No.", ColWidth = 100},
                    new LookupFormatData{FieldName = "Description", FieldTitle = "Description", ColWidth = 250},
                    new LookupFormatData{FieldName = "Base_Unit_of_Measure", FieldTitle = "Base Unit of Measure", ColWidth = 150},
                    new LookupFormatData{FieldName = "Unit_Cost", FieldTitle = "Unit Cost", ColWidth = 120}
                },
                idProperty = "No",
                DisplayField = "No",
                ValueField = "No",
                PageSize = 100,
                CurCompany = GlobalVariable.CompanyName,
                SCOPE = "GLX.Lookup.ItemNo"
            };
            #endregion cboNo
            #region cboType
            cboType = new ComboBox
            {
                DataIndex = "Type",
                Name = "Type",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Type",
                ID = "cboType",
                SelectOnFocus = true,
                AllowBlank = false,
                ReadOnly = true,
                Items = {
                    new ListItem {Text="G/L Account", Value="G/L Account" },
                    new ListItem {Text="Item", Value="Item" },
                }
            };
            #endregion cboType
            #region cboAction_Message
            cboAction_Message = new ComboBox { DataIndex = "Action_Message", Name = "Action_Message", LabelWidth = 150, Anchor = "100%", FieldLabel = "Action Message", ID = "cboAction_Message", SelectOnFocus = true, ReadOnly = true,
                //Items = {
                //    new ListItem {Text="", Value="" },
                //    new ListItem {Text="New", Value="New" },
                //    new ListItem {Text="Change Qty.", Value="Change Qty." },
                //    new ListItem {Text="Reschedule", Value="Reschedule" },
                //    new ListItem {Text="Resched. & Chg. Qty.", Value="Resched. & Chg. Qty." },
                //    new ListItem {Text="Cancel", Value="Cancel" },
                //}
                Store =
                {
                    new Store { Data = ActionMessage, AutoDataBind = true,
                        Model = {
                            new Model {
                                Fields = {
                                    new ModelField{Name = "value"},
                                    new ModelField{Name = "text"},
                                    new ModelField{Name = "displaytext"}
                                }
                            }
                        }
                    }
                }
            };
            #endregion cboAction_Message
            #region Accept Action Message and Description
            cboAccept_Action_Message = new Checkbox
            {
                DataIndex = "Accept_Action_Message",
                Name = "Accept_Action_Message",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Accept Action Message",
                ID = "cboAccept_Action_Message",
                ReadOnly = true
            };
            txtDescription = new TextField
            {
                DataIndex = "Description",
                Name = "Description",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Description",
                ID = "txtDescription",
                ReadOnly = true
            };
            txtRemark = new TextField
            {
                DataIndex = "Remark",
                Name = "Remark",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Remark",
                ID = "txtRemark",
                ReadOnly = false
            };
            #endregion Accept Action Message and Description
            #region cboLocation_Code
            cboLocation_Code = new ComboBoxLookup
            {
                DataIndex = "Location_Code",
                Name = "Location_Code",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Location Code",
                ID = "cboLocation_Code",
                SelectOnFocus = true,
                AllowBlank = true,
                MsgTarget = MessageTarget.Side,
                ProxyUrl = "../../../Handler/LocationListHandler.ashx",
                DataTemplete = {
                    new LookupFormatData{FieldName = "Code", FieldTitle = "Code", ColWidth = 100},
                    new LookupFormatData{FieldName = "Name", FieldTitle = "Name", ColWidth = 150}
                },
                idProperty = "Code",
                DisplayField = "Code",
                ValueField = "Code",
                PageSize = 100,
                CurCompany = GlobalVariable.CompanyName,
                SCOPE = "GLX.Lookup.Location"
            };
            #endregion cboLocation_Code
            #region Original_Quantity and Quantity
            txtOriginal_Quantity = new NumberField
            {
                DataIndex = "Original_Quantity",
                Name = "Original_Quantity",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Original Quantity",
                ID = "txtOriginal_Quantity",
                MinValue = 0,
                MaxValue = 1000000,
                ReadOnly = true
            }; //1.000.000
            txtQuantity = new TextField
            {
                DataIndex = "Quantity",
                Name = "Quantity",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Quantity",
                ID = "txtQuantity",
                MaskRe = @"/[0-9\.]/",
            };
            #endregion Original_Quantity and Quantity
            #region cboUnit_of_Measure_Code
            cboUnit_of_Measure_Code = new ComboBoxLookup
            {
                DataIndex = "Unit_of_Measure_Code",
                Name = "Unit_of_Measure_Code",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Unit of Measure Code",
                ID = "cboUnit_of_Measure_Code",
                SelectOnFocus = true,
                AllowBlank = false,
                MsgTarget = MessageTarget.Side,
                ProxyUrl = "../../../Handler/UOMHandler.ashx",
                DataTemplete = {
                    new LookupFormatData{FieldName = "Code", FieldTitle = "Code", ColWidth = 100},
                    new LookupFormatData{FieldName = "Description", FieldTitle = "Description", ColWidth = 150},
                    new LookupFormatData{FieldName = "Qty_per_Unit_of_Measure", FieldTitle = "Qty per Unit of Measure", ColWidth = 150}

                },
                idProperty = "Code",
                DisplayField = "Code",
                ValueField = "Code",
                PageSize = 100,
                CurCompany = GlobalVariable.CompanyName,
                SCOPE = "GLX.Lookup.UOM"
            };
            #endregion txtUnit_of_Measure_Code
            #region Direct_Unit_Cost and Due_Date
            txtDirect_Unit_Cost = new TextField
            {
                DataIndex = "Direct_Unit_Cost",
                Name = "txtDirect_Unit_Cost",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Direct Unit Cost",
                ID = "txtDirect_Unit_Cost",
                ReadOnly = true
            };
            txtDue_Date = new DateField
            {
                DataIndex = "Due_Date",
                Name = "Due_Date",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Due Date",
                ID = "txtDue_Date",
                Format = "dd/MM/yyyy",
                SubmitFormat = "dd/MM/yyyy"
            };
            #endregion Direct_Unit_Cost and Due_Date
            #region cboVendor_No
            cboVendor_No = new ComboBoxLookup { DataIndex = "Vendor_No", Name = "Vendor_No", LabelWidth = 150, Anchor = "100%", FieldLabel = "Vendor No.", ID = "cboVendor_No", ReadOnly = true,
                AllowBlank = true,
                MsgTarget = MessageTarget.Side,
                ProxyUrl = "../../../Handler/ItemVendorCatalogHandler.ashx",
                DataTemplete = {
                    new LookupFormatData{FieldName = "Vendor_No", FieldTitle = "Vendor No", ColWidth = 100},
                    new LookupFormatData{FieldName = "Vendor_Item_No", FieldTitle = "Vendor Item No.", ColWidth = 100},
                    new LookupFormatData{FieldName = "Lead_Time_Calculation", FieldTitle = "Lead Time Calculation", ColWidth = 150}
                },
                idProperty = "Vendor_No",
                DisplayField = "Vendor_No",
                ValueField = "Vendor_No",
                PageSize = 100,
                CurCompany = GlobalVariable.CompanyName,
                SCOPE = "GLX.Lookup.ItemVendorCatalog"
            };
            #endregion cboVendor_No
            #region cboUOMHandler
            //cboUOMHandler = new ComboBoxLookup {
            //    DataIndex = "Unit_of_Measure_Code",
            //    Name = "Unit_of_Measure_Code",
            //    LabelWidth = 150,
            //    Anchor = "100%",
            //    FieldLabel = "UOM",
            //    ID = "cboUOMHandler",
            //    ReadOnly = false,
            //    AllowBlank = false,
            //    MsgTarget = MessageTarget.Side,
            //    ProxyUrl = "../../../Handler/UOMHandler.ashx",
            //    DataTemplete = {
            //        new LookupFormatData{FieldName = "Code", FieldTitle = "Code", ColWidth = 100},
            //        new LookupFormatData{FieldName = "Description", FieldTitle = "Description", ColWidth = 100},
            //        new LookupFormatData{FieldName = "Qty_per_Unit_of_Measure", FieldTitle = "Qty per Unit of Measure", ColWidth = 200, Format="N0"},
            //    },
            //    idProperty = "Unit_of_Measure_Code",
            //    DisplayField = "Unit_of_Measure_Code",
            //    ValueField = "Unit_of_Measure_Code",
            //    PageSize = 5,
            //    CurCompany = GlobalVariable.CompanyName,
            //    SCOPE = "GLX.Lookup.UOM"
            //};
            //cboUOMHandler.Store.Primary.Parameters.AddRange(new Ext.Net.StoreParameter[] {
            //    new Ext.Net.StoreParameter{Name = "type", Value = String.Format("2|3"), Mode = ParameterMode.Value},
            //});
            #endregion cboUOMHandler
            #region Vendor_Item_No
            txtVendor_Item_No = new TextField
            {
                DataIndex = "Vendor_Item_No",
                Name = "Vendor_Item_No",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Vendor Item No",
                ID = "txtVendor_Item_No",
                ReadOnly = true
            };
            #endregion Vendor_Item_No
            #region cboReplenishment_System
            cboReplenishment_System = new ComboBox
            {
                DataIndex = "Replenishment_System",
                Name = "Replenishment_System",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "Replenishment System",
                ID = "cboReplenishment_System",
                ReadOnly = true,
                Store =
                {
                    new Store { Data = ReplenishmentSystem, AutoDataBind = true,
                        Model = {
                            new Model {
                                Fields = {
                                    new ModelField{Name = "value"},
                                    new ModelField{Name = "text"},
                                    new ModelField{Name = "displaytext"}
                                }
                            }
                        }
                    }
                }
                //Items = {
                //    new ListItem { Text="Purchase", Value="Purchase"},
                //    new ListItem { Text="Prod. Order", Value="Prod. Order"},
                //    new ListItem { Text="Transfer", Value="Transfer"},
                //    new ListItem { Text="Assembly", Value="Assembly"}
                //}
            };
            #endregion cboReplenishment_System
            #region cboNo
            cboNo = new ComboBoxLookup
            {
                DataIndex = "No",
                Name = "No",
                LabelWidth = 150,
                Anchor = "100%",
                FieldLabel = "No.",
                ID = "cboNo",
                SelectOnFocus = true,
                AllowBlank = false,
                MsgTarget = MessageTarget.Side,
                ProxyUrl = "../../../Handler/ItemListHandler.ashx",
                DataTemplete = {
                    new LookupFormatData{FieldName = "No", FieldTitle = "No.", ColWidth = 100},
                    new LookupFormatData{FieldName = "Description", FieldTitle = "Description", ColWidth = 250},
                    new LookupFormatData{FieldName = "Base_Unit_of_Measure", FieldTitle = "Base Unit of Measure", ColWidth = 150},
                    new LookupFormatData{FieldName = "Unit_Cost", FieldTitle = "Unit Cost", ColWidth = 120}
                },
                idProperty = "No",
                DisplayField = "No",
                ValueField = "No",
                PageSize = 100,
                CurCompany = GlobalVariable.CompanyName,
                SCOPE = "GLX.Lookup.ItemNo"
            };
            #endregion cboNo


            TextAreaChangeLogComment = new TextArea { Flex = 1, ID = "TextAreaChangeLogComment", ReadOnly = true };
            hiddenConverted = new Ext.Net.Hidden { DataIndex = "Converted", Name = "Converted", ID = "hiddenConverted" };
            hiddenStatus = new Ext.Net.Hidden { DataIndex = "Status", Name = "Status", ID = "hiddenStatus" };

            #region frmHeader
            frmHeader = new FormPanel
            {
                Header = false,
                Region = Region.North,
                Icon = Ext.Net.Icon.ApplicationForm,
                Border = true,
                ID = "frmHeader",
                TrackResetOnLoad = true,
                Layout = "Hbox",
                Items =
                {
                    new Panel {
                        Layout = "Anchor",
                        Flex = 1,
                        BodyPaddingSummary = "10 10 10 10",
                        Border = false,
                        //Items = { txtKey, txtBatch, cboType, cboNo, cboAction_Message, cboAccept_Action_Message, txtDescription, cboLocation_Code } //txtOriginal_Quantity 
                        Items = { txtKey, txtBatch, hiddenConverted, hiddenStatus, cboNo, txtDescription, txtRemark, 
                            cboUnit_of_Measure_Code, txtDirect_Unit_Cost, cboLocation_Code, txtQuantity, txtDue_Date} //txtOriginal_Quantity, cboType, cboAction_Message, cboAccept_Action_Message, 

                    },
                }
            };
            frmComment = new FormPanel
            {
                ID = "frmComment",
                Title = "Comment",
                Icon = Icon.Comments,
                Border = false,
                Region = Region.South,
                Layout = "Fit",
                Height = 150,
                Items = { TextAreaChangeLogComment }
            };
            
            #endregion frmHeader

            #region Windows            
            this.ID = "winCard";
            this.Maximizable = false;
            this.Minimizable = false;
            this.CloseAction = CloseAction.Destroy;
            this.Icon = Icon.ApplicationEdit;
            this.TopBar.Add(topBar);
            this.Layout = "Border";
            this.Items.AddRange(
                new ItemsCollection<Ext.Net.AbstractComponent>
                {
                    this.frmHeader,frmComment
                }
            );
            #endregion Windows
        }

        private void InitLogis() {
            txtQuantity.Listeners.Focus.Fn = SCOPE + ".QuantityFocus";
            txtQuantity.Listeners.Blur.Fn = SCOPE + ".QuantityBlur";
            txtDirect_Unit_Cost.Listeners.Change.Fn = SCOPE + ".Direct_Unit_CostChange";

            btnSave.Listeners.Click.Handler = SCOPE + ".Save();";
            btnDeleteCard.Listeners.Click.Handler = SCOPE + ".Delete();";
            this.Listeners.BeforeClose.Fn = SCOPE + ".BeforeCloseWindow";
            cboNo.Listeners.Change.Handler = SCOPE + ".NoChange();";
        }

        public void BindingData()
        {
            if (Mode == "edit") {
                /*
                GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service svc = new GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service();
                svc.Url = common.ReBuildUrl(svc.Url, "");
                svc.Credentials = common.CheckCredentials();
                string recId = svc.GetRecIdFromKey(strKey);
                GLXNAVWebApp.ReqWorksheet.ReqWorksheet results = svc.ReadByRecId(JournalBatchName, recId);
                frmHeader.SetValues(results);
                //txtQuantity.Text = results.Quantity.ToString("N2", CultureInfo.InstalledUICulture);
                txtBatch.SetValue(JournalBatchName);
                */

                POWaitingForApprove.BSL_POWaitingForApproveClient client = new POWaitingForApprove.BSL_POWaitingForApproveClient();
                POWaitingForApprove.data_POWaitingForApprove rec = client.GetRecIdFromKey(strKey);
                frmHeader.SetValues(rec);
                TextAreaChangeLogComment.Text = rec.Comment;
            }
            else {
                hiddenConverted.SetValue("false");
                hiddenStatus.SetValue("Open");
                txtDue_Date.SetValue(DateTime.Now);
                cboType.SetValue("Item");
                cboAction_Message.SetValue("New");
                cboAccept_Action_Message.Checked = true;
                txtOriginal_Quantity.SetValue(0);
                txtQuantity.SetValue(0);
                txtDirect_Unit_Cost.SetValue(0);
                cboReplenishment_System.SetValue("Purchase");
                txtBatch.SetValue(JournalBatchName);
            }
        }

        public void LoadUOMContext(string itemNo)
        {
            //ItemUnitsofMeasure.ItemUnitsofMeasure_Service s = new ItemUnitsofMeasure.ItemUnitsofMeasure_Service();
            //s.Url = common.ReBuildUrl(s.Url, GlobalVariable.CompanyName);
            //s.Credentials = common.CheckCredentials();
            ////Filter Item No.
            //List<ItemUnitsofMeasure.ItemUnitsofMeasure_Filter> DataFilter = new List<ItemUnitsofMeasure.ItemUnitsofMeasure_Filter>();
            //ItemUnitsofMeasure.ItemUnitsofMeasure_Filter TypeFilter = new ItemUnitsofMeasure.ItemUnitsofMeasure_Filter();
            //TypeFilter.Field = ItemUnitsofMeasure.ItemUnitsofMeasure_Fields.Item_No;
            //TypeFilter.Criteria = itemNo;
            //DataFilter.Add(TypeFilter);
            //try
            //{
            //    ItemUnitsofMeasure.ItemUnitsofMeasure[] Results = s.ReadMultiple(DataFilter.ToArray(), "", 10);
            //    strUnit_of_Measure.DataSource = Results;
            //    strUnit_of_Measure.DataBind();
            //}
            //catch (Exception e)
            //{
            //    string mes = e.Message;
            //    throw;
            //}
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                this.ResourceManager.AddDirectMethodControl(this);
            }
            base.OnLoad(e);
        }
    }
}