Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Runtime.Serialization.Json
Imports Newtonsoft.Json

Partial Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not (Request.Cookies("UserName")) Is Nothing Then
                Login1.UserName = Request.Cookies("UserName").Value
                'squaredFour.Checked = True
            End If
        End If
    End Sub


    Protected Sub _login_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate
        Try

            'Stpes performed for user login
            'First clear the old session!
            Session.Clear()
            Dim ipaddr As String = ""
            Dim Client_ID As Integer = 0
            Dim ds As New DataSet()
            Login1.UserName = Login1.UserName.Trim
            ds = SSManager.Validate(Login1.UserName, Login1.Password, ipaddr)
            If (ds.Tables(0).Rows.Count > 0) Then
                'redirect to client selection screen
                e.Authenticated = True
                Session("user_id") = ds.Tables(0).Rows(0)("userid")
                Session("email") = ds.Tables(0).Rows(0)("email")
                Session("FirstName") = ds.Tables(0).Rows(0)("fname")
                Session("LastName") = ds.Tables(0).Rows(0)("lname")
                FormsAuthentication.SetAuthCookie(Login1.UserName, True)

                'SSManager.LogVisit(ipaddr, Request.UserAgent, Request.Browser.Browser + " " + Request.Browser.MajorVersion.ToString, ClientID, ds.Tables(1).Rows(0)(0))


                Response.Cookies("UserName").Value = Login1.UserName.Trim
                If ds.Tables(0).Rows(0)("userGroup").ToString = "Admin" Then
                    Session("user_group") = "Admin"
                    Response.Redirect("UserAccess.aspx", False)
				ElseIf ds.Tables(0).Rows(0)("userGroup").ToString = "EmailCheck" Then
                    Dim db As new DBHelper
                    Dim dt As New DataTable
                    Dim strSQL As String = "Select CM.client_name, CM.client_id, SUBSTRING(database_name,8,4) AS dbname, '2' as ListOrder , 'SalesShark.aspx' as hrefPage, CM.GUID as CGUID, UC.userRole" & _
                            " from tbluserclient UC Inner Join client_master CM on UC.client_id = CM.client_id " & _
                            " where CM.Active = 1 and UC.Active = 1 and userid = " & Session("user_id") & " Order by ListOrder, client_name "
                    dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
                    Session("user_group") = "EmailCheck"
                    Session("client_id") = dt.Rows(0)("client_id")
                    Session("dbname") = dt.Rows(0)("dbname")
                    Session("CGUID") = dt.Rows(0)("CGUID")
                    Session("userRole") = dt.Rows(0)("userRole")
                    Response.Redirect("EmailCheck.aspx", False)
				Else
                    Dim db As New DBHelper
                    Dim dt As New DataTable
                    Dim strSQL As String
                    strSQL = "Select CM.* from tbluserclient UC Inner Join client_master CM on UC.client_id = CM.client_id " & _
                            " where userid = " & Session("user_id")
                    dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
                    If dt.Rows.Count > 0 Then
                        Response.Redirect("UserAccess.aspx", True)
                    Else
                        Response.Redirect("AdminMenu.aspx", True)
                    End If
                End If
            End If
            'lblError.Visible = True
            litErrorMsg.Visible = True
            litErrorMsg.Text = "Login failed. Invalid login and/or password."
            e.Authenticated = False
        Catch ex As Exception
            litErrorMsg.Text = "Login failed. Invalid login and/or password."
            e.Authenticated = False
        End Try
    End Sub




End Class

