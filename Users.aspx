<%@ Page Title="Clients" Language="VB" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="Users.aspx.vb" Inherits="Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function delconfirm() {
            if (confirm('Client will be marked as Inactive. Are you sure?')) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>

<section class="container"><!--sales dashboard!-->
            	
    <div class="ac_list_main"><!--sale process page!-->
    	<div class="heading_row topspace crasul_sec"><!--heading row!-->
            <h2>Users
                <a class="new_btn" href="UserMaint.aspx?userid=0">New</a>
                <a class="new_btn" href="AdminMenu.aspx">Back</a>
            </h2>
                
            <div class="clr"></div>
        </div>
                        
        <div class="list_table"><!--table listing!--> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive" id="sort_tab">
            	<thead>
                <tr>
                    <th>User</th>
                    <th>Email</th>
                    <%--<th>Last Login</th>--%>
                    <th>Email Validated</th>
                    <th>Active?</th>
                   	<th># of Clients</th>
                   	<th>Created On</th>
                    <th>Action</th>
                </tr>
                </thead>

                <tbody>
                <asp:Repeater ID="rptClients" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td><%#Eval("UserName")%></td>
                            <td><%#Eval("email")%></td>
                            <%--<td><%#Eval("LastLoginDate")%></td>--%>
                            <td><%#Eval("EmailValidated")%></td>
                            <td><%#Eval("Active")%></td>
                            <td><%#Eval("clientcnt")%></td>
                            <td><%#Eval("AddedDate")%></td>
                            <td>
                                <asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("userID")%>' ID="TempEdit" runat="server"
                                Text="<img src='images/edit.png' />" ToolTip="Edit"></asp:LinkButton>
                                <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("userID")%>' ID="TempDel" runat="server"
                                Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
                                <%--<asp:HiddenField  ID="hdnclientid" Value='<%#Eval("client_id")%>' runat="server" />--%>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </tbody>
            </table>
        </div><!--table listing!-->
    </div><!--sale process page!-->
    
</section><!--sales dashboard!-->

    <script type="text/javascript" >
        $(document).ready(function() {

            $('#sort_tab').DataTable({
                responsive: true,
	            bFilter: false,
	            order: [[0, "asc"]],
	            bInfo: false,
                "language": {
	            "lengthMenu": "_MENU_ Show Records ",
                },
                'iDisplayLength': 100
            });
        });
    </script>

</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

