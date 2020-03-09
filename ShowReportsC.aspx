<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ShowReportsC.aspx.vb" Inherits="ShowReportsC" title="All Reports" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

	<div class="ac_list_main">
        <asp:HiddenField id="hdnclientid" runat="server" />
    	<div class="heading_row space_new crasul_sec">
            	<h2>Reports 
                    <a href="SalesShark.aspx">Back</a>
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
        
        <div class="new_btab dataTables_wrapper"><!--table listing!--> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="sort_tab">
            	<thead>
                    <tr>
                        <th><span>Report Name</span></th>
                        <th><span>Type</span></th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptList" runat="server">
                        <ItemTemplate>
                            <tr>
                 	            <td><a href='<%#Eval("hrefLink")%>'><asp:literal ID="litclientname" runat="server" Text='<%#Eval("RptTitle")%>'></asp:literal></a></td>
                                <td><%#Eval("RptType")%></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
        </div><!--table listing!-->
    </div>

    
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

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

