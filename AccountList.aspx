<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="AccountList.aspx.vb" Inherits="AccountList" title="Accounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<section class="container"><!--sales dashboard!-->
    <div class="ac_list_main">
	    <div class="heading_row space_new">
            <h2>Accounts <a id="btnNew" runat="server" onclick='editct();' href="Account.aspx?BindType=A&AcctId=New">New</a></h2>
            
            <div class="right_fsrc">
                <div class="configure"><i class="fa fa-cog"></i>
        	        <div class="conf_opt">
            	        <ul>
               		        <li><a href="export_pop.html">Export</a></li>
                            <li><a href="select_pop.html">Select Fields</a></li>
                            <li><a href="filter_pop.html">Filter</a></li>
                        </ul>
                    </div>
                </div>
                <div class="clr"></div>



            </div>
            
            
        </div>
                        
        <div class="new_btab dataTables_wrapper"><!--table listing!--> 
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list" id="grdreport">
        	    <thead>
                  <tr>
                    <th width="30%"><span>Company</span></th>
                    <th width="13%"><span>City</span></th>
                    <th width="9%"><span>State</span></th>
                    <th width="10%"><span># Contacts</span></th>
                    <th width="13%"><span>Micro Region</span></th>
                    <th width="13%"><span>Annual Revenue</span></th>
                    <th width="12%"><span>Last Activity Date</span></th>
                  </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptAccounts" runat="server">
                        <ItemTemplate>
                            <tr>
                 	            <td><a href='<%#Eval("hrefLink")%>'><asp:literal ID="litAccount" runat="server" Text='<%#Eval("Company")%>'></asp:literal></a></td>
                                <td><asp:literal ID="litCity" runat="server" Text='<%#Eval("City")%>'></asp:literal></td>
                                <td><asp:literal ID="litState" runat="server" Text='<%#Eval("State")%>'></asp:literal></td>
                                <td><asp:literal ID="litContacts" runat="server" Text='<%#Eval("# Contacts")%>'></asp:literal></td>
                                <td><asp:literal ID="litMicroRegion" runat="server" Text='<%#Eval("Micro Region")%>'></asp:literal></td>
                                <td><asp:literal ID="litAnnualRevenue" runat="server" Text='<%#Eval("Annual Revenue")%>'></asp:literal></td>
                                <td><asp:literal ID="litLastAvtivityDate" runat="server" Text='<%#Eval("Last Activity Date")%>'></asp:literal></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                 
             </tbody>
                 <tr>
             	    <td colspan="8" height="30"></td>
                  </tr>
                 
                  <tr class="total_bar">
             	    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>	
                 </tr>
            </table>
        </div><!--table listing!-->
    </div>
</section>    
</asp:Content>

