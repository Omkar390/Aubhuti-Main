Imports System.Data

Partial Class InitiateSP
    Inherits System.Web.UI.Page

    Private Sub InitiateSP_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not Page.IsPostBack Then
            Try
                pUseTracking.Checked = True
                Dim db As New DBHelper
                Dim dt As DataTable
                dt = db.DataAdapter(Data.CommandType.Text, _
                    " Select '0' as userID, ' Select All' as username UNION " & _
                    " Select U.userID, concat(fname, ' ', lname) as username " & _
                    " FROM users U INNER JOIN tbluserclient UC ON UC.userid = U.userID " & _
                    " WHERE U.Active = 1 and  UC.client_id = " & Session("client_id") & _
                    " order by username").Tables(0)
                ptarSelOwner.DataSource = dt
                ptarSelOwner.DataBind()
                ptarSelOwner.ClearSelection
                BindTemplates()

                plistSelOwner.DataSource = dt
                plistSelOwner.DataBind()
                plistSelOwner.Items.FindByValue(Session("user_id")).Selected = True

                BindData()
                litSelList.Text ="No List Selected"
                litSelTemplate.Text ="No Process Selected"
                BindLists()

            Catch ex As Exception
                lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
                lblErrorMsg.Visible = True
                SSManager.LogAppError(ex, "InitiateSP_Load")
            End Try
        End If

    End Sub

    Protected Sub BindData()

        Dim db As New DBHelperClient
        Dim strSQL As String
        strSQL = " SELECT initiated, num_open, num_open_today, num_closed, num_closed_today, num_skipped, num_skipped_today, open_proc  FROM ( " & _
                 " SELECT  DISTINCT U.username AS initiated, U.userid, " & _
                 " SUM(CASE WHEN (COALESCE(RPDD.procestatus,0) = 1 OR COALESCE(RPDD.procestatus,0) = 5) THEN 0 ELSE 1 END) AS num_open, " & _
                 " SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) <> 1 AND RPD.RPDate > DATE_ADD(CURDATE(),INTERVAL -7 DAY) AND RPD.RPDate <= CURDATE() THEN 1 ELSE 0 END) AS num_open_today, " & _
                 " SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =1 THEN 1 ELSE 0 END) AS num_closed, " & _
                 " SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =1 AND CURDATE() = RPD.RPDate THEN 1 ELSE 0 END) AS num_closed_today, " & _
                 " SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =5 THEN 1 ELSE 0 END) AS num_skipped, " & _
                 " SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =5 AND  CURDATE() = RPD.RPDate THEN 1 ELSE 0 END) AS num_skipped_today " & _
                 " FROM  tblRunProcess AS RP " & _
                 " INNER JOIN client_master.users AS U ON RP.CreatedBy = U.userid " & _
                 " INNER JOIN tblRunProcessDay AS RPD ON RP.RPID = RPD.RPID " & _
                 " INNER JOIN tblRunProcessDayDetail AS RPDD ON RPD.RPDID = RPDD.RPDID " & _
                 " GROUP BY U.username, U.userid ) AS A " & _
                 " LEFT OUTER JOIN ( " & _
                 " SELECT COUNT(RP.RPID) AS open_proc, U.UserName " & _
                 " FROM tblRunProcess RP  " & _
                 " INNER JOIN tblProcess P ON RP.PID = P.PID  " & _
                 " INNER JOIN tblListMaster L ON L.ListID = RP.ListID  " & _
                 " LEFT OUTER JOIN client_master.users U ON RP.createdby = U.userID " & _
                 " INNER JOIN (SELECT COUNT(*) AS Cnt, ListID FROM tblListAC GROUP BY ListID) AS LAC  ON LAC.ListId = L.ListId  " & _
                 " WHERE RPID IN   (  " & _
	             "   SELECT RP.RPID  " & _
	             "   FROM tblRunProcess AS RP  " & _
	             "   INNER JOIN client_master.users AS U ON RP.CreatedBy = U.userid  " & _
	             "   INNER JOIN tblRunProcessDay AS RPD ON RP.RPID = RPD.RPID  " & _
	             "   INNER JOIN tblRunProcessDayDetail AS RPDD ON RPD.RPDID = RPDD.RPDID  " & _
	             "   GROUP BY RP.RPID " & _
	             "   HAVING SUM(CASE WHEN COALESCE(RPDD.procestatus,0) <> 1 THEN 1 ELSE 0 END) > 0   ) " & _
                 "  GROUP BY U.UserName " & _
                 " ) AS B ON A.initiated = B.UserName "
        rptRunProcesses.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
        rptRunProcesses.DataBind()

        strSQL = " SELECT L.ListID, DATE_FORMAT(PStartDate , '%m/%d/%Y') as PStartDate, PName, ListName,  Cnt AS ListCnt, RP.RPID, 'Delete' AS DelIns, U.UserName AS ISR " & _
                 " FROM tblRunProcess RP " & _
                 " INNER JOIN tblProcess P ON RP.PID = P.PID " & _
                 " INNER JOIN tblListMaster L ON L.ListID = RP.ListID " & _
                 " LEFT OUTER JOIN client_master.users U ON RP.createdby = U.userID " & _
                 " INNER JOIN (SELECT COUNT(*) AS Cnt, ListID FROM tblListAC GROUP BY ListID) AS LAC " & _
                 " ON LAC.ListId = L.ListId " & _
                 " WHERE RPID IN " & _
                 "  ( " & _
                 "      SELECT RP.RPID " & _
                 "      FROM tblRunProcess AS RP " & _
                 "      INNER JOIN client_master.users AS U ON RP.CreatedBy = U.userid " & _
                 "      INNER JOIN tblRunProcessDay AS RPD ON RP.RPID = RPD.RPID " & _
                 "      INNER JOIN tblRunProcessDayDetail AS RPDD ON RPD.RPDID = RPDD.RPDID " & _
                 "      GROUP BY RP.RPID " & _
                 "      HAVING SUM(CASE WHEN COALESCE(RPDD.procestatus,0) <> 1 THEN 1 ELSE 0 END) > 0 " & _
                 "  ) " & _
                 " ORDER BY PStartDate "
        rptOpenProcess.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
        rptOpenProcess.DataBind()

        strSQL = " SELECT L.ListID, DATE_FORMAT(PStartDate , '%m/%d/%Y') as PStartDate, PName, ListName,  Cnt AS ListCnt, RP.RPID " & _
                "   FROM tblRunProcess RP " & _
                "   INNER JOIN tblProcess P ON RP.PID = P.PID " & _
                "   INNER JOIN tblListMaster L ON L.ListID = RP.ListID " & _
                "   INNER JOIN (SELECT COUNT(*) AS Cnt, ListID FROM tblListAC GROUP BY ListID) AS LAC " & _
                "   ON LAC.ListId = L.ListId " & _
                "   WHERE RPID IN " & _
                "    ( " & _
                "       SELECT RP.RPID " & _
                "       FROM tblRunProcess AS RP " & _
                "       INNER JOIN client_master.users AS U ON RP.CreatedBy = U.userid " & _
                "       INNER JOIN tblRunProcessDay RPD ON RP.RPID = RPD.RPID " & _
                "       INNER JOIN tblRunProcessDayDetail RPDD ON RPD.RPDID = RPDD.RPDID " & _
                "       GROUP BY RP.RPID " & _
                "       HAVING SUM(CASE WHEN COALESCE(RPDD.procestatus,0) <> 1 THEN 1 ELSE 0 END) = 0 " & _
                "   ) " & _
                "   ORDER BY PStartDate "

        rptClosedProcess.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
        rptClosedProcess.DataBind()

    End Sub

    Protected Sub BindLists()

        Dim db As New DBHelperClient
        Dim strSQL As String
        Dim parms(1) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_Owner", plistSelOwner.SelectedValue )
        parms(1) = New DBHelperClient.Parameters("p_listname", "%" & txtSrchList.Value & "%")
        strSQL = " Select M.id, M.listid, listtype, listname, M.description, CONCAT(fname, ' ', lname) AS username, COUNT(AC.listid) AS Items " & _
                " From tbllistmaster M " & _
                " Left Outer Join tbllistac AC on M.listid = AC.listid " & _
                " Inner Join tblaccount A on AC.t_account_id = A.t_account_id " & _
                " Left Outer Join client_master.users U on M.ownerId = U.userid " & _
                " where listtype = 'Contact' and COALESCE(A.isdeleted, 0) = 0 "
        If plistSelOwner.SelectedValue <> 0 Then 
            strSQL = strSQL & " and M.ownerId = @p_Owner " 
        End If
        If txtSrchList.Value <> "" Then
            strSQL = strSQL & " and M.listname like @p_listname  "
        End If
        strSQL = strSQL & " and (M.arch is NULL or M.arch = '0') " & _
                " Group By M.listid, listtype, listname, M.description, M.ownerid, CONCAT(fname, ' ', lname) " & _
                " order by listtype, listname "

        rptList.DataSource=db.DataAdapter(CommandType.Text,strSQL, parms)
        rptList.DataBind()

    End Sub

    Protected Sub BindTemplates()

        Dim db As New DBHelperClient
        Dim parms(1) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_Owner", ptarSelOwner.SelectedValue )
        parms(1) = New DBHelperClient.Parameters("p_PName", "%" & txtSrchTar.Value & "%")
        Dim strSQL As String
        strSQL = " SELECT PID, PName, PDays, U.username " & _
                " FROM tblProcess P " & _
                " INNER JOIN client_master.users AS U ON P.Owner = U.userid " & _
                " WHERE P.Active = 'Y' "
        If ptarSelOwner.SelectedValue <> 0 Then 
            strSQL = strSQL & " And Owner = @p_Owner " 
        End If
        If txtSrchTar.Value <> "" Then
            strSQL = strSQL & " and PName like @p_PName " 
        End If
        strSQL = strSQL & " ORDER BY PName "
        rptTarget.DataSource=db.DataAdapter(CommandType.Text,strSQL, parms)
        rptTarget.DataBind()

    End Sub

    Private Sub lnkSrchList_Click(sender As Object, e As EventArgs) Handles lnkSrchList.Click,plistSelOwner.SelectedIndexChanged
    
        Try
            BindLists()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('showList();',500);", True)            
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "lnkSrchList_Click")
        End Try

    End Sub
    
    Private Sub lnkSrchTmplt_Click(sender As Object, e As EventArgs) Handles lnkSrchTmplt.Click

        Try
            BindTemplates()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('showTarget();',500);", True)            
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "lnkSrchTmplt_Click")
        End Try

    End Sub

    Private Sub rptList_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptList.ItemCommand

        Try
            If e.CommandName = "Edit"
                hdnSelList.Value = e.CommandArgument
                litSelList.Text = TryCast(e.Item.FindControl("litListName"), Literal).Text 
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('InitiateNew();',500);", True)            
            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptList_ItemCommand")
        End Try

    End Sub

    Private Sub rptTarget_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptTarget.ItemCommand

        Try
            If e.CommandName = "Edit"
                hdnSelTemplate.Value = e.CommandArgument
                litSelTemplate.Text = TryCast(e.Item.FindControl("litTargetName"), Literal).Text
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('InitiateNew();',500);", True)
            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptTarget_ItemCommand")
        End Try

    End Sub

    Private Sub lnkSaveList_Click(sender As Object, e As EventArgs) Handles lnkSaveList.Click

        Try
            Dim db As New DBHelperClient
            Dim parms(5) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            parms(1) = New DBHelperClient.Parameters("p_PID", hdnSelTemplate.Value  )
            parms(2) = New DBHelperClient.Parameters("p_ListId", hdnSelList.Value  )
            parms(3) = New DBHelperClient.Parameters("p_Startdate", pdtVal.Value  )
            parms(4) = New DBHelperClient.Parameters("p_Tracking", pUseTracking.Checked )
            parms(5) = New DBHelperClient.Parameters("p_userId", Session("user_id"))
            db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_CreateProcessDays", parms)

            BindData()

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "lnkSaveList_Click")
        End Try

    End Sub

    Private Sub rptRunProcesses_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptRunProcesses.ItemCommand
        Try
            If e.CommandName = "Open" Then
                Response.Redirect("ReportDetail.aspx?RptName=UserOpenSP&UserName=" & e.CommandArgument, False)
            ElseIf e.CommandName = "OpenToday" Then
                Response.Redirect("ReportDetail.aspx?RptName=UserOpenSPToday&UserName=" & e.CommandArgument, False)
            ElseIf e.CommandName = "Skipped" Then
                Response.Redirect("ReportDetail.aspx?RptName=UserSkippedSP&UserName=" & e.CommandArgument, False)
            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptRunProcesses_ItemCommand")
        End Try
    End Sub

    Private Sub ptarSelOwner_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ptarSelOwner.SelectedIndexChanged
        BindTemplates()
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('showTarget();',100);", True)            
    End Sub

    Private Sub plistSelOwner_SelectedIndexChanged(sender As Object, e As EventArgs) Handles plistSelOwner.SelectedIndexChanged
        BindLists()
        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('showList();',500);", True)            
    End Sub

    Private Sub rptOpenProcess_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptOpenProcess.ItemCommand
        Try
            If e.CommandName = "Open" Then
                Response.Redirect("ReportDetail.aspx?RptName=OpenProcess&RPID=" & e.CommandArgument, False)
            ElseIf e.CommandName = "Del" Then
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms(0) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("p_RPID", e.CommandArgument)
                strSQL = " Delete from tblUListAC where RPDDID in (Select RPDDID from tblRunProcessDayDetail " & _
                         " where RPDID in (Select RPDID from tblRunProcessDay where RPID in " & _
                         " (Select RPID from tblRunProcess where RPID =  @p_RPID ))) ; " & _
                         "   " & _
                         " Delete from tblRunProcessDayDetail where RPDID in " & _
                         " (Select RPDID from tblRunProcessDay where RPID in " & _
                         " (Select RPID from tblRunProcess where RPID =  @p_RPID )) ; " & _
                         "   " & _
                         " Delete from tblRunProcessDay where RPID in " & _
                         " (Select RPID from tblRunProcess where RPID =  @p_RPID) ; " & _
                         "   " & _
                         " Delete from tblRunProcess where RPID = @p_RPID; "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
            End If

            BindData()
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptOpenProcess_ItemCommand")
        End Try
    End Sub

    Private Sub rptClosedProcess_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptClosedProcess.ItemCommand
        Try
            If e.CommandName = "Open" Then
                Response.Redirect("ReportDetail.aspx?RptName=OpenProcess&RPID=" & e.CommandArgument, False)
            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptOpenProcess_ItemCommand")
        End Try
    End Sub
End Class
