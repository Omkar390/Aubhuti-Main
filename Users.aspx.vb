Imports System.Data

Partial Class Users
    Inherits System.Web.UI.Page

    Private Sub ClientUsers_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack
            Session("client_id") = Nothing
            BindClients()
        End If
    End Sub

    Protected Sub BindClients()
        Try
            Dim db As New DBHelper
            Dim strSQL As String
            strSQL = "SELECT U.userID, UserName, fname, lname, U.email, DATE_FORMAT(date_added, '%m/%d/%Y') AS AddedDate " & _
                    " , DATE_FORMAT(date_last_login, '%m/%d/%Y') AS LastLoginDate " & _
                    " , CASE COALESCE(emailvalidated, '1') WHEN 1 THEN 'Yes' ELSE 'No' END AS EmailValidated " & _
                    " , CASE COALESCE(U.Active, '1') WHEN 1 THEN 'Active' ELSE 'InActive' END AS Active " & _
                    " , clientcnt " & _
                    " FROM users U " & _
                    " LEFT OUTER JOIN (SELECT COUNT(*) AS clientcnt, userid FROM tbluserclient where active = 1 GROUP BY userid ) AS  UC ON u.userid = UC.userid " 
 
            Dim dt As New DataTable
            dt = db.DataAdapter(CommandType.Text,strSQL).Tables(0)
            rptClients.DataSource=dt
            rptClients.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Private Sub rptClients_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptClients.ItemCommand
        Try
            If e.CommandName="Edit" Then
                Response.Redirect("UserMaint.aspx?userid=" & e.CommandArgument , False)
            ElseIf e.CommandName = "Del" Then
                Dim strSQL As String
                Dim db As New DBHelper
                Dim parms(0) As DBHelper.Parameters
                parms(0) = New DBHelper.Parameters("userID", e.CommandArgument )
                strSQL = "Update users set Active = 0 where userID = @userID "
                db.ExecuteNonQuery(CommandType.Text,strSQL,parms)
                BindClients()
            End If

        Catch ex As Exception

        End Try
    End Sub
End Class
