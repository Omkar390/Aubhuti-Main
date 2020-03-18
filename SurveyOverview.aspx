<%@ Page Title="Survey Overview" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SurveyOverview.aspx.vb" Inherits="SurveyOverview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="container"><!--sales dashboard!-->
        <asp:HiddenField ID="hdnSurveyID" runat="server" />
	    <div class="ac_list_main">
    	    <div class="top_hd crasul_sec"><!--top heading!-->
        	    <h2>Create Test <a class="new_btn" href="SurveyList.aspx">Back</a></h2>
    	        <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                <asp:Label ID="lblSuccessMsg" runat="server" ForeColor="Green" Visible="false"></asp:Label>
            </div><!--top heading!-->
                    
            <div class="s-tabs">
        	    <div class="s-tab s-tab1 s-tab1-active s-tab-current">
            	    <span>1</span> <p>Overview</p>
                </div>
                <div class="s-tab">
            	    <span>2</span> <p><a id="btntab2" runat="server">Design</a></p>
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
            	    <div class="s-label bottom-five">
                	    <div class="sl-left"><p>Test Active?</p></div>
                        <div class="sl-right">
                    	    <label class="switch">
                                <input id="chkActive" runat="server" checked="checked" class="inputdemo" type="checkbox" />
                                 <div class="slider round"></div>
                            </label>
                        </div>
                    </div>
            	    <div class="s-label bottom-five">
                	    <div class="sl-left"><p>Test Name</p></div>
                    </div>
                    <div id="abm" class="s-white">
                        <input id="txtName" runat="server" type="text" class="text-field"/>
                    </div>
                    <div class="s-label">
                	    <div class="sl-left"><p>Test Type</p></div>
                    </div>
                    <div class="s-white">
                        <asp:DropDownList ID="ddlSurveyType" CssClass="dropdown" runat="server" DataTextField="surveytype" DataValueField="surveytypeid"></asp:DropDownList>
                    </div>
                    <div class="s-label">
                	    <div class="sl-left"><p>Test Description</p></div>
                    </div>
                    <div class="s-white">
                	    <textarea id="txtDesc" runat="server" class="text-field" ></textarea>
                    </div>
                    <%-- <div class="s-label">
                	    <div class="sl-left"><p>Survey URL</p></div>
                    </div>
                    <div class="s-white">
                	    <input type="text" class="text-field" value="http://survey.tic-portal.com" disabled="disabled" />
                    </div>--%>
                </div>

                <div class="survey-right">
                    <div id="divrows" runat="server">
            	        <h2>Yes Test is active</h2>

                        <h3>Tile</h3>
                        <p><asp:Literal ID="litTitle" runat="server"></asp:Literal></p>

                        <h3>Type</h3>
                        <p><asp:Literal ID="litType" runat="server"></asp:Literal></p>

                        <h3>Description</h3>
                        <p><asp:Literal ID="litDescription" runat="server"></asp:Literal></p>

                        <h3>URL</h3>
                        <p><a id="lnkSurvey" runat="server" class="anchor" href=""><u>Click to view Test</u></a></p>

                        <div class="progress">
                          <div class="progress-bar" role="progressbar" aria-valuenow="25" aria-valuemin="0" aria-valuemax="100" style="width: 25%;">
                            25%
                          </div>
                        </div>
                    <%--</div>--%>
                </div>
                <div class="create_button"><a id="btnSave" runat="server" class="btn-black">Save</a></div>
                <div class="clr"></div>
            </div>

        </div>
        </div>
    </section><!--sales dashboard!-->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

