<%@ Page Language="VB" MasterPageFile="~/AccountMaster.master" AutoEventWireup="false" CodeFile="Account.aspx.vb" Inherits="Account" title="Account Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentMain" Runat="Server">

	<div class="heading_row crasul_sec">
		<h2 >Contacts <a onclick='editct();' href="#">New</a></h2>
    	<asp:Label ID="lblErrorMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
		<div class="clr"></div>
	</div>

	<div class="new_btab dataTables_wrapper">
		<table width="100%" border="0" cellspacing="0" cellpadding="0" class="display nowrap ac_list" id="sort_tab">
			<thead>
				<tr>
					<th>Category</th>
					<th>Contacts</th>
					<th>Title</th>
					<th>Persona</th>
					<th>Last</th>
				</tr>
			</thead>
			<tbody>
				<asp:Repeater ID="rptContacts" runat="server">
					<ItemTemplate>
					<tr>
						<td><%#Eval("arealevel")%></td>
						<td><a href='<%#Eval("NavLink")%>'><asp:literal ID="litcontact" runat="server" Text='<%#Eval("ContactName")%>'></asp:literal></a></td>
						<td><%#Eval("title")%></td>
						<td><%#Eval("PersonaDescription")%></td>
						<td><%#Eval("lastactivitydate")%></td>
					</tr>
					</ItemTemplate>
				</asp:Repeater>
			</tbody>
		</table>
	</div>
        
    <script type="text/javascript" >
        $(document).ready(function() {
            $('#sort_tab').DataTable({
                responsive: true,
                bFilter: false,
                bPaginate: false,
                order: [[ 4, "desc" ]],
	            bInfo: false,
                "language": {
	            "lengthMenu": "_MENU_ Show Records ",
	            "emptyTable": "No Contacts",
                },
                'iDisplayLength': 100
            });
        });
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPop" Runat="Server">
</asp:Content>

