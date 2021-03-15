Ext.ns('GLX.FORMS');
GLX.FORMS.ReqWorksheet2 = {
    Convert_ActionMessage_List: function (v, record) {
        switch (v) {
            case "_blank_":
                return " ";
                break;
            case "New":
                return "New";
                break;
            case "Change_Qty":
                return "Change Qty.";
                break;
            case "Reschedule":
                return "Reschedule";
                break;
            case "Resched__x0026__Chg_Qty":
                return "Resched. & Chg. Qty.";
                break;
            case "Cancel":
                return "Cancel";
                break;
        }
    },

    Convert_ReplenishmentSystem_List: function (v, record) {
        switch (v) {
            case "Purchase":
                return "Purchase";
                break;
            case "Prod_Order":
                return "Prod. Order";
                break;
            case "Transfer":
                return "Transfer";
                break;
            case "Assembly":
                return "Assembly";
                break;
            case "_blank_":
                return " ";
                break;
        }
    },

    LoadData: function (batch) {
        App.direct.BindingDataWithFilter(batch, {
            success: function () {

            },
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },

    cboBatchsChange: function (ele, newValue, oldValue) {
        GLX.FORMS.ReqWorksheet2.LoadData(newValue);
    },

    Carry_out_action_message: function () {
        App.direct.Carry_out_action_message({
            success: function (result) {
                if (result.Success == true) {
                } else {
                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                }
                var batch = Ext.getCmp('cboBatchs').getValue();
                GLX.FORMS.ReqWorksheet2.LoadData(batch);
            },
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    }

}

