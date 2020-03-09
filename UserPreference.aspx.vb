Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Exception
Imports System.Security.Cryptography

Partial Class UserPreference
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblErrorMsg.Visible = False
        If Not Page.IsPostBack = True Then
            Try
                If Request.UrlReferrer.ToString.Contains("Clients.aspx") Then
                    btnback.HRef = "Clients.aspx"
                Else
                    btnback.HRef = "Admin.aspx"
                End If

                ddlTimeZone.ClearSelection()
                ddlDtFormat.ClearSelection()

                'txtconame.Focus()
                Dim db As New DBHelperClient
                Dim strSQL As String
                Dim dt As New DataTable

                strSQL = "Select MyDateFormat, MyTimeZone  from tbluser where user_id = " & Session("user_id")
                dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
                If dt.Rows.Count > 0 Then
                    if (dt.Rows(0).Item("MyTimeZone").ToString = "") Then
                        ddlTimeZone.SelectedValue = "Select "
                    Else
                        ddlTimeZone.SelectedValue = dt.Rows(0).Item("MyTimeZone").ToString
                    End If
                    
                    if (dt.Rows(0).Item("MyDateFormat").ToString = "") Then
                        ddlDtFormat.SelectedValue = "Select "
                    Else
                        ddlDtFormat.SelectedValue = dt.Rows(0).Item("MyDateFormat").ToString
                    End If                    
                End If
                    
            Catch ex As Exception
                lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
                lblErrorMsg.Visible = True
                SSManager.LogAppError(ex, "Page_Load")
            End Try

        End If
    End Sub


    Private Sub btnSaveSetting_Click(sender As Object, e As EventArgs) Handles btnSaveSetting.Click
        lblErrorMsg.Text=""
        lblErrorMsg.Visible = False
        lblSuccessMsg.Text = ""
        lblSuccessMsg.Visible = False

        Dim db As New DBHelperClient
        Dim strSQL As String = ""

        Try
            Dim parms(1) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_dtFormat", ddlDtFormat.SelectedValue )
            parms(1) = New DBHelperClient.Parameters("p_timeZone", ddlTimeZone.SelectedValue )
            strSQL = "Update tbluser set MyDateFormat = @p_dtFormat, MyTimeZone = @p_timeZone where user_id = " & Session("user_id")
            db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
            lblSuccessMsg.Text = "User Preferences updated."
            lblSuccessMsg.Visible = True
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try

    End Sub



End Class
