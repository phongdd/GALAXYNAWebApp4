using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Classes
{
    public class ComboBoxLookup : Ext.Net.ComboBox
    {
        private string scope;
        public string SCOPE
        {
            get { return scope; }
            set { scope = value; this.BuildControls(); }
        }
        public string CurCompany { get; set; }
        private List<LookupFormatData> dataTemplate = new List<LookupFormatData>();
        public List<LookupFormatData> DataTemplete
        {
            get { return dataTemplate; }
            set { dataTemplate = value; }
        }

        public string ProxyUrl { get; set; }
        public string idProperty { get; set; }

        public ComboBoxLookup()
        {
            FieldTrigger ft = new FieldTrigger
            {
                //Icon = Ext.Net.TriggerIcon.SimpleArrowDown
            };
            Triggers.Add(ft);
            ListConfig = new BoundList
            {
                LoadingText = "Searching..."
            };
            HideTrigger = false;
            HideBaseTrigger = true;
            MinChars = 3;
            TypeAhead = false;
            ForceSelection = false;
            Editable = true;
            EmptyText = "Type something, at least 3 character...";
            //this.PageSize = 10;
            //TriggerIcon = Ext.Net.TriggerIcon.Search;
            MatchFieldWidth = false;
        }

        public void BuildControls()
        {
            //BuildStore();
            //BuildTemplateHtml();
            BuildStore1();
            InitLogics();
        }

        public void BuildStore1()
        {
            
            JsonReader reader = new JsonReader();
            reader.Root = "data";
            reader.TotalProperty = "total";
            
            AjaxProxy proxy = new AjaxProxy();
            proxy.Url = ProxyUrl;
            proxy.Reader.Add(reader);
            proxy.ActionMethods.Read = HttpMethod.POST;
            proxy.ExtraParams.Add(new Ext.Net.Parameter { Name = "CurCompany", Value = CurCompany, Mode = ParameterMode.Value });

            Model model = new Model();
            foreach (LookupFormatData d in dataTemplate)
            {
                model.Fields.Add(new ModelField(d.FieldName, d.Type));
            }

            Store store = new Store();
            if (ProxyUrl != "")
            {
                store.Proxy.Add(proxy);
                store.Model.Add(model);
            }
            store.AutoLoad = false;

            this.Store.Add(store);

            if (ProxyUrl != "")
            {
                int width = 0;
                string strHeader = "";
                string strRows = "";
                foreach (LookupFormatData d in dataTemplate)
                {
                    width += d.ColWidth;
                    strHeader += String.Format(@"<th style =""font-weight: bold;padding: 3px;background: #3892d3"">{0}</th>", d.FieldTitle);
                    strRows += String.Format(@"<td style=""padding:3px;"">{{{0}}}</td>", d.FieldName);
                }

                this.ListConfig = new BoundList
                {
                    Width = width,
                    ItemSelector = ".x-boundlist-item",
                    Tpl = new XTemplate
                    {
                        Html = String.Format(@"
                    <Html>
					    <tpl for=""."">
						    <tpl if=""[xindex] == 1"">
							    <table class=""cbStates-list"">
								    <tr>
									{0}    
								    </tr>
						    </tpl>
						    <tr class=""x-boundlist-item"">
							    {1}    
						    </tr>
						    <tpl if=""[xcount-xindex]==0"">
							    </table>
						    </tpl>
					    </tpl>
				    </Html>", strHeader, strRows)
                    }
                };
            }
        }
        public void BuildStore()
        {
            JsonReader reader = new JsonReader
            {
                Root = "data",
                IDProperty = idProperty,
                TotalProperty = "total"
            };

            AjaxProxy proxy = new AjaxProxy
            {
                Url = ProxyUrl,
                Reader = { reader },
                ActionMethods = {
                    Read = HttpMethod.POST
                }
            };

            Model model = new Model();
            foreach (LookupFormatData d in dataTemplate)
            {
                model.Fields.Add(d.FieldName, d.Type);
            }

            this.Store.Add(new Ext.Net.Store
            {
                Proxy = { proxy },
                Model = { model },
                AutoLoad = false,
                RemotePaging = true,
                RemoteSort = true,
                WarningOnDirty = false
            });
        }

        public void BuildTemplateHtml()
        {
            int width = 0;
            string strHeader = "";
            string strRows = "";

            foreach (LookupFormatData d in this.dataTemplate)
            {
                if (d.ColWidth == 0) continue;
                width += d.ColWidth;
                strHeader = String.Format(@"<th style =""font-weight: bold;padding: 3px;"">{0}</th> ", d.FieldTitle);
                strRows += String.Format(@"<td style=""padding:3px;"">{{{0}}}</td> ", d.FieldName);
            }

            this.ListConfig = new BoundList
            {
                ItemTpl = new XTemplate
                {
                    Html = String.Format(@"
                                    <tpl for=""."">
                    				<tpl if=""[xindex] == 1"">
                    					<table style = ""width: {0}px;font: 11px tahoma,arial,helvetica,sans-serif;"">
                    						<tr>
                    							{1}
                    						</tr>
                    						</tpl>
                    						<tr class=""list-item"">
                    							{2}
                    						</tr>
                    						<tpl if=""[xcount-xindex]==0"">
                    					</table>
                    				</tpl>
                    			</tpl>", width, strHeader, strRows)
                }
            };
            this.ListConfig.Width = width;
            this.ListConfig.ItemSelector = ".x-boundlist-item";
            this.ListConfig.LoadingText = "Searching...";

        }

        public void InitLogics()
        {
            this.Listeners.Select.Fn = this.SCOPE + ".SelectRecord";
            this.Listeners.Select.Scope = this.SCOPE;

            this.Listeners.TriggerClick.Handler = this.SCOPE + ".OpenLookupWindow();";
        }
    }
}