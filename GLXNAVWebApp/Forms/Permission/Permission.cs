using Ext.Net;
using GLXNAVWebApp.Classes;
using GLXNAVWebApp.GLXAccountSVC;
using GLXNAVWebApp.GLXRequisitionWkshName;
using GLXNAVWebApp.RBSL_LocationAndAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace GLXNAVWebApp.Forms.Permission
{
    [DirectMethodProxyID(IDMode = DirectMethodProxyIDMode.None)]
    public class Permission : Viewport
    {
        //GLXAccount
        //Permissions
        //LocationAndAccount
        //Requisition Wksh.Name
        public const string SCOPE = "GLX.FORMS.Permission";

        Common common = new Common();

        #region define controls
        Ext.Net.FormPanel frmGrid;
        Ext.Net.FormPanel frmHeader;
        Ext.Net.Store strPermission;
        Ext.Net.GridPanel grdPermission;

        Ext.Net.Store strGLXAccount;
        Ext.Net.GridPanel grdGLXAccount;

        Ext.Net.Store strLocationAndAccount;
        Ext.Net.GridPanel grdLocationAndAccount;

        Ext.Net.Store strWorksheet;
        Ext.Net.GridPanel grdWorksheet;

        Ext.Net.ComboBox cboUsername;
        Ext.Net.Store strUsername;

        Ext.Net.Button btnAddUser;
        Ext.Net.Button btnDelete;

        //Difine Controls for Card
        Ext.Net.TextField txtUserName;
        Ext.Net.TextField txtFullName;
        Ext.Net.CheckboxGroup cbgCinema;
        Ext.Net.ComboBox cboLocation;
        Ext.Net.ComboBox cboBatch;
        Ext.Net.RadioGroup rdgRole;
        Ext.Net.ComboBox cboCompanyCard;

        #endregion

        public Permission()
        {
            InitComponent();
            InitLogis();
        }

        private void InitLogis()
        {
            cboUsername.Listeners.Change.Fn = SCOPE + ".cboUsernameChange";
            btnAddUser.Listeners.Click.Handler = SCOPE + ".AddUser();";
            btnDelete.Listeners.Click.Handler = SCOPE + ".DelUser();";
        }

        private void InitComponent()
        {
            #region Button
            btnAddUser = new Button { ID = "btnAddUser", Text = "Add User", Icon = Icon.UserAdd };
            btnDelete = new Button { ID = "btnDelete", Text = "Delete User", Icon = Icon.UserDelete };
            #endregion Button

            #region Card
            txtUserName = new TextField
            {
                ID = "txtUserName",
                EmptyText = "Enter username",
                Flex = 1,
                LabelWidth = 80,
                FieldLabel = "Username",
                AllowBlank = false
            };

            txtFullName = new TextField
            {
                ID = "txtFullName",
                EmptyText = "Enter full name",
                Flex = 1,
                LabelWidth = 80,
                FieldLabel = "Full name",
                AllowBlank = false
            };

            cbgCinema = new CheckboxGroup
            {
                ID = "cbgCinema",
                FieldLabel = "Cinema",
                ColumnsNumber = 3,
                Cls = "x-check-group-alt"
            };

            cboLocation = new ComboBox
            {
                ID = "cboLocation",
                FieldLabel = "Location",
                EmptyText = "Enter Location",
                Flex = 1,
                LabelWidth = 80,
                AllowBlank = false
            };

            cboBatch = new ComboBox
            {
                ID = "cboBatch",
                EmptyText = "Enter Batch",
                Flex = 1,
                LabelWidth = 80,
                FieldLabel = "Batch",
                AllowBlank = false
            };

            rdgRole = new RadioGroup
            {
                ID = "rdgRole",
                FieldLabel = "Role",
                ColumnsNumber = 1,
                Items =
                        {
                            new Radio { ID = "rdoAdmin", BoxLabel = "Admin", Value = "Admin", InputValue = "Admin"},
                            new Radio { ID = "rdoFnb", BoxLabel = "F&B", Value = "FnB", InputValue = "FnB"},
                            new Radio { ID = "rdoAccSite", BoxLabel = "Accounting Site", Value = "AccSite", InputValue = "AccSite", Checked = true},
                            new Radio { ID = "rdoCM", BoxLabel = "Cinema Manager", Value = "CM", InputValue = "CM"}
                        }
            };

            cboCompanyCard = new ComboBox {
                ID = "cboCompanyCard",
                EmptyText = "Enter company",
                Flex = 1,
                LabelWidth = 80,
                FieldLabel = "Company",
                AllowBlank = false
            };
            #endregion 

            #region Account
            strUsername = new Store
            {
                ID = "strUsername",
                Model = {
                    new Model {
                        Fields = {
                            new ModelField { Name = "Account"},
                            new ModelField { Name = "Fullname"},
                        }
                    }
                }
            };
            
            cboUsername = new Ext.Net.ComboBox
            {
                DataIndex = "Account",
                Name = "Account",
                LabelWidth = 80,
                Anchor = "100%",
                FieldLabel = "Account",
                ID = "cboUsername",
                SelectOnFocus = true,
                AllowBlank = false,
                MsgTarget = MessageTarget.Side,
                DisplayField = "Account",
                ValueField = "Account",
                PageSize = 100,
                
            };
            cboUsername.Store.Add(strUsername);
            cboUsername.ListConfig = new BoundList {
                Width = 320,
                Height = 300,
                ItemSelector = ".x-boundlist-item",
                Tpl = new XTemplate {
                    Html = string.Format(@"
                    <Html>
					    <tpl for=""."">
						    <tpl if=""[xindex] == 1"">
							    <table class=""cbStates-list"">
								    <tr>
									<th style =""font-weight: bold;padding: 3px;background: #3892d3"">{0}</th> 
                                    <th style =""font-weight: bold;padding: 3px;background: #3892d3"">{1}</th>   
								    </tr>
						    </tpl>
						    <tr class=""x-boundlist-item"">
							    <td style=""padding:3px;"">{{{0}}}</td>    
                                <td style=""padding:3px;"">{{{1}}}</td> 
						    </tr>
						    <tpl if=""[xcount-xindex]==0"">
							    </table>
						    </tpl>
					    </tpl>
				    </Html>", "Account", "Fullname")
                }
            };
            cboUsername.Triggers.Add(new FieldTrigger { Icon = TriggerIcon.Clear, HideTrigger = true });
            #endregion Account

            #region Permission
            strPermission = new Store
            {
                ID = "strPermission",
                Model = {
                    new Model {
                        Fields = {
                            new ModelField { Name = "UID" },
                            new ModelField { Name = "Company" },
                            new ModelField { Name = "Username" },
                            new ModelField { Name = "Page" },
                            new ModelField { Name = "Action" },
                            new ModelField { Name = "Allow" }
                        }
                    }
                }
            };
            grdPermission = new GridPanel
            {
                ID = "grdPermission",
                BodyCls = "line-body-border",
                CtCls = "line-border",
                Title = "Permissions List",
                Height = 250,
                ColumnModel = {
                    Columns = {
                        new Column { DataIndex = "UID", Text = "UID", Width = 0},
                        new Column { DataIndex = "Company", Text = "Company", MinWidth = 100, Flex = 1},
                        new Column { DataIndex = "Username", Text = "Username", Width = 100},
                        new Column { DataIndex = "Page", Text = "Page", Width = 120},
                        new Column { DataIndex = "Action", Text = "Action", MinWidth = 150, Flex = 1},
                        new Column { DataIndex = "Allow", Text = "Allow", Width = 100},
                    }
                },
                BottomBar = { new PagingToolbar { HideRefresh = true } },
                SelectionModel = { new RowSelectionModel { Mode = SelectionMode.Single } }
            };
            grdPermission.Store.Add(strPermission);
            #endregion Permission

            #region GLXAccount
            strGLXAccount = new Store
            {
                ID = "strGLXAccount",
                Model = {
                    new Model {
                        Fields = {
                            new ModelField { Name = "UID" },
                            new ModelField { Name = "Account" },
                            new ModelField { Name = "Company" }
                        }
                    }
                }
            };
            grdGLXAccount = new GridPanel
            {
                ID = "grdGLXAccount",
                BodyCls = "line-body-border",
                CtCls = "line-border",
                Title = "Company List",
                Height = 250,
                ColumnModel = {
                    Columns = {
                        new Column { DataIndex = "UID", Text = "UID", Width = 0},
                        new Column { DataIndex = "Company", Text = "Company", Width = 100, Flex = 1},
                    }
                },
                BottomBar = { new PagingToolbar { HideRefresh = true } },
                SelectionModel = { new RowSelectionModel { Mode = SelectionMode.Single } }
            };
            grdGLXAccount.Store.Add(strGLXAccount);
            #endregion GLXAccount

            #region LocationAndAccount
            strLocationAndAccount = new Store
            {
                ID = "strLocationAndAccount",
                Model = {
                    new Model {
                        Fields = {
                            new ModelField { Name = "UID" },
                            new ModelField { Name = "Account" },
                            new ModelField { Name = "Location" }
                        }
                    }
                }
            };
            grdLocationAndAccount = new GridPanel
            {
                ID = "grdLocationAndAccount",
                BodyCls = "line-body-border",
                CtCls = "line-border",
                Title =  "Location List",
                Height = 220,
                ColumnModel = {
                    Columns = {
                        new Column { DataIndex = "UID", Text = "UID", Width = 0},
                        new Column { DataIndex = "Location", Text = "Location", Width = 100, Flex = 1},
                    }
                },
                BottomBar = { new PagingToolbar { HideRefresh = true } },
                SelectionModel = { new RowSelectionModel { Mode = SelectionMode.Single } }
            };
            grdLocationAndAccount.Store.Add(strLocationAndAccount);
            #endregion

            #region Worksheet
            strWorksheet = new Store
            {
                ID = "strWorksheet",
                Model = {
                    new Model {
                        Fields = {
                            new ModelField { Name = "UID" },
                            new ModelField { Name = "Worksheet Template Name" },
                            new ModelField { Name = "Name" },
                            new ModelField { Name = "Description" },
                            new ModelField { Name = "Template Type" },
                            new ModelField { Name = "Recurring" },
                            new ModelField { Name = "Company" },
                            new ModelField { Name = "Account" },
                        }
                    }
                }
            };

            grdWorksheet = new GridPanel
            {
                ID = "grdWorksheet",
                BodyCls = "line-body-border",
                CtCls = "line-border",
                Title = "Worksheet List",
                Height = 220,
                ColumnModel = {
                    Columns = {
                        new Column { DataIndex = "UID", Text = "UID", Width = 0},
                        new Column { DataIndex = "Company", Text = "Company", Width = 150},
                        new Column { DataIndex = "Name", Text = "Name", Width = 100},
                        new Column { DataIndex = "Description", Text = "Description", Width = 120, Flex = 1},
                        new Column { DataIndex = "Account", Text = "Account", Width = 100},
                    }
                },
                BottomBar = { new PagingToolbar { HideRefresh = true } },
                SelectionModel = { new RowSelectionModel { Mode = SelectionMode.Single } }
            };
            grdWorksheet.Store.Add(strWorksheet);
            #endregion 

            #region FormPanel
            frmHeader = new FormPanel
            {
                Collapsed = false,
                Collapsible = true,
                Region = Region.North,
                Layout = "HBox",
                Items = {
                    new FormPanel {
                        Layout = "Anchor",
                        Flex = 1,
                        BodyPaddingSummary = "10 10 10 10",
                        Border = false,
                        Items = { cboUsername }
                    },
                    new FormPanel {
                        Layout = "Anchor",
                        Flex = 1,
                        BodyPaddingSummary = "10 10 10 10",
                        Border = false
                    }
                },
                TopBar = {
                    new Toolbar {
                        Items = {
                            btnAddUser,
                            btnDelete
                        }
                    }
                }
            };

            frmGrid = new FormPanel
            {
                Layout = "HBoxLayout",
                Region = Region.Center,
                Header = true,
                BodyPaddingSummary = "5,5,5,5",
                Items = {
                    new Panel {
                        BodyPadding = 5,
                        Layout = "VBoxLayout",
                        Flex = 2,
                        Region = Region.Center,
                        LayoutConfig = { new Ext.Net.VBoxLayoutConfig { Align = VBoxAlign.Stretch} },
                        Items = {
                            grdPermission, grdWorksheet
                        } },
                    new Panel {
                        BodyPaddingSummary = "5,5,5,5",
                        Layout = "Fit",
                        Flex = 1,
                        Region = Region.East,
                        Items = {
                            new Panel { Items = { grdGLXAccount, grdLocationAndAccount } }
                        } }
                }
            };
            #endregion FormPanel

            this.ID = "pageMain";
            this.Layout = "BorderLayout";
            this.Items.AddRange(new ItemsCollection<Ext.Net.AbstractComponent> {
                frmGrid, frmHeader
            });
        }

        #region DirectMethod
        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void BindingDataWithFilter(string username)
        {
            Permissions.BSL_PermissionsClient client = new Permissions.BSL_PermissionsClient();
            Permissions.data_Permissions[] PermissionsList = client.GetPermissionByUser(username);
            strPermission.DataSource = PermissionsList;
            strPermission.DataBind();

            BSL_GLXAccountClient AccountClient = new BSL_GLXAccountClient();
            data_GLXAccount[] Companies = AccountClient.GetAll(username);
            strGLXAccount.DataSource = Companies;
            strGLXAccount.DataBind();

            BSL_RequisitionWkshNameClient WkshClient = new BSL_RequisitionWkshNameClient();
            data_RequisitionWkshName[] WkshList = WkshClient.GetDataByUser(username);
            strWorksheet.DataSource = WkshList;
            strWorksheet.DataBind();

            BSL_LocationAndAccountClient LocationClient = new BSL_LocationAndAccountClient();
            data_LocationAndAccount[] Locations = LocationClient.GetLocationByUser(username);
            strLocationAndAccount.DataSource = Locations;
            strLocationAndAccount.DataBind();
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public void AddUser()
        {
            Ext.Net.Window w;
            w = new Window
            {
                Title = "Add User",
                Icon = Icon.UserAdd,
                CloseAction = CloseAction.Destroy,
                Layout = "FormLayout",
                BodyPadding = 5,
                Draggable = false,
                Resizable = true,
                Height = 450,
                Width = 600,
                Items = {
                    txtUserName,
                    txtFullName,
                    cboCompanyCard,
                    cboLocation,
                    cboBatch,
                    rdgRole,
                    cbgCinema
                },
                Buttons = {
                    new Button {
                        ID = "UserAdd_btnAddUser",
                        Text ="Add",
                        Icon = Icon.UserAdd,
                        Handler = @" if (!#{txtUserName}.validate() 
                                        || !#{txtFullName}.validate() || !#{cboCompanyCard}.validate() 
                                        || !#{cboLocation}.validate() || !#{cboBatch}.validate() || !#{rdgRole}.validate()) {
                                                Ext.Msg.alert('Error', 'All fields are required'); return false;}
                                    else{ 
                                        //debugger; 
                                        var UserName = #{txtUserName}.getValue();
                                        var FullName = #{txtFullName}.getValue();
                                        var Company = #{cboCompanyCard}.getValue();
                                        var Location = #{cboLocation}.getValue();
                                        var Batch = #{cboBatch}.getValue();
                                        var Role = #{rdgRole}.getChecked()[0].inputValue;
                                        var CheckedBoxes = #{cbgCinema}.getChecked();

                                        var Companies = '';
                                        for (i = 0; i < CheckedBoxes.length; i++) {
                                          Companies += CheckedBoxes[i].name + ',';
                                        }
                                        
                                        App.direct.DoAddUser(UserName, FullName, Company, Location, Batch, Role, Companies, {
                                                success: function (result) { 
                                                    if (result.Success == true) {
                                                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: 'Created!', buttons: Ext.Msg.OK });
                                                    } else {
                                                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                                                    }
                                                },
                                                failure: function (error, response) {
                                                    if (error != '')
                                                        Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                                                }
                                            });
                                    }"
                    }
                }
            };
            w.Render();
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse DelUser(string uname)
        {
            BSL_GLXAccountClient client = new BSL_GLXAccountClient();
            string mess = client.DelUser(uname);

            DirectResponse dr = new DirectResponse();
            if (mess == "")
            {
                dr.Success = true;
                dr.Result = String.Format("{{Message:'{0}'}}", "");
            }
            else
            {
                dr.Success = false;
                dr.ErrorMessage = mess;
            }
            return dr;
        }

        [DirectMethod(ShowMask = true, Msg = "Loading....")]
        public DirectResponse DoAddUser(string userName, string fullName, string company, string location, string batch, string role, string companies)
        {
            var dataUser = new data_UserCreateRequest();
            dataUser.Username = userName;
            dataUser.FullName = fullName;
            dataUser.Company = company;
            dataUser.Location = location;
            dataUser.Batch = batch;
            dataUser.Role = role;
            dataUser.Companies = companies;

            BSL_GLXAccountClient client = new BSL_GLXAccountClient();
            string mess = client.createUser(dataUser);

            DirectResponse dr = new DirectResponse();
            if (mess == "")
            {
                dr.Success = true;
                dr.Result = String.Format("{{Message:'{0}'}}", "");
            }
            else
            {
                dr.Success = false;
                dr.ErrorMessage = mess;
            }
            return dr;

        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!Ext.Net.X.IsAjaxRequest)
            {
                this.ResourceManager.AddDirectMethodControl(this);
            }

            #region Load Company Info
            Common common = new Common();
            Companies.Companies_Service svc = new GLXNAVWebApp.Companies.Companies_Service();
            svc.Credentials = common.CheckCredentials();
            Companies.Companies[] results = svc.ReadMultiple(null, "", 20);
            foreach (Companies.Companies item in results)
            {
                //if (item.Name == "GALAXY_MASTERDATA" || item.Name == "GALAXY_HCM" || item.Name == "GALAXY_BMT_COPY") continue;
                cbgCinema.Add(new Checkbox { ID = item.Name, BoxLabel = item.Name });
                cboCompanyCard.Items.Add(new ListItem { Text = item.Name, Value = item.Name });
            }
            #endregion Load Company Info

            #region Load Location and Batch
            List<LocationData> locations = GetLocation();
            List<BatchData> batches = GetBatch();

            foreach (LocationData item in locations)
            {
                cboLocation.Items.Add(new ListItem { Text = item.Name, Value = item.Code });
            }

            foreach (BatchData item in batches)
            {
                cboBatch.Items.Add(new ListItem { Text = item.Name, Value = item.Code });
            }
            #endregion Load Location and Batch

            #region Load User
            BSL_GLXAccountClient AccountClient = new BSL_GLXAccountClient();
            data_Account[] AccountList = AccountClient.GetAllAccount(GlobalVariable.UserName);
            strUsername.DataSource = AccountList;
            strUsername.DataBind();
            #endregion Load User
        }

        public List<LocationData> GetLocation()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("../../Data/Locations.xml"));
            List<LocationData> data = new List<LocationData>();

            foreach (XmlNode LocationNode in xmlDoc.SelectNodes("Locations/location"))
            {
                string Code = LocationNode.SelectSingleNode("Code").InnerText;
                string Name = LocationNode.SelectSingleNode("Name").InnerText;

                data.Add(new LocationData(Code, Name));
            }
            return data;
        }

        public List<BatchData> GetBatch()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(HttpContext.Current.Server.MapPath("../../Data/Batchs.xml"));
            List<BatchData> data = new List<BatchData>();

            foreach (XmlNode BatchNode in xmlDoc.SelectNodes("Batches/batch"))
            {
                string Code = BatchNode.SelectSingleNode("Code").InnerText;
                string Name = BatchNode.SelectSingleNode("Name").InnerText;

                data.Add(new BatchData(Code, Name));
            }
            return data;
        }

    }

    public class LocationData
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public LocationData() { }
        public LocationData(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }

    public class BatchData
    {
        public string Code { get; set; }
        public string Name { get; set; }

        public BatchData() { }
        public BatchData(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}
