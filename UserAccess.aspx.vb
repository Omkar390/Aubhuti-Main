Imports System.Data
Partial Class UserAccess
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session("CGUID") = "716eff48-cb2f-11e6-a817-065cea4aa6c1"
        Session("client_id") = "2"
        Session("dbname") = "2002"
        Session("userRole") = "Administrator"
        Response.Redirect("SalesShark.aspx", False)
        'Try
        '    If Not Page.IsPostBack Then
        '        Dim db As New DBHelper
        '        Dim strSQL As String
        '        Dim dt As New DataTable

        '        If Session("user_group") = "Admin" Then
        '            strSQL = "Select 'Admin' as client_name , '999' as client_id , 'master' as dbname, '1' as ListOrder, 'AdminMenu.aspx' as hrefPage, '' as GUID, '' as userRole " & _
        '                    " UNION " & _
        '                    " Select CM.client_name, CM.client_id, SUBSTRING(database_name,8,4) AS dbname, '2' as ListOrder , '#' as hrefPage, CM.GUID, UC.userRole " & _
        '                    " from tbluserclient UC Inner Join client_master CM on UC.client_id = CM.client_id " & _
        '                    " where CM.Active = 1 and userid = " & Session("user_id") & " Order by ListOrder, client_name "
        '            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
        '            rptClients.DataSource = dt
        '            rptClients.DataBind()

        '        Else
        '            strSQL = "Select CM.client_name, CM.client_id, SUBSTRING(database_name,8,4) AS dbname, '2' as ListOrder , 'SalesShark.aspx' as hrefPage, CM.GUID as CGUID, UC.userRole" & _
        '                    " from tbluserclient UC Inner Join client_master CM on UC.client_id = CM.client_id " & _
        '                    " where CM.Active = 1 and UC.Active = 1 and userid = " & Session("user_id") & " Order by ListOrder, client_name "

        '            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
        '            If dt.Rows.Count = 1 Then
        '                Session("CGUID") = dt.Rows(0).Item("CGUID").ToString
        '                Session("client_id") = dt.Rows(0).Item("client_id").ToString
        '                Session("dbname") = dt.Rows(0).Item("dbname").ToString
        '                Session("userRole") = dt.Rows(0).Item("userRole").ToString
        '                Response.Redirect("SalesShark.aspx", False)
        '            Else
        '                rptClients.DataSource = dt
        '                rptClients.DataBind()
        '            End If
        '        End If

        '    End If
        'Catch ex As Exception

        'End Try
    End Sub


    Protected Sub rptClients_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptClients.ItemCommand
        If e.CommandArgument = "999" Then
            Session("client_id") = e.CommandArgument
            Session("dbname") = "master"
            Response.Redirect("AdminMenu.aspx", False)
        Else
            Session("client_id") = e.CommandArgument
            Session("dbname") = TryCast(e.Item.FindControl("hdndbname"), HiddenField).Value
            Session("CGUID") = TryCast(e.Item.FindControl("hdncGUID"), HiddenField).Value
            Session("userRole") = TryCast(e.Item.FindControl("hdnuserRole"), HiddenField).Value
            Response.Redirect("SalesShark.aspx", False)
        End If
    End Sub
End Class
