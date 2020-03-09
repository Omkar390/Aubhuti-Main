Imports System.Data
Partial Class Users
    Inherits System.Web.UI.Page
    Private isEditMode As Boolean = False
    Dim ColFormat As String()
    Dim ColWidth As String()
    Dim ColLinks As String()
    Dim ColShowHide As String()
    Dim fCount As Integer

    Protected Property IsInEditMode() As Boolean
        Get
            Return Me.isEditMode
        End Get

        Set(ByVal value As Boolean)
            Me.isEditMode = value
        End Set
    End Property

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                hdngrpid.Value = 0
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
            strSQL = " SELECT groupid, groupname, grouptitle FROM tblgroups Order by  groupname "

            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
            rptUsers.DataSource = dt
            rptUsers.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub rptUsers_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptUsers.ItemCommand
        Try
            hdngrpid.Value = e.CommandArgument
            If e.CommandName = "Edit" Then
                Response.Redirect("GroupReportEdit.aspx?reportid=" & hdngrpid.Value.ToString() , False)

            ElseIf e.CommandName = "Del" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "EditUser", "setTimeout('deluser();',500);", True)
            End If
        Catch ex As Exception

        End Try
    End Sub


    Protected Sub btnDel_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDel.ServerClick
        Try
            Dim strSQL As String
            Dim db As New DBHelper
            Dim parms(2) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_grouprptrowid", hdngrpid.Value )
            strSQL = " Delete from tblgroupsrpt where grouprptrowid = @p_grouprptrowid " 
            db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
            BindList()

        Catch ex As Exception

        End Try
    End Sub

End Class
