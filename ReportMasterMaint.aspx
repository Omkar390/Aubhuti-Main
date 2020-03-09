<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="ReportMasterMaint.aspx.vb" Inherits="ReportMasterMaint" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    
    <script type="text/javascript">
        function showwaitpopup(strTab) {
            Page_ClientValidate(strTab);
            alert((Page_IsValid));
            if (Page_IsValid) {
                return true;
            }
            else
                return false;
        }
    </script>

    <br /><br /><br />
    <h3 class="page-title">Create/Update Report</h3>
    <h4><label ID="lblMsg" style="color:Red" runat="server"></label></h4>

<div id="Div1" runat="server" style="display:none">
    <input name="showwait" class="popup" type="button" value="Show" rel="popup1" id="showwait" />
</div>    

    <div class="v_detail">
    <div class="row-fluid">
        <table class="tab_split" cellspacing="0" rules="all" border="1" style="border-collapse:collapse;">
            <tr>
                <td class="form_sec span2">&nbsp;</td>
                <td>
                    <div>
                        <asp:Button CssClass="btn btn-mini btn-success" ID="btnUpdate" runat="server" CausesValidation="True"  ToolTip="Update" Text="Save" ValidationGroup="valReport" />
                        <asp:Button CssClass="btn btn-mini btn-warning" ID="btnCancel" runat="server" CausesValidation="False" ToolTip="Cancel" Text="Cancel"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="form_sec span2">Report Name</td>
                <td>
                    <div style="margin-left:0%">
                        <asp:TextBox ValidationGroup="valReport" CssClass="textfield" MaxLength="100" ID="txtName" runat="server" 
                           TabIndex="1"  Text=''></asp:TextBox>
                        <asp:RequiredFieldValidator Display="Static" ID="rfvtxtname" runat="server" ControlToValidate="txtName" 
                            ValidationGroup="valReport" ErrorMessage="Required"></asp:RequiredFieldValidator>
                        <asp:CheckBox ID="chkActive" runat="server" TabIndex="1" /> Active ?
                    </div>
                    <div style="margin-left:0%">
                    </div>
                </td>
            </tr>
            <tr>
                <td class="form_sec span2">Title</td>
                <td>
                    <div style="margin-left:0%">
                        <asp:TextBox ValidationGroup="valReport" CssClass="textfield" MaxLength="300" ID="txtTitle" runat="server" 
                           TabIndex="2"  Text=''></asp:TextBox>
                        <asp:RequiredFieldValidator Display="Static" ID="rfvtxtTitle" runat="server" ControlToValidate="txtTitle" 
                            ValidationGroup="valReport" ErrorMessage="Required"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="form_sec span2">Help Text</td>
                <td>
                    <div style="margin-left:0%">
                        <textarea runat="server" id="txtHelp" cols="75" style="width:80%" rows="2" tabindex="3"></textarea>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="form_sec span2">Type</td>
                <td>
                    <div style="margin-left:0%">
                        <asp:DropDownList ID="ddlReportType" runat="server" Width="20%" TabIndex="4" AppendDataBoundItems="true"
                            DataTextField="SurveytypeShort" DataValueField="SurveytypeID" >
                            <asp:ListItem Text="Select " Value=""></asp:ListItem>
                            <asp:ListItem Text="Top Level " Value="M"></asp:ListItem>
                            <asp:ListItem Text="Drill Down " Value="C"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator Display="Static" ID="rfvddlReportType" runat="server" ControlToValidate="ddlReportType" 
                            ValidationGroup="valReport" ErrorMessage="Required" InitialValue=""></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="form_sec span2">Icon</td>
                <td>
                    <div style="margin-left:0%">
                        <asp:DropDownList ID="ddlIcon" runat="server" Width="25%" TabIndex="5" AppendDataBoundItems="true"
                            DataTextField="IconDesc" DataValueField="RptIconId" >
                            <asp:ListItem Text="Select Icon" Value="0"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="form_sec span2">Type</td>
                <td>
                    <div style="margin-left:0%">
                        <asp:DropDownList ID="ddlCategory" runat="server" Width="20%" TabIndex="6" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select " Value=""></asp:ListItem>
                            <asp:ListItem Text="Client" Value="Client"></asp:ListItem>
                            <asp:ListItem Text="Master" Value="Master"></asp:ListItem>
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator Display="Static" ID="rfvddlCategory" runat="server" ControlToValidate="ddlCategory" 
                            ValidationGroup="valReport" ErrorMessage="Required" InitialValue=""></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            
