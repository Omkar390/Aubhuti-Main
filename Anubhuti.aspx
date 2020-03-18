<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="Anubhuti.aspx.vb" Inherits="Anubhuti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<div class="sales_con">
        <ul class="menu_cat">
            <li class="rfp">
            	<a href="SurveyList.aspx"> <!--  rfp.html -->
            		<img src="images/rfp2.png">
                	<h3>Define Tests</h3>
                </a>
            </li>
             <li class="account">
            	<a href="userlist.aspx"> <!--  rfp.html -->
            		<%--<img src="images/rfp2.png">--%><img src="images/list.png" />
                	<h3>Assign Tests</h3>
                </a>
            </li>
            <li class="task">
            	<a href="Users.aspx"><%--<a href="admin_user.html">--%>
            		<img src="images/ad_user.png">
                	<h3>User List</h3>
                </a>
            </li>
             <li class="account">
            	<a href="surveyresult.aspx"> <!--  rfp.html -->
            		<%--<img src="images/rfp2.png">--%><img src="images/list.png" />
                	<h3>View Results</h3>
                </a>
            </li>
		</ul>
    </div>
</asp:Content>
