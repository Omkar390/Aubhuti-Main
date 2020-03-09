Imports System.Data

Partial Class Account
    Inherits System.Web.UI.Page
    Public mstr As String = "ctl00$ctl00$ContentPlaceHolder1$"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                SSManager.CheckQS()
                If Not Request.QueryString("AcctId") Is Nothing Then
                    TryCast(Me.Master.FindControl(mstr & "hdnguidaccountId"), HiddenField).Value = Request.QueryString("AcctId")
                    If Request.QueryString("AcctId") = "New" Then
                        TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value = "0"
						ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('editco();',500);", True)
                    Else
                        TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value = SSManager.GetAccountId(Request.QueryString("AcctId"))
                    End If
                    TryCast(Me.Master.FindControl(mstr & "hdncontactId"), HiddenField).Value = "0"
                    TryCast(Me.Master.FindControl(mstr & "hdnguidcontactId"), HiddenField).Value = ""
                End If

                If Not Request.QueryString("CtId") Is Nothing Then
                    TryCast(Me.Master.FindControl(mstr & "hdncontactId"), HiddenField).Value = SSManager.GetContactId(Request.QueryString("CtId"))
                    TryCast(Me.Master.FindControl(mstr & "hdnguidcontactId"), HiddenField).Value = Request.QueryString("CtId")
                End If

                Dim Ctdt As DataTable
                Dim db As New DBHelperClient
                Dim parms(3) As DBHelperClient.Parameters
                If Request.QueryString("BindType") = "A" Then
                    parms(0) = New DBHelperClient.Parameters("p_sFlag", "A")
                    parms(1) = New DBHelperClient.Parameters("p_account_id", TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value)
                    parms(2) = New DBHelperClient.Parameters("p_guid_account_id", TryCast(Me.Master.FindControl(mstr & "hdnguidaccountId"), HiddenField).Value)
                    parms(3) = New DBHelperClient.Parameters("p_contact_id", "0")
                Else
                    parms(0) = New DBHelperClient.Parameters("p_sFlag", "N")
                    parms(1) = New DBHelperClient.Parameters("p_account_id", TryCast(Me.Master.FindControl(mstr & "hdnaccountId"), HiddenField).Value)
                    parms(2) = New DBHelperClient.Parameters("p_guid_account_id", TryCast(Me.Master.FindControl(mstr & "hdnguidaccountId"), HiddenField).Value)
                    parms(3) = New DBHelperClient.Parameters("p_contact_id", TryCast(Me.Master.FindControl(mstr & "hdncontactId"), HiddenField).Value)
                End If
                Ctdt = db.DataAdapter(CommandType.StoredProcedure, "SP_GetContacts", parms).Tables(0)
                rptContacts.DataSource = Ctdt
                rptContacts.DataBind()

            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub
End Class
