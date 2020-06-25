<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="assignlist.aspx.vb" Inherits="assignlist" title="Survey List" %>

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
        	    <h2> Test Assigned to You <%--<a href="userlist.aspx?SurveyID=0" >New</a>--%>
                <%-- <a href="SalesShark.aspx">Back</a>--%></h2>
                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
            </div><!--top heading!-->
     <%-- <div class="new_btab dataTables_wrapper"><!--table listing!--> --%><br/><br/>

            <div class="new_btab dataTables_wrapper"  >
                   <div style="float:left"><!--table listing!--> 
            <%--<table width="30%" border="0" cellspacing="0" cellpadding="0" class="display nowrap ac_list" id="sort_tab" style="float:left">--%>
                           <table width="450%" border="4" cellspacing="0" cellpadding="0" class="display nowrap ac_list" id="sort_tab" >
            	    <thead>
                    <tr>
                        <%--<th><span>SurveyID</span></th>--%>
                         <th><span>User Name</span></th>
                     <%--   <th><span>Survey Name</span></th>--%>
                     <%--  <th><span>Survey Name</span></th>
                        <th><span>Survey Type</span></th> --%>                  
                     <%--   <th><span>userID</span></th>
                        <th><span>Description</span></th>
                        <th><span>Date Created</span></th>
                        <th><span>Created By</span></th>
                        <th><span>Active</span></th>--%>
                        <th><span>Links</span></th>
                       <%-- <th><span>Go to Survey Page</span></th>--%>
                    </tr>
                    </thead>                                
                    <tbody>
                    <asp:Repeater ID="rptSurvey" runat="server" >
                        <ItemTemplate>
                            <tr>
                                <asp:HiddenField ID="SurveyID" runat="server"/>     
                                 <td><%#Eval("UserName") %></td>
                                <%--<td><%#Eval("SurveyName")%></td>--%>
                               <%--   <td><%#Eval("surveytype")%></td> --%>                           
                              <%--  <td><%#Eval("userID") %></td>
                                <td><%#Eval("Description")%></td>
                                <td><%#Eval("createdts")%></td>
                                <td><%#Eval("createdby")%></td>
                                <td><%#Eval("Active")%></td>--%>
        	                    <td>
    	                                               
    	                           <%-- <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("SurveyID")%>' ID="ActDel" runat="server"
                                        Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>--%>
    	                            <asp:LinkButton CommandName="Pre" CommandArgument='<%#Eval("SurveyID")%>' ID="ActPre" runat="server"
                                        Text="Go To Survey Page" ToolTip="Preview"></asp:LinkButton>
                                        <%--  <asp:LinkButton CommandName="Remove" CommandArgument='<%#Eval("userID")%>' ID="ActEdit" runat="server"
                                        Text="<img src='images/minus.png' />" ToolTip="Edit"></asp:LinkButton>   --%>
                                </td>
                            </tr>
                        </ItemTemplate>                        
                    </asp:Repeater>                           
                 </tbody>                             
                </table>    
                              
            </div><!--table listing!--> 
            <%--    <div style="float:left">
                     <asp:Button ID="btnassign" runat="server" Text="Assign Test to User" /> <br />
                               <asp:Button ID="rmvtest" runat="server" Text="Remove Assigned Test"/> 
                </div>
                   --%>


            <!--Table 2 div tag starts from here-->               
             <%-- <div style="float:left" ><!--table listing!-->         
                <table width="40%" border="4" cellspacing="0" cellpadding="0" class="display nowrap ac_list" id="sort_tab1">
            	    <thead>
                    <tr>
                        <%--<th><span>SurveyID</span></th>--%>
                         <%--<th><span>User Name</span></th>--%>
                      <%-- <th><span>Survey Name</span></th>
                        <th><span>Survey Type</span></th>                   
               
                        <th><span>Links</span></th>
                    </tr>
                    </thead>
 
                    <tbody>
                    <asp:Repeater ID="Repeater1" runat="server" >
                        <ItemTemplate>
                            <tr>
                                <asp:HiddenField ID="SurveyID" runat="server"/>     
                                <%-- <td><%#Eval("UserName") %></td>--%>
                                <%-- <td><%#Eval("SurveyName")%></td>
                                 <td><%#Eval("surveytype")%></td>                            
                 
        	                    <td>
    	                            <asp:LinkButton CommandName="Add" CommandArgument='<%#Eval("SurveyID")%>' ID="ActEdit" runat="server"
                                        Text="<img src='images/plus.png' />" ToolTip="Edit"></asp:LinkButton>    	              
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>                                            
                 </tbody>
                </table>
    			
            </div--%>
               
            <!--Table 2 tag ends here-->
            
                </div>
            <!--both tables div tag ends here-->
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

			$('#sort_tab1').DataTable({
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

