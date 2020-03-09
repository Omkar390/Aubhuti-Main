<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="SurveyFinalize.aspx.vb" Inherits="SurveyFinalize" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <section class="container"><!--sales dashboard!-->
        <asp:HiddenField ID="hdnSurveyID" runat="server" />
	    <div class="ac_list_main">
    	    <div class="top_hd"><!--top heading!-->
        	    <h2>Create Survey</h2>
            </div><!--top heading!-->

        <div class="s-tabs">
        	<div class="s-tab s-tab1 s-tab1-active s-tab-active">
            	<span>1</span> <p><a id="btntab1" runat="server">Overview</a></p>
            </div>
            <div class="s-tab s-tab-active">
            	<span>2</span> <p><a id="btntab2" runat="server">Design</a></p>
            </div>
            <div class="s-tab s-tab-active">
            	<span>3</span> <p><a id="btntab3" runat="server">Questions</a></p>
            </div>
            <div class="s-tab s-tab-current s-tab-last">
            	<span>4</span> <p>Finalize</p>
            </div>
            <div class="clr"></div>
        </div>
        <div class="survey-main">
            <div class="survey-right">
                <div class="progress">
                  <div class="progress-bar" role="progressbar" aria-valuenow="100" aria-valuemin="0" aria-valuemax="100" style="width: 100%;">
                    100%
                  </div>
                </div>
                <div class="create_button"><a href="" class="btn-black" id="btnPreview" runat="server">Preview</a></div>
            </div>
            <div class="clr"></div>
        </div>

        </div>
    </section><!--sales dashboard!-->
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ContentPopMain" Runat="Server">
</asp:Content>

