Imports System.Data
Partial Class ShowReportsC
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                BindList()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Sub BindList()
        Try
            Dim db As New DBHelper
            Dim dt As New DataTable
            Dim strSQL As String
            strSQL = " SELECT RptTitle, CONCAT('ReportDetail.aspx?RptName=', RptName) AS hrefLink, 'Report' as RptType " & _
                    " FROM tblrptmaster  " & _
                    " WHERE RptName NOT LIKE 'Chart%' and RptCategory = 'Client' AND RptType = 'T'" & _
                    " UNION " & _
                    " SELECT grouptitle AS RptTile, CONCAT('GroupReportDetail.aspx?RptName=', groupname) AS hrefLink, 'Group Report' AS RptType  " & _
                    " FROM tblgroups  where grouptype = 'T'"  & _
                    " AND groupname not like 'Chart%' " & _
                    " UNION " & _
                     " SELECT RptTitle, CONCAT('ChartDetail.aspx?RptName=', RptName) AS hrefLink, 'Chart' as RptType " & _
                    " FROM tblrptmaster  " & _
                    " WHERE RptName LIKE 'Chart%' and RptCategory = 'Client' AND RptType = 'T'" & _
                    " UNION " & _
                    " SELECT grouptitle AS RptTile, CONCAT('GroupChartDetail.aspx?RptName=', groupname) AS hrefLink, 'Group Chart Report' AS RptType  " & _
                    " FROM tblgroups  where grouptype = 'T'"  & _
                    " AND groupname like 'Chart%' " & _
                    " order by RptTitle "
            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
            rptList.DataSource = dt
            rptList.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class