<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SurveyList.aspx.vb" Inherits="SurveyList" title="Survey List" %>

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

    </script>

    <section class="container"><!--sales dashboard!-->

	    <div class="ac_list_main">
    	    <div class="heading_row topspace  crasul_sec"><!--top heading!-->
        	    <h2>Detail of Survey <a href="SurveyOverview.aspx?SurveyID=0" >New</a>
                <a href="SalesShark.aspx">Back</a></h2>
                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
            </div><!--top heading!-->
            <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="display nowrap ac_list" id="sort_tab" >
            	    <thead>
                    <tr>
                        <th><span>Survey</span></th>
                        <th><span>Type</span></th>
                        <th><span>Decription</span></th>
                        <th><span>Date Created</span></th>
                        <th><span>Created By</span></th>
                        <th><span>Active</span></th>
                        <th><span>Links</span></th>
                    </tr>
                    </thead>
                    <tbody>
                    <asp:Repeater ID="rptSurvey" runat="server">
                        <ItemTemplate>
                            <tr>
                                <asp:HiddenField ID="SurveyID" runat="server" />
                                <td><%#Eval("SurveyName")%></td>
                                <td><%#Eval("surveytype")%></td>
                                <td><%#Eval("Description")%></td>
                                <td><%#Eval("createdts")%></td>
                                <td><%#Eval("createdby")%></td>
                                <td><%#Eval("Active")%></td>
        	                    <td>
    	                            <asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("SurveyID")%>' ID="ActEdit" runat="server"
                                        Text="<img src='images/edit.png' />" ToolTip="Edit"></asp:LinkButton>
    	                            <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("SurveyID")%>' ID="ActDel" runat="server"
                                        Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
    	                            <asp:LinkButton CommandName="Pre" CommandArgument='<%#Eval("SurveyID")%>' ID="ActPre" runat="server"
                                        Text="Preview" ToolTip="Preview"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>                                            
                 </tbody>
                </table>
    			
            </div><!--table listing!-->
            
            
        </div>
    </section><!--sales dashboard!-->

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

