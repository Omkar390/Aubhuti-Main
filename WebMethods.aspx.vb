Imports System.Data
Imports System.Net
Imports System.Net.Mail
Partial Class WebMethods
    Inherits System.Web.UI.Page

    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveAccount(ByVal t_account_id As String, ByVal name As String, ByVal physicalstreet As String, _
        ByVal physicalcity As String, ByVal physicalstate As String, ByVal physicalpostalcode As String, _
        ByVal physicalcountry As String, ByVal phone As String, ByVal fax As String, ByVal website As String, _
        ByVal sic As String, ByVal industry As String, ByVal annualrevenue As String, ByVal actualinvoice As String, _
        ByVal numberofemployees As String, ByVal ownership As String, ByVal tickersymbol As String, _
        ByVal description As String, ByVal tier As String, ByVal invoicerange As String, ByVal valdate As String, _
        ByVal ownerid As String, ByVal microregion As String, ByVal accountstatus As String) As Int32

        Try
            Dim retval As Integer = 0

            Dim db As New DBHelperClient
            Dim parms(25) As DBHelperClient.Parameters
            If t_account_id = 0 Then
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            Else
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "U")
            End If
            parms(1) = New DBHelperClient.Parameters("p_t_account_id", t_account_id)
            parms(2) = New DBHelperClient.Parameters("p_name", name)
            parms(3) = New DBHelperClient.Parameters("p_physicalstreet", physicalstreet)
            parms(4) = New DBHelperClient.Parameters("p_physicalcity", physicalcity)
            parms(5) = New DBHelperClient.Parameters("p_physicalstate", physicalstate)
            parms(6) = New DBHelperClient.Parameters("p_physicalpostalcode", physicalpostalcode)
            parms(7) = New DBHelperClient.Parameters("p_physicalcountry", IIf(physicalcountry = "Select ", System.DBNull.Value, physicalcountry))
            parms(8) = New DBHelperClient.Parameters("p_phone", phone)
            parms(9) = New DBHelperClient.Parameters("p_fax", fax)
            parms(10) = New DBHelperClient.Parameters("p_website", website)
            parms(11) = New DBHelperClient.Parameters("p_sic", "")
            parms(12) = New DBHelperClient.Parameters("p_industry", industry)
            parms(13) = New DBHelperClient.Parameters("p_annualrevenue", IIf((annualrevenue.Replace(",", "") = ""), System.DBNull.Value, annualrevenue.Replace(",", "")))
            parms(14) = New DBHelperClient.Parameters("p_actual_invoice", actualinvoice.Replace(",", ""))
            parms(15) = New DBHelperClient.Parameters("p_numberofemployees", IIf((numberofemployees.Replace(",", "") = ""), System.DBNull.Value, numberofemployees.Replace(",", "")))
            parms(16) = New DBHelperClient.Parameters("p_ownership", IIf(ownership = "Select ", System.DBNull.Value, ownership))
            parms(17) = New DBHelperClient.Parameters("p_tickersymbol", tickersymbol)
            parms(18) = New DBHelperClient.Parameters("p_description", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(description.Replace("%0A%20", "%0A"))))
            parms(19) = New DBHelperClient.Parameters("p_tier", IIf(tier = "Select ", System.DBNull.Value, tier))
            parms(20) = New DBHelperClient.Parameters("p_invoice_range", IIf(invoicerange = "Select ", System.DBNull.Value, invoicerange))
            If valdate = "" Then
                parms(21) = New DBHelperClient.Parameters("p_ValDate", System.DBNull.Value)
            Else
                parms(21) = New DBHelperClient.Parameters("p_ValDate", Convert.ToDateTime(valdate).ToString("yyyy-MM-dd"))
            End If
            parms(22) = New DBHelperClient.Parameters("p_t_ownerid", IIf(ownerid = "Select ", System.DBNull.Value, ownerid))
            parms(23) = New DBHelperClient.Parameters("p_MicroRegion", IIf(MicroRegion = "Select ", System.DBNull.Value, MicroRegion))
            parms(24) = New DBHelperClient.Parameters("p_account_status", IIf(accountstatus = "Select ", System.DBNull.Value, accountstatus))
            parms(25) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))

            If t_account_id = 0 Then
                HttpContext.Current.Session("t_account_id") = db.ExecuteScalar(CommandType.StoredProcedure, "SP_IUDAccount", parms)
                Return HttpContext.Current.Session("t_account_id")
            Else
                db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDAccount", parms)
                Return t_account_id
            End If
        Catch ex As Exception

        End Try



    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function SaveContact(ByVal t_contact_id As String, ByVal t_account_id As String, ByVal salutation As String, _
    ByVal firstname As String, ByVal lastname As String, ByVal title As String, ByVal level As String, _
    ByVal physicalstreet As String, ByVal physicalcity As String, ByVal physicalstate As String, _
    ByVal physicalpostalcode As String, ByVal physicalcountry As String, ByVal phone As String, ByVal extn As String, ByVal fax As String, _
    ByVal cellphone As String, ByVal pbx As String, ByVal email As String, ByVal linkedin As String, _
    ByVal area As String, ByVal status As String, ByVal persona As String, ByVal description As String, ByVal emailstatusid As String) As Int32

        Try
            Dim retval As Integer = 0

            Dim db As New DBHelperClient
            Dim parms(25) As DBHelperClient.Parameters
            If t_contact_id = 0 Then
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            Else
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "U")
            End If
            parms(1) = New DBHelperClient.Parameters("p_t_contact_id", t_contact_id)
            parms(2) = New DBHelperClient.Parameters("p_t_account_id", t_account_id)
            parms(3) = New DBHelperClient.Parameters("p_salutation", IIf(salutation = "Select ", System.DBNull.Value, salutation))
            parms(4) = New DBHelperClient.Parameters("p_firstname", firstname)
            parms(5) = New DBHelperClient.Parameters("p_lastname", lastname)
            parms(6) = New DBHelperClient.Parameters("p_title", title)
            parms(7) = New DBHelperClient.Parameters("p_level", IIf(level = "Select ", System.DBNull.Value, level))
            parms(8) = New DBHelperClient.Parameters("p_physicalstreet", physicalstreet)
            parms(9) = New DBHelperClient.Parameters("p_physicalcity", physicalcity)
            parms(10) = New DBHelperClient.Parameters("p_physicalstate", physicalstate)
            parms(11) = New DBHelperClient.Parameters("p_physicalpostalcode", physicalpostalcode)
            parms(12) = New DBHelperClient.Parameters("p_physicalcountry", IIf(physicalcountry = "Select ", System.DBNull.Value, physicalcountry))
            parms(13) = New DBHelperClient.Parameters("p_phone", phone)
            parms(14) = New DBHelperClient.Parameters("p_extn", extn)
            parms(15) = New DBHelperClient.Parameters("p_fax", fax)
            parms(16) = New DBHelperClient.Parameters("p_cellphone", cellphone)
            parms(17) = New DBHelperClient.Parameters("p_pbx", pbx)
            parms(18) = New DBHelperClient.Parameters("p_email", email)
            parms(19) = New DBHelperClient.Parameters("p_linkedin_url", linkedin)
            parms(20) = New DBHelperClient.Parameters("p_area", IIf(area = "Select ", System.DBNull.Value, area))
            parms(21) = New DBHelperClient.Parameters("p_status", IIf(status = "Select ", System.DBNull.Value, status))
            parms(22) = New DBHelperClient.Parameters("p_persona", IIf(status = "Select ", System.DBNull.Value, persona))
            parms(23) = New DBHelperClient.Parameters("p_description", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(description.Replace("%0A%20", "%0A"))))
            parms(24) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
            parms(25) = New DBHelperClient.Parameters("p_emailstatusid", emailstatusid)
            
            retval = db.ExecuteScalar(CommandType.StoredProcedure, "SP_IUDContact", parms)
            If t_contact_id = 0 Then
                Return retval
            Else
                Return t_contact_id
            End If
        Catch ex As Exception

        End Try

    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function SaveOpportunity(ByVal OppId As String, ByVal OppName As String, ByVal OppStage As String, _
        ByVal OppAmount As String, ByVal OppProbability As String, ByVal OppCloseDate As String, ByVal OppType As String, _
        ByVal OppNextStep As String, ByVal OppLeadSource As String, ByVal OppIsClosed As String, ByVal OppIsWon As String, _
        ByVal OppIsLost As String, ByVal OppIsOpen As String, ByVal OppDescription As String, ByVal AccountId As String) As Int32

        HttpContext.Current.Session("BindType") = "A"
        Dim db As New DBHelperClient

        Dim retval As Integer = 0
        Dim dFlag As String
        Try
            If CInt(OppId) = 0 Then
                dFlag = "I"
            Else
                dFlag = "U"
            End If

            Dim parms(16) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_dFlag", dFlag)
            parms(1) = New DBHelperClient.Parameters("p_t_opportunity_id", OppId)
            parms(2) = New DBHelperClient.Parameters("p_t_account_id", AccountId)
            parms(3) = New DBHelperClient.Parameters("p_name", OppName)
            parms(4) = New DBHelperClient.Parameters("p_description", OppDescription)
            parms(5) = New DBHelperClient.Parameters("p_stagename", IIf(OppStage = "Select ", System.DBNull.Value, OppStage))
            parms(6) = New DBHelperClient.Parameters("p_amount", IIf(OppAmount = "", System.DBNull.Value, Num(OppAmount)))
            parms(7) = New DBHelperClient.Parameters("p_probability", IIf(OppProbability = "", System.DBNull.Value, OppProbability))
            If OppCloseDate.Trim = "" Then
                parms(8) = New DBHelperClient.Parameters("p_closedate", System.DBNull.Value)
            Else
                parms(8) = New DBHelperClient.Parameters("p_closedate", Convert.ToDateTime(OppCloseDate).ToString("yyyy-MM-dd"))
            End If
            parms(9) = New DBHelperClient.Parameters("p_type", IIf(OppType = "Select ", System.DBNull.Value, OppType))
            parms(10) = New DBHelperClient.Parameters("p_nextstep", OppNextStep)
            parms(11) = New DBHelperClient.Parameters("p_leadsource", IIf(OppLeadSource = "Select ", System.DBNull.Value, OppLeadSource))
            parms(12) = New DBHelperClient.Parameters("p_isclosed", IIf(OppIsClosed = "true", 1, 0))
            parms(13) = New DBHelperClient.Parameters("p_iswon", IIf(OppIsWon = "true", 1, 0))
            parms(14) = New DBHelperClient.Parameters("p_islost", IIf(OppIsLost = "true", 1, 0))
            parms(15) = New DBHelperClient.Parameters("p_isopen", IIf(OppIsOpen = "true", 1, 0))
            parms(16) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
            db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDOpportunity", parms)

            retval = 1
        Catch ex As Exception
            Return 0

        End Try
        Return retval

    End Function
    Private Shared Function Num(ByVal value As String) As Integer
    Dim returnVal As String = String.Empty
    Dim collection As MatchCollection = Regex.Matches(value, "\d+")
    For Each m As Match In collection
        returnVal += m.ToString()
    Next
    Return Convert.ToInt32(returnVal)
