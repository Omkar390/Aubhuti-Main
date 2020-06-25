<%@ Page Language="VB" AutoEventWireup="true" CodeFile="SurveyPage.aspx.vb" Inherits="survey_SurveyPage"  Async="true" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<!--[if lt IE 7 ]> <html class="ie ltie7 ltie8 ltie9" lang="en"> <![endif]-->
<!--[if IE 7 ]>    <html class="ie ie7 ltie8 ltie9" lang="en"> <![endif]-->
<!--[if IE 8 ]>    <html class="ie ie8 ltie9" lang="en"> <![endif]-->
<!--[if IE 9 ]>    <html class="ie ie9 ltie10" lang="en"> <![endif]-->
<!--[if gt IE 9]><!--> <html lang="en" class=""> <!--<![endif]-->

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta charset="utf-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge,chrome=1">
<meta name="keywords" content=""/>
<meta name="description" content=""/>
<meta name="viewport" content="width=device-width">
    <title></title>
<!--[if lt IE 9]><script src="http://html5shiv.googlecode.com/svn/trunk/html5.js"></script><![endif]-->

<link rel="stylesheet" type="text/css" href="css/jquery-ui-1.10.4.custom.css" />

<link rel="stylesheet" type="text/css" href="css/style.css"/>

<link href='http://fonts.googleapis.com/css?family=Open+Sans:400,600,700' rel='stylesheet' type='text/css' />

