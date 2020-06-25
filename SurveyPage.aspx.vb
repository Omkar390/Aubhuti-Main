Imports System.Data
Imports System.Data.Common
Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient
Imports System.Drawing

Partial Class survey_SurveyPage
    Inherits System.Web.UI.Page

    Private boolFirstTime As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'CGUID=716eff48-cb2f-11e6-a817-065cea4aa6c1&GUID=e180cc81-ff23-11e6-a817-065cea4aa6c1&SGUID=a58e1c4b-0369-11e7-a817-065cea4aa6c1

        Try
            If Not Page.IsPostBack Then
                'Dim strSQL As String2w
                'change in declaration of object
                Dim strSQL As String
                Dim dt As New DataTable
                Session("ANSWERMARKS") = "0"
                'for testing we used the surveyid and uerid
                'Session("SurveyId") = "6"
                'Session("user_id") = "12"

                'get client guid and client DB from that!
                Session("CGUID") = Request.QueryString("CGUID")
                'Session("CGUID") = "716eff48-cb2f-11e6-a817-065cea4aa6c1"
                Session("CGUID") = "716eff48-cb2f-11e6-a817-065cea4aa6c1"

                Dim db As New DBHelper
                'this shows the client name, client id and dbname
                strSQL = "Select CM.client_name, CM.client_id, SUBSTRING(database_name,8,4) AS dbname " & _
                        " from client_master CM " & _
                        " where CM.Active = 1 and GUID = '" & Session("CGUID") & "'"
                dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
                If dt.Rows.Count = 1 Then
                    Session("client_id") = dt.Rows(0).Item("client_id").ToString
                    Session("dbname") = dt.Rows(0).Item("dbname").ToString
                End If


                'Session("SurveyId") = 1
                'Session("PMode") = "P"
                If Request.QueryString("PMode") Is Nothing Then
                    Session("PMode") = "S"
                    'Session("SurveyId") = 6

                    'Get Survey Id Based on survey GUID!
                    Session("surveyguid") = Request.QueryString("SGUID")

                    'Session("GUID") = "1B187EC2-8D98-45BF-99B3-6FE9845A9692"
                    Session("GUID") = "1B187EC2-8D98-45BF-99B3-6FE9845A9692"
                    Session("meetingguid") = Request.QueryString("GUID")

                    'Dim dbc As New DBHelperClient
                    ''this shows data from client_2002 database from tblsurvey it shows surveyid using the id
                    'strSQL = " Select SurveyId from tblSurvey where id = '" & Session("surveyguid") & "' limit 1"
                    'Session("SurveyId") = dbc.ExecuteScalar(CommandType.Text, strSQL)
                    Session("SurveyId") = Session("surveyguid")
                Else                
                    If Request.QueryString("PMode") = "P" Then
                        Session("PMode") = "P"
                        Session("SurveyId") = Request.QueryString("SGUID")
                        'Session("SurveyId") = "6"
                    End If
                End If

                If Not Session("user_id") Is Nothing Then
                    btnBack.Visible = True
                    btnLogoff.Visible = True
                End If

                Session("AllowRanking") = "N"
                Session("SAVEANSWER") = "Y"
                Session("AlreadyOnRanking") = "N"
                Session("SORTEVENT") = "N"
                Session("RANKDATA") = Nothing
                initsurvey(sender, e)
            End If

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.StackTrace + " - " + ex.Message
        End Try
    End Sub
    Protected Sub initsurvey(ByVal sender As Object, ByVal e As EventArgs)

        Try
            Dim db1 As New DBHelperClient
            Dim dsImg As New DataSet()
            Dim strSql As String

            'for testing purpose we used the static credentials 
            ' Session("SurveyId") = "6"
            ' Session("surveyguid") = Request.QueryString("SGUID")
            ' Session("meetingguid") = Request.QueryString("GUID")
            'Session("GUID") = "1B187EC2-8D98-45BF-99B3-6FE9845A9692"
            ' Session("meetingguid") = "1B187EC2-8D98-45BF-99B3-6FE9845A9692"
            ' Session("PMode") = "P"

            'this query is for showing the image/logo and sizes 
            strSql = "select CONCAT(SurveyID, '_', logo)  as logo, CSLBackColor,  STTextColor,  PTTextColor,  " & _
                    " PTBackColor, CSFont,  STFont, STFontSize, PTFont, PTFontSize from tblsurveydesign where SurveyId = " & Session("SurveyId")
            dsImg = db1.DataAdapter(CommandType.Text, strSql)

            imgsmalllogo.Src = ConfigurationManager.AppSettings("logopath") & Session("client_id") & "/" & dsImg.Tables(0).Rows(0).Item("logo").ToString
            form1.Style.Item("background-color") = dsImg.Tables(0).Rows(0).Item("PTBackColor").ToString
            logodiv.Style.Item("background-color") = dsImg.Tables(0).Rows(0).Item("CSLBackColor").ToString

            If dsImg.Tables(0).Rows(0).Item("STFont").ToString = "" Then
            Else
                lblSurveyh2.Style.Item("line-height") = ((dsImg.Tables(0).Rows(0).Item("STFontSize").ToString) * 1.5) & "px"
                lblSurveyh2.Style.Item("height") = ((dsImg.Tables(0).Rows(0).Item("STFontSize").ToString) * 1.5) & "px"
                lblSurveyName.Style.Item("font-size") = dsImg.Tables(0).Rows(0).Item("STFontSize").ToString & "px"
            End If
            lblQCount.Style.Item("font-size") = dsImg.Tables(0).Rows(0).Item("STFontSize").ToString & "px"

            lblSurveyName.Style.Item("Color") = "#" & dsImg.Tables(0).Rows(0).Item("STTextColor").ToString
            lblSurveyName.Style.Item("font-family") = dsImg.Tables(0).Rows(0).Item("CSFont").ToString
            lblQID.Style.Item("font-family") = dsImg.Tables(0).Rows(0).Item("CSFont").ToString
            lblQID.Style.Item("font-size") = dsImg.Tables(0).Rows(0).Item("STFontSize").ToString & "px"
            lblQCount.Style.Item("font-family") = dsImg.Tables(0).Rows(0).Item("CSFont").ToString
            lblQuestion.Style.Item("Color") = "#" & dsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
            lblQuestion.Style.Item("font-family") = "#" & dsImg.Tables(0).Rows(0).Item("CSFont").ToString
            lblQuestion.Style.Item("font-size") = dsImg.Tables(0).Rows(0).Item("PTFont").ToString & "px"


            'Session("surveyid") = Request.QueryString("SurveyID")
            Session("sqorder") = "0"
            Session("firstquestionorder") = "0"
            Session("lastquestionorder") = "0"

            'get the survey id and also check for last completed question and jump there...

            Dim parms(1) As DBHelperClient.Parameters
            Dim parms1(0) As DBHelperClient.Parameters
            Dim db As New DBHelperClient
            Dim ds1 As New DataSet
            'parms(0) = New DBHelperClient.Parameters("i_surveyguid", Session("surveyguid"))
            'ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyIDByGUID", parms)

            If Session("PMode") = "P" Then
                parms1(0) = New DBHelperClient.Parameters("p_surveyid", Session("SurveyId"))
                ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyIDPreview", parms1)
            Else
                parms(0) = New DBHelperClient.Parameters("p_surveyid", Session("SurveyId"))
                parms(1) = New DBHelperClient.Parameters("p_meetingguid", Session("meetingguid"))
                If Session("SurveyId").ToString.Equals("1") Then
                    ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyID", parms)
                Else
                    ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyIDAP", parms)
                End If
            End If


            Session("surveyid") = ds1.Tables(0).Rows(0)(0)
            Session("surveyresultheaderid") = ds1.Tables(0).Rows(0)(1)
            lblSurveyName.InnerHtml() = ds1.Tables(0).Rows(0)(2)

            If ds1.Tables.Count > 1 Then
                If ds1.Tables(1).Rows.Count > 0 Then
                    Session("sqorder") = ds1.Tables(1).Rows(0)(0)
                End If
            End If

            If ds1.Tables.Count > 2 Then
                If ds1.Tables(2).Rows.Count > 0 Then
                    Session("firstquestionorder") = ds1.Tables(2).Rows(0)(0)
                End If
            End If

            If ds1.Tables.Count > 3 Then
                If ds1.Tables(3).Rows.Count > 0 Then
                    Session("lastquestionorder") = ds1.Tables(3).Rows(0)(0)
                End If
            End If

            If ds1.Tables.Count > 4 Then
                If ds1.Tables(4).Rows.Count > 0 Then
                    lblQCount.InnerHtml = ds1.Tables(4).Rows(0)(0)
                End If
            End If

            If Session("sqorder") = Session("lastquestionorder") Then
                If Session("sqorder") <> 1 And Session("sqorder") <> ds1.Tables(4).Rows(0)(0) Then
                    Session("sqorder") = Session("sqorder") + 1
                End If
            End If
            If Session("sqorder") = Session("firstquestionorder") Then
                Session("sqorder") = Session("sqorder") - 1
            End If
            lblQID.InnerHtml = Session("sqorder") & " of "

            boolFirstTime = True

            gotonext(sender, e)

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.StackTrace
        End Try
    End Sub

    Protected Sub gotonext(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblErrorMessage.Text = ""
            lblErrorMessage.Visible = False

            If Session("sqorder") > 0 Then
                btnPrev.Visible = True
                If Not boolFirstTime And Session("SAVEANSWER") = "Y" Then
                    boolFirstTime = False
                    If Session("PMode") = "P" Then
                    Else
                        SaveAnswer()
                        If lblErrorMessage.Text = "" Then
                        Else
                            Session("sqorder") = Session("sqorder") - 1
                        End If
                    End If
                End If
                'add logic for next question!!!
                If Session("HasLogic") = "1" Then
                    If hdnAnswer.Value = Session("LogicAnswerValue") Then
                        'If Not DirectCast(Me.FindControl("rbList"), RadioButtonList).SelectedValue = Session("LogicAnswerValue") Then
                        'Session("LogicQIDGoto") = "0"
                    Else
                        Session("LogicQIDGoto") = "0"
                    End If
                Else
                    Session("LogicQIDGoto") = "0"
                End If
            Else
                Session("LogicQIDGoto") = "0"
            End If
            If Session("AlreadyOnRanking") = "Y" And Session("SORTEVENT") = "N" Then
                Session("sqorder") = Session("sqorder") - 1
                Session("AlreadyOnRanking") = "N"
                Session("AllowRanking") = "N"
            End If
            Dim parms(3) As DBHelperClient.Parameters
            Dim db As New DBHelperClient
            Dim ds1 As New DataSet
            parms(0) = New DBHelperClient.Parameters("p_surveyid", Session("surveyid"))
            parms(1) = New DBHelperClient.Parameters("p_order", Session("sqorder"))
            parms(2) = New DBHelperClient.Parameters("p_logicqidgoto", Session("LogicQIDGoto"))
            parms(3) = New DBHelperClient.Parameters("p_surveyresultheaderid", Session("surveyresultheaderid"))
            ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyQuestions", parms)
            If ds1.Tables.Count > 0 Then
                If ds1.Tables(0).Rows.Count > 0 Then
                    If Session("AllowRanking") = "Y" Then
                        Dim dt1, dt2 As New DataTable
                        dt1 = Session("RANKDATA")
                        dt2 = dt1.Copy()
                        If Not dt2 Is Nothing Then
                            If dt2.Rows.Count > 0 Then
                                If dt2.Rows(dt2.Rows.Count - 1)("AnswerType") = "AddAns" Then
                                    dt2.Rows(dt2.Rows.Count - 1).Delete()
                                End If
                                'bind the repeater with the selected items
                                rptAnswers.DataSource = dt2
                                rptAnswers.DataBind()
                                Dim lnkUp As ImageButton = TryCast(rptAnswers.Items(0).FindControl("lnkUp"), ImageButton)
                                Dim lnkDown As ImageButton = TryCast(rptAnswers.Items(rptAnswers.Items.Count - 1).FindControl("lnkDown"), ImageButton)
                                lnkUp.Enabled = False
                                lnkDown.Enabled = False

                                SetAnswer(Nothing, 10)
                                Session("AllowRanking") = ds1.Tables(0).Rows(0)("AllowRanking")
                                'show addnewoption if applicable!
                                divAddNew.Visible = False
                                If Qcontainer.Controls.Count > 0 Then
                                    Qcontainer.Controls.Clear()
                                End If
                                'Session("sqorder") = ds1.Tables(0).Rows(0)("field_order")
                                Session("SAVEANSWER") = "N"
                                Session("AlreadyOnRanking") = "Y"
                                If Session("SORTEVENT") = "Y" Then
                                    Session("SORTEVENT") = "N"
                                Else
                                    Session("sqorder") = Session("sqorder") + 1
                                End If
                                Exit Sub
                            End If
                        End If
                    End If
                    Session("SAVEANSWER") = "Y"
                    Session("HasLogic") = ds1.Tables(0).Rows(0)("HasLogic")
                    Session("LogicAnswerValue") = ds1.Tables(0).Rows(0)("LogicAnswerValue")
                    Session("LogicQIDGoto") = ds1.Tables(0).Rows(0)("LogicQIDGoto")
                    Session("anstype") = ds1.Tables(0).Rows(0)("AnsType")
                    hdnAnsType.Value = ds1.Tables(0).Rows(0)("AnsType")
                    Session("SQID") = ds1.Tables(0).Rows(0)("sqrowid")
                    Session("QTEXT") = ds1.Tables(0).Rows(0)("QText")
                    lblQuestion.InnerHtml = ds1.Tables(0).Rows(0)("QText")
                    'Session("AllowRanking") = ds1.Tables(0).Rows(0)("AllowRanking")
                    Session("sqorder") = ds1.Tables(0).Rows(0)("qorder")
                    If Session("AllowRanking") = "Y" Then
                        If ds1.Tables.Count > 2 Then
                            If ds1.Tables(2).Rows.Count > 0 Then
                                Session("RANKDATA") = ds1.Tables(2)
                            End If
                        End If
                    Else
                        Session("RANKDATA") = Nothing
                    End If


                    'show addnewoption if applicable!
                    If ds1.Tables(0).Rows(0)("AddNewOption") = "1" Then
                        divAddNew.Visible = True
                    Else
                        divAddNew.Visible = False
                    End If
                    If Qcontainer.Controls.Count > 0 Then
                        Qcontainer.Controls.Clear()
                    End If
                    If ds1.Tables.Count > 1 Then
                        addcontrolsforprev(ds1)
                    End If
                    lblQID.InnerHtml = Session("sqorder") & " of "
                Else
                    btnNext.Visible = False
                    lblQuestion.InnerHtml = "Thanks for taking the Survey!"  '1
                    btnPrev.Visible = False
                    btnNext.Visible = False
                    lblQID.InnerHtml = Session("sqorder") & " of "
                    Session("sqorder") = "0"
                    'If (Not Request.UrlReferrer Is Nothing) Then
                    'If Request.UrlReferrer.ToString.ToLower.Contains("/surveypage.aspx?") Then
                    ' sendemailtokarl()
                    'End If
                    'End If
                End If
            Else
                btnNext.Visible = False
                lblQuestion.InnerHtml = "Thanks for taking the Survey!"  '2
                btnPrev.Visible = False
                btnNext.Visible = False
                lblQID.InnerHtml = Session("sqorder") & " of "
                Session("sqorder") = "0"
                'If (Not Request.UrlReferrer Is Nothing) Then
                'If Request.UrlReferrer.ToString.ToLower.Contains("/surveypage.aspx?") Then
                'sendemailtokarl()
                'End If
                'End If
            End If

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try

    End Sub

    Private Sub SaveAnswer()
        Try
            'insert into tblSurveyResultDetail
            'handle single answer or multiple
            Select Case Session("anstype").ToString
                Case "Yes or No", "Yes or No or Can't Say", "Select one of many", "Text(100)", "Freeform Text", "Date", "Number", "Scale(1-100)", "STARS"
                    'insert the answer
                    'Session("ANSWER") = DirectCast(Me.FindControl("rbList"), RadioButtonList).SelectedValue
                    If hdnAnswer.Value = "true" Then
                        hdnAnswer.Value = "Yes"
                    ElseIf hdnAnswer.Value = "false" Then
                        hdnAnswer.Value = "No"
                    End If
                    Session("ANSWER") = hdnAnswer.Value
                    InsertAnswer(Session("anstype"), Session("QTEXT"), 0)
                Case "Checkbox Multiple"
                    'insert multiple answers!..possibly
                    If hdnValidateMe.Value = "1" Then
                        If hdnAnswer.Value.Split("|").Length = 1 And hdnAnswerAdd.Value.Trim = "" Then
                            lblErrorMessage.Text = "*Required"
                            lblErrorMessage.Visible = True
                            Exit Sub
                        End If
                    End If
                    For index As Integer = 0 To hdnAnswer.Value.Split("|").Length - 2
                        Session("ANSWER") = hdnAnswer.Value.Split("|")(index)
                        InsertAnswer(Session("anstype"), Session("QTEXT"), 1)
                    Next
                    For index As Integer = 0 To hdnAnswerUnChecked.Value.Split("|").Length - 2
                        Session("ANSWER") = hdnAnswerUnChecked.Value.Split("|")(index)
                        InsertAnswer(Session("anstype"), Session("QTEXT"), 2)
                    Next
                    Session("RANKDATA") = Nothing
                    'Case "CAMERA"
                    '    'InsertAnswer(Session("anstype"), Session("QTEXT"), 0)
                    '    'RefreshCamera()



                    'Select Case Session("marks").ToString
                Case "Agree - Disagree"
                    'insert the answer
                    'Session("ANSWER") = DirectCast(Me.FindControl("rbList"), RadioButtonList).SelectedValue
                    If hdnAnswer.Value = "CompletelyDisagree" Then
                        hdnAnswerMarks.Value = hdnDisagreecomp.Value
                    ElseIf hdnAnswer.Value = "PartiallyDisagree" Then
                        hdnAnswerMarks.Value = hdnDisagreepart.Value
                    ElseIf hdnAnswer.Value = "PartiallyAgree" Then
                        hdnAnswerMarks.Value = hdnAgreepart.Value
                    ElseIf hdnAnswer.Value = "CompletelyAgree" Then
                        hdnAnswerMarks.Value = hdnAgreecomp.Value
                    End If
                    'Session("ANSWER") = hdnAnswerMarks.Value
                    Session("ANSWERMARKS") = hdnAnswerMarks.Value
                    InsertAnswer(Session("anstype"), Session("QTEXT"), 0)

                Case Else

            End Select

            If hdnAnswerAdd.Value.Trim <> "" Then
                Session("ANSWER") = hdnAnswerAdd.Value.Trim
                InsertAnswer("AddAns", "Additional Answer", 0)
            End If

            'get the anser in Session("RANKDATA") for displaying on next screen!
            If Session("RANKDATA") Is Nothing And Session("AllowRanking") = "Y" Then
                Dim parms(1) As DBHelperClient.Parameters
                Dim db As New DBHelperClient
                Dim ds1 As New DataSet
                parms(0) = New DBHelperClient.Parameters("i_SQID", Session("SQID"))
                parms(1) = New DBHelperClient.Parameters("i_surveyguid", Session("surveyguid"))
                ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetDragDropAnswers", parms)
                Session("RANKDATA") = ds1.Tables(0)
            End If

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdAttachList_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles grdAttachList.RowCommand
        Try
            If e.CommandName = "Delete" Then
                Dim db As New DBHelperClient
                Dim parms(0) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("i_SurveyAttachID", e.CommandArgument)
                db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_DELETESURVEYATTACH", parms)
                RefreshCamera()
            End If
        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try
    End Sub

    Protected Sub RefreshCamera()
        Try
            'Show all attachment surveys for header id!
            Dim parms(0) As DBHelperClient.Parameters
            Dim db As New DBHelperClient
            Dim ds1 As New DataSet
            parms(0) = New DBHelperClient.Parameters("i_SurveyResultHeaderID", Session("SurveyResultHeaderID"))
            ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyAttachList", parms)
            If ds1.Tables.Count > 0 Then
                If ds1.Tables(0).Rows.Count > 0 Then
                    grdAttachList.DataSource = ds1
                    grdAttachList.DataBind()
                Else
                    grdAttachList.DataSource = Nothing
                    grdAttachList.DataBind()
                End If
            Else
                grdAttachList.DataSource = Nothing
                grdAttachList.DataBind()
            End If
            SetAnswer(Nothing, 11)

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try
    End Sub

    Protected Sub grdAttachList_OnRowDeleting(ByVal sender As Object, ByVal e As GridViewDeleteEventArgs) Handles grdAttachList.RowDeleting

    End Sub

    Private Sub InsertAnswer(ByVal anstype As String, ByVal qtext As String, ByVal deleteflag As Integer)
        Dim db As New DBHelperClient
        Dim ds1 As New DataSet
        Dim parms(7) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_surveyguid", Session("surveyguid"))
        parms(1) = New DBHelperClient.Parameters("p_SQID", Session("SQID"))
        parms(2) = New DBHelperClient.Parameters("p_QTEXT", qtext)
        parms(3) = New DBHelperClient.Parameters("p_anstype", anstype)
        parms(4) = New DBHelperClient.Parameters("p_ANSWER", Session("ANSWER"))
        parms(5) = New DBHelperClient.Parameters("p_deleteflag", deleteflag)
        parms(6) = New DBHelperClient.Parameters("p_meetingguid", Session("meetingguid"))
        parms(7) = New DBHelperClient.Parameters("p_answermarks", Session("ANSWERMARKS"))

        'parms(7) = New DBHelperClient.Parameters("p_marks", Session("marks"))
        If Session("SurveyId") = "1" Then
            db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertAnswer", parms)
        Else
            db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_InsertAnswerAP", parms)
        End If

    End Sub


    Protected Sub gotoprev(ByVal sender As Object, ByVal e As EventArgs)
        Try
            lblErrorMessage.Text = ""
            lblErrorMessage.Visible = False

            If Not btnNext.Visible Then
                btnNext.Visible = True
            End If
            If Session("sqorder") > 0 Then
                btnPrev.Visible = True
            End If
            Dim parms(3) As DBHelperClient.Parameters
            Dim db As New DBHelperClient
            Dim ds1 As New DataSet

            If Session("PMode") = "P" Then
                parms(0) = New DBHelperClient.Parameters("p_surveyid", Session("surveyid"))
                parms(1) = New DBHelperClient.Parameters("p_order", Session("sqorder"))
                ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyPrevQuestionsPreview", parms)
            Else
                parms(0) = New DBHelperClient.Parameters("p_surveyid", Session("surveyid"))
                parms(1) = New DBHelperClient.Parameters("p_order", Session("sqorder"))
                parms(2) = New DBHelperClient.Parameters("p_surveyguid", Session("surveyguid"))
                parms(3) = New DBHelperClient.Parameters("p_meetingguid", Session("meetingguid"))
                ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyPrevQuestions", parms)
            End If


            If ds1.Tables.Count > 0 Then
                If ds1.Tables(0).Rows.Count > 0 Then
                    'Session("AllowRanking") = ds1.Tables(0).Rows(0)("AllowRanking")
                    Session("SQID") = ds1.Tables(0).Rows(0)("sqrowid")
                    If Session("AllowRanking") = "Y" And Session("AlreadyOnRanking") = "N" Then
                        Session("AlreadyOnRanking") = "Y"
                        'Session("sqorder") = Session("sqorder") + 1
                        SetSessionRankData()
                        Dim dt1, dt2 As New DataTable
                        dt1 = Session("RANKDATA")
                        dt2 = dt1.Copy()
                        If Not dt2 Is Nothing Then
                            If dt2.Rows.Count > 0 Then
                                If dt2.Rows(dt2.Rows.Count - 1)("AnswerType") = "AddAns" Then
                                    dt2.Rows(dt2.Rows.Count - 1).Delete()
                                End If
                                'bind the repeater with the selected items
                                rptAnswers.DataSource = dt2
                                rptAnswers.DataBind()
                                Dim lnkUp As ImageButton = TryCast(rptAnswers.Items(0).FindControl("lnkUp"), ImageButton)
                                Dim lnkDown As ImageButton = TryCast(rptAnswers.Items(rptAnswers.Items.Count - 1).FindControl("lnkDown"), ImageButton)
                                lnkUp.Enabled = False
                                lnkDown.Enabled = False

                                SetAnswer(Nothing, 10)
                                'show addnewoption if applicable!
                                divAddNew.Visible = False
                                If Qcontainer.Controls.Count > 0 Then
                                    Qcontainer.Controls.Clear()
                                End If
                                'Session("sqorder") = ds1.Tables(0).Rows(0)("field_order")
                                Session("SAVEANSWER") = "N"
                                Exit Sub
                            End If
                        End If
                    End If
                    Session("SAVEANSWER") = "Y"
                    Session("AlreadyOnRanking") = "N"
                    Session("HasLogic") = ds1.Tables(0).Rows(0)("HasLogic")
                    Session("LogicAnswerValue") = ds1.Tables(0).Rows(0)("LogicAnswerValue")
                    Session("LogicQIDGoto") = ds1.Tables(0).Rows(0)("LogicQIDGoto")
                    Session("anstype") = ds1.Tables(0).Rows(0)("AnsType")
                    hdnAnsType.Value = ds1.Tables(0).Rows(0)("AnsType")
                    Session("QTEXT") = ds1.Tables(0).Rows(0)("QText")
                    Session("sqorder") = ds1.Tables(0).Rows(0)("qorder")
                    lblQuestion.InnerHtml = ds1.Tables(0).Rows(0)("QText")
                    'show addnewoption if applicable!
                    If ds1.Tables(0).Rows(0)("AddNewOption") = "1" Then
                        divAddNew.Visible = True
                    Else
                        divAddNew.Visible = False
                    End If
                    If Qcontainer.Controls.Count > 0 Then
                        Qcontainer.Controls.Clear()
                    End If
                    addcontrolsforprev(ds1)
                End If
            End If
            lblQID.InnerHtml = Session("sqorder") & " of "

            If Session("firstquestionorder") = Session("sqorder") Then
                btnPrev.Visible = False
            End If

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try

    End Sub

    Private Sub addcontrolsforprev(ByVal ds1 As DataSet)

        Try
            Dim db1 As New DBHelperClient
            Dim DsImg As New DataSet
            Dim strsql As String
            strsql = "select STTextColor,  PTTextColor,  STBackColor, CSFont,  STFont, PTFont, PTFontSize from tblSurveyDesign where SurveyId = " & Session("SurveyId")
            DsImg = db1.DataAdapter(CommandType.Text, strsql)

            Dim strID As String = ""
            Select Case ds1.Tables(0).Rows(0)("AnsType")
                Case "Yes or No"
                    SetAnswer(ds1, 1)
                Case "STARS"
                    SetAnswer(ds1, 7)
                Case "Yes or No or Can't Say"
                    If ds1.Tables(1).Rows.Count > 0 Then
                        Dim li As HtmlGenericControl
                        Dim lst As RadioButton

                        li = New HtmlGenericControl("li")
                        li.Attributes.Add("class", "radio")
                        lst = New RadioButton
                        lst.Text = "Yes"
                        lst.ID = "Yes"
                        lst.GroupName = "radio"
                        lst.Style.Item("Color") = "#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                        lst.Style.Item("font-family") = "#" & DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                        lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString - 2) & "px"
                        li.Controls.Add(lst)
                        ulRadioYesNoCant.Controls.Add(li)

                        li = New HtmlGenericControl("li")
                        li.Attributes.Add("class", "radio")
                        lst = New RadioButton
                        lst.Text = "No"
                        lst.ID = "No"
                        lst.GroupName = "radio"
                        lst.Style.Item("Color") = "#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                        lst.Style.Item("font-family") = "#" & DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                        lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString - 2) & "px"
                        li.Controls.Add(lst)
                        ulRadioYesNoCant.Controls.Add(li)

                        li = New HtmlGenericControl("li")
                        li.Attributes.Add("class", "radio")
                        lst = New RadioButton
                        lst.Text = "Can't Say"
                        lst.ID = "Can't Say"
                        lst.GroupName = "radio"
                        lst.Style.Item("Color") = "#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                        lst.Style.Item("font-family") = "#" & DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                        lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString - 2) & "px"
                        li.Controls.Add(lst)
                        ulRadioYesNoCant.Controls.Add(li)
                    End If
                    SetAnswer(ds1, 6)

                Case "Checkbox Multiple"
                    If ds1.Tables(1).Rows.Count > 0 Then
                        Dim li As HtmlGenericControl
                        Dim lst As CheckBox
                        For index As Integer = 0 To ds1.Tables(1).Rows.Count - 1
                            li = New HtmlGenericControl("li")
                            li.Attributes.Add("class", "checkbox")
                            lst = New CheckBox
                            lst.Text = ds1.Tables(1).Rows(index)(0)
                            lst.ID = ds1.Tables(1).Rows(index)(0)
                            'lst.Attributes("name") = "checkbox"
                            li.Controls.Add(lst)
                            ulCheckBoxes.Controls.Add(li)
                        Next
                    End If

                    If Session("PMode") = "P" Then
                    Else
                        If ds1.Tables(2).Rows.Count > 0 Then
                            For index As Integer = 0 To ds1.Tables(2).Rows.Count - 1
                                If ds1.Tables(2).Rows(index)("AnswerType") <> "AddAns" Then
                                    If Not ulCheckBoxes.FindControl(ds1.Tables(2).Rows(index)("Answer")) Is Nothing Then
                                        DirectCast(ulCheckBoxes.FindControl(ds1.Tables(2).Rows(index)("Answer")), CheckBox).Checked = True
                                    End If
                                End If
                            Next
                        End If
                    End If


                    'additionally...check the ones that have been added newly!
                    If (Not Session("newlyadded") Is Nothing) AndAlso (Not ulCheckBoxes.FindControl(Session("newlyadded")) Is Nothing) Then
                        DirectCast(ulCheckBoxes.FindControl(Session("newlyadded")), CheckBox).Checked = True
                    End If

                    SetAnswer(Nothing, 4)

                Case "Select one of many"
                    If ds1.Tables(1).Rows.Count > 0 Then
                        Dim li As HtmlGenericControl
                        Dim lst As RadioButton
                        For index As Integer = 0 To ds1.Tables(1).Rows.Count - 1
                            li = New HtmlGenericControl("li")
                            li.Attributes.Add("class", "radio")

                            lst = New RadioButton
                            lst.Text = ds1.Tables(1).Rows(index)(0)
                            lst.ID = ds1.Tables(1).Rows(index)(0)
                            lst.GroupName = "radio"
                            lst.Font.Name = DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                            lst.Font.Size = 48
                            lst.ForeColor = System.Drawing.ColorTranslator.FromHtml(DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString)

                            li.Controls.Add(lst)
                            ulRadio.Controls.Add(li)

                        Next

                    End If
                    SetAnswer(ds1, 3)

                Case "Multiple Text Responses"
                    If ds1.Tables(1).Rows.Count > 0 Then
                        Dim li As HtmlGenericControl
                        Dim lst As Label
                        Dim tbox As TextBox
                        For index As Integer = 0 To ds1.Tables(1).Rows.Count - 1
                            li = New HtmlGenericControl("li")
                            li.Attributes.Add("class", "radio")

                            lst = New Label
                            lst.Text = ds1.Tables(1).Rows(index)(0)
                            lst.ID = ds1.Tables(1).Rows(index)(0)
                            lst.Font.Name = DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                            lst.Font.Size = 16
                            lst.ForeColor = System.Drawing.ColorTranslator.FromHtml("#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString)
                            li.Controls.Add(lst)
                            'ulMultipleText.Controls.Add(li)

                            tbox = New TextBox
                            tbox.Text = ""
                            tbox.ID = "txt" + ds1.Tables(1).Rows(index)(0)
                            tbox.Font.Name = DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                            tbox.Font.Size = 16
                            tbox.MaxLength = 200
                            tbox.ForeColor = System.Drawing.ColorTranslator.FromHtml("#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString)
                            li.Controls.Add(tbox)
                            ulMultipleText1.Controls.Add(li)


                        Next

                    End If
                    SetAnswer(ds1, 12)

                Case "Date"
                    divCalendar.Style.Item("Color") = DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                    divCalendar.Style.Item("font-family") = DsImg.Tables(0).Rows(0).Item("PTFont").ToString
                    divCalendar.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFontSize").ToString - 2) & "px"
                    SetAnswer(ds1, 5)
                Case "Number", "Text(100)"
                    SetAnswer(ds1, 9)

                Case "Agree - Disagree"
                    If ds1.Tables(1).Rows.Count > 0 Then
                        Dim li As HtmlGenericControl
                        Dim lst As RadioButton

                        li = New HtmlGenericControl("li")
                        li.Attributes.Add("class", "radio")
                        lst = New RadioButton
                        lst.Text = "Completely Disagree"
                        lst.ID = "Disagree"
                        lst.GroupName = "radio"
                        lst.Style.Item("Color") = "#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                        lst.Style.Item("font-family") = "#" & DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                        'lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString - 2) & "px"
                        lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString) & "px"
                        li.Controls.Add(lst)
                        ulAgreeDisagree.Controls.Add(li)
                        hdnDisagreecomp.Value = ds1.Tables(1).Rows(0)("marks").ToString


                        li = New HtmlGenericControl("li")
                        li.Attributes.Add("class", "radio")
                        lst = New RadioButton
                        lst.Text = "Partially Disagree"
                        lst.ID = "PartiallyDisagree"
                        lst.GroupName = "radio"
                        lst.Style.Item("Color") = "#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                        lst.Style.Item("font-family") = "#" & DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                        'lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString - 2) & "px"
                        lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString) & "px"
                        li.Controls.Add(lst)
                        ulAgreeDisagree.Controls.Add(li)
                        hdnDisagreepart.Value = ds1.Tables(1).Rows(1)("marks").ToString

                        li = New HtmlGenericControl("li")
                        li.Attributes.Add("class", "radio")
                        lst = New RadioButton
                        lst.Text = "Partially Agree"
                        lst.ID = "PartiallyAgree"
                        lst.GroupName = "radio"
                        lst.Style.Item("Color") = "#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                        lst.Style.Item("font-family") = "#" & DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                        'lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString - 2) & "px"
                        lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString) & "px"
                        li.Controls.Add(lst)
                        ulAgreeDisagree.Controls.Add(li)
                        hdnAgreepart.Value = ds1.Tables(1).Rows(2)("marks").ToString

                        li = New HtmlGenericControl("li")
                        li.Attributes.Add("class", "radio")
                        lst = New RadioButton
                        lst.Text = "Completely Agree  "
                        lst.ID = "CompletelyAgree"
                        lst.GroupName = "radio"
                        lst.Style.Item("Color") = "#" & DsImg.Tables(0).Rows(0).Item("PTTextColor").ToString
                        lst.Style.Item("font-family") = "#" & DsImg.Tables(0).Rows(0).Item("CSFont").ToString
                        'lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString - 2) & "px"
                        lst.Style.Item("font-size") = (DsImg.Tables(0).Rows(0).Item("PTFont").ToString) & "px"
                        li.Controls.Add(lst)
                        ulAgreeDisagree.Controls.Add(li)
                        hdnAgreecomp.Value = ds1.Tables(1).Rows(3)("marks").ToString
                    End If
                    SetAnswer(ds1, 13)

                Case "Freeform Text"
                    SetAnswer(ds1, 8)
                Case "Scale(1-100)"
                    SetAnswer(ds1, 2)
                Case "CAMERA"
                    RefreshCamera()
                    SetAnswer(Nothing, 11)
                Case Else

            End Select

            If ds1.Tables(0).Rows(0)("mandatory") = "1" Then
                hdnValidateMe.Value = "1"
            Else 
                hdnValidateMe.Value = "0"
            End If

            If ds1.Tables(0).Rows(0)("AddNewOption") = "1" Then
                Qcontainer.Controls.Add(New LiteralControl("<br />"))
                Dim lblAdd As New Label
                If ds1.Tables(0).Rows(0)("AddAdditionalHdr") = "" Then
                    lblAdd.Text = "Additional Comment:"
                Else
                    lblAdd.Text = ds1.Tables(0).Rows(0)("AddAdditionalHdr").ToString
                End If
                lblAdd.Font.Bold = True
                Qcontainer.Controls.Add(lblAdd)


                Dim txt As New TextBox
                txt.ID = "txtTextAdd"

                If ds1.Tables(0).Rows(0)("AddAdditionalAnsType").ToString = "Textbox" Then
                    txt.TextMode = TextBoxMode.SingleLine
                    txt.Attributes.Add("onkeyup", "countChar(this);")
                Else
                    txt.TextMode = TextBoxMode.MultiLine
                    txt.Rows = 4
                    txt.Attributes.Add("onkeyup", "countCharTA(this);")
                End If


                Dim divChars As New HtmlGenericControl("div")
                divChars.ID = "charNum"

                Dim ctrlcount As Integer = 0
                If ds1.Tables.Count > 2 Then
                    If ds1.Tables(2).Rows.Count > 0 Then
                        ctrlcount = ds1.Tables(2).Rows.Count
                    End If
                End If
                Dim i As Integer = 0
                While ctrlcount > 0
                    If ds1.Tables(2).Rows(i)(0) = "AddAns" Then
                        txt.Text = ds1.Tables(2).Rows(i)(1)
                        Exit While
                    End If
                    i += 1
                    ctrlcount -= 1
                End While

                Qcontainer.Controls.Add(txt)
                Qcontainer.Controls.Add(divChars)

            End If

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try

    End Sub

    Protected Sub SetAnswer(ByVal ds1 As DataSet, ByVal divno As Integer)
        If Not ds1 Is Nothing Then
            If ds1.Tables.Count > 2 Then
                If ds1.Tables(2).Rows.Count > 0 Then
                    hdnAnswer.Value = ds1.Tables(2).Rows(0)("answer")
                Else
                    hdnAnswer.Value = ""
                End If
            Else
                hdnAnswer.Value = ""
            End If
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "showconfpopupscript2", "showdiv(" + divno.ToString + ");", True)
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Sub UpdateSequenceNumber(ByVal seq As String)
        Try
            If seq <> "" Then
                Dim db As New DBHelperClient
                Dim parms(2) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("i_newseq", seq)
                parms(1) = New DBHelperClient.Parameters("i_tablename", "tblSurveyResultDetail")
                parms(2) = New DBHelperClient.Parameters("i_keycolumname", "SurveyResultDetailID")
                db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_UpdateFieldOrder", parms)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub ChangeOrder(ByVal sender As Object, ByVal e As RepeaterCommandEventArgs) Handles rptAnswers.ItemCommand
        Try
            If e.CommandName = "up" Or e.CommandName = "down" Then
                Dim rowIndex As Integer = e.Item.ItemIndex
                Dim id As Integer = Convert.ToInt32(e.CommandArgument)
                Dim neworder As Integer = DirectCast(e.CommandSource, ImageButton).ToolTip
                neworder = If(e.CommandName = "up", neworder - 1, neworder + 1)
                UpdateOrder(id, neworder)

                rowIndex = If(e.CommandName = "up", rowIndex - 1, rowIndex + 1)
                id = Convert.ToInt32(DirectCast(rptAnswers.Items(rowIndex).FindControl("lnkTICROWID"), HtmlGenericControl).InnerHtml)
                neworder = Convert.ToInt32(DirectCast(rptAnswers.Items(rowIndex).FindControl("lblOrder"), HtmlGenericControl).InnerHtml)
                neworder = If(e.CommandName = "up", neworder + 1, neworder - 1)
                UpdateOrder(id, neworder)

                SetSessionRankData()
                Session("SORTEVENT") = "Y"
                Session("AllowRanking") = "Y"
                gotonext(sender, e)
            End If
        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try
    End Sub

    Private Sub SetSessionRankData()
        Try
            Dim parms(1) As DBHelperClient.Parameters
            Dim db As New DBHelperClient
            Dim ds1 As New DataSet
            parms(0) = New DBHelperClient.Parameters("i_SQID", Session("SQID"))
            parms(1) = New DBHelperClient.Parameters("i_surveyguid", Session("surveyguid"))
            ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_GetDragDropAnswers", parms)
            Session("RANKDATA") = ds1.Tables(0)
        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try

    End Sub
    Private Sub UpdateOrder(ByVal id As Integer, ByVal neworder As Integer)
        Try
            Dim db As New DBHelperClient
            Dim parms(3) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("i_neworder", neworder)
            parms(1) = New DBHelperClient.Parameters("i_tablename", "tblSurveyResultDetail")
            parms(2) = New DBHelperClient.Parameters("i_keycolumname", "SurveyResultDetailID")
            parms(3) = New DBHelperClient.Parameters("i_keycolumvalue", id)
            db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_UpdateFieldOrderV2", parms)
        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try

    End Sub

    Protected Sub addnewoption(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            'Add the text to the tblQuestionAnswerValues table
            Session("newlyadded") = txtNewOption.Value.Trim
            Dim parms(4) As DBHelperClient.Parameters
            Dim db As New DBHelperClient
            Dim ds1 As New DataSet
            parms(0) = New DBHelperClient.Parameters("i_qid", Session("SQID"))
            parms(1) = New DBHelperClient.Parameters("i_QAVText", txtNewOption.Value.Trim)
            parms(2) = New DBHelperClient.Parameters("i_userid", "0")
            parms(3) = New DBHelperClient.Parameters("i_order", Session("sqorder"))
            parms(4) = New DBHelperClient.Parameters("i_surveyid", Session("surveyid"))
            Session("sqorder") = db.ExecuteScalar(CommandType.StoredProcedure, "SP_AddNewOption2", parms)
            If Session("sqorder") Is Nothing Then
                Session("sqorder") = "1"
            End If
            txtNewOption.Value = ""
            'refresh the page!...goto same question!
            Session("AllowRanking") = "N"
            gotonext(sender, e)

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try
    End Sub
    Protected Sub sendemailtokarl()
        Try
            Dim parms(1) As DBHelperClient.Parameters
            Dim db As New DBHelperClient
            Dim ds1 As New DataSet
            parms(0) = New DBHelperClient.Parameters("p_iGUID", Session("surveyguid"))
            parms(1) = New DBHelperClient.Parameters("p_meetingguid", Session("meetingguid"))
            if Session("SurveyId") = "1" Then
                ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_SHOWSURVEYRESULTS", parms)
            Else
                ds1 = db.DataAdapter(CommandType.StoredProcedure, "SP_SHOWSURVEYRESULTSAP", parms)
            End If
            If ds1.Tables.Count > 1 Then


                'CHANGE WHEN MOVING TO PROD UNCOMMENT
                'CHANGE WHEN MOVING TO PROD UNCOMMENT
                'CHANGE WHEN MOVING TO PROD UNCOMMENT
                'CHANGE WHEN MOVING TO PROD UNCOMMENT
                'CHANGE WHEN MOVING TO PROD UNCOMMENT
                'CHANGE WHEN MOVING TO PROD UNCOMMENT

                'Dim message As New MailMessage("ITSupport@tic-us.com", "kandersson@tic-us.com," + ds1.Tables(0).Rows(0)("createdby") + "," + ds1.Tables(0).Rows(0)("assignedto"), _
                '                                "Survey Results for " + ds1.Tables(0).Rows(0)(0) + _
                '                               " by " + ds1.Tables(0).Rows(0)(2), getHTML(ds1.Tables(0)) + vbCrLf + getHTML(ds1.Tables(1)))
                If ds1.Tables(0).Rows.Count > 0 Then
                    Dim message As New MailMessage("ITSupport@tic-us.com", ds1.Tables(0).Rows(0)("createdby"), _
                                    "Survey Results for " + ds1.Tables(0).Rows(0)(0) + _
                                   " by " + ds1.Tables(0).Rows(0)(2), getHTML(ds1.Tables(0)) + vbCrLf + getHTML(ds1.Tables(1)))

                    'CHANGE WHEN MOVING TO PROD UNCOMMENT
                    'CHANGE WHEN MOVING TO PROD UNCOMMENT
                    'CHANGE WHEN MOVING TO PROD UNCOMMENT
                    'CHANGE WHEN MOVING TO PROD UNCOMMENT
                    'CHANGE WHEN MOVING TO PROD UNCOMMENT
                    'CHANGE WHEN MOVING TO PROD UNCOMMENT

                    'message.CC.Add(New MailAddress("kandersson@tic-us.com"))
                    'message.Bcc.Add(New MailAddress("moak@tic-us.com"))
                    'also attach the files!
                    If ds1.Tables.Count > 2 Then
                        For index As Integer = 0 To ds1.Tables(2).Rows.Count - 1
                            If Not ds1.Tables(2).Rows(index)("attachment") Is System.DBNull.Value Then
                                Dim memStream As New MemoryStream(DirectCast(ds1.Tables(2).Rows(index)("attachment"), Byte()))
                                Dim attach As New System.Net.Mail.Attachment(memStream, ds1.Tables(2).Rows(index)("AttachmentFile").ToString)
                                message.Attachments.Add(attach)
                            End If
                        Next
                    End If

                    'message.IsBodyHtml = True
                    ''Dim smtp As New SmtpClient("adobe")
                    'Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
                    'smtp.Port = "587"
                    'smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "6fc74441ed8e9b225838843063cb0abb8dca0326")
                    'Dim token As [Object] = Nothing
                    'smtp.SendAsync(message, token)
                End If

                

            End If

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message + vbCrLf + ex.InnerException.Message
        End Try
    End Sub
    Private Shared Function getHTML(ByVal dt As DataTable) As String
        Dim myBuilder As New StringBuilder()

        myBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ")
        'myBuilder.Append("style='border: solid 1px Silver; font-size: Large;'>")
        myBuilder.Append("style='border: solid 1px Silver; font-size: 11pt;font-family:Arial;'>")

        myBuilder.Append("<tr align='center' valign='top'>")
        For Each myColumn As DataColumn In dt.Columns
            myBuilder.Append("<td align='left' valign='top'><b>")
            myBuilder.Append(myColumn.ColumnName)
            myBuilder.Append("</b></td>")
        Next
        myBuilder.Append("</tr>")

        For Each myRow As DataRow In dt.Rows
            myBuilder.Append("<tr align='left' valign='top'>")
            For Each myColumn As DataColumn In dt.Columns
                myBuilder.Append("<td align='left' valign='top'>")
                myBuilder.Append(myRow(myColumn.ColumnName).ToString())
                myBuilder.Append("</td>")
            Next
            myBuilder.Append("</tr>")
        Next
        myBuilder.Append("</table>")

        Return myBuilder.ToString()
    End Function

    Protected Sub AddAttachment(ByVal sender As Object, ByVal e As EventArgs)
        Try

            If HttpContext.Current.Session("SurveyResultHeaderID") <> 0 Then
                'Session("ANSWER") = txtAttachDesc.Value.Trim
                ' Read the file and convert it to Byte Array
                Dim br As BinaryReader = New BinaryReader(txtFileUpload.PostedFile.InputStream)
                Dim bytes As Byte() = br.ReadBytes(Convert.ToInt32(txtFileUpload.PostedFile.InputStream.Length))
                br.Close()
                'insert the file into database
                Dim strQuery As String = "if not exists(select * from tblSurveyAttachments where txtDesc = @i_txtDesc and AttachmentFile = @i_AttachmentFile and attachmentcontenttype = @i_attachmentcontenttype and @i_SurveyResultHeaderID = SurveyResultHeaderID) Insert into tblSurveyAttachments values(@i_SurveyResultHeaderID,@i_txtDesc,@i_AttachmentFile,@Data,@i_attachmentcontenttype,@i_UserName,GETDATE(),NULL,NULL)"
                Dim cmd As SqlCommand = New SqlCommand(strQuery)
                cmd.Parameters.Add("@i_SurveyResultHeaderID", SqlDbType.Int).Value = HttpContext.Current.Session("SurveyResultHeaderID")
                cmd.Parameters.Add("@i_UserName", SqlDbType.VarChar).Value = IIf(Session("user_id") Is Nothing, "", Session("user_id"))
                cmd.Parameters.Add("@i_txtDesc", SqlDbType.VarChar).Value = txtAttachDesc.Value.Trim
                cmd.Parameters.Add("@i_AttachmentFile", SqlDbType.VarChar).Value = txtFileUpload.PostedFile.FileName
                cmd.Parameters.Add("@Data", SqlDbType.VarBinary).Value = bytes
                cmd.Parameters.Add("@i_attachmentcontenttype", SqlDbType.VarChar).Value = txtFileUpload.PostedFile.ContentType

                Dim strConnString As String = Session("clientconnstring")
                Dim con As New SqlConnection(strConnString)
                cmd.CommandType = CommandType.Text
                cmd.Connection = con
                con.Open()
                cmd.ExecuteNonQuery()
            End If

            txtAttachDesc.Value = ""

            RefreshCamera()

        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try

    End Sub

    Protected Sub showattachments(ByVal sender As Object, ByVal e As System.EventArgs)
        Try

            Dim db As New DBHelperClient
            Dim ds As New DataSet
            'Dim strQuery As String = "select AttachmentFile, Attachment,Attachmentcontenttype from tblDocumentDiscussionNotes where noteid=111"
            Dim strQuery As String = "select AttachmentFile, Attachment,Attachmentcontenttype from tblSurveyAttachments where SurveyAttachID=" & DirectCast(sender, ImageButton).CommandArgument
            ds = db.DataAdapter(CommandType.Text, strQuery)
            Dim bytes() As Byte = CType(ds.Tables(0).Rows(0)("Attachment"), Byte())

            Response.Buffer = True
            Response.Charset = ""
            'Response.Cache.SetCacheability(HttpCacheability.NoCache)
            Response.ContentType = ds.Tables(0).Rows(0)("Attachmentcontenttype").ToString()
            Dim strFileName As String = ""
            If ds.Tables(0).Rows(0)("AttachmentFile").ToString().Contains("\") Then
                strFileName = ds.Tables(0).Rows(0)("AttachmentFile").ToString().Substring(InStrRev(ds.Tables(0).Rows(0)("AttachmentFile").ToString(), "\"), ds.Tables(0).Rows(0)("AttachmentFile").ToString().Length - InStrRev(ds.Tables(0).Rows(0)("AttachmentFile").ToString(), "\"))
            Else
                strFileName = ds.Tables(0).Rows(0)("AttachmentFile").ToString()
            End If
            Response.AddHeader("content-disposition", "attachment;filename=" & strFileName)
            Response.BinaryWrite(bytes)
            Response.Flush()
            'Response.Close()
            Response.End()


        Catch ex As Exception
            lblErrorMessage.Visible = True
            lblErrorMessage.Text = ex.Message
        End Try
    End Sub


End Class
