Imports System.Data
Imports System.Globalization

Partial Class ReportDetail2
    Inherits System.Web.UI.Page

#Region "Public Vars"
    Public Shared s As String = ""
    Public Shared ColFormat As String()
    Public Shared ColWidth As String()
    Public Shared ColLinks As String()
    Public Shared ColLinksfinal As String()
    Public Shared bHasLinks As Boolean = False
    Public Shared ColShowHide As String()
    Public Shared allcols As Decimal()
    Public Shared allcolnames As String()
    Public Shared strdisplayLength As String = "10"
    Public Shared defsortcol As String = ""
    Public Shared defsortdir As String = ""
    Public Shared defsortcolindex As String = 0
    Public Shared firsttime As Integer = 0
    Public Shared rptwidth As String = ""
    Public Shared showtotals As Integer = 0
    'Public Shared fwdURL As String = ""
    Public Shared footerhtml As New StringBuilder
    Public Shared QSYEAR As String = ""
#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            lblErrorMsg.Visible = False

            'setyqm()

            If Not Page.IsPostBack Then
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "loadingscript", "$body2 = $('body');$body2.addClass('loading');", True)
               
                'set pager!
                Dim pagercount As Integer = 0
                Dim db As New DBHelper
                Dim parms(1) As DBHelper.Parameters
                parms(0) = New DBHelper.Parameters("userid", HttpContext.Current.Session("user_id"))
                parms(1) = New DBHelper.Parameters("clientid", HttpContext.Current.Session("client_id"))
                Dim strSQL As String = " Select coalesce(DisCnt, 0) as DisCnt " & _
                    " from tblpagenav where clientid = @clientid " & _ 
                    " And userid = @userid order by PNrowid desc limit 1 "
                pagercount = db.ExecuteScalar(CommandType.Text, strSQL,parms)
                If pagercount > 0 Then
                    For i = 0 To ddlPager.Items.Count - 1
                        ddlPager.Items(i).Selected = False
                    Next i
                    ddlPager.Items.FindByValue(pagercount).Selected = True
                End If

                If Not Request.UrlReferrer Is Nothing Then
                    hdnUrl.Value = Request.UrlReferrer.ToString
                Else
                    hdnUrl.Value = ""
                End If

                Response.Buffer = True
                Response.CacheControl = "no-cache"
                Response.AddHeader("Pragma", "no-cache")
                Response.Expires = -1441
                'test line
                If Not Request.UrlReferrer Is Nothing Then
                    hdnUrl.Value = Request.UrlReferrer.ToString
                End If
                firsttime = 1
                'SSManager.CheckAccess()
                If Request.QueryString("RptName") = "AccountList" Or Request.QueryString("RptId") = "71" Then
                    btnNew.Visible = True
                    btnNew.HRef = "Account.aspx?BindType=A&AcctId=New"
                Else
                    btnNew.Visible = False
                End If
                If Not Request.QueryString("year") Is Nothing Then
                    QSYEAR = Request.QueryString("year")
                End If

                'fwdURL = Mid(HttpContext.Current.Request.QueryString.ToString, InStr(HttpContext.Current.Request.QueryString.ToString, "&") + 1)

                If Session("user_id") Is Nothing Then
                    Response.Redirect("Login.aspx", False)
                End If

                'BindGrdCols()
                'BindFilter()
                'BindVariables()

                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "refreshtablescipt1", "callrefreshtable();", True)

            End If

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try

    End Sub

    Protected Sub setyqm()
        Try
            Dim db As New DBHelper
            Dim dsRptDtl As DataSet
            Dim dtRptData As New DataTable
            Dim strSQL As String = ""
            Dim parms(1) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("rptid", HttpContext.Current.Request.QueryString("RptId"))
            parms(1) = New DBHelper.Parameters("rptname", HttpContext.Current.Request.QueryString("RptName").Split(",")(0))
            If HttpContext.Current.Request.QueryString("RptName") Is Nothing Or HttpContext.Current.Request.QueryString("RptName") = "" Then
                If HttpContext.Current.Request.QueryString("RptId") Is Nothing Or HttpContext.Current.Request.QueryString("RptId") = "0" Then
                    Response.Redirect("Login.aspx")
                Else
                    strSQL = "Select * FROM tblrptmaster where RptId = @rptid "
                End If
            Else
                strSQL = "Select * FROM tblrptmaster where RptName = @rptname "
            End If

            dsRptDtl = db.DataAdapter(CommandType.Text, strSQL,parms)


            'show hide year
            If dsRptDtl.Tables.Count > 0 Then
                If dsRptDtl.Tables(0).Rows.Count > 0 Then
                    If dsRptDtl.Tables(0).Rows(0).Item("ShowYQM").ToString = "1" Then
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "callshowyear", "showyear();", True)
                    Else
                        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "callhideyear", "hideyear();", True)
                    End If
                Else
                    ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "callhideyear", "hideyear();", True)
                End If
            Else
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "callhideyear", "hideyear();", True)
            End If

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "setyqm")
        End Try
    End Sub

    Protected Sub BindGrdCols()
        Try
            Dim i As Integer = 0
            Dim dsRptDtl As DataSet
            Dim RptSQLCols As String = ""
            Dim RptSQL As String = ""
            Dim strSQL As String = ""
            Dim db As New DBHelper
            Dim dbc As New DBHelperClient

            Dim dtRptData As New DataTable

            Dim parms(1) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("rptid", HttpContext.Current.Request.QueryString("RptId"))
            parms(1) = New DBHelper.Parameters("rptname", HttpContext.Current.Request.QueryString("RptName").Split(",")(0))

            If HttpContext.Current.Request.QueryString("RptName") Is Nothing Or HttpContext.Current.Request.QueryString("RptName") = "" Then
                If HttpContext.Current.Request.QueryString("RptId") Is Nothing Or HttpContext.Current.Request.QueryString("RptId") = "0" Then
                    Response.Redirect("Login.aspx")
                Else
                    strSQL = "Select * FROM tblrptmaster where RptId = @rptid "
                End If
            Else
                strSQL = "Select * FROM tblrptmaster where RptName = @rptname "
            End If

            dsRptDtl = db.DataAdapter(CommandType.Text, strSQL,parms)
            If dsRptDtl.Tables.Count > 0 Then
                If dsRptDtl.Tables(0).Rows.Count > 0 Then
                    rptwidth = dsRptDtl.Tables(0).Rows(0).Item("rptwidth").ToString
                    showtotals = CInt(IIf(dsRptDtl.Tables(0).Rows(0).Item("showtotals") Is Nothing, 0, IIf(dsRptDtl.Tables(0).Rows(0).Item("showtotals").ToString = "", 0, dsRptDtl.Tables(0).Rows(0).Item("showtotals"))))

                    lblTitle.Text = dsRptDtl.Tables(0).Rows(0).Item("RptTitle").ToString

                    ColShowHide = dsRptDtl.Tables(0).Rows(0).Item("RptShowHide").ToString.Split(",")

                    RptSQL = dsRptDtl.Tables(0).Rows(0).Item("RptSql").ToString
                    RptSQLCols = Left(RptSQL, InStr(RptSQL.ToLower, " from"))

                    dtRptData = dbc.DataAdapter(CommandType.Text, RptSQL.Replace("::", "'")).Tables(0)

                    Dim dtCols As New DataTable
                    Dim drCols As DataRow = Nothing
                    dtCols.Columns.Add(New DataColumn("GrdCol", GetType(String)))
                    dtCols.Columns.Add(New DataColumn("ShowHide", GetType(String)))

                    Erase allcols
                    Erase allcolnames

                    ReDim allcols(dtRptData.Columns.Count)
                    ReDim allcolnames(dtRptData.Columns.Count)

                    Dim rowfound As Integer
                    For i = 0 To dtRptData.Columns.Count - 1
                        allcolnames(i) = dtRptData.Columns(i).ColumnName
                        If ColShowHide(i) = "Y" Then

                            drCols = dtCols.NewRow()
                            drCols("GrdCol") = dtRptData.Columns(i).ToString

                            strSQL = "Select COALESCE(count(*), 0) from tblgrdcolsu " &
                            " where UserId = @UserId And grdname = @grdname " &
                            " And colname = @colname "

                            Dim parms2(2) As DBHelperClient.Parameters
                            parms2(0) = New DBHelperClient.Parameters("UserId", Session("user_id"))
                            parms2(1) = New DBHelperClient.Parameters("grdname", Request.QueryString("RptName"))
                            parms2(2) = New DBHelperClient.Parameters("colname", dtRptData.Columns(i).ToString)

                            rowfound = dbc.ExecuteScalar(CommandType.Text, strSQL, parms2)
                            If rowfound > 0 Then
                                drCols("ShowHide") = "images/cross.png"
                            Else
                                drCols("ShowHide") = "images/tick.png"
                            End If
                            dtCols.Rows.Add(drCols)

                        End If
                    Next

                    rptGrdShowHide.DataSource = dtCols
                    rptGrdShowHide.DataBind()
                    fddlcols.DataSource = dtCols
                    fddlcols.DataBind()

                    'For index = 0 To dtCols.Rows.Count - 1
                    '    If defsortcol = dtCols.Rows(0)(0).ToString Then
                    '        defsortcolindex = index
                    '        Exit For
                    '    End If
                    'Next
                End If
            End If


            strSQL = ""
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

    Protected Sub BindFilter()
        Try
            Dim strSQL As String
            Dim dbc As New DBHelperClient
            Dim dt As New DataTable
            Dim parms(1) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("userid", Session("User_Id"))
            parms(1) = New DBHelperClient.Parameters("rptname", Request.QueryString("RptName"))
            strSQL = "Select rowid, cond, fieldcol, cond2, fieldvalue from tblgrdfilter " &
                    " where userid = @userid " & _
                    " And grdname = @rptname "
            dt = dbc.DataAdapter(CommandType.Text, strSQL,parms).Tables(0)
            rptfilter.DataSource = dt
            rptfilter.DataBind()
            rptvariables.DataSource = dt
            rptvariables.DataBind()
            If dt.Rows.Count > 0 Then
                fddlcond.Style.Item("display") = "block"
            Else
                fddlcond.Style.Item("display") = "none"
            End If
            fddlcond.ClearSelection()
            fddlcols.ClearSelection()
            fddlcond2.ClearSelection()
            fwhere.Value = ""
            
            Dim parms2(1) As DBHelperClient.Parameters
            parms2(0) = New DBHelperClient.Parameters("userid", Session("User_Id"))
            parms2(1) = New DBHelperClient.Parameters("rptname", Request.QueryString("RptName"))
            strSQL = " SELECT COALESCE(GROUP_CONCAT(WhereClause SEPARATOR ' '),'') AS WhereClause " &
                    " FROM " &
                    " (SELECT CONCAT(cond, ' ', '`', fieldcol, '`', ' ', cond2, ' ' , '''', fieldvalue, '''') AS WhereClause " &
                    " FROM tblgrdfilter WHERE userid = @userid " &
                    " And grdname = @rptname ) AS A"

            Session("hdnwhere") = dbc.ExecuteScalar(CommandType.Text, strSQL,parms2)
            If Session("hdnwhere") = "" Then
            Else
                Dim dispFilter As String
                strSQL = "  SELECT REPLACE(REPLACE(COALESCE(GROUP_CONCAT(WhereClause SEPARATOR ', '),''), 'Like', 'contains'), '%', '') AS WhereClause   " &
                        " FROM " &
                        " (SELECT CONCAT(fieldcol, ' : ', cond2, ' ' , '''', fieldvalue, '''') AS WhereClause " &
                        " FROM tblgrdfilter WHERE userid = " & Session("User_Id") &
                        " And grdname = '" & Request.QueryString("RptName") & "') AS A"

                dispFilter = dbc.ExecuteScalar(CommandType.Text, strSQL,parms2)
                divgear.Attributes.Add("background-color", "#f7761f; !important;")
                lblFilters.Text="Data is filtered based on your settings, " & dispFilter
                lblFilters.Attributes.Add("color", "#f7761f")
            End If

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

    Protected Sub BindVariables()
        Try

            Dim strVar As String = ""
            Dim strSQL As String = ""
            For i = 0 To Request.QueryString.Count - 1
                If Request.QueryString.Keys(i).ToLower.Equals("rptname") Or Request.QueryString.Keys(i).ToLower.Equals("rptid") Then
                Else
                    If i = 1 Then
                        strSQL = "Select '" & Request.QueryString.Keys(i).ToString & "' as fieldcol, '" & Request.QueryString.Item(i).ToString & "' as fieldvalue "
                        strVar = Request.QueryString.Keys(i).ToString & ":" & Request.QueryString.Item(i).ToString
                    Else
                        If strSQL = "" Then
                            strSQL = "Select '" & Request.QueryString.Keys(i).ToString & "' as fieldcol, '" & Request.QueryString.Item(i).ToString & "' as fieldvalue "
                            strVar = Request.QueryString.Keys(i).ToString & ":" & Request.QueryString.Item(i).ToString
                        Else
                            strSQL = strSQL & " Union Select '" & Request.QueryString.Keys(i).ToString & "' as fieldcol, '" & Request.QueryString.Item(i).ToString & "' as fieldvalue "
                            strVar = strVar & " | " & Request.QueryString.Keys(i).ToString & ":" & Request.QueryString.Item(i).ToString
                        End If
                    End If
                End If
            Next


            If strSQL = "" Then
                btnedit.Visible = False
                rptvariables.DataSource = Nothing
                rptvariables.DataBind()
            Else
                Dim dbc As New DBHelperClient
                Dim dt As New DataTable
                dt = dbc.DataAdapter(CommandType.Text, strSQL).Tables(0)
                btnedit.Visible = True
                rptvariables.DataSource = dt
                rptvariables.DataBind()
            End If
            lblVars.Text = strVar.Replace("%20", " ").Replace("%2B", "+").Replace("%2b", "+").Replace("%22", """")

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ConvertDataTabletoJson2(Byval YQMVISIBLE As String,ByVal ddlYQM As String, ByVal ddlQVal As String, ByVal ddlMVal As String, ByVal startdate As String, ByVal enddate As String, ByVal ddlYVal As String, ByVal inputstr As String, ByVal ddlpager As String, ByVal sortcol As String, ByVal sortdir As String, ByVal sortcolname As String, ByVal qString As String) As String()

        Dim retval As String() = New String(2) {}

        'get columns array and send to GetaoColumns
        'get columns def array and send to GetaoColumnDefs
        'get data array and send to GetaaData
        YQMVISIBLE = HttpUtility.UrlDecode(YQMVISIBLE)
        ddlYQM = HttpUtility.UrlDecode(ddlYQM)
        ddlQVal = HttpUtility.UrlDecode(ddlQVal)
        ddlMVal = HttpUtility.UrlDecode(ddlMVal)
        startdate = HttpUtility.UrlDecode(startdate)
        enddate = HttpUtility.UrlDecode(enddate)
        ddlYVal = HttpUtility.UrlDecode(ddlYVal)
        inputstr = HttpUtility.UrlDecode(inputstr)
        ddlpager = HttpUtility.UrlDecode(ddlpager)
        sortcol = HttpUtility.UrlDecode(sortcol)
        sortdir = HttpUtility.UrlDecode(sortdir)
        sortcolname = HttpUtility.UrlDecode(sortcolname)
        qString = HttpUtility.UrlDecode(qString)

        Dim dt As DataTable = getData(YQMVISIBLE,ddlYQM, ddlQVal, ddlMVal, startdate, enddate, ddlYVal, inputstr, ddlpager, sortcol, sortdir, sortcolname, qString)

        Dim s As String = DataTableToJSONWithStringBuilder(dt)
        Dim s2 As String = DataTableToJSONWithStringBuilder2(dt)
        Dim s3 As String = DataTableToJSONWithStringBuilder3(dt)

        Dim sb = New StringBuilder()
        sb.Append("{")
        sb.Append("""aaData"": [")
        sb.Append(s)
        sb.Append("]}")
        retval(0) = sb.ToString()

        sb.Clear()
        sb.Append("{")
        sb.Append("""aoColumns"": [")
        sb.Append(s2)
        retval(1) = sb.ToString()


        sb.Clear()
        sb.Append("{")
        sb.Append("""aoColumnDefs"": [")
        'sb.Append("{ ""targets"": 0, ""width"": ""50px"" , ""className"": ""dt-body-right""   },{ ""targets"": 4, ""width"": ""50px"" , ""type"": ""datetime"" ,""format"": ""DD.MM.Y""  },{ ""targets"": 5, ""width"": ""50px"" , ""type"": ""numeric""  }")
        sb.Append("{ ""targets"": 0, ""width"": ""50px"" , ""className"": ""dt-body-right""   },{ ""targets"": 1, ""width"": ""50px"" , ""className"": ""dt-body-right""   },{ ""targets"": 2, ""width"": ""50px"" , ""className"": ""dt-body-right""   }")
        sb.Append("]}")
        retval(2) = sb.ToString()
        Return retval

    End Function

    Public Shared Function getData(Byval YQMVISIBLE As String,ByVal ddlYQM As String, ByVal ddlQVal As String, ByVal ddlMVal As String, ByVal startdate As String, ByVal enddate As String, ByVal ddlYVal As String, ByVal inputstr As String, ByVal ddlpager As String, ByVal sortcol As String, ByVal sortdir As String, ByVal sortcolname As String, ByVal qString As String) As DataTable

        Dim dbc As New DBHelperClient	
        Dim SqlString As String = ""
        Try

            SqlString = "SELECT id,firstname,lastname FROM tblcontact where coalesce(id,'') <> '' limit 10;"

        Catch ex As Exception

        End Try
	    return dbc.DataAdapter(CommandType.Text,SqlString).Tables(0)
    End Function


    Public Shared Function DataTableToJSONWithStringBuilder(table As DataTable) As String
	Dim JSONString = New StringBuilder()
	If table.Rows.Count > 0 Then
		'JSONString.Append("[");
		For i As Integer = 0 To table.Rows.Count - 1
			JSONString.Append("[")
			For j As Integer = 0 To table.Columns.Count - 1
				If j < table.Columns.Count - 1 Then
					'JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\",");
					JSONString.Append("""" + table.Rows(i)(j).ToString() + """,")
				ElseIf j = table.Columns.Count - 1 Then
					'JSONString.Append("\"" + table.Columns[j].ColumnName.ToString() + "\":" + "\"" + table.Rows[i][j].ToString() + "\"");
					JSONString.Append("""" + table.Rows(i)(j).ToString() + """")
				End If
			Next
			If i = table.Rows.Count - 1 Then
				JSONString.Append("]")
			Else
				JSONString.Append("],")
			End If
		Next
		'JSONString.Append("]");
	End If
	Return JSONString.ToString()
End Function
Public Shared Function DataTableToJSONWithStringBuilder2(table As DataTable) As String
	Dim JSONString = New StringBuilder()
	If table.Rows.Count > 0 Then
		'JSONString.Append("[");
		For i As Integer = 0 To table.Columns.Count - 1
			JSONString.Append("[")
			'JSONString.Append("\"sTitle\":" + "\"" + table.Columns[i].ColumnName.ToString() + "\"");
			JSONString.Append("""" + table.Columns(i).ColumnName.ToString() + """")
			If i = table.Columns.Count - 1 Then
				JSONString.Append("]")
			Else
				JSONString.Append("],")
			End If
		Next
		JSONString.Append("]}")
		'JSONString.Append("]");
	End If
	Return JSONString.ToString()
End Function
Public Shared Function DataTableToJSONWithStringBuilder3(table As DataTable) As String
	Dim JSONString = New StringBuilder()
	If table.Rows.Count > 0 Then
		'JSONString.Append("[");
		For i As Integer = 0 To table.Columns.Count - 1
			JSONString.Append("[")
			'JSONString.Append("\"sTitle\":" + "\"" + table.Columns[i].ColumnName.ToString() + "\"");
			JSONString.Append("""" + table.Columns(i).ColumnName.ToString() + """")
			If i = table.Columns.Count - 1 Then
				JSONString.Append("]")
			Else
				JSONString.Append("],")
			End If
		Next
		JSONString.Append("]}")
		'JSONString.Append("]");
	End If
	Return JSONString.ToString()
End Function



    <System.Web.Services.WebMethod(EnableSession:=True)>
    Public Shared Function ConvertDataTabletoJson(Byval YQMVISIBLE As String,ByVal ddlYQM As String, ByVal ddlQVal As String, ByVal ddlMVal As String, ByVal startdate As String, ByVal enddate As String, ByVal ddlYVal As String, ByVal inputstr As String, ByVal ddlpager As String, ByVal sortcol As String, ByVal sortdir As String, ByVal sortcolname As String, ByVal qString As String) As String()

        Dim items As String() = New String(4) {}
        Dim sortcoljqdt As String = ""
        Try

            If sortdir.ToLower.Contains("asc") Then
                sortdir = "asc"
            Else
                sortdir = "desc"
            End If
            
            ''save sortcol and sortdir to DB
            'Dim db As New DBHelper
            'Dim parms(5) As DBHelper.Parameters
            'parms(0) = New DBHelper.Parameters("p_iclientid", HttpContext.Current.Session("client_id"))
            'parms(1) = New DBHelper.Parameters("p_iuserid", HttpContext.Current.Session("user_id"))
            'parms(2) = New DBHelper.Parameters("p_sNavURL", HttpContext.Current.Request.UrlReferrer.ToString)
            'parms(3) = New DBHelper.Parameters("p_sBrowser", HttpContext.Current.Request.Browser.Browser)
            'parms(4) = New DBHelper.Parameters("p_sortcol", sortcol)
            'parms(5) = New DBHelper.Parameters("p_sortdir", sortdir)
            'db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_UPDATEIPAGENAVWITHSORT", parms)
            

            Dim records As DataSet
            records = BindGrids(ddlYQM, ddlQVal, ddlMVal, startdate, enddate, ddlYVal, inputstr, ddlpager, sortcol, sortdir, sortcolname, qString)

            items(0) = ConvertDataTabletoString(YQMVISIBLE,ddlYQM, ddlQVal, ddlMVal, startdate, enddate, ddlYVal, records.Tables(0), sortcol, sortdir, records.Tables(1).Rows(0)(0), sortcoljqdt, qString)
            items(1) = rptwidth '+ "%"
            items(2) = showtotals
            items(3) = footerhtml.ToString()

        Catch ex As Exception
            SSManager.LogAppError(ex, "ConvertDataTabletoJson")
        End Try

        Return items

    End Function

    Public Shared Function ConvertDataTabletoString(ByVal YQMVISIBLE As String,ByVal ddlYQM As String, ByVal ddlQVal As String, ByVal ddlMVal As String, ByVal startdate As String, ByVal enddate As String, ByVal ddlYVal As String, ByVal dt As DataTable, ByVal sortcol As String, ByVal sortdir As String, ByVal reccount As Integer, ByVal sortcoljqdt As String, ByVal qString As String) As String
        Dim str As New StringBuilder
        Dim strInitial As New StringBuilder
        Dim strFinal As New StringBuilder
        Dim strData As New StringBuilder
        Dim strColumnDefs As New StringBuilder

        Dim strcolval As String = ""
        Dim serializer As New System.Web.Script.Serialization.JavaScriptSerializer()
        Dim i As Integer = 0
        Dim usercolhide() As Integer
        ReDim usercolhide(100)
        Dim cnt As Integer = 0
        Dim strSQL As String = ""
        Dim rowfound As Integer = 0
        Dim dbc As New DBHelperClient
        Dim bHasdata As Boolean = False

        Try
            'get index based on sort column defined in the report!
            'sortdir = Array.IndexOf(allcols, defsortcol)

            If firsttime = 1 Then
                If defsortdir <> "" Then
                    sortdir = defsortdir
                End If
                If defsortcol <> "" Then
                    sortcol = """" + defsortcolindex + """"
                End If
                firsttime = 0
            Else
                sortcol = """" + sortcol + """"
            End If


            'str.Append("{ ""dom"": ""'Bfrtip'"", ""buttons"": ""[{extend: 'pdf',title: 'Save current page'}]"", ""bInfo"": true, ""bPaginate"": false, ""order"": [ 0, ""asc"" ], ""bLengthChange"": false, ""language"": {" & _
            strInitial.Append("{ ""deferRender"": true, ""dom"": ""'Bfrtip'"", ""buttons"": ""[{extend: 'pdf',title: 'Save current page'}]"",""bAutoWidth"": false, ""bInfo"": true, " &
                            " ""bPaginate"": false, ""order"": [[ " & sortcol & ", """ & sortdir & """ ]], ""bLengthChange"": false, ""language"": {" &
                            " ""zeroRecords"": ""No records found for entered criteria."", " &
                            " ""sInfo"": ""Showing _START_ to _END_ From " + reccount.ToString + " entries."" " &
                            " }, " &
                            " ""columns"": [")
            '" ""infoEmpty"": ""No records available"", " & _

            Dim strQueryString As String = ""
            strQueryString = HttpUtility.UrlDecode(qString.Replace("+", "%2b"))

            For Each col As DataColumn In dt.Columns
                If ColShowHide(i) <> "N" Then
                    strSQL = "Select COALESCE(count(*), 0) from tblgrdcolsu " &
                " where UserId = @UserId And grdname = @grdname And colname = @colname "

                    Dim parms(2) As DBHelperClient.Parameters
                    parms(0) = New DBHelperClient.Parameters("UserId", HttpContext.Current.Session("user_id"))
                    parms(1) = New DBHelperClient.Parameters("grdname", strQueryString.Split("=")(1).Split(",")(0))
                    parms(2) = New DBHelperClient.Parameters("colname", col.ToString)

                    rowfound = dbc.ExecuteScalar(CommandType.Text, strSQL, parms)
                    usercolhide(cnt) = rowfound
                    cnt = cnt + 1
                    If rowfound > 0 Then
                    Else
                        bHasdata = True
                        str.Append("[")
                        str.Append("""" + col.ColumnName + """")
                        str.Append("],")
                    End If
                End If
                i += 1
            Next
            If bHasdata Then
                str.Remove(str.Length - 1, 1)
            End If

            i = 0
            str.Append("],")

            Dim strDateTargetDD As String = ""
            Dim strDateTargetIndexDD As Integer = 0
            Dim strDateTargetDT As String = ""
            Dim strDateTargetIndexDT As Integer = 0

            strData.Append("""data"": [")
            Dim n As Decimal = 0
            i = 0
            Erase allcols
            ReDim allcols(dt.Columns.Count)
            Redim allcolnames(dt.Columns.Count)
            For Each dr As DataRow In dt.Rows
                cnt = 0
                strData.Append("[")
                For Each col As DataColumn In dt.Columns
                    allcolnames(i) = dt.Columns(i).ColumnName
                    If ColShowHide(i) = "Y" Then
                        If usercolhide(cnt) > 0 Then
                        Else
                            Select Case ColFormat(i)
                                Case "TL"
                                    strcolval = dr(col).ToString
                                Case "TR"
                                    strcolval = "<div style='float:right'>" + dr(col).ToString + "</div>"
                                Case "TC"
                                    strcolval = "<div style='float:left;position: relative;left: 50%;'><div style='float:left;position: relative;left: -50%;'>" + dr(col).ToString + "</div></div>"
                                Case "DD"
                                    strcolval = IIf(dr(col).Equals(""), dr(col), String.Format("{0:M/d/yyyy}", dr(col))).ToString
                                    If strDateTargetDD = "" Then
                                        strDateTargetDD = strDateTargetDD + strDateTargetIndexDD.ToString() + ","
                                        strDateTargetIndexDD += 1
                                    End If
                                Case "DT"
                                    strcolval = dr(col).ToString
                                    If strDateTargetDT = "" Then
                                        strDateTargetDT = strDateTargetDT + strDateTargetIndexDT.ToString() + ","
                                        strDateTargetIndexDT += 1
                                    End If
                                Case "D0"
                                    Dim strDval As String = ""
                                    If dr(col).Equals(System.DBNull.Value) Then
                                        strDval = "0"
                                    ElseIf dr(col).ToString.Trim.Equals("") Then
                                        strDval = "0"
                                    ElseIf IsNumeric(dr(col)) Then
                                        strDval = dr(col)
                                    Else
                                        strDval = "0"
                                    End If
                                    strcolval = "<div style='float:right'>" + String.Format("{0:n0}", CDbl(strDval)) + "</div>"
                                    allcols(i) = allcols(i) + IIf(Decimal.TryParse(IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col)), n), IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col)), "0")
                                Case "D2"
                                    Dim strDval As String = ""
                                    If dr(col).Equals(System.DBNull.Value) Then
                                        strDval = "0"
                                    ElseIf dr(col).ToString.Trim.Equals("") Then
                                        strDval = "0"
                                    ElseIf IsNumeric(dr(col)) Then
                                        strDval = dr(col)
                                    Else
                                        strDval = "0"
                                    End If
                                    strcolval = "<div style='float:right'>" + String.Format("{0:N2}", IIf(Decimal.TryParse(strDval, n), strDval, "0")) + "</div>"
                                    allcols(i) = allcols(i) + IIf(Decimal.TryParse(strDval, n), strDval, "0")
                                Case "C0"
                                    strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0}", IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col))) + "</div>"
                                    allcols(i) = allcols(i) + IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col))
                                Case "C2"
                                    strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0.00}", IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col))) + "</div>"
                                    allcols(i) = allcols(i) + IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col))
                                Case "C4"
                                    strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0.0000}", IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col))) + "</div>"
                                    allcols(i) = allcols(i) + IIf(dr(col).Equals(System.DBNull.Value), "0", dr(col))
                                Case Else
                                    strcolval = dr(col).ToString
                            End Select
                            strcolval = strcolval.Replace("""", "\""")
                            If ColLinks(i) = "" Then
                                strData.Append("""" + strcolval.ToString() + """,")
                            Else
                                'loop to replace querystring params and create URL!
                                Dim newurl As String = ""
                                Dim strURL As String = ColLinks(i).TrimStart("!")
                                'assumption - first param is reportname! so append ignore it!!
                                newurl = strURL.Split("&")(0) + "&"

                                For index = 1 To ColLinks(i).TrimStart("!").Split("&").Length - 1
                                    'If fwdURL.Contains(ColLinks(i).TrimStart("!").Split("&")(index)) Then
                                    '    strURL = strURL.Replace("&" & ColLinks(i).TrimStart("!").Split("&")(index), "")
                                    'Else
                                        'strURL = ColLinks(i).TrimStart("!").Split("&")(0) + strURL.Substring(ColLinks(i).TrimStart("!").Split("&")(0).Length).Replace(ColLinks(i).TrimStart("!").Split("&")(index), ColLinks(i).TrimStart("!").Split("&")(index).ToString + "=" + dr(ColLinks(i).TrimStart("!").Split("&")(index)).ToString)
                                        If ColLinks(i).TrimStart("!").Split("&")(index).Contains("=") Then
                                            'strURL = strURL.Replace(ColLinks(i).TrimStart("!").Split("&")(index), ColLinks(i).TrimStart("!").Split("&")(index).ToString + "=" + HttpUtility.UrlEncode(dr(ColLinks(i).TrimStart("!").Split("&")(index)).ToString.Replace(" ", "%20")).ToString)
                                            newurl = newurl + ColLinks(i).TrimStart("!").Split("&")(index).Replace(" ", "%20").ToString + "&"
                                        Else
                                            'strURL = strURL.Replace(ColLinks(i).TrimStart("!").Split("&")(index), ColLinks(i).TrimStart("!").Split("&")(index).ToString + "=" + HttpUtility.UrlEncode(dr(ColLinks(i).TrimStart("!").Split("&")(index)).ToString.Replace(" ", "%20")).ToString)
                                            newurl = newurl + ColLinks(i).TrimStart("!").Split("&")(index) + "=" + HttpUtility.UrlEncode(dr(ColLinks(i).TrimStart("!").Split("&")(index)).ToString).ToString + "&"
                                            'HttpUtility.UrlEncode(dr(ColLinks(i).TrimStart("!").Split("&")(index)).ToString.Replace(" ", "%20")).ToString
                                        End If
                                    'End If
                                Next
                                If newurl.EndsWith("&") Then
                                    newurl = newurl.Substring(0,newurl.Length-1)
                                End If

                                'strURL += "&" & HttpUtility.UrlDecode(fwdURL)
                                strURL = newurl

                                If YQMVISIBLE = "block" Then
                                    If ddlYQM = "Year" Then
                                        strURL = strURL + "&year=" + ddlYVal
                                    End If
                                    If ddlYQM = "Quarter" Then
                                        strURL = strURL + "&year=" + ddlYVal
                                        strURL = strURL + "&quarter=" + ddlQVal
                                    End If
                                    If ddlYQM = "Month" Then
                                        strURL = strURL + "&year=" + ddlYVal
                                        strURL = strURL + "&month=" + ddlMVal
                                    End If
                                End If

                                'append the existing querystring at the end after removing the rptname from it!
                                If qString.Contains("&") Then
                                    strURL = strURL + qString.Substring(qString.IndexOf("&"))
                                End If

                                strURL = strURL.Replace("+", "%2B")
                                'strURL = Replace(strURL, "&", "?", , 1)
                                If strURL.Contains(".aspx") Then
                                    strURL = Replace(strURL, "&", "?", , 1)
                                    strData.Append("""<a href='" + strURL + "'>" + strcolval.ToString() + "</a>"",")
                                ElseIf Left(strURL, 1) = "@" Then
                                    'strData.Append("""<a href='GroupReportDetail.aspx?rptname=" + strURL.Substring(1) + "'>" + strcolval.ToString() + "</a>"",")
                                    strData.Append("""<a href='GroupReportDetail.aspx?RptName=" + Mid(strURL, 2) + "'>" + strcolval.ToString() + "</a>"",")
                                Else
                                    If strURL.Split("&")(0).Tolower().Equals("rptname") Then
                                        strData.Append("""<a href='ReportDetail.aspx?rptname=" + dr("RptName").ToString() + "'>" + strcolval.ToString() + "</a>"",")
                                    Else
                                        strData.Append("""<a href='ReportDetail.aspx?rptname=" + strURL + "'>" + strcolval.ToString() + "</a>"",")
                                    End If
                                End If
                            End If
                        End If
                        cnt = cnt + 1
                    End If
                    i += 1
                Next
                If strData.ToString.EndsWith(",") Then
                    strData.Remove(strData.Length - 1, 1)
                End If
                strData.Append("],")
                If strData.ToString.EndsWith("[],") Then
                    strData.Remove(strData.Length - 3, 3)
                End If
                i = 0
            Next
            'append total row if applicable- show to sum of int columns!!
            'build this html!!! <tfoot><tr><th>Total <div style="float:right">9000</div></th><th></th></tr></tfoot>
            Dim firsttime2 As Boolean = True
            footerhtml.Clear()

            If showtotals = 1 Then
                cnt = 0
                footerhtml.Append("<tfoot><tr>")
                For Each col As DataColumn In dt.Columns
                    If ColShowHide(i) = "Y" Then
                        If usercolhide(cnt) > 0 Then
                        Else
                            If firsttime2 Then
                                footerhtml.Append("<th><b>Total</b> ")
                                firsttime2 = False
                            Else
                                footerhtml.Append("<th>")
                            End If
                            Select Case ColFormat(i)
                                Case "D0"
                                    If cnt = 0 Then
                                        strcolval = "<div style='float:right'>" + String.Format("{0:n0}", allcols(i)).ToString() + "</div>"
                                    Else
                                        strcolval = "<div style='float:right'>" + String.Format("{0:n0}", allcols(i)).ToString() + "</div>"
                                    End If
                                Case "D2"
                                    If cnt = 0 Then
                                        strcolval = "<div style='float:right'>" + String.Format("{0:N2}", allcols(i)).ToString() + "</div>"
                                    Else
                                        strcolval = "<div style='float:right'>" + String.Format("{0:N2}", allcols(i)).ToString() + "</div>"
                                    End If
                                Case "C0"
                                    If cnt = 0 Then
                                        strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0}", allcols(i)).ToString() + "</div>"
                                    Else
                                        strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0}", allcols(i)).ToString() + "</div>"
                                    End If

                                Case "C2"
                                    If cnt = 0 Then
                                        strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0.00}", allcols(i)).ToString() + "</div>"
                                    Else
                                        strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0.00}", allcols(i)).ToString() + "</div>"
                                    End If

                                Case "C4"
                                    If cnt = 0 Then
                                        strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0.0000}", allcols(i)).ToString() + "</div>"
                                    Else
                                        strcolval = "<div style='float:right'>" + String.Format("{0:$#,##0.0000}", allcols(i)).ToString() + "</div>"
                                    End If

                                Case Else
                                    strcolval = ""
                            End Select
                            footerhtml.Append(strcolval + "</th>")
                        End If
                        cnt = cnt + 1
                    End If
                    i += 1
                Next
                footerhtml.Append("</tr></tfoot>")
            End If
            footerhtml.ToString().TrimStart("{").TrimEnd("}").ToString()
            'If str.ToString.EndsWith(",") Then
            '    str.Remove(str.Length - 1, 1)
            'End If
            'str.Append("] ")

            strDateTargetDT = strDateTargetDT + strDateTargetDD
            If strDateTargetDT.ToString.EndsWith(",") Then
                strDateTargetDT = strDateTargetDT.Remove(strDateTargetDT.Length - 1, 1)
            End If
            '----------------------------------------------------------------------------------------------------------------------------------------
            strColumnDefs.Append("""columnDefs"": [")
            i = 0
            Dim haswidth As Integer = 0
            For Each col As DataColumn In dt.Columns
                If ColShowHide(i) = "Y" Then
                    If ColWidth(i) <> "" Then
                        strColumnDefs.Append("{ ""width"": """ + ColWidth(i).ToString().Replace(" ", "") + """, ""targets"": " + i.ToString() + " },")
                        haswidth = 1
                    End If
                End If
                i += 1
            Next
            If strColumnDefs.ToString.EndsWith(",") Then
                strColumnDefs.Remove(strColumnDefs.Length - 1, 1)
            End If
            If strDateTargetDT <> "" Then
                If haswidth = 1 Then
                    'strColumnDefs.Append(", { ""type"": ""extract-date"", ""targets"": [" + strDateTargetDT + "] } ")
                    strColumnDefs.Append(", { ""type"": ""extract-date"", ""targets"": [""" + allcolnames(strDateTargetDT) + """] } ")
                Else
                    'strColumnDefs.Append(" { ""type"": ""extract-date"", ""targets"": [" + strDateTargetDT + "] } ")
                    strColumnDefs.Append(" { ""type"": ""extract-date"", ""targets"": [""" + allcolnames(strDateTargetDT) + """] } ")
                End If
                sortcol = strDateTargetDT
                strDateTargetDT = ""
            End If
            If strColumnDefs.ToString.EndsWith(",") Then
                strColumnDefs.Remove(strColumnDefs.Length - 1, 1)
            End If
            '----------------------------------------------------------------------------------------------------------------------------------------
            strFinal.Append(strInitial.ToString())
            strFinal.Append(str)

            strFinal.Append(strColumnDefs.ToString())
            strFinal.Append("],")

            If strData.ToString.EndsWith(",") Then
                strData.Remove(strData.Length - 1, 1)
            End If
            strFinal.Append(strData.ToString())

            strFinal.Append("]")
            strFinal.Append(" }")



        Catch ex As Exception
            SSManager.LogAppError(ex, "ConvertDataTabletoString")
        End Try
        Return Regex.Replace(Regex.Replace(strFinal.ToString, "\r\n?|\n", "<br/>"), "\t", " ").Replace("\", "\\").Replace("\\""", "\""")
    End Function

    Protected Shared Function BindGrids(ByVal ddlYQM As String, ByVal ddlQVal As String, ByVal ddlMVal As String, ByVal startdate As String, ByVal enddate As String, ByVal ddlYVal As String, ByVal inputstr As String, ByVal ddlpager As String, ByVal sortcol As String, ByVal sortdir As String, ByVal sortcolname As String, ByVal qString As String) As DataSet
        Dim dsRptDtl As DataSet
        Dim RptSQL As String = ""
        Dim RptSQLCount As String = ""
        Dim RptSQLSaved As String = ""

        Dim RptSQLOrig As String = ""
        Dim strSQL As String = ""
        Dim db As New DBHelper
        Dim strQueryString As String = ""

        Dim dtRptData As New DataSet
        Try
            'replace %2B with space! this issue is for + sign. %2B gets decoded as + which we don't want!
            qString = qString.Replace("%2b", " ").Replace("%2B", " ")
            'strQueryString = System.Web.HttpUtility.UrlDecode(qString)
            strQueryString = HttpUtility.UrlDecode(qString)
            strQueryString = strQueryString.Replace("%2b", "+").Replace("%2B", "+")
            strQueryString = strQueryString.Replace("%22", """").Replace("%20", " ")


            Dim strReportName As String = strQueryString.Split("&")(0).Split("=")(1)
            Dim parms(0) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("rptnameid", strReportName.TrimEnd("#"))
            If strReportName Is Nothing Or strReportName = "" Then
                HttpContext.Current.Response.Redirect("Login.aspx")
            Else
                If System.Web.HttpUtility.UrlDecode(qString).Split("&")(0).Split("=")(0).ToLower.Contains("rptid") Then
                    strSQL = "Select * FROM tblrptmaster where RptId = @rptnameid "
                Else
                    strSQL = "Select * FROM tblrptmaster where RptName = @rptnameid "
                End If
            End If
            dsRptDtl = db.DataAdapter(CommandType.Text, strSQL,parms)

            ColFormat = dsRptDtl.Tables(0).Rows(0).Item("RptColsFormat").ToString.Split(",")
            ColWidth = dsRptDtl.Tables(0).Rows(0).Item("RptColWidths").ToString.Split(",")
            ColLinks = dsRptDtl.Tables(0).Rows(0).Item("RptLinks").ToString.Split(",")
            ColShowHide = dsRptDtl.Tables(0).Rows(0).Item("RptShowHide").ToString.Split(",")

            defsortcol = dsRptDtl.Tables(0).Rows(0).Item("defsortcol").ToString()
            defsortdir = dsRptDtl.Tables(0).Rows(0).Item("defsortdir").ToString()
            '------------------------   new sorting START ----------------------------------------
            Dim dsRptSort As DataSet
            Dim parms2(1) As DBHelper.Parameters
            parms2(0) = New DBHelper.Parameters("userid", HttpContext.Current.Session("user_id"))
            parms2(1) = New DBHelper.Parameters("clientid", HttpContext.Current.Session("client_id"))
            strSQL = " Select PNrowid, coalesce(ColSort, '') as ColSort, coalesce(ColDir, '') as ColDir, coalesce(DisCnt, 0) as DisCnt " & _
                    " from tblpagenav where clientid = @clientid " & _
                    " And userid = @userid order by PNrowid desc limit 1 "
            dsRptSort = db.DataAdapter(CommandType.Text, strSQL,parms2)

            Dim parms3(0) As DBHelper.Parameters
            parms3(0) = New DBHelper.Parameters("ddlpager", ddlpager)
            If sortcolname = "" Then
                If dsRptSort.Tables(0).Rows(0).Item("ColSort").ToString = "" Then
                    strSQL = " Update tblpagenav set ColSort = '" & defsortcol & "', ColDir = '" & defsortdir & "', Discnt = @ddlpager " & _ 
                            " where PNrowid = " & dsRptSort.Tables(0).Rows(0).Item("PNrowid").ToString
                    db.ExecuteNonQuery(CommandType.Text, strSQL,parms3)
                Else 
                    defsortcol = dsRptSort.Tables(0).Rows(0).Item("ColSort").ToString()
                    defsortdir = dsRptSort.Tables(0).Rows(0).Item("ColDir").ToString()
                    If ddlpager = "50" Then
                        ddlpager = dsRptSort.Tables(0).Rows(0).Item("DisCnt").ToString()
                    Else
                        'update tblpagenav 
                        strSQL = " Update tblpagenav set Discnt = @ddlpager where PNrowid = " & dsRptSort.Tables(0).Rows(0).Item("PNrowid").ToString
                        db.ExecuteNonQuery(CommandType.Text, strSQL,parms3)
                    End If
                End If
            Else
                defsortcol = sortcolname
                defsortdir = sortdir
                strSQL = " Update tblpagenav set ColSort = '" & defsortcol & "', ColDir = '" & defsortdir & "', Discnt = @ddlpager " & _ 
                        " where PNrowid = " & dsRptSort.Tables(0).Rows(0).Item("PNrowid").ToString
                db.ExecuteNonQuery(CommandType.Text, strSQL,parms3)
            End If
            '------------------------   new sorting END   ----------------------------------------

            Dim k As Integer = -1
            For index = 0 To ColShowHide.Length - 1
                If ColShowHide(index) = "Y" Then
                    k += 1
                    If defsortcol = allcolnames(index) Then
                        defsortcolindex = k
                        Exit For
                    End If
                End If
            Next


            RptSQL = dsRptDtl.Tables(0).Rows(0).Item("RptSql").ToString
            RptSQLOrig = RptSQL
            'Session("RptSQL") = RptSQL
            'Session("RptName") = dsRptDtl.Tables(0).Rows(0).Item("RptTitle").ToString

            'add YQM based on selection!
            Select Case ddlYQM
                Case "Quarter"
                    'replace year with 1=1 and replace ST and SF with Q and M
                    If RptSQL.Contains("::ST::") Then
                        RptSQL = RptSQL.Replace("::ST::", ddlYQM)
                        RptSQL = RptSQL.Replace("::SF::", "'" + ddlQVal + "'")
                    End If
                Case "Month"
                    'replace year with 1=1 and replace ST and SF with Q and M
                    If RptSQL.Contains("::ST::") Then
                        RptSQL = RptSQL.Replace("::ST::", ddlYQM)
                        RptSQL = RptSQL.Replace("::SF::", "'" + ddlMVal + "'")
                    End If
                Case "Range"
                    If RptSQL.Contains("::SD::") And (startdate <> "") Then
                        'Dim strdate1 As String = hdnDateRange.Substring(0, hdnDateRange.IndexOf(" - ")).Replace(",", "")
                        'Dim dt1 As String = Date.ParseExact(strdate1, "MMMM d yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                        'Dim strdate2 As String = hdnDateRange.Substring(hdnDateRange.IndexOf(" - ") + 3).Replace(",", "")
                        'Dim dt2 As String = Date.ParseExact(strdate2, "MMMM d yyyy", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd")
                        RptSQL = RptSQL.Replace("::SD::", "'" + startdate + "'")
                        RptSQL = RptSQL.Replace("::ED::", "'" + enddate + "'")
                    End If
                Case Else
                    If RptSQL.Contains(".::ST::") Then
                        Dim newstr As String = ""
                        Dim juststr As String = ""
                        newstr = RptSQL.Substring(0, RptSQL.IndexOf(".::ST::"))
                        juststr = StrReverse(newstr).ToString.Substring(0, StrReverse(RptSQL).ToString.IndexOf(" "))
                        juststr = StrReverse(juststr)
                        If juststr.Substring(0, 1) = "(" Then
                            juststr = juststr.Substring(1, Len(juststr) - 1)
                        End If
                        RptSQL = Replace(RptSQL, juststr + "." + "::ST::", 1)
                        RptSQL = RptSQL.Replace("::SF::", "1")
                    End If
            End Select
            'replace year
            If RptSQL.ToLower.Contains("::year::") Then
                If QSYEAR <> "" Then
                    RptSQL = RptSQL.Replace("::year::", QSYEAR)
                    RptSQL = RptSQL.Replace("::Year::", QSYEAR)
                    RptSQL = RptSQL.Replace("::YEAR::", QSYEAR)
                Else
                    RptSQL = RptSQL.Replace("::year::", ddlYVal)
                    RptSQL = RptSQL.Replace("::Year::", ddlYVal)
                    RptSQL = RptSQL.Replace("::YEAR::", ddlYVal)
                End If
            End If

            RptSQL = RptSQL.Replace("::SD::", "1900-01-01")
            RptSQL = RptSQL.Replace("::ED::", "2100-01-01")

            RptSQL = RptSQL.Replace("::ST::", "1")
            RptSQL = RptSQL.Replace("::SF::", "1")


            Dim dbR As New DBHelperClient
            'append limit !
            Dim RptSQL2 As String = ""
            'Dim strLimit As String = daterangeval

            RptSQL = RptSQL.Replace(";", "")

            'add search criteria...if any...using HAVING!
            Dim strHaving As String = ""
            If HttpContext.Current.Request.Params("columns") <> "" Then
                For i = 0 To HttpContext.Current.Request.Params("iColumns") - 1
                    If Not HttpContext.Current.Request.Params("sSearch_" + i.ToString) Is Nothing Then
                        If HttpContext.Current.Request.Params("sSearch_" + i.ToString) <> "" Then
                            If strHaving <> "" Then
                                Select Case HttpContext.Current.Request.Params("sSearch_" + i.ToString).Trim.Substring(0, 1).ToLower
                                    Case ">", "<", "="
                                        If HttpContext.Current.Request.Params("sSearch_" + i.ToString).Trim.Length > 1 Then
                                            strHaving = strHaving + " AND `" + HttpContext.Current.Request.Params("columns").Split(",")(i).Trim("""") + "`  " + HttpContext.Current.Request.Params("sSearch_" + i.ToString)
                                        End If
                                    Case Else 'like case
                                        strHaving = strHaving + " AND `" + HttpContext.Current.Request.Params("columns").Split(",")(i).Trim("""") + "` like '%" + HttpContext.Current.Request.Params("sSearch_" + i.ToString) + "%'"
                                End Select
                            Else
                                Select Case HttpContext.Current.Request.Params("sSearch_" + i.ToString).Trim.Substring(0, 1).ToLower
                                    Case ">", "<", "="
                                        If HttpContext.Current.Request.Params("sSearch_" + i.ToString).Trim.Length > 1 Then
                                            strHaving = strHaving + "  `" + HttpContext.Current.Request.Params("columns").Split(",")(i).Trim("""") + "`  " + HttpContext.Current.Request.Params("sSearch_" + i.ToString)
                                        End If
                                    Case Else 'like case
                                        strHaving = strHaving + "  `" + HttpContext.Current.Request.Params("columns").Split(",")(i).Trim("""") + "` like '%" + HttpContext.Current.Request.Params("sSearch_" + i.ToString) + "%'"
                                End Select
                            End If
                        End If
                    End If
                Next
            End If

            'add order by
            If strHaving <> "" Then
                RptSQL = RptSQL + " having " + strHaving
            End If

            'replace querystring params, if any!
            'If System.Web.HttpUtility.UrlDecode(qString).Split("&").Count > 1 Then
            '    For index = 1 To qString.Split("&").Count - 1
            '        RptSQL = RptSQL.Replace("::" + System.Web.HttpUtility.UrlDecode(qString).Split("&")(index).Split("=")(0) + "::", "'" + System.Web.HttpUtility.UrlDecode(qString).Split("&")(index).Split("=")(1).Trim() + "'")
            If strQueryString.Split("&").Count > 1 Then
                For index = 1 To strQueryString.Split("&").Count - 1
                    RptSQL = RptSQL.Replace("::" + strQueryString.Split("&")(index).Split("=")(0) + "::", "'" + strQueryString.Split("&")(index).Split("=")(1).Trim() + "'")
                Next
            End If

            Dim strPair As String()
            If inputstr <> "" Then
                strPair = inputstr.Split("|")
            End If
            Dim tmpWhere As String = ""
            If Not strPair Is Nothing Then
                For index = 0 To strPair.Length - 2
                    If strPair(index).ToString().Split("=")(1) <> "" Then
                        If tmpWhere <> "" Then
                            tmpWhere += " and `" + strPair(index).ToString().Split("=")(0) + "` like '%" + strPair(index).ToString().Split("=")(1) + "%' "
                        Else
                            tmpWhere = " `" + strPair(index).ToString().Split("=")(0) + "` like '%" + strPair(index).ToString().Split("=")(1) + "%' "
                        End If
                    End If
                Next
            End If

            RptSQLSaved = RptSQL

            If HttpContext.Current.Session("hdnwhere") <> "" Then
                RptSQL = "Select * from (" & RptSQL & ") as A  where " & HttpContext.Current.Session("hdnwhere")
            Else
                RptSQL = "Select * from (" & RptSQL & ") as A  "
            End If

            If HttpContext.Current.Session("hdnwhere") <> "" Then
                If tmpWhere <> "" Then
                    RptSQL += " and " + tmpWhere
                End If
            Else
                If tmpWhere <> "" Then
                    RptSQL += "  where " + tmpWhere
                End If
            End If
            'append the count SQL and then extract count!
            '-------------------------------------------------------
            If HttpContext.Current.Session("hdnwhere") <> "" Then
                RptSQLCount = "Select count(*) from (" & RptSQLSaved & ") as A  where " & HttpContext.Current.Session("hdnwhere")
            Else
                RptSQLCount = "Select count(*) from (" & RptSQLSaved & ") as A  "
            End If
            If HttpContext.Current.Session("hdnwhere") <> "" Then
                If tmpWhere <> "" Then
                    RptSQLCount += " and " + tmpWhere
                End If
            Else
                If tmpWhere <> "" Then
                    RptSQLCount += "  where " + tmpWhere
                End If
            End If
            '-------------------------------------------------------


            If sortcolname = "" Then
                sortcolname = defsortcol
                sortcol = defsortcolindex
                sortdir = defsortdir
            End If

            If sortcolname = "" Then
                RptSQL = RptSQL + " order by `" + (CInt(sortcol)).ToString() + "` " + IIf(sortdir.Contains("asc"), "asc", "desc")
                'If defsortcol = "" Then
                '    RptSQL = RptSQL + " order by " + (CInt(sortcol) + 1).ToString() + " " + realsortdir
                'Else
                '    RptSQL = RptSQL + " order by " + defsortcol + " " + defsortdir
                'End If
            Else
                RptSQL = RptSQL + " order by `" + sortcolname + "` " + IIf(sortdir.Contains("asc"), "asc", "desc")
            End If

            'If RptSQL.ToLower.Contains(" limit ") Then
            '    RptSQL = RptSQL.Replace("::LI::",ddlpager)
            'Else
            '        RptSQL = RptSQL + " limit " + ddlpager 'strLimit
            'End If
            If ddlpager <> "100%" Then
                RptSQL = RptSQL + " limit " + ddlpager 'strLimit
            End If


            dtRptData = dbR.DataAdapter(CommandType.Text, RptSQL + ";" + RptSQLCount)

        Catch ex As Exception
            SSManager.LogAppError(ex, "BindGrids")
        End Try

        Return dtRptData

    End Function

    'Private Sub rptGrdShowHide_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptGrdShowHide.ItemCommand
    '    Try
    '        Dim db As New DBHelperClient
    '        Dim strSQL As String
    '        If e.CommandName = "Active" Then
    '            Dim parms(2) As DBHelperClient.Parameters
    '            parms(0) = New DBHelperClient.Parameters("p_userid", Session("user_id"))
    '            parms(1) = New DBHelperClient.Parameters("p_grdname", Request.QueryString("RptName"))
    '            parms(2) = New DBHelperClient.Parameters("p_colname", e.CommandArgument)
    '            strSQL = " Insert into tblgrdcolsu (UserId, grdname, colname) values (@p_userid, @p_grdname, @p_colname) "
    '            db.ExecuteNonQuery(CommandType.Text,strSQL,parms)
    '        Else
    '            strSQL = "Delete from tblgrdcolsu " & _
    '                    " where UserId = " & Session("User_Id") & _
    '                    " And grdname = '" & Request.QueryString("RptName") & "'" & _
    '                    " And colname = '" & e.CommandArgument & "'" 
    '            db.ExecuteNonQuery(CommandType.Text,strSQL)
    '        End If
    '        BindGrdCols()
    '        ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "refreshtablescipt22", "refreshtable();", True)
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Private Sub lnkAdd_Click(sender As Object, e As EventArgs) Handles lnkAdd.Click
        Try
            addfilter()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('showfilters();',500);", True)

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "lnkAdd_Click")
        End Try
    End Sub

    Protected Sub addfilter()
        Dim db As New DBHelperClient
        Dim strSQL As String
        Dim parms(5) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_userid", Session("user_id"))
        parms(1) = New DBHelperClient.Parameters("p_grdname", Request.QueryString("RptName"))
        If rptfilter.Items.Count > 0 Then
            parms(2) = New DBHelperClient.Parameters("p_cond", fddlcond.SelectedValue)
        Else
            parms(2) = New DBHelperClient.Parameters("p_cond", "")
        End If
        parms(3) = New DBHelperClient.Parameters("p_cols", fddlcols.SelectedValue)
        parms(4) = New DBHelperClient.Parameters("p_cond2", fddlcond2.SelectedValue)
        If fddlcond2.SelectedValue = "Like" Then
            parms(5) = New DBHelperClient.Parameters("p_fieldvalue", "%" & fwhere.Value & "%")
        Else
            parms(5) = New DBHelperClient.Parameters("p_fieldvalue", fwhere.Value)
        End If
        strSQL = " Insert into tblgrdfilter (userid, grdname, cond, fieldcol, cond2, fieldvalue ) " &
                " values (@p_userid, @p_grdname, @p_cond, @p_cols, @p_cond2, @p_fieldvalue)"
        db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
        BindFilter()

    End Sub

    Private Sub rptfilter_ItemCommand(source As Object, e As RepeaterCommandEventArgs) Handles rptfilter.ItemCommand
        Try
            If e.CommandName = "Del" Then
                Dim dbc As New DBHelperClient
                If e.Item.ItemIndex = 0 And rptfilter.Items.Count > 1 Then
                    Dim nextrowid As String = DirectCast(rptfilter.Items(e.Item.ItemIndex + 1).FindControl("fDel"), LinkButton).CommandArgument.ToString
                    dbc.ExecuteNonQuery(CommandType.Text, "update tblgrdfilter set cond = '' where rowid = " & nextrowid)
                End If
                dbc.ExecuteNonQuery(CommandType.Text, "Delete from tblgrdfilter where rowid = " & e.CommandArgument)
                BindFilter()
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "AddNewAcct", "setTimeout('showfilters();',500);", True)
            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "rptfilter_ItemCommand")
        End Try
    End Sub

    Public Sub submitfilter(sender As Object, e As EventArgs)
        Try
            'check if a condition is added or not
            If fwhere.Value.Trim <> "" Then
                addfilter()
            End If
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "refreshtablescipt2", "callrefreshtable();", True)

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "submitfilter")
        End Try
    End Sub

    Public Sub btnShowHide_ServerClick(sender As Object, e As EventArgs)
        Try

            Dim db As New DBHelperClient
            Dim strSQL As String

            For Each item As RepeaterItem In rptGrdShowHide.Items
                Dim parms2(2) As DBHelperClient.Parameters
                parms2(0) = New DBHelperClient.Parameters("userid", HttpContext.Current.Session("user_id"))
                parms2(1) = New DBHelperClient.Parameters("rptname", Request.QueryString("RptName"))
                parms2(2) = New DBHelperClient.Parameters("colname", DirectCast(item.FindControl("GrdCol"), HtmlInputText).Value)
                strSQL = "Delete from tblgrdcolsu " &
                        " where UserId = @userid " &
                        " And grdname = @rptname " &
                        " And colname = @colname "
                db.ExecuteNonQuery(CommandType.Text, strSQL,parms2)

                If DirectCast(item.FindControl("hdnsrc"), HiddenField).Value = "images/cross.png" Then
                    Dim parms(2) As DBHelperClient.Parameters
                    parms(0) = New DBHelperClient.Parameters("p_userid", Session("user_id"))
                    parms(1) = New DBHelperClient.Parameters("p_grdname", Request.QueryString("RptName"))
                    parms(2) = New DBHelperClient.Parameters("p_colname", DirectCast(item.FindControl("GrdCol"), HtmlInputText).Value)
                    strSQL = " Insert into tblgrdcolsu (UserId, grdname, colname) values (@p_userid, @p_grdname, @p_colname) "
                    db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
                End If

            Next
            BindGrdCols()
            ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "refreshtablescipt3", "callrefreshtable();", True)
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "btnShowHide_ServerClick")
        End Try
    End Sub

    Private Sub lnkSaveVar_Click(sender As Object, e As EventArgs) Handles lnkSaveVar.Click
        Try
            Dim strReDir As String = ""
            Dim tmpstr As String = ""
            If Request.QueryString.Keys(0).ToLower.Equals("rptname") Then
                strReDir = "ReportDetail.aspx?RptName=" & Request.QueryString.Item(0).ToString
            End If
            For Each item As RepeaterItem In rptvariables.Items
                If TryCast(item.FindControl("pName"), Label).Text.ToLower.Contains("date") Then

                    If TryCast(item.FindControl("pName"), Label).Text.ToLower = "sdate" Or
                        TryCast(item.FindControl("pName"), Label).Text.ToLower = "edate" Then
                        Dim db As New DBHelper
                        Dim parms2(0) As DBHelper.Parameters
                        parms2(0) = New DBHelper.Parameters("palue", TryCast(item.FindControl("pValue"), HtmlInputText).Value)
                        Dim strSQL As String = "SELECT DATE_FORMAT(STR_TO_DATE(@pvalue, '%m/%d/%Y'), '%Y-%m-%d')"
                        tmpstr = db.ExecuteScalar(CommandType.Text, strSQL,parms2)
                        strReDir = strReDir & "&" & TryCast(item.FindControl("pName"), Label).Text & "=" & tmpstr
                    Else
                        strReDir = strReDir & "&" & TryCast(item.FindControl("pName"), Label).Text & "=" & TryCast(item.FindControl("pValue"), HtmlInputControl).Value
                    End If
                Else
                    strReDir = strReDir & "&" & TryCast(item.FindControl("pName"), Label).Text & "=" & TryCast(item.FindControl("pValue"), HtmlInputControl).Value
                End If
            Next
            Response.Redirect(strReDir)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnback_ServerClick(sender As Object, e As EventArgs) Handles btnback.ServerClick
        SSManager.BackRedir()
    End Sub

    Private Sub rptvariables_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptvariables.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If TryCast(e.Item.FindControl("pName"), Label).Text.ToLower.Contains("date") Then

                If TryCast(e.Item.FindControl("pName"), Label).Text.ToLower = "sdate" Or
                    TryCast(e.Item.FindControl("pName"), Label).Text.ToLower = "edate" Then
                    Dim db As New DBHelper
                    Dim parms2(0) As DBHelper.Parameters
                    parms2(0) = New DBHelper.Parameters("palue", TryCast(e.Item.FindControl("pValue"), HtmlInputText).Value)
                    Dim strSQL As String = "SELECT DATE_FORMAT(@pvalue, '%m/%d/%Y')"
                    TryCast(e.Item.FindControl("pValue"), HtmlInputText).Value = db.ExecuteScalar(CommandType.Text, strSQL,parms2)
                    TryCast(e.Item.FindControl("pValue"), HtmlInputText).Attributes.Add("class", "date datepicker")
                End If

            End If
        End If
    End Sub


End Class
