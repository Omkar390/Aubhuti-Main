Imports System.Data

Partial Class Weblogs
    Inherits System.Web.UI.Page
    Public mstr As String = "ctl00$ctl00$ContentPlaceHolder1$"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            SSManager.CheckQS()
            If Not Request.QueryString("AcctId") Is Nothing Then
                TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value = SSManager.GetAccountId(Request.QueryString("AcctId"))
                TryCast(Me.Master.FindControl(mstr & "hdnguidaccountId"), HiddenField).Value = Request.QueryString("AcctId")
            End If

			If Not Request.QueryString("CtId") Is Nothing Then
                TryCast(Me.Master.FindControl(mstr & "hdncontactId"), HiddenField).Value = SSManager.GetContactId(Request.QueryString("CtId"))
                TryCast(Me.Master.FindControl(mstr & "hdnguidcontactId"), HiddenField).Value = Request.QueryString("CtId")
            End If

            Dim db As New DBHelperClient
            Dim dt As New DataTable
            dt = SSManager.GetAllCo("S", TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value)

            Dim CoName As String
            CoName = dt.Rows(0).Item("name").ToString()
            CoName = CoName.Replace("inc", "").Replace("Inc", "").Replace("Corp", "").Replace("Company", "").Replace("Co", "").Replace("Group", "")
            CoName = CoName.Replace("Corporation", "").Replace("Limited", "").Replace("L.P", "").Replace("LLC", "").Replace("Ltd", "").Replace(".", "")

            Dim strSQL As String
            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("CoName", "%" & CoName & "%")
            strSQL = " SELECT  DATE_FORMAT(VisitDateTime, '%m/%d/%Y') AS VisitDate " & _
                    " , wl.BrowserName " & _
                    " , REPLACE(ActualWebPageName,'http://www.technology-insight.com/','') AS WebPageName " & _
                    " , wl.IpAddress " & _
                    " FROM tblweblogs wl, tblipdetails ipd ,  tblCountry c " & _
                    " WHERE(wl.IpAddress = ipd.IpAddress) " & _
                    " AND c.CountryCode = ipd.CountryCode " & _
                    " AND ( c.Countrycode = 'US' ) " & _
                    " AND organizationname LIKE @CoName " & _
                    " ORDER BY VisitDateTime DESC;"
            rptWeblogs.DataSource = db.DataAdapter(CommandType.Text, strSQL,parms).Tables(0)
            rptWeblogs.DataBind()

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

End Class
