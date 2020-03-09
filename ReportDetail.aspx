<%@ Page Title="" Language="VB" MasterPageFile="~/ReportMaster.master" AutoEventWireup="false" CodeFile="ReportDetail.aspx.vb" Inherits="ReportDetail" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentMain" Runat="Server">

    <style type="text/css">
        /* Start by setting display:none to make this hidden.
           Then we position it in relation to the viewport window
           with position:fixed. Width, height, top and left speak
           for themselves. Background we set to 80% white with
           our animation centered, and no-repeating */
        .mymodal {
            display:    none;
            position:   fixed;
            z-index:    90000;
            top:        0;
            left:       0;
            height:     100%;
            width:      100%;
            background: rgba( 255, 255, 255, .9 ) 
                        url('images/ajax-loader.gif') 
                        50% 50% 
                        no-repeat;
        }

        /* When the body has the loading class, we turn
           the scrollbar off with overflow:hidden */
        body.loading {
            overflow: hidden;   
        }

        /* Anytime the body has the loading class, our
           mymodal element will be visible */
        body.loading .mymodal {
            display: block;
        }
    </style>

    <script type="text/javascript">
        function backAway() {
            //if it was the first page
            if (history.length === 1) {
                window.location = "http://salesshark.us/"
            } else {
                history.back();
            }
        }
        function showhideqf() {
            if($(".t-fields").css('display') == 'none'){
                $(".t-fields").show();
            }
            else {
                $(".t-fields").find("input:text").each(function() {
                    this.value = "";
                })
                $(".t-fields").hide();
            }
        }

        //hide year ddl
        function hideyear() {
            document.getElementById("ddlYQM").style.display = 'none';
            document.getElementById("ddlYVal").style.display = 'none';
        }
        function showyear() {
            document.getElementById("ddlYQM").style.display = 'block';
            document.getElementById("ddlYVal").style.display = 'block';
        }
        function hiderange() {
            document.getElementById("reportrangediv").style.visibility = 'hidden';
        }
        function showrange() {
            document.getElementById("reportrangediv").style.visibility = 'visible';
        }

        function exporttoexcel() {
            $(".dt-button.buttons-excel.buttons-html5").click();
        }
        function exporttopdf() {
            $(".dt-button.buttons-pdf.buttons-html5").click();
        }
        function exporttoprint() {
            $(".dt-button.buttons-print.buttons-html5").click();
        }
        function exporttocsv() {
            $(".dt-button.buttons-csv.buttons-html5").click();
        }

        function flipimgtick(tickid) {
            //if (tickid.src.includes('images/tick.png')) {
            if(tickid.src.indexOf("images/tick.png") >= 0) {
                tickid.src = 'images/cross.png';
                var hdnid = document.getElementById(tickid.id.replace('tick', 'hdnsrc'));
                hdnid.value = 'images/cross.png';
            }
            else {
                tickid.src = 'images/tick.png';
                var hdnid = document.getElementById(tickid.id.replace('tick', 'hdnsrc'));
                hdnid.value = 'images/tick.png';
            }
            return false;
        }

        function flipimgcross(id) {
            return false;
        }
        function callrefreshtablefromsort(ctrl, col) {
            document.getElementById("hdnsortcol").value = col;
            document.getElementById("hdnsortdir").value = $(ctrl).attr('aria-label');
            document.getElementById("hdnsortcolname").value = ctrl.innerHTML;
            callrefreshtable();
        }

        function callrefreshtable() {
            //return false;
            var strVar = '';
            $("#displayTable input").each(function () {
                strVar = strVar + this.id + '=' + this.value + '|';
            });
            refreshtable(strVar);
        }
        function setyqm() {
            if (document.getElementById("ddlYQM").value == 'Year') {
                document.getElementById("ddlYVal").style.visibility = 'visible';
                document.getElementById("ddlQVal").style.display = 'none';
                document.getElementById("ddlMVal").style.display = 'none';
                document.getElementById("reportrangediv").style.visibility = 'hidden';
            }
            else if (document.getElementById("ddlYQM").value == 'Quarter') {
                document.getElementById("ddlYVal").style.visibility = 'visible';
                document.getElementById("ddlQVal").style.display = 'block';
                document.getElementById("ddlMVal").style.display = 'none';
                document.getElementById("reportrangediv").style.visibility = 'hidden';
            }
            else if (document.getElementById("ddlYQM").value == 'Month') {
                document.getElementById("ddlYVal").style.visibility = 'visible';
                document.getElementById("ddlMVal").style.display = 'block';
                document.getElementById("ddlQVal").style.display = 'none';
                document.getElementById("reportrangediv").style.visibility = 'hidden';
            }
            else if (document.getElementById("ddlYQM").value == 'Range') {
                document.getElementById("reportrangediv").style.visibility = 'visible';
                document.getElementById("ddlQVal").style.display = 'none';
                document.getElementById("ddlMVal").style.display = 'none';
                document.getElementById("ddlYVal").style.visibility = 'hidden';
            }
            callrefreshtable();
        }


        function contentPageLoad() {
            //refreshtable();
        }

        //$body2 = $("body");
        //window.onload = new function () {
        //    $body2.addClass("loading");
        //};
        //window.onunload = new function () {
        //    $body2.removeClass("loading");
        //};
        //window.onbeforeunload = new function () {
        //    $body2.removeClass("loading");
        //};


        $(document).ready(function ($) {

            jQuery.extend(jQuery.fn.dataTableExt.oSort, {
                "extract-date-pre": function(value) {
                    var date = $(value, 'span')[0].innerHTML;
                    date = date.split('/');
                    return Date.parse(date[0] + '/' + date[1] + '/' + date[2])
                },
                "extract-date-asc": function(a, b) {
                    return ((a < b) ? -1 : ((a > b) ? 1 : 0));
                },
                "extract-date-desc": function(a, b) {
                    return ((a < b) ? 1 : ((a > b) ? -1 : 0));
                }
            });

            //display please wait!!
            $body = $("body");
            $(document).on({
                ajaxStart: function () { $body.addClass("loading"); },
                ajaxStop: function () { $body.removeClass("loading"); }
            });

            $(".daterangepicker .dropdown-menu").css("display", "none");


            var start = moment().subtract(180, 'days');
            var end = moment();

            function cb(start, end) {
                $('#reportrange span').html(start.format('MMMM D, YYYY') + ' - ' + end.format('MMMM D, YYYY'));
                document.getElementById("hdnstartdate").value = start.format('YYYY-MM-DD');
                document.getElementById("hdnenddate").value = end.format('YYYY-MM-DD');
            }

            $('#reportrange').daterangepicker({
                startDate: start,
                endDate: end,
                ranges: {
                    'Prior Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')],
                    'Current Month': [moment().startOf('month'), moment().endOf('month')],
                    'Prior Quarter': [moment().quarter(moment().quarter() - 1).startOf('quarter'), moment().quarter(moment().quarter() - 1).endOf('quarter')],
                    'Current Quarter': [moment().startOf('quarter'), moment().endOf('quarter')],
                    'Prior Year and YTD': [moment().subtract(1, 'year').startOf('month').startOf('day'), moment()],
                    'Prior Year': [moment().subtract(1, 'year').quarter(1).startOf('quarter'), moment().subtract(1, 'year').quarter(4).endOf('quarter')],
                    'Year to Date': [moment().quarter(1).startOf('quarter'), moment()]
                }
            }, cb);

            cb(start, end);

            
            $('#reportrange').on('apply.daterangepicker', function (ev, picker) {
                document.getElementById("hdnstartdate").value = picker.startDate.format('YYYY-MM-DD');
                document.getElementById("hdnenddate").value = picker.endDate.format('YYYY-MM-DD');

                callrefreshtable();
            });

            $('.configure').on('click', function () {
                $('.conf_opt').toggle();
            });

        });

    function showfilters() {
        $("#showfilters").show();
    }

    function showvariables() {
        $("#showvariables").show();
    }

    function showhidecols() {
        $("#showhidecols").show();
    }

    function addsearch() {
        //form string of params
        callrefreshtable();


        /*
        var table = $('#displayTable').DataTable();
        $("#displayTable input").each(function () {
            alert($(this).parent().index());
                table
                    .column($(this).parent().index() + ':visible')
                    .search(this.value)
                    .draw();
            });
            */



            //$("#displayTable thead input").on('keyup change', function () {
            //    table
            //        .column($(this).parent().index() + ':visible')
            //        .search(this.value)
            //        .draw();
            //});
        }
        function refreshtable(inputstr) {
            //alert(picker.startDate.format('YYYY-MM-DD'));
            //alert(picker.endDate.format('YYYY-MM-DD'));
            var qString = "?" + window.location.href.split("?")[1];
            $.ajax({
                type: "POST",
                //"url": 'json.txt',
                url: "reportdetail.aspx/ConvertDataTabletoJson",
                /// pass the the search textbox values from inputstr!!!!! or may be pass that whole string - 
                data: '{RANGEVISIBLE: "' + document.getElementById("reportrangediv").style.visibility + '",YQMVISIBLE: "' + document.getElementById("ddlYQM").style.display + '",ddlYQM: "' + document.getElementById("ddlYQM").value + '",ddlQVal: "' + document.getElementById("ddlQVal").value + '",ddlMVal: "' + document.getElementById("ddlMVal").value + '",startdate: "' + document.getElementById("hdnstartdate").value + '",enddate: "' + document.getElementById("hdnenddate").value + '",ddlYVal: "' + document.getElementById("ddlYVal").value + '",inputstr: "' + inputstr + '",ddlpager: "' + document.getElementById("ctl00_ctl00_ContentPlaceHolder1_ContentMain_ddlPager").value + '",sortcol: "' + document.getElementById("hdnsortcol").value + '",sortdir: "' + document.getElementById("hdnsortdir").value + '",sortcolname: "' + document.getElementById("hdnsortcolname").value + '",qString: "' + qString + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (str) {
                    //alert(str.d);
                    //var json = str;

                    var json = JSON.parse(str.d[0]);
                    var tableHeaders = '';
                    var tableHeaders2 = '';
                    var j = 0;
                    var res;
                    if (inputstr != '') {
                        res = inputstr.split("|");
                    }
                    $.each(json.columns, function (i, val) {
                        tableHeaders += "<th style='text-align:center' onclick='callrefreshtablefromsort(this," + i + ");'>" + val + "</th>";
                        if (inputstr != '') {
                            tableHeaders2 += "<td>" + "<input id='" + val + "' type='text' value='" + res[j].split("=")[1] + "' onkeypress='if(event.keyCode == 13) {addsearch(); return false;}'/>" + "</td>";
                        }
                        else {
                            tableHeaders2 += "<td>" + "<input id='" + val + "'  type='text' onkeypress='if(event.keyCode == 13) {addsearch(); return false;}'/>" + "</td>";
                        }
                        j = j + 1;
                    });
                    if(tableHeaders2 != ''){
                        //tableHeaders += "<th></th>";
                        tableHeaders2 = tableHeaders2.substring(0,tableHeaders2.length-7)
                        tableHeaders2 += "style='width:50%;margin-right:5px' />&nbsp;&nbsp;&nbsp;<input type='button' style='vertical-align:top' name='' onclick='addsearch();' />" + "</td>";
                        //tableHeaders2 += "<td><input type='button' name='' onclick='addsearch();' />" + "</td>";
                        //onclick='refreshtableclick();' 
                        /*
                        if (inputstr == '') {
                            $("#headerDiv").empty();
                            $("#headerDiv").append('<table id="displayTableH" width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive"><thead><tr class="t-fields">' + tableHeaders2 + '</tr></thead></table>');
                        }
                        */
                    }
                    else {
                        tableHeaders2 = 'No data found.'
                    }
                    $("#tableDiv").empty();
                    $("#tableDiv").append('<table id="displayTable" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive"><thead><tr class="t-fields" style="display:none">' + tableHeaders2 + '</tr><tr>' + tableHeaders + '</tr></thead>' + str.d[3] + '</table>');
                    //$("#tableDiv").append('<table id="displayTable" width="100%" border="0" cellspacing="0" cellpadding="0" class="ac_list responsive"><thead><tr>' + tableHeaders + '</tr></thead></table>');

                    // populate the search textbox values!! from inputstr!!!!


                    //alert('1');
                    //var json2 = json.slice(0, -1);
                    //var json3 = JSON.parse(json2);
                    //alert(json);
                    $('#displayTable').dataTable(json);
                    //alert('2');
                    $(".dataTables_filter").hide();
                    $(".dt-buttons").hide();

                    $("#displayTable").width(str.d[1]);
                    if (str.d[2] == "1") {
                        //$('#displayTable tr:last').css("background-color", "rgb(252, 176, 64)");
                        $('#displayTable tfoot th').css("background-color", "rgb(252, 176, 64)");
                    }
                    //hide the div if ------------------------
                    var hasval = 0;
                    $(".t-fields").find("input:text").each(function () {
                        if (this.value == "") {
                            hasval = 0;
                        }
                        else {
                            hasval = 1;
                            return false;
                        }
                    });
                    if (hasval == 0) {
                        $(".t-fields").hide();
                    }
                    else {
                        $(".t-fields").show();
                    }
                    //hide the div if ------------------------
                    //$body3 = $("body");
                    //$body3.removeClass("loading");
                }
            });
            //setTimeout("addsearch();", 10000);
            //$("#ddlPager").css("padding-top", "0px");
            $("#ddlPager").css("line-height", "17px");
            $("#ddlYQM").css("line-height", "17px");
            $("#ddlYVal").css("line-height", "17px");
            $("#ddlQVal").css("line-height", "17px");
            $("#ddlMVal").css("line-height", "17px");
            //$body4 = $("body");
            //$body4.removeClass("loading");

        }


        function refreshtableclick() {
            var table = $('#displayTable').DataTable();
            $("#displayTable thead input").on('keyup change', function () {
                table
                    .column($(this).parent().index() + ':visible')
                    .search(this.value)
                    .draw();
            });
            //$.ajax({
            //    type: "POST",
            //    url: "reportdetail2.aspx/ConvertDataTabletoJson",
            //    data: '{ddlYQM: "' + document.getElementById("ddlYQM").value + '",ddlQVal: "' + document.getElementById("ddlQVal").value + '",ddlMVal: "' + document.getElementById("ddlMVal").value + '",startdate: "' + document.getElementById("hdnstartdate").value + '",enddate: "' + document.getElementById("hdnenddate").value + '",ddlYVal: "' + document.getElementById("ddlYVal").value + '"}',
            //    contentType: "application/json; charset=utf-8",
            //    dataType: "json",
            //    success: function (str) {
            //        var json = JSON.parse(str.d);
            //        table = $('#example').DataTable();
            //        table.destroy();
            //        table.dataTable(json);
            //        $(".dataTables_filter").hide();
            //    }
            //});
        }
    </script>

    <input type="hidden" id="hdnstartdate" />
    <input type="hidden" id="hdnenddate" />
    <input type="hidden" id="hdnsortcol" value="1" />
    <input type="hidden" id="hdnsortcolname" value="" />
    <input type="hidden" id="hdnsortdir" value="sorting" />
    
    <section class="container"><!--sales dashboard!-->
	<div class="ac_list_main">
        <asp:HiddenField ID="hdnUrl" runat="server" />
    	<div class="heading_row space_new crasul_sec">
            	<h2><asp:Label runat="server" ID="lblTitle">Accounts</asp:Label>&nbsp;&nbsp;&nbsp;<a id="btnNew" runat="server" href="Account.aspx?BindType=A&AcctId=New">New</a>
