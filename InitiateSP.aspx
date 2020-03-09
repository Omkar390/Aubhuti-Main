<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="InitiateSP.aspx.vb" Inherits="InitiateSP" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function InitiateNew() {
            //document.getElementById("ctl00_ContentPlaceHolder1_hdnBlastProfileId").value = '0';
            //document.getElementById("ctl00_ContentPlaceHolder1_pListName").value = '';
            //document.getElementById("ctl00_ContentPlaceHolder1_pSelOwner").value = 'Select ';
            //document.getElementById("divrpt").style.display = 'none';
            //document.getElementById("ctl00_ContentPlaceHolder1_divadd").style.display = 'none';
            $("#initiateprocess").show();
        }


        function showList() {
            $("#initiateprocess").show();
            $("#picklist").show();
        }

        function showTarget() {
            $("#initiateprocess").show();
            $("#picktarget").show();
        }

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

        <input type="hidden" id="hdnBlastProfileId" runat="server" style="display:none" />
        <input type="hidden" id="hdnSelTargetID" runat="server" />
        <input type="button" id="btnMeetRefresh" runat="server" style="display:none" />
        <input type="hidden" id="hdnSelTemplate" runat="server" style="display:none" />
        <input type="hidden" id="hdnSelList" runat="server" style="display:none" />

    	<div class="ac_list_main">

            <div class="heading_row topspace crasul_sec">
    	        <h2>Run Sale Process 
                    <a onclick='InitiateNew();' href="#"> Initiate New</a>
                    <a href="SalesProcess.aspx"> Back</a>
    	        </h2>
                <div class="clr"></div>
                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
            </div>
                                	    
            <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="sort_tab">
            	    <thead>
                    <tr>
                        <th width="20%"><span>Initiated By</span></th>
                        <th width="13%"><span># Open Processes</span></th>
                   	    <th width="10%"><span># Open Items</span></th>
                        <th width="10%"><span># Open Today</span></th>
                        <th width="10%"><span># Closed</span></th>
                        <th width="13%"><span># Closed Today</span></th>
                        <th width="10%"><span># Skipped</span></th>
                        <th width="14%"><span># Skipped Today</span></th>
                    </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptRunProcesses" runat="server">
                            <ItemTemplate>
                            <tr>
                                <td><%#Eval("initiated")%></td>
                                <td><%#Eval("open_proc")%></td>
                                <td><asp:LinkButton ID="lnkOpen" runat="server" CommandName="Open" Text='<%#Eval("num_open")%>' CommandArgument='<%#Eval("initiated")%>'></asp:LinkButton></td>
                                <td><asp:LinkButton ID="lnkOpenToday" runat="server" CommandName="OpenToday" Text='<%#Eval("num_open_today")%>' CommandArgument='<%#Eval("initiated")%>'></asp:LinkButton></td>
                                <td><%#Eval("num_closed")%></td>
                                <td><%#Eval("num_closed_today")%></td>
                                <td><asp:LinkButton ID="lnkSkipped" runat="server" CommandName="Skipped" Text='<%#Eval("num_skipped")%>' CommandArgument='<%#Eval("initiated")%>'></asp:LinkButton></td>
                                <td><%#Eval("num_skipped_today")%></td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div><!--table listing!-->
            
        </div>

    	<div class="ac_list_main">

            <div class="heading_row topspace crasul_sec">
    	        <h4>Open Processes</h4>
                <div class="clr"></div>
            </div>
            <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="sort_tab2">
            	    <thead>
                    <tr>
                        <th width="10%"><span>Start Date</span></th>
                   	    <th width="30%"><span>Name</span></th>
                        <th width="30%"><span>List</span></th>
                        <th width="10%"><span>Contacts</span></th>
                        <th width="10%"><span>ISR</span></th>
                        <%--<th width="10%"><span>Action</span></th>--%>
                    </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptOpenProcess" runat="server">
                            <ItemTemplate>
                            <tr>
                                <td><%#Eval("PStartDate")%></td>
                                <td><asp:LinkButton ID="lnkOpen" runat="server" CommandName="Open" Text='<%#Eval("PName")%>' CommandArgument='<%#Eval("RPID")%>'></asp:LinkButton></td>
                                <td><%#Eval("ListName")%></td>
                                <td><%#Eval("ListCnt")%></td>
                                <td><%#Eval("ISR")%></td>
                                <%--<td><asp:LinkButton ID="lnkDel" runat="server" CommandName="Del" Text='<%#Eval("DelIns")%>' OnClientClick="return delconfirm();"  CommandArgument='<%#Eval("RPID")%>'></asp:LinkButton></td>--%>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div><!--table listing!-->
            
        </div>

    	<div class="ac_list_main">
            <div class="heading_row topspace crasul_sec">
    	        <h4>Closed Processes</h4>
                <div class="clr"></div>
            </div>
            <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="sort_tab3">
            	    <thead>
                    <tr>
                        <th width="10%"><span>Start Date</span></th>
                   	    <th width="35%"><span>Name</span></th>
                        <th width="35%"><span>List</span></th>
                        <th width="20%"><span>Contacts</span></th>
                    </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptClosedProcess" runat="server">
                            <ItemTemplate>
                            <tr>
                                <td><%#Eval("PStartDate")%></td>
                                <td><asp:LinkButton ID="lnkOpen" runat="server" CommandName="Open" Text='<%#Eval("PName")%>' CommandArgument='<%#Eval("RPID")%>'></asp:LinkButton></td>
                                <td><%#Eval("ListName")%></td>
                                <td><%#Eval("ListCnt")%></td>
                            </tr>
                            </ItemTemplate>
                        </asp:Repeater>
                    </tbody>
                </table>
            </div><!--table listing!-->

        </div>

    </section><!--sales dashboard!-->
            
    <section class='e_detailpop' id='initiateprocess'  style="display:none;height:100%"><!--sales dashboard!-->

	    <div class="create_edit">
            <div class="close_btn1"><img src="images/close.png" onclick='$("#initiateprocess").hide();'  style="cursor:pointer" /></div>
            <h3 class="template_heading">Initiate Sales Process</h3>
            <br />
            <div class="ed_pro"><!--edit profile section!-->
                <div class="pick_sec"><!-- selection section!-->
            	    <div class="pick_left"><!--left section!-->
                	    <a href="#" style="cursor:default"><asp:Literal ID="litSelList" runat="server"></asp:Literal></a>
                        <a href="#" onclick='showList();' >Choose List</a>
                    </div><!--left section!-->
                    <div class="pick_rt">
                	    <a href="#"  style="cursor:default" class="target"><asp:Literal ID="litSelTemplate" runat="server"></asp:Literal></a>
                        <a href="#" onclick='showTarget();' class="target">Choose Sales Process</a>
                    </div>
                    <div class="clr"></div>
                </div><!-- selection section!-->

                <div class="create_form">
                    <div class="create_row"><!--row !-->
       			        <div class="create_left"><!--left section!-->
            		        <div class="field_name"><label>Start Date</label></div>
                		    <div class="inp_f">
                                <em class="out_border"></em>
                                    <input id="pdtVal" runat="server" type="text" class="date datepicker" autocomplete="off" />
                		    </div>
                		    <div class="clr"></div>
                        </div><!--left section!-->
            	        <div class="create_left create_rt"><!--create right !-->
            			    <div class="field_name"><label>Use Tracking?</label></div>
                		    <div class="inp_f">
                                <em class="out_border"></em><input  id="pUseTracking" runat="server"  type="checkbox" class="check" />
                		    </div>
                		    <div class="clr"></div>
            	        </div><!--create right !-->
            	        <div class="clr"></div>
                    </div>
            	
                    <br /><br />

                    <div class="create_row">
        		        <div class="create_button">
                            <a href="#" onclick='$("#initiateprocess").hide();' class="cancel">Cancel</a>&nbsp;&nbsp; 
                            <asp:LinkButton ID="lnkSaveList" CssClass="ok" runat="server" >Submit</asp:LinkButton>
        		        </div>
       		        </div>


                </div>

            </div><!--edit profile section!-->

            <br />

        </div>

    	
