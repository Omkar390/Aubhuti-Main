<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="GroupChartDetail.aspx.vb" Inherits="GroupChartDetail" title="Group Chart Detail" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="http://www.google.com/jsapi" type="text/javascript"></script>
    <script type="text/javascript">
        // Global variable to hold data
        // Load the Visualization API and the piechart package.
        google.load('visualization', '1', { packages: ['corechart'] });
    </script>

    <div>
        <asp:Literal ID="lt1" runat="server"></asp:Literal>
        <asp:Literal ID="lt2" runat="server"></asp:Literal>
        <asp:Literal ID="lt3" runat="server"></asp:Literal>
        <asp:Literal ID="lt4" runat="server"></asp:Literal>
        <asp:Literal ID="lt5" runat="server"></asp:Literal>
    </div>   

    <div id="chart_div1" style="top: 0px;width: 600px; height: 350px; padding-bottom:15px;"></div>   
    <div id="chart_div2" style="top: 300px;width: 600px; height: 350px;padding-bottom:15px;"></div>   
    <div id="chart_div3" style="top: 600px;width: 600px; height: 350px;padding-bottom:15px;"></div>   
    <div id="chart_div4" style="top: 600px;width: 600px; height: 350px;padding-bottom:15px;"></div>   
    <div id="chart_div5" style="top: 600px;width: 600px; height: 350px;padding-bottom:15px;"></div>   

</asp:Content>