<script type="text/javascript">
    function validate(grpname) {
        var validated = Page_ClientValidate(grpname);
        if (validated) {
            return true;
        }
        else
            return false;
    }
    function validateadd() {
        if (document.getElementById("txtNewOption").value.trim() == '')
            return false;
        else
            return true;
    }

    function countCharMore(val) {
        var len = val.value.length;
        if (len >= 1000) {
            val.value = val.value.substring(0, 1000);
        } else {
        $('#charNumMore').text(1000 - len);
        }
    };  


    function countChar(val) {
        var len = val.value.length;
        if (len >= 100) {
            val.value = val.value.substring(0, 100);
        } else {
        $('#charNum').text(100 - len);
        }
    };

    function countCharTA(val) {
        var len = val.value.length;
        if (len >= 500) {
            val.value = val.value.substring(0, 500);
        } else {
            $('#charNum').text(500 - len);
        }
    };

    function hideall() {
    $("#lstYesNo").hide();
    $("#divSlider").hide();
    $("#divRadio").hide();
    $("#divCheckBoxes").hide();
    $("#divCalendar").hide();
    $("#divRadioYesNoCant").hide();
    $("#divStars").hide();
    $("#divFreeText").hide();
    $("#divFreeText100").hide();
    $("#divShowSort").hide();
    $("#divCamera").hide();
    $("#divMultipleText").hide();
	$("#divAgreeDisagree").hide();
}
function showdiv(n) {
    hideall();
        switch(n)
        {
            case 1:
                if ($('#<%=hdnAnswer.ClientID %>').val() == 'No')
                    $("#myonoffswitch").removeAttr('checked');
                else
                    $("#myonoffswitch").attr('checked');

                $("#lstYesNo").show();
                break;
            case 2:
                $(".step_slider").slider();
                $(".step_slider").slider('value', $('#<%=hdnAnswer.ClientID %>').val());
                $(".slider_step").text($('#<%=hdnAnswer.ClientID %>').val());
                $("#divSlider").show();
                break;
            case 3:
                var list = document.getElementById("ulRadio"); //Client ID of the radiolist
                if (list != null) {
                    var inputs = list.getElementsByTagName("input");
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].id == $('#<%=hdnAnswer.ClientID %>').val()) {
                            inputs[i].checked = true;
                            break;
                        }
                    }
                }
                $("#divRadio").show();
                break;
            case 4:
                $("#divCheckBoxes").show();
                break;
            case 5:
                $('.datepicker').datepicker();
                $('.datepicker').datepicker('setDate', $('#<%=hdnAnswer.ClientID %>').val());
                $("#divCalendar").show();
                break;
            case 6:
                var list = document.getElementById("ulRadioYesNoCant"); //Client ID of the radiolist
                if (list != null) {
                    var inputs = list.getElementsByTagName("input");
                    for (var i = 0; i < inputs.length; i++) {
                        if (inputs[i].id == $('#<%=hdnAnswer.ClientID %>').val()) {
                            inputs[i].checked = true;
                            break;
                        }
                    }
                }
                $("#divRadioYesNoCant").show();
                break;
            case 7:
                if ($('#<%=hdnAnswer.ClientID %>').val() != '') {
                    //var strID = ".star_rating .star ." + $('#<%=hdnAnswer.ClientID %>').val();
                    //$('#' + strID).click();
                    $(".star_rating").removeClass().addClass("star_rating active_" + $('#<%=hdnAnswer.ClientID %>').val());
                }
                $("#divStars").show();
                break;
            case 8:
                $("#txtFreeText").val($('#<%=hdnAnswer.ClientID %>').val());
                $("#divFreeText").show();
                break;
            case 9:
                $("#txtText100").val($('#<%=hdnAnswer.ClientID %>').val());
                $("#divFreeText100").show();
                break;
            case 10:
                $("#divShowSort").show();
                break;
            case 11:
                $("#divCamera").show();
                break;
			case 12:
				$("#divMultipleText").show();
				break;
			case 13:
				var list = document.getElementById("ulAgreeDisagree"); //Client ID of the radiolist
				if (list != null) {
					var inputs = list.getElementsByTagName("input");
					for (var i = 0; i < inputs.length; i++) {
						if (inputs[i].id == $('#<%=hdnAnswer.ClientID %>').val()) {
							inputs[i].checked = true;
							break;
						}
					}
				}
				$("#divAgreeDisagree").show();
				break;
           default:
        }
    }

    function saveanswer() {
        if (document.getElementById("txtTextAdd") == null)
            $('#<%=hdnAnswerAdd.ClientID %>').val('');
        else
            $('#<%=hdnAnswerAdd.ClientID %>').val(document.getElementById("txtTextAdd").value.trim());

        var anstypeval = $('#<%=hdnAnsType.ClientID %>').val();

        if ((anstypeval == 'Date')) {
            $('#<%=hdnAnswer.ClientID %>').val($(".datepicker").val());
        }
        else if ((anstypeval == 'STARS')) {
//        if ($("#hdnValidateMe").val() == '1') {
//            if (($("#hdnStarValue").val() == 0) && ($('#<%=hdnAnswer.ClientID %>').val() == 0)) {
//                    $("#lblRequired").show();
//                    return false;
//                }
//            }                
            $('#<%=hdnAnswer.ClientID %>').val($("#hdnStarValue").val());
        }
        else if ((anstypeval == 'Yes or No')) {
            $('#<%=hdnAnswer.ClientID %>').val(document.getElementById("myonoffswitch").checked);
        }
        else if ((anstypeval == 'Scale(1-100)')) {
        if ($("#hdnValidateMe").val() == '1') {
            if ($(".slider_step").text() == '') {
                $("#lblRequired").show();
                return false;
            }
        }
        $('#<%=hdnAnswer.ClientID %>').val($(".slider_step").text());
        }
        else if ((anstypeval == 'Yes or No or Can\'t Say')) {
            var list = document.getElementById("ulRadioYesNoCant"); //Client ID of the radiolist
            if (list != null) {
                var inputs = list.getElementsByTagName("input");
                var selected;
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selected = inputs[i];
                        break;
                    }
                }
                if ($("#hdnValidateMe").val() == '1') {
                    if (selected === undefined) {
                        $("#lblRequired").show();
                        return false;
                    }
                }                
                if (selected) {
                    $('#<%=hdnAnswer.ClientID %>').val(selected.id);
                }
            }
        }
        else if ((anstypeval == 'Select one of many')) {
            var list = document.getElementById("ulRadio"); //Client ID of the radiolist
            if (list != null) {
                var inputs = list.getElementsByTagName("input");
                var selected;
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selected = inputs[i];
                        break;
                    }
                }
                if ($("#hdnValidateMe").val() == '1') {
                    if (selected === undefined) {
                        $("#lblRequired").show();
                        return false;
                    }
                }                
                if (selected) {
                    $('#<%=hdnAnswer.ClientID %>').val(selected.id);
                }
            }
        }
        else if ((anstypeval == 'Number') || (anstypeval == 'Text(100)')) {
            if ($("#hdnValidateMe").val() == '1') {
                if ($("#txtText100").val().trim() == '') {
                    $("#lblRequired").show();
                    return false;
                }
            }                
            if (document.getElementById("txtText100") != null)
                $('#<%=hdnAnswer.ClientID %>').val($("#txtText100").val().trim());
        }
        else if ((anstypeval == 'Freeform Text')) {
            if ($("#hdnValidateMe").val() == '1') {
                if ($("#txtFreeText").val().trim() == '') {
                    $("#lblRequired").show();
                    return false;
                }
            }                
            if (document.getElementById("txtFreeText") != null)
                $('#<%=hdnAnswer.ClientID %>').val($("#txtFreeText").val().trim());
        }
        else if (anstypeval == 'Checkbox Multiple') {
            var selected = '';
            var unselected = '';
            var list = document.getElementById("ulCheckBoxes"); //Client ID of the radiolist
            if (list != null) {
                var inputs = list.getElementsByTagName("input");
                for (var i = 0; i < inputs.length; i++) {
                    if (inputs[i].checked) {
                        selected = selected + inputs[i].id + '|';
                    }
                    else {
                        unselected = unselected + inputs[i].id + '|';
                    }
				}
                 
////                if (($("#hdnValidateMe").val() == '1') && ($("#divShowSort").css('display') == 'none') && ($("#divCheckBoxes").css('display') == 'block')) {
////                    if (selected === undefined) {
////                        $("#lblRequired").show();
////                        return false;
////                    }
////                }                
                $('#<%=hdnAnswer.ClientID %>').val(selected);
                $('#<%=hdnAnswerUnChecked.ClientID %>').val(unselected);
            }
		}
		else if ((anstypeval == 'Agree - Disagree')) {
			var list = document.getElementById("ulAgreeDisagree"); //Client ID of the radiolist
			if (list != null) {
				var inputs = list.getElementsByTagName("input");
				var selected;
				for (var i = 0; i < inputs.length; i++) {
					if (inputs[i].checked) {
						selected = inputs[i];
						break;
					}
				}
				
				if (selected) {
					$('#<%=hdnAnswer.ClientID %>').val(selected.id);
                }
			}
		}
		
        return true;
    } 
    
