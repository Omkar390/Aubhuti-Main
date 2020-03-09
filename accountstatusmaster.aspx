<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="accountstatusmaster.aspx.vb" Inherits="accountstatusmaster" title="Account Status Master" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function delconfirm() {
            if (confirm('Record will be deleted. Are you sure?')) {
                return true;
            }
            else {
                return false;
            }
        }

        function newtemplate() {
            document.getElementById("ctl00_ContentPlaceHolder1_pStName").value = '';
            document.getElementById("ctl00_ContentPlaceHolder1_hdnSelTargetID").value = '0';
            $("#edittemplate").show();
        }  

        function edittemplate() {
            $("#edittemplate").show();
        }  

    </script>

    <section class="container"><!--sales dashboard!-->
        <input type="hidden" id="hdnSelTargetID" runat="server" />
        <input type="button" id="btnMeetRefresh" runat="server" style="display:none" />
    	<div class="ac_list_main">
            <div class="heading_row topspace crasul_sec">
    	            <h2>Account - Status Master&nbsp
                        <a onclick='newtemplate();' href="#"> New</a>
                        <a class="new_btn" href="DataSetup.aspx">Back</a>
    	            </h2>
                    <div class="clr"></div>
            </div>
            <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="sort_tab">
            	    <thead>
                    <tr>
                        <th width="65%">Account Status</th>
                        <th width="15%">Created By</th>
                        <th width="10%">Created Date</th>
                        <th width="10%"></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptMR" runat="server">
                        <ItemTemplate>
                        <tr>
                            <td><%#Eval("stname")%></td>
                            <td><%#Eval("createdby")%></td>
                            <td><%#Eval("createdts")%></td>
                            <td>
                                <asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("atstrowid").ToString() + "|" + Eval("stname").ToString()%>' ID="TempEdit" runat="server"
                                Text="<img src='images/edit.png' />" ToolTip="Edit"></asp:LinkButton>
                                <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("atstrowid")%>' ID="TempDel" runat="server"
                                Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
                            </td>
                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                 </tbody>
                </table>
            </div><!--table listing!-->
        </div>
    </section><!--sales dashboard!-->
            
    <section class='e_detailpop' id='edittemplate'  style="display:none;height:100%"><!--Email Detail Pop up!-->
	    <div class="create_edit" style="width:50%">
    	        <div class="close_btn1"><img src="images/close.png"  style="cursor:pointer" onclick='$("#edittemplate").hide();' /></div>
            <h3 class="template_heading">Add / Edit Status</h3>
            <div class="create_form"><!--Create Edit form!-->
                   
       		<div class="create_row"><!--row !-->
       		    <div class="create_left" style="width:100%"><!--left section!-->
           	        <div class="field_name"><label>Account Status:</label></div>
                    <div class="inp_f">
                        <asp:TextBox ID="pStName" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvpStName" Display="Dynamic" ValidationGroup="pArea"
                            runat="server" ControlToValidate="pStName" InitialValue="">
                            <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                        </asp:RequiredFieldValidator>
                    </div>
                    <div class="clr"></div>
                </div><!--left section!-->
                <div class="clr"></div>
            </div>
            <br />
       		<div class="create_row"><!--row !-->
       		    <div class="create_left"><!--left section!-->
                    <div class="create_button">
                        <asp:LinkButton ID="lnkSaveList" CssClass="ok" runat="server" ValidationGroup="pArea" >Save</asp:LinkButton>
                    </div>
                    <div class="clr"></div>
                </div><!--left section!-->
                <div class="clr"></div>
            </div>


            </div><!--Create Edit form!-->
        </div>
    </section><!--Email Detail Pop up!-->

    <script type="text/javascript" >
    $(document).ready(function() {

        $('#sort_tab').DataTable({
            responsive: true,
	        bFilter: false,
	        bInfo: false,
            "language": {
	        "lengthMenu": "_MENU_ Show Records ",
            },
            'iDisplayLength': 100
        });
    });
    </script>
    
</asp:Content>

