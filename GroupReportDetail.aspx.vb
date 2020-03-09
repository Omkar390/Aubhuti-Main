Imports System.Data
Partial Class GroupReportDetail
    Inherits System.Web.UI.Page
    Protected BindLists As String = String.Empty
    'Public Shared s As String = ""
    Public Shared ColFormat As String()
    'Public Shared ColWidth As String()
    Public Shared ColLinks As String()
    'Public Shared ColLinksfinal As String()
    'Public Shared bHasLinks As Boolean = False
    Public Shared ColShowHide As String()
    Public Shared allcols As Decimal()
    'Public Shared strdisplayLength As String = "10"
    'Public Shared defsortcol As String = ""
    'Public Shared defsortdir As String = ""
    'Public Shared defsortcolindex As String = 0
    'Public Shared firsttime As Integer = 0
    Public Shared rptwidth As String = ""
    Public Shared colval As String = ""
    Public Shared showtotals As Integer = 0
    Public Shared fwdURL As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
			BindLists = Page.ClientScript.GetPostBackEventReference(btnListRefresh, String.Empty)
            If Not Page.IsPostBack Then
                Response.Buffer = True
                Response.CacheControl = "no-cache"
                Response.AddHeader("Pragma", "no-cache")
                Response.Expires = -1441
                Response.AppendHeader("Cache-Control", "no-store")

                hdnUrl.Value = Request.UrlReferrer.ToString
                fwdURL = Mid(HttpContext.Current.Request.QueryString.ToString, InStr(HttpContext.Current.Request.QueryString.ToString, "&") + 1)
                GetReports()
                BindVariables()
            End If

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            lblErrorMsg.Text = ex.Message
            'SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub
    Private Sub btnback_ServerClick(sender As Object, e As EventArgs) Handles btnback.ServerClick
        SSManager.BackRedir()
    End Sub


    Protected Sub GetReports()
        Try
            Dim db As New DBHelper()
            Dim strSQL As String
            strSQL = " SELECT grouptitle, rptid FROM tblgroups G INNER JOIN tblgroupsrpt GR ON G.groupid = GR.groupid " & _
                    " WHERE groupname = '" & Request.QueryString("RptName") & "'" & _
                    " ORDER BY orderno " 
            Dim dt As New DataTable()
            dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
            For i = 0 To dt.Rows.Count - 1
                If i = 0 Then
                    lblTitle.Text = dt.Rows(0).Item("grouptitle").ToString
                End If
                BindData(i + 1, dt.Rows(i).Item("rptid").ToString )
            Next

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub BindVariables()
        Try

            Dim strVar As String = ""
            Dim strSQL As String = ""
            Dim parms(1) As DBHelperClient.Parameters

            For i = 0 To Request.QueryString.Count - 1
                If Request.QueryString.Keys(i) = "RptName" Then
                Else
                    parms(0) = New DBHelperClient.Parameters("p_fieldcol", Request.QueryString.Keys(i).ToString)
                    parms(1) = New DBHelperClient.Parameters("p_fieldval", Request.QueryString.Item(i).ToString)

                    If i = 1 Then
                        strSQL = "Select @p_fieldcol as fieldcol, @p_fieldval as fieldvalue "
                        strVar = Request.QueryString.Keys(i).ToString & ":" & Request.QueryString.Item(i).ToString
                    Else
                        If strSQL = "" Then
                            strSQL = "Select @p_fieldcol as fieldcol, @p_fieldval as fieldvalue "
                            strVar = Request.QueryString.Keys(i).ToString & ":" & Request.QueryString.Item(i).ToString
                        Else
                            strSQL = strSQL & " Union Select @p_fieldcol as fieldcol, @p_fieldval as fieldvalue "
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
                dt = dbc.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)
                btnedit.Visible = True
                rptvariables.DataSource = dt
                rptvariables.DataBind()
            End If
            lblVars.Text = strVar

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

    Protected Sub BindData(ByVal rptCnt As String, ByVal rptId As String )
        Try
            Dim i As Integer = 0
            Dim dsRptDtl As DataSet
            Dim RptSQLCols As String
            Dim RptSQL As String
            Dim strSQL As String
            Dim db As New DBHelper
            Dim dbc As New DBHelperClient

            Dim parms(0) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("p_rptId", rptId)
            Dim dtRptData As New DataTable
            strSQL = "Select * FROM tblrptmaster where RptId = @p_rptId " 

            dsRptDtl = db.DataAdapter(CommandType.Text, strSQL, parms)
            rptwidth = dsRptDtl.Tables(0).Rows(0).Item("rptwidth").ToString
            showtotals = CInt(IIf(dsRptDtl.Tables(0).Rows(0).Item("showtotals") Is Nothing,0,IIf(dsRptDtl.Tables(0).Rows(0).Item("showtotals").ToString = "",0,dsRptDtl.Tables(0).Rows(0).Item("showtotals"))))

            Dim hdrstr As String
            hdrstr = " <div class='heading_row topspace  crasul_sec'> "
            hdrstr += " <h4>" & dsRptDtl.Tables(0).Rows(0).Item("RptTitle").ToString & ""
            hdrstr += "<a href='ReportDetail.aspx?RptName=" & dsRptDtl.Tables(0).Rows(0).Item("RptName").ToString & "&" & Mid(Request.QueryString.ToString, InStr(Request.QueryString.ToString, "&") + 1) & "'> "
            hdrstr += "<img title='Click here to View this Report' src='images/arrow.jpg'></a> </h4>"
            hdrstr += " </div> "

            ColShowHide = dsRptDtl.Tables(0).Rows(0).Item("RptShowHide").ToString.Split(",")
            ColFormat = dsRptDtl.Tables(0).Rows(0).Item("RptColsFormat").ToString.Split(",")
            ColLinks = dsRptDtl.Tables(0).Rows(0).Item("RptLinks").ToString.Split(",")

            RptSQL = dsRptDtl.Tables(0).Rows(0).Item("RptSql").ToString
            RptSQLCols = Left(RptSQL, InStr(RptSQL.ToLower," from"))

            'replace querystring params, if any!
            Dim qString As String()
            qString = System.Web.HttpUtility.UrlDecode(Request.QueryString.ToString()).Split("&")
            If qString.Count > 1 Then
                For index = 1 To qString.Count - 1
                    If qString(index).Split("=")(0).ToLower.Equals("year") Then
                        RptSQL = RptSQL.Replace("::" + "year" + "::", "'" + qString(index).Split("=")(1) + "'")
                        RptSQL = RptSQL.Replace("::" + "YEAR" + "::", "'" + qString(index).Split("=")(1) + "'")
                        RptSQL = RptSQL.Replace("::" + "Year" + "::", "'" + qString(index).Split("=")(1) + "'")
                    Else
                        RptSQL = RptSQL.Replace("::" + qString(index).Split("=")(0) + "::", "'" + System.Web.HttpUtility.UrlDecode(qString(index).Split("=")(1)).Replace("'", "''") + "'")
                    End If
                    If qString(index).Split("=")(0).ToLower.Equals("quarter") Then
                        RptSQL = RptSQL.Replace("::ST::", "quarter")
                        RptSQL = RptSQL.Replace("::ST::", "Quarter")
                        RptSQL = RptSQL.Replace("::ST::", "QUARTER")
                        RptSQL = RptSQL.Replace("::SF::", "'" + qString(index).Split("=")(1) + "'")
                    End If
                    If qString(index).Split("=")(0).ToLower.Equals("month") Then
                        RptSQL = RptSQL.Replace("::ST::", "month")
                        RptSQL = RptSQL.Replace("::ST::", "Month")
                        RptSQL = RptSQL.Replace("::ST::", "MONTH")
                        RptSQL = RptSQL.Replace("::SF::", "'" + qString(index).Split("=")(1) + "'")
                    End If
                Next
            End If
            If RptSQL.Contains(".::ST::") Then
                'get index of .::ST::
                'get length till that!
                'get last index of space 
                ' replace that index+1 till index of .::ST:: to replace!!!
                Dim iSTIndex As Integer = RptSQL.IndexOf(".::ST::")
                Dim strTillST As String = RptSQL.Substring(0,RptSQL.IndexOf(".::ST::")+7)
                Dim iSpace As Integer = strTillST.LastIndexOf(" ")
                RptSQL = RptSQL.Replace(RptSQL.Substring(iSpace + 1,iSTIndex-iSpace+6),"1")
                'RptSQL = RptSQL.Replace("::ST::", "1")
                RptSQL = RptSQL.Replace("::SF::", "1")
            Else If RptSQL.Contains("::ST::") Then
                RptSQL = RptSQL.Replace("::ST::", "1")
                RptSQL = RptSQL.Replace("::SF::", "1")
            End If
            
            dtRptData = dbc.DataAdapter(CommandType.Text, RptSQL).Tables(0)
            Redim allcols(dtRptData.Columns.Count)

            Dim tbstr As String = ""
            For i = 0 To dtRptData.Columns.Count - 1 
                If i = 0 Then
                    tbstr = " <div class='new_btab dataTables_wrapper'> "
                    tbstr += " <table width='" & rptwidth & "' border='0' cellspacing='0' cellpadding='0' class='myexample display nowrap ac_list sort_tab' > "
                    tbstr += " <thead> "
                    tbstr += " <tr> "
                End If
                If ColShowHide(i) = "Y" Then
                    tbstr += " <th style='text-align: center;'>" & dtRptData.Columns(i).ColumnName & "</th> "
                End If
            Next
            tbstr += " </tr> "
            tbstr += " </thead> "

            Dim dtRptCnt As Integer

            If dtRptData.Rows.Count > 0 Then
                tbstr += " <tbody> "
                    
                If dtRptData.Rows.Count > 500 Then
                    dtRptCnt = 50
                Else        
                    dtRptCnt = dtRptData.Rows.Count
                End If
                'For i = 0 To dtRptData.Rows.Count - 5
                For i = 0 To dtRptCnt - 1
                    tbstr += " <tr> "
                    For j = 0 To dtRptData.Columns.Count - 1
                        If ColShowHide(j) = "Y" Then
                            Select Case ColFormat(j)
                                Case "TL"
                                    colval = dtRptData.Rows(i).Item(j).ToString
                                Case "TR"
                                    colval = "<div style='float:right;margin-right:20px'>" + dtRptData.Rows(i).Item(j).ToString + "</div>"
                                Case "TC"
                                    colval = "<div style='float:left;position: relative;left: 50%;'><div style='float:left;position: relative;left: -50%;'>" + dtRptData.Rows(i).Item(j).ToString + "</div></div>"
                                Case "DD"
                                    colval = IIf(dtRptData.Rows(i).Item(j).ToString.Equals(""),dtRptData.Rows(i).Item(j).ToString,String.Format("{0:M/d/yyyy}", dtRptData.Rows(i).Item(j).ToString)).ToString
                                Case "DT"
                                    colval = dtRptData.Rows(i).Item(j).ToString
                                Case "D0"
                                    If dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value) Or dtRptData.Rows(i).Item(j).ToString = "" Then
                                        colval = "<div style='float:right;margin-right:20px'>" + "0" + "</div>"
                                    Else
                                        If isnumeric(dtRptData.Rows(i).Item(j)) Then
                                            colval = "<div style='float:right;margin-right:20px'>" + String.Format("{0:N0}", Integer.Parse(CInt(dtRptData.Rows(i).Item(j)).ToString)).ToString() + "</div>"
                                        Else
                                            colval = "<div style='float:right;margin-right:20px'>" + dtRptData.Rows(i).Item(j).ToString + "</div>"
                                        End If
                                    End If
                                    If dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value) Or dtRptData.Rows(i).Item(j).ToString = "" Then
                                        allcols(j) = allcols(j) + 0
                                    Else
                                        allcols(j) = allcols(j) + CDec(dtRptData.Rows(i).Item(j))
                                    End If
                                Case "D2"
                                    If dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value) Or dtRptData.Rows(i).Item(j).ToString = "" Then
                                        colval = "<div style='float:right;margin-right:20px'>" + "0.00" + "</div>"
                                    Else
                                        colval = "<div style='float:right;margin-right:20px'>" + String.Format("{0:N2}", Cdec(dtRptData.Rows(i).Item(j).ToString)).ToString() + "</div>"
                                    End If
                                    If dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value) Or dtRptData.Rows(i).Item(j).ToString = "" Then
                                        allcols(j) = allcols(j) + 0
                                    Else
                                        allcols(j) = CDec(allcols(j)) + CDec(dtRptData.Rows(i).Item(j))
                                    End If
                                Case "C0"
                                    colval = "<div style='float:right;margin-right:20px'>" + String.Format("{0:$#,##0}",IIf(dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value),"0",dtRptData.Rows(i).Item(j).ToString)) + "</div>"
                                    If dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value) Or dtRptData.Rows(i).Item(j).ToString = "" Then
                                        allcols(j) = allcols(j) + 0
                                    Else
                                        allcols(j) = allcols(j) + dtRptData.Rows(i).Item(j).ToString
                                    End If
                                Case "C2"
                                    colval = "<div style='float:right;margin-right:20px'>" + String.Format("{0:$#,##0.00}",IIf(dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value),"0",dtRptData.Rows(i).Item(j).ToString)) + "</div>"
                                    If dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value) Or dtRptData.Rows(i).Item(j).ToString = "" Then
                                        allcols(j) = allcols(j) + 0
                                    Else
                                        allcols(j) = allcols(j) + dtRptData.Rows(i).Item(j).ToString
                                    End If
                                Case "C4"
                                    colval = "<div style='float:right;margin-right:20px'>" + String.Format("{0:$#,##0.0000}",IIf(dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value),"0",dtRptData.Rows(i).Item(j).ToString)) + "</div>"
                                    If dtRptData.Rows(i).Item(j).ToString.Equals(System.DBNull.Value) Or dtRptData.Rows(i).Item(j).ToString = "" Then
                                        allcols(j) = allcols(j) + 0
                                    Else
                                        allcols(j) = allcols(j) + dtRptData.Rows(i).Item(j).ToString
                                    End If
                                Case Else
                                    colval = dtRptData.Rows(i).Item(j).ToString
                            End Select

                            If ColLinks(j) = "" Then
                            Else
                                'loop to replace querystring params
                                Dim strURL As String = ColLinks(j).TrimStart("!")

                                For index = 1 To ColLinks(j).TrimStart("!").Split("&").Length-1
                                    If ColLinks(j).TrimStart("!").Split("&")(index).Contains("=")
                                    Else
                                        strURL = strURL.Replace(ColLinks(j).TrimStart("!").Split("&")(index),ColLinks(j).TrimStart("!").Split("&")(index).ToString + "=" + dtRptData.Rows(i).Item(ColLinks(j).TrimStart("!").Split("&")(index)).ToString)
                                    End If
                                    ' If fwdURL.Contains(ColLinks(i).TrimStart("!").Split("&")(index)) Then
                                        ' strURL = strURL.Replace("&" & ColLinks(i).TrimStart("!").Split("&")(index), "")
                                    ' Else
                                        ' strURL = strURL.Replace(ColLinks(j).TrimStart("!").Split("&")(index),ColLinks(j).TrimStart("!").Split("&")(index).ToString + "=" + dtRptData.Rows(i).Item(ColLinks(j).TrimStart("!").Split("&")(index)).ToString)
                                    ' End If
                                Next
                                'strURL += "&" & fwdURL
                                If strURL.Contains(".aspx") Then
                                    strURL = replace(strURL, "&", "?", , 1)
        				            colval = "<a href='" + strURL + "'>" + colval + "</a>"
                                ElseIf Left(strURL, 1) = "@"
        				            colval = "<a href='GroupReportDetail.aspx?RptName=" + mid(strURL,2) + "'>" + colval.ToString() + "</a>"
                                Else
        				            colval = "<a href='ReportDetail.aspx?RptName=" + strURL + "'>" + colval + "</a>"
                                End If
                            End If

                            tbstr += " <td>" & colval & " </td> "
                        End If
                    Next
                    tbstr += " </tr> "
                Next
                tbstr += " </tbody> "

                If showtotals = 1 Then
                    tbstr += "<tfoot>"
                    tbstr += " <tr style='background-color: rgb(252, 176, 64);'>"
                    For k = 0 To dtRptData.Columns.Count - 1
                        If ColShowHide(k) = "Y" Then
                            Select Case ColFormat(k)
                                Case "TL"
									If k = 0 Then
                                        tbstr += " <td><b>Total </b></td> "
                                    Else
                                        tbstr += " <td></td> "
									End If
                                Case "TR"
									If k = 0 Then
                                        tbstr += " <td><b>Total </b></td> "
                                    Else
                                        tbstr += " <td></td> "
									End If
                                Case "TC"
									If k = 0 Then
                                        tbstr += " <td><b>Total </b></td> "
                                    Else
                                        tbstr += " <td></td> "
									End If
                                Case "DD"
									If k = 0 Then
                                        tbstr += "<td><b>Total </b></td> "
                                    Else
                                        tbstr += " <td></td> "
									End If
                                Case "DT"
									If k = 0 Then
                                        tbstr += "<td><b>Total </b></td> "
                                    Else
                                        tbstr += " <td></td> "
									End If
                                Case "D0"
									If k = 0 Then
										tbstr += "<td><div style='float:right;margin-right:20px'>" + "Total " + String.Format("{0:N0}",allcols(k)).ToString() + "</div></td>"
									Else
										tbstr += "<td><div style='float:right;margin-right:20px'>" + String.Format("{0:N0}",CInt(allcols(k))).ToString() + "</div></td>"
									End If
                                Case "D2"
									If k = 0 Then
										tbstr += "<td><div style='float:right;margin-right:20px'>" + "Total " + String.Format("{0:N2}",allcols(k)).ToString() + "</div></td>"
									Else
										tbstr += "<td><div style='float:right;margin-right:20px'>" + String.Format("{0:N2}",CDec(allcols(k))).ToString() + "</div></td>"
									End If
                                Case "C0"
									If k = 0 Then
										tbstr += "<td><div style='float:right;margin-right:20px'>" + "Total " + String.Format("{0:$#,##0}",allcols(k)).ToString() + "</div></td>"
									Else
										tbstr += "<td><div style='float:right;margin-right:20px'>" + String.Format("{0:$#,##0}",allcols(k)).ToString() + "</div></td>"
									End If
                                Case "C2"
									If k = 0 Then
										tbstr += "<td><div style='float:right;margin-right:20px'>" + "Total " + String.Format("{0:$#,##0.00}",allcols(k)).ToString() + "</div></td>"
									Else
										tbstr += "<td><div style='float:right;margin-right:20px'>" + String.Format("{0:$#,##0.00}",allcols(k)).ToString() + "</div></td>"
									End If
                                Case "C4"
									If k = 0 Then
										tbstr += "<td><div style='float:right;margin-right:20px'>" + "Total " + String.Format("{0:$#,##0.0000}",allcols(k)).ToString() + "</div></td>"
									Else
										tbstr += "<td><div style='float:right;margin-right:20px'>" + String.Format("{0:$#,##0.0000}",allcols(k)).ToString() + "</div></td>"
									End If
                                Case Else
									If k = 0 Then
                                        tbstr += " <td><b>Total </b></td> "
                                    Else
                                        tbstr += " <td></td> "
									End If
                            End Select 
                        End If
                        'k+=1
			        Next
                    tbstr += " </tr> "
                    tbstr += " </tfoot> "
                End If

                tbstr += " </table> "
                tbstr += " </div> "
            Else
                tbstr += " </table> "
                tbstr += " </div> "
            End If


            If rptCnt = 1 Then
                rpthdr1.Text = hdrstr 
                rptlist1.Text = tbstr
            ElseIf rptCnt = 2
                rpthdr2.Text = hdrstr 
                rptlist2.Text = tbstr
            ElseIf rptCnt = 3
                rpthdr3.Text = hdrstr 
                rptlist3.Text = tbstr
            ElseIf rptCnt = 4
                rpthdr4.Text = hdrstr 
                rptlist4.Text = tbstr
            ElseIf rptCnt = 5
                rpthdr5.Text = hdrstr 
                rptlist5.Text = tbstr
            ElseIf rptCnt = 6
                rpthdr6.Text = hdrstr 
                rptlist6.Text = tbstr
            ElseIf rptCnt = 7
                rpthdr7.Text = hdrstr 
                rptlist7.Text = tbstr
            ElseIf rptCnt = 8
                rpthdr8.Text = hdrstr 
                rptlist8.Text = tbstr
            ElseIf rptCnt = 9
                rpthdr9.Text = hdrstr 
                rptlist9.Text = tbstr
            ElseIf rptCnt = 10
                rpthdr10.Text = hdrstr 
                rptlist10.Text = tbstr
            End If

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            'SSManager.LogAppError(ex, "Page_Load")
        End Try
    End Sub

    Private Sub lnkSaveVar_Click(sender As Object, e As EventArgs) Handles lnkSaveVar.Click
        Try
            Dim strReDir As String = ""
            Dim tmpstr As String = ""
            If Request.QueryString.Keys(0) = "RptName" Then
                strReDir = "GroupReportDetail.aspx?RptName=" & Request.QueryString.Item(0).ToString
            End If
            For each item As RepeaterItem  In rptvariables.Items
                If TryCast(Item.FindControl("pName"), Label).Text.ToLower.Contains("date") Then

                    If TryCast(Item.FindControl("pName"), Label).Text.ToLower = "sdate" Or _
                        TryCast(Item.FindControl("pName"), Label).Text.ToLower = "edate" Then
                        Dim db As New DBHelper
                        Dim parms(0) As DBHelper.Parameters
                        parms(0) = New DBHelper.Parameters("p_pValue", TryCast(Item.FindControl("pValue"), HtmlInputText).Value)
                        Dim strSQL As String = "SELECT DATE_FORMAT(STR_TO_DATE('@p_pValue', '%m/%d/%Y'), '%Y-%m-%d')"
                        tmpstr = db.ExecuteScalar(CommandType.Text, strSQL, parms)
                        strReDir = strReDir & "&" & TryCast(Item.FindControl("pName"), Label).Text & "=" & tmpstr
                    Else
                        strReDir = strReDir & "&" & TryCast(Item.FindControl("pName"), Label).Text & "=" & TryCast(Item.FindControl("pValue"), HtmlInputControl).Value
                    End If
                Else
                    strReDir = strReDir & "&" & TryCast(Item.FindControl("pName"), Label).Text & "=" & TryCast(Item.FindControl("pValue"), HtmlInputControl).Value
                End If
            Next
            Response.Redirect(strReDir)
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
        End Try
    End Sub

    Private Sub rptvariables_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptvariables.ItemDataBound
        If e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem Then
            If TryCast(e.Item.FindControl("pName"), Label).Text.ToLower.Contains("date") Then

				If TryCast(e.Item.FindControl("pName"), Label).Text.ToLower = "sdate" Or _
                    TryCast(e.Item.FindControl("pName"), Label).Text.ToLower = "edate" Then
                    Dim db As New DBHelper
                    Dim parms(0) As DBHelper.Parameters
                    parms(0) = New DBHelper.Parameters("p_pValue", TryCast(e.Item.FindControl("pValue"), HtmlInputText).Value)
                    Dim strSQL As String = "SELECT DATE_FORMAT(@p_pValue, '%m/%d/%Y')"
                    TryCast(e.Item.FindControl("pValue"), HtmlInputText).Value = db.ExecuteScalar(CommandType.Text, strSQL, parms)
                    TryCast(e.Item.FindControl("pValue"), HtmlInputText).Attributes.Add("class", "date datepicker")
                End If

			End If
        End If
    End Sub

End Class