<asp:Label runat="server" id="lblVars" style="font-size: medium;"></asp:Label>
                    <a class="new_btn" href="#" id="btnedit" onclick="showvariables();" runat="server">Edit</a>&nbsp;&nbsp;&nbsp;
                                        <a class="new_btn" href="#" id="btnback" runat="server">Back</a>
<!--                    onclick="JavaScript:window.history.back(1);return false;" -->
            	</h2>
                <div class="configure"  runat="server" id="divgear"><i class="fa fa-cog"></i>
                	<div class="conf_opt">
                    	<ul>
                            <li>
                                <a href="#" onclick="showhideqf();">Quick Filter</a>
                            </li>
                            <li>
                                <a href="#" onclick="showhidecols();">Select Fields</a>
                            </li>
                            <li>
                                <a href="#" onclick="showfilters();">Filter</a>
                            </li>
							<li><a href="#" onclick="showvariables();">Variables</a></li>
                       		<li><a href="#" onclick="exporttoexcel();">Excel</a></li>
                       		<li><a href="#" onclick="exporttocsv();">CSV</a></li>
                       		<li><a href="#" onclick="exporttopdf();">PDF</a></li>
                        </ul>
                    </div>
                </div>
                <div id="divFilters" style="margin-right:320px">
                    <div class="year pull-right" style="width:850px;margin-right:-350px">
                        <div>
                	        <select class="dropdown" onchange="setyqm();" id="ddlYQM" style="width:120px;float:left;display:none">
                    	        <option value="Year">Year</option>
                                <option value="Quarter">Quarter</option>
                                <option value="Month">Month</option>
                            </select>
                        </div>
                        <div style="float:left;margin-left:10px;width:70px">
                	        <select class="dropdown" onchange="setyqm();" id="ddlYVal" style="display:none">
                                <option value="2020">2020</option>
                                <option value="2019">2019</option>
                                <option value="2018">2018</option>
                                <option value="2017">2017</option>
                    	        <option value="2016">2016</option>
                            </select>
                        </div>
                        <div style="float:left;margin-left:10px;width:70px">
                	        <select class="dropdown" onchange="setyqm();" style="display:none" id="ddlQVal">
                    	        <option value="1">Q1</option>
                                <option value="2">Q2</option>
                                <option value="3">Q3</option>
                                <option value="4">Q4</option>
                            </select>
                        </div>
                        <div style="float:left;margin-left:10px;width:70px">
                	        <select class="dropdown" onchange="callrefreshtable();" style="display:none" id="ddlMVal">
                    	        <option value="1">Jan</option>
                    	        <option value="2">Feb</option>
                    	        <option value="3">Mar</option>
                    	        <option value="4">Apr</option>
                    	        <option value="5">May</option>
                    	        <option value="6">Jun</option>
                    	        <option value="7">Jul</option>
                    	        <option value="8">Aug</option>
                    	        <option value="9">Sep</option>
                    	        <option value="10">Oct</option>
                    	        <option value="11">Nov</option>
                    	        <option value="12">Dec</option>
                            </select>
                        </div>
                        <div id="reportrangediv" style="float:left;margin-left:-260px;width:70px;visibility:hidden;margin-top:10px">
				            <div id="reportrange" style="background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%;">
                                <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;
                                <span runat="server" id="txtDate"></span> <b class="caret"></b>
                            </div>
                        </div>
                        <div style="float:right;margin-left:10px;width:270px">Show Records: 
                	        <select class="dropdown" onchange="callrefreshtable();" id="ddlPager" style="width:70px;padding:-0px 5px 0px 6px" runat="server">
                    	        <option value="10">10</option>
                    	        <option value="20">20</option>
                    	        <option value="50" selected="selected">50</option>
                    	        <option value="100">100</option>
                    	        <option value="500">500</option>
                    	        <option value="1000">1000</option>
                    	        <option value="100%">All</option>
                            </select>
                        </div>

