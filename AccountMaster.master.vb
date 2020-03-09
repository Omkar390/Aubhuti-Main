Imports System.Data
Imports System.Globalization
Imports System.Net.Mail

Imports System.IO
Imports System.Net
Imports System.Exception

Imports Limilabs.Client.IMAP
Imports Limilabs.Mail
Imports Limilabs.Mail.MIME

Imports LumiSoft.Net
Imports LumiSoft.Net.Mail
Imports LumiSoft.Net.MIME
Imports LumiSoft.Net.IMAP
Imports LumiSoft.Net.IMAP.Client
Imports System.Security.Cryptography
Imports AjaxControlToolkit

Partial Class AccountMaster
    Inherits System.Web.UI.MasterPage
    Protected BindAccount As String = String.Empty
    Protected BindContact As String = String.Empty
    Protected BindActivity As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim dt As New DataTable
            Dim ds As New DataSet
            Dim tmpstr As String = ""
                BindAccount = Page.ClientScript.GetPostBackEventReference(btnAcctRefresh, String.Empty)
                BindContact = Page.ClientScript.GetPostBackEventReference(btnCtRefresh, String.Empty)
                BindActivity = Page.ClientScript.GetPostBackEventReference(btnActRefresh, String.Empty)

			If Not Request.QueryString("mrid") Is Nothing Then
                hdnmrid.Value = Request.QueryString("mrid")
            End If

			
            If Not Page.IsPostBack Then

                Dim dbM As New DBHelper
                Dim dbMRM As New DBHelperClient
                Dim strSQL As String
                Dim dsO As New DataSet
                Dim dsMR As New DataSet
                Dim dt1 As New DataTable
                strSQL = " SELECT U.userid, U.username FROM users U " & _
                        " INNER JOIN tbluserclient UC ON U.userid = UC.userid " & _
                        " WHERE UC.userGroup = 'AE' AND UC.client_id = " & Session("client_id")
                dsO = dbM.DataAdapter(CommandType.Text, strSQL)
                pOwner.DataSource = dsO.Tables(0)
                pOwner.DataBind()
                ddlOwner.DataSource = dsO.Tables(0)
                ddlOwner.DataBind()

                strSQL = " Select EmailSignature from tbluser where Email = '" & Session("email") & "'"
                'editor.Text = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
                dt1 = dbMRM.DataAdapter(CommandType.Text, strSQL).Tables(0)

                If dt1.Rows.Count > 0 Then
                    pQEEmailBody.Value = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
                    pEmailBody.Value = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
                End If

                Dim dbU As New DBHelper
                Dim dtU As New DataTable

                strSQL = " SELECT U.userID, U.email " & _
                        " FROM users U INNER JOIN tbluserclient UC ON UC.userid = U.userID " & _
                        " WHERE U.Active = 1 AND  UC.client_id = " & Session("client_id") & _
                        " ORDER BY username "
                dtU = dbU.DataAdapter(CommandType.Text, strSQL).Tables(0)
                pSelFrom.DataSource = dtU
                pSelFrom.DataBind()
                pSelFrom.ClearSelection()
                pSelFrom.Items.FindByText(Session("email")).Selected = True

                pSelReplyTo.DataSource = dtU
                pSelReplyTo.DataBind()
                pSelReplyTo.ClearSelection()
                pSelReplyTo.Items.FindByText(Session("email")).Selected = True

                'Dim dt As New DataTable
                Dim dbC As New DBHelperClient 
                dt = dbC.DataAdapter(CommandType.Text, _
                                    " Select '0' as TemplateTypeID, ' Select ' as TemplateType UNION " & _
                                    " Select TemplateTypeID, TemplateType from tblTemplateType order by TemplateType " ).Tables(0)
                pTemplateType.DataSource=dt
                pTemplateType.DataBind()

				strSQL = "Select '0' as EmailTemplateID, 'an Email Template' as emailtemplatename  "
				ddlSubject.DataSource = dbC.DataAdapter(CommandType.Text,strSQL).Tables(0)
				ddlSubject.DataBind()

                strSQL = " SELECT mrid,mrname FROM tblmicroregionmaster where coalesce(isdeleted,0) = 0 order by mrname; " & _
                        " SELECT ctarowid, aname FROM tblctareamaster where coalesce(isdeleted,0) = 0 order by aname; " & _
                        " SELECT ctstrowid, stname FROM tblstatusmaster where coalesce(isdeleted,0) = 0 order by stname; " & _
                        " SELECT ctlrowid, lname FROM tbllevelmaster where coalesce(isdeleted,0) = 0 order by lname; " & _
                        " SELECT atstrowid, stname FROM tblaccountstatusmaster  where coalesce(isdeleted,0) = 0 order by stname; " & _
                        " SELECT InventoryID, InventoryShort FROM tblinventory WHERE COALESCE(IsDeleted, 0) = 0 order by InventoryShort "

                dsMR = dbMRM.DataAdapter(CommandType.Text, strSQL)
                pMicroRegion.DataSource = dsMR.Tables(0)
                pMicroRegion.DataBind()
                pMicroRegion.Items.Insert(0, New ListItem("Select ", "0"))

                pCtArea.DataSource = dsMR.Tables(1)
                pCtArea.DataBind()
                pCtArea.Items.Insert(0, New ListItem("Select ", "0"))

                pCtStatus.DataSource = dsMR.Tables(2)
                pCtStatus.DataBind()
                pCtStatus.Items.Insert(0, New ListItem("Select ", "0"))

                pCtLevel.DataSource = dsMR.Tables(3)
                pCtLevel.DataBind()
                pCtLevel.Items.Insert(0, New ListItem("Select ", "0"))

                pStatus.DataSource = dsMR.Tables(4)
                pStatus.DataBind()
                pStatus.Items.Insert(0, New ListItem("Select ", "0"))

                pddlInv.DataSource = dsMR.Tables(5)
                pddlInv.DataBind()
                pddlInv.Items.Insert(0, New ListItem("Select ", "0"))

                pCountry.Items.FindByValue("US").Selected = True
                pCtCountry.Items.FindByValue("US").Selected = True

                Dim tmpSQL As String
                tmpSQL = "Select activitytypeshort, activitytypeid from tblactivitytype where Coalesce(isdeleted, 0) = 0  And activitytypeshort Like 'Call%' order by activitytypeshort ; "
				Dim dbS As New DBHelperClient
                ds = dbS.DataAdapter(CommandType.Text, tmpSQL)

                pType.DataSource = ds.Tables(0)
                pType.DataBind()

                tmpSQL = "Select tiertype, tiertypeid from tbltiertype where Coalesce(isdeleted, 0) = 0  order by tiertype ; "
                ds = dbS.DataAdapter(CommandType.Text, tmpSQL)
                pTier.DataSource = ds.Tables(0)
                pTier.DataBind()

                tmpSQL = "SELECT emailstatus,emailstatusid FROM tblemailstatus WHERE COALESCE(isdeleted,0) = 0 ORDER BY emailstatus; "
                ds = dbS.DataAdapter(CommandType.Text, tmpSQL)
                ddlEmailStatus.DataSource = ds.Tables(0)
                ddlEmailStatus.DataBind()
                ddlEmailStatus.ClearSelection
                Try
                    ddlEmailStatus.Items.FindByValue("1").Selected = True
                Catch ex As Exception

                End Try
                

                tmpSQL = "Select PersonaId, PersonaDescription from tblpersona where Coalesce(isdeleted, 0) = 0  order by PersonaDescription ; "
                ds = dbS.DataAdapter(CommandType.Text, tmpSQL)
                pCtPersona.DataSource = ds.Tables(0)
                pCtPersona.DataBind()
                pCtPersona.Items.Insert(0, New ListItem("Select ", "0"))

                Dim db As New DBHelperClient
                Select Case Request.QueryString("BindType")
                    Case "A"

                        tblContact.Visible = False
                        tblQuickClick.Visible = False
                        tblInv.Visible = False
                        hdncontactId.Value = "0"

                        BindAcct()

                        Dim CoName As String
                        CoName = lithdrCo.Text.Replace("inc", "").Replace("Inc", "").Replace("Corp", "").Replace("Company", "").Replace("Co", "").Replace("Group", "")
                        CoName = CoName.Replace("Corporation", "").Replace("Limited", "").Replace("L.P", "").Replace("LLC", "").Replace("Ltd", "").Replace(".", "")

                        Dim parms(1) As DBHelperClient.Parameters
                        parms(0) = New DBHelperClient.Parameters("p_t_account_id", hdnaccountId.Value)
                        parms(1) = New DBHelperClient.Parameters("p_SearchTerm", lithdrCo.Text)
                        ds = db.DataAdapter(CommandType.StoredProcedure, "SP_GetAcctCounts", parms)

                        litEvents.Text = 0
                        litMeetings.Text = ds.Tables(1).Rows(0).Item(0).ToString
                        litSurveys.Text = ds.Tables(2).Rows(0).Item(0).ToString
                        litOppors.Text = ds.Tables(3).Rows(0).Item(0).ToString
                        litActivities.Text = ds.Tables(4).Rows(0).Item(0).ToString
                        litWebLogs.Text = ds.Tables(5).Rows(0).Item(0).ToString
                        litLists.Text = ds.Tables(6).Rows(0).Item(0).ToString
                        litAttachs.Text = ds.Tables(7).Rows(0).Item(0).ToString
                        litCalls.Text = ds.Tables(8).Rows(0).Item(0).ToString
                        litEmails.Text = ds.Tables(9).Rows(0).Item(0).ToString
                        litContacts.Text = ds.Tables(10).Rows(0).Item(0).ToString

                        tmpstr = "?BindType=A&AcctId=" & hdnguidaccountId.Value
                        aAccount.HRef = "Account.aspx" & tmpstr
                        lnkflip.Visible = False

                        pCtAddress.Value = pAddress.Value
                        pCtCity.Value = pCity.Value
                        pCtState.Value = pState.Value
                        pCtZip.Value = pZip.Value
                        pCtCountry.ClearSelection()
                        pCtCountry.Items.FindByValue(pCountry.SelectedValue).Selected = True

                    Case "C"

                        dt = SSManager.GetAllCo("S", hdnaccountId.Value)
                        lithdrAO.Text = dt.Rows(0).Item("TxtOwnerId").ToString
                        hdnownerId.Value = dt.Rows(0).Item("t_ownerid").ToString
                        lithdrCo.Text = dt.Rows(0).Item("name").ToString
                        tblAccount.Visible = False
                        litCtCoPhone.Text = dt.Rows(0).Item("phone").ToString
                        lnkCtCoPhoneHdn.InnerHtml = dt.Rows(0).Item("phone").ToString
                        lnkCtCoPhoneHdn.HRef = "skype:" & dt.Rows(0).Item("phone").ToString & "?call"

                        BindCt()

                        Dim CoName As String
                        CoName = lithdrCo.Text.Replace("inc", "").Replace("Inc", "").Replace("Corp", "").Replace("Company", "").Replace("Co", "").Replace("Group", "")
                        CoName = CoName.Replace("Corporation", "").Replace("Limited", "").Replace("L.P", "").Replace("LLC", "").Replace("Ltd", "").Replace(".", "")

                        Dim parms(2) As DBHelperClient.Parameters
                        parms(0) = New DBHelperClient.Parameters("p_account_id", hdnaccountId.Value)
                        parms(1) = New DBHelperClient.Parameters("p_contact_id", hdncontactId.Value)
                        parms(2) = New DBHelperClient.Parameters("p_SearchTerm", CoName)
                        ds = db.DataAdapter(CommandType.StoredProcedure, "SP_GetCtCounts", parms)

                        litEvents.Text = 0
                        litMeetings.Text = ds.Tables(1).Rows(0).Item(0).ToString
                        litSurveys.Text = ds.Tables(2).Rows(0).Item(0).ToString
                        litOppors.Text = ds.Tables(3).Rows(0).Item(0).ToString
                        litActivities.Text = ds.Tables(4).Rows(0).Item(0).ToString
                        litWebLogs.Text = ds.Tables(5).Rows(0).Item(0).ToString
                        litLists.Text = ds.Tables(6).Rows(0).Item(0).ToString
                        litAttachs.Text = ds.Tables(7).Rows(0).Item(0).ToString
                        litCalls.Text = ds.Tables(8).Rows(0).Item(0).ToString
                        litEmails.Text = ds.Tables(9).Rows(0).Item(0).ToString
                        litContacts.Text = ds.Tables(10).Rows(0).Item(0).ToString

                        tmpstr = "?BindType=A&AcctId=" & hdnguidaccountId.Value
                        aAccount.HRef = "Account.aspx" & tmpstr
                        lnkflip.HRef = "Account.aspx" & tmpstr
                        lnkflip.Visible = True
                        tmpstr = "?BindType=C&AcctId=" & hdnguidaccountId.Value & "&CtId=" & hdnguidcontactId.Value

                End Select

                aEvent.HRef = "Events.aspx" & tmpstr
                aMeeting.HRef = "Meetings.aspx" & tmpstr
                aSurvey.HRef = "Surveys.aspx" & tmpstr
                aOpportunity.HRef = "Opportunities.aspx" & tmpstr
                aActivity.HRef = "Activities.aspx" & tmpstr
                aWeblog.HRef = "Weblogs.aspx" & tmpstr
                aList.HRef = "DashLists.aspx" & tmpstr
                aAttachment.HRef = "Attachments.aspx" & tmpstr
                aCall.HRef = "Calls.aspx" & tmpstr
                aEmail.HRef = "Emails.aspx" & tmpstr

                pSendto.Text = litCtEmail.Text
				
				                'show account edit pop-up
                If Request.QueryString("edit") = "1" Then
                    frommicroregionreport.Value = "1"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "editcoscript", "editco();", True)
                Else
                    frommicroregionreport.Value = "0"
                End If

            End If

			
        Catch ex As Exception            
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

    Protected Sub BindAcctOthers()
        Try

            Dim dt As New DataTable
            Dim ds As New DataSet
            Dim db As New DBHelperClient

            Dim parmsAO(1) As DBHelperClient.Parameters
            parmsAO(0) = New DBHelperClient.Parameters("p_account_id", hdnaccountId.Value)
            parmsAO(1) = New DBHelperClient.Parameters("p_contact_id", hdncontactId.Value)
            ds = New DataSet
            ds = db.DataAdapter(CommandType.StoredProcedure, "SP_GetAcctOther", parmsAO)
            rptSys.DataSource = ds.Tables(0)
            rptSys.DataBind()
            rptPR.DataSource = ds.Tables(1)
            rptPR.DataBind()
            rptAS.DataSource = ds.Tables(2)
            rptAS.DataBind()
        Catch ex As Exception
            SSManager.LogAppError(ex, "BindAcctOthers")
        End Try

    End Sub

    Protected Sub BindInv()
        Try

            Dim dt As New DataTable
            Dim ds As New DataSet
            Dim db As New DBHelperClient

            Dim parmsAO(0) As DBHelperClient.Parameters
            parmsAO(0) = New DBHelperClient.Parameters("p_contact_id", hdncontactId.Value)
            ds = New DataSet
            Dim tmpSQL As String
            tmpSQL = " SELECT activity_id, InventoryShort, DATE_FORMAT(activity_date , '%m/%d/%Y') as activity_date " & _
                    " FROM tblactivities A " & _
                    " INNER JOIN tblinventory I ON A.inventory_id = I.InventoryId " & _
                    " WHERE t_contact_id = " & hdncontactId.Value & " AND activity_type = 'Inventory' "

            ds = db.DataAdapter(CommandType.Text, tmpSQL, parmsAO)
            rptInv.DataSource = ds.Tables(0)
            rptInv.DataBind()

        Catch ex As Exception
            SSManager.LogAppError(ex, "BindAcctOthers")
        End Try

    End Sub

    Protected Sub btnAcctRefresh_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAcctRefresh.ServerClick
		'show account edit pop-up
        If Request.QueryString("edit") = "1" Then
            frommicroregionreport.Value = "1"
			hdnmrid.Value = Request.QueryString("mrid")
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "CancelAccountscript", "CancelAccount();", True)
        ElseIf Request.QueryString("AcctId") = "New" Then
            Dim AcctId As String
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_acctid", hdnaccountId.Value)
            strSQL = "Select id from tblaccount where t_account_id = @p_acctid "
            AcctId = db.ExecuteScalar(CommandType.Text, strSQL, parms)
            Response.Redirect("Account.aspx?BindType=A&AcctId=" & AcctId, False)
        Else
            BindAcct()
        End If
    End Sub

    Protected Sub btnCtRefresh_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCtRefresh.ServerClick
        If Request.QueryString("BindType") = "A" Then
            Response.Redirect("Account.aspx?BindType=A&AcctId=" & Request.QueryString("AcctId"), False)
        Else
            'BindCt()
            'UpdatePanelCt.Update()
            Response.Redirect("Activities.aspx?BindType=C&AcctId=" & Request.QueryString("AcctId") & "&CtId=" & Request.QueryString("CtId"), False)
        End If
    End Sub

    Protected Sub btnActRefresh_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnActRefresh.ServerClick
        Response.Redirect("Activities.aspx?BindType=C&AcctId=" & Request.QueryString("AcctId") & "&CtId=" & Request.QueryString("CtId"), False)
    End Sub

    Protected Sub BindAcct()

        Try
            BindAcctOthers()

            Dim dt As New DataTable
            Dim db As New DBHelperClient
            dt = SSManager.GetAllCo("S", hdnaccountId.Value)

            lithdrAO.Text = dt.Rows(0).Item("TxtOwnerId").ToString
            lithdrCo.Text = dt.Rows(0).Item("name").ToString

            litCoName.Text = lithdrCo.Text
            pCoName.Value = litCoName.Text
            litAddress.Text = dt.Rows(0).Item("physicalstreet").ToString
            If litAddress.Text = "," Then
                litAddress.Text = ""
            End If
            litCityStZip.Text = dt.Rows(0).Item("physicalcity").ToString + ", " + dt.Rows(0).Item("physicalstate").ToString _
                                + ", " +  dt.Rows(0).Item("physicalpostalcode").ToString
            If litCityStZip.Text = ",,," Then
                litCityStZip.Text = ""
            End If
            If dt.Rows(0).Item("physicalcountry").ToString = "Select " Then
                litCountry.Text = ""
            Else
                litCountry.Text = dt.Rows(0).Item("physicalcountry").ToString
            End If
            aCoMap.HRef = "http://maps.google.com/maps/place/" & Replace(litAddress.Text, "#", "") & ", " & Replace(litCityStZip.Text, "#", "")  & ", " & Replace(litCountry.Text, "#", "")
            pAddress.Value = dt.Rows(0).Item("physicalstreet").ToString
            pCity.Value = dt.Rows(0).Item("physicalcity").ToString
            pState.Value = dt.Rows(0).Item("physicalstate").ToString
            pZip.Value = dt.Rows(0).Item("physicalpostalcode").ToString
            pCountry.ClearSelection()
            If dt.Rows(0).Item("physicalcountry").ToString = "" Then
            Else
                pCountry.Items.FindByValue(dt.Rows(0).Item("physicalcountry").ToString).Selected = True
            End If

            litPhone.Text = dt.Rows(0).Item("phone").ToString
            lnkCoPhoneHdn.InnerHtml = dt.Rows(0).Item("phone").ToString
            lnkCoPhoneHdn.HRef = "skype:" & dt.Rows(0).Item("phone").ToString & "?call"

            pPhone.Value = litPhone.Text
            pFax.Value = dt.Rows(0).Item("phone").ToString

            litIndustry.Text = dt.Rows(0).Item("industry").ToString
            pIndustry.Value = litIndustry.Text

            Dim annrev As Double
            annrev = CDbl(dt.Rows(0).Item("Revenue").ToString)
            If annrev > 999999999 Then
                litRevenue.Text = "$ " & annrev.ToString("#,##0,,,.000 B", CultureInfo.InvariantCulture)
            ElseIf annrev > 999999 Then
                litRevenue.Text = "$ " & annrev.ToString("#,##0,,.000 M", CultureInfo.InvariantCulture)
            ElseIf annrev > 999 Then
                litRevenue.Text = "$ " & annrev.ToString("#,##0,.000 K", CultureInfo.InvariantCulture)
            Else
                litRevenue.Text = "$ " & annrev.ToString
            End If

            'value.ToString("#,##0,,M", CultureInfo.InvariantCulture)
            'value.ToString("#,##0,,,B", CultureInfo.InvariantCulture)
            'value.ToString("#,##0,K", CultureInfo.InvariantCulture)

            pAnnRev.Value = dt.Rows(0).Item("Revenue").ToString

            Dim numinv As Double
            If dt.Rows(0).Item("NumInvoices").ToString = "" Then
                numinv = 0
            Else
                numinv = CDbl(dt.Rows(0).Item("NumInvoices").ToString)
            End If

            If dt.Rows(0).Item("InvRange").ToString = "Select " Then
                litInvoices.Text = "Unknown : [" & numinv.ToString("#,##0, K", CultureInfo.InvariantCulture) & "]"
            Else
                litInvoices.Text = dt.Rows(0).Item("InvRange").ToString & " : [" & numinv.ToString("#,##0, K", CultureInfo.InvariantCulture) & "]"
            End If
            pInvoices.Value = dt.Rows(0).Item("NumInvoices").ToString

            litTicker.Text = dt.Rows(0).Item("Ticker").ToString
            aTicker.HRef = "https://www.google.com/finance?q=" & litTicker.Text
            pTicker.Value = litTicker.Text

            litWebsite.Text = dt.Rows(0).Item("website").ToString
            aWebsite.HRef = "http://" & litWebsite.Text
            pWebsite.Value = litWebsite.Text

            If dt.Rows(0).Item("Tier").ToString = "Select " Then
                litTier.Text = ""
            Else
                litTier.Text = dt.Rows(0).Item("Tier").ToString
            End If
            pTier.ClearSelection()
            Try
                pTier.Items.FindByValue(dt.Rows(0).Item("Tier").ToString).Selected = True
            Catch ex As Exception

            End Try

            If dt.Rows(0).Item("MicroRegion").ToString = "Select " Then
                litMicroRegion.Text = ""
            Else
                litMicroRegion.Text = dt.Rows(0).Item("MicroRegion").ToString
            End If

            pMicroRegion.ClearSelection()
            pMicroRegion.Items.FindByValue(dt.Rows(0).Item("micro_region").ToString).Selected = True

            If dt.Rows(0).Item("status").ToString = "Select " Then
                litStatus.Text = ""
            Else
                litStatus.Text = dt.Rows(0).Item("status").ToString
            End If
            txtacctdesc.InnerHtml = Server.HtmlDecode(dt.Rows(0).Item("description").ToString)

            pStatus.ClearSelection()
            pStatus.Items.FindByValue(dt.Rows(0).Item("atstrowid").ToString).Selected = True

            pOwnership.ClearSelection()
            pOwnership.Items.FindByValue(dt.Rows(0).Item("Ownership").ToString).Selected = True
            pEmployees.Value = dt.Rows(0).Item("number_of_employees").ToString
            pInvRange.ClearSelection()
            Try
                pInvRange.Items.FindByValue(dt.Rows(0).Item("InvRange").ToString).Selected = True
            Catch ex As Exception
                pInvRange.ClearSelection()
            End Try
            'pInterest.Items.FindByValue("1").Selected = True
            pOwner.ClearSelection()
            Try
                pOwner.Items.FindByValue(dt.Rows(0).Item("OwnerId").ToString).Selected = True
            Catch ex As Exception
            End Try
            ddlOwner.ClearSelection()
            Try
                ddlOwner.Items.FindByValue(dt.Rows(0).Item("OwnerId").ToString).Selected = True
            Catch ex As Exception
            End Try
            hdnownerId.value = dt.Rows(0).Item("OwnerId").ToString
            pdtVal.Value = dt.Rows(0).Item("DateValidated").ToString
            pDescription.Value = Server.HtmlDecode(dt.Rows(0).Item("description").ToString)
            pCrtdByOn.Text = dt.Rows(0).Item("CreatedBy").ToString & " " & dt.Rows(0).Item("CreatedOn").ToString
            pUptdByOn.Text = dt.Rows(0).Item("ModifiedBy").ToString & " " & dt.Rows(0).Item("ModifiedDate").ToString

        Catch ex As Exception
            SSManager.LogAppError(ex, "BindAcct")
        End Try

    End Sub

    Protected Sub BindCt()
        Try
            Dim db As New DBHelperClient
            Dim Ctdt As DataTable
            Dim parms(3) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_sFlag", "S")
            parms(1) = New DBHelperClient.Parameters("p_account_id", hdnaccountId.Value)
            parms(2) = New DBHelperClient.Parameters("p_guid_account_id", hdnguidaccountId.Value)
            parms(3) = New DBHelperClient.Parameters("p_contact_id", hdncontactId.Value)
            Ctdt = db.DataAdapter(CommandType.StoredProcedure, "SP_GetContacts", parms).Tables(0)

            pCtCoName.Text = lithdrCo.Text
            pCtSalutation.ClearSelection()
            pCtSalutation.Items.FindByValue(Ctdt.Rows(0).Item("salutation").ToString).Selected = True

            If Ctdt.Rows(0).Item("salutation").ToString = "Select " Then
                litCtName.Text = Ctdt.Rows(0).Item("Dfirstname").ToString & " " & Ctdt.Rows(0).Item("Dlastname").ToString
            Else 
                litCtName.Text = Ctdt.Rows(0).Item("salutation").ToString & " " & Ctdt.Rows(0).Item("Dfirstname").ToString & " " & Ctdt.Rows(0).Item("Dlastname").ToString
            End If
            pCtFirstName.Value = Ctdt.Rows(0).Item("firstname").ToString
            pCtLastName.Value = Ctdt.Rows(0).Item("lastname").ToString

            litCtTitle.Text = Ctdt.Rows(0).Item("title").ToString
            pCtTitle.Value = litCtTitle.Text

            litCtPersona.Text = Ctdt.Rows(0).Item("PersonaDescription").ToString

            litCtAddress.Text = Ctdt.Rows(0).Item("mailingstreet").ToString
            pCtAddress.Value = litCtAddress.Text

            litCtCityStZip.Text = Ctdt.Rows(0).Item("mailingcity").ToString & ", " & Ctdt.Rows(0).Item("mailingstate").ToString & _
                                 ", " & Ctdt.Rows(0).Item("mailingpostalcode").ToString
            pCtCity.Value = Ctdt.Rows(0).Item("mailingcity").ToString
            pCtState.Value = Ctdt.Rows(0).Item("mailingstate").ToString
            pCtZip.Value = Ctdt.Rows(0).Item("mailingpostalcode").ToString

            litCtCountry.Text = Ctdt.Rows(0).Item("mailingcountry").ToString
            pCtCountry.ClearSelection()
            pCtCountry.Items.FindByValue(Ctdt.Rows(0).Item("country").ToString).Selected = True
            
            litCtPhone.Text = Ctdt.Rows(0).Item("phone").ToString
            pCtPhone.Value = litCtPhone.Text
            lnkCtPhoneHdn.InnerHtml = Ctdt.Rows(0).Item("phone").ToString
            lnkCtPhoneHdn.HRef = "skype:" & Ctdt.Rows(0).Item("phone").ToString & "?call"
            litCtPhoneExtn.Text = Ctdt.Rows(0).Item("extension").ToString
            pCtPhoneExtn.Value = litCtPhoneExtn.Text

            pCtFax.Value = Ctdt.Rows(0).Item("phone").ToString
            pCtPBX.Value = Ctdt.Rows(0).Item("pbx").ToString

            ddlEmailStatus.ClearSelection()
            Try
                ddlEmailStatus.Items.FindByValue(Ctdt.Rows(0).Item("emailstatusid").ToString).Selected = True
            Catch ex As Exception

            End Try


			If Ctdt.Rows(0).Item("emailoptout").ToString = "1" Then
                optoutcolor.Visible = True
                optoutcolor.Attributes.Add("title", "Contact has opted out from Email Notifications.")
                optoutcolor.Attributes.Add("class", "sprite sprite-4")
                btnInclude.Visible = True
                btnExlude.Visible = False
                imgex.Visible = False
            Else
                btnInclude.Visible = False
                btnExlude.Visible = True
                imgex.Visible = True
            End If

			''----- added 21-Mar-2016 ------
            ''calculate the color!
            'Dim strCallColor As String = ""
            'Dim ts As TimeSpan
            'If CtDt.Rows(0).Item("callveridate").ToString = "" Then
            '    ts = DateTime.Now - DateTime.Now.AddYears(-10)
            'Else
            '    ts = DateTime.Now - Convert.ToDateTime(CtDt.Rows(0).Item("callveridate").ToString)
            'End If
            'If (ts.TotalDays / 30) < 6 Then
            '    strCallColor = "1"
            'ElseIf ((ts.TotalDays / 30) > 6) And ((ts.TotalDays / 30) < 12) Then
            '    strCallColor = "2"
            'ElseIf ((ts.TotalDays / 30) > 12) And ((ts.TotalDays / 30) < 24) Then
            '    strCallColor = "3"
            'ElseIf (ts.TotalDays / 30) > 24 Then
            '    strCallColor = "4"
            'End If

            'phonecolor.Attributes.Add("class", "sprite sprite-" + strCallColor)

            'If strCallColor = "1" Then
            '    phonecolor.Attributes.Add("title", "Verified in last 6 Months" + " : " + CtDt.Rows(0).Item("callveridate").ToString)
            'ElseIf strCallColor = "2" Then
            '    phonecolor.Attributes.Add("title", "Verified in last 6 to 12 Months" + " : " + CtDt.Rows(0).Item("callveridate").ToString)
            'ElseIf strCallColor = "3" Then
            '    phonecolor.Attributes.Add("title", "Verified in last 2 Years" + " : " + CtDt.Rows(0).Item("callveridate").ToString)
            'Else
            '    phonecolor.Attributes.Add("title", "Not verified in last 2 Years" + " : " + CtDt.Rows(0).Item("callveridate").ToString)
            'End If


            'calculate the color!
            'Dim strEmailColor As String = ""
            'Dim ts2 As TimeSpan
            'If CtDt.Rows(0).Item("emailveridate").ToString = "" Then
            '    ts2 = DateTime.Now - DateTime.Now.AddYears(-10)
            'Else
            '    ts2 = DateTime.Now - Convert.ToDateTime(CtDt.Rows(0).Item("emailveridate").ToString)
            'End If
            'If (ts2.TotalDays / 30) < 6 Then
            '    strEmailColor = "1"
            'ElseIf ((ts2.TotalDays / 30) > 6) And ((ts2.TotalDays / 30) < 12) Then
            '    strEmailColor = "2"
            'ElseIf ((ts2.TotalDays / 30) > 12) And ((ts2.TotalDays / 30) < 24) Then
            '    strEmailColor = "3"
            'ElseIf (ts2.TotalDays / 30) > 24 Then
            '    strEmailColor = "4"
            'End If
            'emailinfocolor.Attributes.Add("class", "sprite sprite-" + strEmailColor)

            'If strEmailColor = "1" Then
            '    emailinfocolor.Attributes.Add("title", "Verified in last 6 Months" + " : " + CtDt.Rows(0).Item("emailveridate").ToString)
            'ElseIf strEmailColor = "2" Then
            '    emailinfocolor.Attributes.Add("title", "Verified in last 6 to 12 Months" + " : " + CtDt.Rows(0).Item("emailveridate").ToString)
            'ElseIf strEmailColor = "3" Then
            '    emailinfocolor.Attributes.Add("title", "Verified in last 2 Years" + " : " + CtDt.Rows(0).Item("emailveridate").ToString)
            'Else
            '    emailinfocolor.Attributes.Add("title", "Not verified in last 2 Years" + " : " + CtDt.Rows(0).Item("emailveridate").ToString)
            'End If
            '----- added 21-Mar-2016 --------------



            litCtCellPhone.Text = Ctdt.Rows(0).Item("cellphone").ToString
            pCtCell.Value = litCtCellPhone.Text
            lnkCtCellPhoneHdn.InnerHtml = Ctdt.Rows(0).Item("cellphone").ToString
            lnkCtCellPhoneHdn.HRef = "skype:" & Ctdt.Rows(0).Item("cellphone").ToString & "?call"


            litCtEmail.Text = Ctdt.Rows(0).Item("email").ToString
            litEmailStatus.Text = Ctdt.Rows(0).Item("EmailStatus").ToString
            pCtEmail.Value = litCtEmail.Text
            pQESendto.Text = litCtEmail.Text

			'----   added 28-Feb-2017   check for excluded email.
            Dim strSQL As String = ""
            Dim iCount As Integer = 0
            strSQL = " select count(*) from tblEmailExclude where email = '" & CtDt.Rows(0).Item("Demail").ToString & "'"
            iCount = db.ExecuteScalar(CommandType.Text, strSQL)
            If iCount > 0 Then
                btnInclude.Visible = True
            End If
            '----   added 28-Feb-2017   check for excluded email.

            pCtLinkedIn.Value = Ctdt.Rows(0).Item("linkedin_url").ToString
            If pCtLinkedIn.Value = "" Then
                aLinkedIn.HRef = ""
            Else
                aLinkedIn.HRef = pCtLinkedIn.Value
            End If

            pCtLevel.ClearSelection()
            pCtLevel.Items.FindByValue(Ctdt.Rows(0).Item("level").ToString).Selected = True

            pCtArea.ClearSelection()
            pCtArea.Items.FindByValue(Ctdt.Rows(0).Item("area").ToString).Selected = True
            pCtStatus.ClearSelection()
            pCtStatus.Items.FindByValue(Ctdt.Rows(0).Item("STATUS").ToString).Selected = True
            Try
                pCtPersona.Items.FindByValue(Ctdt.Rows(0).Item("persona").ToString).Selected = True
            Catch ex As Exception
            End Try
            pCtCrtdByOn.Text = Ctdt.Rows(0).Item("CreatedBy").ToString & " " & Ctdt.Rows(0).Item("CreatedOn").ToString
            pCtUptdByOn.Text = Ctdt.Rows(0).Item("ModifiedBy").ToString & " " & Ctdt.Rows(0).Item("ModifiedDate").ToString
            pCtDescription.Value = Ctdt.Rows(0).Item("description").ToString

            If Ctdt.Rows(0).Item("nlt").ToString = "1" Then
                lnkNLT.Text = "<img src='images/tick.png' />"
                lnkNLT.ToolTip = "Back again"
                lnknlt.OnClientClick = ""
            Else
                lnkNLT.Text = "<img src='images/Delete.png' />"
                lnkNLT.ToolTip = "No longer there"
                lnknlt.OnClientClick = "return delconfirm();"
            End If

            BindAcctOthers()

            BindInv()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub


    Protected Sub rptAcctOthrs_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) _
        Handles rptSys.ItemCommand, rptAS.ItemCommand, rptPR.ItemCommand

        Try
            If e.CommandName = "Del" Then
                Dim strSQL As String
                strSQL = "Delete from tblacctothers where acctothrowid = @p_acctothrowid "
                Dim db As New DBHelperClient
                Dim parms(0) as DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("p_acctothrowid", e.CommandArgument)
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
                BindAcctOthers()
            End If
        Catch ex As Exception
            SSManager.LogAppError(ex, "rptAcctOthrs_ItemCommand")
        End Try

    End Sub

    Private Sub lnkQESaveEmail_Click(sender As Object, e As EventArgs) Handles lnkQESaveEmail.Click
        Try
            Dim mailMsg As New MailMessage
            mailMsg.From = New System.Net.Mail.MailAddress(Session("email"))
            mailMsg.Subject = pQESubject.Text

            If pQESendTo.Text.Trim <> "" Then
                pQESendTo.Text = pQESendTo.Text.Trim.Trim(";")
                For index As Integer = 0 To pQESendTo.Text.Split(";").Length - 1
                    mailMsg.To.Add(New System.Net.Mail.MailAddress(pQESendTo.Text.Split(";")(index)))
                Next
            End If

            mailMsg.IsBodyHtml = True
            mailMsg.Headers.Add("X-MC-Track", "opens,clicks_all")
            mailMsg.Headers.Add("X-MC-AutoHtml", "1")
            'mailMsg.Body = editor.Text
            mailMsg.Body = pQEEmailBody.Value

            Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
            smtp.Port = "587"
            smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "1")

            smtp.Send(mailMsg)

            Dim builder As New MailBuilder()
            builder.Subject = pQESubject.Text
            builder.[To].Add(New Headers.MailBox(pQESendto.Text, pQESendto.Text))
            'builder.Html = editor.Text
            builder.Html = pQEEmailBody.Value
            Dim email As IMail = builder.Create()

            Dim strSQL As String
            Dim dt As New DataTable
            Dim db As New DBHelperClient

            strSQL = " SELECT CU.Email, CU.EmailPassword, CU.IncomingServer, CU.IncomingServerPort, " & _
                    " CU.OutgoingServer, CU.OutgoingServerPort,  CU.UseSSLIncoming, CU.user_id, U.UserName " & _
                    " FROM tbluser CU " & _ 
                    " INNER JOIN client_master.users U ON CU.user_id = U.userID " & _
                    " WHERE CU.user_id = " & Session("user_id")
            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)

            Dim IncomingServer As String
            Dim IncomingServerPort As String
            Dim OutgoingUserName As String
            Dim OutgoingPassword As String
            Dim OutgoingServer As String
            Dim OutgoingServerPort As String
            Dim UseSSLOutgoing As String

            IncomingServer = dt.Rows(0).Item("IncomingServer").ToString
            IncomingServerPort = dt.Rows(0).Item("IncomingServerPort").ToString
            OutgoingUserName = dt.Rows(0).Item("Email").ToString
            OutgoingPassword = Decrypt(dt.Rows(0).Item("EmailPassword").ToString)
            OutgoingServer = dt.Rows(0).Item("OutgoingServer").ToString
            OutgoingServerPort = dt.Rows(0).Item("OutgoingServerPort").ToString
            UseSSLOutgoing = IIf(dt.Rows(0)("UseSSLIncoming") Is System.DBNull.Value, 0, dt.Rows(0)("UseSSLIncoming"))

            Using imap As New Imap()
                imap.Connect(IncomingServer, IncomingServerPort, True) ' or ConnectSSL for SSL
                imap.Login(OutgoingUserName.Substring(0, OutgoingUserName.IndexOf("@")), OutgoingPassword)
                imap.UploadMessage("Sent Items", email)
                imap.Close()
            End Using

            pEmailBody.Value = ""
            pSubject.Text = ""
            pQEEmailBody.Value = ""
            pQESubject.Text = ""
            Dim dbMRM As New DBHelperClient
            Dim dt1 As New DataTable
                
            strSQL = " Select EmailSignature from tbluser where Email = '" & Session("email") & "'"
            'editor.Text = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
            dt1 = dbMRM.DataAdapter(CommandType.Text, strSQL).Tables(0)
            If dt1.Rows.Count > 0 Then
                pQEEmailBody.Value = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
                pEmailBody.Value = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
            End If
        Catch ex As Exception
            SSManager.LogAppError(ex, "lnkQESaveEmail_Click")
        End Try
    End Sub

    Private Sub lnkNLT_Click(sender As Object, e As EventArgs) Handles lnkNLT.Click
        Try
            Dim strSQL As String
            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_t_contact_id", hdncontactId.Value)

            If lnkNLT.ToolTip="No longer there" Then
                strSQL = "Update tblcontact set NLT = 1 where t_contact_id = @p_t_contact_id "
            Else
                strSQL = "Update tblcontact set NLT = 0 where t_contact_id = @p_t_contact_id "
            End If
            Dim db As New DBHelperClient
            db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
            Response.Redirect("Account.aspx?BindType=A&AcctId=" & Request.QueryString("AcctId") , False)

        Catch ex As Exception
            SSManager.LogAppError(ex, "lnkNLT_Click")
        End Try
    End Sub


    Private Sub pTemplateType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles pTemplateType.SelectedIndexChanged
        Try
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_EBTemplateTypeID", pTemplateType.SelectedValue)
            strSQL = "Select '' as EmailTemplateID, ' an Email Template' as emailtemplatename  Union Select EmailTemplateID , emailtemplatename  from tblEmailTemplate Where EBTemplateTypeID = @p_EBTemplateTypeID order by emailtemplatename "
            ddlSubject.DataSource = Nothing
            ddlSubject.DataBind()
            ddlSubject.DataSource = db.DataAdapter(CommandType.Text,strSQL, parms).Tables(0)
            ddlSubject.DataBind()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('newemail();',500);", True)
        Catch ex As Exception
            SSManager.LogAppError(ex, "pTemplateType_SelectedIndexChanged")
        End Try
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function SetMessage(ByVal EmailTemplateID As String, ByVal SendTo As String, ByVal ContactId As String, ByVal acctid As String) As String()
        Try
            Dim strArr As String() = New String(2) {}

            Dim db As New DBHelperClient
            Dim parms2(1) As DBHelperClient.Parameters
            parms2(0) = New DBHelperClient.Parameters("p_EmailTemplateID", EmailTemplateID)
            If SendTo.Split(";").Length = 1 Then
                parms2(1) = New DBHelperClient.Parameters("p_SendTo", SendTo)
            Else
                parms2(1) = New DBHelperClient.Parameters("p_SendTo", SendTo.Split(";")(0))
            End If

            Dim ds As DataSet = db.DataAdapter(CommandType.StoredProcedure, "SP_GetSSEmailTemplates", parms2)


            Dim strBody As String = ds.Tables(0).Rows(0)("Body").ToString
            Dim strSubject As String = ds.Tables(0).Rows(0)("Subject").ToString

            strBody = strBody.Replace("::UserNameFirst::", HttpContext.Current.Session("FirstName"))
            strBody = strBody.Replace("::UserNameFull::", HttpContext.Current.Session("FirstName") & " " & HttpContext.Current.Session("LastName"))

            If ds.Tables(1).Rows.Count > 0 Then
                For index As Integer = 0 To ds.Tables(1).Columns.Count - 1
                    If strBody.Contains("::" + ds.Tables(1).Columns.Item(index).ColumnName + "::") Then
                        strBody = strBody.Replace("::" + ds.Tables(1).Columns.Item(index).ColumnName + "::", ds.Tables(1).Rows(0).Item(index))
                    End If
                    If strSubject.Contains("::" + ds.Tables(1).Columns.Item(index).ColumnName + "::") Then
                        strSubject = strSubject.Replace("::" + ds.Tables(1).Columns.Item(index).ColumnName + "::", ds.Tables(1).Rows(0).Item(index))
                    End If
                Next
            End If

            'populate signature
            Dim dsSig As New DataSet
            Dim TempSig As String
            dsSig = db.DataAdapter(CommandType.Text, "SELECT EmailSignature FROM tblUser Where user_id = " & HttpContext.Current.Session("user_id"))
            If dsSig.Tables(0).Rows.Count > 0 Then
                TempSig = "<BR/>" & dsSig.Tables(0).Rows(0).Item(0).ToString
            Else
                TempSig = ""
            End If

            If strBody.Contains("::Signature::") Or strBody.Contains("::SIGNATURE::") Or strBody.Contains("::signature::") Then
                strBody = strBody.Replace("::Signature::", TempSig).Replace("::SIGNATURE::", TempSig).Replace("::signature::", TempSig)
            Else
                strBody = strBody & TempSig
            End If

            '--------------------------     added 18-Feb-2018   ESTIMATED SAVINGS START --------------------------
            Dim strsursymbol As String = ""
            If strBody.Contains("::SavingsCalc") Then
                If strBody.Contains("::SavingsCalcUSD::") Then
                    strsursymbol = "$"
                End If
                If strBody.Contains("::SavingsCalcGBP::") Then
                    strsursymbol = "£"
                End If
                If strBody.Contains("::SavingsCalcEURO::") Then
                    strsursymbol = "€"
                End If
                '-------------------
                Dim db2 As New DBHelperClient
                Dim parms22(1) As DBHelperClient.Parameters
                parms22(0) = New DBHelperClient.Parameters("v_t_account_id", acctid)
                parms22(1) = New DBHelperClient.Parameters("v_curr_symbol", strsursymbol)
                Dim dsestsavings As DataSet = db.DataAdapter(CommandType.StoredProcedure, "sp_emailblastsavingsest", parms22)
                '-----------------------
                If dsestsavings.Tables.Count > 0 Then
                    If dsestsavings.Tables(0).Rows.Count > 0 Then
                        If strBody.Contains("::SavingsCalcUSD::") Then
                            strBody = strBody.Replace("::SavingsCalcUSD::", dsestsavings.Tables(0).Rows(0)("v_text"))
                        End If
                        If strBody.Contains("::SavingsCalcGBP::") Then
                            strBody = strBody.Replace("::SavingsCalcGBP::", dsestsavings.Tables(0).Rows(0)("v_text"))
                        End If
                        If strBody.Contains("::SavingsCalcEURO::") Then
                            strBody = strBody.Replace("::SavingsCalcEURO::", dsestsavings.Tables(0).Rows(0)("v_text"))
                        End If
                    End If
                End If
            End If
            '--------------------------     added 18-Feb-2018   ESTIMATED SAVINGS END--------------------------
            strArr(0) = strBody
            strArr(1) = strSubject

            Return strArr

        Catch ex As Exception

        End Try

    End Function

    Protected Function Decrypt(ByVal stringToDecrypt As String) As String
        Try
            Dim key() As Byte = {}
            Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

            Dim inputByteArray(stringToDecrypt.Length) As Byte

            key = System.Text.Encoding.UTF8.GetBytes("PVPSCRSTIPL".Substring(0, 8))
            Dim des As New DESCryptoServiceProvider()
            stringToDecrypt = stringToDecrypt.Replace(" ", "+")
            inputByteArray = Convert.FromBase64String(stringToDecrypt)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateDecryptor(key, IV), _
                CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
            Return encoding.GetString(ms.ToArray())
        Catch ex As Exception
        End Try

    End Function

    Private Sub lnkSaveEmail_Click(sender As Object, e As EventArgs) Handles lnkSaveEmail.Click
        Try
            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim dt As New DataTable

            Dim ex As Exception
            Dim mailMsg As New MailMessage
            mailMsg.From = New System.Net.Mail.MailAddress(pSelFrom.SelectedItem.Text)
            If pSelReplyTo.SelectedValue.Trim <> "" Then
                mailMsg.ReplyTo = New System.Net.Mail.MailAddress(pSelReplyTo.SelectedItem.Text)
            End If
            mailMsg.Subject = pSubject.Text
            Dim bytes1 As Byte()
            Dim bytes2 As Byte()
            Dim bytes3 As Byte()
            Dim bytes4 As Byte()

            If FileAttach1.FileName <> "" Then
                Dim br As BinaryReader
                br = New BinaryReader(FileAttach1.PostedFile.InputStream)
                bytes1 = br.ReadBytes(Convert.ToInt32(FileAttach1.PostedFile.InputStream.Length))
                br.Close()
                Dim memStream As New MemoryStream(bytes1)
                mailMsg.Attachments.Add(New System.Net.Mail.Attachment(memStream, FileAttach1.FileName.ToString))
            End If

            If FileAttach2.FileName <> "" Then
                Dim br As BinaryReader
                br = New BinaryReader(FileAttach2.PostedFile.InputStream)
                bytes2 = br.ReadBytes(Convert.ToInt32(FileAttach2.PostedFile.InputStream.Length))
                br.Close()
                Dim memStream As New MemoryStream(bytes2)
                mailMsg.Attachments.Add(New System.Net.Mail.Attachment(memStream, FileAttach2.FileName.ToString))
            End If

            If FileAttach3.FileName <> "" Then
                Dim br As BinaryReader
                br = New BinaryReader(FileAttach3.PostedFile.InputStream)
                bytes3 = br.ReadBytes(Convert.ToInt32(FileAttach3.PostedFile.InputStream.Length))
                br.Close()
                Dim memStream As New MemoryStream(bytes3)
                mailMsg.Attachments.Add(New System.Net.Mail.Attachment(memStream, FileAttach3.FileName.ToString))
            End If

            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_EmailTemplateID", ddlSubject.SelectedValue)
            strSQL = " Select CONCAT(CAST(AttachId AS CHAR(100)),'_',AttachFileName) AS AttachFileName,AttachContentType " & _
                    " ,AttachFileName AS AttachFileNamePlain FROM tblemailtemplateattach WHERE EmailTemplateID = @p_EmailTemplateID "  
            dt = db.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)

            Dim memStreamAtt As MemoryStream
            For i = 0 To dt.Rows.Count - 1
                memStreamAtt = New MemoryStream(System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings("TemplateAttachPath") + Session("client_id") + "\" + dt.Rows(0)("AttachFileName").ToString))
                mailMsg.Attachments.Add(New System.Net.Mail.Attachment(memStreamAtt, dt.Rows(0)("AttachFileNamePlain").ToString))
            Next


            If pSendTo.Text.Trim <> "" Then
                pSendTo.Text = pSendTo.Text.Trim.Trim(";")
                For index As Integer = 0 To pSendTo.Text.Split(";").Length - 1
                    mailMsg.To.Add(New System.Net.Mail.MailAddress(pSendTo.Text.Split(";")(index)))
                Next
            End If

            If pSendCC.Text.Contains("@") Then
                pSendCC.Text = pSendCC.Text.Trim.Trim(";")
                For index As Integer = 0 To pSendCC.Text.Split(";").Length - 1
                    mailMsg.CC.Add(New System.Net.Mail.MailAddress(pSendCC.Text.Split(";")(index)))
                Next
            End If

            mailMsg.IsBodyHtml = True
            mailMsg.Headers.Add("X-MC-Track", "opens,clicks_all")
            mailMsg.Headers.Add("X-MC-AutoHtml", "1")
            mailMsg.Body = pEmailBody.Value

            Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
            smtp.Port = "587"
            smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "1")

            smtp.Send(mailMsg)

            Dim builder As New MailBuilder()
            builder.Subject = pSubject.Text
            builder.[To].Add(New Headers.MailBox(pSendTo.Text, pSendTo.Text))
            builder.Html = pEmailBody.Value

            Dim att As MimeData
            If FileAttach1.FileName <> "" Then
                att = builder.AddAttachment(bytes1)
                att.FileName = FileAttach1.FileName
            End If
            If FileAttach2.FileName <> "" Then
                att = builder.AddAttachment(bytes2)
                att.FileName = FileAttach1.FileName
            End If
            If FileAttach3.FileName <> "" Then
                att = builder.AddAttachment(bytes3)
                att.FileName = FileAttach1.FileName
            End If

            For i = 0 To dt.Rows.Count - 1
				att = builder.AddAttachment(System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings("TemplateAttachPath") + Session("client_id") + "\" + dt.Rows(0)("AttachFileName").ToString))
				att.FileName = dt.Rows(0)("AttachFileNamePlain").ToString
            Next

            Dim email As IMail = builder.Create()

            strSQL = " SELECT CU.Email, CU.EmailPassword, CU.IncomingServer, CU.IncomingServerPort, " & _
                    " CU.OutgoingServer, CU.OutgoingServerPort,  CU.UseSSLIncoming, CU.user_id, U.UserName " & _
                    " FROM tbluser CU " & _ 
                    " INNER JOIN client_master.users U ON CU.user_id = U.userID " & _
                    " WHERE CU.user_id = " & Session("user_id")
            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)

            Dim IncomingServer As String
            Dim IncomingServerPort As String
            Dim OutgoingUserName As String
            Dim OutgoingPassword As String
            Dim OutgoingServer As String
            Dim OutgoingServerPort As String
            Dim UseSSLOutgoing As String

            IncomingServer = dt.Rows(0).Item("IncomingServer").ToString
            IncomingServerPort = dt.Rows(0).Item("IncomingServerPort").ToString
            OutgoingUserName = dt.Rows(0).Item("Email").ToString
            OutgoingPassword = Decrypt(dt.Rows(0).Item("EmailPassword").ToString)
            OutgoingServer = dt.Rows(0).Item("OutgoingServer").ToString
            OutgoingServerPort = dt.Rows(0).Item("OutgoingServerPort").ToString
            UseSSLOutgoing = IIf(dt.Rows(0)("UseSSLIncoming") Is System.DBNull.Value, 0, dt.Rows(0)("UseSSLIncoming"))

            builder.From.Add(New Headers.MailBox(pSelFrom.SelectedItem.Text, dt.Rows(0).Item("UserName").ToString))
			If pSendCC.Text.Contains("@") Then
                pSendCC.Text = pSendCC.Text.Trim.Trim(";")
                For index As Integer = 0 To pSendCC.Text.Split(";").Length - 1
                    email.Cc.Add(New Headers.MailBox(pSendCC.Text.Split(";")(index).ToString()))
                    'builder.Cc.Add(New Headers.MailAddress( pSendCC.Text.Split(";")(index).ToString()))
                Next
            End If
			
            Using imap As New Imap()
                imap.Connect(IncomingServer, IncomingServerPort, True) ' or ConnectSSL for SSL
                imap.Login(OutgoingUserName.Substring(0, OutgoingUserName.IndexOf("@")), OutgoingPassword)
                imap.UploadMessage("Sent Items", email)
                imap.Close()
            End Using

            Dim dbMRM As New DBHelperClient
            Dim parmsa(1) As DBHelperClient.Parameters
            parmsa(0) = New DBHelperClient.Parameters("p_t_account_id", hdnaccountId.Value)
            parmsa(1) = New DBHelperClient.Parameters("p_t_contact_id", hdncontactId.Value)
            strSQL = " UPDATE tblcontact SET lastactivitydate = CURDATE()  WHERE t_account_id = @p_t_account_id AND t_contact_id = @p_t_contact_id " 
            dbMRM.ExecuteScalar(CommandType.Text,strSQL, parmsa)

            strSQL = " UPDATE tblaccount SET lastactivitydate = CURDATE()  WHERE t_account_id = @p_t_account_id "
            dbMRM.ExecuteScalar(CommandType.Text,strSQL, parmsa)

            pEmailBody.Value = ""
            pSubject.Text = ""
            pTemplateType.ClearSelection()
            ddlSubject.ClearSelection()
            pQEEmailBody.Value = ""
            pQESubject.Text = ""
            strSQL = " Select EmailSignature from tbluser where Email = '" & Session("email") & "'"
            'editor.Text = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
            pQEEmailBody.Value = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))
            pEmailBody.Value = "<br/> " & System.Web.HttpUtility.HtmlDecode(dbMRM.ExecuteScalar(CommandType.Text,strSQL))

        Catch ex As Exception
            SSManager.LogAppError(ex, "lnkSaveEmail_Click")
        End Try
    End Sub

    'Protected Sub CopyToIMAP(byval Receiver As String, byval Subject As String, ByVal Body As string) 
    '    Try
    '        Dim db As New DBHelperClient
    '        Dim strSQL As String
    '        strSQL = " SELECT CU.Email, CU.EmailPassword, CU.IncomingServer, CU.IncomingServerPort, " & _
    '                " CU.OutgoingServer, CU.OutgoingServerPort,  CU.UseSSLIncoming, CU.user_id, U.UserName " & _
    '                " FROM tbluser CU " & _ 
    '                " INNER JOIN client_master.users U ON CU.user_id = U.userID " & _
    '                " WHERE CU.user_id = " & Session("user_id")
    '        Dim dt As New DataTable
    '        dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)

    '        Dim IncomingServer As String
    '        Dim IncomingServerPort As String
    '        Dim OutgoingUserName As String
    '        Dim OutgoingPassword As String
    '        Dim OutgoingServer As String
    '        Dim OutgoingServerPort As String
    '        Dim UseSSLOutgoing As String

    '        IncomingServer = dt.Rows(0).Item("IncomingServer").ToString
    '        IncomingServerPort = dt.Rows(0).Item("IncomingServerPort").ToString
    '        OutgoingUserName = dt.Rows(0).Item("Email").ToString
    '        OutgoingPassword = Decrypt(dt.Rows(0).Item("EmailPassword").ToString)
    '        OutgoingServer = dt.Rows(0).Item("OutgoingServer").ToString
    '        OutgoingServerPort = dt.Rows(0).Item("OutgoingServerPort").ToString
    '        UseSSLOutgoing = IIf(dt.Rows(0)("UseSSLIncoming") Is System.DBNull.Value, 0, dt.Rows(0)("UseSSLIncoming"))

    '        Dim builder As New MailBuilder()
    '        builder.Subject = Subject
    '        builder.From.Add(New Headers.MailBox(pSelFrom.SelectedItem.Text, dt.Rows(0).Item("UserName").ToString))
    '        builder.[To].Add(New Headers.MailBox(Receiver, Receiver))
    '        builder.Html = Body

    '        Dim att As MimeData

    '        Dim bytes1 As Byte()
    '        Dim bytes2 As Byte()
    '        Dim bytes3 As Byte()

    '        If FileAttach1.FileName <> "" Then
    '            Dim br As BinaryReader
    '            br = New BinaryReader(FileAttach1.PostedFile.InputStream)
    '            bytes1 = br.ReadBytes(Convert.ToInt32(FileAttach1.PostedFile.InputStream.Length))
    '            br.Close()
    '            att = builder.AddAttachment(bytes1)
    '            att.FileName = FileAttach1.FileName
    '        End If

    '        If FileAttach2.FileName <> "" Then
    '            Dim br As BinaryReader
    '            br = New BinaryReader(FileAttach2.PostedFile.InputStream)
    '            bytes2 = br.ReadBytes(Convert.ToInt32(FileAttach2.PostedFile.InputStream.Length))
    '            br.Close()
    '            att = builder.AddAttachment(bytes2)
    '            att.FileName = FileAttach1.FileName
    '        End If

    '        If FileAttach3.FileName <> "" Then
    '            Dim br As BinaryReader
    '            br = New BinaryReader(FileAttach3.PostedFile.InputStream)
    '            bytes3 = br.ReadBytes(Convert.ToInt32(FileAttach3.PostedFile.InputStream.Length))
    '            br.Close()
    '            att = builder.AddAttachment(bytes3)
    '            att.FileName = FileAttach1.FileName
    '        End If
                
    '        strSQL = " Select CONCAT(CAST(AttachId AS CHAR(100)),'_',AttachFileName) AS AttachFileName,AttachContentType " & _
    '                " ,AttachFileName AS AttachFileNamePlain FROM tblemailtemplateattach WHERE EmailTemplateID = " &  ddlSubject.SelectedValue
    '        dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)

    '        If dt.Rows.Count > 0 Then
    '            att = builder.AddAttachment(System.IO.File.ReadAllBytes(ConfigurationManager.AppSettings("TemplateAttachPath") + Session("client_id") + "\" + dt.Rows(0)("AttachFileName").ToString))
    '            att.FileName = dt.Rows(0)("AttachFileNamePlain").ToString
    '        End If

    '        Dim email As IMail = builder.Create()
    '        Using imap As New Imap()
    '            imap.Connect(IncomingServer, IncomingServerPort, True) ' or ConnectSSL for SSL
    '            imap.Login(OutgoingUserName.Substring(0, OutgoingUserName.IndexOf("@")), OutgoingPassword)
    '            ' The name of the folder depends on your IMAP server
    '            imap.UploadMessage("Sent Items", email)
    '            imap.Close()
    '        End Using

    '    Catch ex As Exception
    '    End Try
 

    'End Sub

	Protected Sub updateemail(ByVal sender As Object, ByVal e As System.EventArgs)
        '----   added 28-Feb-2017   check for excluded email.
        Dim db As New DBHelperClient
        Dim strSQL As String = ""
        Dim parms(0) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_email", litCtEmail.Text)
        strSQL = " delete from tblEmailExclude where email = @p_email;update tblcontact set emailoptout = 0 WHERE Email = @p_email;"
        db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
        Response.Redirect("Activities.aspx?BindType=C&AcctId=" & Request.QueryString("AcctId") & "&CtId=" & Request.QueryString("CtId"), False)
        '----   added 28-Feb-2017   check for excluded email.
    End Sub

	Protected Sub excludeemail(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim db As New DBHelperClient
        Dim strSQL As String = ""
        Dim parms(0) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_email", litCtEmail.Text)
        strSQL = " insert into tblEmailExclude (email,reason,createddate) values (@p_email,'optout',CURRENT_TIMESTAMP()) ;update tblcontact set emailoptout = 1 WHERE Email = @p_email;"
        db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
        Response.Redirect("Activities.aspx?BindType=C&AcctId=" & Request.QueryString("AcctId") & "&CtId=" & Request.QueryString("CtId"), False)
    End Sub

    Protected Sub rptInv_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptInv.ItemCommand
        Try
            Dim db As New DBHelperClient
            Dim strSQL As String
            Dim parms(0) as DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_activity_id", e.CommandArgument)
            strSQL = "Delete from tblactivities where activity_id = @p_activity_id "
            db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
            Response.Redirect("Activities.aspx?BindType=C&AcctId=" & Request.QueryString("AcctId") & "&CtId=" & Request.QueryString("CtId"), False)

        Catch ex As Exception

        End Try
    End Sub
End Class

