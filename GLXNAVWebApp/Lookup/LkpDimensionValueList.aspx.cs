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
    public partial class LkpDimensionValueList : System.Web.UI.Page
    {
        public virtual string SCOPE { get { return "GLX.Lookup.LkpDimensionValueList"; } }
        public virtual string StrFields { get { return "'No;Description;Base_Unit_of_Measure'"; } }
        public string SelectMethod = "SelectMethod";
        Common common = new Common();

        const int fetchSize = 10;
        string bookmarkKey = null;

        private Ext.Net.Panel pnlMain;
        private Ext.Net.Button btnRefresh;
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

        private void LoadData()
        {
            DimensionValueList.DimensionValueList_Service svc = new DimensionValueList.DimensionValueList_Service();
            svc.Url = common.ReBuildUrl(svc.Url, GlobalVariable.CompanyName);
            svc.Credentials = common.CheckCredentials();
            List<DimensionValueList.DimensionValueList_Filter> DataFilter = new List<DimensionValueList.DimensionValueList_Filter>();

            #region Filter
            //Replenishment_System = Purchase
            DimensionValueList.DimensionValueList_Filter dimValueFilter_Code2 = new DimensionValueList.DimensionValueList_Filter();
            dimValueFilter_Code2.Field = DimensionValueList.DimensionValueList_Fields.Code2;
            dimValueFilter_Code2.Criteria = "<>''";
            DataFilter.Add(dimValueFilter_Code2);

            DimensionValueList.DimensionValueList_Filter dimValueFilter_Code = new DimensionValueList.DimensionValueList_Filter();
            dimValueFilter_Code.Field = DimensionValueList.DimensionValueList_Fields.Code;
            dimValueFilter_Code.Criteria = "CIN*";
            DataFilter.Add(dimValueFilter_Code);

            DimensionValueList.DimensionValueList_Filter dimValueFilter_Blocked = new DimensionValueList.DimensionValueList_Filter();
            dimValueFilter_Blocked.Field = DimensionValueList.DimensionValueList_Fields.Blocked;
            dimValueFilter_Blocked.Criteria = "No";
            DataFilter.Add(dimValueFilter_Blocked);
            #endregion

            DimensionValueList.DimensionValueList[] Results = svc.ReadMultiple(DataFilter.ToArray(), "", 10);
            List<DimensionValueList.DimensionValueList> ListResult = new List<DimensionValueList.DimensionValueList>();
            while (Results.Length > 0)
            {
                bookmarkKey = Results.Last().Key;
                ListResult.AddRange(Results);
                Results = svc.ReadMultiple(DataFilter.ToArray(), bookmarkKey, fetchSize);
            }
            this.storeMain.DataSource = ListResult;
            this.storeMain.DataBind();
        }

        private void InitLogics()
        {
            this.btnRefresh.Listeners.Click.Handler = this.SCOPE + ".Refresh();";
            this.btnSelect.Listeners.Click.Handler = this.SCOPE + ".Select();";
            this.grdMain.Listeners.CellDblClick.Fn = this.SCOPE + ".Select";
        }

        private void InitComponents()
        {
            this.btnRefresh = new Ext.Net.Button { Icon = Icon.ArrowRefresh, Text = "Tải lại", ToolTip = "Tải lại" };
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
                        Items = { this.btnRefresh, new ToolbarSeparator(), this.btnSelect }
                    }
                },
                ContentControls = { this.grdMain }
            };
            this.viewPort = new Viewport { Layout = "Border", Items = { this.pnlMain } };
            this.Form.Controls.Add(this.viewPort);
        }
    }
}