<%--				        <div id="reportrange" class="pull-right" style="margin-right:50px !important;background: #fff; cursor: pointer; padding: 5px 10px; border: 1px solid #ccc; width: 100%;visibility:hidden">
                            <i class="glyphicon glyphicon-calendar fa fa-calendar"></i>&nbsp;
                            <span runat="server" id="txtDate"></span> <b class="caret"></b>
                        </div>--%>
                    </div>
                </div>
                <div class="clr"></div>
                <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>

            </div>  
        <div>
            <h4><asp:Label runat="server" ID="lblFilters" style="color: #f7761f;"></asp:Label>&nbsp;&nbsp;&nbsp;</h4>
        </div>

        <div class="list_table"><!--table listing!--> 
             <div id="headerDiv"></div>
             <div id="tableDiv"></div>
        </div><!--table listing!-->
        
        
    </div>
    </section><!--sales dashboard!-->
<%--                <a href="#" class="new_btn">New</a>--%>

<section class='e_detailpop' id='showhidecols' style="display:none;height:100%"><!--Email Detail Pop up!-->
	<div class="create_edit">
    	<div class="close_btn1"><img src="images/close.png" style="cursor:pointer" onclick='$("#showhidecols").hide();' /></div>
       	<h3>Columns to Show/Hide ?</h3>
       <div class="create_form"><!--from section!-->
			<asp:Repeater ID="rptGrdShowHide" runat="server">
				<ItemTemplate>
       	        <div class="add-box">
                    <div class="ctrls">
                        <a href=""><img enableviewstate="true" id="tick" runat="server" src='<%#Eval("ShowHide")%>' onclick="return flipimgtick(this);" alt="" /></a>
                        <asp:HiddenField ID="hdnsrc" runat="server" Value='<%#Eval("ShowHide")%>' />
                    </div>
                    <input id="GrdCol" runat="server" type="text" value='<%#Eval("GrdCol")%>'  readonly="readonly" />
                </div>
				</ItemTemplate>
			</asp:Repeater>
            <div class="clr"></div>
            <div class="create_button"><a href="#" class="cancel" onclick='$("#showhidecols").hide();'>Cancel</a>&nbsp;&nbsp; 
                <a href="#" id="btnShowHide" runat="server" class="ok" onserverclick="btnShowHide_ServerClick">Submit</a>
            </div>
       </div><!--from section!-->
      
    </div>