End Function
    'Public Shared Function FetchMasters(ByVal prefixText As String) As String()
    <System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()> _
    Public Shared Function FetchMasters(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()

        Dim items As New List(Of String)(20)
        Dim i As Integer = 1

        Dim db As New DBHelper
        Dim ds As New DataSet
        Dim parms(0) As DBHelper.Parameters
        parms(0) = New DBHelper.Parameters("prefixText", "%" + prefixText.ToString + "%")

        If contextKey = "System" Then
            ds = db.DataAdapter(CommandType.Text, "SELECT sysdescription as text, sysid as value FROM tblsystemmstr Where sysdescription like @prefixText ",parms)
        ElseIf contextKey = "Profit Recovery Firm" Then
            ds = db.DataAdapter(CommandType.Text, "SELECT prdescription as text, prid as value FROM tblprofitrecoverymstr Where prdescription like @prefixText ",parms)
        End If
        Try
            For Each dr As DataRow In ds.Tables(0).Rows
                items.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(Trim(dr("text").ToString()), dr("value").ToString()))
                i += 1
                If i = 21 Then
                    Exit For
                End If
            Next
            '9822250613 --- 24202251 Anand Vaidya

        Catch ex As Exception

        End Try
        Return items.ToArray()
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function SaveActivity(ByVal ActId As Integer, ByVal ActDate As String, ByVal ActTime As String, ByVal AssignedTo As String, _
                ByVal ActType As String, ByVal ActPriority As String, ByVal ActStatus As String, ByVal RemDate As String, _
                ByVal Details As String, ByVal AccountId As String, ByVal ContactId As String) As Int32
        Try

            Dim db As New DBHelperClient
            Dim parms(13) As DBHelperClient.Parameters
            If ActId = 0 Then
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            Else
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "U")
            End If
            parms(1) = New DBHelperClient.Parameters("p_activity_id", ActId)
            parms(2) = New DBHelperClient.Parameters("p_t_account_id", AccountId)
            parms(3) = New DBHelperClient.Parameters("p_t_contact_id", ContactId)
            If ActDate = "" Then
                parms(4) = New DBHelperClient.Parameters("p_activity_date", System.DBNull.Value)
            Else
                parms(4) = New DBHelperClient.Parameters("p_activity_date", Convert.ToDateTime(ActDate).ToString("yyyy-MM-dd"))
            End If
            parms(5) = New DBHelperClient.Parameters("p_activity_time", ActTime)
            parms(6) = New DBHelperClient.Parameters("p_assigned_to", IIf(AssignedTo = "Select ", System.DBNull.Value, AssignedTo))
            parms(7) = New DBHelperClient.Parameters("p_activity_type", IIf(ActType = "Select ", System.DBNull.Value, ActType))
            parms(8) = New DBHelperClient.Parameters("p_priority", IIf(ActPriority = "Select ", System.DBNull.Value, ActPriority))
            parms(9) = New DBHelperClient.Parameters("p_activity_status", IIf(ActStatus = "Select ", System.DBNull.Value, ActStatus))
            If RemDate = "" Then
                parms(10) = New DBHelperClient.Parameters("p_reminder_date", System.DBNull.Value)
            Else
                parms(10) = New DBHelperClient.Parameters("p_reminder_date", Convert.ToDateTime(RemDate).ToString("yyyy-MM-dd"))
            End If
            parms(11) = New DBHelperClient.Parameters("p_activity_detail", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(Details.Replace("%0A%20", "%0A"))))
            parms(12) = New DBHelperClient.Parameters("p_inventory_id", System.DBNull.Value)
            parms(13) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
            db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDActivity", parms)

            'If AssignedTo = "Select " Or AssignedTo = HttpContext.Current.Session("user_id") Then
            'Else
            '    Dim dbE As New DBHelper
            '    Dim sEmail As String
            '    Dim strSQL As String
            '    strSQL = " select ISNULL(email, '') as email from users where userID = " & AssignedTo
            '    sEmail = dbE.ExecuteScalar(CommandType.Text, strSQL)
            '    If sEmail = "" Then
            '    Else
            '        Dim mailMsg As New System.Net.Mail.MailMessage("ITSupport@tic-us.com", sEmail)
            '        mailMsg.Subject = "An activity has been assigned to you."
            '        Dim strBody As String = ""
            '        strBody = "<div style='font:12px Verdana, Geneva, sans-serif;'>"
            '        strBody += "An activity has been assigned to you <br/> <br/>"
            '        strBody += "<br/> "
            '        strBody += "Activity: " + System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(Details.Replace("%0A%20", "%0A")))
            '        strBody += "<br/> "
            '        strBody += "Date updated : " + System.DateTime.Now()
            '        strBody += "<br/> <br/> Regards, <br/> <br/> TIC Support Team."
            '        strBody += "</div>"
            '        mailMsg.Body = strBody
            '        mailMsg.IsBodyHtml = True

            '        Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
            '        smtp.Port = "587"
            '        smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "1")

            '        Dim token As [Object] = Nothing
            '        smtp.SendAsync(mailMsg, token)
            '    End If
            'End If

            Return 1
        Catch ex As Exception
            Return 0
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveMeeting(ByVal ApptId As Integer, ByVal MeetDate As String, ByVal MeetTime As String, _
            ByVal TimeZone As String, ByVal MeetLength As String, ByVal MeetInterface As String, ByVal MeetType As String, _
            ByVal AssignedTo As String, ByVal CurEnv As String, ByVal DisPoints As String, ByVal IceBr As String, _
            ByVal AccountId As String, ByVal ContactId As String) As Int32
        Try

            Dim db As New DBHelperClient
            Dim parms(14) As DBHelperClient.Parameters
            If ApptId = 0 Then
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            Else
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "U")
            End If
            parms(1) = New DBHelperClient.Parameters("p_appt_id", ApptId)
            parms(2) = New DBHelperClient.Parameters("p_t_account_id", AccountId)
            parms(3) = New DBHelperClient.Parameters("p_t_contact_id", ContactId)
            If MeetDate = "" Then
                parms(4) = New DBHelperClient.Parameters("p_mtgdate", System.DBNull.Value)
            Else
                parms(4) = New DBHelperClient.Parameters("p_mtgdate", Convert.ToDateTime(MeetDate).ToString("yyyy-MM-dd"))
            End If
            parms(5) = New DBHelperClient.Parameters("p_mtgtime", MeetTime)
            parms(6) = New DBHelperClient.Parameters("p_mtgtimezone", IIf(TimeZone = "Select ", System.DBNull.Value, TimeZone))
            parms(7) = New DBHelperClient.Parameters("p_mtglength", IIf(MeetLength = "Select ", System.DBNull.Value, MeetLength))
            parms(8) = New DBHelperClient.Parameters("p_mtginterface", IIf(MeetInterface = "Select ", System.DBNull.Value, MeetInterface))
            parms(9) = New DBHelperClient.Parameters("p_mtgtype", IIf(MeetType = "Select ", System.DBNull.Value, MeetType))
            parms(10) = New DBHelperClient.Parameters("p_mtgassignedto", IIf(AssignedTo = "Select ", System.DBNull.Value, AssignedTo))
            parms(11) = New DBHelperClient.Parameters("p_curenv", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(CurEnv.Replace("%0A%20", "%0A"))))
            parms(12) = New DBHelperClient.Parameters("p_dispoints", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(DisPoints.Replace("%0A%20", "%0A"))))
            parms(13) = New DBHelperClient.Parameters("p_icebr", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(IceBr.Replace("%0A%20", "%0A"))))
            parms(14) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
            db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDMeeting", parms)

            Return 1
        Catch ex As Exception
            Return 0
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
   Public Shared Function ConfMeeting(ByVal ApptId As Integer) As Int32
        Try

            Dim db As New DBHelperClient
            Dim parms(1) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("apptid", ApptId)
            parms(1) = New DBHelperClient.Parameters("userid", HttpContext.Current.Session("user_id"))

            Dim strSQL As String
            strSQL = " UPDATE tblmeeting set mtgconfirmedby = @userid , mtgconfirmedon = CURRENT_TIMESTAMP  WHERE appt_id = @apptid"
            db.ExecuteNonQuery(Data.CommandType.Text, strSQL,parms)
            Return 1

        Catch ex As Exception
            Return 0
        End Try
    End Function


    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveCall(ByVal t_contact_id As String, ByVal t_account_id As String, ByVal calledphone As String, _
                                ByVal calltype As String, ByVal acttype As String, ByVal description As String) As Int32

        Try
            Dim retval As Integer = 0
            Dim db As New DBHelperClient

            If calltype = "S" Then
				Dim parms(5) As DBHelperClient.Parameters
				parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
				parms(1) = New DBHelperClient.Parameters("p_t_contact_id", t_contact_id)
				parms(2) = New DBHelperClient.Parameters("p_t_account_id", t_account_id)
				parms(3) = New DBHelperClient.Parameters("p_call_id", 0)
				parms(4) = New DBHelperClient.Parameters("p_called_phone", calledphone)
				parms(5) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
				db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IDCall", parms)
            End If

            ' Dim tmpSQL As String
            ' tmpSQL = "Select activitytypeid from tblactivitytype where activitytypeshort = 'Call' "
            ' Dim activitytypeid As Integer = 0
            ' activitytypeid = db.ExecuteScalar(CommandType.Text, tmpSQL)

            If calltype = "L" Then
            Dim parmsA(13) As DBHelperClient.Parameters
				parmsA(0) = New DBHelperClient.Parameters("p_dFlag", "I")
				parmsA(1) = New DBHelperClient.Parameters("p_activity_id", 0)
				parmsA(2) = New DBHelperClient.Parameters("p_t_account_id", t_account_id)
				parmsA(3) = New DBHelperClient.Parameters("p_t_contact_id", t_contact_id)
				parmsA(4) = New DBHelperClient.Parameters("p_activity_date", DateTime.Now.ToString("yyyy-MM-dd"))
                parmsA(5) = New DBHelperClient.Parameters("p_activity_time", System.DBNull.Value)
				parmsA(6) = New DBHelperClient.Parameters("p_assigned_to", HttpContext.Current.Session("user_id"))
				parmsA(7) = New DBHelperClient.Parameters("p_activity_type", acttype)
				parmsA(8) = New DBHelperClient.Parameters("p_priority", "Normal")
				parmsA(9) = New DBHelperClient.Parameters("p_activity_status", "Complete")
				parmsA(10) = New DBHelperClient.Parameters("p_reminder_date", System.DBNull.Value)
				parmsA(11) = New DBHelperClient.Parameters("p_activity_detail", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(description.Replace("%0A%20", "%0A"))))
				parmsA(12) = New DBHelperClient.Parameters("p_inventory_id", System.DBNull.Value)
				parmsA(13) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
				db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDActivity", parmsA)
            End If

            Return t_account_id

        Catch ex As Exception

        End Try

    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveNote(ByVal t_contact_id As String, ByVal t_account_id As String, ByVal description As String) As Int32

        Try
            Dim db As New DBHelperClient

            Dim tmpSQL As String
            tmpSQL = "Select activitytypeid from tblactivitytype where activitytypeshort = 'Note' "
            Dim activitytypeid As Integer = 0
            activitytypeid = db.ExecuteScalar(CommandType.Text, tmpSQL)

            Dim retval As Integer = 0
            Dim parms(13) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            parms(1) = New DBHelperClient.Parameters("p_activity_id", 0)
            parms(2) = New DBHelperClient.Parameters("p_t_account_id", t_account_id)
            parms(3) = New DBHelperClient.Parameters("p_t_contact_id", t_contact_id)
            parms(4) = New DBHelperClient.Parameters("p_activity_date", DateTime.Now.ToString("yyyy-MM-dd"))
            parms(5) = New DBHelperClient.Parameters("p_activity_time", System.DBNull.Value)
            parms(6) = New DBHelperClient.Parameters("p_assigned_to", HttpContext.Current.Session("user_id"))
            parms(7) = New DBHelperClient.Parameters("p_activity_type", "Notes")
            parms(8) = New DBHelperClient.Parameters("p_priority", "Normal")
            parms(9) = New DBHelperClient.Parameters("p_activity_status", "Complete")
            parms(10) = New DBHelperClient.Parameters("p_reminder_date", System.DBNull.Value)
            parms(11) = New DBHelperClient.Parameters("p_activity_detail", System.Web.HttpUtility.HtmlDecode(HttpUtility.UrlDecode(description.Replace("%0A%20", "%0A"))))
            parms(12) = New DBHelperClient.Parameters("p_inventory_id", System.DBNull.Value)
            parms(13) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
            db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDActivity", parms)

            Return t_account_id

        Catch ex As Exception

        End Try

    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveList(ByVal ListId As Integer, ByVal ListType As String, ByVal ListName As String, _
                                ByVal ListDesc As String, ByVal ListOwner As String, ByVal AcctId As String, _
                                ByVal CtId As String) As Int32
        Try

            Dim GetListId As String = ""
            Dim db As New DBHelperClient
            Dim parms(7) As DBHelperClient.Parameters
            If ListId = 0 Then
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            Else
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "U")
            End If
            parms(1) = New DBHelperClient.Parameters("p_list_id", ListId)
            parms(2) = New DBHelperClient.Parameters("p_list_type", IIf(ListType = "Select ", System.DBNull.Value, ListType))
            parms(3) = New DBHelperClient.Parameters("p_list_name", ListName)
            parms(4) = New DBHelperClient.Parameters("p_list_desc", ListDesc)
            parms(5) = New DBHelperClient.Parameters("p_list_owner", IIf(ListOwner = "Select ", System.DBNull.Value, ListOwner))
            parms(6) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
            If ListId = 0 Then
                GetListId = db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDListMaster", parms)
            Else
                db.ExecuteNonQuery(Data.CommandType.StoredProcedure, "SP_IUDListMaster", parms)
            End If

            If AcctId <> 0 Then
                Dim parmsLAC(3) As DBHelperClient.Parameters
                parmsLAC(0) = New DBHelperClient.Parameters("listid", GetListId)
                If ListType = "Account" Then
                    parmsLAC(1) = New DBHelperClient.Parameters("t_account_id", AcctId)
                    parmsLAC(2) = New DBHelperClient.Parameters("t_contact_id", 0)
                Else
                    parmsLAC(1) = New DBHelperClient.Parameters("t_account_id", AcctId)
                    parmsLAC(2) = New DBHelperClient.Parameters("t_contact_id", CtId)
                End If
                parmsLAC(3) = New DBHelperClient.Parameters("User", HttpContext.Current.Session("user_id"))

                Dim strSQL As String
                strSQL = "Insert into tbllistac (listid, t_account_id, t_contact_id, createdby, createdts) " & _
                            " Values ( @listid, @t_account_id, @t_contact_id, @User, CURRENT_TIMESTAMP) "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parmsLAC)
            End If

            Return 1
        Catch ex As Exception
            Return 0
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function ValidateDupAttr(ByVal AttrType As String, ByVal AttrDesc As String, ByVal AccountId As String) As Boolean
        Try

            Dim retVal As Boolean = False
            Dim str As String
            Dim db As New DBHelperClient
            Dim AttrTypeS As String = ""

            Select Case AttrType
                Case Is = "System"
                    AttrTypeS = "SY"
                Case Is = "Profit Recovery Firm"
                    AttrTypeS = "PR"
                Case Is = "Total # of systems"
                    AttrTypeS = "TS"
                Case Is = "Use Audit Software"
                    AttrTypeS = "AS"
                Case Is = "Purchasing Card"
                    AttrTypeS = "PC"
            End Select

            Dim parms(2) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("description", Trim(AttrDesc))
            parms(1) = New DBHelperClient.Parameters("acctothtype", AttrTypeS)
            parms(2) = New DBHelperClient.Parameters("accountId", AccountId)
            str = " Description from tblacctothers where description = @description and acctothtype = @acctothtype and t_account_id = @accountId "

            If ConfigurationManager.AppSettings("DATA.PROVIDER").ToString.Contains("MySql") Then
                str = "Select " & str & " limit 1 "
            Else
                str = "Select top 1 " & str
            End If
            Dim ds As DataSet = db.DataAdapter(CommandType.Text, str,parms)

            If ds.Tables(0).Rows.Count > 0 Then
                retVal = False
            Else
                retVal = True
            End If
            Return retVal

        Catch ex As Exception
            Return False
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveAccountAttr(ByVal CoId As String, ByVal AttrType As String, ByVal TypeId As String, _
       ByVal Description As String) As Int32

        Dim retval As Integer = 0
        Dim AttrTypeS As String = ""
        Try
            Select Case AttrType
                Case Is = "System"
                    AttrTypeS = "SY"
                Case Is = "Profit Recovery Firm"
                    AttrTypeS = "PR"
                Case Is = "Total # of systems"
                    AttrTypeS = "TS"
                Case Is = "Use Audit Software"
                    AttrTypeS = "AS"
                Case Is = "Purchasing Card"
                    AttrTypeS = "PC"
            End Select

            Dim db As New DBHelperClient
            Dim parms(4) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("t_account_id", CoId)
            parms(1) = New DBHelperClient.Parameters("AcctOthType", AttrTypeS)
            If TypeId = "" Then
                parms(2) = New DBHelperClient.Parameters("TypeId", "0")
            Else
                parms(2) = New DBHelperClient.Parameters("TypeId", TypeId)
            End If
            parms(3) = New DBHelperClient.Parameters("Description", Description)
            parms(4) = New DBHelperClient.Parameters("user_id", HttpContext.Current.Session("user_id"))

            Dim strSQL As String
            strSQL = " Insert into tblacctothers (t_account_id, acctothtype, typeid, description, createdby, createdts " & _
                    " ,modifiedby , modifiedts ) Values ( @t_account_id, @AcctOthType, @TypeId, @Description, @user_id " & _
                    " ,CURRENT_TIMESTAMP, @user_id ,CURRENT_TIMESTAMP) "
            db.ExecuteScalar(CommandType.Text, strSQL, parms)
            retval = 1

        Catch ex As Exception
            Return 0

        End Try
        Return retval

    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
    Public Shared Function SaveInvent(ByVal CoId As String, ByVal CtId As String, ByVal InvId As String) As Int32

        Dim retval As Integer = 0
        Dim AttrTypeS As String = ""
        Try

            Dim db As New DBHelperClient
            Dim parms(13) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            parms(1) = New DBHelperClient.Parameters("p_activity_id", System.DBNull.Value)
            parms(2) = New DBHelperClient.Parameters("p_t_account_id", CoId)
            parms(3) = New DBHelperClient.Parameters("p_t_contact_id", CtId)
            parms(4) = New DBHelperClient.Parameters("p_activity_date", System.DateTime.Now.ToString("yyyy-MM-dd"))
            parms(5) = New DBHelperClient.Parameters("p_activity_time", System.DBNull.Value)
            parms(6) = New DBHelperClient.Parameters("p_assigned_to", System.DBNull.Value)
            parms(7) = New DBHelperClient.Parameters("p_activity_type", "Inventory")
            parms(8) = New DBHelperClient.Parameters("p_priority", System.DBNull.Value)
            parms(9) = New DBHelperClient.Parameters("p_activity_status", "Complete")
            parms(10) = New DBHelperClient.Parameters("p_reminder_date", System.DBNull.Value)
            parms(11) = New DBHelperClient.Parameters("p_activity_detail", System.DBNull.Value)
            parms(12) = New DBHelperClient.Parameters("p_inventory_id", InvId)
            parms(13) = New DBHelperClient.Parameters("p_user_id", HttpContext.Current.Session("user_id"))
            db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_IUDActivity", parms)

            retval = 1

        Catch ex As Exception
            Return 0

        End Try
        Return retval

    End Function


    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveListA(ByVal ListSelA As String, ByVal AccountId As String, ByVal ContactId As String) As Int32

        Dim retval As Integer = 0
        Dim db As New DBHelperClient
        Try

            Dim parms(3) As DBHelperClient.Parameters

            parms(0) = New DBHelperClient.Parameters("listid", ListSelA)
            parms(1) = New DBHelperClient.Parameters("t_account_id", AccountId)
            parms(2) = New DBHelperClient.Parameters("t_contact_id", ContactId)
            parms(3) = New DBHelperClient.Parameters("User", HttpContext.Current.Session("user_id"))

            Dim strSQL As String
            strSQL = "Insert into tbllistac (listid, t_account_id, t_contact_id, createdby, createdts) " & _
                        " Values ( @listid, @t_account_id, @t_contact_id, @User, CURRENT_TIMESTAMP) "
            db.ExecuteNonQuery(CommandType.Text, strSQL, parms)

            retval = 1
        Catch ex As Exception
            retval = 0

        End Try
        Return retval
    End Function

	    <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function SaveOwner(ByVal t_account_id As String, ByVal owner As String) As Int32

        Try
            Dim strSQL As String
            Dim parms(1) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("ownerid", owner)
            parms(1) = New DBHelperClient.Parameters("account_id", t_account_id)

            strSQL = "Update tblaccount set t_ownerid = @ownerid where t_account_id = @account_id "
            Dim db As New DBHelperClient
            db.ExecuteNonQuery(CommandType.Text, strSQL,parms)

            Return t_account_id

        Catch ex As Exception

        End Try

    End Function

	     <System.Web.Services.WebMethod(EnableSession:=True)> _
	Public Shared Function fnSaveDash(ByVal dashid As Integer,ByVal dashname As String) As Int32
        Try

            Dim GetListId As String = ""
            Dim db As New DBHelper
            Dim parms(1) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("i_dashlistid", dashid)
            parms(1) = New DBHelper.Parameters("i_dashname", dashname)
            db.ExecuteNonQuery(Data.CommandType.StoredProcedure, "SP_SaveDash", parms)
            Return 1

        Catch ex As Exception
            Return 0
        End Try
    End Function

     <System.Web.Services.WebMethod(EnableSession:=True)> _
