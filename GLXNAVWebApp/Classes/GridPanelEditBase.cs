using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Classes
{
    public class GridPanelEditBase : GridPanel
    {
        public string[] ValidateColumns { get; set; }
        public string SCOPE { get; set; }

        private List<LookupFormatData> dataTemplate;
        public List<LookupFormatData> DataTemplate
        {
            get
            {
                return dataTemplate;
            }
            set
            {
                dataTemplate = value;
                this.BuildGrid();
                this.BuildStore(true);
            }
        }

        private string columnExpand;
        public string ColumnExpand
        {
            get
            {
                return columnExpand;
            }
            set
            {
                columnExpand = value;
            }
        }

        public string ProxyUrl { get; set; }

        public string UIDg { get; set; }

        public string DocumentType { get; set; }

        public string DocumentNo { get; set; }

        public string CurCompany { get; set; }

        private ToolTip toolTip;

        //ReadOnly = True => all columns'grid are read only
        public bool ReadOnly { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        public void BuildGrid()
        {
            Ext.Net.GridView gridView;
            if (String.IsNullOrEmpty(this.columnExpand))
            {
                gridView = new Ext.Net.GridView();
            }
            else
            {
                gridView = new Ext.Net.GridView
                {
                    Configs = { ForceFit = true }
                };
            }

            if (ReadOnly == true)
            {
                foreach (LookupFormatData fmtData in this.dataTemplate)
                {
                    fmtData.ReadOnly = ReadOnly;
                }
            }

            foreach (LookupFormatData fmtData in this.dataTemplate)
            {
                fmtData.ID = fmtData.ID != "" ? fmtData.ID : fmtData.FieldName;
                if (fmtData.EditControl != null)
                {
                    Column colCbo = new Column
                    {
                        ID = String.Format("{0}ColID", fmtData.ID),
                        DataIndex = fmtData.FieldName,
                        Text = fmtData.FieldTitle,
                        Width = fmtData.ColWidth,
                        Hidden = fmtData.ColWidth == 0
                    };
                    if (String.Equals(fmtData.FieldName, this.columnExpand))
                        colCbo.Flex = 1;
                    //Makeup for option string
                    if (fmtData.IsRenderer)
                    {
                        string funcRenderer = fmtData.FnRenderer;
                        Ext.Net.Renderer rd = new Renderer { Fn = SCOPE + "." + funcRenderer };
                        colCbo.Renderer = rd;
                    }

                    if (!fmtData.ReadOnly)
                        colCbo.Editor.Add(fmtData.EditControl);
                    this.ColumnModel.Columns.Add(colCbo);
                }
                else if (fmtData.EditControl == null)
                {
                    switch (fmtData.Type)
                    {
                        case ModelFieldType.String:
                            #region String
                            Column colText = new Column
                            {
                                ID = String.Format("{0}ColID", fmtData.ID),
                                DataIndex = fmtData.FieldName,
                                Text = fmtData.FieldTitle,
                                Width = fmtData.ColWidth,
                                Hidden = fmtData.ColWidth == 0,
                                Align = Alignment.Left
                            };
                            if (String.Equals(fmtData.FieldName, this.columnExpand))
                                colText.Flex = 1;
                            if (fmtData.IsRenderer)
                            {
                                string funcRenderer = fmtData.FnRenderer;
                                Ext.Net.Renderer rd = new Renderer { Fn = SCOPE + "." + funcRenderer };
                                colText.Renderer = rd;
                            }
                            if (!fmtData.ReadOnly == true)
                            {
                                TextField textEditor = new TextField
                                {
                                    SelectOnFocus = true,
                                    //AllowBlank = false,
                                    //BlankText = "Not blank",
                                    MsgTarget = MessageTarget.Side
                                };

                                if (fmtData.MaxLength > 0)
                                {
                                    textEditor.MaxLength = fmtData.MaxLength;
                                    textEditor.MaxLengthText = String.Format("Chiều dài tối đa là {0} kí tự", textEditor.MaxLength);
                                }
                                if (fmtData.AllowBlank == false)
                                {
                                    textEditor.AllowBlank = false;
                                    textEditor.BlankText = String.Format("Trường bắt buộc nhập giá trị");
                                }

                                colText.Editor.Add(textEditor);
                            }

                            this.ColumnModel.Columns.Add(colText);
                            #endregion
                            break;
                        case ModelFieldType.Boolean:
                            #region Boolean
                            CheckColumn colBool = new CheckColumn
                            {
                                ID = String.Format("{0}ColID", fmtData.ID),
                                DataIndex = fmtData.FieldName,
                                Text = fmtData.FieldTitle,
                                Width = fmtData.ColWidth,
                                Hidden = fmtData.ColWidth == 0,
                                Editable = false,
                                Align = Alignment.Center
                            };
                            if (String.Equals(fmtData.FieldName, this.columnExpand))
                                colBool.Flex = 1;
                            if (!fmtData.ReadOnly == true)
                            {
                                colBool.Editable = true;
                                //TextField cbEditor = new TextField { SelectOnFocus = true };
                                //Ext.Net.Checkbox cbEditor = new Checkbox { Cls = "x-grid-checkheader-editor" };
                                //colBool.Editor.Add(cbEditor);
                            }
                            this.ColumnModel.Columns.Add(colBool);
                            #endregion
                            break;
                        case ModelFieldType.Date:
                            #region Date
                            DateColumn colDate = new DateColumn
                            {
                                ID = String.Format("{0}ColID", fmtData.ID),
                                DataIndex = fmtData.FieldName,
                                Text = fmtData.FieldTitle,
                                Width = fmtData.ColWidth,
                                Hidden = fmtData.ColWidth == 0,
                                Format = "dd/MM/yyyy",
                                Align = Alignment.Center
                            };
                            if (String.Equals(fmtData.FieldName, this.columnExpand))
                                colDate.Flex = 1;
                            if (!fmtData.ReadOnly == true)
                            {
                                DateField dateEditor = new DateField
                                {
                                    ID = String.Format("{0}_{1}_dateEditor", this.ClientID, fmtData.FieldName),
                                    Format = "dd/MM/yyyy",
                                    Vtype = "daterange",
                                    SelectOnFocus = true
                                };
                                colDate.Editor.Add(dateEditor);
                            }
                            //                            colDate.Renderer.Handler = @"if(value == 'Mon Jan 01 1 07:00:00 GMT+0700 (SE Asia Standard Time)')
                            //                                           {return null;}
                            //                                           else{return Ext.Date.format(value, 'd/m/Y');}";
                            this.ColumnModel.Columns.Add(colDate);
                            #endregion
                            break;
                        case ModelFieldType.Float:
                            #region Float
                            NumberColumn colFloat = new NumberColumn
                            {
                                ID = String.Format("{0}ColID", fmtData.ID),
                                DataIndex = fmtData.FieldName,
                                Text = fmtData.FieldTitle,
                                Width = fmtData.ColWidth,
                                Hidden = fmtData.ColWidth == 0,
                                Format = "0.00/i",
                                Align = Alignment.Right,
                            };
                            if (!String.IsNullOrEmpty(fmtData.Format)) colFloat.Format = fmtData.Format;
                            if (String.Equals(fmtData.FieldName, this.columnExpand)) colFloat.Flex = 1;
                            if (!fmtData.ReadOnly == true)
                            {
                                //phongdd-100
                                //TextField textEdit = new TextField { StyleSpec = "text-align:right;vertical-align:bottom", SelectOnFocus = true };
                                NumberField FloatEditInt = new NumberField
                                {
                                    StyleSpec = "text-align:right;vertical-align:bottom",
                                    SelectOnFocus = true,
                                    MinValue = 0,
                                    AllowDecimals = true,
                                    DecimalPrecision = 3
                                };
                                //end phongdd-100
                                colFloat.Editor.Add(FloatEditInt);
                            }
                            this.ColumnModel.Columns.Add(colFloat);
                            #endregion
                            break;
                        case ModelFieldType.Int:
                            #region Number
                            NumberColumn colNumber = new NumberColumn
                            {
                                ID = String.Format("{0}ColID", fmtData.ID),
                                DataIndex = fmtData.FieldName,
                                Text = fmtData.FieldTitle,
                                Width = fmtData.ColWidth,
                                Hidden = fmtData.ColWidth == 0,
                                Align = Alignment.Right,

                            };
                            if (!String.IsNullOrEmpty(fmtData.Format)) colNumber.Format = fmtData.Format;
                            if (String.Equals(fmtData.FieldName, this.columnExpand)) colNumber.Flex = 1;
                            //Bat buoc go dau phay
                            if (!fmtData.ReadOnly == true)
                            {
                                //phongdd-100
                                //TextField textEditInt = new TextField { StyleSpec = "text-align:right;vertical-align:bottom", SelectOnFocus = true };
                                NumberField textEditInt = new NumberField { StyleSpec = "text-align:right;vertical-align:bottom", SelectOnFocus = true, MinValue = 0, AllowDecimals = true };
                                //end phongdd-100
                                colNumber.Editor.Add(textEditInt);
                            }
                            this.ColumnModel.Columns.Add(colNumber);
                            #endregion
                            break;
                        default:
                            #region auto
                            Column colAuto = new Column
                            {
                                ID = String.Format("{0}ColID", fmtData.ID),
                                DataIndex = fmtData.FieldName,
                                Text = fmtData.FieldTitle,
                                Width = fmtData.ColWidth,
                                Hidden = fmtData.ColWidth == 0,
                                Align = Alignment.Left
                            };
                            if (String.Equals(fmtData.FieldName, this.columnExpand)) colAuto.Flex = 1;
                            if (fmtData.ReadOnly == true)
                            {
                                TextField textEditAuto = new TextField { StyleSpec = "text-align:right;vertical-align:bottom", SelectOnFocus = true };
                                colAuto.Editor.Add(textEditAuto);
                            }
                            this.ColumnModel.Columns.Add(colAuto);
                            #endregion
                            break;
                    }
                }
            }
            this.toolTip = new ToolTip
            {
                Delegate = ".x-grid3-cell",
                TrackMouse = true,
            };
            this.Plugins.Add(new CellEditing { ClicksToEdit = 1 });
            this.ToolTips.Add(toolTip);
            this.SelectionModel.Add(new CellSelectionModel { });
            this.View.Add(gridView);
        }

        public void BuildStore(bool isAutoLoad)
        {
            JsonReader reader = new JsonReader
            {
                Root = "d",
                TotalProperty = "total",
            };

            AjaxProxy proxy = new AjaxProxy
            {
                Url = ProxyUrl,
                Reader = { reader },
                Json = true,
                ActionMethods = {
                    Read = HttpMethod.POST
                },
                ExtraParams = {
                    new Ext.Net.Parameter{Name = "DocumentType", Value = DocumentType, Mode = ParameterMode.Value},
                    new Ext.Net.Parameter{Name = "DocumentNo", Value = DocumentNo, Mode = ParameterMode.Value},
                    new Ext.Net.Parameter{Name = "CurCompany", Value = CurCompany, Mode = ParameterMode.Value},
                }
            };

            Model model = new Model();
            foreach (LookupFormatData d in dataTemplate)
            {
                ModelField mf = d.Type == ModelFieldType.Auto ? new ModelField(d.FieldName) : new ModelField(d.FieldName, d.Type);
                if (d.Type == ModelFieldType.Date)
                {
                    mf.DateFormat = "M$";
                    //mf.Convert.Handler = @"if(value == '\/Date(-62135596800000)\/') { return null; }else { var date = new Date(parseInt(value.substr(6))); return date; } ";
                }
                model.Fields.Add(mf);
            }

            this.Store.Add(new Ext.Net.Store
            {
                Proxy = { proxy },
                Model = { model },
                AutoLoad = true,
                RemotePaging = true,
                WarningOnDirty = false
            });
        }
    }
}