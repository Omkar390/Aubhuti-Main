﻿<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="showresult.aspx.vb" Inherits="showresult" title="Survey List" %>

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
        	    <h2>Detail of Survey <%--<a href="userlist.aspx?userID=0" >New</a>--%>
                <a href="Anubhuti.aspx">Back</a></h2>
                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
            </div><!--top heading!-->
            <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="display nowrap ac_list" id="sort_tab" >
            	    <thead>
                    <tr>
                        <%--<th><span>SurveyID</span></th>--%>
                         <th><span>User Name</span></th>
                        <th><span>Survey Question</span></th>
                        <th><span>Answer Type</span></th>
                        <th><span>answer</span></th>
                      <%--  <th><span>Survey Type</span></th> --%>                      
                     <%--   <th><span>userID</span></th>
                        <th><span>Description</span></th>
                        <th><span>Date Created</span></th>
                        <th><span>Created By</span></th>--%>
              <%--          <th><span>IS Completed</span></th>--%>
                      <%--  <th><span>Links</span></th>--%>
                    </tr>
                    </thead>
                    <tbody>
                    <asp:Repeater ID="rptSurvey1" runat="server">
                        <ItemTemplate>
                            <tr>
                                <asp:HiddenField ID="SurveyID" runat="server" />     
                                 <td><%#Eval("UserName") %></td>
                                <td><%#Eval("Surveyquestion")%></td>
                                 <td><%#Eval("Answertype")%></td>
                                 <td><%#Eval("answer")%></td>
                             <%--   <td><%#Eval("surveytype")%></td>    --%>                           
                              <%--  <td><%#Eval("userID") %></td>
                                <td><%#Eval("Description")%></td>
                                <td><%#Eval("createdts")%></td>
                                <td><%#Eval("createdby")%></td>--%>
                              <%--  <td><%#Eval("iscompleted")%></td>--%>
        	                    <td>
    	                            
    	                           <%-- <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("SurveyID")%>' ID="ActDel" runat="server"
                                        Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
    	                            <asp:LinkButton CommandName="Pre" CommandArgument='<%#Eval("SurveyID")%>' ID="ActPre" runat="server"
                                        Text="Preview" ToolTip="Preview"></asp:LinkButton>--%>
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

