using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Forms.ReqWorksheet2
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public class CtlReqWorksheet2 : Viewport
    {
        #region Variables
        public const int fetchSize = 10;
        public string bookmarkKey = null;
        public const string SCOPE = "GLX.FORMS.ReqWorksheet2";
        string currentpage = "ReqWorksheet2";
        Common common = new Common();

        Ext.Net.GridPanel grdItems;
        Ext.Net.Store strItems;
        Ext.Net.Store strBatchs;
        Ext.Net.SelectBox cboBatchs;

        Ext.Net.Button btnCarry_out_action_message;
        #endregion
        public CtlReqWorksheet2()
        {
            InitComponent();
            InitLogis();
            BindDataComboBox();
        }

        private void InitLogis()
        {
            cboBatchs.Listeners.Change.Fn = SCOPE + ".cboBatchsChange";

            btnCarry_out_action_message.Listeners.Click.Handler = SCOPE + ".Carry_out_action_message();";
        }

        private void InitComponent()
        {

            btnCarry_out_action_message = new Button { Icon = Icon.ControllerAdd, Text = " Carry Out Action Message", };

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

            strItems = new Store
            {
                ID = "strItems",
                Model = {
                    new Model {
                        IDProperty = "Key",
                        Fields = {
                            new ModelField { Name = "Type" },
                            new ModelField { Name = "No" },
                            new ModelField { Name = "Action_Message" },
                            new ModelField { Name = "Accept_Action_Message" },
                            new ModelField { Name = "Description" },
                            new ModelField { Name = "Description2" },
                            new ModelField { Name = "Description_2" },
                            new ModelField { Name = "Vietnamese_Description" },
                            new ModelField { Name = "Location_Code" },
                            new ModelField { Name = "Shortcut_Dimension_1_Code" },
                            new ModelField { Name = "Shortcut_Dimension_2_Code" },
                            new ModelField { Name = "Quantity" },
                            new ModelField { Name = "Unit_of_Measure_Code" },
                            new ModelField { Name = "Direct_Unit_Cost" },
                            new ModelField { Name = "Due_Date" },
                            new ModelField { Name = "Vendor_No" },
                            new ModelField { Name = "Vendor_Item_No" },
                            new ModelField { Name = "Replenishment_System" }
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
                        new Column{ DataIndex = "Type", Text = "Type", Width = 90 },
                        new Column{ DataIndex = "No", Text = "No", Width = 150 },
                        new Column{ DataIndex = "Action_Message", Text = "Action Message", Width = 150 },
                        new Column{ DataIndex = "Accept_Action_Message", Text = "Accept Action Message", Width = 150 },
                        new Column{ DataIndex = "Description", Text = "Description", Width = 200, Flex = 1, MinWidth = 200 },
                        new Column{ DataIndex = "Description_2", Text = "Description 2", Width = 200, Flex = 1, MinWidth = 200 },
                        new Column{ DataIndex = "Vietnamese_Description", Text = "Vietnamese Description", Width = 200, Flex = 1, MinWidth = 200 },
                        new Column{ DataIndex = "Location_Code", Text = "Location Code", Width = 120 },
                        new Column{ DataIndex = "Shortcut_Dimension_1_Code", Text = "Entity Code", Width = 120 },
                        new NumberColumn{ DataIndex = "Quantity", Text = "Quantity", Width = 120, Format = "0,000.00", Align = Alignment.Right },
                        new Column{ DataIndex = "Unit_of_Measure_Code", Text = "UOM Code", Width = 100 },
                        new NumberColumn{ DataIndex = "Direct_Unit_Cost", Text = "Direct Unit Cost", Width = 120, Format = "0,000.00", Align = Alignment.Right },
                        new DateColumn{ DataIndex = "Due_Date", Text = "Due Date", Width = 120, Format = "dd/MM/yyyy", Align = Alignment.Left},
                        new Column{ DataIndex = "Vendor_No", Text = "Vendor_No", Width = 150 },
                        new Column{ DataIndex = "Vendor_Item_No", Text = "Vendor Item No", Width = 150 },
                        new Column{ DataIndex = "Replenishment_System", Text = "Replenishment_System", Width = 100 },
                    }
                },
                TopBar = {
                    new Toolbar {
                        Items = {
                            cboBatchs, btnCarry_out_action_message,
                            new ToolbarFill()
                        }
                    }
                },
                Plugins = { new FilterHeader { } },
                BottomBar = { new PagingToolbar { HideRefresh = true } },
                SelectionModel = {
                    new RowSelectionModel{
                        //Mode = SelectionMode.Single
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

                    new Ext.Net.HyperLink {
                        Width = 250,
                        Icon = Icon.PackageGo,
                        Target = "_blank",
                        NavigateUrl = "../ReqWorksheet",
                        Text = "Req. Worksheets"
                    }
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
        public void BindingDataWithFilter(string batch)
        {
            GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service svc = new GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Service();
            svc.Url = common.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
            svc.Credentials = common.CheckCredentials();

            //GLXNAVWebApp.ReqWorksheet.ReqWorksheet_Fields.

            GLXNAVWebApp.ReqWorksheet.ReqWorksheet[] result = svc.ReadMultiple(batch, null, bookmarkKey, fetchSize);
            List<GLXNAVWebApp.ReqWorksheet.ReqWorksheet> reqWorksheet = new List<GLXNAVWebApp.ReqWorksheet.ReqWorksheet>();
            while (result.Length > 0)
            {
                bookmarkKey = result.Last().Key;
                reqWorksheet.AddRange(result);
                result = svc.ReadMultiple(batch, null, bookmarkKey, fetchSize);
            }
            strItems.DataSource = reqWorksheet;
            strItems.DataBind();
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse Carry_out_action_message()
        {
            DirectResponse dr = new DirectResponse();
            try
            {
                GLXNAVWebApp.PurchaseOrder_CU.PurchaseOrder purchaseOrder_cu = new GLXNAVWebApp.PurchaseOrder_CU.PurchaseOrder();
                purchaseOrder_cu.Url = common.ReBuildUrl(purchaseOrder_cu.Url, GlobalVariable.CompanyName);
                purchaseOrder_cu.Credentials = common.CheckCredentials();
                purchaseOrder_cu.UseOneJnl();

                dr.Success = true;
                //dr.Result = String.Format("{{Message:'{0}'}}", result);
            }
            catch (Exception ex)
            {
                dr.Success = false;
                dr.ErrorMessage = ex.Message;
            }

            return dr;
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