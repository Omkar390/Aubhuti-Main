<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AccountContactMerge.aspx.vb" Inherits="AccountContactMerge" title="Account Contact Merge" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<%--ctl00_ContentPlaceHolder1_txtIEmailServer
ctl00_ContentPlaceHolder1_txtIPort
ctl00_ContentPlaceHolder1_txtISSL

ctl00_ContentPlaceHolder1_txtOEmailServer
ctl00_ContentPlaceHolder1_txtOPort
ctl00_ContentPlaceHolder1_txtOSSL--%>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

    <script type="text/javascript" src="js/jquery.maskedinput.js"></script>

    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {
            $("#ctl00_ContentPlaceHolder1_txtcozip").mask("?99999-9999");
            $("#ctl00_ContentPlaceHolder1_txtbillzip").mask("?99999-9999");
            $("#ctl00_ContentPlaceHolder1_txtcophoneno").mask("?999-999-9999");
        });



        function copyfromI() {
            if (document.getElementById("ctl00_ContentPlaceHolder1_chkUseSame").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_txtOEmailServer").value = document.getElementById("ctl00_ContentPlaceHolder1_txtIEmailServer").value;
                document.getElementById("ctl00_ContentPlaceHolder1_txtOPort").value = document.getElementById("ctl00_ContentPlaceHolder1_txtIPort").value;
                document.getElementById("ctl00_ContentPlaceHolder1_ddlOSSL").value = document.getElementById("ctl00_ContentPlaceHolder1_ddlISSL").value;
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_txtOEmailServer").value = '';
                document.getElementById("ctl00_ContentPlaceHolder1_txtOPort").value = '';
                document.getElementById("ctl00_ContentPlaceHolder1_ddlOSSL").value = 'Yes';
            }
            return false;
        }                
    </script>

    <section class="container"><!--container section!-->
	    <div class="top_hd"><!--top heading!-->
        	<h2>Merge Account / Contact</h2>
        </div><!--top heading!-->
        <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
        
        <div class="process_main"> 
            <div class="process_form"><!--process form section!-->
                <div class="process_left" style="width:100%">
                    <div style="float:left;width:15%">
                        <label>Merge ?</label>
                    </div>
                    <div style="float:left;width:10%">
                        <asp:DropDownList ID="ddlSelect" runat="server" TabIndex="19">
                            <asp:ListItem Value="Account"></asp:ListItem>
                            <asp:ListItem Value="Contact"></asp:ListItem>
                        </asp:DropDownList>                        
                    </div>
                </div>
            </div>

            <br /><br />
            <div class="clr"></div>
            <div class="process_form"><!--process form section!-->
                <div class="process_left" style="width:100%">
                    <div style="float:left;width:15%">
                        <label>Search</label>
                    </div>
                    <div style="float:left;width:35%">
                        <asp:TextBox ID="txtconame" runat="server" MaxLength="50" style="width:95%" TabIndex="1" ></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvtxtconame" Display="Dynamic" ValidationGroup="pCoInfo" 
                            runat="server" ControlToValidate="txtconame" InitialValue="">
                            <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="create_button text_left">
                        <a class="ok" id="btnSrch" runat="server">Search</a>
                        <a class="cancel" id="btnClear" runat="server">Clear</a>&nbsp;&nbsp; 
                    </div>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->

            <br />
            <div class="clr"></div>
            <div class="process_form"><!--process form section!-->
                <div class="process_left" style="width:100%">
                    <div class="create_button text_left">
                        <a class="ok" id="btnMerge" runat="server">Merge</a>
                    </div>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->
            
            <div class="process_form"><!--process form section!-->
                <div class="process_left" id="tblSrch" runat="server" style="width:100%">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="display nowrap ac_list" id="sort_tab">
			            <thead>
                            <tr>
                                <th>Master</th>
                                <th>Select to Merge</th>
                                <th>Account</th>
                                <th>Contact</th>
                            </tr>
			            </thead>
                        <asp:Repeater id="rptAcct" runat="server" >
                            <ItemTemplate>
                            <tr>
                                <td><center>
                                    <asp:RadioButton runat="server" ID="radMaster" GroupName="grpradMaster" AutoPostBack="true"
                                            OnCheckedChanged="radMaster_CheckedChanged" style="width:50%;height:50%" />
                                </center></td>
                                <td><center>
                                    <asp:CheckBox runat="server" ID="chkMerge" style="width:50%;height:50%"/>
                                </center></td>
                                <td><a href='<%#Eval("NavLink")%>'><%#Eval("AcctName")%></a><asp:Literal ID="litAcctId" runat="server" Visible="false" Text='<%#Eval("t_account_id")%>'></asp:Literal></td>
                                <td><a href='<%#Eval("NavLink")%>'><%#Eval("CtName")%><asp:Literal ID="litCtId" runat="server" Visible="false" Text='<%#Eval("t_contact_id")%>'></asp:Literal></td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                        <tr id="NoRecs" runat="server">
                            <td></td>
                            <td>No Records found</td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->

         </div>
    </section><!--container section!-->
</asp:Content>


