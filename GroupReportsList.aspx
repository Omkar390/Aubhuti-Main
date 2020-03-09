<%@ Page Language="VB" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="GroupReportsList.aspx.vb" Inherits="Users" title="Group Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        jQuery(function($) {
        })
        function edituser() {
            $("#edituser").show();
        }  
        function deluser() {
            $("#deluser").show();
        }  
    </script>
            
	<div class="ac_list_main">
        <asp:HiddenField id="hdngrpid" runat="server" />
    	<div class="heading_row topspace crasul_sec">
            <h2>Reports 
<%--                <asp:LinkButton ID="btnNew" PostBackUrl="~/GroupReportEdit.aspx?reportid=0" runat="server" Text="New" CssClass="new_btn"></asp:LinkButton>--%>
                <a class="new_btn" href="GroupReportEdit.aspx?reportid=0">New</a>
                <a class="new_btn" href="AdminMenu.aspx">Back</a>
            </h2>
            <div class="configure"><i class="fa fa-cog"></i>
                <div class="conf_opt">
                    <ul>
	                    <li><a href="#">Option 1</a></li>
                        <li><a href="#">Option 2</a></li>
                        <li><a href="#">Option 3</a></li>
                    </ul>
                </div>
            </div>
            <div class="clr"></div>
        </div>
        
        <div class="list_table"><!--table listing!--> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive" id="sort_tab">
            	<thead>
                  <tr>
                    <th width="19%"><span>Name</span></th>
                    <th width="17%"><span>Title</span></th>
                    <th width="11%"><span>Action</span></th>
                  </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptUsers" runat="server">
                        <ItemTemplate>
                            <tr>
                 	            <td><asp:literal ID="litRptName" runat="server" Text='<%#Eval("groupname")%>'></asp:literal></td>
                                <td><asp:literal ID="litTitle" runat="server" Text='<%#Eval("grouptitle")%>'></asp:literal></td>
                                <td><asp:LinkButton ID="btnuseredit" runat="server" CommandArgument='<%#Eval("groupid")%>' 
                                    CommandName='Edit'><img src="images/edit.png" /></asp:LinkButton>
                                    <asp:LinkButton ID="btnuserdel" runat="server" CommandArgument='<%#Eval("groupid")%>' 
                                    CommandName='Del'><img src="images/delete.png" /></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>                                                    
                </tbody>
            </table>
        </div>
    </div>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
                
    <section class='e_detailpop' id='deluser' style="display:none"><!--Email Detail Pop up!-->
	    <div class="create_edit">
    	    <div class="close_btn1"></div>
            <h1 class="dalete_text">Do you want to Delete this record?</h1>
            <div class="create_button">
                <input type="submit" id="btnDel" runat="server" value="Yes" class="ok" causesvalidation="false" />&nbsp;&nbsp; 
                <a class="ok" href="#" onclick='$("#deluser").hide();' >No</a></div>
            <br />
            <br />
        </div>
    </section><!--Email Detail Pop up!-->

</asp:Content>