<%--            <tr>
                <td class="form_sec span3">Report Columns</td>
                <td>
                    <div style="margin-left:0%">
                        <asp:TextBox ValidationGroup="valReport" CssClass="textfield" MaxLength="2" ID="txtRptCols" runat="server" 
                           TabIndex="4"  Text='0' Width="5%"></asp:TextBox>
                        <asp:CompareValidator Display="Static" ID="cvtxtRptCols" Operator="DataTypeCheck" runat="server" Type="Integer" 
                            ValidationGroup="valReport" ErrorMessage="Numeric" ControlToValidate="txtRptCols"></asp:CompareValidator>
                    </div>
                </td>
            </tr>--%>
            <tr>
                <td class="form_sec span2">Query</td>
                <td>
                    <div style="margin-left:0%">
                        <textarea runat="server" id="txtQuery" cols="75" style="width:80%" rows="8" tabindex="7"></textarea>
                        <asp:RequiredFieldValidator ID="rfvtxtRptQuery" runat="server" ControlToValidate="txtQuery" 
                            ValidationGroup="valReport" ErrorMessage="Required" Display="Dynamic"></asp:RequiredFieldValidator>
                        <%--<asp:Button CssClass="btn btn-mini btn-success" ID="btnGenGrd" runat="server" CausesValidation="True"  ToolTip="Update" Text="Generate Cols" />--%>
                    </div>
                </td>
            </tr>
        </table>

    
    <br />
    
    <h4 class="title">Field Details</h4>
        <table cellpadding="10" cellspacing="0" border="1" width="80%" class="tab_split">
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
                    <td><asp:Literal runat="server" ID="litFName" visible='<%#Not CBool(IsInEditMode)%>' Text='<%#Eval("FName")%>'></asp:Literal></td>
                    <td><asp:DropDownList ID="ddlRptColsFormat" runat="server" Width="50%" TabIndex="3">
                            <asp:ListItem Text="TL" Value="TL"></asp:ListItem>
                            <asp:ListItem Text="TR" Value="TR"></asp:ListItem>
                            <asp:ListItem Text="DD" Value="DD"></asp:ListItem>
                            <asp:ListItem Text="DT" Value="DT"></asp:ListItem>
                            <asp:ListItem Text="D0" Value="D0"></asp:ListItem>
                            <asp:ListItem Text="D2" Value="D2"></asp:ListItem>
                        </asp:DropDownList></td>
                    <td><asp:DropDownList ID="ddlRptShowHide" runat="server" Width="80%" TabIndex="3">
                            <asp:ListItem Text="Y" Value="Y"></asp:ListItem>
                            <asp:ListItem Text="N" Value="N"></asp:ListItem>
                        </asp:DropDownList></td>
                    <td><asp:textbox runat="server" ID="litRptColWidths" style="width:90%" visible='<%#Not CBool(IsInEditMode)%>' Text='<%#Eval("RptColWidths")%>'></asp:textbox>
                        <asp:CompareValidator ID="cvlitRptColWidths" runat="server" ControlToValidate="litRptColWidths" Display="Dynamic"
                            ErrorMessage="Numeric" Operator="DataTypeCheck" SetFocusOnError="True" Type="Integer"></asp:CompareValidator></td>
                    <td><asp:textbox runat="server" ID="litRptLinks" style="width:95%" visible='<%#Not CBool(IsInEditMode)%>' Text='<%#Eval("RptLinks")%>'></asp:textbox></td>
                </tr>
                </ItemTemplate>
            </asp:Repeater> 

        </table>
        </div>
    </div>




        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder3" runat="server">

</asp:Content>