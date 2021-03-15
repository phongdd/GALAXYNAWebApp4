using Ext.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GLXNAVWebApp.Forms.PurchaseOrder
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Form.Controls.Add(new ResourceManager() { ShowWarningOnAjaxFailure = false });
            this.Form.Controls.Add(new CtlPO());
        }
    }
}