using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Forms.PurchaseOrder
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public class CtlPO: Viewport
    {
        Common common = new Common();
        public const int fetchSize = 10;
        public string bookmarkKey = null;
        public const string SCOPE = "GLX.FORMS.PO";

        Ext.Net.DateField dfFromDate;
        Ext.Net.DateField dfToDate;
        Ext.Net.GridPanel grdItems;
        Ext.Net.Store strItems;
        Ext.Net.Button btnAddItem;
        Ext.Net.Button btnEditItem;
        Ext.Net.Button btnSearch;
        Ext.Net.Button btnPrint;
        Ext.Net.Panel pnlCenter;
        Ext.Net.Panel pnlLeft;
        Ext.Net.Window PrintPreview;

        public CtlPO()
        {
            InitComponent();
            InitLogis();
        }

        private void InitComponent()
        {
            btnSearch = new Button { Icon = Icon.Find, Text = "Find", ToolTip = "Find" };
            btnAddItem = new Button { Icon = Icon.ImageAdd, Text = "Thêm", ToolTip = "Thêm" };
            btnEditItem = new Button { Icon = Icon.ImageEdit, Text = "Sửa", ToolTip = "Sửa" };
            btnPrint = new Button { Icon = Icon.Printer, Text = "Print", ToolTip = "Print" };

            int daysInMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            DateTime FromDate = DateTime.Now.AddDays(-1 * daysInMonth);

            dfFromDate = new DateField
            {
                LabelWidth = 100,
                FieldLabel = "Document Date",
                Anchor = "100%",
                Format = "dd/MM/yyyy",
                SubmitFormat = "dd/MM/yyyy",
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
                LabelWidth = 20,
                Anchor = "100%",
                Format = "dd/MM/yyyy",
                SubmitFormat = "dd/MM/yyyy",
                SelectOnFocus = true,
                SelectedDate = DateTime.Now,
                ID = "dfToDate",
                Vtype = "daterange",
                EnableKeyEvents = true,
                CustomConfig = {
                    new ConfigItem {Name = "startDateField", Value = "dfFromDate", Mode = ParameterMode.Value}
                }
            };

            strItems = new Store
            {
                ID = "strItems",
                Model = {
                    new Model {
                        IDProperty = "Key",
                        Fields = {
                            new ModelField { Name = "Key" },
                            new ModelField { Name = "No" },
                            new ModelField { Name = "Buy_from_Vendor_No" },
                            new ModelField { Name = "Buy_from_Vendor_Name" },
                            new ModelField { Name = "Status" },
                            new ModelField { Name = "Document_Date", Type = ModelFieldType.Date },
                            new ModelField { Name = "Posting_Date", Type = ModelFieldType.Date },
                            new ModelField { Name = "Order_Date", Type = ModelFieldType.Date },
                            new ModelField { Name = "Vendor_Authorization_No" },
                            new ModelField { Name = "Vista_Order_No"},
                            new ModelField { Name = "Location_Code" },
                            new ModelField { Name = "Assigned_User_ID" },
                            new ModelField { Name = "Job_Queue_Status" },
                            new ModelField { Name = "Amount", Type = ModelFieldType.Float },
                            new ModelField { Name = "Amount_Including_VAT", Type = ModelFieldType.Float },
                        }
                    }
                }
            };

            grdItems = new GridPanel
            {
                ID = "grdItems",
                BodyCls = "line-body-border",
                CtCls = "line-border",
                ColumnModel =
                {
                    Columns = {
                        new Column{ DataIndex = "Key", Text = "Key", Width = 0 },
                        new Column{ DataIndex = "No", Text = "No", Width = 120, },
                        new Column{ DataIndex = "Buy_from_Vendor_No", Text = "Vendor No.", Width = 120 },
                        new Column{ DataIndex = "Buy_from_Vendor_Name", Text = "Buy from Vendor Name", Width = 220, Flex = 1 },
                        new Column{ DataIndex = "Location_Code", Text = "Location_Code", Width = 120 },
                        new Column{ DataIndex = "Status", Text = "Status", Width = 120 },
                        new DateColumn{ DataIndex = "Document_Date", Text = "Document Date", Width = 120, Format = "dd/MM/yyyy" },
                        new DateColumn{ DataIndex = "Posting_Date", Text = "Posting Date", Width = 120, Format = "dd/MM/yyyy" },
                        //new DateColumn{ DataIndex = "Order_Date", Text = "Order Date", Width = 120, Format = "dd/MM/yyyy" },
                    }
                },
                TopBar = {
                    new Toolbar {
                        Items = {
                            dfFromDate, dfToDate, btnSearch,//btnAddItem, btnEditItem
                            btnPrint
                        }
                    }
                },
                Plugins = { new FilterHeader { } },
                BottomBar = { new PagingToolbar { HideRefresh = true } },
                SelectionModel = {
                    new RowSelectionModel{
                        Mode = SelectionMode.Single
                    }
                }
            };

            grdItems.Store.Add(strItems);

            pnlCenter = new Panel
            {
                Layout = "Fit",
                Region = Region.Center,
                Items = { grdItems }
            };
            pnlLeft = new Panel {
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
                        NavigateUrl = "../../Login.aspx",
                        Text = "Relogin"
                    },

                    new Ext.Net.HyperLink {
                        Width = 250,
                        Icon = Icon.PackageGo,
                        Target = "_blank",
                        NavigateUrl = "../ReqWorksheet",
                        Text = "Req. Worksheets"
                    },

                    //new Ext.Net.HyperLink {
                    //    Width = 250,
                    //    Icon = Icon.Page,
                    //    Target = "_blank",
                    //    NavigateUrl = "../ReqWorksheet2",
                    //    Text = "Req. Worksheets 2"
                    //}

                }
            };
            this.ID = "pageMain";
            this.Layout = "BorderLayout";
            this.Items.AddRange(new ItemsCollection<Ext.Net.AbstractComponent> {
                pnlLeft,
                pnlCenter
            });
        }

        private void InitLogis()
        {
            dfFromDate.Listeners.KeyUp.Fn = SCOPE + ".OnKeyUp";
            dfToDate.Listeners.KeyUp.Fn = SCOPE + ".OnKeyUp";
            dfToDate.Listeners.Focus.Fn = SCOPE + ".OnKeyUp";
            dfFromDate.Listeners.Focus.Fn = SCOPE + ".OnKeyUp";
            btnSearch.Listeners.Click.Handler = SCOPE + ".Search();";
            btnPrint.Listeners.Click.Handler = SCOPE + ".Print();";
            grdItems.Listeners.ItemDblClick.Handler = SCOPE + ".grdItemsDblClick();";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Ext.Net.X.IsAjaxRequest)
            {
                this.ResourceManager.AddDirectMethodControl(this);
                BindingDataWithFilter("", "");
            }
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void LoadCard(string Key, string DocNo, string Mode)
        {
            CtlPOEdit winCard = new CtlPOEdit(Key, DocNo, Mode);
            winCard.Maximize();
            winCard.Render();
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void BindingDataWithFilter(string f, string t)
        {
            DateTime FDate;
            DateTime TDate;
            if (string.IsNullOrEmpty(f) || string.IsNullOrEmpty(t))
            {
                bookmarkKey = null;
                FDate = dfFromDate.SelectedDate;
                TDate = dfToDate.SelectedDate;
            }
            else {
                FDate = DateTime.Parse(f);
                TDate = DateTime.Parse(t);
            }
            //if (TDate.AddMonths(-6) > FDate)
            //{
            //    Ext.Net.X.Msg.Show(new MessageBoxConfig
            //    {
            //        Title = "Thông báo",
            //        Message = "Bạn vui lòng chọn lại ngày để xem. <br> Lưu ý: Thông tin chỉ được truy vấn trong vòng 6 tháng.",
            //        Icon = MessageBox.Icon.WARNING,
            //        Buttons = MessageBox.Button.OK
            //    });
            //}

            PurchaseOrderList.PurchaseOrderList_Service svc = new PurchaseOrderList.PurchaseOrderList_Service();
            svc.Url = common.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
            svc.Credentials = common.CheckCredentials();

            List<PurchaseOrderList.PurchaseOrderList_Filter> ReqFilters = new List<PurchaseOrderList.PurchaseOrderList_Filter>();

            PurchaseOrderList.PurchaseOrderList_Filter DocDateFilter = new PurchaseOrderList.PurchaseOrderList_Filter();
            DocDateFilter.Field = PurchaseOrderList.PurchaseOrderList_Fields.Document_Date;
            DocDateFilter.Criteria = String.Format("{0}..{1}", dfFromDate.SelectedDate.Date.ToString("MM/dd/yyyy"), dfToDate.SelectedDate.Date.ToString("MM/dd/yyyy"));
            ReqFilters.Add(DocDateFilter);

            PurchaseOrderList.PurchaseOrderList[] results = svc.ReadMultiple(ReqFilters.ToArray(), bookmarkKey, fetchSize);
            List<PurchaseOrderList.PurchaseOrderList> POList = new List<PurchaseOrderList.PurchaseOrderList>();
            while (results.Length > 0)
            {
                bookmarkKey = results.Last().Key;
                POList.AddRange(results);
                results = svc.ReadMultiple(ReqFilters.ToArray(), bookmarkKey, fetchSize);
            }

            strItems.DataSource = POList;
            strItems.DataBind();
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void Print(string strKey, string docNo)
        {
            ShowReport(docNo);
        }

        private void ShowReport(string UID)
        {
            PrintPreview = new Window
            {
                ID = "PrintPreview",
                Maximizable = false,
                Minimizable = false,
                Closable = true,
                Modal = true,
                Frame = true,
                CloseAction = CloseAction.Destroy,
                Loader = new ComponentLoader
                {
                    Url = String.Format(@"Preview.aspx?UID={0}", UID),
                    Mode = LoadMode.Frame,
                    LoadMask = { ShowMask = true }
                }
            };
            PrintPreview.Maximize();
            PrintPreview.Render();
        }

    }
}