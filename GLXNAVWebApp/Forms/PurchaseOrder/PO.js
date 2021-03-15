Ext.ns('GLX.FORMS');
GLX.FORMS.PO = {
    Search: function () {
        var fromDate = Ext.Date.format(Ext.getCmp('dfFromDate').getValue(), 'm/d/Y');
        var toDate = Ext.Date.format(Ext.getCmp('dfToDate').getValue(), 'm/d/Y');
        App.direct.BindingDataWithFilter(fromDate, toDate, {
            success: function () {
            },
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },

    Print: function () {
        var grid = Ext.getCmp('grdItems');
        var gridSelection = grid.getSelectionModel().getSelection()[0];
        if (!gridSelection) {
            Ext.Msg.show({
                title: 'Message',
                msg: 'Choose a record, please.',
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                }
            }); return;
        }
        var strKey = gridSelection.raw.Key;
        var docNo = gridSelection.raw.No;

        App.direct.Print(strKey, docNo, {
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Thông Báo', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },

    grdItemsDblClick: function () {
        this.Edit();
    },

    Edit: function () {
        var grid = Ext.getCmp('grdItems');
        var gridSelection = grid.getSelectionModel().getSelection()[0];
        if (!gridSelection) {
            Ext.Msg.show({
                title: 'Message',
                msg: 'Choose a record, please.',
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                }
            }); return;
        }
        var strKey = gridSelection.raw.Key;
        var docNo = gridSelection.raw.No;
        App.direct.LoadCard(strKey, docNo, "edit", {
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },

    OnKeyUp: function () {
        var me = this,
            v = me.getValue(),
            field;
        if (me.startDateField) {
            field = Ext.getCmp(me.startDateField);
            field.setMaxValue(v);
            me.dateRangeMax = v;
        } else if (me.endDateField) {
            field = Ext.getCmp(me.endDateField);
            field.setMinValue(v);
            me.dateRangeMin = v;
        }
        field.validate();
    }
}
GLX.FORMS.POEdit = {
    CalcDateTime: function (vDate) {
        //The getTimezoneOffset() method returns the time difference between UTC time and local time, in minutes.
        //For example, If your time zone is GMT+2, -120 will be returned.
        if (vDate == null) return vDate;
        var d = new Date();
        var offset = d.getTimezoneOffset() / 60;
        vDate.setHours(vDate.getHours() + offset);
        return vDate;
    },

    GetComp: function () {
        var str = Ext.util.Cookies.get('user_cookies');
        if (str != null)
            return str.split('&')[1].split('=')[1];
        else
            return "";
    },

    Save: function () {
        debugger;
        var CurCompany = GLX.FORMS.POEdit.GetComp();
        if (CurCompany == "")
        {
            Ext.Msg.show({ icon: Ext.MessageBox.ERROR, msg: 'Session timeout', buttons: Ext.Msg.OK });
            return;
        }

        var frm = Ext.getCmp("frmHeader");
        if (!frm.getForm().isValid()) return;

        var data = frm.getForm().getFieldValues(false, 'dataIndex');
        data.undefined = undefined;
        data.Document_Date = this.CalcDateTime(data.Document_Date);
        data.Posting_Date = this.CalcDateTime(data.Posting_Date);
        var values = Ext.encode(data);

        var grid = Ext.getCmp('grdLine');
        var store = grid.store;
        var detailModified = store.getChangedData();

        //Get modify + new records, new record is the last of object
        var obj = new Object();
        var newArray = new Array();
        var updateRecord = store.getUpdatedRecords();
        for (var i = 0; i < updateRecord.length; i++) {
            newArray.push(updateRecord[i].data);
        }

        var newRecord = store.getNewRecords();
        for (var i = 0; i < newRecord.length; i++) {
            newArray.push(newRecord[i].data);
        }
        if (newArray.length > 0)
            obj['Created'] = newArray;

        var detail = obj['Created'];

        Ext.net.DirectMethod.request({
            eventMask: {
                showMask: true
            },
            url: "../PurchaseOrder/SvcPurchaseOrder.asmx/UpdateData",
            cleanRequest: true,
            json: true,
            params: {
                HdrChangedData: values,
                dData: Ext.encode(detail),
                LineChangedData: Ext.encode(detailModified),
                CurCompany: CurCompany
            },
            success: this.successfunc,
            failure: function (error, response) {
                Ext.Msg.alert("Lỗi", error);
            }
        });
    },

    successfunc: function (result) {
        debugger;
        ////Ext.Msg.hide();
        //neu update co loi => bao loi, khong loi => reload data
        var ret = Ext.decode(result.Result) || [];
        if (!result.Success) {
            if (ret.control != 'grid') {
                Ext.Msg.show({
                    title: "Update Failed",
                    buttons: {
                        "yes": "OK"
                    },
                    fn: function (buttonId, text) {
                        switch (buttonId) {
                            case "yes":
                                var control = Ext.getCmp("txtBuy_from_Vendor_No");
                                control.focus(true, 100);
                                break;
                        }
                    },
                    icon: Ext.MessageBox.ERROR,
                    msg: result.ErrorMessage
                });
                return;
            }
            else {
                Ext.Msg.show({
                    title: "Update Failed",
                    buttons: {
                        "yes": "OK"
                    },
                    fn: function (buttonId, text) {
                        switch (buttonId) {
                            case "yes":
                                //grid.startEditing(1, 1);
                                break;
                        }
                    },
                    icon: Ext.MessageBox.ERROR,
                    msg: result.ErrorMessage
                });
                return;
            }
        }
        else {
            var record = Ext.decode(ret.record);
            GLX.FORMS.POEdit.ReloadData(record);
        }
    },

    ReloadData: function (record) {
        // Header
        if (record == undefined || record == null) return;
        Ext.getCmp('Key').setValue(record.Key);
        var CurCompany = GLX.FORMS.POEdit.GetComp();
        if (CurCompany == "") {
            Ext.Msg.show({ icon: Ext.MessageBox.ERROR, msg: 'Session timeout', buttons: Ext.Msg.OK });
            return;
        }
        // Lines
        var grid = Ext.getCmp('grdLine');
        grid.getStore().getProxy().extraParams = { DocumentType: '', DocumentNo: record.No, CurCompany: CurCompany };
        grid.getStore().load({
            callback: function (records, operation, success) {
                //if (!selected)
                //    grid.getSelectionModel().select({ row: 1, column: 1 }); //Focus lại sau khi di chuyển record
            }
        });
    },

    PostReceive: function () {
        var docNo = Ext.getCmp("txtNo").getValue();
        var CCompany = GLX.FORMS.POEdit.GetComp();
        if (CCompany == "") {
            Ext.Msg.show({ icon: Ext.MessageBox.ERROR, msg: 'Session timeout', buttons: Ext.Msg.OK });
            return;
        }
        Ext.net.DirectMethod.request({
            eventMask: {
                showMask: true
            },
            url: "../PurchaseOrder//SvcPurchaseOrder.asmx/PostReceive",
            cleanRequest: true,
            json: true,
            params: {
                docNo: docNo,
                CurCompany: CCompany
            },
            success: this.successfunc,
            failure: function (error, response) {
                Ext.Msg.alert("Lỗi", error);
            }
        });
    }
}