</script>
</head>
<body style="height:100%;width:100%">
    <form id="form1" runat="server" style="height:100%;width:100%">

        <asp:HiddenField runat="server" ID="hdnAnswer" />
        <asp:HiddenField runat="server" ID="hdnAnswerMarks" />

        <asp:HiddenField runat="server" ID="hdnAnswerUnChecked" />
        <asp:HiddenField runat="server" ID="hdnAnswerAdd" />
        <asp:HiddenField runat="server" ID="hdnAnsType" />
        <asp:HiddenField runat="server" ID="hdnStarValue" Value="0" />
        <asp:HiddenField runat="server" ID="hdnValidateMe" Value="0" />
        <asp:HiddenField runat="server" ID="hdnDisagreecomp" />
        <asp:HiddenField runat="server" ID="hdnDisagreepart" />
        <asp:HiddenField runat="server" ID="hdnAgreepart" />
        <asp:HiddenField runat="server" ID="hdnAgreecomp" />

    
    
    <br /><br />    
    <div class="align_center" id="logodiv" runat="server">
       
        <img runat="server"  alt="" id="imgsmalllogo" height="70" width="170"/>
    </div>
    <br /><br />    

    <div class="wrap">

        <header id="header">
        	<h2 id="lblSurveyh2" runat="server" style="width:150%">
            	<label id="lblSurveyName" runat="server"></label>
                <div class="right step"><label id="lblQID" runat="server"></label> <label id="lblQCount" runat="server"></label></div>
            </h2>
        </header>
                <br />
                <asp:Label runat="server" ID="lblErrorMessage" Visible="false" ForeColor="Red"></asp:Label>
        
        <main id="main_content">
        
    	    <h1 runat="server" id="lblQuestion"></h1>   
    	    <div id="lblRequired" style="display:none;margin-bottom:15px;     ">
        	    <label style="color:Red">*Required</label>
    	    </div> 	    
            <div id="divRadio" style="display:none">
                <ul class="list" runat="server" id="ulRadio">
                </ul>
            </div>
            <div id="divMultipleText" style="display:none">
                <%--<ul class="list" runat="server" id="ulMultipleText"></ul>--%>
                <ul class="list" runat="server" id="ulMultipleText1"></ul>
            </div>

            <div id="divRadioYesNoCant" style="display:none">
                <ul class="list" runat="server" id="ulRadioYesNoCant">
                </ul>
                </div>

                <div id="divAgreeDisagree" style="display:none">
                <ul class="list" runat="server" id="ulAgreeDisagree">
                </ul>
           
           
            </div>                
            <div id="divCheckBoxes" style="display:none">
                <ul class="list" runat="server" id="ulCheckBoxes">
                </ul>
            </div>                
            <div id="divCalendar" runat="server" style="display:none">
                <div class="calendar_container"><div class="datepicker"></div></div>
            </div>
            <div id="divFreeText" style="display:none">
                <textarea cols="30" id="txtFreeText" onkeyup="countCharMore(this);" name="txtFreeText"></textarea>
                <div id="charNumMore"></div>
            </div>
            <div id="divFreeText100" style="display:none">
                <input type="text" maxlength="100" id="txtText100" name="txtText100" />
            </div>
            <div id="lstYesNo" style="display:none">
                <div class="onoffswitch">
                    <input type="checkbox" id="myonoffswitch" name="onoffswitch" class="onoffswitch-checkbox" checked >
                    <label class="onoffswitch-label" for="myonoffswitch">
                        <div class="onoffswitch-inner"></div>
                        <div class="onoffswitch-switch"></div>
                    </label>
                </div>
            </div>
            <div id="divSlider" style="display:none">
                <div class="step_slider"></div>                
                <div class="slider_step">0</div>     
            </div>
            <div id="divCamera" style="display:none">
                    <table class="add_new">
                        <tr>
                            <td>
                                <h3>Description</h3>
                                <input type="text" validationgroup="addattach" maxlength="100" id="txtAttachDesc" name="txtAttachDesc" runat="server"/>
