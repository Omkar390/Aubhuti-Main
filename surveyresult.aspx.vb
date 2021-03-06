﻿Imports System.Data
Partial Class surveyresult
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
        'strSQL = " SELECT SurveyID, SurveyName, ST.surveytype, Description, " &
        '        " DATE_FORMAT(ST.createdts, '%m/%d/%Y') AS createdts, U.UserName AS createdby, " &
        '        " CASE S.Active WHEN '1' THEN 'Yes' ELSE 'No' END AS Active " &
        '        " FROM tblsurvey S " &
        '        " INNER JOIN tblsurveytype ST ON S.SurveyType = ST.surveytypeid " &
        '        " LEFT OUTER JOIN client_master.users U ON S.CreatedBy = U.userID " &
        '        " WHERE S.isdeleted IS NULL "

        'strSQL = "SELECT SurveyID,SurveyName,ST.surveytype, U.userID,U.UserName " &
        '         " FROM tblsurvey S " &
        '         " INNER JOIN tblsurveytype ST ON S.SurveyType = ST.surveytypeid " &
        '        " LEFT OUTER JOIN client_master.users U ON S.CreatedBy = U.userID " &
        '        " WHERE S.isdeleted IS NULL "

        'strSQL = "SELECT U.userID,U.UserName,ts.surveyID,ts.SurveyName " &
        '         " FROM client_master.users U " &
        '         " INNER JOIN client_2002.tblsurvey ts ON  "

        strSQL = "select tst.userID,U.UserName,s.SurveyName,tst.iscompleted from tblassignedtest tst " &
                 " inner join client_master.users U on tst.userID = U.userID " &
                 " inner join tblsurvey s on tst.SurveyID = s.SurveyID "


        rptSurvey.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
        rptSurvey.DataBind()

    End Sub


    Protected Sub rptSurvey_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptSurvey.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                'Response.Redirect("SurveyOverview.aspx?SurveyID=" & e.CommandArgument, False)
                'Response.Redirect("assignlist.aspx?userID=" & e.CommandArgument, False)
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
