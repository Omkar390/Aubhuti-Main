Imports System.Data

Partial Class SurveyQuestion
    Inherits System.Web.UI.Page

    Private Sub SurveyQuestion_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack = True Then

            Try
                'hidden field stores value of the surveyID
                hdnSurveyID.Value = Request.QueryString("SurveyID")
                hdnSurveyQuestionID.Value = 0
                BindData()
                ClearAll()
                divQuesAns.Style.Item("display") = "none"
            Catch ex As Exception
                lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
                lblErrorMsg.Visible = True
                SSManager.LogAppError(ex, "Page_Load")
            End Try
            'If ddltabledata.Items("select").Value Then
            '    ddltabledata.Visible = False
            '    Return
            'End If
        End If
    End Sub

    'Bind data with the values of the is active and ranking etc
    Protected Sub BindData()

        Dim strSQL As String
        Dim db As New DBHelperClient
        Dim parms1(0) As DBHelperClient.Parameters
        parms1(0) = New DBHelperClient.Parameters("surveyid", hdnSurveyID.Value)

        'it uses client_2002 database and uses tblsurveyquestion table it shows sqrowid, qtext, active, mandatory, qorder, anstype and tabledata
        strSQL = "Select sqrowid, qtext, case coalesce(active, 0) when 1 then 'Yes' else 'No' End as active, " &
                " case coalesce(mandatory, 0) when 1 then 'Yes' else 'No' end as mandatory, qorder ," &
                " anstype,Tabledata,intelligencetype, " &
                " case  ifnull(intelligencetype ,'') when 1 then 'Linguistic' when 2 then 'Logical'  " &
                " when 3 then 'Musical' when 4 then 'Bodily-Kinaesthetic' when 5 then 'Spatial' when 6 then 'Interpersonal'  " &
                " when 7 then 'Intrapersonal' Else '' end as itype " &
                "  from tblsurveyquestion where surveyid = @surveyid order by qorder "
        Dim dt As New DataTable
        dt = db.DataAdapter(CommandType.Text, strSQL,parms1).Tables(0)
        rptSurveyQuestions.DataSource = dt
        rptSurveyQuestions.DataBind()

        'bind goto DDL
        DropDownList2.AppendDataBoundItems = True
        DropDownList2.DataTextField = "qtext"
        DropDownList2.DataValueField = "sqrowid"
        DropDownList2.DataSource = dt
        DropDownList2.DataBind()
        'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "showhidedivscript", "return showhidediv();", True)
        ScriptManager.RegisterStartupScript(Page, Me.Page.[GetType](), "showhidedivscript", "return showhidediv();", True)

        'DropDownList2.Items.FindByValue(dt.Rows(0).Item("LogicQIDGoto").ToString).Selected = True
        'DropDownList1.Items.FindByValue(dt.Rows(0).Item("LogicAnswerValue").ToString).Selected = True
        'chkHaslogic.Checked = dt.Rows(0).Item("HasLogic").ToString

    End Sub
    'Bind question and answer to table (repeater)
    Protected Sub BindQA(byval sqrowid As String)

        Dim strSQL As String
        Dim db As New DBHelperClient
        Dim parms1(0) As DBHelperClient.Parameters
        parms1(0) = New DBHelperClient.Parameters("sqrowid", sqrowid)

        'in client_2002 database(tblsurveyquestionanswer) it shows sqarowid, sqrowid, qatext, createdby, createddts using the sqrowid
        strSQL = " Select * from tblsurveyquestionanswer where sqrowid = @sqrowid "
                    rptQuesAns.DataSource = db.DataAdapter(CommandType.Text, strSQL,parms1)
                    rptQuesAns.DataBind()
        txtDesc.Text = ""

    End Sub

    Protected Sub ClearAll()

        txtqtext.Value = ""
        chkActive.Checked = False
        chkMandatory.Checked = False
        chknewoption.Checked = False
        txtaddcomment.Value = ""
        chkshowcomment.Checked = False
        ddlCommentType.ClearSelection()
        txtheader.Value = ""
        chkranking.Checked = False
        ddlAnsType.ClearSelection()
        ddlintell.ClearSelection()
        rptQuesAns.DataSource = Nothing
        rptQuesAns.DataBind()
        divQuesAns.Style.Item("display") = "none"


        chkHaslogic.Checked = False
        DropDownList1.ClearSelection
        DropDownList1.Items(0).Selected = True
        DropDownList2.ClearSelection
        If DropDownList2.Items.Count > 0 Then
            DropDownList2.Items(0).Selected = True
        End If
        If ddlintell.Items.Count > 0 Then
            ddlintell.Items(0).Selected = True
        End If


    End Sub


    'save button for inserting or updating data
    Private Sub btnSave_ServerClick(sender As Object, e As EventArgs) Handles btnSave.ServerClick

        Try
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim parms(17) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_surveyid", hdnSurveyID.Value)
            parms(1) = New DBHelperClient.Parameters("p_qtext", txtqtext.Value)
            parms(2) = New DBHelperClient.Parameters("p_active", chkActive.Checked)
            parms(3) = New DBHelperClient.Parameters("p_mandatory", chkMandatory.Checked)
            parms(4) = New DBHelperClient.Parameters("p_addnewoption", chknewoption.Checked)
            parms(5) = New DBHelperClient.Parameters("p_showcomment", chkshowcomment.Checked)
            parms(6) = New DBHelperClient.Parameters("p_commenttype", ddlCommentType.SelectedValue)
            parms(7) = New DBHelperClient.Parameters("p_commentheader", txtheader.Value)
            parms(8) = New DBHelperClient.Parameters("p_allowranking", chkActive.Checked)
            parms(9) = New DBHelperClient.Parameters("p_anstype", ddlAnsType.SelectedValue)
            parms(10) = New DBHelperClient.Parameters("p_User", Session("user_id"))
            parms(12) = New DBHelperClient.Parameters("p_haslogic", chkHaslogic.Checked)
            parms(13) = New DBHelperClient.Parameters("p_LogicAnswerValue", DropDownList1.SelectedValue)
            parms(14) = New DBHelperClient.Parameters("p_LogicQIDGoto", DropDownList2.SelectedValue)
            parms(15) = New DBHelperClient.Parameters("sqrowid", hdnSurveyQuestionID.Value)
            parms(16) = New DBHelperClient.Parameters("p_Tabledata", ddltabledata.SelectedValue)
            parms(17) = New DBHelperClient.Parameters("p_intelligencetype", ddlintell.SelectedValue)

            Dim parms1(0) As DBHelperClient.Parameters
            parms1(0) = New DBHelperClient.Parameters("surveyid", hdnSurveyID.Value)
            strSQL = " select COALESCE(MAX(COALESCE(qorder, 0)), 0) + 1  from tblsurveyquestion where surveyid = @surveyid "
            hdnMaxQNo.Value = db.ExecuteScalar(CommandType.Text, strSQL, parms1)
            parms(11) = New DBHelperClient.Parameters("p_MaxQNo", hdnMaxQNo.Value)

            If hdnSurveyQuestionID.Value = 0 Then
                'in client_2002(tblsurveyquestion) it insert into all columns 
                strSQL = " Insert into tblsurveyquestion (surveyid, qtext, active, mandatory, addnewoption, " &
                        " showcomment, commenttype, commentheader, allowranking, anstype, Tabledata, intelligencetype,qorder, createdby, createdts, " &
                        " updatedby, updatedts,HasLogic,LogicAnswerValue,LogicQIDGoto ) " &
                        " Values (@p_surveyid, @p_qtext, @p_active, @p_mandatory, @p_addnewoption, " &
                        " @p_showcomment, @p_commenttype, @p_commentheader, @p_allowranking, @p_anstype,@p_Tabledata,@p_intelligencetype ,@p_MaxQNo , @p_User, CURRENT_TIMESTAMP, " &
                        " @p_user, CURRENT_TIMESTAMP,@p_haslogic,@p_LogicAnswerValue,@p_LogicQIDGoto ) ; " &
                        " Select LAST_INSERT_ID() ; "
                hdnSurveyQuestionID.Value = db.ExecuteScalar(CommandType.Text, strSQL, parms)
                If ddlAnsType.SelectedValue = "Select one of many" Or ddlAnsType.SelectedValue = "Checkbox Multiple" Then
                    divQuesAns.Style.Item("display") = "block"
                    BindQA(hdnSurveyQuestionID.Value)
                Else
                    divQuesAns.Style.Item("display") = "none"
                    'ClearAll()
                End If
                BindData()
            Else
                'in client_2002(tblsurveyquestion) updates values from the database if existing record is selected
                strSQL = " Update tblsurveyquestion set surveyid = @p_surveyid, qtext = @p_qtext, active = @p_active " &
                        " , mandatory = @p_mandatory, addnewoption = @p_addnewoption, showcomment = @p_showcomment " &
                        " , commenttype = @p_commenttype, commentheader = @p_commentheader, allowranking = @p_allowranking " &
                        " , anstype = @p_anstype,Tabledata= @p_Tabledata,intelligencetype=@p_intelligencetype ,updatedby = @p_User, updatedts = CURRENT_TIMESTAMP " &
                        " , HasLogic = @p_haslogic,LogicAnswerValue = @p_LogicAnswerValue,LogicQIDGoto = @p_LogicQIDGoto " &
                        " Where  sqrowid = @sqrowid "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
                BindData()
            End If

            If (ddlAnsType.SelectedValue = "Yes or No" Or ddlAnsType.SelectedValue = "Yes or No or Can't Say" Or ddlAnsType.SelectedValue = "Agree - Disagree") Then
                If rptQuesAns.Items.Count > 0 Then

                    Dim parms2(0) As DBHelperClient.Parameters
                    parms2(0) = New DBHelperClient.Parameters("sqrowid", hdnSurveyQuestionID.Value)
                    'strSQL = "Delete from tblsurveyquestionanswer where sqrowid = @sqrowid "
                    strSQL = "Delete from tblsurveyquestionanswer where sqrowid = @sqrowid "
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms2)
                End If

                Dim parmsQ(3) As DBHelperClient.Parameters
                parmsQ(0) = New DBHelperClient.Parameters("p_sqrowid", hdnSurveyQuestionID.Value)
                parmsQ(1) = New DBHelperClient.Parameters("p_User", Session("user_id"))
                'in client_2002(tblsurveyquestionanswer) sqrowid, qatext, createdby, createdts this values we inserting into this table
                strSQL = " Insert into tblsurveyquestionanswer (sqrowid, qatext, createdby, createdts,marks) Values (@p_sqrowid, @p_qatext, @p_User, CURRENT_TIMESTAMP,@p_marks) "


                Select Case ddlAnsType.SelectedValue
                    Case "Select one of many"
                        'pop up
                    Case "Checkbox Multiple"
                        'pop up
                    Case "Yes or No"
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "Yes")
                        parmsQ(3) = New DBHelperClient.Parameters("p_marks", "1")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "No")
                        parmsQ(3) = New DBHelperClient.Parameters("p_marks", "0")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)

                    Case "Yes or No or Can't Say"
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "Yes")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "No")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "Can't Say")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)

                    Case "Agree - Disagree"
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "Completely Disagree")
                        parmsQ(3) = New DBHelperClient.Parameters("p_marks", "1")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "Partially Disagree")
                        parmsQ(3) = New DBHelperClient.Parameters("p_marks", "2")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "Partially Agree")
                        parmsQ(3) = New DBHelperClient.Parameters("p_marks", "3")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)
                        parmsQ(2) = New DBHelperClient.Parameters("p_qatext", "Completely Agree")
                        parmsQ(3) = New DBHelperClient.Parameters("p_marks", "4")
                        db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)
                    Case Else
                        'hide repeater
                End Select
            End If
            ClearAll()
            lblSuccessMsg.Text = "Test has been updated successfully"
            lblSuccessMsg.Visible = True
            hdnSurveyQuestionID.Value = 0
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "btnSave_ServerClick")
        End Try
    End Sub


    'repeater template for the commands to execute(edit,delete,up and down)
    Private Sub rptSurveyQuestions_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptSurveyQuestions.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                hdnSurveyQuestionID.Value = e.CommandArgument

                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms1(0) As DBHelperClient.Parameters
                parms1(0) = New DBHelperClient.Parameters("sqrowid", e.CommandArgument)
                'in client_2002(tblsurveyquestion) we select all columns
                strSQL = "Select qtext, active, mandatory, addnewoption, showcomment, commentheader, " &
                        " commenttype, allowranking, anstype,Coalesce(Tabledata,'select') as Tabledata,intelligencetype,Coalesce(HasLogic, 0) as HasLogic, " &
                        " Coalesce(LogicAnswerValue, '') as LogicAnswerValue, " &
                        " Coalesce(LogicQIDGoto, '') as LogicQIDGoto, " &
                        " case  ifnull(intelligencetype ,'') when 1 then 'Linguistic' when 2 then 'Logical'  " &
                        " when 3 then 'Musical' when 4 then 'Bodily-Kinaesthetic' when 5 then 'Spatial' when 6 then 'Interpersonal'  " &
                        " when 7 then 'Intrapersonal' Else '' end as itype " &
                        " from tblsurveyquestion where sqrowid = @sqrowid "
                Dim dt As New DataTable
                dt = db.DataAdapter(CommandType.Text, strSQL, parms1).Tables(0)
                'data is displayed on the texboxes and other controls 
                txtqtext.Value = dt.Rows(0).Item("qtext").ToString
                chkActive.Checked = dt.Rows(0).Item("active").ToString
                chkMandatory.Checked = dt.Rows(0).Item("mandatory").ToString
                chkNewOption.Checked = dt.Rows(0).Item("addnewoption").ToString
                chkShowComment.Checked = dt.Rows(0).Item("showcomment").ToString
                txtaddcomment.Value = dt.Rows(0).Item("commentheader").ToString
                ddlCommentType.ClearSelection()
                ddlCommentType.Items.FindByValue(dt.Rows(0).Item("commenttype").ToString).Selected = True
                txtheader.Value = dt.Rows(0).Item("commentheader").ToString
                chkranking.Checked = dt.Rows(0).Item("allowranking").ToString
                ddlAnsType.ClearSelection()
                ddlAnsType.Items.FindByValue(dt.Rows(0).Item("anstype").ToString).Selected = True
                ddltabledata.ClearSelection()
                ddltabledata.Items.FindByValue(dt.Rows(0).Item("Tabledata").ToString).Selected = True
                ddlintell.SelectedValue = dt.Rows(0).Item("intelligencetype").ToString


                chkHaslogic.Checked = dt.Rows(0).Item("HasLogic").ToString
                DropDownList1.ClearSelection()
                If dt.Rows(0).Item("LogicAnswerValue").ToString = "" Then
                Else
                    DropDownList1.Items.FindByValue(dt.Rows(0).Item("LogicAnswerValue").ToString).Selected = True
                End If

                DropDownList2.ClearSelection()
                If dt.Rows(0).Item("LogicQIDGoto").ToString = "" Then
                Else
                    DropDownList2.Items.FindByValue(dt.Rows(0).Item("LogicQIDGoto").ToString).Selected = True
                End If

                If ddlAnsType.SelectedValue = "Select one of many" Or ddlAnsType.SelectedValue = "Checkbox Multiple" Then
                    divQuesAns.Style.Item("display") = "block"
                    BindQA(hdnSurveyQuestionID.Value)
                Else
                    divQuesAns.Style.Item("display") = "none"
                End If
                If ddlAnsType.SelectedValue = "Select Table" Then
                    ddltabledata.Style.Item("display") = "block"
                    BindQA(hdnSurveyQuestionID.Value)
                Else
                    ddltabledata.Style.Item("display") = "none"
                End If



            ElseIf e.CommandName = "Del" Then
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms1(0) As DBHelperClient.Parameters
                parms1(0) = New DBHelperClient.Parameters("sqrowid", e.CommandArgument)
                strSQL = "Delete from tblsurveyquestionanswer where sqrowid = @sqrowid "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms1)
                strSQL = "Delete from tblsurveyquestion where sqrowid = @sqrowid "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms1)
                BindData()
            ElseIf e.CommandName = "Up" Then

                If CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) = 1 Then
                Else
                    Dim sqrowidprev As Integer
                    Dim strSQL As String
                    Dim db As New DBHelperClient
                    Dim parms1(3) As DBHelperClient.Parameters
                    parms1(0) = New DBHelperClient.Parameters("surveyid", hdnSurveyID.Value)
                    parms1(1) = New DBHelperClient.Parameters("qorder", CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) - 1)
                    parms1(2) = New DBHelperClient.Parameters("sqrowid", e.CommandArgument)
                    parms1(3) = New DBHelperClient.Parameters("sqrowidprev", sqrowidprev)


                    'in client_2002(tblsurveyquestion) it selects the sqrowid using the surveyid and question order
                    strSQL = "Select sqrowid from tblsurveyquestion where surveyid = @surveyid and qorder = @qorder "
                    sqrowidprev = db.ExecuteScalar(CommandType.Text, strSQL, parms1)

                    'in client_2002(tblsurveyquestion) it updates the  question order using sqrowid
                    strSQL = "Update tblsurveyquestion set qorder = @qorder where sqrowid = @sqrowid "
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms1)

                    'in client_2002(tblsurveyquestion) add an qorder in this query
                    strSQL = "Update tblsurveyquestion set qorder = qorder + 1 where sqrowid = @sqrowidprev "
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms1)

                    BindData()
                End If
            ElseIf e.CommandName = "Down" Then
                Dim sqrowidnext As Integer
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms1(3) As DBHelperClient.Parameters
                parms1(0) = New DBHelperClient.Parameters("surveyid", hdnSurveyID.Value)
                parms1(1) = New DBHelperClient.Parameters("qorder", CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) + 1)
                parms1(2) = New DBHelperClient.Parameters("sqrowid", e.CommandArgument)
                'in client_2002(tblsurveyquestion) it checks non-null values for the surveyid passed below
                strSQL = " select max(coalesce(qorder, 0))  from tblsurveyquestion where surveyid = @surveyid "
                hdnMaxQNo.Value = db.ExecuteScalar(CommandType.Text, strSQL, parms1)

                If CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) = hdnMaxQNo.Value Then
                Else
                    'in client_2002(tblsurveyquestion) it checks for the sqrowid from the surveyid and qorder
                    strSQL = "Select sqrowid from tblsurveyquestion where surveyid = @surveyid and qorder = @qorder "
                    sqrowidnext = db.ExecuteScalar(CommandType.Text, strSQL, parms1)
                    'IN client_2002(tblsurveyquestion) set order for the sqrowid 
                    strSQL = "Update tblsurveyquestion set qorder = @qorder where sqrowid = @sqrowid "
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms1)

                    parms1(3) = New DBHelperClient.Parameters("sqrowidnext", sqrowidnext)
                    'IN client_2002(tblsurveyquestion)it upward the qorder for the sqrowid
                    strSQL = "Update tblsurveyquestion set qorder = qorder - 1 where sqrowid = @sqrowidnext "
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms1)

                    BindData()
                End If


            End If

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptSurveyQuestions_ItemCommand")
        End Try

    End Sub


    'clear all button
    Private Sub btlClear_ServerClick(sender As Object, e As EventArgs) 'Handles btlClear.ServerClick
        ClearAll()
        hdnSurveyQuestionID.Value = 0
    End Sub


    'Answers are saved in the tblsurveyquestionanswer table
    Private Sub btnSaveAns_ServerClick(sender As Object, e As EventArgs) Handles btnSaveAns.ServerClick
        Try
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim parmsQ(2) As DBHelperClient.Parameters
            parmsQ(0) = New DBHelperClient.Parameters("p_sqrowid", hdnSurveyQuestionID.Value)
            parmsQ(1) = New DBHelperClient.Parameters("p_User", Session("user_id"))
            parmsQ(2) = New DBHelperClient.Parameters("p_qatext", txtDesc.Text)
            'IN client_2002(tblsurveyquestionanswer) we are inserting sqrowid, qatext, createdby and createddts
            strSQL = " Insert into tblsurveyquestionanswer (sqrowid, qatext, createdby, createdts) Values (@p_sqrowid, @p_qatext, @p_User, CURRENT_TIMESTAMP) "
            db.ExecuteNonQuery(CommandType.Text, strSQL, parmsQ)

            BindQA(hdnSurveyQuestionID.Value)

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "btnSaveAns_ServerClick")
        End Try
    End Sub


    'repeater for delete button
    Private Sub rptQuesAns_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptQuesAns.ItemCommand
        Try
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim parms1(0) As DBHelperClient.Parameters
            parms1(0) = New DBHelperClient.Parameters("sqarowid", e.CommandArgument)
            'Client_2002(tblsurveyquestionanswer) deletes record using the sqarowid
            strSQL = " Delete from tblsurveyquestionanswer where sqarowid = @sqarowid "
            db.ExecuteNonQuery(CommandType.Text, strSQL,parms1)

            BindQA(hdnSurveyQuestionID.Value)

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptQuesAns_ItemCommand")
        End Try
    End Sub

    Private Sub btntab1_ServerClick(sender As Object, e As EventArgs) Handles btntab1.ServerClick
        Response.Redirect("SurveyOverview.aspx?SurveyID=" & hdnSurveyID.Value)
    End Sub

    Private Sub btntab2_ServerClick(sender As Object, e As EventArgs) Handles btntab2.ServerClick
        If hdnSurveyID.Value = 0 Then
            lblErrorMsg.Text = "Please Save survey first."
        Else
            Response.Redirect("SurveyDesign.aspx?SurveyID=" & hdnSurveyID.Value)
        End If
    End Sub

    Private Sub btntab4_ServerClick(sender As Object, e As EventArgs) Handles btntab4.ServerClick
        If hdnSurveyID.Value = 0 Then
            lblErrorMsg.Text = "Please Save survey first."
        Else
            Response.Redirect("SurveyFinalize.aspx?SurveyID=" & hdnSurveyID.Value)
        End If
    End Sub
End Class
