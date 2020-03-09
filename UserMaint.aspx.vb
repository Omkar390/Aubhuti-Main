Imports System.Data
Imports System.Net
Imports System.Net.Mail

Partial Class UserMaint
    Inherits System.Web.UI.Page

    Private Sub ClientUserMaint_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("userid") <> 0  Then
                HdrPage.InnerHtml = "Update User"
                hdnuserId.Value = Request.QueryString("userid")
                BindUser(hdnuserId.Value)
                BindClients(hdnuserId.Value)
            Else
                hdnclientIduserId.Value = 0
                HdrPage.InnerHtml = "Create New User"
            End If
        End If
    End Sub

    Protected Sub BindUser(byval userclientid As String)

        Try
            Dim db As New DBHelper
            Dim strSQL As String
            Dim parms(0) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("userID", hdnuserId.Value)
            strSQL = " SELECT userID, fname, lname, U.email, phone_work , userPassword, userGroup,  U.title " & _
                    " FROM users U WHERE userid =  @userID "
            Dim dt As New DataTable
            dt = db.DataAdapter(CommandType.Text,strSQL,parms).Tables(0)
            txtFirstName.Text = dt.Rows(0).Item("fname").ToString
            txtLastName.Text = dt.Rows(0).Item("lname").ToString
            txtEmail.Text = dt.Rows(0).Item("email").ToString
            txtPhone.Value = dt.Rows(0).Item("phone_work").ToString
            hdnPassword.Value = dt.Rows(0).Item("userPassword").ToString
            txtPassword.Attributes("value") = dt.Rows(0).Item("userPassword").ToString
            txtCnfPassword.Attributes("value") = dt.Rows(0).Item("userPassword").ToString
            txtTitle.Value = dt.Rows(0).Item("title").ToString
            ddlRole.Items.FindByValue(dt.Rows(0).Item("userGroup").ToString).Selected = True

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub BindClients(byval userid As String)

        Try
            Dim db As New DBHelper
            Dim strSQL As String
            Dim parms(0) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("userID", hdnuserId.Value)
            strSQL = " SELECT userclientid, UC.client_id, client_name, userGroup, userRole, CASE UC.active WHEN '1' THEN 'Active' ELSE 'InActive' END AS active " & _
                     " FROM tbluserclient UC " & _
                     " INNER JOIN client_master C ON UC.client_id = C.client_id " & _
                     " WHERE userid = @userID and C.Active = 1 "

            Dim dt As New DataTable
            dt = db.DataAdapter(CommandType.Text,strSQL,parms).Tables(0)
            rptClients.DataSource = dt
            rptClients.DataBind()

            strSQL = " Select '0' as client_id, 'Select ' as client_name UNION " & _
                     " SELECT client_id, client_name FROM client_master " & _
                     " WHERE Active = 1 And client_id Not IN ( " & _
                     " SELECT UC.client_id " & _
                     " FROM tbluserclient UC " & _
                     " INNER JOIN client_master C ON UC.client_id = C.client_id " & _
                     " WHERE userid = @userID And UC.active = 1) "
            dt = db.DataAdapter(CommandType.Text, strSQL,parms).Tables(0)
            pClients.DataSource = dt
            pClients.DataBind()
            hdnRemClients.Value = dt.Rows.Count - 1

        Catch ex As Exception

        End Try

    End Sub

    Private Sub lnkSave_Click(sender As Object, e As EventArgs) Handles lnkSave.Click
        Try
            Dim db As New DBHelper
            Dim parms(9) as DBHelper.Parameters
            If hdnuserId.Value = "" Then
                parms(0) = New DBHelper.Parameters("p_dFlag", "I")
                parms(1) = New DBHelper.Parameters("p_userid", "0")
            Else
                parms(0) = New DBHelper.Parameters("p_dFlag", "U")
                parms(1) = New DBHelper.Parameters("p_userid", hdnuserId.Value)
            End If
            parms(2) = New DBHelper.Parameters("p_firstname", txtFirstName.Text)
            parms(3) = New DBHelper.Parameters("p_lastname", txtLastName.Text)
            parms(4) = New DBHelper.Parameters("p_email", txtEmail.Text)
            parms(5) = New DBHelper.Parameters("p_phonework", txtPhone.Value)
            parms(6) = New DBHelper.Parameters("p_password", txtPassword.Text)
            parms(7) = New DBHelper.Parameters("p_title", txtTitle.Value)
            parms(8) = New DBHelper.Parameters("p_userrole", ddlRole.SelectedValue)
            parms(9) = New DBHelper.Parameters("p_updatedby", Session("user_id"))
            If hdnuserId.Value = "" Then
                hdnuserId.Value = db.ExecuteScalar(CommandType.StoredProcedure, "SP_IUUser", parms)
            Else
                db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IUUser", parms)
            End If
            If hdnPassword.Value = txtPassword.Text Then
            Else
                SendEmailToUser(txtEmail.Text, txtPassword.Text)
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub lnkResPwd_Click(sender As Object, e As EventArgs) Handles lnkResPwd.Click
        Try

            Dim strSQL As String
            Dim parms(0) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("userID", hdnuserId.Value)
            strSQL = "Update users set userPassword = 'Welcome1234!' where userID = @userID "
            Dim db As New DBHelper
            db.ExecuteNonQuery(CommandType.Text,strSQL,parms)

            SendEmailToUser(txtEmail.Text, "Welcome1234!")

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub SendEmailToUser(ByVal txtemail As String, ByVal strPwd As String)
        Try
            Dim mailMsg As New MailMessage
            mailMsg.From = New System.Net.Mail.MailAddress("ITSupport@tic-us.com")
            mailMsg.To.Add(New System.Net.Mail.MailAddress(txtemail))
            mailMsg.Bcc.Add(New System.Net.Mail.MailAddress("moak@tic-us.com"))

            mailMsg.Subject = "Your SalesShark password has been reset."
            mailMsg.IsBodyHtml = True
            mailMsg.Headers.Add("X-MC-Track", "opens,clicks_all")
            mailMsg.Headers.Add("X-MC-AutoHtml", "1")

            Dim strBody As String = ""
            strBody = "<div style='font:12px Verdana, Geneva, sans-serif;'>"
            strBody += "Your SalesShark password has been reset as is as below:"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "New Password: " & strPwd
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "Date Reset: " + Date.Now.ToLongDateString + " " + Date.Now.ToLongTimeString
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "<br/>"
            strBody += "Regards,"
            strBody += "<br/>"
            strBody += "SalesShark Support Team."
            strBody += "</div>"
            mailMsg.Body = strBody

            Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
            smtp.Port = "587"
            smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "1")
            smtp.Send(mailMsg)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnSaveClient_ServerClick(sender As Object, e As EventArgs) Handles btnSaveClient.Click
        Try
            Dim db As New DBHelper
            Dim strSQL As String
            Dim parms1(1) As DBHelper.Parameters
            parms1(0) = New DBHelper.Parameters("userID", hdnuserId.Value)
            parms1(1) = New DBHelper.Parameters("clientid", pClients.SelectedValue)
            strSQL = " Delete from tbluserclient where client_id = @clientid and userid = @userID "
            db.ExecuteNonQuery(CommandType.Text, strSQL,parms1)

            Dim parms(4) as DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_userid", hdnuserId.Value)
            parms(1) = New DBHelper.Parameters("p_clientid", pClients.SelectedValue)
            parms(2) = New DBHelper.Parameters("p_active", "1")
            parms(3) = New DBHelper.Parameters("p_userGroup", pddlMktgRole.SelectedValue )
            parms(4) = New DBHelper.Parameters("p_userRole", pddlAccessRole.SelectedValue)
            strSQL = " Insert into tbluserclient (userid, client_id, active, userGroup, userRole) " & _
                    " Values (@p_userid, @p_clientid, @p_active, @p_userGroup, @p_userRole) "
            db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
            BindClients(hdnuserId.Value)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub rptClients_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptClients.ItemCommand
        Try
            Dim db As New DBHelper
            Dim strSQL As String

            If e.CommandName = "Del" Then
                Dim parms1(0) As DBHelper.Parameters
                parms1(0) = New DBHelper.Parameters("userclientid", e.CommandArgument)
                strSQL = " Update tbluserclient set Active = 0 where userclientid = @userclientid "
                db.ExecuteNonQuery(CommandType.Text, strSQL,parms1)
                BindClients(hdnuserId.Value)
            Else
                hdnclientId.Value = TryCast(e.Item.FindControl("hdnclientid"), HiddenField).Value
                pddlAccessRole.ClearSelection()
                pddlAccessRole.Items.FindByValue(TryCast(e.Item.FindControl("hdnuserGroup"), HiddenField).Value).Selected = True
                pddlMktgRole.ClearSelection()
                pddlMktgRole.Items.FindByValue(TryCast(e.Item.FindControl("hdnuserRole"), HiddenField).Value).Selected = True
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "newclient", "newclient(1);", True)
            End If
            
        Catch ex As Exception

        End Try
    End Sub
End Class
