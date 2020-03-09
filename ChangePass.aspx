<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ChangePass.aspx.vb" Inherits="ChangePass" title="Change My Password" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
    function validatepage(){
        var validated = Page_ClientValidate('usermaint');
        if (!validated)
            return false;
        else
            return true;
    }
    </script>
    <section class="container"><!--container section!-->
	    <div class="top_hd"><!--top heading!-->
        	<h2>Change My Password</h2>
        </div><!--top heading!-->
        
        <div class="process_main"> 
        	<asp:Label ID="lblErrorMsg" runat="server" ForeColor="Red"></asp:Label>
            <div class="process_form"><!--process form section!-->
                <div class="process_left" >
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password" Style="width:95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Display="Static" ValidationGroup="usermaint"
                        runat="server" ControlToValidate="txtPassword" InitialValue="">
                        <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revtxtPassword" runat="server" ValidationGroup="usermaint"
                    ControlToValidate="txtPassword" Display="Static" >Invalid password</asp:RegularExpressionValidator>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->

          
            <div class="process_form"><!--process form section!-->
                <div class="process_left">
                    <label>Confirm Password</label>
                    <asp:TextBox ID="txtCnfPassword" runat="server" MaxLength="50" TextMode="Password" Style="width:95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtCnfPassword" Display="Static" ValidationGroup="usermaint"
                        runat="server" ControlToValidate="txtCnfPassword" InitialValue="">
                        <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revtxtCnfPassword" runat="server" ValidationGroup="usermaint"
                    ControlToValidate="txtCnfPassword" Display="Static" >Invalid password</asp:RegularExpressionValidator>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->

            <div class="create_button text_left">
                <asp:LinkButton ID="btnSaveSetting" runat="server" CssClass="ok" OnClientClick="return validatepage();" >Submit</asp:LinkButton>
                <a class="cancel" id="btnback" runat="server" href="SalesShark.aspx">Back</a>&nbsp;&nbsp; 
            </div>
            <br /><br />            
            <div class="process_form">
                <asp:Label id="lblMsg" runat="server"></asp:Label>
            </div>

         </div>
    </section><!--container section!-->
</asp:Content>


