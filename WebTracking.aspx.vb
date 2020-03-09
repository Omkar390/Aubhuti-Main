Imports System.Data

Partial Class WebTracking
    Inherits System.Web.UI.Page

    Private Sub WebTracking_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try 
            If Not Page.IsPostBack = True Then
				SSManager.CheckAccess()

                'get the client GUID
                Dim strGUID As String = ""
                Dim db As New DBHelper
                strGUID = db.ExecuteScalar(CommandType.Text, "select GUID from client_master where client_id = " & Session("client_id"))

                txtScript.Text = "(function() { <br />  " & _
                        "&nbsp &nbsp &nbsp &nbsp &nbsp var ga = document.createElement('script');  <br />  " & _
                        "&nbsp &nbsp &nbsp &nbsp &nbsp ga.type = 'text/javascript';  <br />  " & _
                        "&nbsp &nbsp &nbsp &nbsp &nbsp ga.async = true;  <br />  " & _
                        "&nbsp &nbsp &nbsp &nbsp &nbsp ga.src = 'http://salesshark.us/sstracking.js?cid=" & strGUID & "';  <br />  " & _
                        "&nbsp &nbsp &nbsp &nbsp &nbsp var s = document.getElementsByTagName('script')[0];  <br />  " & _
                        "&nbsp &nbsp &nbsp &nbsp &nbsp s.parentNode.insertBefore(ga, s);  <br />  " & _
                        "})();  <br />  " 
            End If
        Catch ex As Exception
            Response.Write(ex.Message.ToString())
        End Try

    End Sub
End Class
