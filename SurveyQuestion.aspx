<%@ Page Title="Survey Question" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SurveyQuestion.aspx.vb" Inherits="SurveyQuestion" %>

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
		};
		//function showhidediv() {
		//	alert('1');
		//	var ddlanstype = document.getElementById("ctl00_ContentPlaceHolder1_ddlAnsType");
		//	var ddldatatype = document.getElementById("ctl00_ContentPlaceHolder1_ddltabledata");
		//	alert(document.getElementById("ctl00_ContentPlaceHolder1_ddlAnsType").value);
		//	if (ddlanstype.Value == "Select Table") {
		//		ddldatatype.style.display == "block";
		//	} else {
		//		ddldatatype.style.display ==  "none";
		//	}
		//	return false;
		//}

		function showhidediv() {
			var ddlanstype = document.getElementById("ctl00_ContentPlaceHolder1_ddlAnsType");
			var ddldatatype = document.getElementById("ctl00_ContentPlaceHolder1_ddltabledata");
			ctl00_ContentPlaceHolder1_ddltabledata.style.display = ctl00_ContentPlaceHolder1_ddlAnsType.value == "Select Table" ? "block" : "none";
		}

    </script>
    

    <section class="container"><!--sales dashboard!-->
        <asp:HiddenField ID="hdnSurveyID" runat="server" />
        <asp:HiddenField ID="hdnSurveyQuestionID" runat="server" />
        <asp:HiddenField ID="hdnMaxQNo" runat="server" />

	    <div class="ac_list_main">
    	    <div class="top_hd"><!--top heading!-->
        	    <h2>Create Survey<a class="new_btn" href="SurveyList.aspx">Back</a></h2>
    	        <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            </div><!--top heading!-->

            <div class="s-tabs">
        	    <div class="s-tab">
            	    <span>1</span> <p><a id="btntab1" runat="server">Overview</a></p>
                </div>
                <div class="s-tab">
            	    <span>2</span> <p><a id="btntab2" runat="server">Design</a></p>
                </div>
                <div class="s-tab s-tab1 s-tab1-active s-tab-current">
            	    <span>3</span> <p>Questions</p>
                </div>
                <div class="s-tab s-tab-last">
            	    <span>4</span> <p><a id="btntab4" runat="server">Finalize</a></p>
                </div>
                <div class="clr"></div>
            </div>

        <div class="survey-main">
        	<div class="survey-left">
            	<div class="s-label">
                	<div class="sl-left"><p>Question Text</p></div>
                </div>
                <div class="s-white">
                    <textarea class="text-field" id="txtqtext" runat="server"></textarea>
                </div>
                
                <div class="s-label bottom-five">
                	<div class="sl-left"><p>Question Active?</p></div>
					<div class="sl-right">
                    	<label class="switch">
                            <input checked="" class="inputdemo" type="checkbox" id="chkActive" runat="server" />
                             <div class="slider round"></div>
                        </label>
                    </div>
                </div>

                <div class="s-label bottom-five">
                	<div class="sl-left"><p>Question Mandatory?</p></div>
					<div class="sl-right">
                    	<label class="switch">
                            <input checked="" class="inputdemo" type="checkbox" id="chkMandatory" runat="server" />
                             <div class="slider round"></div>
                        </label>
                    </div>
                </div>

                <div class="s-label bottom-five" style="display: none;">
                	<div class="sl-left"><p>Allow Adding New Option?</p></div>
					<div class="sl-right">
                    	<label class="switch">
                            <input checked="" class="inputdemo" type="checkbox" id="chkNewOption" runat="server" />
                             <div class="slider round"></div>
                        </label>
                    </div>
                </div>

				<div id="add_comment" class="s-white" style="display: none">
                    <input class="text-field" name="" placeholder="Add option" type="text" id="txtaddcomment" runat="server" />
                </div>
				
                <div class="s-label" style="display: none">
                	<div class="sl-left"><p>Show Comment?</p></div>
					<div class="sl-right">
                    	<label class="switch">
                            <input checked="" class="inputdemo" type="checkbox"  id="chkShowComment" runat="server" />
                             <div class="slider round"></div>
                        </label>
                    </div>
                </div>
                <div class="s-white" style="display: none">
                    <asp:DropDownList ID="ddlCommentType" runat="server" Width="40%" TabIndex="4"  CssClass="dropdown">
                        <asp:ListItem Text="Select " Value = ""></asp:ListItem>
                        <asp:ListItem Text="Textbox" Value="Textbox"></asp:ListItem>
                        <asp:ListItem Text="Textarea" Value="Textarea"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="s-label" style="display: none">
                	<div class="sl-left"><p>Comment Header?</p></div>
                </div>
                <div class="s-white" style="display: none">
                	<input type="text" class="text-field" id="txtheader" runat="server" />
                </div>
                <div class="s-label bottom-five" style="display: none">
                	<div class="sl-left"><p>Allow ranking?</p></div>
					<div class="sl-right">
                    	<label class="switch">
                            <input checked="" class="inputdemo" type="checkbox" id="chkranking" runat="server" />
                             <div class="slider round"></div>
                        </label>
                    </div>
                </div>



                <div class="s-label bottom-five" style="display: none">
                	<div class="sl-left"><p>Has Logic?</p></div>
					<div class="sl-right" >
                    	<label class="switch">
                            <input checked="false" class="inputdemo" type="checkbox" id="chkHaslogic" runat="server" />
                             <div class="slider round"></div>
                        </label>
                    </div>
                </div>
                <div class="s-label" style="display: none;">
                	<div class="sl-left"><p>Expected Answer?</p></div>
                </div>
                <div class="s-white" style="display: none">
                    <asp:DropDownList ID="DropDownList1" runat="server" Width="40%" TabIndex="4"  CssClass="dropdown">
                        <asp:ListItem Text="No" Value="No"></asp:ListItem>
                        <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="s-label" style="display: none">
                	<div class="sl-left"><p>Goto Question:</p></div>
                </div>
                <div class="s-white" style="display: none">
                    <asp:DropDownList ID="DropDownList2" runat="server" Width="40%" TabIndex="4"  CssClass="dropdown">
                        <asp:ListItem Text="Select" Value="0"></asp:ListItem>
                    </asp:DropDownList>
                </div>


                <div class="s-label">
                	<div class="sl-left"><p>Answer Type?</p></div>
                </div>
                <div class="s-white">
                    <asp:DropDownList ID="ddlAnsType" runat="server" Width="90%" CssClass="dropdown" onchange="return showhidediv();">
                        <asp:ListItem Text="Select " Value = ""></asp:ListItem>
                        <asp:ListItem Text="Yes or No" Value="Yes or No"></asp:ListItem>
                        <asp:ListItem Text="Yes or No or Can't Say" Value="Yes or No or Can't Say"></asp:ListItem>
                        <asp:ListItem Text="Checkbox Multiple" Value = "Checkbox Multiple"></asp:ListItem>
                        <asp:ListItem Text="Select one of many" Value = "Select one of many"></asp:ListItem>
                        <asp:ListItem Text="Date" Value = "Date"></asp:ListItem>
                        <asp:ListItem Text="Number" Value = "Number"></asp:ListItem>
                        <asp:ListItem Text="Text(100)" Value = "Text(100)"></asp:ListItem>
                        <asp:ListItem Text="Freeform Text" Value = "Freeform Text"></asp:ListItem>
                        <asp:ListItem Text="Scale(1-100)" Value = "Scale(1-100)"></asp:ListItem>
                        <asp:ListItem Text="STARS" Value = "STARS"></asp:ListItem>
                        <asp:ListItem Text="CAMERA" Value = "CAMERA"></asp:ListItem>
                        <asp:ListItem Text="Multiple Text Response" Value = "Multiple Text Response"></asp:ListItem>
                        <asp:ListItem Text="Select Table" Value="Select Table"  ></asp:ListItem>
                        <asp:ListItem Text="Agree - Disagree" Value="Agree - Disagree"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddltabledata" runat="server" Width="90%" CssClass="dropdown"  style="display:none">
                        <asp:ListItem Text="Select" Value="select"></asp:ListItem>
                        <asp:ListItem Text="Table1" Value="Table1"></asp:ListItem>
                        <asp:ListItem Text="Table2" Value="Table2"></asp:ListItem>
                        <asp:ListItem Text="Table3" Value="Table2"></asp:ListItem>
                    </asp:DropDownList> 

                    <div class="s-label">
                	<div class="sl-left"><p>Intelligence Type?</p></div>
                </div>
                <div class="s-white">
                     <asp:DropDownList ID="ddlintell" runat="server" Width="90%" CssClass="dropdown" >
                         <asp:ListItem Text="Select Answer" Value="0" />
                        <asp:ListItem Text="Linguistic" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Logical" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Musical" Value="3"></asp:ListItem>
                        <asp:ListItem Text="Bodily-Kinaesthetic" Value="4"></asp:ListItem>
                         <asp:ListItem Text="Spatial" Value="5"></asp:ListItem>
                         <asp:ListItem Text="Interpersonal" Value="6"></asp:ListItem>
                         <asp:ListItem Text="Intrapersonal" Value="7"></asp:ListItem>
                    </asp:DropDownList>

                    </div>
                   
                    
                    <div class="new_btab dataTables_wrapper" id="divQuesAns" runat="server">
		                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="sort_tab">
			                <thead>
				                <tr>
					                <th><span>Action</span></th>
					                <th><span>Answer &nbsp&nbsp<a href="#" onclick='$("#addAcctAttr").show();' class="call_log">Add New</a></span></th>
				                </tr>
			                </thead>
			                <tbody>
					            <asp:Repeater ID="rptQuesAns" runat="server">
						            <ItemTemplate>
						            <tr>
							            <td>
								            <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("sqarowid")%>' ID="ActDel" runat="server"
									            Text="<img src='images/Delete.png' />" OnClientClick="return delconfirm();" ToolTip="Delete"></asp:LinkButton>
							            </td>
							            <td><%#Eval("qatext")%></td>
						            </tr>
						            </ItemTemplate>
					            </asp:Repeater>
			                </tbody>
		                </table>
	                </div>
                </div>

            </div>
                    
            <div class="survey-right">
    	        <div class="heading_row topspace  crasul_sec"><!--top heading!-->
        	        <h2>Questions</h2>
                </div><!--top heading!-->

                <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="display nowrap ac_list" >
            	        <thead>
                        <tr>
                            <th><span>Question</span></th>
                            <th><span>Active</span></th>
                            <th><span>Mandatory</span></th>
                            <th><span>Answer Type</span></th>
                            <th><span>Links</span></th>
                        </tr>
                        </thead>
                        <tbody>
                        <asp:Repeater ID="rptSurveyQuestions" runat="server">
                            <ItemTemplate>
                                <tr>
                                    <asp:HiddenField ID="SurveyID" runat="server" />
                                    <td><%#Eval("qtext")%></td>
                                    <td><%#Eval("active")%></td>
                                    <td><%#Eval("mandatory")%></td>
                                    <td><%#Eval("anstype")%></td>
        	                        <td>
    	                                <asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("sqrowid")%>' ID="ActEdit" runat="server"
                                            Text="<img src='images/edit.png' />" ToolTip="Edit"></asp:LinkButton>
    	                                <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("sqrowid")%>' ID="ActDel" runat="server"
                                            Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
    	                                <asp:LinkButton CommandName="Up" CommandArgument='<%#Eval("sqrowid")%>' ID="ActUp" runat="server"
                                            Text="Up" ToolTip="Up" ></asp:LinkButton>
    	                                <asp:LinkButton CommandName="Down" CommandArgument='<%#Eval("sqrowid")%>' ID="ActDown" runat="server"
                                            Text="Down" ToolTip="Down" ></asp:LinkButton>
                                        <asp:HiddenField ID="hdnOrder" runat="server" Value='<%#Eval("qorder")%>' />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater>                                            
                     </tbody>
                    </table>
    			
                </div><!--table listing!-->

                <div class="progress">
                    <div class="progress-bar" role="progressbar" aria-valuenow="75" aria-valuemin="0" aria-valuemax="100" style="width: 75%;">75%</div>
                </div>
                <div class="create_button">
                    <a id="btnSave" runat="server" class="btn-black">Save</a>
                    <a id="btlClear" runat="server" class="btn-black">Clear</a>
                </div>
            </div>
            <div class="clr"></div>
        </div>

        </div>
    </section><!--sales dashboard!-->


    <section class='e_detailpop' id='addAcctAttr' style="display:none"><!--Edit Contact!-->
	    <div class="create_edit" style="cursor:pointer;width:40%">
    	    <div class="close_btn1" onclick='$("#addAcctAttr").hide();'><img src="images/close.png" /></div>
            <h3 class="template_heading">Save Answer Value</h3>
            <div class="create_form"><!--Create Edit form!-->
                <div class="create_row"><!--row !-->
       		        <div class="create_left" style="width:80%"><!--left section!-->
            	        <div class="field_name"><label>Description:</label></div>
                        <div class="inp_f" >
                            <asp:TextBox ID="txtDesc" runat="server" MaxLength="1000" ValidationGroup="CEAttr1" Text=""></asp:TextBox>
                        </div>
                        <div class="clr"></div>
                    </div><!--left section!-->
                    <div class="clr"></div>
                </div><!--row !-->
                <div class="create_row"><!--row !-->
       		        <div class="create_left"><!--left section!-->
            	        <div class="field_name"><label>&nbsp;</label></div>
                        <div class="inp_f"><input id="btnSaveAns" runat="server" type="submit" value="Submit" class="add" /></div>
                        <div class="clr"></div>
                    </div><!--left section!-->
                    <div class="clr"></div>
                </div><!--row !-->
           </div><!--Create Edit form!-->
        </div>
    </section><!--Edit Contact Pop up!-->

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

