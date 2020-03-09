<%@ Page Language="VB" AutoEventWireup="false" CodeFile="UserAccess.aspx.vb" Inherits="UserAccess" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,  initial-scale=1, maximum-scale=1, minimum-scale=1">
    <title>User Access</title>
    <link href='https://fonts.googleapis.com/css?family=Raleway:400,300,500,700,800' rel='stylesheet' type='text/css'>
    <link href='https://fonts.googleapis.com/css?family=Lato:400,300,700,900' rel='stylesheet' type='text/css'>
    <link href="css/custom.css" rel="stylesheet" type="text/css">
    <link href="css/font-awesome.css" rel="stylesheet" type="text/css">
</head>
<body class="login_bg">
    <form id="form1" runat="server">
        <section class="log_sec"><!--login section!-->
	        <div class="reg_con">
		        <h4>Welcome to SalesShark</h4>
	        </div>
            <div class="register_sec">
    	        <h4>What client do you want to access?</h4>
                <ul class="client_access">
                    <asp:Repeater ID="rptClients" runat="server">
                        <ItemTemplate>
        	                <li><asp:LinkButton ID="lnkclient" runat="server" CommandName="Lnk" CommandArgument='<%# Eval("client_id") %>' 
        	                    Text='<%# Eval("client_name") %>' ><i class="fa fa-user"></i></asp:LinkButton>
        	                    <asp:HiddenField runat="server" ID="hdnclientid" Value='<%#Eval("client_id")%>' />
        	                    <asp:HiddenField runat="server" ID="hdndbname" Value='<%#Eval("dbname")%>' />
        	                    <asp:HiddenField runat="server" ID="hdncGUID" Value='<%#Eval("GUID")%>' />
        	                    <asp:HiddenField runat="server" ID="hdnuserRole" Value='<%#Eval("userRole")%>' />
        	                </li>
                        </ItemTemplate>
                    </asp:Repeater>
                </ul>
            </div>
        </section><!--login section!-->
    </form>
</body>
</html>
