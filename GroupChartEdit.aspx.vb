Imports System.Data

Partial Class GroupChartEdit
    Inherits System.Web.UI.Page

#Region "Globalvars"
    Private isEditMode As Boolean = False
    Dim ColFormat As String()
    Dim ColWidth As String()
    Dim ColLinks As String()
    Dim ColShowHide As String()

        Protected Property IsInEditMode() As Boolean
        Get
            Return Me.isEditMode
        End Get

        Set(ByVal value As Boolean)
            Me.isEditMode = value
        End Set
    End Property
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        lblMsg.Text = ""
        lblErrMsg.Text = ""

        If Not Page.IsPostBack = True Then
            Try
                txtChartname.Focus()
                If Request.QueryString("reportid") = 0 Then
                    hdnreportid.Value = 0
                    txtChartname.Text = ""
                Else
                    hdnreportid.Value = Request.QueryString("reportid") 
                    Bindchart()
                End If
            Catch ex As Exception
                lblErrMsg.Text = ex.Message
            End Try
        End If
    End Sub

    Protected Sub btnsubmit_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        Try
            Dim db As New DBHelper
            Dim strSQL As String

             If(txtChartname.Text = "" Or txtCharttitle.Text = "") Then
                lblErrMsg.Text = " Please enter Chart Name and Chart Title "
                Exit Sub
            End If

            Dim parms(2) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("groupname", txtChartname.Text)
            parms(1) = New DBHelper.Parameters("grouptitle", txtCharttitle.Text)
            parms(2) = New DBHelper.Parameters("User", Session("user_id"))

            If hdnreportid.Value = 0 Then
                strSQL = " Insert into tblgroups (groupname, grouptitle, grouptype, createdby, createdon, updatedby, updatedon) " & _
                         " Values ( @groupname, @grouptitle, 'T', @User, CURRENT_TIMESTAMP, @User, CURRENT_TIMESTAMP ) ; select LAST_INSERT_ID() ;"
                hdnreportid.Value = db.ExecuteScalar(CommandType.Text, strSQL,parms)
                lblMsg.Text = " Group chart created successfully "                
            Else
                strSQL = " Update tblgroups set groupname = @groupname, grouptitle = @grouptitle, updatedby = @User, updatedon = CURRENT_TIMESTAMP where groupid = " & hdnreportid.Value
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
                            
                lblMsg.Text = " Group chart details updated successfully "                
            End If
            
        Catch ex As Exception
             lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Protected Sub Bindchart()
        Try
            Dim db As New DBHelper
            Dim parms(1) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_groupid", hdnreportid.Value)

            Dim strSQL As String
            strSQL = " Select groupname, grouptitle from tblgroups where groupid = @p_groupid "
            Dim dt As New DataTable()
            dt = db.DataAdapter(CommandType.Text, strSQL,parms).Tables(0)
            txtChartname.Text = dt.Rows(0).Item("groupname").ToString
            txtCharttitle.Text  = dt.Rows(0).Item("grouptitle").ToString

            strSQL = " Select grouprptrowid, RG.rptid, concat(RptName, ' --- ', RptTitle) as RptName, orderno " & _
                    " from tblgroupsrpt RG inner join tblrptmaster R on RG.rptid = R.RptId " & _
                    " where groupid = @p_groupid  order by orderno "

            ChartReports.DataSource = db.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)
            ChartReports.DataBind()

        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try

    End Sub

    Protected Sub searchCharts_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles searchCharts.ServerClick
        Try
            Dim db As New DBHelper
            Dim strSQL As String
            Dim SrchString As String

            SrchString = pSrchText.Value.ToUpper()
            SrchString = SrchString.Replace("CHART","")

            Dim parms(1) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_groupid", hdnreportid.Value)
            parms(1) = New DBHelper.Parameters("p_RptName", "CHART%" & SrchString & "%")

            strSQL = " SELECT RptId, concat(RptName, ' --- ', RptTitle) as RptName from tblrptmaster " & _
                " WHERE UPPER(RptName) like  @p_RptName and Active = 1 " & _ 
                " and RptId not in (select Rptid from tblgroupsrpt where groupid = " & hdnreportid.Value & " ) " & _
                " LIMIT 100 "

            Dim dt As New DataTable
            dt = db.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)
            If dt.Rows.Count > 0 Then
                ChartAccts.DataSource = dt
                ChartAccts.DataBind()
                ChartAccts.Visible = True
                pNoResults.Visible = False
            Else
                ChartAccts.Visible = False
                pNoResults.Visible = True
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "EditN", "setTimeout('showlist();',250);", True)

        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub ChartAccts_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ChartAccts.ItemCommand
        Try
            Dim db As New DBHelper
            Dim strSQL As String
            Dim maxorderno As String

            btnsubmit_ServerClick(source, e)

            Dim parmss(1) As DBHelper.Parameters
            parmss(0) = New DBHelper.Parameters("p_groupid", hdnreportid.Value)
            strSQL = "SELECT (COALESCE(MAX(orderno), 0) + 1) AS maxorderno FROM tblgroupsrpt where groupid = @p_groupid "
            maxorderno = db.ExecuteScalar(CommandType.Text, strSQL, parmss)

            If e.CommandName = "Add" Then
                Dim parms(2) As DBHelper.Parameters
                 parms(0) = New DBHelper.Parameters("groupid", hdnreportid.Value)
                parms(1) = New DBHelper.Parameters("rptid", e.CommandArgument)
                parms(2) = New DBHelper.Parameters("orderno", maxorderno)

                strSQL = "Insert into tblgroupsrpt (groupid, rptid, orderno) " & _
                            " Values ( @groupid, @rptid, @orderno ) "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
                Bindchart()
            End If

        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub

    Private Sub ChartReports_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles ChartReports.ItemCommand
        Try
            Dim strSQL As String
            Dim db As New DBHelper
            If e.CommandName = "Del" Then
                Dim parms(1) As DBHelper.Parameters
                parms(0) = New DBHelper.Parameters("p_grouprptrowid", e.CommandArgument)
                parms(1) = New DBHelper.Parameters("p_groupid", hdnreportid.Value)
                Dim delorderno As Integer
                delorderno = CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value)
                strSQL = " Delete from tblgroupsrpt where grouprptrowid = @p_grouprptrowid " 
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)


                strSQL = " Select grouprptrowid from tblgroupsrpt where groupid = @p_groupid  and orderno > " & delorderno
                Dim dt As New DataTable
                dt = db.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)
                For i = 0 To dt.Rows.Count - 1
                    strSQL = "Update tblgroupsrpt set orderno = orderno - 1 where grouprptrowid = " & dt.Rows(i).Item("grouprptrowid").ToString
                    db.ExecuteNonQuery(CommandType.Text, strSQL)
                Next

            ElseIf e.CommandName = "Up"
                If CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) = 1 Then
                    lblMsg.Text = "Already at first record."
                Else
                    Dim sqrowidprev As Integer
                    Dim parms(2) As DBHelper.Parameters
                    parms(0) = New DBHelper.Parameters("p_groupid", hdnreportid.Value)
                    parms(1) = New DBHelper.Parameters("p_orderno", CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) - 1)
                    parms(2) = New DBHelper.Parameters("p_grouprptrowid", e.CommandArgument)

                    strSQL = "Select grouprptrowid from tblgroupsrpt where groupid = @p_groupid and orderno = @p_orderno " 
                    sqrowidprev = db.ExecuteScalar(CommandType.Text, strSQL, parms)

                    strSQL = "Update tblgroupsrpt set orderno = @p_orderno where grouprptrowid = @p_grouprptrowid " 
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms)

                    strSQL = "Update tblgroupsrpt set orderno = orderno + 1 where grouprptrowid = " & sqrowidprev 
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
                End If

            ElseIf e.CommandName = "Down"
                Dim parms(2) As DBHelper.Parameters
                parms(0) = New DBHelper.Parameters("p_groupid", hdnreportid.Value)
                parms(1) = New DBHelper.Parameters("p_orderno", CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) + 1)
                parms(2) = New DBHelper.Parameters("p_grouprptrowid", e.CommandArgument)

                Dim hdnMaxQNo As Integer
                strSQL = " Select max(coalesce(orderno, 0))  from tblgroupsrpt where groupid = @p_groupid "
                hdnMaxQNo = db.ExecuteScalar(CommandType.Text, strSQL, parms)

                If CInt(TryCast(e.Item.FindControl("hdnOrder"), HiddenField).Value) = hdnMaxQNo Then
                    lblMsg.Text = "Already at last record."
                Else
                    Dim sqrowidnext As Integer
                    strSQL = "Select grouprptrowid from tblgroupsrpt where groupid = @p_groupid and orderno = @p_orderno " 
                    sqrowidnext = db.ExecuteScalar(CommandType.Text, strSQL, parms)

                    strSQL = "Update tblgroupsrpt set orderno = @p_orderno  where grouprptrowid = @p_grouprptrowid "
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms)

                    strSQL = "Update tblgroupsrpt set orderno = orderno - 1 where grouprptrowid = " & sqrowidnext
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms)

                End If
            End If
            Bindchart()

        Catch ex As Exception
            lblErrMsg.Text = ex.Message
        End Try
    End Sub
End Class