</section><!--Email Detail Pop up!-->

<section class='e_detailpop' id='showfilters' style="display:none;height:100%"><!--Email Detail Pop up!-->
	<div class="create_edit">
    	<div class="close_btn1"><img src="images/close.png" style="cursor:pointer" onclick='$("#showfilters").hide();'  /></div>
       <h3>Filter</h3>
       <div class="filter_form"><!--filter form section!-->
       		<div class="filter_row_top">
            	<div class="filter1">
                    <asp:DropDownList ID="fddlcond" runat="server" >
                        <asp:ListItem Text="And" Value="And"></asp:ListItem>
                        <asp:ListItem Text="Or" Value="Or"></asp:ListItem>
                    </asp:DropDownList>
            	</div>
                <div class="filter2">
                    <asp:DropDownList ID="fddlcols" runat="server" style="width:100%" DataTextField="GrdCol" DataValueField="GrdCol">
                    </asp:DropDownList>
                </div>
                <div class="filter3">
                    <asp:DropDownList ID="fddlcond2" runat="server" >
                        <asp:ListItem Text="=" Value="="></asp:ListItem>
                        <asp:ListItem Text=">" Value=">"></asp:ListItem>
                        <asp:ListItem Text="<" Value="<"></asp:ListItem>
                        <asp:ListItem Text="Contains" Value="Like"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="filter2"><input id="fwhere" runat="server" type="text" required /></div>
                <div class="filter1">&nbsp;&nbsp; 
					<asp:LinkButton ID="lnkAdd" runat="server" Text="<img src='images/add.png' />" ToolTip="Add"></asp:LinkButton>
                </div>
                <div class="clr"></div>
            </div>
			<asp:Repeater ID="rptfilter" runat="server">
				<ItemTemplate>
                    <div class="filter_row"><!--row sec!-->
            	        <div class="filter1"><input type="text" placeholder='<%#Eval("cond")%>' readonly="readonly" /></div>
                        <div class="filter2"><input type="text" placeholder='<%#Eval("fieldcol")%>' readonly="readonly" /></div>
                        <div class="filter3"><input type="text" placeholder='<%#Eval("cond2")%>' readonly="readonly" /></div>
                        <div class="filter2"><input type="text" placeholder='<%#Eval("fieldvalue")%>' readonly="readonly" /></div>
                        <div class="filter1">&nbsp;&nbsp; 
							<asp:LinkButton CommandName="Del" CommandArgument='<%#Eval("rowid")%>' ID="fDel" runat="server"
								Text="<img src='images/Delete.png' />" ToolTip="Delete"></asp:LinkButton>
                        </div>
                        <div class="clr"></div>
                    </div><!--row sec!-->
                </ItemTemplate>
            </asp:Repeater>
            
            <div class="create_form">
        		<div class="create_button"><a class="cancel" href="#" onclick='$("#showfilters").hide();'>Cancel</a>&nbsp;&nbsp; <a class="ok" href="#" onclick='$("#showfilters").hide();callrefreshtable();' runat="server" onserverclick="submitfilter">Submit</a></div>
       		</div>
       </div><!--filter form section!-->
       
    </div>