Public Shared Function CheckDupEmail(ByVal email As String, byval t_contact_id As String) As Integer
        Try

            Dim retVal As Integer = 0
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim ctemail As String
            Dim emailcnt As Integer

            If email = "" Then
                Return 0
            End If
            Dim parms1(0) As DBHelperClient.Parameters
            parms1(0) = New DBHelperClient.Parameters("clientid", HttpContext.Current.Session("client_id"))

            strSQL = "SELECT ctemail FROM client_master.client_master WHERE client_id = @clientid "
            ctemail = db.ExecuteScalar(CommandType.Text, strSQL,parms1)

            If ctemail = "Always Allow" Then
                Return 0
            Else
                Dim parms2(1) As DBHelperClient.Parameters
                parms2(0) = New DBHelperClient.Parameters("email", email)
                parms2(1) = New DBHelperClient.Parameters("contact_id", t_contact_id)
                If t_contact_id = 0 Then
                    strSQL = "SELECT COUNT(*) FROM tblcontact WHERE email = @email "
                Else
                    strSQL = "SELECT COUNT(*) FROM tblcontact WHERE email = @email and t_contact_id <> @contact_id "
                End If

                emailcnt = db.ExecuteScalar(CommandType.Text, strSQL,parms2)
                If emailcnt = 0 Then
                    Return 0
                ElseIf ctemail = "Do not allow" Then
                    Return 1
                ElseIf ctemail = "Allow by alert the user they are entering a duplicate"
                    Return 2
                End If
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

        <System.Web.Services.WebMethod(EnableSession:=True)> _
	Public Shared Function CheckDupAcct(ByVal coname As String, byval t_account_id As String) As Integer
        Try

            Dim retVal As Integer = 0
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim dt As new DataTable
            Dim parms(1) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("coname", coname)
            parms(1) = New DBHelperClient.Parameters("t_account_id", t_account_id)

            If t_account_id =  0 Then
                strSQL = "SELECT 1 FROM tblaccount WHERE LOWER(NAME) = LOWER(@coname) "
            Else
                strSQL = "SELECT 1 FROM tblaccount WHERE t_account_id <> @t_account_id and LOWER(NAME) = LOWER(@coname)"
            End If
            dt = db.DataAdapter(CommandType.Text, strSQL,parms).Tables(0)

            If dt.Rows.Count > 0 Then
                Return 1
            Else
                Return 0
            End If

        Catch ex As Exception
            Return False
        End Try
    End Function

    <System.Web.Services.WebMethod(EnableSession:=True)> _
	Public Shared Function GetPassword(ByVal email As String) As String()

        Try
            Dim retval As Integer = 0
            Dim values(1) As String
            Dim db As New DBHelper
            Dim parms(0) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("email", email)

            Dim strSQL As String
            strSQL = "select userPassword from users where userLogin = @email "
            values(0) = db.ExecuteScalar(CommandType.Text, strSQL,parms)

            If values(0) = "" Then
                values(1) = "Invalid user."
            Else
                Dim mailMsg As New MailMessage
                mailMsg.From = New System.Net.Mail.MailAddress("ITSupport@tic-us.com")
                mailMsg.To.Add(New System.Net.Mail.MailAddress(email))

                mailMsg.Subject = "Your SalesShark password."
                mailMsg.IsBodyHtml = True
                mailMsg.Headers.Add("X-MC-Track", "opens,clicks_all")
                mailMsg.Headers.Add("X-MC-AutoHtml", "1")

                Dim strBody As String = ""
                strBody = "<div style='font:12px Verdana, Geneva, sans-serif;'>"
                strBody += "Your SalesShark password is as below:"
                strBody += "<br/>"
                strBody += "<br/>"
                strBody += "Password: " & values(0)
                strBody += "<br/>"
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

                values(1) = "Please check your email for password."

            End If

            Return values

        Catch ex As Exception

        End Try

    End Function
	
End Class
