Imports System.Data

Partial Class AccountContactMerge
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack = True Then
            Try
                btnMerge.Visible = False
                NoRecs.Visible = True
            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub btnSrch_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSrch.ServerClick
        BindData()
    End Sub

    Protected Sub BindData()
        Try
            tblSrch.Visible = True
            Dim AcctCt As DataSet
            Dim db As New DBHelperClient
            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_SearchTerm", "%" & txtconame.Text & "%")
            AcctCt = db.DataAdapter(CommandType.StoredProcedure, "SP_GetCoCt", parms)
            If ddlSelect.SelectedValue = "Account" Then
                If AcctCt.Tables(0).Rows.Count > 0 Then
                    btnMerge.Visible = True
                    NoRecs.Visible = False
                    rptAcct.DataSource = AcctCt.Tables(0)
                    rptAcct.DataBind()
                Else
                    btnMerge.Visible = False
                    NoRecs.Visible = True
                    rptAcct.DataSource = Nothing
                    rptAcct.DataBind()
                End If
            Else
                If AcctCt.Tables(1).Rows.Count > 0 Then
                    btnMerge.Visible = True
                    NoRecs.Visible = False
                    rptAcct.DataSource = AcctCt.Tables(1)
                    rptAcct.DataBind()
                Else
                    btnMerge.Visible = False
                    NoRecs.Visible = True
                    rptAcct.DataSource = Nothing
                    rptAcct.DataBind()
                End If
            End If


        Catch ex As Exception

        End Try

    End Sub
    Protected Sub radMaster_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs)

        For Each item As RepeaterItem In rptAcct.Items
            If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                Dim rb = DirectCast(item.FindControl("radMaster"), RadioButton)
                rb.Checked = False
            End If
        Next
        Dim SenderRB As RadioButton = sender
        SenderRB.Checked = True

    End Sub

    Private Sub btnMerge_ServerClick(sender As Object, e As EventArgs) Handles btnMerge.ServerClick
        Try
            Dim i As Int32
            i = 0
            Dim New_t_account_id As String = 0
            Dim Old_t_account_id As String = 0
            Dim New_t_contact_id As String = 0
            Dim Old_t_contact_id As String = 0

            For Each item As RepeaterItem In rptAcct.Items
                If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                    Dim rb = DirectCast(item.FindControl("radMaster"), RadioButton)
                    If rb.Checked = True Then
                        If ddlSelect.SelectedItem.Text = "Account" Then
                            New_t_account_id = DirectCast(item.FindControl("litAcctId"), Literal).Text
                        Else
                            New_t_account_id = DirectCast(item.FindControl("litAcctId"), Literal).Text
                            New_t_contact_id = DirectCast(item.FindControl("litCtId"), Literal).Text
                        End If
                        Exit For
                    End If
                End If
            Next

            If New_t_account_id = 0 And New_t_contact_id = 0 Then
                lblErrorMsg.Text = "Please select a Master " & ddlSelect.SelectedValue & " to Merge."
                Exit Sub
            End If

            For Each item As RepeaterItem In rptAcct.Items
                If item.ItemType = ListItemType.Item OrElse item.ItemType = ListItemType.AlternatingItem Then
                    Dim cb = DirectCast(item.FindControl("chkMerge"), CheckBox)
                    If cb.Checked = True Then
                        Dim db As New DBHelperClient
                        If ddlSelect.SelectedValue = "Account" Then
                            Old_t_account_id = DirectCast(item.FindControl("litAcctId"), Literal).Text
                            Old_t_contact_id = DirectCast(item.FindControl("litCtId"), Literal).Text
                            If Old_t_account_id = New_t_account_id Then
                            Else
                                Dim parms(3) As DBHelperClient.Parameters
                                parms(0) = New DBHelperClient.Parameters("p_Flag", "U")
                                parms(1) = New DBHelperClient.Parameters("p_old_t_account_id", Old_t_account_id)
                                parms(2) = New DBHelperClient.Parameters("p_new_t_account_id", New_t_account_id)
                                parms(3) = New DBHelperClient.Parameters("p_user_id", Session("user_id"))
                                db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_UAccountMerge", parms)
                            End If
                        Else
                            Old_t_account_id= DirectCast(item.FindControl("litAcctId"), Literal).Text
                            Old_t_contact_id = DirectCast(item.FindControl("litCtId"), Literal).Text
                            If Old_t_contact_id = New_t_contact_id Then
                            Else
                                Dim parms(4) As DBHelperClient.Parameters
                                parms(0) = New DBHelperClient.Parameters("p_Flag", "U")
                                parms(1) = New DBHelperClient.Parameters("p_old_t_account_id", Old_t_account_id)
                                parms(2) = New DBHelperClient.Parameters("p_new_t_account_id", New_t_contact_id)
                                parms(3) = New DBHelperClient.Parameters("p_old_t_contact_id", Old_t_contact_id)
                                parms(4) = New DBHelperClient.Parameters("p_user_id", Session("user_id"))
                                db.ExecuteScalar(Data.CommandType.StoredProcedure, "SP_UContactMerge", parms)
                            End If
                        End If
                    End If
                End If
            Next

            BindData()
        Catch ex As Exception

        End Try
    End Sub
End Class