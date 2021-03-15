Ext.ns('GLX.FORMS');
GLX.FORMS.Permission = {
    cboUsernameChange: function (ele, newValue, oldValue) {
        App.direct.BindingDataWithFilter(newValue, {
            success: function () {},
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },

    DelUser: function () {
        //debugger;
        var cboUsername = Ext.getCmp("cboUsername");
        if (!cboUsername.validate()) {
            Ext.Msg.alert('Error', 'All fields are required');
            return false;
        } else {
            var uname = cboUsername.getValue();
            App.direct.DelUser(uname, {
                success: function (result) {
                    if (result.Success == true) {
                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: 'Deleted!', buttons: Ext.Msg.OK });
                    } else {
                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                    }
                },
                failure: function (error, response) {
                    if (error != '')
                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                }
            });
        }
    

    },

    AddUser: function () {
        App.direct.AddUser({
            success: function (result) { },
            failure: function (error, response) { }
        });
    },
    DoAddUser: function () {
        debugger;
        //if (!#{txtUserName}.validate() )
        var UserName = Ext.getCmp("txtUserName").getValue();
        var FullName = Ext.getCmp("txtFullName").getValue();
        App.direct.DoAddUser(UserName, FullName, {
            success: function () { },
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    }
}