</section><!--Email Detail Pop up!-->


<section class='e_detailpop' id='showvariables' style="display:none;height:100%"><!--Email Detail Pop up!-->
	<div class="create_edit" style="width: 35%;">
    	<div class="close_btn1"><img src="images/close.png" style="cursor:pointer" onclick='$("#showvariables").hide();'  /></div>
       <h3>Variables</h3>
       <div class="filter_form"><!--filter form section!-->
			<asp:Repeater ID="rptvariables" runat="server">
				<ItemTemplate>
                    <div class="filter_row"><!--row sec!-->
                        <div class="filter1">&nbsp;&nbsp;</div>
                        <div class="filter2"><asp:Label runat="server" ID="pName" Text='<%#Eval("fieldcol")%>'></asp:Label></div>
                        <%--<div class="filter1">&nbsp;&nbsp;</div>--%>
                        <div class="filter2"><input id="pValue" runat="server" type="text" value='<%#Eval("fieldvalue")%>'  /></div>
                        <div class="clr"></div>
                    </div><!--row sec!-->
                </ItemTemplate>
            </asp:Repeater>
            
            <div class="create_form">
        		<div class="create_button">
                    <a class="cancel" href="#" onclick='$("#showvariables").hide();'>Cancel</a>&nbsp;&nbsp; 
                    <asp:LinkButton ID="lnkSaveVar" runat="server" CssClass="ok">Submit</asp:LinkButton>
        		</div>
       		</div>
       </div><!--filter form section!-->
       
    </div>
</section><!--Email Detail Pop up!-->

</asp:Content>

