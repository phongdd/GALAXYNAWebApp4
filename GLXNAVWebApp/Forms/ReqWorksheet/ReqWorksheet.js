Ext.ns('GLX.FORMS');
GLX.FORMS.ReqWorksheet = {
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
     
    AddNew: function () {       
        var batch = Ext.getCmp("cboBatchs").getValue();
        if (!batch) {
            Ext.Msg.show({
                title: 'Message',
                msg: 'Choose a batch, please.',
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                }
            }); return;
        }
        App.direct.LoadCard('new', batch, {
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
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
        var strKey = gridSelection.raw.UID;
        var converted = gridSelection.raw.Converted;
        var batch = Ext.getCmp("cboBatchs").getValue();
        
        App.direct.LoadCard('edit', gridSelection.raw, {
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },
    Delete: function () {
        var keyValue = GLX.FORMS.ReqWorksheet.GetCurrentKey();
        if (keyValue == '') {
            Ext.Msg.show({
                title: 'Message',
                msg: 'Choose a record, please.',
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                }
            }); return;
        } else {
            GLX.FORMS.ReqWorksheetEdit.DeleteAction(keyValue, false);
        }
    },

    //Approve
    Approve: function (type) {
        Ext.MessageBox.confirm('Microsoft Dynamics NAV', 'Do you want to approve?', function (btn) {
            if (btn == 'yes') {
                var status = 1;
                if (type != 'CM') status = 2;
                var keyValue = GLX.FORMS.ReqWorksheet.GetCurrentKey();
                if (keyValue == '') {
                    Ext.Msg.show({
                        title: 'Message',
                        msg: 'Choose a record, please.',
                        buttons: Ext.Msg.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) {
                        }
                    }); return;
                } else {
                    App.direct.Approve(keyValue, status, {
                        success: function (result) {
                            if (result.Success == true) {
                                if (result.Result != "")
                                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                            }
                            GLX.FORMS.ReqWorksheet.Reload();
                        },
                        failure: function (error, response) {
                            if (error != "")
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                            GLX.FORMS.ReqWorksheet.Reload();
                        }
                    });
                }
            }
        });
    },
    CMApprove: function () {
        GLX.FORMS.ReqWorksheet.Approve('CM');
    },
    FnBApprove: function () {
        GLX.FORMS.ReqWorksheet.Approve('FNB');
    },

    //Reopen
    Reopen: function (type) {
        Ext.MessageBox.confirm('Microsoft Dynamics NAV', 'Do you want to reopen?', function (btn) {
            if (btn == 'yes') {
                var keyValue = GLX.FORMS.ReqWorksheet.GetCurrentKey();
                if (keyValue == '') {
                    Ext.Msg.show({
                        title: 'Message',
                        msg: 'Choose a record, please.',
                        buttons: Ext.Msg.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) {
                        }
                    }); return;
                } else {
                    App.direct.Reopen(keyValue, type, {
                        success: function (result) {
                            if (result.Success == true) {
                                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                            }
                            GLX.FORMS.ReqWorksheet.Reload();
                        },
                        failure: function (error, response) {
                            if (error != "")
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                            GLX.FORMS.ReqWorksheet.Reload();
                        }
                    });
                }
            }
        });
    },
    FnBReopen: function () {
        GLX.FORMS.ReqWorksheet.Reopen(2);
    },
    CMReopen: function () {
        GLX.FORMS.ReqWorksheet.Reopen(1);
    },

    SplitRequest: function () {
        var keyValue = GLX.FORMS.ReqWorksheet.GetCurrentKey();
        if (keyValue == '') {
            Ext.Msg.show({
                title: 'Message',
                msg: 'Choose a record, please.',
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                }
            }); return;
        } else {
            App.direct.SplitRequest(keyValue, {
                success: function (result) {
                    if (result.Success == true) {
                        //var ret = Ext.decode(result.Result) || [];
                        if (ret.Message != "")
                            Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    } else {
                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                    }
                    GLX.FORMS.ReqWorksheet.Reload();
                },
                failure: function (error, response) {
                    if (error != "")
                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                    GLX.FORMS.ReqWorksheet.Reload();
                }
            });
        }
    },
    ConvertToNAV: function () {
        Ext.MessageBox.confirm('Microsoft Dynamics NAV', 'Are you sure?', function (btn) {
            if (btn == 'yes') {
                var keyValue = GLX.FORMS.ReqWorksheet.GetCurrentKey();
                if (keyValue == '') {
                    Ext.Msg.show({
                        title: 'Message',
                        msg: 'Choose a record, please.',
                        buttons: Ext.Msg.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) {
                        }
                    }); return;
                } else {
                    App.direct.ConvertToNAV(keyValue, {
                        success: function (result) {
                            if (result.Success == true) {
                                //var ret = Ext.decode(result.Result) || [];
                                if (ret.Message != "")
                                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                            }
                            GLX.FORMS.ReqWorksheet.Reload();
                        },
                        failure: function (error, response) {
                            if (error != "")
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                            GLX.FORMS.ReqWorksheet.Reload();
                        }
                    });
                }
            }
        });
    },
    ConvertToNAVItemReclassJournal: function () {
        Ext.MessageBox.confirm('Microsoft Dynamics NAV', 'Are you sure?', function (btn) {
            if (btn == 'yes') {
                var keyValue = GLX.FORMS.ReqWorksheet.GetCurrentKey();
                if (keyValue == '') {
                    Ext.Msg.show({
                        title: 'Message',
                        msg: 'Choose a record, please.',
                        buttons: Ext.Msg.OK,
                        icon: Ext.MessageBox.WARNING,
                        fn: function (btn) {
                        }
                    }); return;
                } else {
                    App.direct.ConvertToNAVItemReclassJournal(keyValue, {
                        success: function (result) {
                            if (result.Success == true) {
                                //var ret = Ext.decode(result.Result) || [];
                                if (ret.Message != "")
                                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                            }
                            GLX.FORMS.ReqWorksheet.Reload();
                        },
                        failure: function (error, response) {
                            if (error != "")
                                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                            GLX.FORMS.ReqWorksheet.Reload();
                        }
                    });
                }
            }
        });

    },

    GetCurrentKey: function () {
        //var grid = Ext.getCmp('grdItems');
        //var gridSelection = grid.getSelectionModel().getSelection()[0];
        //var keyValue = '';
        //if (gridSelection)
        //    keyValue = gridSelection.raw.UID;
        //return keyValue;

        var grid = Ext.getCmp('grdItems');
        var gridSelection = grid.getSelectionModel().getSelection();
        var keyValue = '';
        for (x of gridSelection) {
            keyValue += x.raw.UID;
            keyValue += ';';
        }
        return keyValue;
    },

    AddComment: function () {
        var keyValue = GLX.FORMS.ReqWorksheet.GetCurrentKey();
        if (keyValue == '') {
            Ext.Msg.show({
                title: 'Message',
                msg: 'Choose a record, please.',
                buttons: Ext.Msg.OK,
                icon: Ext.MessageBox.WARNING,
                fn: function (btn) {
                }
            }); return;
        } else {
            App.direct.AddComment(keyValue, {
                success: function (result) {
                    if (result.Success == true) { }
                },
                failure: function (error, response) { }
            });
        }
    },
    DoAddComment: function () {
        var C_uid = Ext.getCmp("Comment_HiddenUID"),
            C_cmtText = Ext.getCmp("TextAreaChangeLog");

        if (C_uid !== undefined)
            var uid = C_uid.value;
        if (C_cmtText !== undefined)
            var cmtText = C_cmtText.value;
        App.direct.DoAddComment(uid, cmtText, {
            success: function (result) {
                if (result.Success == true) { }
            },
            failure: function (error, response) { }
        });
    },
    ShowLog: function () {
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
        var ChangeLog = gridSelection.raw.ChangeLog;
        App.direct.ShowLog(ChangeLog, {
            success: function (result) {
                
            },
            failure: function (error, response) {
               
            }
        });

    },
    ExportExcel: function () {
        var batch = Ext.getCmp("cboBatchs").getValue();
        var dfFromDate = Ext.getCmp("dfFromDate").getValue();
        var dfToDate = Ext.getCmp("dfToDate").getValue();
        
        App.direct.ExportExcel(batch, dfFromDate, dfToDate, {
            isUpload: true,
            buffer: 300,
            success: function (result) {
                if (result != '') {
                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: result, buttons: Ext.Msg.OK });
                }
                setTimeout(function () {
                }, 2000);
            },
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
        
    },

    submitValue: function (grid, hiddenFormat, format) {
        hiddenFormat.setValue(format);
        grid.submitData(false, { isUpload: true });
    },

    //setTimeout(function () {
    //}, 2000);

    Reload: function () {
        var batch = Ext.getCmp("cboBatchs").getValue();
        var dfFromDate = Ext.getCmp("dfFromDate").getValue();
        var dfToDate = Ext.getCmp("dfToDate").getValue();
        var status = Ext.getCmp("cboStatus").getValue();
        App.direct.BindingDataWithFilter(batch, dfFromDate, dfToDate, status, {
            success: function () {
                
            },
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },
    grdItemsDblClick: function () {
        this.Edit();
    },
    cboBatchsChange: function (ele, newValue, oldValue) {
        var dfFromDate = Ext.getCmp("dfFromDate").getValue();
        var dfToDate = Ext.getCmp("dfToDate").getValue();
        var status = Ext.getCmp("cboStatus").getValue();
        App.direct.BindingDataWithFilter(newValue, dfFromDate, dfToDate, status, {
            success: function () {

            },
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },
    DateChanged: function () {
        //var fromDate = Ext.Date.format(Ext.getCmp('dfFromDate').getValue(), 'm/d/Y');
        //var toDate = Ext.Date.format(Ext.getCmp('dfToDate').getValue(), 'm/d/Y');
        var dfFromDate = Ext.getCmp("dfFromDate").getValue();
        var dfToDate = Ext.getCmp("dfToDate").getValue();
        var batch = Ext.getCmp('cboBatchs').getValue();
        var status = Ext.getCmp("cboStatus").getValue();
        App.direct.BindingDataWithFilter(batch, dfFromDate, dfToDate, status, {
            success: function () {
                var grdItems = Ext.getCmp('grdItems');
                grdItems.store.clearFilter();
            },
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },
    cboStatusChange: function (ele, newValue, oldValue) {
        var dfFromDate = Ext.getCmp("dfFromDate").getValue();
        var dfToDate = Ext.getCmp("dfToDate").getValue();
        var batch = Ext.getCmp("cboBatchs").getValue();
        App.direct.BindingDataWithFilter(batch, dfFromDate, dfToDate, newValue, {
            success: function () {
                var grdItems = Ext.getCmp('grdItems');
                grdItems.store.clearFilter();
            },
            failure: function (error, response) {
                if (error != "")
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
    },
    SetItemNo: function (v) {
        App.direct.GLXSetCookie(v, {
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    }
}

GLX.FORMS.ReqWorksheetEdit = {
    QuantityBlur: function (el, ev) {
        el.setValue(Ext.util.Format.number(el.getValue().replace(/[,]/g, ''), '0,0.00'));
    },
    QuantityFocus: function (el, The) {
        el.setValue(el.getValue().replace(/[,]/g, ''));
    },
    Direct_Unit_CostChange: function (el, newValue, oldValue) {
        el.setValue(Ext.util.Format.number(newValue.replace(/[,]/g, ''), '0,0.00'));
    },
    GetComp: function () {
        debugger;
        var str = Ext.util.Cookies.get('user_cookies');
        return str.split('&')[1].split('=')[1];
    },
    CalcDateTime: function (vDate) {
        //The getTimezoneOffset() method returns the time difference between UTC time and local time, in minutes.
        //For example, If your time zone is GMT+2, -120 will be returned.
        if (vDate == null) return vDate;
        var d = new Date();
        var offset = d.getTimezoneOffset() / 60;
        vDate.setHours(vDate.getHours() + offset);
        return vDate;
    },
    Save: function () {
        var frm = Ext.getCmp("frmHeader");
        var data1 = frm.getForm().getValues();
        var data = frm.getForm().getFieldValues(false, 'dataIndex');
        data.undefined = undefined;
        //data.Due_Date = this.CalcDateTime(data.Due_Date);
        var values = Ext.encode(data);
        var batch = Ext.getCmp("txtBatch").getValue();    //data.Batch;
        var successfunc = null;
        if (option != null)
            successfunc = option['success'];
        else
            successfunc = function (result) {
                ////
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
                                        var control = Ext.getCmp("cboSell_to_Customer_No");
                                        switch (ret.control) {
                                            case 'cboSell_to_Customer_No':
                                                control.focus(true, 100);
                                                break;
                                        }
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
                                        grid.startEditing(1, 1);
                                        break;
                                }
                            },
                            icon: Ext.MessageBox.ERROR,
                            msg: "Some errors at line."
                        });
                        return;
                    }
                }
                else {
                    //Insert Request Order to Service Order if save successfully
                    var record = Ext.decode(ret.record);
                    API.FORMS.ProcessStockOut.ReloadData(record);
                    //API.General.CallServiceFn('AddRequestToServiceOrder', record.Account_ID, record.Document_Type, record.No, []);
                }
            };

        App.direct.UpdateData2(batch, values, {
            success: function (result) {
                if (result.Success == true) {
                    var ret = Ext.decode(result.Result) || [];
                    if (ret.Message != "")
                        Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: ret.Message, buttons: Ext.Msg.OK });
                } else {
                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                }
                GLX.FORMS.ReqWorksheet.Reload();
            },
            failure: function (error, response) {
                if (error != "")
                    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                GLX.FORMS.ReqWorksheet.Reload();
            }
        });
        /*
        Ext.net.DirectMethod.request({
            eventMask: {
                showMask: true
            },
            url: "..//..//..//..//ReqWorksheet//SvcReqWorksheet.asmx/UpdateData",
            cleanRequest: true,
            json: true,
            params: {
                Batch: batch,
                HdrChangedData: values,
                CurCompany: GLX.FORMS.ReqWorksheetEdit.GetComp(),
            },
            success: successfunc,
            failure: function (error, response) {
                Ext.Msg.alert("Lỗi", error);
            }
        });
        */
    },
    Delete: function () {
        var keyValue = Ext.getCmp('txtKey').getValue();
        GLX.FORMS.ReqWorksheetEdit.DeleteAction(keyValue, true);
    },
    DeleteAction: function (keyValue, hasCard) {
        Ext.MessageBox.confirm('Microsoft Dynamics NAV', 'Do you want to delete the selected line or lines?', function (btn) {
            if (btn == 'yes') {
                App.direct.TobeDeleted(keyValue, {
                    success: function (result) {
                        if (result.Success == true) {
                            //var ret = Ext.decode(result.Result) || [];
                            //if (ret.Message != "")
                            //    Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: ret.Message, buttons: Ext.Msg.OK });
                            //else {
                            //    var win = Ext.getCmp('winCard');
                            //    if (win != undefined || win != null)
                            //        win.close();
                            //}
                            var win = Ext.getCmp('winCard');
                            if (win != undefined || win != null)
                                win.close();
                        } else {
                            Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.WARNING, msg: result.ErrorMessage, buttons: Ext.Msg.OK });
                        }
                        GLX.FORMS.ReqWorksheet.Reload();
                    },
                    failure: function (error, response) {
                        if (error != "")
                            Ext.Msg.show({ title: 'Message', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
                        GLX.FORMS.ReqWorksheet.Reload();
                    }
                });
                /*
                Ext.net.DirectMethod.request({
                    url: "..//..//..//..//ReqWorksheet//SvcReqWorksheet.asmx/DeleteData",
                    cleanRequest: true,
                    json: true,
                    params: {
                        key: keyValue,
                        CurCompany: GLX.FORMS.ReqWorksheetEdit.GetComp()
                    },
                    success: function (result) {
                        if (result.Result == 'Completed with error') {
                            Ext.Msg.show({
                                title: 'Thông Báo',
                                icon: Ext.MessageBox.WARNING,
                                msg: result.ErrorMessage,
                                buttons: Ext.Msg.OK
                            });
                        } else {
                            if (hasCard == true) {
                                var win = Ext.getCmp('winCard');
                                if (win != undefined || win != null)
                                    win.close();
                            } else {
                                GLX.FORMS.ReqWorksheet.Reload();
                            }
                        }
                    },
                    failure: function (error, response) {
                        Ext.Msg.alert("Lỗi", error);
                    }
                });
                */
            }
        });
    },
    BeforeCloseWindow: function () {
        GLX.FORMS.ReqWorksheet.Reload();
    },
    NoChange: function (c) {
        //GLX.FORMS.ReqWorksheet.SetItemNo("ABC123");
    },
}

Ext.ns('GLX.Lookup');
GLX.Lookup.ItemNo = {
    SelectRecord: function (combo, record, index) {
        if (combo != null) {
            combo.setValue(record[0].data.No);
        }
        var txtDescription = Ext.getCmp('txtDescription');
        txtDescription.setValue(record[0].data.Description);
        var cboUnit_of_Measure_Code = Ext.getCmp('cboUnit_of_Measure_Code');
        cboUnit_of_Measure_Code.setValue(record[0].data.Base_Unit_of_Measure);
        var txtDirect_Unit_Cost = Ext.getCmp('txtDirect_Unit_Cost');
        txtDirect_Unit_Cost.setValue(record[0].data.Unit_Cost);

        //var cboVendor_No = Ext.getCmp('cboVendor_No');
        //cboVendor_No.setValue('');
        //var txtVendor_Item_No = Ext.getCmp('txtVendor_Item_No');
        //txtVendor_Item_No.setValue('');
        var itemNo = record[0].data.No;
        GLX.FORMS.ReqWorksheet.SetItemNo(itemNo);
        App.direct.LoadUOMContext(itemNo, {
            success: function (result) {
            },
            failure: function (error, response) {
                Ext.Msg.show({ title: 'Báo lỗi', icon: Ext.MessageBox.ERROR, msg: error, buttons: Ext.Msg.OK });
            }
        });
    },
    OpenLookupWindow: function () {
        //var title = 'Items';
        //var url = '../../../Lookup/LkpCustomerItems.aspx?SelectMethod=API.Lookup.ItemNo.SelectRecord';
        //App.direct.OpenLookupWindow(title, url, {
        //    failure: function (error, response) {
        //    }
        //});
    }
};

GLX.Lookup.Location = {
    SelectRecord: function (combo, record, index) {
        if (combo != null) {
            combo.setValue(record[0].data.Code);
        }
    },
};

GLX.Lookup.ItemVendorCatalog = {
    SelectRecord: function (combo, record, index) {
        if (combo != null) {
            combo.setValue(record[0].data.Vendor_No);
        }
        var txtVendor_Item_No = Ext.getCmp('txtVendor_Item_No');
        txtVendor_Item_No.setValue(record[0].data.Vendor_Item_No);
    },
};

GLX.Lookup.UOM = {
    SelectRecord: function (combo, record, index) {
        if (combo != null) {
            combo.setValue(record[0].data.Code);
        }
    },

}

Ext.ns("API.Lookup");
API.Lookup.EntityCode = {
    SelectRecord: function (combo, record, index) {
        Ext.getCmp('cboma_kh').setValue(record[0].data.ma_kh);
        Ext.getCmp('txtten_kh').setValue(record[0].data.ten_kh);
        Ext.getCmp('txtnguoi_daidien').setValue(record[0].data.ten_kh);
        Ext.getCmp('txtdiachi').setValue(record[0].data.diachi);
        Ext.getCmp('txtmaso_thue').setValue(record[0].data.maso_thue);
        Ext.getCmp('txtsotk_nh').setValue(record[0].data.sotk_nh);
    },
    OpenLookupWindow: function () {
        var title = "Entity Code";
        var url = '../../../Lookup/Lkp_DimensionValueList.aspx?SelectMethod=API.Lookup.EntityCode.SelectRecord';
        App.direct.OpenLookupWindow(title, url, {
            failure: function (error, response) {
            }
        });
    }
}