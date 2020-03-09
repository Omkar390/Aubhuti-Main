Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.UI.DataVisualization.Charting

Partial Class isrdash
  Inherits System.Web.UI.Page
    Public donutC As String
    Public donutE As String
    Public donutI As String
    Public donutM As String

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
		If Not IsPostBack Then
            hdnconfigdays.Value = ConfigurationManager.AppSettings("NumDays").ToString
            hdnnumdays.Value = ConfigurationManager.AppSettings("NumDays").ToString

            Dim db As New DBHelper
            Dim strSQL As String
            strSQL = " SELECT U.userid , UserName " & _
                    "   FROM tbluserclient UC " & _
                    "   INNER JOIN users U ON U.userid = UC.userid " & _
                    "   WHERE COALESCE(UC.Active,0) = 1 and UC.client_id = " & Session("client_id") & _
                    "   And UC.userGroup = 'ISR' and userRole = 'User' order by UserName desc "
            ddlISR.datasource = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
            ddlISR.databind()

            hdnstartdate.Value = (DateTime.Now.Year).ToString + "-" + (DateTime.Now.Month).ToString + "-" + (DateTime.Now.Day - 2).ToString
            hdnenddate.Value = (DateTime.Now.Year).ToString + "-" + (DateTime.Now.Month).ToString + "-" + (DateTime.Now.Day - 1).ToString
            'hdnenddate.Value = DateTime.Now.ToString("yyyy-MM-dd")

            Dim strtemp As String = ddlISR.SelectedValue
            BindData()
        End If

	End Sub

    Protected Sub BindData()
        Try
            Dim db As New DBHelperClient
            Dim strSQL As String

            littotdaysCalls.Text = hdnnumdays.Value
            littotdaysEmails.Text = hdnnumdays.Value
            littotdaysInventory.Text = hdnnumdays.Value

            '--- Calls with Total
            strSQL = " SELECT count(*) " & _
                    "   FROM tblcalls C " & _
                    "   WHERE C.CreatedBy = " & ddlISR.SelectedValue & _
                    "   AND CAST(createdts AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            litdCalls.Text = db.ExecuteScalar(CommandType.Text, strSQL)
            litCallsPerformed.Text = litdCalls.Text 

            donutC = "   function drawChartC() { " & _
                        "  var data1 = google.visualization.arrayToDataTable([ " & _
                        "   ['Effort', 'Amount given'], " & _
                        "   ['Watch TV', " & litdCalls.Text & "], " & _
                        "   ['My all', 50] " & _
                        "   ]); " & _
                        "   var chart1 = new google.visualization.PieChart(document.getElementById('donut_1')); " & _
                        "   chart1.draw(data1, options); " & _
                        "   }   "

            '--- Emails with total
            strSQL = " SELECT COUNT(*) " & _
                    "   FROM tblEMAILS E " & _
                    "   INNER JOIN client_master.users AS U ON E.OwnerFrom = U.userid " & _
                    "   WHERE E.OwnerFrom = "  & ddlISR.SelectedValue & _
                    "   AND CAST(DateSentRec AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            litdEmails.Text = db.ExecuteScalar(CommandType.Text, strSQL)
            litEmailsSent.Text = litdEmails.Text

            donutE = "  function drawChartE() { " & _
                        "   var data2 = google.visualization.arrayToDataTable([ " & _
                        "   ['Effort', 'Amount given'], " & _
                        "   ['Watch TV', " & litdEmails.Text & "], " & _
                        "   ['My all', 50] " & _
                        "   ]); " & _
                        "   var chart2 = new google.visualization.PieChart(document.getElementById('donut_2')); " & _
                        "   chart2.draw(data2, options);    " & _
                        "        } "

            '--- Inventory with total
            strSQL = "  SELECT COUNT(*) " & _
                    "   FROM tblactivities A " & _
                    "   INNER JOIN client_master.users AS U ON A.CreatedBy = U.userid " & _
                    "   WHERE A.CreatedBy = "  & ddlISR.SelectedValue & _
                    "   AND A.activity_type =  'Inventory' " & _
                    "   AND CAST(A.createdts AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            litdInventory.Text = db.ExecuteScalar(CommandType.Text, strSQL)
            litInvManaged.Text = litdInventory.Text

            donutI = "  function drawChartI() { " & _
                        "   var data3 = google.visualization.arrayToDataTable([ " & _
                        "   ['Effort', 'Amount given'], " & _
                        "   ['Watch TV', " & litdInventory.Text & "], " & _
                        "   ['My all', 50] " & _
                        "   ]); " & _
                        "   var chart3 = new google.visualization.PieChart(document.getElementById('donut_3')); " & _
                        "   chart3.draw(data3, options); " & _
                        "   } "

            strSQL = "   SELECT 'Cancelled' AS Descrip, COUNT(*) AS Cnt " & _
                    "   FROM tblmeeting M " & _
                    "   LEFT OUTER JOIN client_master.users U ON M.createdby = U.userid " & _
                    "   LEFT OUTER JOIN tblmeetingtype MT ON MT.MeetingTypeID = M.mtgtype " & _
                    "   WHERE M.createdby = " & ddlISR.SelectedValue & " AND MT.MeetingTypeID = 5 " & _
                    "   AND CAST(M.createdts AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            Dim MtgCan As String
            MtgCan = db.DataAdapter(CommandType.Text, strSQL).Tables(0).Rows(0).Item(1)

            strSQL = "   SELECT 'Done' AS Descrip, COUNT(*) AS Cnt " & _
                    "   FROM tblmeeting M " & _
                    "   LEFT OUTER JOIN client_master.users U ON M.createdby = U.userid " & _
                    "   WHERE M.createdby = " & ddlISR.SelectedValue & _
                    "   AND IFNULL(mtgoccurred, 'N') = 'Y' " & _
                    "   AND CAST(M.createdts AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            Dim MtgDone As String
            MtgDone = db.DataAdapter(CommandType.Text, strSQL).Tables(0).Rows(0).Item(1)

            strSQL = "   SELECT 'Pending' AS Descrip, COUNT(*) AS Cnt " & _
                    "   FROM tblmeeting M " & _
                    "   LEFT OUTER JOIN client_master.users U ON M.createdby = U.userid " & _
                    "   WHERE M.createdby = " & ddlISR.SelectedValue & _
                    "   AND IFNULL(mtgoccurred, 'N') = 'N' " & _
                    "   AND CAST(M.createdts AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            Dim MtgPend As String
            MtgPend = db.DataAdapter(CommandType.Text, strSQL).Tables(0).Rows(0).Item(1)
            litMtgToConfirm.Text = MtgPend
            litTotMeet.Text = MtgCan + MtgDone + MtgPend

            donutM = "  function drawChartM() { " & _
                    "   var datap = google.visualization.arrayToDataTable([ " & _
                    "   ['Meetings', 'Count'], " & _
                    "   ['Done', " & MtgDone & "], " & _
                    "   ['Cancelled', " & MtgCan & "], " & _
                    "   ['Pending', " & MtgPend & "] " & _
                    "   ]); " & _
                    "   var optionsM = { " & _
                    "   legend: 'none', " & _
                    "   backgroundColor: 'transparent', " & _
                    "   pieSliceTextStyle: { fontSize: 24 }, " & _
                    "   pieSliceBorderColor: 'transparent', " & _
                    "   chartArea: { 'width': '100%', 'height': '100%' }, " & _
                    "   slices: [{ color: '#30c96a' }, { color: '#de9118' }, { color: '#ff4817' }] " & _
                    "   }; " & _
                    "   var chart = new google.visualization.PieChart(document.getElementById('piechart')); " & _
                    "   chart.draw(datap, optionsM); " & _
                    "   } "

            strSQL = "   SELECT COUNT(*) AS Cnt " & _
                    "   FROM tblmeeting M " & _
                    "   LEFT OUTER JOIN client_master.users U ON M.createdby = U.userid " & _
                    "   WHERE M.createdby = " & ddlISR.SelectedValue & _
                    "   AND mtgconfirmedon is NOT NULL  " & _
                    "   AND CAST(M.createdts AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            litMtgConfirmed.Text = db.ExecuteScalar(CommandType.Text, strSQL)

            strSQL = "  SELECT COUNT(*) " & _
                    "   FROM tblactivities A " & _
                    "   INNER JOIN client_master.users AS U ON A.CreatedBy = U.userid " & _
                    "   WHERE A.CreatedBy = " & ddlISR.SelectedValue & _
                    "   AND CAST(A.createdts AS DATE) BETWEEN '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'"
            litActivities.Text = db.ExecuteScalar(CommandType.Text, strSQL)
            litActPerf.Text = litActivities.Text

            strSQL =  " SELECT num_open, num_closed " & _
                    "   FROM (  " & _
	                "   SELECT  DISTINCT U.username AS initiated, U.userid " & _
	                "   ,  SUM(CASE WHEN (COALESCE(RPDD.procestatus,0) = 1 OR COALESCE(RPDD.procestatus,0) = 5) THEN 0 ELSE 1 END) AS num_open " & _
	                "   ,  SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) <> 1 AND RPD.RPDate > DATE_ADD(CURDATE(),INTERVAL -7 DAY) AND RPD.RPDate <= CURDATE() THEN 1 ELSE 0 END) AS num_open_today " & _
	                "   ,  SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =1 THEN 1 ELSE 0 END) AS num_closed " & _
	                "   ,  SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =1 AND CURDATE() = RPD.RPDate THEN 1 ELSE 0 END) AS num_closed_today " & _
	                "   ,  SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =5 THEN 1 ELSE 0 END) AS num_skipped " & _
	                "   ,  SUM(CASE WHEN COALESCE(RPDD.procestatus, 0) =5 AND  CURDATE() = RPD.RPDate THEN 1 ELSE 0 END) AS num_skipped_today  " & _
	                "   FROM  tblRunProcess AS RP  " & _
	                "   INNER JOIN client_master.users AS U ON RP.CreatedBy = U.userid  " & _
	                "   INNER JOIN tblRunProcessDay AS RPD ON RP.RPID = RPD.RPID  " & _
	                "   INNER JOIN tblRunProcessDayDetail AS RPDD ON RPD.RPDID = RPDD.RPDID  " & _
	                "   WHERE RP.CreatedBy = " & ddlISR.SelectedValue & _
                    "   And CAST(RP.PStartDate AS Date) between '" & hdnstartdate.Value & "' AND '" & hdnenddate.Value & "'" & _
	                "   GROUP BY U.username, U.userid ) AS A  "
            Dim dtSt As New DataTable()
            dtSt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
            If dtSt.Rows.Count > 0 Then
                litCurStreamTasks.Text = dtSt.Rows(0).Item(0).ToString
                litStreamTasksComp.Text = dtSt.Rows(0).Item(1).ToString
            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnView_ServerClick(sender As Object, e As EventArgs) Handles btnView.ServerClick
        BindData()

    End Sub
End Class
