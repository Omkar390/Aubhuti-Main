Imports System.Data

Partial Class AccountList
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
            Dim db As New DBHelperClient
            Dim dt As New DataTable
            Dim strSQL As String
            strSQL = " SELECT id, name AS Company, physicalcity AS City, ctcnt AS '# Contacts', physicalstate AS State, " & _
                    " micro_region AS 'Micro Region', annual_revenue AS 'Annual Revenue', " & _
                    " lastactivitydate AS 'Last Activity Date', Concat('Account.aspx?BindType=A&AcctId=', id) as hrefLink " & _
                    " FROM tblaccount A " & _
                    " LEFT OUTER JOIN (SELECT COUNT(*) AS ctcnt, t_account_id FROM tblcontact GROUP BY t_account_id) Ct " & _
                    " ON A.t_account_id = Ct.t_account_id "
            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
            rptAccounts.DataSource = dt
            rptAccounts.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class
