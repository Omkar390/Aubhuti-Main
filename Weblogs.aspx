<%@ Page Language="VB" MasterPageFile="~/AccountMaster.master" AutoEventWireup="false" CodeFile="Weblogs.aspx.vb" Inherits="Weblogs" title="Weblogs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentMain" Runat="Server">

    <div class="heading_row crasul_sec">
    	<h2>Weblogs</h2>
    	<asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
		<div class="clr"></div>
    </div>

	<div class="new_btab dataTables_wrapper"  style="overflow:auto;max-height:350px">
		<table width="100%" border="0" cellspacing="0" cellpadding="0" class=" display nowrap ac_list" id="sort_tab">
			<thead>
				<tr>
					<th>Visit Date</th>
					<th>Browser</th>
					<th>Web Page</th>
					<th>IpAddress</th>
				</tr>
			</thead>

			<tbody>
				<asp:Repeater ID="rptWeblogs" runat="server">
					<ItemTemplate>
					<tr>
						<td valign="top"><%#Eval("VisitDate")%></td>
						<td valign="top"><%#Eval("BrowserName")%></td>
						<td valign="top"><%#Eval("WebPageName")%></td>
						<td valign="top"><%#Eval("IpAddress")%></td>
					</tr>
					</ItemTemplate>
				</asp:Repeater>
			</tbody>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPop" Runat="Server">
</asp:Content>