<%--            <div class="ed_pro"><!--edit profile section!-->
        	    <div class="name_sec">
            	    <div class="name_row">
                	    <label>Name</label> <input type="text" />
                    </div>
                </div>
                <div class="pick_sec"><!-- selection section!-->
            	    <div class="pick_left"><!--left section!-->
                	    <a href="sale_list.html">Choose List</a>
                    </div><!--left section!-->
                    <div class="pick_rt">
                	    <a href="#" class="target">Choose Sale Process</a>
                    </div>
                    <div class="clr"></div>
                </div><!-- selection section!-->
            
                <div class="name_sec top_mg">
            	    <div class="name_row">
                	    <label>Start Date</label> <input type="text" class="date" />
                    </div>
                    <div class="name_row">
                	    <label>&nbsp;</label> <input type="text" placeholder="60" class="number_list" />
                    </div>
                </div>
            </div><!--edit profile section!-->--%>
        
    </section><!--sales dashboard!-->

    <section class='e_detailpop' id='picklist' style="display:none;height:90%" ><!--Email Detail Pop up!-->
	    <div class="create_edit">
    	    <div class="close_btn1"><img src="images/close.png" style="cursor:pointer" onclick='$("#picklist").hide(); $("#initiateprocess").show();'  /></div>
       	    <h3>Search and Pick a List</h3>
            <div class="create_form"><!--from section!-->

       		    <div class="create_row"><!--row !-->
       		        <div class="create_left"><!--left section!-->
                        <div class="inp_f">
						    <input type="search" placeholder="Search List" id="txtSrchList" runat="server" />
						    <asp:LinkButton ID="lnkSrchList" runat="server">
                                <img class="search_ico" src="images/search_ico.png" style="margin-top:-2%;" />
						    </asp:LinkButton>
                        </div>
                        <div class="clr"></div>
                    </div><!--left section!-->
                    <div class="create_left create_rt"><!--create right !-->
            	        <div class="field_name">
                            <label>Select List Owner:</label>
            	        </div>
                        <div class="inp_f">
                            <asp:DropDownList runat="server" ID="plistSelOwner" AutoPostBack="true" DataTextField="username" DataValueField="userID" ></asp:DropDownList>
                        </div>
                        <div class="clr"></div>
                    </div><!--create right !-->
                    <div class="clr"></div>
      	        </div>

                <div class="lst_created scroll_list"><!--last created template!-->
        	        <h2>Pick a List</h2>
       		        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive" id="sort_tab">
            	        <thead>
                            <tr>
                                <th width="77%"><span>Template</span></th>
                   	            <th width="23%"><span>Contacts</span></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptList" runat="server">
                                <ItemTemplate>
                                    <tr>
                 	                    <td><asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("listid")%>' ID="TempEdit" runat="server"
                                                Text="Select" ToolTip="Edit" CssClass="select_btn" ></asp:LinkButton>
                                             <asp:literal ID="litListName" runat="server" Text='<%#Eval("listname")%>'></asp:literal>
                 	                    </td>
                                        <td><%#Eval("Items")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div><!--last created template!-->

            </div><!--from section!-->
        </div>
    </section><!--Email Detail Pop up!-->
    
    <section class='e_detailpop' id='picktarget' style="display:none;height:90%" ><!--Email Detail Pop up!-->
	    <div class="create_edit">
    	    <div class="close_btn1"><img src="images/close.png" style="cursor:pointer" onclick='$("#picktarget").hide(); $("#initiateprocess").show();'  /></div>
       	    <h3>Pick Sales Process Template</h3>
            <div class="create_form"><!--from section!-->

       		    <div class="create_row"><!--row !-->
       		        <div class="create_left"><!--left section!-->
                        <div class="inp_f">
						    <input type="search" placeholder="Search Template" id="txtSrchTar" runat="server" />
						    <asp:LinkButton ID="lnkSrchTmplt" runat="server">
                                <img class="search_ico" src="images/search_ico.png" style="margin-top:-2%;" />
						    </asp:LinkButton>
                        </div>
                        <div class="clr"></div>
                    </div><!--left section!-->
                    <div class="create_left create_rt"><!--create right !-->
            	        <div class="field_name">
                            <label>Owner:</label>
            	        </div>
                        <div class="inp_f">
                            <asp:DropDownList runat="server" ID="ptarSelOwner" DataTextField="username" DataValueField="userID"  AutoPostBack="true" ></asp:DropDownList>
                        </div>
                        <div class="clr"></div>
                    </div><!--create right !-->
                    <div class="clr"></div>
      	        </div>

                <div class="lst_created scroll_list"><!--last created template!-->
        	        <h2>Recently List</h2>
       		        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive" id="sort_tab">
            	        <thead>
                            <tr>
                                <th width="77%"><span>Template</span></th>
                                <th width="12%"><span>Days</span></th>
                   	            <th width="11%"><span>Owner</span></th>
                            </tr>
                        </thead>
                        <tbody>
                            <asp:Repeater ID="rptTarget" runat="server">
                                <ItemTemplate>
                                    <tr>
                 	                    <td><asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("PID")%>' ID="TempEdit" runat="server"
                                                Text="Select" ToolTip="Edit" CssClass="select_btn" ></asp:LinkButton>
                                             <asp:literal ID="litTargetName" runat="server" Text='<%#Eval("PName")%>'></asp:literal></td>
                                        <td><%#Eval("PDays")%></td>
                                        <td><%#Eval("username")%></td>
                                    </tr>
                                </ItemTemplate>
                            </asp:Repeater>
                        </tbody>
                    </table>
                </div><!--last created template!-->

            </div><!--from section!-->
        </div>
    </section><!--Email Detail Pop up!-->

    <script type="text/javascript" >
        $(document).ready(function() {

            $('#sort_tab').DataTable({
                responsive: true,
	            bFilter: false,
	            bInfo: false,
                'iDisplayLength': 1000
        });

            $('#sort_tab2').DataTable({
                responsive: true,
                bFilter: false,
                bInfo: false,
                'iDisplayLength': 1000
        });

            $('#sort_tab3').DataTable({
                responsive: true,
                bFilter: false,
                bInfo: false,
                'iDisplayLength': 1000
        });

        });
    </script>
                    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

