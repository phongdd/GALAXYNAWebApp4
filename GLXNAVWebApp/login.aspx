<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="GLXNAVWebApp.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign in - MS Dynamics NAV Tool</title>
    <meta charset="UTF-8"/>
    <link href="Resources/CSS/Login.css" rel="stylesheet" /> 
    <script src="Resources/JS/jquery-3.2.1.min.js"></script>
    <script type="text/javascript">
        window.onload = function () {
            var strOption = '<%=arrCompanies%>';
            var arrayOption = strOption.split(",");
            arrayOption.forEach(addOption);
        }
        function GoLogin() {
            var usn = document.getElementById("Username").value;
            var pwd = document.getElementById("Password").value;
            var com = document.getElementById("Companies").value;
            if (!com || !pwd || !usn) {
                Ext.Msg.show({
                    title: 'Message',
                    msg: 'All fields are required.',
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function (btn) {
                    }
                }); return;
            }
            var jvalue = '{username: "' + usn + '", password: "' + pwd + '", company: "' + com + '" }';

            $.ajax({
                type: "POST",
                url: "login.aspx/DoAuthentication",
                data: jvalue,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: OnFailure
            });
        }
        function OnSuccess(response) {
            if (response.d != "") {
                // Buttons: Ext.Msg.[
                //  8: CANCEL
                //  4: NO
                //  1: OK
                //  9: OKCANCEL
                //  2: YES
                //  6: YESNO
                //  14:YESNOCANCEL]
                // Icons: Ext.MessageBox.[ERROR|INFO|QUESTION|WARNING]
                Ext.Msg.show({
                    title: 'Message',
                    msg: response.d,
                    buttons: Ext.Msg.OK,
                    icon: Ext.MessageBox.WARNING,
                    fn: function (btn) {
                    }
                });

            } else {
                //window.location.href = "Forms/ReqWorksheet/Default.aspx";
                window.location.href = "Forms/PurchaseOrder/Default.aspx";
                //window.location.href = "Forms/Permission/Default.aspx";
                //window.location.href = "Forms/DataReview/Default.aspx";
            }
        }
        function OnFailure(response) {
            alert(response.d);
        }
        function addOption(item, index, arr) {
            var x = document.getElementById("Companies");
            var option = document.createElement("option");
            option.text = item;
            option.value = item;
            x.add(option);
        }
    </script>

</head>
<body>
    <ext:ResourceManager ID="ResourceManager1" runat="server" />
    <div class="wrapper">
	    <div class="container">
            <div class="titleWelcome">
		        <h2>WELCOME TO MICROSOFT DYNAMICS NAVISION TOOL</h2>
		    </div>
		    <form class="form">
                <input type="text" id="Username"  placeholder="Username" runat="server" />
			    <input type="password" id="Password" placeholder="Password" runat="server" />
				<select onclick="return false;" id="Companies" runat="server">
					<option value="">--Select your cinema--</option>
				</select>
                <input id="btnGetTime" type="button" value="Login" onclick = "GoLogin()" />
		    </form>
	    </div>
	    <ul class="bg-bubbles">
		    <li></li>
		    <li></li>
		    <li></li>
		    <li></li>
		    <li></li>
		    <li></li>
		    <li></li>
		    <li></li>
		    <li></li>
		    <li></li>
	    </ul>
    </div>
</body>
</html>
