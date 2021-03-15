using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GLXNAVWebApp.Classes
{
    public class LookupFormatData
    {
        public string Format { get; set; }

        private string fieldName = "";
        public string FieldName
        {
            get { return fieldName; }
            set { fieldName = value; }
        }

        private string fieldTitle = "";
        public string FieldTitle
        {
            get { return fieldTitle; }
            set { fieldTitle = value; }
        }

        private ModelFieldType type = ModelFieldType.String;
        public ModelFieldType Type
        {
            get { return type; }
            set { type = value; }
        }

        private int colWidth = 0;
        public int ColWidth
        {
            get { return colWidth; }
            set { colWidth = value; }
        }

        private Ext.Net.Field editControl = null;
        public Ext.Net.Field EditControl
        {
            get { return editControl; }
            set { editControl = value; }
        }

        private List<System.Web.UI.WebControls.WebControl> refControl = null;
        public List<System.Web.UI.WebControls.WebControl> RefContro
        {
            get { return refControl; }
            set { refControl = value; }
        }

        private bool alwaysChange = true;
        public bool AlwaysChange
        {
            get { return alwaysChange; }
            set { alwaysChange = value; }
        }

        private bool readOnly = false;
        public bool ReadOnly
        {
            get { return readOnly; }
            set { readOnly = value; }
        }

        private string id = "";
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private bool isRenderer = false;
        /// <summary>
        /// Sử dụng cho việc gọi function khi render column
        /// Function name: SCOPE + fnRenderer
        /// </summary>
        public bool IsRenderer
        {
            get { return isRenderer; }
            set { isRenderer = value; }
        }


        private string fnRenderer = string.Empty;
        /// <summary>
        /// Sử dụng cho việc gọi function khi render column
        /// Function name: SCOPE + fnRenderer
        /// </summary>
        public string FnRenderer
        {
            get { return fnRenderer; }
            set { fnRenderer = value; }
        }

        private bool allowBlank = true;
        public bool AllowBlank
        {
            get { return allowBlank; }
            set { allowBlank = value; }
        }

        private int maxLength = 0;
        public int MaxLength
        {
            get { return maxLength; }
            set { maxLength = value; }
        }
    }
}