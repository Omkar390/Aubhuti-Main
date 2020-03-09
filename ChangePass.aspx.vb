Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Exception
Imports System.Security.Cryptography

Partial Class ChangePass
    Inherits System.Web.UI.Page

    Private strregex As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack = True Then
            Try

                getpasswordrule()

            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub btnSaveSetting_Click(sender As Object, e As EventArgs) Handles btnSaveSetting.Click
        Try
            If txtPassword.Text.Trim <> "" And Regex.IsMatch(txtPassword.Text.Trim,revtxtPassword.ValidationExpression) Then
                Dim db As New DBHelper
                Dim strSQL As String
                strSQL = "Update users set userPassword = '" & txtPassword.Text.Trim & "' where userid = " & Session("user_id")
                db.ExecuteNonQuery(CommandType.Text,strSQL)

                txtPassword.Text = ""
                txtCnfPassword.Text = ""
                lblErrorMsg.Text = "Password updated successfully."
                lblErrorMsg.Visible = True
            Else
                lblErrorMsg.Text = "Invalid Password."
                lblErrorMsg.Visible = True
            End If

        Catch ex As Exception
             lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "getpasswordrule")
        End Try
    End Sub

       Private sub getpasswordrule()
        Try 
            Dim db As New DBHelper
            Dim strSQL As String
            Dim dt As New DataTable
            strSQL = " Select * from tbluserpasswordrule where client_id = " & Session("client_id")
            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
            lblMsg.Text = "<br />"
            lblMsg.Text = "Minimum Password length : " & dt.Rows(0).Item("minpasswdlen").ToString & "<br />"
            lblMsg.Text += "Maximum Password length : " & dt.Rows(0).Item("maxpasswdlen").ToString & "<br />"
            If dt.Rows(0).Item("reqspchar").ToString = "1" Then
                lblMsg.Text += "Include special characters : " & dt.Rows(0).Item("spchars").ToString & "<br />"
            End If

            If dt.Rows(0).Item("maxpasswdlen").ToString <> "" Then
                txtPassword.MaxLength = dt.Rows(0).Item("maxpasswdlen").ToString
            End If
            '?=.*
            'strregex = "^(?:[a-zA-Z0-9" + dt.Rows(0).Item("spchars").ToString + "]){" +  dt.Rows(0).Item("minpasswdlen").ToString + "," + dt.Rows(0).Item("maxpasswdlen").ToString + "}$"
            'strregex = "^(?:[a-zA-Z0-9])(?=.*[" + dt.Rows(0).Item("spchars").ToString + "]){" +  dt.Rows(0).Item("minpasswdlen").ToString + "," + dt.Rows(0).Item("maxpasswdlen").ToString + "}$"
            strregex = "^(?=.*[A-Za-z0-9])(?=.*\d)(?=.*[" + dt.Rows(0).Item("spchars").ToString + "])[A-Za-z\d" + dt.Rows(0).Item("spchars").ToString + "]{" + dt.Rows(0).Item("minpasswdlen").ToString + "," + dt.Rows(0).Item("maxpasswdlen").ToString + "}$"

            revtxtPassword.ValidationExpression = strregex
            revtxtCnfPassword.ValidationExpression = strregex

            Session("canmtchprepasswd") = dt.Rows(0).Item("canmtchprepasswd").ToString
            Session("cancontainfirstname") = dt.Rows(0).Item("cancontainfirstname").ToString
            Session("cancontainlastname") = dt.Rows(0).Item("cancontainlastname").ToString
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "getpasswordrule")
        End Try
    End sub

   
End Class
