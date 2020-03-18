Imports System.Data
Imports System.IO

Partial Class SurveyDesign
    Inherits System.Web.UI.Page

    Private Sub SurveyDesign_Load(sender As Object, e As EventArgs) Handles Me.Load
        hdnSurveyID.Value = Request.QueryString("SurveyID")
        If Not Page.IsPostBack = True Then

            Try
                BindDesign()
            Catch ex As Exception
                lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
                lblErrorMsg.Visible = True
                SSManager.LogAppError(ex, "SurveyDesign_Load")
            End Try
        End If
    End Sub

    Protected Sub BindDesign()

        Dim db As New DBHelperClient
        Dim dt As New DataTable
        Dim strSQL As String
        Dim parms(0) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("SurveyID", hdnSurveyID.Value)
        strSQL = " SELECT * from tblsurveydesign Where SurveyID = @SurveyID "
        dt = db.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)

        If dt.Rows.Count > 0 Then
            'fupCompanyLogoSmall
            hdnDesignFound.Value = "1"
            txtCSLBackColor.Value = dt.Rows(0).Item("CSLBackColor").ToString
            txtCSSBackColor.Value = dt.Rows(0).Item("CSSBackColor").ToString

            txtSTitle.Value = dt.Rows(0).Item("STitle").ToString
            txtSTBackColor.Value = dt.Rows(0).Item("STBackColor").ToString
            ddlSTFont.Items.FindByValue(dt.Rows(0).Item("STFont").ToString).Selected = True
            ddlSTFontSize.Items.FindByValue(dt.Rows(0).Item("STFontSize").ToString).Selected = True
            hdntitlebold.Value = dt.Rows(0).Item("STBold").ToString
            hdntitleitalics.Value = dt.Rows(0).Item("STItalic").ToString
            hdntitleunderline.Value = dt.Rows(0).Item("STUnderline").ToString
            txtSTFontColor.Value = dt.Rows(0).Item("STTextColor").ToString

            txtPTBackColor.Value = dt.Rows(0).Item("PTBackColor").ToString
            ddlPTFont.Items.FindByValue(dt.Rows(0).Item("PTFont").ToString).Selected = True
            ddlPTSize.Items.FindByValue(dt.Rows(0).Item("PTFontSize").ToString).Selected = True
            hdnpagebold.Value = dt.Rows(0).Item("PTBold").ToString
            hdnpageitalics.Value = dt.Rows(0).Item("PTItalic").ToString
            hdnpageunderline.Value = dt.Rows(0).Item("PTUnderline").ToString
            txtPTTextColor.Value = dt.Rows(0).Item("PTTextColor").ToString

            'SurveyLogo\2\1_C-logo.png
            'output.Src = ConfigurationManager.AppSettings("surveylogo") & Session("client_id") & "\" & hdnSurveyID.Value & "_" & dt.Rows(0).Item("logo").ToString
            output.Src = ConfigurationManager.AppSettings("SurveyLogoPathURL") & Session("client_id") & "/" & hdnSurveyID.Value & "_" & dt.Rows(0).Item("logo").ToString
            If dt.Rows(0).Item("LPUse").ToString = "1" Then
                chkLandingPage.Checked = True
                txtLPText.Text = Encoding.UTF8.GetString(dt.Rows(0).Item("LPText"))
                showLP.Style.Item("display") = "block"
            Else
                chkLandingPage.Checked = False
                showLP.Style.Item("display") = "none"
                txtLPText.Text = ""
            End If

        Else

            hdnDesignFound.Value = "0"
            txtCSLBackColor.Value = "#AAFFAA"
            txtCSSBackColor.Value = "#FFFFCA"

            txtSTitle.Value = ""
            txtSTBackColor.Value = "#FFBD9D"
            ddlSTFont.ClearSelection()
            ddlSTFontSize.ClearSelection()
            hdnpagebold.Value = "0"
            hdnpageitalics.Value = "0"
            hdnpageunderline.Value = "0"
            txtSTFontColor.Value = "#000000"

            txtPTBackColor.Value = "#B9B9FF"
            ddlPTFont.ClearSelection()
            ddlPTSize.ClearSelection()
            hdntitlebold.Value = "0"
            hdntitleitalics.Value = "0"
            hdntitleunderline.Value = "0"
            txtPTTextColor.Value = "#000000"

            output.Src = "images/anubhuti logo.png"
            chkLandingPage.Checked = False
            showLP.Style.Item("display") = "none"
            txtLPText.Text = ""

        End If

    End Sub


    'Button to overview survey
    Private Sub btntab1_ServerClick(sender As Object, e As EventArgs) Handles btntab1.ServerClick
        Response.Redirect("SurveyOverview.aspx?SurveyID=" & hdnSurveyID.Value)
    End Sub

    'Button for Questions
    Private Sub btntab3_ServerClick(sender As Object, e As EventArgs) Handles btntab3.ServerClick
        If hdnSurveyID.Value = 0 Then
            lblErrorMsg.Text = "Please Save survey first."
        Else
            Response.Redirect("SurveyQuestion.aspx?SurveyID=" & hdnSurveyID.Value)
        End If
    End Sub

    'Done button or save button
    Private Sub btnSave_ServerClick(sender As Object, e As EventArgs) Handles btnSave.ServerClick
        Try

            Dim strSQL  As String
            Dim db As New DBHelperClient
            Dim parms(25) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_SurveyID", hdnSurveyID.Value)
            parms(1) = New DBHelperClient.Parameters("p_CSLBackColor", txtCSLBackColor.Value)
            parms(2) = New DBHelperClient.Parameters("p_CSSBackColor", txtCSSBackColor.Value)
            parms(3) = New DBHelperClient.Parameters("p_STitle", txtSTitle.Value)
            parms(4) = New DBHelperClient.Parameters("p_STBackColor", txtSTBackColor.Value)
            parms(5) = New DBHelperClient.Parameters("p_STFont", ddlSTFont.SelectedValue)
            parms(6) = New DBHelperClient.Parameters("p_STFontSize", ddlSTFontSize.SelectedValue)
            parms(7) = New DBHelperClient.Parameters("p_STBold", hdntitlebold.Value)
            parms(8) = New DBHelperClient.Parameters("p_STItalic", hdntitleitalics.Value)
            parms(9) = New DBHelperClient.Parameters("p_STUnderline", hdntitleunderline.Value)
            parms(10) = New DBHelperClient.Parameters("p_STTextColor", txtSTFontColor.Value)
            parms(11) = New DBHelperClient.Parameters("p_PTBackColor", txtPTBackColor.Value)
            parms(12) = New DBHelperClient.Parameters("p_PTFont", ddlPTFont.SelectedValue)
            parms(13) = New DBHelperClient.Parameters("p_PTFontSize", ddlPTSize.SelectedValue)
            parms(14) = New DBHelperClient.Parameters("p_PTBold", hdnpagebold.Value)
            parms(18) = New DBHelperClient.Parameters("p_PTItalic", hdnpageitalics.Value)
            parms(19) = New DBHelperClient.Parameters("p_PTUnderline", hdnpageunderline.Value)
            parms(20) = New DBHelperClient.Parameters("p_PTTextColor", txtPTTextColor.Value)
            parms(21) = New DBHelperClient.Parameters("p_LPUse", chkLandingPage.Checked)
            parms(22) = New DBHelperClient.Parameters("p_LPText", txtLPText.Text)
            If fupCompanyLogoSmall.PostedFile.FileName = "" Then
                parms(23) = New DBHelperClient.Parameters("p_logo", "")
            Else
                parms(23) = New DBHelperClient.Parameters("p_logo", fupCompanyLogoSmall.PostedFile.FileName.Substring(fupCompanyLogoSmall.PostedFile.FileName.LastIndexOf("\")+1))
            End If
            parms(24) = New DBHelperClient.Parameters("p_User", Session("user_id"))
            parms(25) = New DBHelperClient.Parameters("SurveyID", hdnSurveyID.Value)

            If hdnDesignFound.Value = 0 Then
                strSQL = " Insert into tblsurveydesign (SurveyID, CSLBackColor, CSSBackColor, " & _
                         " STitle, STBackColor, STFont, STFontSize, STBold, STItalic, STUnderline, STTextColor, " & _
                         " PTBackColor, PTFont, PTFontSize, PTBold, PTItalic, PTUnderline, PTTextColor, LPUse, " & _
                         " LPText, logo, CreatedBy, CreatedOn )" & _
                         " Values ( @p_SurveyID, @p_CSLBackColor, @p_CSSBackColor, " & _
                         " @p_STitle, @p_STBackColor, @p_STFont, @p_STFontSize, @p_STBold, @p_STItalic, @p_STUnderline, @p_STTextColor, " & _ 
                         " @p_PTBackColor, @p_PTFont, @p_PTFontSize, @p_PTBold, @p_PTItalic, @p_PTUnderline, @p_PTTextColor, " & _
                         " @p_LPUse, @p_LPText, @p_logo, @p_User, CURRENT_TIMESTAMP ) ; " 
                hdnSurveyID.Value = db.ExecuteScalar(CommandType.Text, strSQL, parms)
            Else
                strSQL = " Update tblsurveydesign set CSLBackColor = @p_CSLBackColor, CSSBackColor = @p_CSSBackColor " & _
                         " , STitle = @p_STitle, STBackColor = @p_STBackColor, STFont = @p_STFont, STFontSize = @p_STFontSize " & _
                         " , STBold = @p_STBold, STItalic = @p_STItalic, STUnderline = @p_STUnderline, STTextColor = @p_STTextColor " & _
                         " , PTBackColor = @p_PTBackColor, PTFont = @p_PTFont, PTFontSize = @p_PTFontSize " & _
                         " , PTBold = @p_PTBold, PTItalic = @p_PTItalic, PTUnderline = @p_PTUnderline, PTTextColor = @p_PTTextColor " & _
                         " , LPUse = @p_LPUse, LPText = @p_LPText " 
				If fupCompanyLogoSmall.PostedFile.FileName = "" Then
				Else
                    strSQL  = strSQL & " , logo = @p_logo "
                End If
                strSQL = strSQL & " , ModifiedBy = @p_User , ModifiedOn = CURRENT_TIMESTAMP " & _
                        " Where SurveyID = @SurveyID "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
            End If

            lblSuccessMsg.Text = "Survey has been updated successfully"
            lblSuccessMsg.Visible = True
            
            'save small logo!
            If Not System.IO.Directory.Exists(ConfigurationManager.AppSettings("SurveyLogoPath") & Session("client_id")) Then
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings("SurveyLogoPath") & Session("client_id"))
            End If

            If Not fupCompanyLogoSmall.PostedFile Is Nothing Then
                fupCompanyLogoSmall.PostedFile.SaveAs(ConfigurationManager.AppSettings("surveylogopath") & Session("client_id") & "\" & hdnSurveyID.Value & "_" & fupCompanyLogoSmall.FileName)
            End If

            BindDesign()

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "btnSave_ServerClick")
        End Try
    End Sub

    'Finalize button code
    Private Sub btntab4_ServerClick(sender As Object, e As EventArgs) Handles btntab4.ServerClick
        If hdnSurveyID.Value = 0 Then
            lblErrorMsg.Text = "Please Save survey first."
        Else
            Response.Redirect("SurveyFinalize.aspx?SurveyID=" & hdnSurveyID.Value)
        End If
    End Sub
End Class
