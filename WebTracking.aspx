<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="WebTracking.aspx.vb" Inherits="WebTracking" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="container"><!--container section!-->
	    <div class="heading_row space_new crasul_sec"><!--top heading!-->
	        <h2>Web Tracking
                <a href="Admin.aspx">Back</a>
            </h2>
        </div><!--top heading!-->
        <br /><br /><br />
        <div class="web_tracking">
     	    <p><span>Copy the code below on your website in order to view Weblog reports</span></p>
            <div class="edit_fm">
                <asp:Label ID="txtScript" runat="server"></asp:Label>
                <%--<textarea class="ana_code" rows="10" enabled="false" id="txtScript" runat="server"></textarea>--%>
            </div>
             <br />
             <br />
         </div>
     
    </section><!--container section!-->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

