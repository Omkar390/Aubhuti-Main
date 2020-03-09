<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="GroupReportDetail.aspx.vb" Inherits="GroupReportDetail" title="Group Report" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script type="text/javascript">

        window.onload = function () {
            $body.addClass("loading");
        };
        //display please wait!!
        $body = $("body");
        $(document).on({
            ajaxStart: function () { $body.addClass("loading"); },
            ajaxStop: function () { $body.removeClass("loading"); }
        });


        function showvariables() {
            $("#showvariables").show();
        }
        function backAway() {
            //if it was the first page
            if (history.length === 1) {
                window.location = "http://salesshark.us/"
            } else {
                history.back();
            }
        }
    </script>
        
    <section class="container"><!--sales dashboard!-->
        <input type="button" id="btnListRefresh" runat="server" style="display:none" />
        <input type="hidden" id="hdnListId" runat="server" />

	    <div class="ac_list_main">
			<asp:HiddenField ID="hdnUrl" runat="server" />
    	    <div class="heading_row space_new crasul_sec">
          	    <h2><asp:Label runat="server" ID="lblTitle">Accounts</asp:Label>&nbsp;&nbsp;&nbsp;
                <asp:Label runat="server" id="lblVars" style="font-size: medium;"></asp:Label>
                <a class="new_btn" href="#" id="btnedit" onclick="showvariables();" runat="server">Edit</a>
                <a class="new_btn" href="#" id="btnback" runat="server" >Back</a></h2>&nbsp;&nbsp;&nbsp;
                <!-- onclick="JavaScript:window.history.back(1);return false;" -->
                <div class='clr'></div>    
            </div>

            <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>

            <asp:Literal ID="rpthdr1" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist1" runat="server"></asp:Literal>

            <br />
            <asp:Literal ID="rpthdr2" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist2" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr3" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist3" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr4" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist4" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr5" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist5" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr6" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist6" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr7" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist7" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr8" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist8" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr9" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist9" runat="server"></asp:Literal>                        

            <br />
            <asp:Literal ID="rpthdr10" runat="server"></asp:Literal>
            <asp:Literal ID="rptlist10" runat="server"></asp:Literal>                        

<%--            <div class="new_btab dataTables_wrapper"><!--table listing!--> 
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="myexample display nowrap ac_list"  id="sort_tab">
            	    <thead>
                    <tr>
                        <th>VendorNum</th>
                        <th>VendorName</th>
                        <th>AddressLine1</th>
                        <th>City</th>
                        <th>Zip</th>
                        <th>Country</th>
                        <th>POBox</th>
                        <th>Telephone</th>
                    </tr>
                    </thead>
                                                
                    <tbody>
                        <tr>
                            <td>1</td>
                            <td>Hangman Hardware</td>
                            <td>6400 Variel Avenue</td>
                            <td>Woodland Hills</td>
                            <td>United States</td>
                            <td>United States</td>
                            <td>Unknown-No</td>
                            <td></td>
                        </tr>
                    </tbody>
                </table>
            </div><!--table listing!-->--%>

        </div>
    </section><!--sales dashboard!-->
            
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

    <script type="text/javascript" >
        $(document).ready(function() {

            $('.sort_tab').DataTable({
                responsive: true,
                bFilter: false,
                "lengthMenu": [[10, 25, 50, 100, 500, -1], [10, 25, 50, 100, 500, 1000]],
                "iDisplayLength": 25
            });
        });
    </script>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