<%--                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" validationgroup="addattach" runat="server" ControlToValidate="txtAttachDesc" ErrorMessage="*" Display="Dynamic" InitialValue=""></asp:RequiredFieldValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h3>Click "Choose File" to Attach a File or Take a Photo and then click "Upload".</h3>
                                <asp:FileUpload  ID="txtFileUpload" runat="server" Width="75%"/>
                                <input id="btnAttach" validationgroup="addattach" class="button" type="submit" value="Upload" runat="server" onserverclick="AddAttachment" onclick="return validate('addattach');" />
                            </td>
                        </tr>
                    </table>  
                    <asp:GridView runat="server" ID="grdAttachList" AutoGenerateColumns="false" HeaderStyle-CssClass="header" CssClass="grid" 
                            EmptyDataText="No Attachments so far.">
                    <Columns>
                        <asp:BoundField DataField="txtDesc" HeaderText="Description" ItemStyle-Width="60%" />
                        <asp:TemplateField HeaderText="Attach." ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                          <ItemTemplate>
                                <center>
                                  <asp:ImageButton runat="server" ID="imgbtn" ImageUrl="~/images/attach.png" ToolTip="Open Attachment"
                                        CommandArgument='<%#Eval("SurveyAttachID")%>' OnClick="showattachments"
                                        CausesValidation="false" Visible='<%# not(Eval("AttachmentFile")="")%>' />
                                    </center>
                           </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delete" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20%">
                          <ItemTemplate>
                                <center>
                                <asp:LinkButton CommandArgument='<%#Eval("SurveyAttachID")%>' CssClass="icon delete" ID="btndelete" runat="server" CausesValidation="false" CommandName="Delete"
                                  OnClientClick="var con = confirm('Are you sure you want to delete?');if (con){return true;}else{return false;}"></asp:LinkButton>
                                  </center>
                           </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>                               
            </div>

            <div id="divStars" style="display:none">
                <p>Scale of 1-10 with 10 being the highest</p>
                <span class="star_rating">
                    <span class="star one" id="one_star"></span>
                    <span class="star two" id="two_stars"></span>
                    <span class="star three" id="three_stars"></span>
                    <span class="star four" id="four_stars"></span>
                    <span class="star five" id="five_stars"></span>
                    <span class="star six" id="six_stars"></span>
                    <span class="star seven" id="seven_stars"></span>
                    <span class="star eight" id="eight_stars"></span>
                    <span class="star nine" id="nine_stars"></span>
                    <span class="star ten" id="ten_stars"></span>
                </span>            
            </div>
            <div id="divAddNew" runat="server" visible="false">
                <div>
                <div style="float:left">
                    <input validationgroup="addnew" type="text" maxlength="100" id="txtNewOption" runat="server" style="width:100%" placeholder="Add New Option" />
                </div>
                <div style="float:left;margin-top:10px;margin-left:5px">
                    <input validationgroup="addnew" class="button" type="submit" value="Add" runat="server" onserverclick="addnewoption" onclick="return validateadd();" />
                </div>
                </div>
                <div class="clear"></div>
            </div>
