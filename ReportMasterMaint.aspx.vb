Imports System.Web.Configuration.WebConfigurationManager
Imports System.Data
Imports System.IO
'Imports Microsoft.SqlServer.Management.Smo
'Imports Microsoft.SqlServer.Management.Common
'Imports System.Data.SqlReport

Partial Class ReportMasterMaint
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
                Dim ds As DataSet
                Dim db As New DBHelper
                ds = db.DataAdapter(CommandType.Text, "Select RptIconId, IconDesc from tblrpticons")
                ddlIcon.DataSource = ds.Tables(0)
                ddlIcon.DataBind()

                If Request.QueryString("RptId") = 0 Then
                    Session("RptId") = 0
                    txtName.Text = ""
                    chkActive.Checked = True
                    txtTitle.Text = ""
                    'txtRptCols.Text = "0"
                    ddlReportType.ClearSelection()
                    txtQuery.Value = ""
                Else
                    Session("RptId") = Request.QueryString("RptId")
                    BindReport()
                End If
            End If
        Catch ex As Exception
            'lblMsg.InnerText = Utils.ShowError(ex.Message)
        End Try
    End Sub

    Sub BindReport()
        Dim dt As DataTable
        Dim db As New DBHelper
        Dim parms1(0) As DBHelper.Parameters
        parms1(0) = New DBHelper.Parameters("RptId", Session("RptId"))
        dt = db.DataAdapter(CommandType.Text, "Select * from tblRptMaster where RptId = @RptId " ,parms1).Tables(0)
        txtName.Text = dt.Rows(0).Item("RptName").ToString
        chkActive.Checked = dt.Rows(0).Item("Active").ToString
        txtTitle.Text = dt.Rows(0).Item("RptTitle").ToString
        txtHelp.Value = dt.Rows(0).Item("HelpText").ToString
        ddlReportType.Items.FindByValue(dt.Rows(0)("RptType")).Selected = True
        ddlCategory.Items.FindByValue(dt.Rows(0)("RptCategory")).Selected = True
        txtQuery.Value = dt.Rows(0).Item("RptSql").ToString
        ddlIcon.Items.FindByValue(dt.Rows(0)("RptIcon")).Selected = True
        ColFormat = dt.Rows(0).Item("RptColsFormat").ToString.Split(",")
        ColWidth = dt.Rows(0).Item("RptColWidths").ToString.Split(",")
        ColLinks = dt.Rows(0).Item("RptLinks").ToString.Split(",")
        ColShowHide = dt.Rows(0).Item("RptShowHide").ToString.Split(",")

        BindRpt(dt.Rows(0).Item("RptSql").ToString)

    End Sub
    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.click
        Try
            Dim temp As String = ""
            Dim txtColFormat As String = ""
            Dim txtShowHide As String = ""
            Dim txtLinks As String = ""
            Dim txtWidths As String = ""
            Dim dt As New DataTable

            If Len(Trim(txtHelp.Value)) > 200 Then
                'lblMsg.InnerText = Utils.ShowError("Pl restrict help to 200 characters, reduce it by " & (Len(Trim(txtHelp.Value)) - 200) & " characters.")
                Exit Sub
            End If

            If ddlCategory.SelectedValue = "Master" Then
                Dim dbm As New DBHelper
                Try
                    dt = dbm.DataAdapter(CommandType.Text, txtQuery.Value.Replace("::", "'")).Tables(0)
                Catch ex As Exception
                    'lblMsg.InnerText = Utils.ShowError(" Error in SQL statement: " & ex.Message)
                    Exit Sub
                End Try
            Else
                Dim dbc As New DBHelperCopy
                Try
                    dt = dbc.DataAdapter(CommandType.Text, txtQuery.Value.Replace("::", "'")).Tables(0)
                Catch ex As Exception
                    'lblMsg.InnerText = Utils.ShowError(" Error in SQL statement: " & ex.Message)
                    Exit Sub
                End Try
            End If

            Dim j As Integer = 0
            Dim chklinks As String()
            For Each item As RepeaterItem In rptColsDetails.Items

                Dim linkField As Boolean = False
                If Trim(TryCast(item.FindControl("litRptLinks"), TextBox).Text) <> "" Then
                    chklinks = TryCast(item.FindControl("litRptLinks"), TextBox).Text.ToString.Split("&")
                    If chklinks.Length <> 0 Then
                        For k = 1 To chklinks.Length - 1
                            For Each itemfield As RepeaterItem In rptColsDetails.Items
                                If TryCast(itemfield.FindControl("litFName"), Literal).Text = chklinks(k) Then
                                    linkField = True
                                End If
                            Next
                            If linkField = False Then
                                'lblMsg.InnerText = Utils.ShowError(" Error in Report Links: Link field not avaible in query fields")
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


            Dim db As New DBHelper
            Dim parms(15) As DBHelper.Parameters
            If Session("RptId") = 0 Then
                parms(0) = New DBHelper.Parameters("sFlag", "I")
            Else
                parms(0) = New DBHelper.Parameters("sFlag", "U")
            End If
            parms(1) = New DBHelper.Parameters("iRptId", Session("RptId"))
            parms(2) = New DBHelper.Parameters("sRptName", txtName.Text)
            parms(3) = New DBHelper.Parameters("sRptTitle", txtTitle.Text)
            parms(4) = New DBHelper.Parameters("sRptType", ddlReportType.SelectedValue)
            parms(5) = New DBHelper.Parameters("sRptCategory", ddlCategory.SelectedValue)
            parms(6) = New DBHelper.Parameters("sRptSql", txtQuery.Value)
            parms(7) = New DBHelper.Parameters("sRptColsFormat", txtColFormat)
            parms(8) = New DBHelper.Parameters("sRptLinks", txtLinks)
            parms(9) = New DBHelper.Parameters("sRptShowHide", txtShowHide)
            parms(10) = New DBHelper.Parameters("sRptColWidths", txtWidths)
            parms(11) = New DBHelper.Parameters("bActive", chkActive.Checked)
            parms(12) = New DBHelper.Parameters("iRptCols", "0")
            parms(13) = New DBHelper.Parameters("iRptIcon", ddlIcon.SelectedValue)
            parms(14) = New DBHelper.Parameters("sHelpText", txtHelp.Value)
            parms(15) = New DBHelper.Parameters("iuser", Session("UserID"))

            If Session("RptId") = 0 Then
                Session("RptId") = db.DataAdapter(CommandType.StoredProcedure, "SP_IURptMaster", parms).Tables(0).Rows(0)(0).ToString
            Else
                db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IURptMaster", parms)
            End If

            Dim dbR As New DBHelper
            Dim parmsR(1) As DBHelper.Parameters
            parmsR(0) = New DBHelper.Parameters("sPageName", "ReportDetail.aspx")
            parmsR(1) = New DBHelper.Parameters("sGridName", txtTitle.Text)
            db.ExecuteNonQuery(CommandType.StoredProcedure, "ResetGrdCols", parmsR)

            lblMsg.InnerText = " Report Saved."
            BindReport()

        Catch ex As Exception
            'lblMsg.InnerText = Utils.ShowError(ex.Message)
        End Try
    End Sub

    'Protected Sub btnGenGrd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenGrd.Click
    '    BindRpt(txtQuery.Value)
    'End Sub

    Protected Sub BindRpt(ByVal RptSql As String)
        Try
            Dim dbc As New DBHelper
            Dim dbm As New DBHelper
            Dim odt As New DataTable

            If ddlCategory.SelectedValue = "Master" Then
                odt = dbm.DataAdapter(CommandType.Text, RptSql.Replace("::", "'")).Tables(0)
            Else
                odt = dbc.DataAdapter(CommandType.Text, RptSql.Replace("::", "'")).Tables(0)
            End If


            fCount = odt.Columns.Count

            Dim flipdt As New DataTable
            Dim i As Integer = 0
            Dim r As DataRow

            Dim db As New DBHelper
            flipdt = db.DataAdapter(CommandType.Text, "SELECT '' AS FName, '' AS RptColsFormat, '' AS RptShowHide, '' AS RptColWidths, '' AS RptLinks FROM tblRptMaster LIMIT 0  ").Tables(0)

            For i = 0 To odt.Columns.Count - 1
                r = flipdt.NewRow()
                r(0) = odt.Columns(i).ToString
                flipdt.Rows.Add(r)
            Next i

            rptColsDetails.DataSource = flipdt
            rptColsDetails.DataBind()

            If Session("RptId") = 0 Then
            Else
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
            End If

        Catch ex As Exception
            'lblMsg.InnerText = Utils.ShowError(ex.Message)
        End Try

    End Sub

    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("ReportList.aspx?RptCategory=" & Request.QueryString("RptCategory"))
    End Sub
End Class
