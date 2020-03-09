Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Exception

Partial Class MasterPageAdmin
    Inherits System.Web.UI.MasterPage
    Protected BindActivity As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("user_id") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        litFName.Text = Session("user_name")
        litDateTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm")
        pEmail.Text = Session("email")
        pAddInf.Text = Request.Url.ToString
        'If Not Page.IsPostBack Then
        '    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "showfirsttime", "fnLoadTable();", True)
        'End If

	End Sub


    Protected Sub UpdateTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
        litDateTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm")
    End Sub

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click
        Try
            Dim iTrackerNo As Integer = SaveContactUsInfo()

            Dim mailMsg As New MailMessage
            mailMsg.From = New System.Net.Mail.MailAddress("ITSupport@tic-us.com")
            mailMsg.To.Add(New System.Net.Mail.MailAddress("akarve@tic-us.com"))

            mailMsg.Subject = "A new issue has been logged."
            mailMsg.IsBodyHtml = True
            mailMsg.Headers.Add("X-MC-Track", "opens,clicks_all")
            mailMsg.Headers.Add("X-MC-AutoHtml", "1")

            Dim strBody As String = ""
            strBody = "<div style='font:12px Verdana, Geneva, sans-serif;'>"
            strBody += "Thank you for bringing this to our attention.   Here is the information you entered:"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "Comment: " + pComments.Value
            strBody += "<br/>"
            strBody += "<br/>"
            'strBody += "Page: " + Request.Url.ToString.Substring(Request.Url.ToString.LastIndexOf("/") + 1, Request.Url.ToString.LastIndexOf(".aspx") - Request.Url.ToString.LastIndexOf("/") + 4)
            strBody += "URL: " + pAddInf.Text
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "User Name:" + Session("UserFirstName")
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "Date Entered: " + Date.Now.ToLongDateString + " " + Date.Now.ToLongTimeString
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "Tracker Number: " + iTrackerNo.ToString
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "We will get back to you shortly with our analysis."
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "Regards,"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "Technology Insight Support Team."
            strBody += "</div>"
            mailMsg.Body = strBody

            Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
            smtp.Port = "587"
            smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "1")

            'Dim token As Object = Nothing
            'smtp.Send(mailMsg,token)
            smtp.Send(mailMsg)

        Catch ex As Exception

        End Try
    End Sub

    Protected Function SaveContactUsInfo() As Integer
        Try
            Dim strSQL As String
            strSQL = "Insert into tblcontactus (clientid, webpage, message, createdby, createdts ) " & _
                    " Values (@p_clientid, @p_webpage, @p_message, @p_user, CURRENT_TIMESTAMP) ; " & _
                    " Select Last_Insert_ID(); "
            Dim db As New DBHelper
            Dim parms(3) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_clientid", Session("client_id") )
            parms(1) = New DBHelper.Parameters("p_webpage", pAddInf.Text)
            parms(2) = New DBHelper.Parameters("p_message", pComments.Value)
            parms(3) = New DBHelper.Parameters("p_user", Session("user_id"))
            Return db.ExecuteScalar(CommandType.Text, strSQL, parms)
        Catch ex As Exception
        End Try
    End Function

End Class

'1394000101016903

'PUNB0139400
