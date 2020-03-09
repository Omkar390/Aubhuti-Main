<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="isrdash.aspx.vb" Inherits="isrdash" title="isrdash" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="https://www.google.com/jsapi" type="text/javascript"></script>
    <link href="https://fonts.googleapis.com/css?family=Lato:300,300i,400,400i,700,700i,900,900i" rel="stylesheet">
    <link href="https://fonts.googleapis.com/css?family=Roboto+Condensed:300,300i,400,400i,700,700i" rel="stylesheet">


    <style type="text/css"> 
        .donutCell
        {
            position: relative;
        }

        .donutDiv
        {
            width: 165px;
            height: 165px;
        }
        .centerLabel
        {
            position: absolute;
            left: 2px;
            top: 2px;
            width: 165px;
            line-height: 165px;
            text-align: center;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 36px;
            color: white;
        }
    </style>

    <script type="text/javascript">


        $(document).ready(function ($) {

            function toDate(dateStr) {
                var parts = dateStr.split("-");
                return new Date(parts[2], parts[1], parts[0]);
            }

            $(".daterangepicker .dropdown-menu").css("display", "none");

            var start = new Date(document.getElementById("ctl00_ContentPlaceHolder1_hdnstartdate").value);
            var end = new Date(document.getElementById("ctl00_ContentPlaceHolder1_hdnenddate").value);

            function cb(start, end) {
                $('#reportrange span').html(moment(start).format('MMMM D, YYYY') + ' - ' + moment(end).format('MMMM D, YYYY'));
                document.getElementById("ctl00_ContentPlaceHolder1_hdnstartdate").value = moment(start).format().substring(0, 10);
                document.getElementById("ctl00_ContentPlaceHolder1_hdnenddate").value = moment(end).format().substring(0, 10);
            }

            $('#reportrange').daterangepicker({
                startDate: start,
                endDate: end,
                ranges: {
                    'Prior Week': [moment().subtract(1, 'week').startOf('week'), moment().subtract(1, 'week').endOf('week')],
                    'Prior Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
                }
            }, cb);

            cb(start, end);

            //'Current Month': [moment().startOf('month'), moment().endOf('month')],
            //'Prior Quarter': [moment().quarter(moment().quarter() - 1).startOf('quarter'), moment().quarter(moment().quarter() - 1).endOf('quarter')],
            //'Current Quarter': [moment().startOf('quarter'), moment().endOf('quarter')],
            //'Prior Year and YTD': [moment().subtract(1, 'year').quarter(1).startOf('quarter'), moment()],
            //'Prior Year': [moment().subtract(1, 'year').quarter(1).startOf('quarter'), moment().subtract(1, 'year').quarter(4).endOf('quarter')],
            //'Year to Date': [moment().quarter(1).startOf('quarter'), moment()]
            
            $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
                document.getElementById("ctl00_ContentPlaceHolder1_hdnstartdate").value = moment(picker.startDate).format().substring(0, 10);
                document.getElementById("ctl00_ContentPlaceHolder1_hdnenddate").value = moment(picker.endDate).format().substring(0, 10);
                document.getElementById("ctl00_ContentPlaceHolder1_hdnnumdays").value =
                    (moment(picker.endDate).diff(moment(picker.startDate), 'days')) * parseInt(document.getElementById("ctl00_ContentPlaceHolder1_hdnconfigdays").value);
            });

        });
    </script>


    <style type="text/css">
        .isr-bg{background-color:#3f4954;}
        *{margin:0; padding:0; box-sizing:border-box; -moz-box-sizing:border-box; -webkit-box-sizing:border-box;}
        img{max-width:100%;}
        .clr{clear:both;}
        .img-full{display:block; width:100%;}
        .isr-wrap{color:#fff; font-family: 'Lato', sans-serif; font-size:22px;}
        .isr-container{width:90%; max-width:1450px; margin:0 auto;}
        .isr-top{padding:15px 0;}
        .it-left{float:left;}
        .it-right{float:right;}
        .isr-left{float:left; width:calc(65% - 30px);}
        .isr-right{float:right; width:35%;}
        .isr-holder{background-color:#28313c; margin-bottom:30px; border-radius:25px; padding:40px 0;}
        .isr-box{text-align:center; float:left; padding:0 20px; width:100%;}
        .three-box .isr-box{width:33.33%;}
        .four-box .isr-box{width:25%; margin-bottom:30px;}
        .img-holder{display:inline-block; margin-bottom:10px;}
        .isr-top .field{float:left; font-size:18px; background-image:url("images/arrow_down.png"); background-color:#3f4954; width:350px; padding:5px 20px; border-radius:10px; background-repeat:no-repeat; background-position: calc(100% - 20px) center; height:50px; border:1px solid #28313c; color:#fff; appearance:none; -webkit-appearance:none; -moz-appearance:none; -o-appearance:none; -ms-appearance:none;}
        .isr-top .field:focus{outline:0;}
        .isr-top h1{font-size:26px; font-weight:500; padding-top:6px;}
        .isr-count{font-family: 'Roboto Condensed', sans-serif; display:block; font-size:70px; font-weight:300; line-height:148px; width:146px; height:146px; border-radius:100%; -moz-border-radius:100%; -webkit-border-radius:100%; -o-border-radius:100%;}
        .orange-bg{background:#ff4817;}
        .green-bg{background:#30c96a;}
        .yellow-bg{background:#de9118;}
        .round-btn{background-color:#ff4817; color:#fff; font-size:18px; text-align:center; padding:0 35px; height:50px; line-height:50px; display:inline-block; text-decoration:none; border-radius:50px;}
        .meeting-actions a{display:inline-block; color:#fff; margin:0 5px; border-radius:10px; font-size:16px; text-decoration:none; padding:10px; width:95px;}
        .ms-wrap h2{font-size:30px; font-weight:500;}
        .ms-wrap h3{margin-bottom:20px; font-weight:500;}
        .meeting-states{margin:20px 0;}
        .isr-holder2{padding:53px 0;}

        @media all and (max-width:1500px){
        .isr-box p{font-size:20px;}
        }

        @media all and (max-width:1280px){
        .isr-container{max-width:980px;}
        .isr-top .field{width:300px;}
        }
        @media all and (max-width:980px){
        .isr-container{max-width:750px;}
        .isr-left{width:100%; margin-bottom:20px;}
        .isr-right{width:100%;}
        }
        @media all and (max-width:767px){
        .isr-container{max-width:92%;}
        .isr-left{width:100%; margin-bottom:20px;}
        .isr-right{width:100%;}
        .it-left, .it-right{width:100%;}
        .three-box .isr-box{width:100%;}
        .four-box .isr-box{width:100%;}
        .isr-top h1{font-size:24px; margin-bottom:20px;}
        .isr-left{margin-bottom:15px;}
        .isr-holder{padding:15px 0;}
        .isr-box{padding:15px;}
        .meeting-actions a{margin:5px;}
        .four-box .isr-box{margin:0;}
        .isr-count{font-size:60px; width:125px; height:125px; line-height:125px;}
        }
    </style>

    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });

        google.charts.setOnLoadCallback(drawChartC);
        google.charts.setOnLoadCallback(drawChartE);
        google.charts.setOnLoadCallback(drawChartI);
        google.charts.setOnLoadCallback(drawChartM);

        <% =donutC %>
        <% =donutE %>
        <% =donutI %>
        <% =donutM %>
        
        var options = {
            pieHole: 0.8,
            pieSliceTextStyle: {
                color: 'black',
            },
            legend: 'none',
            pieSliceText: 'none',
            backgroundColor: 'transparent',
            pieSliceBorderColor: "transparent",
            chartArea: { 'width': '100%', 'height': '100%' },
            slices: [{ color: '#29C7CA' }, { color: '#525C67' }]
        };

    </script>

    <section class="isr-wrap" style="background-color:#3f4954">
        <input type="hidden" id="hdnstartdate" runat="server" />
        <input type="hidden" id="hdnenddate" runat="server" />
        <asp:HiddenField ID="hdnnumdays" runat="server" />
        <asp:HiddenField ID="hdnconfigdays" runat="server" />

	    <div class="isr-top">
		    <div class="isr-container">
			    <div class="isr-left">
				    <div class="it-left"><h1>ISR Daily Dashboard</h1></div>
				    <div class="clr"></div>
			    </div>
		    </div>
		    <div class="isr-container">
			    <div class="isr-left">
				    <div class="it-left">
                        <div id="reportrangediv" style="float:left;width:120px;margin-left: 3%;margin-top: 0.4%;">
				            <div id="reportrange" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%;">
                                <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;
                                <span runat="server" id="txtDate" style="background-color: black;"></span> <b class="caret"></b>
                            </div>
                        </div>
				    </div>
				    <div class="it-right">
                        <asp:DropDownList CssClass="field" ID="ddlISR" runat="server" DataTextField="UserName" DataValueField="userid">
                        </asp:DropDownList>
				    </div>
				    <div class="clr"></div>
			    </div>
			    <div class="isr-right">
				    <div class="it-right">
					    <a href="#" id="btnView" runat="server" class="round-btn">View</a>
				    </div>
				    <div class="clr"></div>
			    </div>
			    <div class="clr"></div>
		    </div>
	    </div>
                    
	    <div class="isr-container">
		    <div>
		    <div class="isr-left">
			    <div class="isr-holder three-box">
				    <div class="isr-box">
                        <div class="donutCell" style="margin-left: 12%;">
                            <div id="donut_1" class="donutDiv"></div>
                            <div class="centerLabel">
                                <asp:Literal ID="litdCalls" runat="server"></asp:Literal>/<asp:Literal ID="littotdaysCalls" runat="server"></asp:Literal>
                            </div>
                        </div>
                        <p style="margin-top: 7%;">Calls</p>
				    </div>
				    <div class="isr-box">
                        <div class="donutCell" style="margin-left: 12%;">
                            <div id="donut_2" class="donutDiv"></div>
                            <div class="centerLabel">
                                <asp:Literal ID="litdEmails" runat="server"></asp:Literal>/<asp:Literal ID="littotdaysEmails" runat="server"></asp:Literal>
                            </div>
                        </div>
					    <p style="margin-top: 7%;">Emails</p>
				    </div>
				    <div class="isr-box">
                        <div class="donutCell" style="margin-left: 12%;">
                            <div id="donut_3" class="donutDiv"></div>
                            <div class="centerLabel">
                                <asp:Literal ID="litdInventory" runat="server"></asp:Literal>/<asp:Literal ID="littotdaysInventory" runat="server"></asp:Literal>
                            </div>
                        </div>
					    <p style="margin-top: 7%;">Inventory</p>
				    </div>
				    <div class="clr"></div>
			    </div>
			    <div class="isr-holder isr-holder2 three-box">
				    <div class="isr-box">
					    <div class="img-holder"><span class="isr-count orange-bg">
                            <asp:Literal ID="litCurStreamTasks" runat="server"></asp:Literal>
                        </span></div>
					    <p>Current Stream Tasks</p>
				    </div>
				    <div class="isr-box">
					    <div class="img-holder"><span class="isr-count orange-bg">
                            <asp:Literal ID="litActivities" runat="server"></asp:Literal>
                        </span></div>
					    <p>Activities</p>
				    </div>
				    <div class="isr-box">
					    <div class="img-holder"><span class="isr-count orange-bg">
                            <asp:Literal ID="litMtgToConfirm" runat="server"></asp:Literal>
					    </span></div>
					    <p>Meeting to Confirm</p>
				    </div>
				    <div class="clr"></div>
			    </div>
		    </div>
		    <div class="isr-right">
			    <div class="isr-holder ms-wrap">
				    <div class="isr-box">
					    <h2>Meeting YTD Summary</h2>
<%--					    <div class="meeting-states"><img src="images/meeting-states.png" alt="" /></div>--%>
                        <div id="piechart" style="width: 350px; height: 350px;margin-left:2%;margin-top:5%"></div>
                        <h3>Total Meeting: <asp:Literal id="litTotMeet" runat="server"></asp:Literal></h3>
					    <div class="meeting-actions">
						    <a class="green-bg" href="">Done</a>
						    <a class="orange-bg" href="">Pending</a>
						    <a class="yellow-bg" href="">Canceled</a>
					    </div>
				    </div>
				    <div class="clr"></div>
			    </div>
		    </div>
		    <div class="clr"></div>
		    </div>
		    <div class="isr-holder isr-holder2 four-box">
			    <div class="isr-box">
				    <div class="img-holder"><span class="isr-count green-bg">
                        <asp:Literal ID="litCallsPerformed" runat="server"></asp:Literal>
                        </span></div>
				    <p>Calls Performed</p>
			    </div>
			    <div class="isr-box">
				    <div class="img-holder"><span class="isr-count green-bg">
                        <asp:Literal ID="litEmailsSent" runat="server"></asp:Literal>
                    </span></div>
				    <p>Emails Sent</p>
			    </div>
			    <div class="isr-box">
				    <div class="img-holder"><span class="isr-count green-bg">
                        <asp:Literal ID="litInvManaged" runat="server"></asp:Literal>
                    </span></div>
				    <p>Inventory Managed</p>
			    </div>
			    <div class="isr-box">
				    <div class="img-holder"><span class="isr-count green-bg">
                        <asp:Literal ID="litStreamTasksComp" runat="server"></asp:Literal>
                        </span></div>
				    <p>Stream Tasks Done</p>
			    </div>
			    <div class="isr-box">
				    <div class="img-holder"><span class="isr-count green-bg">
                        <asp:Literal ID="litActPerf" runat="server"></asp:Literal></span></div>
				    <p>Activities Performed</p>
			    </div>
			    <div class="isr-box">
				    <div class="img-holder"><span class="isr-count green-bg">
                        <asp:Literal ID="litMtgConfirmed" runat="server"></asp:Literal>
                    </span></div>
				    <p>Meeting Confirmed</p>
			    </div>
			    <div class="clr"></div>
		    </div>
	    </div>
    </section>

</asp:Content>