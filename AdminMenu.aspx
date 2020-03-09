<%@ Page Language="VB" MasterPageFile="~/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="AdminMenu.aspx.vb" Inherits="AdminMenu" title="Admin Menu" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="sales_con">
        <ul class="menu_cat menu_cat_new">
           <li class="opp">
            	<a href="Clients.aspx">
            		<img src="images/client.png">
                	<h3>Clients</h3>
                </a>
            </li>
            <li class="task">
            	<a href="Users.aspx"><%--<a href="admin_user.html">--%>
            		<img src="images/ad_user.png">
                	<h3>Users</h3>
                </a>
            </li>
           <li class="account">
            	<a href="ReportsList.aspx">
            		<img src="images/pam.png">
                	<h3>PAM Reports</h3>
                </a>
            </li>
            <li class="report">
            	<a href="ShowReports.aspx">
            		<img src="images/report.png">
                	<h3>Admin Reports</h3>
                </a>
            </li>            
           <li class="report">
            	<a href="GroupReportsList.aspx">
            		<img src="images/pam.png">
                	<h3>Group Reports</h3>
                </a>
            </li>
				<li class="account">
					<!-- <a href="service_offer.html"> -->
					<a href="ReportDetail.aspx?RptName=Bugs">
						<img src="images/tracking_setup.png">
						<h3>Open Bugs</h3>
					</a>
				</li>
				<li class="account">
					<!-- <a href="service_offer.html"> -->
					<a href="ReportDetail.aspx?RptName=FixedBugs">
						<img src="images/tracking_setup.png">
						<h3>Resolved Bugs</h3>
					</a>
				</li>
				<li class="account">
					<!-- <a href="service_offer.html"> -->
					<a href="ReportDetail.aspx?RptName=QueryBugs">
						<img src="images/tracking_setup.png">
						<h3>Query Bugs</h3>
					</a>
				</li>
                 <li class="report">
            	    <a href="GroupChartsList.aspx">
            		    <img src="images/pam.png">
                	    <h3>Group Charts</h3>
                    </a>
                </li>
                <li class="report">
            	    <a href="DailyMonitor.aspx">
            		    <img src="images/report.png">
                	    <h3>Daily Job Monitor</h3>
                    </a>
                </li>
        </ul>
    </div>
</asp:Content>

