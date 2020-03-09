<%@ Page Language="VB" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="UserMaint.aspx.vb" Inherits="UserMaint" title="User Maintenance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">
        function pwdconfirm() {
            if (confirm('Paswowrd will be reset and sent via email. Are you sure?')) {
                return true;
            }
            else {
                return false;
            }
        }

        function newclient(val) {
            //alert('1');
            if (val == 0) {
                document.getElementById("divClient").style.display = 'block';
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdnRemClients").value == 0) {
                    alert("No more clients to add.");
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_pClients").value = 0;
                    document.getElementById("ctl00_ContentPlaceHolder1_pddlAccessRole").value = "Administrator";
                    document.getElementById("ctl00_ContentPlaceHolder1_pddlMktgRole").value = "AE";
                    $("#editlist").show();
                }
            }
            else {
                document.getElementById("divClient").style.display = 'none';
                $("#editlist").show();
            }
        }

        //function SaveList() {

        //    var validated = Page_ClientValidate('pList');
        //    alert(validated);
        //    if (!validated)
        //        return false;

        //}
        
    </script>

<section class="container"><!--sales dashboard!-->
	
    <div class="sale_process"><!--sale process page!-->
    	<h2 id="HdrPage" runat="server"></h2>
        <asp:HiddenField ID="hdnuserId" runat="server" />
        <asp:HiddenField ID="hdnclientIduserId" runat="server" />
        <asp:HiddenField ID="hdnclientId" runat="server" />
        <asp:HiddenField ID="hdnPassword" runat="server" />
        <asp:HiddenField ID="hdnRemClients" runat="server" />

        <div class="process_main"> 
            <div class="process_form"><!--process form section!-->
                <div class="process_left">
                    <label>First Name</label>
                    <asp:TextBox ID="txtFirstName" runat="server" MaxLength="50" Style="width:95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtFirstName" Display="Dynamic" ValidationGroup="usermaint"
                        runat="server" ControlToValidate="txtFirstName" InitialValue="">
                        <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                    </asp:RequiredFieldValidator>
                </div>
                <div class="process_left process_rt">
                    <label>Last Name</label>
                    <asp:TextBox ID="txtLastName" runat="server" MaxLength="50" Style="width:95%"></asp:TextBox>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->
            
            <div class="process_form"><!--process form section!-->
                <div class="process_left">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" MaxLength="100" Style="width:95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtEmail" Display="Dynamic" ValidationGroup="usermaint"
                        runat="server" ControlToValidate="txtEmail" InitialValue="">
                        <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="regextxtPCtEmail" runat="server" ControlToValidate="txtEmail"
                        Display="Dynamic" ErrorMessage="Invalid Email" ValidationGroup="usermaint"
                        ValidationExpression="^[A-Za-z0-9](([_\.\-]?[a-zA-Z0-9]+)*)@([A-Za-z0-9]+)(([\.\-]?[a-zA-Z0-9]+)*)\.([A-Za-z]{2,})$">
                    </asp:RegularExpressionValidator>
                </div>
                <div class="process_left process_rt">
                    <label>Phone</label>
                    <input id="txtPhone" runat="server" type="text" Style="width:95%"/>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->

            <div class="process_form"><!--process form section!-->
                <div class="process_left">
                    <label>Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" MaxLength="50" TextMode="Password" Style="width:95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtPassword" Display="Dynamic" ValidationGroup="usermaint"
                        runat="server" ControlToValidate="txtPassword" InitialValue="">
                        <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revtxtPassword" runat="server" ValidationGroup="usermaint"
                    ControlToValidate="txtPassword" Display="Dynamic" ErrorMessage="7 character minimum for password"
                    ValidationExpression="^([a-zA-Z0-9~!@#$%^&*()]{7,20})$">Password needs to be alteast 7 characters</asp:RegularExpressionValidator>
                </div>
                <div class="process_left process_rt">
                    <label>Confirm Password</label>
                    <asp:TextBox ID="txtCnfPassword" runat="server" MaxLength="50" TextMode="Password" Style="width:95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtCnfPassword" Display="Dynamic" ValidationGroup="usermaint"
                        runat="server" ControlToValidate="txtCnfPassword" InitialValue="">
                        <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                    </asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revtxtCnfPassword" runat="server" ValidationGroup="usermaint"
                    ControlToValidate="txtCnfPassword" Display="Dynamic" ErrorMessage="7 character minimum for password"
                    ValidationExpression="^([a-zA-Z0-9~!@#$%^&*()]{7,20})$">Password needs to be alteast 7 characters</asp:RegularExpressionValidator>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->

            <div class="process_form"><!--process form section!-->
                <div class="process_left">
                    <label>Access Role</label>
                    <asp:DropDownList ID="ddlRole" runat="server" Style="width:95%">
                        <asp:ListItem Value="Select " Text="Select "></asp:ListItem>
                        <asp:ListItem Value="Admin" Text="Admin"></asp:ListItem>
                        <asp:ListItem Value="User" Text="User"></asp:ListItem>
                        <asp:ListItem Value="EmailCheck" Text="Email Check"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfvddlRole" Display="Dynamic" ValidationGroup="usermaint"
                        runat="server" ControlToValidate="ddlRole" InitialValue="Select ">
                        <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                    </asp:RequiredFieldValidator>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->
            
            <div class="process_form"><!--process form section!-->
                <div class="process_left">
                    <label>Title</label>
                    <input id="txtTitle" runat="server" type="text" Style="width:95%"/>
                </div>
                <div class="clr"></div>
            </div><!--process form section!-->
         </div>
        
        <div class="process_form"><!--process form section!-->
            <asp:LinkButton ID="lnkResPwd" CssClass="link_obj" runat="server" OnClientClick="return pwdconfirm();">Reset Password</asp:LinkButton>&nbsp;&nbsp; 
        </div><!--process form section!-->
                
        <div class="create_button text_left">
            <asp:LinkButton ID="lnkSave" CssClass="ok" runat="server" ValidationGroup="usermaint">Save</asp:LinkButton>&nbsp;&nbsp; 
            <a href="Users.aspx" class="cancel">Cancel</a>
        </div>

        <br /><br />

    	<div class="heading_row topspace crasul_sec"><!--heading row!-->
            <h2>Assigned Clients
                <a href="#" class="new_btn" onclick='newclient(0);'>New</a>
            </h2>
                
            <div class="clr"></div>
        </div>
        <div class="process_main"> 
            <div class="list_table"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive" id="sort_tab">
            	    <thead>
                    <tr>
                        <th>Clients</th>
                        <th>Access Role</th>
                        <th>Marketing Role</th>
                        <th>Active?</th>
                        <th>Action</th>
                    </tr>
                    </thead>

                    <tbody>
                    <asp:Repeater ID="rptClients" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td><%#Eval("client_name")%></td>
                                <td><%#Eval("userRole")%></td>
                                <td><%#Eval("userGroup")%></td>
                                <td><%#Eval("Active")%></td>
                                <td>
                                    <asp:HiddenField ID="hdnclientid" runat="server" Value='<%#Eval("client_id")%>' />
                                    <asp:HiddenField ID="hdnuserGroup" runat="server" Value='<%#Eval("userRole")%>' />
                                    <asp:HiddenField ID="hdnuserRole" runat="server" Value='<%#Eval("userGroup")%>' />
                                    <asp:LinkButton CommandName="Edit" CommandArgument='<%#Eval("userclientid")%>' ID="TempEdit" runat="server"
                                    Text="<img src='images/edit.png' />" ToolTip="Edit"></asp:LinkButton>
                                    <asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("userclientid")%>' ID="TempDel" runat="server"
                                    Text="<img src='images/Delete.png' />" ToolTip="Delete" OnClientClick="return delconfirm();"></asp:LinkButton>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                    </tbody>
                </table>
            </div><!--table listing!-->

        </div>
                  
    </div><!--sale process page!-->
    
