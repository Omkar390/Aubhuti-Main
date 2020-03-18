Imports System.Data
Imports System.Web.UI.Control
Partial Class showresult
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                BindSurvey()
            Catch ex As Exception
                lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
                lblErrorMsg.Visible = True
                SSManager.LogAppError(ex, "Page_Load")
            End Try
        End If
    End Sub

    Protected Sub BindSurvey()

        Dim db As New DBHelperClient
        Dim strSQL As String


        'strSQL = "select U.UserName,tsd.Surveyquestion,tsd.Answertype,tsd.answer from tblsurveyresultdetail tsd " &
        '         " inner join tblsurveyquestion tq on  tsd.surveyquestionid = tq.sqrowid " &
        '         " left join tblassignedtest ast on tq.SurveyID= ast.SurveyID " &
        '         " inner join client_master.users U on ast.userID= U.userID "
        'strSQL = "select U.UserName,Surveyquestion,tsd.Answertype,tsd.answer " &
        '           " From tblsurveyresultdetail tsd " &
        '          " left join tblassignedtest ast on tsd.surveyquestionid = ast.surveyID " &
        '          " Left join client_master.users U on ast.userID= U.userID "

        strSQL = "select U.UserName,Surveyquestion,tsd.Answertype,tsd.answer " &
                " From tblsurveyresultdetail tsd " &
                " inner join tblsurveyquestion tq on tsd.surveyquestionid = tq.sqrowid " &
                " Left Join tblsurveyresultheader th on tq.surveyid = th.t_account_id " &
                " left join client_master.users U on th.t_contact_id = U.userID "


        rptSurvey1.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
        rptSurvey1.DataBind()

    End Sub


    Protected Sub rptSurvey1_ItemCommand(ByVal source As Object, ByVal e As RepeaterCommandEventArgs) Handles rptSurvey1.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                'Response.Redirect("SurveyOverview.aspx?SurveyID=" & e.CommandArgument, False)
                Response.Redirect("showresult.aspx?userID=" & e.CommandArgument, False)
                'ElseIf e.CommandName = "Del" Then
                '    Dim strSQL As String
                '    Dim db As New DBHelperClient
                '    Dim parms(1) As DBHelperClient.Parameters
                '    parms(0) = New DBHelperClient.Parameters("userid", Session("user_id"))
                '    parms(1) = New DBHelperClient.Parameters("SurveyID", e.CommandArgument)
                '    strSQL = " Update tblsurvey SET isdeleted = '1' ,ModifiedOn = CURRENT_TIMESTAMP " &
        '       " ,ModifiedBy = @userid WHERE SurveyID = @SurveyID "
        '    db.ExecuteNonQuery(Data.CommandType.Text, strSQL, parms)
        '    BindSurvey()
        'ElseIf e.CommandName = "Pre" Then
        '    'Dim strURL As String
        '    'strURL = "http://localhost:54464/Surveypage.aspx?PMODE=P&CGUID=" & Session("CGUID") & "&SurveyID=" & e.CommandArgument
        '    'Response.Redirect(strURL, False)
        '    Dim url As String
        '    url = "http://localhost:54464/Surveypage.aspx?PMODE=P&CGUID=" & Session("CGUID") & "&SGUID=" & e.CommandArgument
        '    Dim s As String = "window.open('" & url + "', '_blank', 'width=screen.width,height=screen.height,fullscreen=yes');"
        '    ClientScript.RegisterStartupScript(Me.GetType(), "OPEN_WINDOW", s, True)
        End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptSurvey_ItemCommand")
        End Try
    End Sub

End Class
