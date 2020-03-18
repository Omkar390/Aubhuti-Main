Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Exception
Imports System.Security.Cryptography

Partial Class MasterPage
    Inherits System.Web.UI.MasterPage
    Protected BindActivity As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("user_id") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If
		
		If  Session("user_group") = "EmailCheck" Then
            Response.Redirect("Login.aspx")
        End If

        'litFName.Text = Session("user_name")
        'pEmail.Text = Session("email")
        'pAddInf.Text = Request.Url.ToString

        'Dim db As New DBHelper
        'Dim strSQL As String
        'strSQL = "SELECT client_name FROM client_master " & _
        '        " WHERE client_id = " & Session("client_id")

        'lblClientName.text = "Abubhuti"
        'litDateTime.Text = System.DateTime.Now.ToString("MM-dd-yyyy hh:mm")

        'If Session("client_id") = "999" Then
        '    lblClientName.text = "<b>" &  "Admin" & "&nbsp&nbsp&nbsp&nbsp</b>"
        'Else
        '    lblClientName.text = "<b>" &  db.DataAdapter(CommandType.Text, strSQL).Tables(0).rows(0).item("client_name").ToString & "&nbsp&nbsp&nbsp&nbsp</b>"

        'End If

        'log the pagename in DB!
        'LogPageName()

        If Not Page.IsPostBack Then
            'BindLists()
            'SSManager.IPageNav(Session("client_id"), Session("user_id"), Request.Url.ToString, Request.Browser.Browser)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "showfirsttime", "fnLoadTable();", True)
        End If

	End Sub

    Protected Sub btnSrch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSrch.Click
        Dim EncStr As String =  EDManager.Encrypt("Srch=" & txtsrch.Value)
        Response.Redirect("SearchResults.aspx?" & EncStr )
    End Sub

    Protected Sub UpdateTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
        litDateTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm")
    End Sub

    Protected Sub BindLists()
        Dim dtuac As New DataTable
        Dim dt As New DataTable

        If Not Session("client_id").ToString.Equals("999") Then
            Dim db As New DBHelperClient
            Dim strSQL As String
            strSQL = "SELECT LM.ListId , listtype, listname, LM.Id " & _
                    " FROM tblulistac LUAC " & _
                    " INNER JOIN tblListMaster LM ON LUAC.ListId = LM.ListId " & _
                    " WHERE Urowid = " & Session("user_id") & " LIMIT 1 "
            dtuac = db.DataAdapter(CommandType.Text, strSQL).Tables(0)

            If dtuac.Rows.Count > 0 Then
                If dtuac.Rows(0).Item("listtype").ToString = "Account" Then
                    btnSeeAll.HRef = "MyListAcctCt.aspx?ListType=Account&Id=" & dtuac.Rows(0).Item("Id").ToString
                    dt = SSManager.GetAllListAC("ACCU", dtuac.Rows(0).Item("ListId").ToString).Tables(0)
                Else
                    btnSeeAll.HRef = "MyListAcctCt.aspx?ListType=Contact&Id=" & dtuac.Rows(0).Item("Id").ToString
                    dt = SSManager.GetAllListAC("CONU", dtuac.Rows(0).Item("ListId").ToString).Tables(0)
                End If

                For i = 0 To dt.Rows.Count - 1
                    If Request.QueryString("BindType") = "A" Then
                        If dt.Rows(i).Item("AcctId") = Request.QueryString("AcctId") Then
                            SSManager.IUDUListAC("D", dt.Rows(i).Item("ListId"), dt.Rows(i).Item("t_account_id"), 0)
                            dt.Rows(i).Delete()
                            dt.AcceptChanges()
                            Exit For
                        End If
                    End If
                    If Request.QueryString("BindType") = "C" Then
                        If dt.Rows(i).Item("CtId").ToString = Request.QueryString("CtId").ToString Then
                            SSManager.IUDUListAC("D", dt.Rows(i).Item("ListId"), dt.Rows(i).Item("t_account_id"), dt.Rows(i).Item("t_contact_id"))
                            dt.Rows(i).Delete()
                            dt.AcceptChanges()
                            Exit For
                        End If
                    End If
                Next

                If dt.Rows.Count > 0 Then
                    litHotListName.Text = dtuac.Rows(0).Item("listname").ToString
                    rptHotList.DataSource = dt
                    rptHotList.DataBind()
                    divSeeAll.Style.Item("display") = "block"
                Else
                    litHotListName.Text = ""
                    rptHotList.DataSource = Nothing
                    rptHotList.DataBind()
                    divSeeAll.Style.Item("display") = "none"
                End If

            Else
                litHotListName.Text = ""
                rptHotList.DataSource = Nothing
                rptHotList.DataBind()
                divSeeAll.Style.Item("display") = "none"
            End If
        End If


    End Sub

    Protected Sub saveattach(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim postedfilename As Object
            Dim postedfilecontenttype As Object
            Dim postedfilelength As Integer

            If FileUpload1.PostedFile.ContentLength = 0 Then
                postedfilename = ""
            Else
                postedfilename = FileUpload1.PostedFile.FileName
                postedfilecontenttype = FileUpload1.PostedFile.ContentType
                postedfilelength = FileUpload1.PostedFile.ContentLength
            End If

            Dim iTrackerNo As Integer = SaveContactUsInfo(postedfilename)
            If FileUpload1.PostedFile.ContentLength <> 0 Then
                FileUpload1.PostedFile.SaveAs(ConfigurationManager.AppSettings("contactuspath") & Session("client_id") & "/" & iTrackerNo & "_" & postedfilename)
            End If

            Dim mailMsg As New MailMessage
            mailMsg.From = New System.Net.Mail.MailAddress("ITSupport@tic-us.com")
            mailMsg.To.Add(New System.Net.Mail.MailAddress("akarve@tic-us.com"))
            mailMsg.Cc.Add(New System.Net.Mail.MailAddress("moak@tic-us.com"))

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

            Dim bytes1 As Byte()
            If FileUpload1.FileName <> "" Then
                Dim br As BinaryReader
                br = New BinaryReader(FileUpload1.PostedFile.InputStream)
                bytes1 = br.ReadBytes(Convert.ToInt32(FileUpload1.PostedFile.InputStream.Length))
                br.Close()
                Dim memStream As New MemoryStream(bytes1)
                mailMsg.Attachments.Add(New System.Net.Mail.Attachment(memStream, FileUpload1.FileName.ToString))
            End If

            'Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
            'smtp.Port = "587"
            'smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "1")

            'Dim token As Object = Nothing
            'smtp.Send(mailMsg,token)
            ' smtp.Send(mailMsg)
            pComments.Value = ""

        Catch ex As Exception
             SSManager.LogAppError(ex, "saveattach")
        End Try
    End Sub

    Protected Function SaveContactUsInfo(byval filename As String) As Integer
        Try
            Dim strSQL As String
            strSQL = "Insert into tblcontactus (clientid, webpage, message, filename, priority, createdby, createdts ) " & _
                    " Values (@p_clientid, @p_webpage, @p_message, @p_filename, @p_priority, @p_user, CURRENT_TIMESTAMP) ; " & _
                    " Select Last_Insert_ID(); "
            Dim db As New DBHelperClient
            Dim parms(5) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_clientid", Session("client_id") )
            parms(1) = New DBHelperClient.Parameters("p_webpage", pAddInf.Text)
            parms(2) = New DBHelperClient.Parameters("p_message", pComments.Value)
            parms(3) = New DBHelperClient.Parameters("p_filename", filename)
            parms(4) = New DBHelperClient.Parameters("p_priority", "High")
            parms(5) = New DBHelperClient.Parameters("p_user", Session("user_id"))
            Return db.ExecuteScalar(CommandType.Text, strSQL, parms)
        Catch ex As Exception
        End Try
    End Function

    Private Sub rptHotList_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptHotList.ItemCommand
        Try
            If e.CommandName = "GoTo" Then
                If TryCast(e.Item.FindControl("hdnCtId"), HiddenField).Value = "0" Then
                    Response.Redirect("Account.aspx?BindType=A&AcctId=" & TryCast(e.Item.FindControl("hdnAcctId"), HiddenField).Value , False)
                Else
                    Response.Redirect("Activities.aspx?BindType=C&AcctId=" & TryCast(e.Item.FindControl("hdnAcctId"), HiddenField).Value & _
                                        "&CtId=" & TryCast(e.Item.FindControl("hdnCtId"), HiddenField).Value , False)
                End If
            End If

        Catch ex As Exception

        End Try

    End Sub

	Public Sub LogPageName()
        Try
            Dim s As String = left(Request.Url.ToString.ToLower, Request.Url.ToString.ToLower.LastIndexOf("aspx")+ 4)
            Dim currentPage As String = ""
            If s.ToLower.Contains("reportdetail.aspx") Then
                If Not s.Contains("&") Then
                    s = s + "&"
                End If
                If s.ToString.ToLower.Contains("rptname=") Then
                    currentPage = s.Substring(s.ToLower.IndexOf("rptname=") + 8, s.ToLower.IndexOf("&") - s.ToLower.IndexOf("rptname=") - 8)
                Else
                    currentPage = s.Substring(s.ToLower.IndexOf("rptid=") + 6, s.ToLower.IndexOf("&") - s.ToLower.IndexOf("rptid=") - 6)
                End If
                If currentPage.ToLower.Contains(".aspx") Then
                    currentPage = currentPage.Replace(".aspx", "").Replace(".ASPX", "")
                End If
            Else
                currentPage = s.Substring(s.ToLower.LastIndexOf("/") + 1, s.ToLower.LastIndexOf("aspx") - s.ToLower.LastIndexOf("/") - 2)
            End If
            Dim db As New DBHelper
            Dim parms(3) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_i_pagename", currentPage)
            parms(1) = New DBHelper.Parameters("p_i_PAMLoginDetailsId", Session("user_id"))
            parms(2) = New DBHelper.Parameters("p_i_username", Session("user_id"))
            parms(3) = New DBHelper.Parameters("p_i_linknamefull", s)
            db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_AddPageAccess", parms)
        Catch e As Exception
            SSManager.LogAppError(e, "Page_Load")
        End Try
    End Sub
	
End Class

'1394000101016903

'PUNB0139400
