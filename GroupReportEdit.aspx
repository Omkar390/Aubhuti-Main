<%@ Page Language="VB" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="GroupReportEdit.aspx.vb" Inherits="ReportEdit" title="Group Report Edit" %>

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

        function addtolist() {
            document.getElementById("ctl00_ContentPlaceHolder1_divselusers").style.display = 'none' ;
            $("#editninja").show();
        }

        function showlist() {
            document.getElementById("ctl00_ContentPlaceHolder1_divselusers").style.display = 'block';
            $("#editninja").show();
        }

    </script>

    <section class="container"><!--container section!-->
	    <div class="heading_row topspace crasul_sec"><!--top heading!-->
        	<h2>Group Report Edit <asp:Literal runat="server" id="lblRpTName"></asp:Literal>
                <a class="new_btn" href="GroupReportsList.aspx">Back</a>&nbsp;&nbsp;&nbsp;&nbsp;
                <a class="new_btn" onclick='addtolist();'>Add Report</a>
        	</h2>
            <div class="clr"></div>
        </div><!--top heading!-->
        <asp:HiddenField ID="hdnreportid" runat="server" />
        <asp:Label ForeColor="Green" runat="server" ID="lblMsg"></asp:Label>
        <asp:Label ForeColor="Red" runat="server" ID="lblErrMsg"></asp:Label>
        
        <div class="process_main"> 
            <div class="process_form"><!--process form section!-->
                <table>
                    <tr>
                        <td style="width:30%">
                            <label>Report Name (No spaces)</label>
                        </td>
                        <td style="width:30%">
                            <label>Report Title</label>
                        </td>
                        <td style="width:40%">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtrptname" runat="server" MaxLength="50" TabIndex="1" required style="width: 180px;"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtrpttitle" runat="server" MaxLength="50" TabIndex="2" required style="width: 240px;"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Button ID="btnsubmit" runat="server" OnClientClick="return validateform();" CssClass="mybutton" style="width: 40%;margin-left: 10%;"
                                Text="Save Report" CausesValidation="true"/>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="clr"></div>
            
            <br />
           
            <div class="create_row" id="colDetails" runat="server" style="overflow:auto;max-height:200px"  ><!--row !-->
                <table border="0" cellspacing="0" cellpadding="0" class="field_detail responsive" width="100%">
            	    <thead>
	                <tr>
    	                <th style="text-align:left"><strong>Report</strong></th>
    	                <th style="text-align:left"><strong>Links</strong></th>
                    </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater runat="server" ID="rptReports" >
                            <ItemTemplate>
                            <tr class="odd gradeX">
                                <td><asp:Literal runat="server" ID="litFName" Text='<%#Eval("RptName")%>'></asp:Literal></td>
        	                    <td>
    	                            <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("grouprptrowid")%>' ID="ActDel" runat="server"
                                        Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
    	                            <asp:LinkButton CommandName="Up" CommandArgument='<%#Eval("grouprptrowid")%>' ID="ActUp" runat="server"
                                        Text="Up" ToolTip="Up" ></asp:LinkButton>
    	                            <asp:LinkButton CommandName="Down" CommandArgument='<%#Eval("grouprptrowid")%>' ID="ActDown" runat="server"
                                        Text="Down" ToolTip="Down" ></asp:LinkButton>
                                    <asp:HiddenField ID="hdnOrder" runat="server" Value='<%#Eval("orderno")%>' />
                                </td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater> 
                    </tbody>

                </table>
            </div>

            <br /><br />
         </div>

    </section><!--container section!-->


    <section class='e_detailpop' id='editninja' style="display:none"><!--Email Detail Pop up!-->
	    <div class="det_pop">
    	    <div class="close_btn1"><img src="images/close.png"  style="cursor:pointer" onclick='$("#editninja").hide();'  /></div>
            <h3>Add Report</h3>
            <div class="create_form"><!--Create Edit form!-->
           
                <div id="divusers" runat="server">
                    <div class="create_row"><!--row !-->
                        <div class="n_left"><label>Search:</label></div>
                        <div class="n_inpfield"><input type="text" id="pSrchText" runat="server"   /></div>
                        <div class="n_sbt"><input type="submit" id="searchReports" runat="server" value="GO" /></div>
                        <div class="clr"></div>
                    </div><!--row !-->
                  
                    <div id="divselusers" runat="server">
                        <div class="s_result"><!--current member section!-->
                            <h2>Search Results</h2>
                            <div class="member_list" style="overflow:auto;max-height:200px">
                                <p runat="server" id="pNoResults">No matching results found.</p>
                                <asp:Repeater id="rptAccts" runat="server">
                                    <ItemTemplate>
                                        <p>
                                            <asp:LinkButton CommandName="Add" CommandArgument='<%#Eval("rptid")%>' ID="NinDel" 
                                                runat="server" Text="<img src='images/green-add.png' />" ToolTip="Add" ></asp:LinkButton>
                                            <%#Eval("rptname")%>
                                        </p> 
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                        </div><!--current member section!-->
                        <div class="clr"></div>
                    </div>

                </div>
                
            </div><!--Create Edit form!-->
        </div>
    </section><!--Email Detail Pop up!-->

</asp:Content>