<%--            <asp:UpdatePanel runat="server">
            <ContentTemplate>--%>
                <div id="divShowSort" style="display:none">
                    <asp:Repeater runat="server" ID="rptAnswers">
                        <HeaderTemplate>
                                           <h1>Rank the items using arrows.</h1>
                                <ul class="list">
                        </HeaderTemplate>
                        <ItemTemplate>
                                <li>
                                        <div>
                                            <div style="float:left;margin-bottom:-15px;width:80%">
                                                <label runat="server" style="display:none" ID="lblOrder" ><%#Eval("field_order")%></label>
                                                <label runat="server" style="display:none" ID="lnkTICROWID" ><%#Eval("SurveyResultDetailID")%></label>
                                                <h1 runat="server" id="lblStepNameShort"><%#Eval("Answer")%></h1>
                                            </div>
                                            <div style="float:right;margin-bottom:-15px">
                                                <asp:ImageButton ImageUrl="~/images/up.png" ID="lnkUp" ToolTip='<%#Eval("field_order")%>' CommandName="up" CommandArgument='<%#Eval("SurveyResultDetailID")%>' runat="server" />
                                                <asp:ImageButton ImageUrl="~/images/down.png" ID="lnkDown" ToolTip='<%#Eval("field_order")%>' CommandName="down" CommandArgument='<%#Eval("SurveyResultDetailID")%>' runat="server"/>
                                            </div>
                                            <div class="clear"></div>
                                        </div>
                                </li>
                            </ItemTemplate>
                            <FooterTemplate>
                                </ul>
                            </FooterTemplate>
                    </asp:Repeater> 
                </div>
<%--        </ContentTemplate>
        </asp:UpdatePanel>--%>

            <asp:PlaceHolder runat="server" ID="Qcontainer">
                <div id="charNum"></div>
            </asp:PlaceHolder>
            <div class="clearfix nav_arrows">
                <asp:LinkButton CssClass="arrow left" runat="server" ID="btnPrev" CausesValidation="false" Visible="false" OnClick="gotoprev"></asp:LinkButton>
                <asp:LinkButton validationgroup="validatesurvey" CssClass="arrow right" runat="server" ID="btnNext" OnClientClick="return saveanswer();" OnClick="gotonext"></asp:LinkButton>
            </div>
        
        </main><!--end main_content-->
        <br />
        <footer id="footer">
            <a href="SurveyList.aspx" runat="server" id="btnBack" visible="false" class="red_button wide">Back to List</a>
            <a href="login.aspx" runat="server" id="btnLogoff" visible="false" class="red_button wide">Log Off</a>
        </footer>
    </div><!--end page-->

    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.9.1/jquery.min.js"></script>
    <script>    window.jQuery || document.write('<script src="js/jquery-1.9.1.min.js"><\/script>')</script>
    <script src="js/jquery-ui-1.10.4.custom.min.js"></script>
    <script src="js/jquery.ui.touch-punch.min.js"></script>
    <script src="js/main.js"></script>
    
</form>
</body>
</html>
