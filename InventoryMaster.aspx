<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="InventoryMaster.aspx.vb" Inherits="InventoryMaster" title="Inventory Master" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

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

        function newtemplate() {
            document.getElementById("ctl00_ContentPlaceHolder1_hdnSelTargetID").value = '0';
            //document.getElementById("ctl00_ContentPlaceHolder1_pMRName").value = '';
            $("#edittemplate").show();
        }  

        function edittemplate() {
            $("#edittemplate").show();
        }  

    </script>

    <section class="container"><!--sales dashboard!-->
        <input type="hidden" id="hdnSelTargetID" runat="server" />
        <input type="button" id="btnMeetRefresh" runat="server" style="display:none" />

    	    <div class="ac_list_main">

        <div class="heading_row topspace crasul_sec">
    	        <h2>Inventory Types &nbsp
                    <a onclick='newtemplate();' href="#"> New</a>
                    <a class="new_btn" href="DataSetup.aspx">Back</a>
    	        </h2>
                <div class="clr"></div>
        </div>
                                	    
        <div class="new_btab dataTables_wrapper"><!--table listing!--> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="sort_tab">
            	<thead>
                <tr>
                    <th width="40%">Name</th>
                    <th width="10%">Type</th>
                    <th width="10%">SKU</th>
                    <th width="10%">Attachment</th>
                    <th width="10%">Created By</th>
                    <th width="10%">Created Date</th>
                    <th width="10%"></th>
                </tr>
            </thead>
            <tbody>
<%--                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>--%>
                    <asp:Repeater ID="rptMR" runat="server">
                        <ItemTemplate>
                        <tr>
                            <td><%#Eval("InventoryShort")%></td>
                            <td><%#Eval("CommType")%></td>
                            <td><%#Eval("InventorySKU")%></td>
                            <td><asp:LinkButton CommandName="Download" CommandArgument='<%#Eval("InventoryID") %>' ID="LinkButton2" runat="server"
                                Text='<%#Eval("Attachment")%>' ToolTip="Edit"></asp:LinkButton>
                            </td>
                            <td><%#Eval("createdby")%></td>
                            <td><%#Eval("createdts")%></td>
                            <td>
                                <asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("InventoryID") %>' ID="TempEdit" runat="server"
                                Text="<img src='images/edit.png' />" ToolTip="Edit"></asp:LinkButton>
                                <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("InventoryID")%>' ID="TempDel" runat="server"
                                Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
                            </td>
                        </tr>
                        </ItemTemplate>
                    </asp:Repeater>
<%--                    </ContentTemplate>
    	                <Triggers>
    	                    <asp:AsyncPostBackTrigger ControlID="btnListRefresh" EventName="serverclick" />
    	                </Triggers>
                    </asp:UpdatePanel>--%>

             </tbody>

            </table>
    			
        </div><!--table listing!-->
            
        </div>

    </section><!--sales dashboard!-->
            
    <section class='e_detailpop' id='edittemplate'  style="display:none;height:100%"><!--Email Detail Pop up!-->
	    <div class="create_edit" style="width:40%">
    	        <div class="close_btn1"><img src="images/close.png"  style="cursor:pointer" onclick='$("#edittemplate").hide();' /></div>
            <h3 class="template_heading">Add / Edit Inventory Master</h3>
            <div class="create_form"><!--Create Edit form!-->
                   
       		<div class="create_row"><!--row !-->
       		    <div class="create_left" style="width:80%"><!--left section!-->
           	        <div class="field_name"><label>Name:</label></div>
                    <div class="inp_f">
                        <asp:TextBox ID="pName" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvpName" Display="Dynamic" ValidationGroup="pMic"
                            runat="server" ControlToValidate="pName" InitialValue="">
                            <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                        </asp:RequiredFieldValidator>
                    </div>
                </div><!--left section!-->
            </div>
       		<div class="create_row"><!--row !-->
       		    <div class="create_left" style="width:80%"><!--left section!-->
           	        <div class="field_name"><label>Type:</label></div>
                    <div class="inp_f">
                        <asp:DropDownList ID="pType" runat="server">
                            <asp:ListItem Value="Email"></asp:ListItem>
                            <asp:ListItem Value="Mailing"></asp:ListItem>
                        </asp:DropDownList>                        
                    </div>
                    <div class="clr"></div>
                </div><!--left section!-->
                <div class="clr"></div>
            </div>
       		<div class="create_row"><!--row !-->
       		    <div class="create_left" style="width:80%"><!--left section!-->
           	        <div class="field_name"><label>SKU:</label></div>
                    <div class="inp_f">
                        <asp:TextBox ID="pSKU" runat="server" MaxLength="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvpSKU" Display="Dynamic" ValidationGroup="pMic"
                            runat="server" ControlToValidate="pSKU" InitialValue="">
                            <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                        </asp:RequiredFieldValidator>
                    </div>
                </div><!--left section!-->
                <div class="clr"></div>
            </div>

            <div class="create_row"><!--row !-->
                <div class="create_left" style="width:80%"><!--left section!-->
                    <div class="field_name"><label>Upload file</label></div>
                    <div class="inp_f">
                        <input type="file" id="pfileUpload" runat="server" /> 
                    </div>
                    <div class="clr"></div>
                </div><!--left section!-->
                <div class="clr"></div>
            </div><!--row !-->

            <div class="create_row" id="divatt" runat="server"><!--row !-->
                <div class="create_left" style="width:80%"><!--left section!-->
                    <div class="field_name"><label>Upload file</label></div>
                    <div class="inp_f">
                        <asp:Literal ID="pfile" runat="server"></asp:Literal> 
                        <asp:LinkButton ID="LinkButton1" runat="server"
						    Text="<img src='images/attach_ico.png' style='width: 7%'/>" OnClick="showattachment" ></asp:LinkButton>
                        <asp:LinkButton ID="lnkdelAttach" runat="server"
						    Text="<img src='images/delete.png' />" OnClientClick="return delconfirm();" ></asp:LinkButton>
                    </div>
                    <div class="clr"></div>
                </div><!--left section!-->
                <div class="clr"></div>
            </div><!--row !-->

       		<div class="create_row"><!--row !-->
       		    <div class="create_left"><!--left section!-->
                    <div class="create_button">
                        <asp:LinkButton ID="lnkSaveList" CssClass="ok" runat="server" ValidationGroup="pMic" >Save</asp:LinkButton>
                    </div>
                    <div class="clr"></div>
                </div><!--left section!-->
                <div class="clr"></div>
            </div>


            </div><!--Create Edit form!-->
        </div>
    </section><!--Email Detail Pop up!-->

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

