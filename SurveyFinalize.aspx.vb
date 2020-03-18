
Partial Class SurveyFinalize
    Inherits System.Web.UI.Page
    Private Sub SurveyFinalize_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack = True Then
                hdnSurveyID.value = Request.QueryString("SurveyID")
            End If
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btntab1_ServerClick(sender As Object, e As EventArgs) Handles btntab1.ServerClick
         Response.Redirect("SurveyOverview.aspx?SurveyID=" & hdnSurveyID.Value)
    End Sub

    Private Sub btntab2_ServerClick(sender As Object, e As EventArgs) Handles btntab2.ServerClick
         Response.Redirect("SurveyDesign.aspx?SurveyID=" & hdnSurveyID.Value)
    End Sub

    Private Sub btntab3_ServerClick(sender As Object, e As EventArgs) Handles btntab3.ServerClick
         Response.Redirect("SurveyQuestion.aspx?SurveyID=" & hdnSurveyID.Value)
    End Sub

    Private Sub btnPreview_ServerClick(sender As Object, e As EventArgs) Handles btnPreview.ServerClick
        Dim url As String
        url = "http://localhost:54464/Surveypage.aspx?PMODE=P&CGUID=" & Session("CGUID") & "&SGUID=" & hdnSurveyID.Value
        Dim s As String = "window.open('" & url + "', '_blank', 'width=screen.width,height=screen.height,fullscreen=yes');"
        ClientScript.RegisterStartupScript(Me.GetType(), "OPEN_WINDOW", s, True)
    End Sub
End Class
