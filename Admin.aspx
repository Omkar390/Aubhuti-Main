<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Admin.aspx.vb" Inherits="Admin" title="Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="container"><!--sales dashboard!-->
	    <div class="sales_con">
        <ul class="menu_cat  menu_cat_new">
           <li class="account">
        	    <!-- <a href="EmailSetup.aspx"> -->
        	    <a href="EmailSetup.aspx">
        		    <img src="images/email_setting.png" />
            	    <h3>Email Setup</h3>
                </a>
            </li>
           <li class="account">
        	    <!-- <a href="EmailSetup.aspx"> -->
        	    <a href="Signature.aspx">
        		    <img src="images/email_setting.png" />
            	    <h3>Signature Setup</h3>
                </a>
            </li>
            <% If Session("userRole") = "Administrator" Then %>
				<li class="contact">
					<!-- <a href="webtracking.html"> -->
					<a href="WebTracking.aspx">
						<img src="images/tracking_setup.png" />
						<h3>Web Tracking <br />Setup</h3>
					</a>
				</li>
				<li class="meet">
					<!-- <a href="user.html"> -->
					<a href="ClientUsers.aspx">
						<img src="images/ad_user.png" />
						<h3>Users</h3>
					</a>
				</li>
				<li class="market">
					<a href="CompanyInfo.aspx">
						<img src="images/comp_ifo.png" />
						<h3>Company Info</h3>
					</a>
				</li>
				<li class="rfp">
					<!-- <a href="datausage.html"> -->
					<a href="#">
						<img src="images/data.png" />
						<h3>Data Usage</h3>
					</a>
				</li>
				 <li class="weblog">
					<!-- <a href="datasetup.html"> -->
					<a href="DataSetup.aspx">
						<img src="images/data_setup.png">
						<h3>Data Setup</h3>
					</a>
				</li>
				<li class="admin">
					<!-- <a href="service_offer.html"> -->
					<a href="#">
						<img src="images/service_offer.png">
						<h3>Service Offering</h3>
					</a>
				</li>
				<li class="contact">
					<!-- <a href="webtracking.html"> -->
					<a href="PasswordPolicy.aspx">
						<img src="images/tracking_setup.png" />
						<h3>Password Policy</h3>
					</a>
				</li>
            <%End If %>
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
                <li class="account">        	        
        	        <a href="UserPreference.aspx">
        		        <img src="images/email_setting.png" />
            	        <h3>User Preferences</h3>
                    </a>
                <li class="contact">
					<a href="CreateMyList.aspx">
						<img src="images/tracking_setup.png" />
						<h3>Create Custom List</h3>
					</a>
				</li>

        </ul>
        </div>
    </section><!--sales dashboard!-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

