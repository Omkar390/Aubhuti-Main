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
                hdnrptid.Value = 0
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
            Dim parms1(0) As DBHelper.Parameters
            parms1(0) = New DBHelper.Parameters("srch", "%" & txtsrch.Text.ToLower & "%")
            If txtsrch.Text = "" Then
                strSQL = " SELECT RptId, RptName, RptTitle, Case RptType when 'T' then 'Top Level' Else 'DrillDown' End as RptType, RptCategory, " & _
                        " Concat('ReportMasterMaint.aspx?RptId=', RptId) as hrefLink, " & _ 
                        " DATE_FORMAT(updatedon, '%m/%d/%Y') as updatedon " & _
                        " FROM tblrptmaster  " & _
					    " Order by RptName "
            Else
                strSQL = " SELECT RptId, RptName, RptTitle, Case RptType when 'T' then 'Top Level' Else 'DrillDown' End as RptType, RptCategory, " & _
                        " Concat('ReportMasterMaint.aspx?RptId=', RptId) as hrefLink, " & _ 
                        " DATE_FORMAT(updatedon, '%m/%d/%Y') as updatedon " & _
                        " FROM tblrptmaster  " & _
                        " where LCASE(RptName) like @srch or LCASE(RptTitle) like @srch " & _
					    " Order by RptName "
            End If

            dt = db.DataAdapter(CommandType.Text, strSQL,parms1).Tables(0)
            rptUsers.DataSource = dt
            rptUsers.DataBind()

            'grdUsers.DataSource = dt
            'grdUsers.DataBind()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub lnkUCreate_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkUCreate.ServerClick
        Try
            Dim dt As New DataTable
            Dim txtColFormat As String = ""
            Dim txtShowHide As String = ""
            Dim txtLinks As String = ""
            Dim txtWidths As String = ""
            Dim tmpRSQL As String = ""
            Dim userid As String
            lblMsg.Text = ""
            'userid = RTManager.CreateEditUser(hdnrptid.Value, txtUFName.Value, txtULName.Value, ddlURole.SelectedValue, txtUEmail.Value, _
            '                         txtUPhone.Value, txtUTitle.Value)
            Dim strSQL As String


            If hdnrptid.Value = 0 Then
                Try

                    tmpRSQL = txtRSQL.Value.Replace("::ST::", "quarter")
                    If ddlRCat.SelectedValue = "Master" Then
                        Dim dbm As New DBHelper
                        dt = dbm.DataAdapter(CommandType.Text, tmpRSQL.Replace("::", "'")).Tables(0)
                    Else
                        Dim dbc As New DBHelperCopy
                        dt = dbc.DataAdapter(CommandType.Text, tmpRSQL.Replace("::", "'")).Tables(0)
                    End If
                Catch ex As Exception
                    lblMsg.Text = "<span style='color:red'>" + " Error in SQL statement: " & ex.Message & "</span>"
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "EditUser", "setTimeout('edituser();',500);", True)
                    Exit Sub
                End Try

                For i = 0 To dt.Columns.Count - 1
                    If i = 0 Then
                        txtColFormat = ""
                        txtShowHide = ""
                        txtWidths = ""
                        txtLinks = ""
                    Else
                        txtColFormat = txtColFormat & "," & ""
                        txtShowHide = txtShowHide & "," & ""
                        txtWidths = txtWidths & "," & ""
                        txtLinks = txtLinks & "," & ""
                    End If
                Next
            Else
                Dim j As Integer = 0
                Dim chklinks As String()
                For Each item As RepeaterItem In rptColsDetails.Items

                    Dim linkField As Boolean = False
                    If Trim(TryCast(item.FindControl("litRptLinks"), TextBox).Text) <> "" Then
                        chklinks = TryCast(item.FindControl("litRptLinks"), TextBox).Text.ToString.Split("&")
                        If chklinks.Length <> 0 Then
                            For k = 1 To chklinks.Length - 1
                                For Each itemfield As RepeaterItem In rptColsDetails.Items
                                    If chklinks(k).Contains("=") Then
                                        linkField = True
                                    Else
                                        If TryCast(itemfield.FindControl("litFName"), Literal).Text = chklinks(k) Then
                                            linkField = True
                                        End If
                                    End If
                                Next
                                If linkField = False Then
                                    lblMsg.Text = "<span style='color:red'>" + " Error in Report Links: Link field not avaible in query fields" + "</span>"
                                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "EditUser", "setTimeout('edituser();',500);", True)
                                    Exit Sub
                                End If
                                linkField = False
                            Next
                        End If
                    End If

                    If j = 0 Then
                        txtColFormat = TryCast(item.FindControl("ddlRptColsFormat"), DropDownList).SelectedValue
                        txtShowHide = TryCast(item.FindControl("ddlRptShowHide"), DropDownList).SelectedValue
                        txtWidths = TryCast(item.FindControl("litRptColWidths"), TextBox).Text
                        txtLinks = TryCast(item.FindControl("litRptLinks"), TextBox).Text
                    Else
                        txtColFormat = txtColFormat & "," & TryCast(item.FindControl("ddlRptColsFormat"), DropDownList).SelectedValue
                        txtShowHide = txtShowHide & "," & TryCast(item.FindControl("ddlRptShowHide"), DropDownList).SelectedValue
                        txtWidths = txtWidths & "," & TryCast(item.FindControl("litRptColWidths"), TextBox).Text
                        txtLinks = txtLinks & "," & TryCast(item.FindControl("litRptLinks"), TextBox).Text
                    End If
                    j = j + 1
                Next
            End If

            Dim db As New DBHelper
            Dim parms(22) As DBHelper.Parameters
            If hdnrptid.Value = 0 Then
                parms(0) = New DBHelper.Parameters("sFlag", "I")
            Else
                parms(0) = New DBHelper.Parameters("sFlag", "U")
            End If
            parms(1) = New DBHelper.Parameters("iRptId", hdnrptid.Value)
            parms(2) = New DBHelper.Parameters("sRptName", txtRName.Value)
            parms(3) = New DBHelper.Parameters("sRptTitle", txtRTitle.Value)
            parms(4) = New DBHelper.Parameters("sRptType", ddlRType.SelectedValue)
            parms(5) = New DBHelper.Parameters("sRptCategory", ddlRCat.SelectedValue)
            parms(6) = New DBHelper.Parameters("sRptSql", txtRSQL.Value)
            parms(7) = New DBHelper.Parameters("sRptColsFormat", txtColFormat)
            parms(8) = New DBHelper.Parameters("sRptLinks", txtLinks)
            parms(9) = New DBHelper.Parameters("sRptShowHide", txtShowHide)
            parms(10) = New DBHelper.Parameters("sRptColWidths", txtWidths)
            parms(11) = New DBHelper.Parameters("bActive", "1")
            parms(12) = New DBHelper.Parameters("iRptCols", "0")
            parms(13) = New DBHelper.Parameters("iRptIcon", "0")
            parms(14) = New DBHelper.Parameters("sHelpText", "")
            'If chkShowYQM.Checked = True Then
            '    parms(15) = New DBHelper.Parameters("sShowYQM", "1")
            'Else
            '    parms(15) = New DBHelper.Parameters("sShowYQM", "0")
            'End If
            parms(15) = New DBHelper.Parameters("sDateFilterType", ddlDateFilterType.SelectedValue)
            'If chkShowDates.Checked = True Then
            '    parms(16) = New DBHelper.Parameters("sShowDates", "1")
            'Else
            parms(16) = New DBHelper.Parameters("sShowDates", "0")
            'End If
            parms(17) = New DBHelper.Parameters("iuser", Session("user_id"))
            parms(18) = New DBHelper.Parameters("icharttype", ddlChartType.SelectedValue)
            parms(19) = New DBHelper.Parameters("idefsortcol", ddlDefaultSort.SelectedValue)
            parms(20) = New DBHelper.Parameters("idefsortdir", ddlSortDir.SelectedValue)
            parms(21) = New DBHelper.Parameters("sRptWidth", txtWidth.Text.Trim)
            If chkShowTotals.Checked = True Then
                parms(22) = New DBHelper.Parameters("sShowTotals", "1")
            Else
                parms(22) = New DBHelper.Parameters("sShowTotals", "0")
            End If

            If hdnrptid.Value = 0 Then
                hdnrptid.Value = db.DataAdapter(CommandType.StoredProcedure, "SP_IURptMaster2", parms).Tables(0).Rows(0)(0).ToString
            Else
                db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IURptMaster2", parms)
            End If

            BindList()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "EditUser", "setTimeout('edituser();',500);", True)

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub rptUsers_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptUsers.ItemCommand
        Try
            hdnrptid.Value = e.CommandArgument
            Dim dt As DataTable
            Dim db As New DBHelper
            Dim parms1(0) As DBHelper.Parameters
            parms1(0) = New DBHelper.Parameters("RptId", e.CommandArgument)
            dt = db.DataAdapter(CommandType.Text, "Select * from tblrptmaster where RptId = @RptId " ,parms1).Tables(0)
            txtRName.Value = dt.Rows(0).Item("RptName").ToString
            txtRTitle.Value = dt.Rows(0).Item("RptTitle").ToString
            ddlRType.ClearSelection()
            ddlRType.Items.FindByValue(dt.Rows(0)("RptType")).Selected = True
            ddlRCat.ClearSelection()
            ddlRCat.Items.FindByValue(dt.Rows(0)("RptCategory")).Selected = True
            txtRSQL.Value = dt.Rows(0).Item("RptSql").ToString
            'If dt.Rows(0).Item("ShowYQM").ToString = "" Then
            '    chkShowYQM.Checked = False
            'Else
            '    chkShowYQM.Checked = dt.Rows(0).Item("ShowYQM").ToString
            'End If
            ddlDateFilterType.ClearSelection()
            ddlDateFilterType.Items.FindByValue(dt.Rows(0).Item("DateFilterType").ToString).Selected = True
            'If dt.Rows(0).Item("ShowDates").ToString = "" Then
            '    chkShowDates.Checked = False
            'Else
            '    chkShowDates.Checked = dt.Rows(0).Item("ShowDates").ToString
            'End If
            ColFormat = dt.Rows(0).Item("RptColsFormat").ToString.Split(",")
            ColWidth = dt.Rows(0).Item("RptColWidths").ToString.Split(",")
            ColLinks = dt.Rows(0).Item("RptLinks").ToString.Split(",")
            ColShowHide = dt.Rows(0).Item("RptShowHide").ToString.Split(",")

            If e.CommandName = "Edit" Then
                BindRpt(dt.Rows(0).Item("RptSql").ToString)
            End If

            ddlDefaultSort.ClearSelection()
			Try
				If dt.Rows(0)("defsortcol").ToString.Equals("") Then
					ddlDefaultSort.Items(0).Selected = True
				Else
					ddlDefaultSort.Items.FindByValue(dt.Rows(0)("defsortcol").ToString).Selected = True
				End If
			Catch ex As Exception

			End Try

            ddlSortDir.ClearSelection()
            If dt.Rows(0)("defsortdir").ToString.Equals("") Then
                ddlSortDir.Items(0).Selected = True
            Else
                ddlSortDir.Items.FindByValue(dt.Rows(0)("defsortdir").ToString).Selected = True
            End If
            

            If dt.Rows(0)("charttype").ToString = "" Then
            Else
                ddlChartType.ClearSelection()
                ddlChartType.Items.FindByValue(dt.Rows(0)("charttype")).Selected = True
            End If
			
            txtWidth.Text = dt.Rows(0).Item("rptwidth").ToString

            If dt.Rows(0).Item("showtotals").ToString = "" Then
                chkShowTotals.Checked = False
            Else
                chkShowTotals.Checked = dt.Rows(0).Item("showtotals").ToString
            End If


            If e.CommandName = "Edit" Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "EditUser", "setTimeout('edituser();',500);", True)
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
            Dim parms1(0) As DBHelper.Parameters
            parms1(0) = New DBHelper.Parameters("RptId", hdnrptid.Value)
            strSQL = " Delete from tblrptmaster where RptId = @RptId "
            db.ExecuteNonQuery(CommandType.Text, strSQL,parms1)
            BindList()

        Catch ex As Exception

        End Try
    End Sub


    Protected Sub BindRpt(ByVal RptSql As String)
        Try
            Dim dbc As New DBHelperCopy
            Dim dbm As New DBHelper
            Dim odt As New DataTable

            RptSql = RptSql.Replace("::ST::", "quarter")

            If ddlRCat.SelectedValue = "Master" Then
                odt = dbm.DataAdapter(CommandType.Text, RptSql.Replace("::", "'")).Tables(0)
            Else
                odt = dbc.DataAdapter(CommandType.Text, RptSql.Replace("::", "'")).Tables(0)
            End If

            fCount = odt.Columns.Count

            Dim flipdt As New DataTable
            Dim i As Integer = 0
            Dim r As DataRow

            Dim db As New DBHelper
            flipdt = db.DataAdapter(CommandType.Text, "SELECT '' AS FName, '' AS RptColsFormat, '' AS RptShowHide, '' AS RptColWidths, '' AS RptLinks FROM tblrptmaster LIMIT 0  ").Tables(0)

            For i = 0 To odt.Columns.Count - 1
                r = flipdt.NewRow()
                r(0) = odt.Columns(i).ToString
                flipdt.Rows.Add(r)
            Next i

            colDetails.Visible = True
            rptColsDetails.DataSource = flipdt
            rptColsDetails.DataBind()

                        
            'bind default sort
            ddlDefaultSort.DataTextField = "FName"
            ddlDefaultSort.DataValueField = "FName"
            ddlDefaultSort.DataSource = flipdt
            ddlDefaultSort.DataBind

            'If Session("RptId") = 0 Then
            'Else
            If ColFormat(0) = "" Then
            Else
                Dim j As Integer = 0
                For Each item As RepeaterItem In rptColsDetails.Items
                    TryCast(item.FindControl("ddlRptColsFormat"), DropDownList).Items.FindByValue(ColFormat(j)).Selected = True
                    TryCast(item.FindControl("ddlRptShowHide"), DropDownList).Items.FindByValue(ColShowHide(j)).Selected = True
                    TryCast(item.FindControl("litRptColWidths"), TextBox).Text = ColWidth(j)
                    TryCast(item.FindControl("litRptLinks"), TextBox).Text = ColLinks(j)
                    j = j + 1
                Next
            End If
            'End If

        Catch ex As Exception
            If ex.Message = "Index was outside the bounds of the array." Then
            Else
                lblMsg.Text = "<span style='color:red'>" + ex.Message + "</span>"
            End If
        End Try

    End Sub

    Protected Sub btnNew_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNew.Click
        hdnrptid.Value = 0
        txtRName.Value = ""
        txtRTitle.Value = ""
        ddlRType.ClearSelection()
        ddlRCat.ClearSelection()
        txtRSQL.Value = ""
        rptColsDetails.DataSource = Nothing
        rptColsDetails.DataBind()
        colDetails.Visible = False
        'chkShowYQM.Checked = False
        ddlDateFilterType.ClearSelection()
        txtWidth.Text = ""
        'chkShowDates.Checked = False
        chkShowTotals.Checked = False

        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "EditUser", "setTimeout('edituser();',500);", True)
    End Sub

    Private Sub btnSrch_ServerClick(sender As Object, e As EventArgs) Handles btnSrch.ServerClick
        Try
            BindList()
        Catch ex As Exception

        End Try
    End Sub
End Class
