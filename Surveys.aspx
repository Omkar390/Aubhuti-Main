<%@ Page Language="VB" MasterPageFile="~/AccountMaster.master" AutoEventWireup="false" CodeFile="Surveys.aspx.vb" Inherits="Surveys" title="Survey" %>
                        
<asp:Content ID="Content1" ContentPlaceHolderID="ContentMain" Runat="Server">
    <div class="heading_row crasul_sec">
    	<h2>Surveys</h2>
    	<asp:Label ID="lblErrorMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
        <div class="clr"></div>
    </div>
    
    <div class="email_tab">
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="responsive contact_tab"  id="sort_tab">
            <thead>
                <tr>
                    <th><span>Date</span></th>
                    <th width="89%"><span>Contact</span></th>
                </tr>
            </thead>
            <tbody>
                <asp:Repeater ID="rptSurvey" runat="server">
                    <ItemTemplate>
                        <tr>
                            <td width="11%" valign="top"><%#Eval("mtgdate")%>
                                <asp:HiddenField ID="hdnsurveyresultheaderid" runat="server" Value='<%#Eval("surveyresultheaderid")%>' /></td>
                            <td width="89%" valign="top">
         	                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="survey_table">
                                    <tr>
                                        <td colspan="2"><%#Eval("Contact")%></td>
                                    </tr>
                                    <asp:Repeater ID="rptSurveyDetail" runat="server">
                                        <ItemTemplate>
                                        <tr>
                                            <td width="44%"><strong><%#Eval("surveyquestion")%></strong></td>
                                            <td width="56%"><span><%#Eval("answer")%></span></td>
                                        </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>                
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>                                
            </tbody>
        </table>
	</div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPop" Runat="Server">
</asp:Content>

