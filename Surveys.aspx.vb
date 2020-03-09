Imports System.Data

Partial Class Surveys
    Inherits System.Web.UI.Page
    Public mstr As String = "ctl00$ctl00$ContentPlaceHolder1$"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try

            SSManager.CheckQS()
            If Not Request.QueryString("AcctId") Is Nothing Then
                TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value = SSManager.GetAccountId(Request.QueryString("AcctId"))
                TryCast(Me.Master.FindControl(mstr & "hdnguidaccountId"), HiddenField).Value = Request.QueryString("AcctId")
            End If

            If Not Request.QueryString("CtId") Is Nothing Then
                TryCast(Me.Master.FindControl(mstr & "hdncontactId"), HiddenField).Value = SSManager.GetContactId(Request.QueryString("CtId"))
                TryCast(Me.Master.FindControl(mstr & "hdnguidcontactId"), HiddenField).Value = Request.QueryString("CtId")
            End If

            Dim dt As DataTable
            Dim db As New DBHelperClient
            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_account_id", TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value)
            dt = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveys", parms).Tables(0)
            rptSurvey.DataSource = dt
            rptSurvey.DataBind()

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

    Protected Sub rptSurvey_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptSurvey.ItemDataBound
        Try
            If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
                Dim dt As DataTable
                Dim db As New DBHelperClient
                Dim parms(0) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("p_surveyresultheaderid", _
                                        TryCast(e.Item.FindControl("hdnsurveyresultheaderid"), HiddenField).Value)
                dt = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSurveyDetail", parms).Tables(0)
                TryCast(e.Item.FindControl("rptSurveyDetail"), Repeater).DataSource = dt
                TryCast(e.Item.FindControl("rptSurveyDetail"), Repeater).DataBind()

            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptSurvey_ItemDataBound")
        End Try
    End Sub

End Class
