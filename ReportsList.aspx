<%@ Page Language="VB" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="ReportsList.aspx.vb" Inherits="Users" title="Reports" %>

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
            

    <script type="text/javascript">

        function clearSrch() {
            document.getElementById("ctl00_ContentPlaceHolder1_txtsrch").value = '';
        }

    </script>

    <section class="container"><!--sales dashboard!-->

	    <div class="ac_list_main">
            <asp:HiddenField id="hdnrptid" runat="server" />
    	    <div class="heading_row topspace crasul_sec">
                <h2>Reports 
                    <asp:LinkButton ID="btnNew" runat="server" Text="New"></asp:LinkButton>
                    <a class="new_btn" href="AdminMenu.aspx" >Back</a>
                </h2>
                <div class="clr"></div>
            </div>
        
            <div class="process_form"><!--process form section!-->
                <div class="process_left" style="width:100%">
                    <div style="float:left;width:5%;margin-left:1%">
                        <label><b>Search</b></label>
                    </div>
                    <div style="float:left;width:20%">
                        <asp:TextBox ID="txtsrch" runat="server" MaxLength="50" style="width:95%" TabIndex="1" ></asp:TextBox>
                    </div>
                    <div class="create_button text_left">
                        <a class="ok" id="btnSrch" runat="server">Search</a>
                        <a class="cancel" id="btnClear" onclick='clearSrch();' runat="server">Clear</a>&nbsp;&nbsp; 
                    </div>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->

            <div class="list_table"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive" id="sort_tab">
            	    <thead>
                      <tr>
                        <th width="19%"><span>Name</span></th>
                        <th width="17%"><span>Title</span></th>
                        <th width="17%"><span>Type</span></th>
                        <th width="14%"><span>Category</span></th>
                        <th width="14%"><span>Last Updated</span></th>
                        <th width="11%"><span>Action</span></th>
                      </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptUsers" runat="server">
                            <ItemTemplate>
                                <tr>
                 	                <td><asp:literal ID="litRptName" runat="server" Text='<%#Eval("RptName")%>'></asp:literal></td>
                                    <td><asp:literal ID="litTitle" runat="server" Text='<%#Eval("RptTitle")%>'></asp:literal></td>
                                    <td><asp:literal ID="litType" runat="server" Text='<%#Eval("RptType")%>'></asp:literal></td>
                                    <td><%#Eval("RptCategory")%></td>
                                    <td><%#Eval("updatedon")%></td>
                                    <td><asp:LinkButton ID="btnuseredit" runat="server" CommandArgument='<%#Eval("RptId")%>' 
                                        CommandName='Edit'><img src="images/edit.png" /></asp:LinkButton>
                                        <asp:LinkButton ID="btnuserdel" runat="server" CommandArgument='<%#Eval("RptId")%>' 
                                        CommandName='Del'><img src="images/delete.png" /></asp:LinkButton></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>                                                    
                    </tbody>
                </table>
            </div>
        </div>

    </section><!--sales dashboard!-->

    <script type="text/javascript" >
        $(document).ready(function () {
            $('#sort_tab').DataTable({
                responsive: true,
                bFilter: false,
                bPaginate: false,
                order: [[4, "desc"]],
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

    <section class="e_detailpop" id='edituser' style="display:none"><!--sales dashboard!-->
	    <div class="create_edit" >
    	    <div class="close_btn1"><img src="images/close.png" style="cursor:pointer" onclick='$(".e_detailpop").hide();' /></div>
                <h3 class="template_heading"><asp:literal id="Literal1" runat="server" Text="Create Report"></asp:literal></h3>
                <div class="create_form"><!--Create Edit form!-->
                    <div class="create_row"><!--row !-->
       		            <div><!--left section!-->
            	            <div class="field_name"><label>Notes: 1. Do not keep "LIMIT" and "ORDER BY" clauses in your SQL. 2. Start a Chart name with the word Chart.</label>&nbsp;&nbsp;<asp:Literal runat="server" ID="lblMsg"></asp:Literal></div>
                        </div>
                        <div class="clr"></div>
                    </div>
                    <div class="create_row"><!--row !-->
       		            <div class="create_left"><!--left section!-->
            	            <div class="field_name"><label>Report Name<em>*</em>:</label></div>
                            <div class="inp_f"><input type="text" id="txtRName" runat="server" required /></div>
                            <div class="clr"></div>
                        </div><!--left section!-->
                        <div class="create_left create_rt"><!--create right !-->
            	            <div class="field_name"><label> Report Title </label></div>
                            <div class="inp_f"><input type="text" id="txtRTitle" runat="server" required /></div>
                            <div class="clr"></div>
                        </div><!--create right !-->
                        <div class="clr"></div>
                    </div><!--row !-->
                                 
                    <div class="create_row"><!--row !-->
       		            <div class="create_left"><!--left section!-->
            	            <div class="field_name"><label>SQL</label></div>
                            <div class="inp_f"><textarea id="txtRSQL" cols="98" rows="10" runat="server" required style="width:230%" /></div>
                            <div class="clr"></div>
                        </div><!--left section!-->
                        <div class="clr"></div>
                    </div><!--row !-->

                    <div class="create_row"><!--row !-->
       		            <div class="create_left"><!--left section!-->
            	            <div class="field_name"><label>Type</label></div>
                            <div class="inp_f">
                                <div>
                                    <div style="float:left">
                                        <asp:DropDownList ID="ddlRType" runat="server" required Width="100px">
                                            <asp:ListItem Value="">Select</asp:ListItem>
                                            <asp:ListItem Value="T">Top Level</asp:ListItem>
                                            <asp:ListItem Value="D">Drilldown</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div style="float:left">
            	                        <div class="field_name" style="margin-left:15px;width:100px"><label>Show Totals</label></div>
                                        <div class="inp_f" style="float:left;width:20%;margin-left:-15px"><asp:CheckBox runat="server" ID="chkShowTotals" /></div>
                                    </div>
                                    </div>
                            </div>
                            <div class="clr"></div>
                        </div><!--left section!-->
                        <div class="create_left create_rt"><!--create right !-->
            	            <div class="field_name"><label>Category</label></div>
                            <div class="inp_f">
                                <asp:DropDownList ID="ddlRCat" runat="server" required Width="100px" >
                                    <asp:ListItem Value="">Select</asp:ListItem>
                                    <asp:ListItem Value="Client">Client</asp:ListItem>
                                    <asp:ListItem Value="Master">Master</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="clr"></div>
                        </div><!--create right !-->
                        <div class="clr"></div>
                    </div><!--row !-->

                    <div class="create_row"><!--row !-->
       		            <div class="create_left"><!--left section!-->
            	            <div class="field_name"><label>Date Filter</label></div>
                            <div class="inp_f" style="float:left;width:20%">
                                <%--<asp:CheckBox runat="server" ID="chkShowYQM" />--%>
                                <asp:DropDownList ID="ddlDateFilterType" runat="server" >
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                        <asp:ListItem Value="YQM">YQM</asp:ListItem>
                                        <asp:ListItem Value="RANGE">Date Range</asp:ListItem>
                                </asp:DropDownList>
                            </div>
            	            <div class="field_name" style="float:left;width:20%"><label>Width (% or px)</label></div>
                            <div class="inp_f" style="float:left;width:30%"><asp:TextBox runat="server" ID="txtWidth" Width="50px" Text="100%"></asp:TextBox></div>
                               
                            <div class="clr"></div>
                        </div><!--left section!-->
                        <div class="create_left create_rt"><!--create right !-->
            	            <div class="field_name"><label>Default Sort</label></div>
                            <div class="inp_f">
                                <asp:DropDownList ID="ddlDefaultSort" runat="server" >
                                </asp:DropDownList>
                            </div>
                            <div class="clr"></div>
                        </div><!--create right !-->
<%--                        <div class="create_left create_rt"><!--create right !-->
            	            <div class="field_name"><label>Show Dates</label></div>
                            <div class="inp_f"><asp:CheckBox runat="server" ID="chkShowDates" /></div>
                            <div class="clr"></div>
                        </div><!--create right !-->--%>
                        <div class="clr"></div>
                    </div><!--row !-->

                    <div class="create_row"><!--row !-->
       		            <div class="create_left"><!--left section!-->
                                <div class="field_name"><label>Type</label></div>
                                <div class="inp_f">
                                    <asp:DropDownList ID="ddlChartType" runat="server">
                                        <asp:ListItem Value="">Select</asp:ListItem>
                                        <asp:ListItem Value="Bar">Bar</asp:ListItem>
                                        <asp:ListItem Value="Column">Column</asp:ListItem>
                                        <asp:ListItem Value="StackedColumn">StackedColumn</asp:ListItem>
                                        <asp:ListItem Value="Pie">Pie</asp:ListItem>
                                        <asp:ListItem Value="Area">Area</asp:ListItem>
                                        <asp:ListItem Value="Line">Line</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </div>
                        <div class="clr"></div>
                        <div class="create_left create_rt"><!--create right !-->
            	            <div class="field_name"><label>Sort Dir</label></div>
                            <div class="inp_f">
                                <asp:DropDownList ID="ddlSortDir" runat="server" >
                                        <asp:ListItem Value="asc">Ascending</asp:ListItem>
                                        <asp:ListItem Value="desc">Descending</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="clr"></div>
                        </div><!--create right !-->
                    </div>


                    <div class="create_row" id="colDetails" runat="server" style="overflow:auto;max-height:200px"  ><!--row !-->
                        <table border="0" cellspacing="0" cellpadding="0" class="field_detail responsive">
                        
	                        <tr>
    	                        <th><strong>Field</strong></th>
    	                        <th><strong>Column Format</strong></th>
                                <th><strong>Show</strong></th>
                                <th><strong>Columns Widths</strong></th>
                                <th><strong>Report Links</strong></th>
                            </tr>

                            <asp:Repeater runat="server" ID="rptColsDetails" >
                                <ItemTemplate>
                                <tr class="odd gradeX">
                                    <td><asp:Literal runat="server" ID="litFName" Text='<%#Eval("FName")%>'></asp:Literal></td>
                                    <td><asp:DropDownList ID="ddlRptColsFormat" runat="server" Width="50%" TabIndex="3" CssClass="or_sel">
                                            <asp:ListItem Text="TL" Value="TL"></asp:ListItem>
                                            <asp:ListItem Text="TR" Value="TR"></asp:ListItem>
                                            <asp:ListItem Text="TC" Value="TC"></asp:ListItem>
                                            <asp:ListItem Text="DD" Value="DD"></asp:ListItem>
                                            <asp:ListItem Text="DT" Value="DT"></asp:ListItem>
                                            <asp:ListItem Text="D0" Value="D0"></asp:ListItem>
                                            <asp:ListItem Text="D2" Value="D2"></asp:ListItem>
                                            <asp:ListItem Text="C0" Value="C0"></asp:ListItem>
                                            <asp:ListItem Text="C2" Value="C2"></asp:ListItem>
                                            <asp:ListItem Text="C4" Value="C4"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td><asp:DropDownList ID="ddlRptShowHide" runat="server" TabIndex="3" CssClass="or_sel">
                                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                                        </asp:DropDownList></td>
                                    <td><asp:textbox runat="server" ID="litRptColWidths" style="width:50%" visible='<%#Not CBool(IsInEditMode)%>' Text='<%#Eval("RptColWidths")%>'></asp:textbox>
                                        <asp:CompareValidator ID="cvlitRptColWidths" runat="server" ControlToValidate="litRptColWidths" Display="Dynamic"
                                            ErrorMessage="Numeric" Operator="DataTypeCheck" SetFocusOnError="True" Type="Integer"></asp:CompareValidator></td>
                                    <td><asp:textbox runat="server" ID="litRptLinks" style="width:95%" visible='<%#Not CBool(IsInEditMode)%>' Text='<%#Eval("RptLinks")%>'></asp:textbox></td>
                                </tr>
                                </ItemTemplate>
                            </asp:Repeater> 

                        </table>
                    </div>
              
                    <div class="create_row"><!--row !-->
       		            <div class="create_left"><!--left section!-->
            	            <div class="field_name"><label>&nbsp;</label></div>
                            <div class="inp_f"><input type="submit" id="lnkUCreate" runat="server" value="Save" class="add" /></div>
                            <div class="clr"></div>
                        </div><!--left section!-->
                        <div class="clr"></div>
                  </div><!--row !-->

           </div><!--Create Edit form!-->
        </div>
    </section>

</asp:Content>

