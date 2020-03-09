<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="UserPreference.aspx.vb" Inherits="UserPreference" title="User Preferences" %>
<%--<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>--%>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="ckeditor/samples/js/sample.js"></script>
   
    <script type="text/javascript">

    </script>

	<div class="top_hd"><!--top heading!-->
        <h2>User Preferences</h2>
    </div><!--top heading!-->
        
    <div class="process_main"> 
        <asp:Label ID="lblErrorMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        <asp:Label ID="lblSuccessMsg" runat="server" Visible="false" ForeColor="Green"></asp:Label>

        <div class="field_name"><label>Date Format</label></div>
        <div class="inp_f">
            <asp:DropDownList ID="ddlDtFormat" runat="server" Width="30%" TabIndex="2" CssClass="dropdown page_font_action">                
                <asp:ListItem Text="MM/DD/YYYY" Value="MM/DD/YYYY"></asp:ListItem>
                <asp:ListItem Text="MM-DD-YYYY" Value="MM-DD-YYYY"></asp:ListItem>
                <asp:ListItem Text="DD/MM/YYYY" Value="DD/MM/YYYY"></asp:ListItem>
                <asp:ListItem Text="DD-MM-YYYY" Value="DD-MM-YYYY"></asp:ListItem>
                <asp:ListItem Text="DD-MON-YYYY" Value="DD-MON-YYYY"></asp:ListItem>
                <asp:ListItem Text="YYYY-MM-DD" Value="YYYY-MM-DD"></asp:ListItem>                            
            </asp:DropDownList>

            <div class="clr"></div>
            <div class="clr"></div>
            <br />          
                        
            <div class="field_name"><label>Time Zone</label></div>
            
            <asp:DropDownList ID="ddlTimeZone" runat="server" Width="30%" TabIndex="3" CssClass="dropdown page_font_action">                                
                <asp:ListItem Value="EST">US - EST</asp:ListItem>
                <asp:ListItem Value="CST">US - CST</asp:ListItem>
                <asp:ListItem Value="MST">US - MST</asp:ListItem>
                <asp:ListItem Value="PST">US - PST</asp:ListItem>
                <asp:ListItem Value="BST">GB - BST</asp:ListItem>
            </asp:DropDownList>

            <br />
            <br />
            <br />
        </div>            
                        
        <div class="create_button text_left">
            <asp:LinkButton ID="btnSaveSetting" runat="server" CssClass="ok" >Submit</asp:LinkButton>
            <a class="cancel" id="btnback" runat="server">Back</a>&nbsp;&nbsp; 
        </div>
    </div>    

</asp:Content>