</section><!--sales dashboard!-->

    <section class='e_detailpop' id='editlist' style="display:none;height:100%"><!--Email Detail Pop up!-->
	    <div class="create_edit">
    	    <div class="close_btn1"><img src="images/close.png" onclick='$("#editlist").hide();'  /></div>
            <h3>Add Client</h3>
            <div class="create_form"><!--Create Edit form!-->
                <div class="create_row" id="divClient" style="display:none"><!--row !-->
       		        <div class="create_left"><!--left section!-->
            	        <div class="field_name"><label>Client</label></div>
                        <div class="inp_f">
                            <asp:DropDownList ID="pClients" runat="server" Width="90%" DataTextField="client_name" DataValueField="client_id" ValidationGroup="pList"></asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvpClients" Display="Dynamic" ValidationGroup="pList"
                                runat="server" ControlToValidate="pClients" InitialValue="0">
                                <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                            </asp:RequiredFieldValidator>
                        </div>
                    </div><!--left section!-->
                    <div class="clr"></div>
                </div><!--row !-->
                <div class="create_row"><!--row !-->
       		        <div class="create_left"><!--left section!-->
            	        <div class="field_name"><label>Access Role</label></div>
                        <div class="inp_f">
                            <asp:DropDownList ID="pddlAccessRole" runat="server" Style="width:90%" ValidationGroup="pList">
                                <%--<asp:ListItem Value="Select " Text="Select "></asp:ListItem>--%>
                                <asp:ListItem Value="Administrator" Text="Administrator"></asp:ListItem>
                                <asp:ListItem Value="User" Text="User"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvpddlAccessRole" Display="Dynamic" ValidationGroup="pList"
                                runat="server" ControlToValidate="pddlAccessRole" InitialValue="">
                                <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="clr"></div>
                    </div><!--left section!-->
                    <div class="create_left create_rt"><!--create right !-->
        	            <div class="field_name"><label>Marketing Role</label></div>
                        <div class="inp_f">
                            <asp:DropDownList ID="pddlMktgRole" runat="server" Style="width:90%" ValidationGroup="pList">
                                <%--<asp:ListItem Value="Select " Text="Select"></asp:ListItem>--%>
                                <asp:ListItem Value="AE" Text="AE"></asp:ListItem>
                                <asp:ListItem Value="ISR" Text="ISR"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvpddlMktgRole" Display="Dynamic" ValidationGroup="pList"
                                runat="server" ControlToValidate="pddlMktgRole" InitialValue="">
                                <img src="images/star.gif" height="10px" width="10px" style="margin-top:5px;margin-left:5px"/>
                            </asp:RequiredFieldValidator>
                        </div>
                        <div class="clr"></div>
                    </div><!--create right !-->
                    <div class="clr"></div>
                </div><!--row !-->
                <div class="create_button">
                    <a href="#" class="cancel"  onclick='$("#editlist").hide();'  >Cancel</a>
                    <asp:LinkButton ID="btnSaveClient" CssClass="ok" runat="server" CausesValidation="false" ValidationGroup="pList">Submit</asp:LinkButton>
                </div>
            </div><!--Create Edit form!-->
        </div>
    </section><!--Email Detail Pop up!-->

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>
