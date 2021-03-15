using Ext.Net;
using GLXNAVWebApp.Classes;
using GLXNAVWebApp.POWaitingForApprove;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.Xsl;
using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace GLXNAVWebApp.Forms.ReqWorksheet
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public class CtlReqWorksheet : Viewport
    {
        #region Variables
        public const int fetchSize = 10;
        public string bookmarkKey = null;
        public const string SCOPE = "GLX.FORMS.ReqWorksheet";
        string currentpage = "ReqWorksheet";
        Common common = new Common();

        Ext.Net.GridPanel grdItems;
        Ext.Net.Store strItems;
        Ext.Net.Store strBatchs;
        Ext.Net.SelectBox cboBatchs;
        Ext.Net.Button btnAddItem;
        Ext.Net.Button btnAddComment;
        Ext.Net.Button btnChangeLog;

        Ext.Net.SplitButton btnExport;
        MenuItem btnToExcel;
        MenuItem btnToCSV;

        Ext.Net.SplitButton btnFnB;
        MenuItem miFnB_Approve;
        MenuItem miFnB_Reopen;

        Ext.Net.SplitButton btnCM;
        MenuItem miCM_Approve;
        MenuItem miCM_Reopen;

        Ext.Net.SplitButton btnNAV;
        MenuItem miNav_SplitRequest;
        MenuItem miNAV_ConvertToRequest;
        MenuItem miNAV_ConvertToItemReclassJournal;

        Ext.Net.DateField dfFromDate;
        Ext.Net.DateField dfToDate;

        Ext.Net.Hidden hdFormatType;
        Ext.Net.SelectBox cboStatus;
        #endregion

        public CtlReqWorksheet()
        {
            InitComponent();
            InitLogis();
            //BindingDataWithFilter();
        }

        public void InitComponent()
        {

            #region Button
            btnAddItem = new Button { Icon = Icon.ImageAdd, Text = "Add", };
            btnChangeLog = new Button { Icon = Icon.Time, Text = "Show Log" };
            btnAddComment = new Button { Icon = Icon.Comment, Text = "Comment" };

            miFnB_Approve = new MenuItem { Text = "F&B Approve", Icon = Icon.ArrowMerge };
            miFnB_Reopen = new MenuItem { Text = "F&B Reopen", Icon = Icon.GroupEdit };
            btnFnB = new SplitButton
            {
                Text = "F&B",
                Icon = Icon.Group,
                Menu = { new Menu { Items = {
                    miFnB_Approve, miFnB_Reopen
                    } } }
            };

            miCM_Approve = new MenuItem { Text = "CM Approve", Icon = Icon.ArrowIn };
            miCM_Reopen = new MenuItem { Text = "CM Reopen", Icon = Icon.GroupEdit };
            btnCM = new SplitButton
            {
                Text = "CM",
                Icon = Icon.Group,
                Menu = { new Menu { Items = { miCM_Approve, miCM_Reopen } } }
            };

            miNav_SplitRequest = new MenuItem { Text = "split", Icon = Icon.ApplicationSplit };
            miNAV_ConvertToRequest = new MenuItem { Text = "Convert To Request", Icon = Icon.ArrowIn };
            miNAV_ConvertToItemReclassJournal = new MenuItem { Text = "Convert To Item Reclass Journal", Icon = Icon.ArrowIn };
            btnNAV = new SplitButton
            {
                Text = "NAV",
                Icon = Icon.Group,
                Menu = { new Menu { Items = { miNav_SplitRequest, miNAV_ConvertToRequest, miNAV_ConvertToItemReclassJournal } } }
            };

            btnToExcel = new MenuItem { Text = "To Excel", Icon = Icon.PageExcel };
            btnToCSV = new MenuItem { Text = "To CSV", Icon = Icon.PageAttach };
            btnExport = new Ext.Net.SplitButton
            {
                Text = "Export",
                Icon = Icon.PageCode,
                Menu = { new Menu { Items = { btnToExcel, btnToCSV } } }

            };

            #endregion Form

            #region Date
            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime FromDate = DateTime.Now.AddDays(-1 * daysInMonth);
            dfFromDate = new DateField
            {
                LabelWidth = 75,
                FieldLabel = "Created on",
                Width = 185,
                SelectOnFocus = true,
                SelectedDate = FromDate,
                ID = "dfFromDate",
                EnableKeyEvents = true,
                Vtype = "daterange",
                CustomConfig = {
                    new ConfigItem{Name = "endDateField", Value= "dfToDate", Mode = ParameterMode.Value }
                }
            };
            dfToDate = new DateField
            {
                Width = 100,
                SelectOnFocus = true,
                SelectedDate = DateTime.Now,
                ID = "dfToDate",
                Vtype = "daterange",
                EnableKeyEvents = true,
                CustomConfig = {
                    new ConfigItem {Name = "startDateField", Value = "dfFromDate", Mode = ParameterMode.Value}
                }
            };

            hdFormatType = new Hidden { ID = "FormatType" };
            #endregion

            #region Grid + Batch
            strBatchs = new Store
            {
                ID = "strBatchs",
                AutoDataBind = true,
                Model = {
                    new Model {
                        Fields = {
                            new ModelField { Name = "UID" },
                            new ModelField { Name = "Worksheet_Template_Name" },
                            new ModelField { Name = "Name" },
                            new ModelField { Name = "Description" },
                            new ModelField { Name = "Template_Type" },
                            new ModelField { Name = "Recurring", Type = ModelFieldType.Boolean },
                            new ModelField { Name = "Company" }
                        }
                    }
                }
            };
            cboBatchs = new SelectBox
            {
                ID = "cboBatchs",
                DisplayField = "Name",
                ValueField = "Name",
                EmptyText = "Select a batch...",
                Width = 125
            };
            cboBatchs.Store.Add(strBatchs);

            cboStatus = new SelectBox
            {
                ID = "cboStatus",
                DisplayField = "Status",
                ValueField = "Status",
                SelectedItems = { new ListItem { Index = 0} },
                Width = 125,
                Items = {
                    new ListItem { Text = "Open", Value = "1", Mode = ParameterMode.Raw},
                    new ListItem { Text = "Approved By CM", Value = "2", Mode = ParameterMode.Raw},
                    new ListItem { Text = "Approved By F&B", Value = "3", Mode = ParameterMode.Raw},
                    new ListItem { Text = "Completed", Value = "4", Mode = ParameterMode.Raw},
                    new ListItem { Text = "To be Deleted", Value = "5", Mode = ParameterMode.Raw},
                }
            };

            strItems = new Store
            {
                ID = "strItems",
                Model = {
                    new Model {
                        IDProperty = "Key",
                        Fields = {
                            new ModelField { Name = "UID" },
                            new ModelField { Name = "Batch" },
                            new ModelField { Name = "No" },
                            new ModelField { Name = "Type" },
                            new ModelField { Name = "Action_Message" },
                            new ModelField { Name = "Accept_Action_Message" },
                            new ModelField { Name = "Description" },
                            new ModelField { Name = "Remark" },
                            new ModelField { Name = "Location_Code" },
                            new ModelField { Name = "Original_Quantity" },
                            new ModelField { Name = "Quantity" },
                            new ModelField { Name = "Unit_of_Measure_Code" },
                            new ModelField { Name = "Direct_Unit_Cost" },
                            new ModelField { Name = "Due_Date" },
                            new ModelField { Name = "Vendor_No" },
                            new ModelField { Name = "Vendor_Item_No" },
                            new ModelField { Name = "Replenishment_System" },
                            new ModelField { Name = "Status" },
                            new ModelField { Name = "Comment" },
                            new ModelField { Name = "ConvertType" },
                            new ModelField { Name = "Converted" },
                            new ModelField { Name = "CreatedBy" },
                            new ModelField { Name = "CreatedOn" },
                            new ModelField { Name = "UpdatedBy" },
                            new ModelField { Name = "UpdatedOn" },
                            new ModelField { Name = "ApprovedBy" },
                            new ModelField { Name = "ApprovedOn" },
                            new ModelField { Name = "ChangeLog" }
                        }
                    }
                },
                //Sorters = {
                //    new DataSorter { Property="Converted", Direction = SortDirection.DESC },
                //    new DataSorter { Property="Due_Date", Direction = SortDirection.DESC }
                //}
            };
            grdItems = new GridPanel
            {
                ID = "grdItems",
                BodyCls = "line-body-border",
                CtCls = "line-border",
                ColumnModel =
                {
                    Columns = {
                        //new Column{ DataIndex = "UID", Text = "UID", Width = 100 },
                        new Column{ DataIndex = "No", Text = "No", Width = 150 },
                        new Column{ DataIndex = "Status", Text = "Status", Width = 150 },
                        new Column{ DataIndex = "Converted", Text = "Converted", Width = 100 },
                        new Column{ DataIndex = "ConvertType", Text = "Convert Type", Width = 120 },
                        new Column{ DataIndex = "Description", Text = "Description", Width = 200, Flex = 1, MinWidth = 200 },
                        new Column{ DataIndex = "Remark", Text = "Remark", Width = 150 },
                        new Column{ DataIndex = "Location_Code", Text = "Location Code", Width = 120 },
                        new NumberColumn{ DataIndex = "Quantity", Text = "Quantity", Width = 120, Format = "0,000.00", Align = Alignment.Right },
                        new Column{ DataIndex = "Unit_of_Measure_Code", Text = "UOM Code", Width = 100 },
                        new NumberColumn{ DataIndex = "Direct_Unit_Cost", Text = "Avg. Cost", Width = 140, Format = "0,000.00", Align = Alignment.Right },
                        new DateColumn{ DataIndex = "Due_Date", Text = "Due Date", Width = 120, Format = "dd/MM/yyyy", Align = Alignment.Left},
                        new DateColumn{ DataIndex = "CreatedOn", Text = "Created On", Width = 120, Format = "dd/MM/yyyy", Align = Alignment.Left },
                        new Column{ DataIndex = "CreatedBy", Text = "Created By", Width = 100 }
                    }
                },
                TopBar = {
                    new Toolbar {
                        Items = {
                            hdFormatType,
                            cboBatchs, new ToolbarSeparator(), dfFromDate, dfToDate,
                            new ToolbarSeparator(), cboStatus,
                            new ToolbarFill(), btnAddItem, btnAddComment,
                            new ToolbarSeparator(), btnCM,
                            new ToolbarSeparator(), btnFnB, btnNAV,
                            new ToolbarSeparator(), btnChangeLog,
                            btnExport
                        }
                    }
                },
                Plugins = { new FilterHeader { } },
                BottomBar = { new PagingToolbar { HideRefresh = true } },
                SelectionModel = {
                    new RowSelectionModel { 
                        //CheckboxSelectionModel
                        Mode = SelectionMode.Multi
                    }
                },

            };
            grdItems.Store.Add(strItems);

            ModelField StatusType = grdItems.Store[0].Model[0].Fields.FirstOrDefault(i => i.Name == "Action_Message");
            StatusType.Convert.Fn = SCOPE + ".Convert_ActionMessage_List";

            ModelField ReplenishmentSystem = grdItems.Store[0].Model[0].Fields.FirstOrDefault(i => i.Name == "Replenishment_System");
            ReplenishmentSystem.Convert.Fn = SCOPE + ".Convert_ReplenishmentSystem_List";
            #endregion

            #region Panel
            Panel center = new Panel
            {
                Layout = "Fit",
                Region = Region.Center,
                Items = { grdItems }
            };
            Panel pnlLeft = new Panel
            {
                Width = 250,
                Title = "Menu",
                Collapsible = true,
                Collapsed = true,
                Region = Region.West,
                Layout = "VBoxLayout",
                Items =
                {
                     new Ext.Net.HyperLink {
                         Width = 250,
                        Icon = Icon.Accept,
                        Target = "_blank",
                        NavigateUrl = "../../login.aspx",
                        Text = "Relogin"
                    },

                    new Ext.Net.HyperLink {
                        Width = 250,
                        Icon = Icon.PackageGo,
                        Target = "_blank",
                        NavigateUrl = "../PurchaseOrder",
                        Text = "Purchase Order"
                    },

                    //new Ext.Net.HyperLink {
                    //    Width = 250,
                    //    Icon = Icon.PackageGo,
                    //    Target = "_blank",
                    //    NavigateUrl = "../ReqWorksheet2",
                    //    Text = "Req. Worksheets 2"
                    //}
                }
            };
            #endregion

            #region Viewport
            this.ID = "pageMain";
            this.Layout = "BorderLayout";
            this.Items.AddRange(new ItemsCollection<Ext.Net.AbstractComponent> {
                pnlLeft, center
            });
            #endregion 
        }

        private void InitLogis()
        {
            btnAddItem.Listeners.Click.Handler = SCOPE + ".AddNew();";

            //CM
            btnCM.Listeners.Click.Handler = SCOPE + ".CMApprove();";
            miCM_Approve.Listeners.Click.Handler = SCOPE + ".CMApprove();";
            miCM_Reopen.Listeners.Click.Handler = SCOPE + ".CMReopen();";

            //FnB
            btnFnB.Listeners.Click.Handler = SCOPE + ".FnBApprove();";
            miFnB_Approve.Listeners.Click.Handler = SCOPE + ".FnBApprove();";
            miFnB_Reopen.Listeners.Click.Handler = SCOPE + ".FnBReopen();";

            //NAV
            btnNAV.Listeners.Click.Handler = SCOPE + ".ConvertToNAV();";
            miNav_SplitRequest.Listeners.Click.Handler = SCOPE + ".SplitRequest();";
            miNAV_ConvertToRequest.Listeners.Click.Handler = SCOPE + ".ConvertToNAV();";
            miNAV_ConvertToItemReclassJournal.Listeners.Click.Handler = SCOPE + ".ConvertToNAVItemReclassJournal();";

            btnChangeLog.Listeners.Click.Handler = SCOPE + ".ShowLog();";
            btnAddComment.Listeners.Click.Handler = SCOPE + ".AddComment();";

            //btnExportExcel.Listeners.Click.Handler = SCOPE + ".ExportExcel();";
            btnExport.Listeners.Click.Handler   = SCOPE + ".submitValue(#{grdItems}, #{FormatType}, 'xls');";
            btnToExcel.Listeners.Click.Handler  = SCOPE + ".submitValue(#{grdItems}, #{FormatType}, 'xls');";
            btnToCSV.Listeners.Click.Handler    = SCOPE + ".submitValue(#{grdItems}, #{FormatType}, 'csv');";

            dfFromDate.Listeners.KeyUp.Fn = SCOPE + ".OnKeyUp";
            dfToDate.Listeners.KeyUp.Fn = SCOPE + ".OnKeyUp";
            dfToDate.Listeners.Focus.Fn = SCOPE + ".OnKeyUp";
            dfFromDate.Listeners.Focus.Fn = SCOPE + ".OnKeyUp";

            dfFromDate.Listeners.Change.Fn = SCOPE + ".DateChanged";
            dfToDate.Listeners.Change.Fn = SCOPE + ".DateChanged";
            cboBatchs.Listeners.Change.Fn = SCOPE + ".cboBatchsChange";
            cboStatus.Listeners.Change.Fn = SCOPE + ".cboStatusChange";

            //Open Edit Windows
            grdItems.Listeners.ItemDblClick.Handler = SCOPE + ".grdItemsDblClick();";

            strItems.SubmitData += StrItems_SubmitData;
        }

        private void StrItems_SubmitData(object sender, StoreSubmitDataEventArgs e)
        {
            string format = this.hdFormatType.Value.ToString();

            XmlNode xml = e.Xml;

            System.Web.HttpContext.Current.Response.Clear();

            switch (format)
            {
                case "xml":
                    string strXml = xml.OuterXml;
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Req.xml");
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Length", strXml.Length.ToString());
                    System.Web.HttpContext.Current.Response.ContentType = "application/xml";
                    System.Web.HttpContext.Current.Response.Write(strXml);
                    break;

                case "xls":
                    System.Web.HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Req.xls");
                    XslCompiledTransform xtExcel = new XslCompiledTransform();
                    xtExcel.Load(HttpContext.Current.Server.MapPath("~/bin/export/Excel.xsl"));
                    xtExcel.Transform(xml, null, System.Web.HttpContext.Current.Response.OutputStream);
                    break;

                case "csv":
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=Req.csv");
                    XslCompiledTransform xtCsv = new XslCompiledTransform();
                    xtCsv.Load(HttpContext.Current.Server.MapPath("~/bin/export/Csv.xsl"));
                    xtCsv.Transform(xml, null, System.Web.HttpContext.Current.Response.OutputStream);
                    break;
            }
            System.Web.HttpContext.Current.Response.End();
        }

        public void BindDataComboBox()
        {
            using (GLXRequisitionWkshName.BSL_RequisitionWkshNameClient client = new GLXRequisitionWkshName.BSL_RequisitionWkshNameClient())
            {
                string comp = GlobalVariable.CompanyName;
                string UserName = GlobalVariable.UserName;
                IList<GLXRequisitionWkshName.data_RequisitionWkshName> BatchList = client.GetAll(comp, UserName);
                strBatchs.DataSource = BatchList;
                strBatchs.DataBind();
            }
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void BindingDataWithFilter(string batch, DateTime fromdate, DateTime todate, string status)
        {
            todate = todate.AddDays(1);
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            data_POWaitingForApprove[] ReqWorksheetList = client.GetAll(batch, fromdate, todate, GlobalVariable.UserName);
            List<data_POWaitingForApprove> data2 = null;
           
            switch (status)
            {
                case "1": //Open
                    data2 = ReqWorksheetList.Where(p => p.Status == "Open").ToList();
                    break;
                case "2": //Approved By CM
                    data2 = ReqWorksheetList.Where(p => p.Status == "Approved By CM").ToList();
                    break;
                case "3": //Approved By F&B
                    data2 = ReqWorksheetList.Where(p => p.Status == "Approved By F&B").ToList();
                    break;
                case "4": //Completed
                    data2 = ReqWorksheetList.Where(p => p.Status == "Completed").ToList();
                    break;
                case "5": //To be Deleted
                    data2 = ReqWorksheetList.Where(p => p.Status == "To be Deleted").ToList();
                    break;
                default:
                    data2 = ReqWorksheetList.ToList();
                    break;
            }
            strItems.DataSource = data2;
            strItems.DataBind();
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void LoadCard(string Mode, string data)
        {
            string key = string.Empty;
            string converted = string.Empty;
            string batch = string.Empty;
            string status = string.Empty;

            if (Mode == "new")
            {
                batch = data;
            }
            else
            {
                Dictionary<string, object> jsonData = JSON.Deserialize<Dictionary<string, object>>(data);
                foreach (KeyValuePair<string, object> item in jsonData)
                {
                    string k = item.Key;
                    string v = item.Value == null ? "" : item.Value.ToString();

                    if (k == "UID") key = v;
                    if (k == "Converted") converted = v;
                    if (k == "Status") status = v;
                    if (k == "Batch") batch = v;
                }
            }

            CtlReqWorksheetEdit winCard = new CtlReqWorksheetEdit(Mode, key, converted, batch, status);
            int h = (status == "Open" || status == string.Empty) ? 490 : 455;
            winCard.Title = "Requisition Worksheet";
            winCard.Width = 750;
            winCard.Height = h;
            winCard.Render();
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void GLXSetCookie(string itemNo)
        {
            HttpContext.Current.Response.Cookies["ItemNo"].Value = itemNo;
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void LoadUOMContext(string itemNo)
        {
            Context.Response.AddHeader("itemNo", itemNo);
        }

        //Update done
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse Approve(string keys, string status)
        {
            DirectResponse dr = new DirectResponse();
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            string action = string.Empty;
            string[] arrKey = keys.Split(';');

            #region Check Permission
            if (status == "1")
            {
                status = "Approved By CM";
                action = "CMApprove";
            }
            else
            {
                status = "Approved By F&B";
                action = "FnBApprove";
            }

            string msg = "You do not have permission to do this action.";
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
            {
                dr.Success = true;
                dr.Result = String.Format("Message:'{0}'", msg);
                return dr;
            }
            #endregion

            try
            {
                string result = "";
                foreach (string key in arrKey)
                {
                    if (key == "") continue;
                    result += client.UpdateToApprove(key, GlobalVariable.UserName, status);
                    result += "<br>";
                }
                dr.Success = true;
                dr.Result = result;
                return dr;
            }
            catch (Exception ex)
            {
                dr.Success = false;
                dr.ErrorMessage = ex.Message;
                return dr;
            }
        }

        //Update done
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse Reopen(string keys, string type)
        {
            DirectResponse dr = new DirectResponse();
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            string[] arrKey = keys.Split(';');

            #region Check Permission
            string action = "Reopen_FnB";
            if (type == "1")
                action = "Reopen_CM";
            string msg = "You do not have permission to do this action.";
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
            {
                dr.Success = true;
                //dr.Result = String.Format("{{Message:'{0}'}}", msg);
                dr.Result = String.Format("Message:'{0}'", msg);
                return dr;
            }
            #endregion

            try
            {
                string result = "";
                foreach (string key in arrKey)
                {
                    if (key == "") continue;
                    result += client.Reopen(key, GlobalVariable.UserName);
                    result += "<br>";
                }
                dr.Success = true;
                //dr.Result = String.Format("{{Message:'{0}'}}", result);
                dr.Result = result;
                return dr;
            }
            catch (Exception ex)
            {
                dr.Success = false;
                dr.ErrorMessage = ex.Message;
                return dr;
            }
        }


        //Update done
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse TobeDeleted(string key)
        {
            DirectResponse dr = new DirectResponse();
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            #region Check Permission

            string action = "Delete";
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
            {
                dr.Success = true;
                //dr.Result = String.Format("{{Message:'{0}'}}", );
                dr.Result = String.Format("Message:'{0}'", "You do not have permission to do this action.");
                return dr;
            }
            #endregion

            try
            {
                string result = client.TobeDeleted(key, GlobalVariable.UserName, "To be Deleted");
                dr.Success = true;
                //dr.Result = String.Format("{{Message:'{0}'}}", result);
                //dr.Result = String.Format("Message:'{0}'", result);
                dr.Result = result;
                return dr;
            }
            catch (Exception ex)
            {
                dr.Success = false;
                dr.ErrorMessage = ex.Message;
                return dr;
            }
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse UpdateData2(string batch, data_POWaitingForApprove data)
        {
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            DirectResponse dr = new DirectResponse();

            #region Check Permission
            string action = "Insert";
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
            {
                dr.Success = true;
                //dr.Result = String.Format("{{Message:'{0}'}}", "You do not have permission to do this action.");
                dr.Result = String.Format("Message:'{0}'", "You do not have permission to do this action.");
                return dr;
            }
            #endregion

            try
            {
                string r = client.UpdateData(data.UID, data, GlobalVariable.UserName);
                dr.Success = true;
                if (r == "Object cannot be found")
                    dr.Result = String.Format("{{Message:'{0}', UID:'{1}'}}", r, r);
                else
                    dr.Result = String.Format("{{Message:'{0}', UID:'{1}'}}", "", r);
                dr.ErrorMessage = r;
                return dr;
            }
            catch (Exception ex)
            {
                dr.Success = false;
                dr.Result = String.Format("{{Message:'{0}'}}", ex.Message);
                return dr;
            }
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void SplitRequest(string uid)
        {
            Ext.Net.X.MessageBox.Show(new MessageBoxConfig { Icon = MessageBox.Icon.WARNING, Message = "You do not have permission to do this action.", Title = "Message" });
            //DirectResponse dr = new DirectResponse();
            //return dr;
        }

        //Update done
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse ConvertToNAV(string keys)
        {
            DirectResponse dr = new DirectResponse();
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            string[] arrKey = keys.Split(';');

            #region Check Permission
            if (GlobalVariable.CompanyName == null || GlobalVariable.CompanyName == "")
                HttpContext.Current.Response.Redirect("/login.aspx");

            string action = "ConvertToNAV";
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
            {
                dr.Success = true;
                dr.Result = String.Format("Message:'{0}'", "You do not have permission to do this action.");
                return dr;
                //dr.Result = String.Format("{{Message:'{0}'}}", "You do not have permission to do this action.");
            }
            #endregion

            try
            {
                string msg = string.Empty;
                Common Clsc = new Common();
                foreach (string key in arrKey)
                {
                    if (key == "") continue;
                    #region check something
                    data_POWaitingForApprove dt = client.GetRecIdFromKey(key);
                    if (dt.Status != "Approved By F&B")
                    {
                        dr.Success = true;
                        dr.Result = "Status must be [Approved By F&B].";
                        return dr;
                        //dr.Result = String.Format("{{Message:'{0}'}}", );
                        //dr.Result = String.Format("Message:'{0}'", "Status must be [Approved By F&B].");
                    }
                    #endregion check something

                    if (!string.IsNullOrWhiteSpace(dt.UID))
                    {
                        string ConvertType = "Req. Worksheet";
                        GLXNAVWebApp.ReqWorksheet_svc.ReqWorksheet_Service svc = new GLXNAVWebApp.ReqWorksheet_svc.ReqWorksheet_Service();
                        svc.Url = Clsc.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
                        svc.Credentials = Clsc.CheckCredentials();
                        GLXNAVWebApp.ReqWorksheet_svc.ReqWorksheet req = new GLXNAVWebApp.ReqWorksheet_svc.ReqWorksheet();

                        //default
                        req.TypeSpecified = true;
                        req.Type = GLXNAVWebApp.ReqWorksheet_svc.Type.Item;

                        req.Action_MessageSpecified = true;
                        req.Action_Message = GLXNAVWebApp.ReqWorksheet_svc.Action_Message.New;

                        req.Replenishment_System = GLXNAVWebApp.ReqWorksheet_svc.Replenishment_System.Purchase;
                        req.Accept_Action_Message = true;

                        //User input
                        req.No = dt.No;
                        req.Description = dt.Description;
                        req.Location_Code = dt.Location_Code;

                        req.QuantitySpecified = true;
                        req.Quantity = dt.Quantity == null ? 0 : (decimal)dt.Quantity;

                        req.Unit_of_Measure_Code = dt.Unit_of_Measure_Code;
                        req.Direct_Unit_Cost = dt.Direct_Unit_Cost == null ? 0 : (decimal)dt.Direct_Unit_Cost;

                        req.Due_DateSpecified = true;
                        req.Due_Date = (DateTime)dt.Due_Date;

                        req.Vietnamese_Description = dt.Remark;

                        //Convert to NAV
                        svc.Create(dt.Batch, ref req);

                        //Update status local DB
                        msg += client.UpdateConverted(dt.UID, ConvertType, GlobalVariable.UserName);
                        msg += "<br>";
                    }
                }
                dr.Success = true;
                dr.Result = msg;
                //dr.Result = String.Format("{{Message:'{0}'}}", msg);
                //dr.Result = String.Format("Message:'{0}'", msg);
            }
            catch (Exception ex)
            {
                dr.Success = false;
                dr.Result = String.Format("{{Message:'{0}'}}", ex.Message);
            }
            return dr;
        }

        //Update done 
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse ConvertToNAVItemReclassJournal(string keys)
        {
            DirectResponse dr = new DirectResponse();
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            string[] arrKey = keys.Split(';');

            #region Check Permission
            if (GlobalVariable.CompanyName == null || GlobalVariable.CompanyName == "")
                HttpContext.Current.Response.Redirect("/login.aspx");

            string action = "ConvertToNAV_ItemReclassJournal";
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
            {
                dr.Success = true;
                //dr.Result = String.Format("{{Message:'{0}'}}", "You do not have permission to do this action.");
                dr.Result = String.Format("Message:'{0}'", "You do not have permission to do this action.");
                return dr;
            }
            #endregion

            try
            {
                string msg = string.Empty;
                Common Clsc = new Common();
                foreach (string key in arrKey)
                {
                    if (key == "") continue;
                    #region check something
                    data_POWaitingForApprove dt = client.GetRecIdFromKey(key);
                    if (dt.Status != "Approved By F&B")
                    {
                        dr.Success = true;
                        dr.Result = "Status must be [Approved By F&B].";
                        return dr;
                        //dr.Result = String.Format("{{Message:'{0}'}}", "Status must be [Approved By F&B].");
                        //dr.Result = String.Format("Message:'{0}'", "Status must be [Approved By F&B]. ");
                    }
                    #endregion

                    if (!string.IsNullOrWhiteSpace(dt.UID))
                    {
                        string ConvertType = "Item Reclass Jnl.";
                        GLXNAVWebApp.Item_Reclass_Journal.Item_Reclass_Journal_Service svc = new GLXNAVWebApp.Item_Reclass_Journal.Item_Reclass_Journal_Service();
                        svc.Url = Clsc.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
                        svc.Credentials = Clsc.CheckCredentials();
                        GLXNAVWebApp.Item_Reclass_Journal.Item_Reclass_Journal IRJ = new GLXNAVWebApp.Item_Reclass_Journal.Item_Reclass_Journal();

                        IRJ.Posting_Date = DateTime.Now;
                        IRJ.Posting_DateSpecified = true;
                        IRJ.Item_No = dt.No;
                        IRJ.ItemDescription = dt.Description;
                        IRJ.Description = dt.Description;
                        IRJ.Vietnamese_Description = dt.Description;
                        IRJ.Location_Code = dt.Location_Code;
                        IRJ.New_Location_Code = dt.Location_Code;
                        IRJ.Quantity = dt.Quantity == null ? 0 : (decimal)dt.Quantity;
                        IRJ.QuantitySpecified = true;
                        IRJ.Unit_of_Measure_Code = dt.Unit_of_Measure_Code;

                        //Convert to NAV
                        svc.Create("DEFAULT", ref IRJ);

                        //Update status local DB
                        msg += client.UpdateConverted(dt.UID, ConvertType, GlobalVariable.UserName);
                        msg += "<br>";
                    }
                }
                dr.Success = true;
                dr.Result = msg;
                //dr.Result = String.Format("{{Message:'{0}'}}", msg);
                //dr.Result = String.Format("Message:'{0}'", msg);
            }
            catch (Exception ex)
            {
                dr.Success = false;
                dr.Result = String.Format("{{Message:'{0}'}}", ex.Message);
            }
            return dr;
        }

        //Update - done
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public string AddComment(string keys)
        {
            string[] arrKey = keys.Split(';');
            string uid = arrKey[arrKey.Length - 2]; //Lấy record cuối cùng được chọn

            string action = "AddComment";

            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
                Ext.Net.X.MessageBox.Show(new MessageBoxConfig { Icon = MessageBox.Icon.WARNING, Message = "You do not have permission to do this action.", Title = "Message" });
            else {
                Ext.Net.Window w;
                w = new Window
                {
                    Title = "Add Comment",
                    Icon = Icon.Comment,
                    CloseAction = CloseAction.Destroy,
                    Layout = "Border",
                    Items = {
                        new ItemsCollection<Ext.Net.AbstractComponent> {
                            new Panel {
                                Header = false,
                                Region = Region.Center,
                                Layout = "Fit",
                                Flex = 1,
                                Border = false,
                                Items = {
                                    new Hidden {
                                        Text = uid,
                                         ID = "Comment_HiddenUID"
                                    },
                                    new TextArea {
                                        Flex = 1,
                                        ID = "TextAreaChangeLog",
                                    }
                                },
                                TopBar = {
                                    new ToolbarCollection {
                                        new Toolbar {
                                            ID = "AddCommentToolbar",
                                            Items = {
                                                new Button{
                                                    ID = "Comment_btnAddComment",
                                                    Text ="Add",
                                                    Icon = Icon.BookAdd,
                                                    Handler = SCOPE + ".DoAddComment(); "
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
                w.Width = 600;
                w.Height = 200;
                w.Render();
            }
            return "";
        }

        //Update done
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public string DoAddComment(string uid, string cmtText)
        {
            POWaitingForApprove.BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            client.InsertComment(uid, cmtText, GlobalVariable.UserName);
            return "";
        }


        //Update done
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void ShowLog(string ChangeLog)
        {
            string action = "ShowLog";

            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            if (!client.CheckPermission(GlobalVariable.CompanyName, GlobalVariable.UserName, currentpage, action))
            {
                Ext.Net.X.MessageBox.Show(new MessageBoxConfig { Icon = MessageBox.Icon.WARNING, Message = "You do not have permission to do this action.", Title = "Message" });
            }
            else
            {
                Ext.Net.Window w;
                w = new Window
                {
                    CloseAction = CloseAction.Destroy,
                    Layout = "Border",
                    Items = {
                        new ItemsCollection<Ext.Net.AbstractComponent> {
                            new Panel {
                                Header = false,
                                Region = Region.Center,
                                Layout = "Fit",
                                Flex = 1,
                                Border = false,
                                Items = {
                                    new TextArea {
                                        Flex = 1,
                                        ID = "TextAreaChangeLog",
                                        Value = ChangeLog
                                    }
                                }
                            }
                        }
                    }
                };
                w.Width = 600;
                w.Height = 500;
                w.Render();
            }
        }


        //Update 
        [DirectMethod(Msg = "Loading....", Timeout = 720000)]
        public string ExportExcel(string batch, DateTime fromdate, DateTime todate)
        {
            todate = todate.AddDays(1);
            BSL_POWaitingForApproveClient client = new BSL_POWaitingForApproveClient();
            data_POWaitingForApprove[] ReqWorksheet = client.GetAll(batch, fromdate, todate, GlobalVariable.UserName);
            List<data_POWaitingForApprove> ReqWorksheetList = ReqWorksheet.ToList();

            string filename = Guid.NewGuid().ToString().Replace("-", "") + GlobalVariable.UserName + ".xlsx";
            string p3 = HttpContext.Current.Server.MapPath("~/bin/export");
            string customExcelSavingPath = Path.Combine(p3, filename);

            Classes.Utilities utilities = new Utilities();
            //utilities.GenerateExcel(utilities.ConvertToDataTable(ReqWorksheetList), customExcelSavingPath);
            utilities.WriteDataTableToExcel(utilities.ConvertToDataTable(ReqWorksheetList), "Req", customExcelSavingPath, "Details");


            downloadFile(customExcelSavingPath);
            return "";
        }

        [DirectMethod(ShowMask = true)]
        public void OpenLookupWindow(string title, string url)
        {
            Ext.Net.Window w = new Ext.Net.Window();
            w.ID = "LookupWindow" + Guid.NewGuid().ToString().Replace("-", "");
            w.Title = title;
            w.Modal = true;
            ComponentLoader cl = new ComponentLoader
            {
                Url = url,
                Mode = LoadMode.Frame,
                LoadMask = { ShowMask = true }
            };
            w.Loader = cl;
            w.Render();
        }

        public void downloadFile(string fullpath)
        {
            string monthYear = DateTime.Now.ToShortDateString().Replace(",", "-");
            string filename = monthYear + "_" + "ReqWorkSheet" + "_" + GlobalVariable.UserName + ".xlsx";

            System.Web.HttpContext.Current.Response.ClearContent();
            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
            System.Web.HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment; filename=" + filename + ";");
            System.Web.HttpContext.Current.Response.TransmitFile(fullpath);
            System.Web.HttpContext.Current.Response.Flush();
            System.Web.HttpContext.Current.Response.End();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!Ext.Net.X.IsAjaxRequest)
            {
                this.ResourceManager.AddDirectMethodControl(this);
                BindDataComboBox();
            }
            base.OnLoad(e);
        }
    }
}