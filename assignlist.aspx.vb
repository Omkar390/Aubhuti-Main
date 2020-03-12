Imports System.Data
Partial Class assignlist
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
        Dim strSQL1 As String
        'strSQL = " SELECT SurveyID, SurveyName, ST.surveytype, Description, " &
        '        " DATE_FORMAT(ST.createdts, '%m/%d/%Y') AS createdts, U.UserName AS createdby, " &
        '        " CASE S.Active WHEN '1' THEN 'Yes' ELSE 'No' END AS Active " &
        '        " FROM tblsurvey S " &
        '        " INNER JOIN tblsurveytype ST ON S.SurveyType = ST.surveytypeid " &
        '        " LEFT OUTER JOIN client_master.users U ON S.CreatedBy = U.userID " &
        '        " WHERE S.isdeleted IS NULL "

        strSQL = "SELECT tst.userID,tst.SurveyID,US.userID, " &
                 " US.UserName FROM client_2002.tblassignedtest tst " &
                 " INNER JOIN client_master.users US ON US.userID = tst.userID " &
                 " WHERE tst.isdeleted Is NULL"
        '" Full JOIN client_master.users U ON S.SurveyID = U.userID " &


        strSQL1 = "SELECT SurveyID,SurveyName,ST.surveytype, U.userID,U.UserName " &
                 " FROM tblsurvey S " &
                 " INNER JOIN tblsurveytype ST ON S.SurveyType = ST.surveytypeid " &
                " LEFT OUTER JOIN client_master.users U ON S.SurveyID = U.userID " &
                " WHERE S.isdeleted Is NULL "


        rptSurvey.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
        rptSurvey.DataBind()
        Repeater1.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL1).Tables(0)
        Repeater1.DataBind()

    End Sub


    Protected Sub rptSurvey_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptSurvey.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                'Response.Redirect("SurveyOverview.aspx?SurveyID=" & e.CommandArgument, False)
                Response.Redirect("userlist.aspx?userID=" & e.CommandArgument, False)
                'ElseIf e.CommandName = "Remove" Then
                '    Dim strSQL As String
                '    Dim db As New DBHelperClient
                '    Dim parms(1) As DBHelperClient.Parameters
                '    'parms(0) = New DBHelperClient.Parameters("userid", Request.QueryString("userID"))
                '    parms(1) = New DBHelperClient.Parameters("userid", e.CommandArgument)
                '    strSQL = " Update tblassignedtest SET isdeleted = '1' ,modifieddts = CURRENT_TIMESTAMP " &
                '       " ,modifiedby = @userid WHERE userID= @userid "
                '    db.ExecuteNonQuery(Data.CommandType.Text, strSQL, parms)
                '    BindSurvey()
            ElseIf e.CommandName = "Del" Then
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms(1) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("userid", Request.QueryString("userID"))
                parms(1) = New DBHelperClient.Parameters("surveyid", e.CommandArgument)
                strSQL = " Update tblassignedtest SET isdeleted = '1' ,modifieddts = CURRENT_TIMESTAMP " &
                   " ,modifiedby = @userid WHERE SurveyID=@surveyid "
                db.ExecuteNonQuery(Data.CommandType.Text, strSQL, parms)
                BindSurvey()

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


    Protected Sub Repeater1_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles Repeater1.ItemCommand
        Try
            If e.CommandName = "Add" Then
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms(1) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("userid", Request.QueryString("userID"))
                parms(1) = New DBHelperClient.Parameters("SurveyID", e.CommandArgument)
                strSQL = " INSERT INTO tblassignedtest (userID,SurveyID,createddts,createdby,modifiedby,modifieddts) VALUES " &
                          " ( @userid, @SurveyID, CURRENT_TIMESTAMP, @userid,@userid,CURRENT_TIMESTAMP) "
                db.ExecuteNonQuery(Data.CommandType.Text, strSQL, parms)
                BindSurvey()
            End If
        Catch ex As Exception
            lblErrorMsg.Visible = True
            lblErrorMsg.Text = ex.StackTrace + "-" + ex.Message
        End Try
    End Sub

End Class
