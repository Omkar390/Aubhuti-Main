'Imports System.Collections.Generic
'Imports System.Linq
'Imports System.Web
'Imports System.Web.UI
'Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
'Imports System.Configuration
Imports System.Web.Services

Partial Class GroupChartDetail
  Inherits System.Web.UI.Page

    Dim str As New StringBuilder()

	Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
		If Not IsPostBack Then
            '-------------      existing code     START  -----------------------------------------------------
            'show charts list from tbldashlist with links
            Dim db As New DBHelper()
            Dim strSQL As String
            Dim parms(0) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_groupname", Request.QueryString("RptName"))
            strSQL = " SELECT RM.RptId, RM.RptName, RM.RptTitle, ChartType FROM tblgroups G INNER JOIN tblgroupsrpt GR ON G.groupid = GR.groupid " & _
                    " INNER JOIN tblrptmaster RM on RM.RptId = GR.RptId " & _
                    " WHERE groupname = @p_groupname "  & _
                    " ORDER BY orderno " 
            Dim dt As New DataTable()
            dt = db.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)

            'dt = db.DataAdapter(CommandType.Text,"SELECT rm.rptid,charttype,RptTitle FROM = " & Request.QueryString("dashlistid")).Tables(0)
            'Dim str1 As String = ""
            'Dim str2 As String = ""
            'Dim str3 As String = ""
            For i = 0 To dt.Rows.Count - 1
                'str1 = str1 + """" + dt.Rows(index)(0).ToString() + ""","
                'str2 = str2 + """" + dt.Rows(index)(1).ToString() + ""","
                'str3 = str3 + """" + dt.Rows(index)(2).ToString() + ""","
                'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "scriptdrawchart", "drawchart('" + dt.Rows(index)(0).ToString() + "','" + dt.Rows(index)(1).ToString() + "','" + dt.Rows(index)(2).ToString() + "','" + (index.ToString + "');", True)
                'draw chart
                ShowChart(dt.Rows(i)("RptId").ToString(),dt.Rows(i)("ChartType").ToString(),dt.Rows(i)("RptTitle").ToString(),i+1)
            Next
            'str1 = str1.Substring(0,str1.Length-1)
            'str2 = str2.Substring(0,str2.Length-1)
            'str3 = str3.Substring(0,str3.Length-1)
            'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "scriptshowcharts", "showcharts('" + str1 + "','" + str2 + "','" + str3 + "');", True)
            '-------------      existing code     END  -----------------------------------------------------
		End If

	End Sub
    Protected Sub ShowChart(ByVal reportid As Integer,byval charttype As String,byval charttitle As String,byval divindex As Integer)
            Dim dt As New DataTable()
            dt = GetData(reportid)       
            
		    str.Append("<script type=text/javascript> google.load( *visualization*, *1*, {packages:[*corechart*]});" & _
                        " google.setOnLoadCallback(drawChart" + divindex.ToString + "); " & _
                        " function drawChart" + divindex.ToString + "() {")

        Select Case charttype
                Case "StackedColumn"
                    
                    str.Append(" var data" + divindex.ToString + " = new google.visualization.DataTable();")
                    str.Append("            data" + divindex.ToString + ".addColumn('")
                    str.Append("string" + "','" + dt.Columns(0).ColumnName + "');")
                    For index = 1 To dt.Columns.Count-1
                        str.Append("            data" + divindex.ToString + ".addColumn('")
                        str.Append("number" + "','" + dt.Columns(index).ColumnName + "');")
                    Next

                    str.Append("data" + divindex.ToString + ".addRows(" + dt.Rows.Count.ToString + ");")

		            For i As Integer = 0 To dt.Rows.Count - 1
                        For index = 0 To dt.Columns.Count-1
			                str.Append("data" + divindex.ToString + ".setValue( " + i.ToString + "," + index.ToString + "," + "'" + dt.Rows(i)(index).ToString() + "');")
                        Next
		            Next
                    str.Append(" var options = {title: '" + charttitle + "'};" )
		            str.Append(" var chart" + divindex.ToString + " = new google.visualization.ColumnChart(document.getElementById('chart_div" + divindex.ToString + "'));")
		            str.Append(" chart" + divindex.ToString + ".draw(data" + divindex.ToString + ",{isStacked:true, width:600, height:350, hAxis: {showTextEvery:1, slantedText:true}});}")

                Case "Column"
                    str.Append(" var data" + divindex.ToString + " = new google.visualization.DataTable();")
                    str.Append("            data" + divindex.ToString + ".addColumn('")
                    str.Append("string" + "','" + dt.Columns(0).ColumnName + "');")
                    For index = 1 To dt.Columns.Count-1
                        str.Append("            data" + divindex.ToString + ".addColumn('")
                        str.Append("number" + "','" + dt.Columns(index).ColumnName + "');")
                    Next

                    str.Append("data" + divindex.ToString + ".addRows(" + dt.Rows.Count.ToString + ");")

		            For i As Integer = 0 To dt.Rows.Count - 1
                        For index = 0 To dt.Columns.Count-1
			                str.Append("data" + divindex.ToString + ".setValue( " + i.ToString + "," + index.ToString + "," + "'" + dt.Rows(i)(index).ToString() + "');")
                        Next
		            Next

                    str.Append(" var options = {title: '" + charttitle + "'};" )

                    str.Append(" var chart" + divindex.ToString + " = new google.visualization.ColumnChart(document.getElementById('chart_div" + divindex.ToString + "'));")
		            str.Append(" chart" + divindex.ToString + ".draw(data" + divindex.ToString + ",{isStacked:false, width:600, height:350, hAxis: {showTextEvery:1, slantedText:true}});}")

				Case "Line"                    

                    str.Append(" var data" + divindex.ToString + " = google.visualization.arrayToDataTable([")
                    str.Append("[")
                    For index = 0 To dt.Columns.Count-1
                        str.Append("'" + dt.Columns(index).ColumnName + "',")
                    Next
                    str.Remove(str.Length - 1, 1)
                    str.Append("],")
                    For i As Integer = 0 To dt.Rows.Count - 1
                        str.Append("[")
                        For index = 0 To dt.Columns.Count-1
			                str.Append( dt.Rows(i)(index).ToString() + ",")
                        Next
                        str.Remove(str.Length - 1, 1)
                        str.Append("],")
		            Next
                    str.Remove(str.Length - 1, 1)
                    str.Append("]);")

                    str.Append(" var options = {title: '" + charttitle + "'};" )
                    str.Append(" var chart" + divindex.ToString + " = new google.visualization.LineChart(document.getElementById('chart_div" + divindex.ToString + "'));")
		            str.Append(" chart" + divindex.ToString + ".draw(data" + divindex.ToString + ",options);}")

            Case "Area"
            Case "Pie"
                    str.Append(" var data" + divindex.ToString + " = google.visualization.arrayToDataTable([")
                    str.Append("[")
                    For index = 0 To dt.Columns.Count-1
                        str.Append("'" + dt.Columns(index).ColumnName + "',")
                    Next
                    str.Remove(str.Length - 1, 1)
                    str.Append("],")
                    For i As Integer = 0 To dt.Rows.Count - 1
                        str.Append("[")
                        For index = 0 To dt.Columns.Count-1
			                If ( index Mod 2 = 0) Then
			                    str.Append( "'" + dt.Rows(i)(index).ToString() + "',")
                            Else
                                str.Append( dt.Rows(i)(index).ToString() + ",")
                            End If
                        Next
                        str.Remove(str.Length - 1, 1)
                        str.Append("],")
		            Next
                    str.Remove(str.Length - 1, 1)
                    str.Append("]);")

                    str.Append(" var options = {title: '" + charttitle + "'};" )
                    str.Append(" var chart" + divindex.ToString + " = new google.visualization.PieChart(document.getElementById('chart_div" + divindex.ToString + "'));")
		            str.Append(" chart" + divindex.ToString + ".draw(data" + divindex.ToString + ",options);}")
            Case Else

            End Select


		    str.Append("</script>")
            'directcast(Me.FindControl("ctl00_ct100_ContentPlaceHolder1_lt" + divindex.ToString ),Literal).Text = str.ToString().TrimEnd(","C).Replace("*"C, """"C)

        If (divindex = 1)
            lt1.Text = str.ToString().TrimEnd(","C).Replace("*"C, """"C)
        ElseIf (divindex = 2)
            lt2.Text = str.ToString().TrimEnd(","C).Replace("*"C, """"C)
        ElseIf (divindex = 3)
            lt3.Text = str.ToString().TrimEnd(","C).Replace("*"C, """"C)
        ElseIf (divindex = 4)
            lt4.Text = str.ToString().TrimEnd(","C).Replace("*"C, """"C)
        ElseIf (divindex = 5)
            lt5.Text = str.ToString().TrimEnd(","C).Replace("*"C, """"C)
        End If
    End Sub

   Protected Function GetData(ByVal reportid As Integer) as DataTable    
    
    Dim dt As New DataTable()
    Dim db As New DBHelperClient
    Dim strSQL As String = ""
        'strSQL = " SELECT grouptitle, rptid FROM tblgroups G INNER JOIN tblgroupsrpt GR ON G.groupid = GR.groupid " & _
        '               " WHERE groupname = '" & Request.QueryString("RptName") & "'" & _
        '               " ORDER BY orderno "
        Dim parms(0) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_RptId", reportid.ToString())
        strSQL = db.ExecuteScalar(CommandType.Text, "SELECT RptSql FROM client_master.`tblrptmaster` WHERE RptId = @p_RptId ", parms)
        dt = db.DataAdapter(CommandType.Text,strSQL).Tables(0)
	Return dt

End Function

        <WebMethod> _
Public Shared Function GetChartData(ByVal reportid As Integer) As List(Of ChartDetails)

    Dim dt As New DataTable()
    Dim db As New DBHelperClient
    Dim strSQL As String = ""
    Dim parms(0) As DBHelperClient.Parameters
    parms(0) = New DBHelperClient.Parameters("p_RptId", reportid.ToString())
    strSQL = db.ExecuteScalar(CommandType.Text,"SELECT RptSql FROM client_master.`tblrptmaster` WHERE RptId = @p_RptId ", parms)
    dt = db.DataAdapter(CommandType.Text,strSQL).Tables(0)

	Dim dataList As New List(Of ChartDetails)()

	For Each dtrow As DataRow In dt.Rows
		Dim details As New ChartDetails()
		details.PlanName = dtrow(0).ToString()
		details.PaymentAmount = Convert.ToInt32(dtrow(1))

		dataList.Add(details)
	Next
	Return dataList


End Function

    <WebMethod> _
Public Shared Function GetColumnChartData() As List(Of ChartDetails)
	Using con As New SqlConnection(ConfigurationManager.ConnectionStrings("GCConnectionString").ToString())
		Dim cmd As New SqlCommand("Usp_Getdata", con)
		cmd.CommandType = CommandType.StoredProcedure
		Dim da As New SqlDataAdapter()
		da.SelectCommand = cmd
		Dim dt As New DataTable()
		da.Fill(dt)

		Dim dataList As New List(Of ChartDetails)()

		For Each dtrow As DataRow In dt.Rows
			Dim details As New ChartDetails()
			details.PlanName = dtrow(0).ToString()
			details.PaymentAmount = Convert.ToInt32(dtrow(1))

			dataList.Add(details)
		Next
		Return dataList
	End Using
End Function

    Public Class ChartDetails
	Public Property PlanName() As String
		Get
			Return m_PlanName
		End Get
		Set
			m_PlanName = Value
		End Set
	End Property
	Private m_PlanName As String
	Public Property PaymentAmount() As Integer
		Get
			Return m_PaymentAmount
		End Get
		Set
			m_PaymentAmount = Value
		End Set
	End Property
	Private m_PaymentAmount As Integer
End Class

    Protected Sub ShowSummary()
   '     lblTitle.Text = "Executive Summary"
   '         Dim x As String()
   '         Dim y As Integer()

			'Dim dt As New DataTable()
			'Dim db As New DBHelperClient
   '         dt = db.DataAdapter(CommandType.Text,"SELECT COALESCE(stagename,'Open') as stagename,SUM(amount) AS amt FROM tblopportunity WHERE COALESCE(stagename,'Open') IN ('Open','Initial Meeting','Closed Won') GROUP BY stagename ORDER  BY SUM(amount) DESC").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart1.Series(0).Points.DataBindXY(x, y)
			'Chart1.Series(0).ChartType = SeriesChartType.Funnel
   '         Chart1.Series(0).IsValueShownAsLabel = True
			'Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
			'Chart1.Legends(0).Enabled = True
   '         Chart1.Titles(0).Text = "Sales Pipeline"
   '         Chart1.Series(0).Label = "#VALY{$#,##0.00}"
   '         Chart1.Series(0).LegendText = "#VALX"
   '         div1.Visible = True

   '         dt = db.DataAdapter(CommandType.Text,"SELECT A.weekno,COALESCE(A.acctcount,0) AS Accounts,COALESCE(B.contactcount,0) AS Contacts,COALESCE(C.oppcount,0) AS Opps,COALESCE(D.actcount,0) AS Activities " & _
   '                                             " FROM (SELECT CONCAT('Week ',WEEK(createdts)) AS weekno, COUNT(*) AS acctcount , 0 AS contactcount,0 AS oppcount,0 AS actcount " & _
   '                                             " FROM tblaccount WHERE TIMESTAMPDIFF(DAY,createdts,CURRENT_TIMESTAMP()) < 180 GROUP BY WEEK(createdts) ORDER  BY WEEK(createdts)) AS A " & _
   '                                             " LEFT OUTER JOIN  (SELECT CONCAT('Week ',WEEK(createdts)) AS weekno, 0 AS acctcount , COUNT(*) AS contactcount,0 AS oppcount,0 AS actcount " & _
   '                                             " FROM tblcontact WHERE TIMESTAMPDIFF(DAY,createdts,CURRENT_TIMESTAMP()) < 180 GROUP BY WEEK(createdts) ORDER  BY WEEK(createdts)) AS B ON A.weekno = B.weekno " & _
   '                                             " LEFT OUTER JOIN (SELECT CONCAT('Week ',WEEK(createdts)) AS weekno, 0 AS acctcount , 0 AS contactcount,COUNT(*) AS oppcount,0 AS actcount " & _
   '                                             " FROM tblopportunity WHERE TIMESTAMPDIFF(DAY,createdts,CURRENT_TIMESTAMP()) < 180 GROUP BY WEEK(createdts) ORDER  BY WEEK(createdts)) AS C ON A.weekno = C.weekno " & _
   '                                             " LEFT OUTER JOIN (SELECT CONCAT('Week ',WEEK(createdts)) AS weekno, 0 AS acctcount , 0 AS contactcount,0 AS oppcount,COUNT(*) AS actcount " & _
   '                                             " FROM tblactivities WHERE TIMESTAMPDIFF(DAY,createdts,CURRENT_TIMESTAMP()) < 180 GROUP BY WEEK(createdts) ORDER  BY WEEK(createdts)) AS D ON A.weekno = D.weekno").Tables(0)
   '         For colIndex As Integer = 1 To dt.Columns.Count - 1
	  '          Chart2.Series.Add(dt.Columns(colIndex).ColumnName)
	  '          Chart2.Series(dt.Columns(colIndex).ColumnName).ChartType = SeriesChartType.StackedColumn
	  '          Chart2.Series(dt.Columns(colIndex).ColumnName).BorderWidth = 2
	  '      Next

   '         For Each row As DataRow In dt.Rows
   '             For colIndex As Integer = 1 To dt.Columns.Count - 1
		 '           Dim columnName As String = dt.Columns(colIndex).ColumnName
		 '           Dim YVal As Integer = CInt(row(columnName))
		 '           Chart2.Series(columnName).Points.AddXY(row(0), YVal)
   '             Next
   '         Next 
   '         Chart2.Titles(0).Text = "Documents Created"
   '         div2.Visible = True

        
   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT('Week ',WEEK(MtgDate)) AS weekno,SUM(CASE mtginterface WHEN 'Face-to-Face' THEN 1 ELSE 0 END) AS 'Face-to-Face',SUM(CASE mtginterface WHEN 'Web based' THEN 1 ELSE 0 END) AS 'Web based',SUM(CASE mtginterface WHEN 'Phone Call' THEN 1 ELSE 0 END) AS 'Phone Call' FROM tblMeeting WHERE TIMESTAMPDIFF(DAY,MtgDate,CURRENT_TIMESTAMP()) < 180 GROUP BY WEEK(MtgDate) ORDER  BY WEEK(MtgDate)").Tables(0)
   '         For colIndex As Integer = 1 To dt.Columns.Count - 1
	  '          Chart3.Series.Add(dt.Columns(colIndex).ColumnName)
	  '          Chart3.Series(dt.Columns(colIndex).ColumnName).ChartType = SeriesChartType.StackedColumn
	  '          Chart3.Series(dt.Columns(colIndex).ColumnName).BorderWidth = 2
	  '      Next

   '         For Each row As DataRow In dt.Rows
   '             For colIndex As Integer = 1 To dt.Columns.Count - 1
		 '           Dim columnName As String = dt.Columns(colIndex).ColumnName
		 '           Dim YVal As Integer = CInt(row(columnName))
		 '           Chart3.Series(columnName).Points.AddXY(row(0), YVal)
   '             Next
   '         Next    
   '         Chart3.Titles(0).Text = "Meetings Status"
   '         div3.Visible = True

    End Sub

    Protected Sub ShowDocDahs()
   '         Dim x As String()
   '         Dim y As Integer()

			'Dim dt As New DataTable()
			'Dim db As New DBHelperClient
   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblaccount WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart1.Series(0).Points.DataBindXY(x, y)
			'Chart1.Series(0).ChartType = SeriesChartType.StackedBar
			'Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
			'Chart1.Legends(0).Enabled = True
   '         Chart5.Titles(0).Text = "Sales Pipeline"
   '         div1.Visible = True

   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblcontact WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart4.Series(0).Points.DataBindXY(x, y)
			'Chart4.Series(0).ChartType = SeriesChartType.Column
			'Chart4.ChartAreas("ChartArea4").Area3DStyle.Enable3D = True
			'Chart4.Legends(0).Enabled = True
   '         div4.Visible = True

   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblopportunity WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart5.Series(0).Points.DataBindXY(x, y)
			'Chart5.Series(0).ChartType = SeriesChartType.Line
			'Chart5.ChartAreas("ChartArea5").Area3DStyle.Enable3D = True
			'Chart5.Legends(0).Enabled = True
   '         div5.Visible = True

   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblactivities WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart6.Series(0).Points.DataBindXY(x, y)
			'Chart6.Series(0).ChartType = SeriesChartType.Area
			'Chart6.ChartAreas("ChartArea6").Area3DStyle.Enable3D = True
			'Chart6.Legends(0).Enabled = True
   '         div6.Visible = True


    End Sub


    Protected Sub fnsample()

   '         Dim x As String()
   '         Dim y As Integer()

			'Dim dt As New DataTable()
			'Dim db As New DBHelperClient
   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblaccount WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart1.Series(0).Points.DataBindXY(x, y)
			''Chart1.Series(0).ChartType = SeriesChartType.Pie
			''Chart1.Series(0).ChartType = SeriesChartType.Linevbc
			''Chart1.Series(0).ChartType = SeriesChartType.Column
			''Chart1.Series(0).ChartType = SeriesChartType.Doughnut
			''Chart1.Series(0).ChartType = SeriesChartType.Funnel
			''Chart1.Series(0).ChartType = SeriesChartType.Radar
			'Chart1.Series(0).ChartType = SeriesChartType.StackedBar
			'Chart1.ChartAreas("ChartArea1").Area3DStyle.Enable3D = True
			'Chart1.Legends(0).Enabled = True

   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblcontact WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart2.Series(0).Points.DataBindXY(x, y)
			'Chart2.Series(0).ChartType = SeriesChartType.Column
			'Chart2.ChartAreas("ChartArea2").Area3DStyle.Enable3D = True
			'Chart2.Legends(0).Enabled = True

   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblopportunity WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart3.Series(0).Points.DataBindXY(x, y)
			'Chart3.Series(0).ChartType = SeriesChartType.Line
			'Chart3.ChartAreas("ChartArea3").Area3DStyle.Enable3D = True
			'Chart3.Legends(0).Enabled = True

   '         dt = db.DataAdapter(CommandType.Text,"SELECT CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) AS Mon,COUNT(*) AS acctcount FROM tblactivities WHERE YEAR(createdts) = '2016' GROUP BY CONCAT(SUBSTRING(MONTHNAME(createdts),1,3),'-',YEAR(createdts)) ORDER BY createdts").Tables(0)
			'x = New String(dt.Rows.Count - 1) {}
			'y = New Integer(dt.Rows.Count - 1) {}
			'For i As Integer = 0 To dt.Rows.Count - 1
			'	x(i) = dt.Rows(i)(0).ToString()
			'	y(i) = Convert.ToInt32(dt.Rows(i)(1))
			'Next
			'Chart4.Series(0).Points.DataBindXY(x, y)
			'Chart4.Series(0).ChartType = SeriesChartType.Area
			'Chart4.ChartAreas("ChartArea4").Area3DStyle.Enable3D = True
			'Chart4.Legends(0).Enabled = True

    End Sub
End Class

                   
            'For Each row As DataRow In dt.Rows
	           ' ' For each Row add a new series
	           ' Dim seriesName As String = row("mtginterface").ToString()
            '    If seriesName <> "" Then
            '        Try     
            '            'If Chart5.Series.IndexOf(seriesName) <> -1 Then
	           '             Chart5.Series.Add(seriesName)
	           '             Chart5.Series(seriesName).ChartType = SeriesChartType.StackedColumn
	           '             Chart5.Series(seriesName).BorderWidth = 2
            '            'Else
            '            'End If
            '        Catch ex As Exception

            '        End Try
	           '     For colIndex As Integer = 1 To dt.Columns.Count - 1
		          '      ' For each column (column 1 and onward) add the value as a point
		          '      Dim columnName As String = dt.Columns(colIndex).ColumnName
		          '      Dim YVal As Integer = CInt(row("mtgcount"))
		          '      Chart5.Series(seriesName).Points.AddXY(row("weekno"), YVal)
	           '     Next
            '    End If
            'Next