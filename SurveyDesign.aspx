<%@ Page Title="Survey Design" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SurveyDesign.aspx.vb" Inherits="SurveyDesign" %>
<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
	<script type="text/javascript" src="js/jscolor.js"></script>
    <script type="text/javascript">
        function ShowLP() {
            alert('1');
            alert(document.getElementById("ctl00_ContentPlaceHolder1_chkLandingPage").checked);
            if (document.getElementById("ctl00_ContentPlaceHolder1_chkLandingPage").checked == true) {
                document.getElementById("ctl00_ContentPlaceHolder1_showLP").style.display = 'block';
            }
            else {
                document.getElementById("ctl00_ContentPlaceHolder1_showLP").style.display = 'none';
            }
        }
    </script>
    <section class="container"><!--sales dashboard!-->
	    <div class="ac_list_main">
            <asp:HiddenField ID="hdnSurveyID" runat="server" />
            <asp:HiddenField ID="hdnDesignFound" runat="server" />

            <asp:HiddenField ID="hdntitlebold" runat="server" />
            <asp:HiddenField ID="hdntitleitalics" runat="server" />
            <asp:HiddenField ID="hdntitleunderline" runat="server" />
            <asp:HiddenField ID="hdnpagebold" runat="server" />
            <asp:HiddenField ID="hdnpageitalics" runat="server" />
            <asp:HiddenField ID="hdnpageunderline" runat="server" />

    	    <div class="top_hd"><!--top heading!-->
        	    <h2>Create Survey<a class="new_btn" href="SurveyList.aspx">Back</a></h2>
    	        <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            </div><!--top heading!-->
                    
            <div class="s-tabs">
        	    <div class="s-tab">
            	    <span>1</span> <p><a id="btntab1" runat="server">Overview</a></p>
                </div>
                <div class="s-tab s-tab1 s-tab1-active s-tab-current">
            	    <span>2</span> <p>Design</p>
                </div>
                <div class="s-tab">
            	    <span>3</span> <p><a id="btntab3" runat="server">Questions</a></p>
                </div>
                <div class="s-tab s-tab-last">
            	    <span>4</span> <p><a id="btntab4" runat="server">Finalize</a></p>
                </div>
                <div class="clr"></div>
            </div>

        <div class="survey-main">
        	<div class="survey-left">
                <div class="s-label">
                    <div class="sl-left"><p>Survey Logo</p></div>
                </div>
                <div class="s-white">
                    <div class="browse-file">
                        <asp:FileUpload runat="server" ID="fupCompanyLogoSmall" CssClass="textfield" TabIndex="1" />
                    </div>
                </div>
                <div class="s-label">
                    <div class="sl-left"><p>Logo Background Color</p></div>
                </div>
                <div class="s-white">
                    <input id="txtCSLBackColor" runat="server" type="color" value="#AAFFAA" class="background_field logo_background_action" />
                </div>
                <div class="s-label">
                    <div class="sl-left"><p>Survey Background Color</p></div>
                </div>
                <div class="s-white">
                    <input id="txtCSSBackColor" runat="server" type="color" value="#FFFFCA" class="background_field survey_background_action" />
                </div>

                <br />
                <h3>Survey Configuration</h3>
                <div class="s-label">
                    <div class="sl-left"><p>Title</p></div>
                </div>
                <div class="s-white">
                    <input id="txtSTitle" runat="server" type="text" class="text-field survey_title_action" placeholder="Survey for ABM and Associates LLP" />
                </div>
                <div class="s-label">
                    <div class="sl-left"><p>Title Background Color</p></div>
                </div>
                <div class="s-white">
                    <input id="txtSTBackColor" runat="server" type="color" value="#FFBD9D" class="background_field sc_background_action" />
                </div>
                <div class="s-label">
                    <div class="sl-left"><p>Title Font</p></div>
                </div>
                <div class="s-white">
                    <asp:DropDownList ID="ddlSTFont" runat="server" Width="30%" TabIndex="4" CssClass="dropdown sc_font_action">
                        <asp:ListItem Text="Arial" Value="Arial"></asp:ListItem>
                        <asp:ListItem Text="Courier New" Value="Courier New"></asp:ListItem>
                        <asp:ListItem Text="Helvetica" Value="Helvetica"></asp:ListItem>
                        <asp:ListItem Text="Tahoma" Value="Tahoma"></asp:ListItem>
                        <asp:ListItem Text="Times New Roman" Value="Times New Roman"></asp:ListItem>
                        <asp:ListItem Text="Verdana" Value="Verdana"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlSTFontSize" runat="server" Width="30%" TabIndex="4" CssClass="dropdown box-half pull-left sc_fontsize_action">
                        <asp:ListItem Text="20" Value="8"></asp:ListItem>
                        <asp:ListItem Text="22" Value="10"></asp:ListItem>
                        <asp:ListItem Text="24" Value="12"></asp:ListItem>
                        <asp:ListItem Text="26" Value="14"></asp:ListItem>
                        <asp:ListItem Text="28" Value="18"></asp:ListItem>
                        <asp:ListItem Text="30" Value="24"></asp:ListItem>
                        <asp:ListItem Text="32" Value="36"></asp:ListItem>
                        <asp:ListItem Text="34" Value="36"></asp:ListItem>
                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                        <asp:ListItem Text="40" Value="36"></asp:ListItem>
                        <asp:ListItem Text="44" Value="36"></asp:ListItem>
                        <asp:ListItem Text="48" Value="36"></asp:ListItem>
                    </asp:DropDownList>
                    <input ID="txtSTFontColor" runat="server" type="color" value="#000000" class="background_field box-half pull-right sc_color_action" />
                    <div class="f-style">
                        <a href="javascript:void()" class="control_bold"><img src="images/icon_bold.png" alt="Bold" /></a>
                        <a href="javascript:void()" class="control_italic"><img src="images/icon_italic.png" alt="Italic" /></a>
                        <a href="javascript:void()" class="control_underline"><img src="images/icon_underline.png" alt="Underline" /></a>
                    </div>
                    <div class="clr"></div>
                </div>

                <br />
                <h3>Page Configuration</h3>
                <div class="s-label">
                    <div class="sl-left"><p>Background Color</p></div>
                </div>
                <div class="s-white">
                    <input id="txtPTBackColor" runat="server" type="color" value="#B9B9FF" class="background_field page_background_action" />
                </div>
                <div class="s-label">
                    <div class="sl-left"><p>Font</p></div>
                </div>
                <div class="s-white">
                    <asp:DropDownList ID="ddlPTFont" runat="server" Width="30%" TabIndex="4" CssClass="dropdown page_font_action">
                        <asp:ListItem Text="Arial" Value="Arial"></asp:ListItem>
                        <asp:ListItem Text="Courier New" Value="Courier New"></asp:ListItem>
                        <asp:ListItem Text="Helvetica" Value="Helvetica"></asp:ListItem>
                        <asp:ListItem Text="Tahoma" Value="Tahoma"></asp:ListItem>
                        <asp:ListItem Text="Times New Roman" Value="Times New Roman"></asp:ListItem>
                        <asp:ListItem Text="Verdana" Value="Verdana"></asp:ListItem>
                    </asp:DropDownList>
                    <asp:DropDownList ID="ddlPTSize" runat="server" Width="30%" TabIndex="4" CssClass="dropdown box-half pull-left page_fontsize_action">
                        <asp:ListItem Text="20" Value="8"></asp:ListItem>
                        <asp:ListItem Text="22" Value="10"></asp:ListItem>
                        <asp:ListItem Text="24" Value="12"></asp:ListItem>
                        <asp:ListItem Text="26" Value="14"></asp:ListItem>
                        <asp:ListItem Text="28" Value="18"></asp:ListItem>
                        <asp:ListItem Text="30" Value="24"></asp:ListItem>
                        <asp:ListItem Text="32" Value="36"></asp:ListItem>
                        <asp:ListItem Text="34" Value="36"></asp:ListItem>
                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                        <asp:ListItem Text="40" Value="36"></asp:ListItem>
                        <asp:ListItem Text="44" Value="36"></asp:ListItem>
                        <asp:ListItem Text="48" Value="36"></asp:ListItem>
                    </asp:DropDownList>
                    <input id="txtPTTextColor" runat="server" type="color" value="#000000" class="background_field box-half pull-right page_color_action" />
                    <div class="f-style">
                        <a href="javascript:void()" class="page_control_bold"><img src="images/icon_bold.png" alt="Bold" /></a>
                        <a href="javascript:void()" class="page_control_italic"><img src="images/icon_italic.png" alt="Italic" /></a>
                        <a href="javascript:void()" class="page_control_underline"><img src="images/icon_underline.png" alt="Underline" /></a>
                    </div>
                    <div class="clr"></div>
                </div>
                <br />
                <h3>Landing Page</h3>
                <div class="s-label">
                    <div class="sl-left"><p>Use Landing Page</p></div>
                    <div class="sl-right">
                        <label class="switch">
                            <input id="chkLandingPage" runat="server" onclick="ShowLP();" checked="" class="inputdemo" type="checkbox" />                            
                            <div class="slider round"></div>
                        </label>
                    </div>
                </div>

                <br />
            </div>
            <div class="survey-right">
                <div class="r-top survey_background_container">
                    <div class="logo_background_container">
                        <img id='output' runat="server" src="" alt="logo" class="survey-logo" />
                    </div>
                </div>
                <div class="r-top survey_configuration_container">
                    <p class="survey_title">Survey for ABM and Associates LLP</p>
                </div>
                <div class="r-top page_configuration_container">
                    <p>Some Content</p>
                    <p>Some Content</p>
                    <p>Some Content</p>
                </div>
                <div class="r-top" id="showLP" runat="server">
                    <CKEditor:CKEditorControl ShiftEnterMode="P"  EnterMode="P" ResizeEnabled="false" 
                    RemovePlugins="elementspath" ToolbarCanCollapse="false"  ID="txtLPText" runat="server" 
                    DisableObjectResizing="true" ToolbarBasic="true" Height="240px" Text=''></CKEditor:CKEditorControl>                   
                </div>
                <div class="progress">
                    <div class="progress-bar" role="progressbar" aria-valuenow="60" aria-valuemin="0" aria-valuemax="100" style="width: 60%;">60%</div>
                </div>
                <div class="create_button"><a  id="btnSave" runat="server" class="btn-black">Done</a></div>
            </div>
            <div class="clr"></div>
        </div>

    </div>
    </section><!--sales dashboard!-->
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_pageLoaded(function () {

            // set at the time of load
            var logo_background_color = $(".logo_background_action").val();
            $(".logo_background_container").css("background", logo_background_color);
            var survey_background_color = $(".survey_background_action").val();
            $(".survey_background_container").css("background", survey_background_color);

            // survey config
            var survey_title = $(".survey_title_action").val();
            $(".survey_title").html(survey_title);

            var sc_color_action = $(".sc_color_action").val();
            $(".survey_title").css("color", sc_color_action);

            var sc_background_action = $(".sc_background_action").val();
            $(".survey_title").css("background", sc_background_action);

            var font_family = $(".sc_font_action").val();
            $(".survey_configuration_container").css("font-family", font_family);

            var font_size = $(".sc_fontsize_action").val();
            $(".survey_configuration_container").css("font-size", font_size + 'px');

            if (document.getElementById("ctl00_ContentPlaceHolder1_hdntitlebold").value == 0) {
                $(".control_bold").toggleClass('active');
                $(".survey_title").toggleClass('bold');
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_hdntitleitalics").value == 0) {
                $(".control_italic").toggleClass('active');
                $(".survey_title").toggleClass('italic');
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_hdntitleunderline").value == 0) {
                $(".control_underline").toggleClass('active');
                $(".survey_title").toggleClass('underline');
            }

            // page config

            var page_background_action = $(".page_background_action").val();
            $(".page_configuration_container").css("background", page_background_action);

            var page_color_action = $(".page_color_action").val();
            $(".page_configuration_container").css("color", page_color_action);

            var page_font_family = $(".page_font_action").val();
            $(".page_configuration_container").css("font-family", page_font_family);

            var page_font_size = $(".page_fontsize_action").val();
            $(".page_configuration_container").css("font-size", page_font_size + 'px');

            if (document.getElementById("ctl00_ContentPlaceHolder1_hdnpagebold").value == 0) {
                $(".page_control_bold").toggleClass('active');
                $(".page_configuration_container").toggleClass('bold');
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_hdnpageitalics").value == 0) {
                $(".page_control_italic").toggleClass('active');
                $(".page_configuration_container").toggleClass('italic');
            }

            if (document.getElementById("ctl00_ContentPlaceHolder1_hdntitleunderline").value == 0) {
                $(".page_control_underline").toggleClass('active');
                $(".page_configuration_container").toggleClass('underline');
            }

            // end set at the time of load


            // Logo Background Color
            $(".logo_background_action").change(function () {
                var logo_background_color = $(this).val();
                $(".logo_background_container").css("background", logo_background_color);
            });

            // Survey Background Color
            $(".survey_background_action").change(function () {
                var survey_background_color = $(this).val();
                $(".survey_background_container").css("background", survey_background_color);
            });

            // Survey Config
            $(".survey_title_action").change(function () {
                var survey_title = $(this).val();
                $(".survey_title").html(survey_title);
            });

            $(".sc_color_action").change(function () {
                var sc_color_action = $(this).val();
                $(".survey_title").css("color", sc_color_action);
            });
            $(".sc_background_action").change(function () {
                var sc_background_action = $(this).val();
                $(".survey_title").css("background", sc_background_action);
            });
            $(".sc_font_action").change(function () {
                var font_family = $(this).val();
                $(".survey_configuration_container").css("font-family", font_family);
            });
            $(".sc_fontsize_action").change(function () {
                var font_size = $(this).val();
                $(".survey_configuration_container").css("font-size", font_size + 'px');
            });

            $(".control_bold").click(function () {
                $(this).toggleClass('active');
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdntitlebold").value == 0) {
                    alert('1');
                    document.getElementById("ctl00_ContentPlaceHolder1_hdntitlebold").value = 1;
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdntitlebold").value = 0;
                }
                $(".survey_title").toggleClass('bold')
            });
            $(".control_italic").click(function () {
                $(this).toggleClass('active');
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdntitleitalics").value == 0) {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdntitleitalics").value = 1;
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdntitleitalics").value = 0;
                }
                $(".survey_title").toggleClass('italic')
            });
            $(".control_underline").click(function () {
                $(this).toggleClass('active');
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdntitleunderline").value == 0) {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdntitleunderline").value = 1;
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdntitleunderline").value = 0;
                }
                $(".survey_title").toggleClass('underline')
            });

            // Page Configuration
            $(".page_background_action").change(function () {
                var page_background_action = $(this).val();
                $(".page_configuration_container").css("background", page_background_action);
            });
            $(".page_color_action").change(function () {
                var page_color_action = $(this).val();
                $(".page_configuration_container").css("color", page_color_action);
            });
            $(".page_configuration_action").change(function () {
                var page_content_action = $(this).val();
                var pagetext = page_content_action.split('\n');
                $(".page_configuration_container").html(pagetext.join('<br/>'));
            });
            $(".page_font_action").change(function () {
                var page_font_family = $(this).val();
                $(".page_configuration_container").css("font-family", page_font_family);
            });
            $(".page_fontsize_action").change(function () {
                var page_font_size = $(this).val();
                $(".page_configuration_container").css("font-size", page_font_size + 'px');
            });
            $(".page_control_bold").click(function () {
                $(this).toggleClass('active');
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdnpagebold").value == 0) {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdnpagebold").value = 1;
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdnpagebold").value = 0;
                }
                $(".page_configuration_container").toggleClass('bold')
            });
            $(".page_control_italic").click(function () {
                $(this).toggleClass('active');
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdnpageitalics").value == 0) {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdnpageitalics").value = 1;
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdnpageitalics").value = 0;
                }
                $(".page_configuration_container").toggleClass('italic')
            });
            $(".page_control_underline").click(function () {
                $(this).toggleClass('active');
                if (document.getElementById("ctl00_ContentPlaceHolder1_hdnpageunderline").value == 0) {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdnpageunderline").value = 1;
                }
                else {
                    document.getElementById("ctl00_ContentPlaceHolder1_hdnpageunderline").value = 0;
                }
                $(".page_configuration_container").toggleClass('underline')
            });

        });
    </script>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

