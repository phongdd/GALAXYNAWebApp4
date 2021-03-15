using Ext.Net;
using GLXNAVWebApp.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GLXNAVWebApp.Lookup
{
    public partial class Lkp_Items : System.Web.UI.Page
    {
        public virtual string SCOPE { get { return "GLX.Lookup.Lkp_Items"; } }
        public virtual string StrFields { get { return "'No;Description;Base_Unit_of_Measure'"; } }
        public string SelectMethod = "SelectMethod";
        Common common = new Common();

        const int fetchSize = 10;
        string bookmarkKey = null;

        private Ext.Net.Panel pnlMain;
        private Ext.Net.Button btnRefresh;
        private Ext.Net.Button btnAddNew;
        private Ext.Net.Button btnSelect;
        private Ext.Net.Viewport viewPort;
        private Ext.Net.GridPanel grdMain;
        private Ext.Net.Store storeMain;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["SelectMethod"] == null)
                return;
            SelectMethod = Request.QueryString["SelectMethod"].ToString();
            InitComponents();
            if (!IsPostBack)
            {
                InitLogics();
            }
            LoadData();
        }

        private void InitLogics()
        {
            this.btnRefresh.Listeners.Click.Handler = this.SCOPE + ".Refresh();";
            this.btnAddNew.Listeners.Click.Handler = this.SCOPE + ".AddNew();";
            this.btnSelect.Listeners.Click.Handler = this.SCOPE + ".Select();";

            this.grdMain.Listeners.CellDblClick.Fn = this.SCOPE + ".Select";
        }

        private void LoadData()
        {
            ItemList.ItemList_Service s = new ItemList.ItemList_Service();
            s.Url = common.ReBuildUrl(s.Url, "");
            s.Credentials = common.CheckCredentials();
            List<ItemList.ItemList_Filter> DataFilter = new List<ItemList.ItemList_Filter>();

            #region Filter
            //Replenishment_System = Purchase
            ItemList.ItemList_Filter ReplenishmentSystemFilter = new ItemList.ItemList_Filter();
            ReplenishmentSystemFilter.Field = ItemList.ItemList_Fields.Replenishment_System;
            ReplenishmentSystemFilter.Criteria = "Purchase";
            DataFilter.Add(ReplenishmentSystemFilter);
            #endregion

            ItemList.ItemList[] Results = s.ReadMultiple(DataFilter.ToArray(), "", 10);
            List<ItemList.ItemList> ListResult = new List<ItemList.ItemList>();
            while (Results.Length > 0)
            {
                bookmarkKey = Results.Last().Key;
                ListResult.AddRange(Results);
                Results = s.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            }
            this.storeMain.DataSource = ListResult;
            this.storeMain.DataBind();
        }


        private void InitComponents()
        {
            this.btnRefresh = new Ext.Net.Button { Icon = Icon.ArrowRefresh, Text = "Tải lại", ToolTip = "Tải lại" };
            this.btnAddNew = new Ext.Net.Button { Icon = Icon.ApplicationAdd, Text = "Thêm", ToolTip = "Thêm mới" };
            this.btnSelect = new Ext.Net.Button { Icon = Icon.Find, Text = "Chọn", ToolTip = "Chọn và đóng" };
            this.storeMain = new Ext.Net.Store
            {
                ID = "storeMain",
                PageSize = 20,
                AutoLoad = true,
                Model = {
                    new Model{
                        IDProperty = "No",
                        Fields = {
                            new ModelField { Name = "No"},
                            new ModelField { Name = "Description"},
                            new ModelField { Name = "Base_Unit_of_Measure"}
                        }
                    }
                }
            };
            this.grdMain = new Ext.Net.GridPanel
            {
                ID = "grdMain",
                ColumnModel =
                {
                    Columns = {
                        new Column{ DataIndex = "No", Text = "No", Width = 100},
                        new Column{ DataIndex = "Description", Text = "Description", Width = 100},
                        new Column{ DataIndex = "Base_Unit_of_Measure", Text = "Base Unit of Measure", Width = 100}
                    }
                },
                Plugins = { new FilterHeader { } },
                BottomBar = { new PagingToolbar { } },
                SelectionModel = { new RowSelectionModel { Mode = SelectionMode.Single } }
            };
            this.grdMain.Store.Add(this.storeMain);
            this.pnlMain = new Ext.Net.Panel
            {
                Region = Region.Center,
                Border = false,
                TopBar = {
                    new Toolbar
                    {
                        Items = { this.btnRefresh, this.btnAddNew, new ToolbarSeparator(), this.btnSelect }
                    }
                },
                ContentControls = { this.grdMain }
            };
            this.viewPort = new Viewport { Layout = "Border", Items = { this.pnlMain } };
            this.Form.Controls.Add(this.viewPort);
        }

    }
}