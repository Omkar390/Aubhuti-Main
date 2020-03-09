Imports System.Data

Partial Class SurveyOverview
    Inherits System.Web.UI.Page

    Private Sub SurveyOverview_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack = True
                Dim dbC As New DBHelperClient
                Dim strSQL As String
                strSQL = " SELECT surveytypeid, surveytype FROM tblsurveytype where coalesce(isdeleted,0) = 0 order by surveytype ; " 
                ddlSurveyType.DataSource = dbC.DataAdapter(Data.CommandType.Text,strSQL)
                ddlSurveyType.DataBind()
                ddlSurveyType.Items.Insert(0, New ListItem("Select ", "0"))

                hdnSurveyID.value =  Request.QueryString("SurveyID")
                BindOverview()

            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "SurveyOverview_Load")
        End Try

    End Sub

    Protected Sub BindOverview()

        Dim db As New DBHelperClient 
        Dim dt As New DataTable 
        Dim strSQL As String 
        Dim parms1(0) As DBHelperClient.Parameters
        parms1(0) = New DBHelperClient.Parameters("SurveyID", Request.QueryString("SurveyID"))
        strSQL = " SELECT surveytypeid, SurveyID, SurveyName, ST.SurveyType, Description, Active, URL  " & _
                " FROM tblsurvey S " & _
                " LEFT OUTER JOIN tblsurveytype ST ON ST.surveytypeid = S.SurveyType " & _
                " Where SurveyID = @SurveyID "
        dt = db.DataAdapter(CommandType.Text, strSQL,parms1).Tables(0)
            
        If dt.Rows.Count > 0 Then
            divrows.Style.Item("display") = "block"
            litTitle.Text = dt.Rows(0).Item("SurveyName").ToString
            txtName.Value = dt.Rows(0).Item("SurveyName").ToString
            litType.Text = dt.Rows(0).Item("SurveyType").ToString
            ddlSurveyType.ClearSelection()
            ddlSurveyType.Items.FindByValue(dt.Rows(0).Item("surveytypeid").ToString).Selected = True
            litDescription.Text = dt.Rows(0).Item("Description").ToString
            txtDesc.Value = dt.Rows(0).Item("Description").ToString
            chkActive.Checked = dt.Rows(0).Item("Active").ToString 
        Else
            divrows.Style.Item("display") = "none"
            litTitle.Text = ""
            txtName.Value = ""
            litType.Text = ""
            ddlSurveyType.ClearSelection()
            litDescription.Text = ""
            txtDesc.Value = ""
        End If

    End Sub

    Private Sub btnSave_ServerClick(sender As Object, e As EventArgs) Handles btnSave.ServerClick
        Try

            Dim strSQL As String
            Dim db As New DBHelperClient
            Dim parms(5) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_SurveyName", txtName.Value)
            parms(1) = New DBHelperClient.Parameters("p_SurveyType", ddlSurveyType.SelectedValue)
            parms(2) = New DBHelperClient.Parameters("p_Description", txtDesc.Value)
            parms(3) = New DBHelperClient.Parameters("p_Active", chkActive.Checked)
            parms(4) = New DBHelperClient.Parameters("p_User", Session("user_id"))
            parms(5) = New DBHelperClient.Parameters("SurveyID", hdnSurveyID.Value)

            If hdnSurveyID.Value = 0 Then
                strSQL = " Insert into tblsurvey (SurveyName, SurveyType, Description, Active, CreatedBy, CreatedOn )" &
                         " Values (@p_SurveyName, @p_SurveyType, @p_Description, @p_Active, @p_User, CURRENT_TIMESTAMP ) ; " &
                         " Select LAST_INSERT_ID() ; "
                hdnSurveyID.Value = db.ExecuteScalar(CommandType.Text, strSQL, parms)
                lblSuccessMsg.Text = "Survey has been created successfully"
                lblSuccessMsg.Visible = True
            Else
                strSQL = " Update tblsurvey set SurveyName = @p_SurveyName, SurveyType = @p_SurveyType " &
                         " , Description = @p_Description, Active = @p_Active, ModifiedBy = @p_User " &
                         " , ModifiedOn = CURRENT_TIMESTAMP Where SurveyID = @SurveyID "
                db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
                lblSuccessMsg.Text = "Survey has been updated successfully"
                lblSuccessMsg.Visible = True
            End If

        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "btnSave_ServerClick")
        End Try
    End Sub

    Private Sub btntab3_ServerClick(sender As Object, e As EventArgs) Handles btntab3.ServerClick
        If hdnSurveyID.Value = 0 Then
            lblErrorMsg.Text = "Please Save survey first."
        Else
            Response.Redirect("SurveyQuestion.aspx?SurveyID=" & hdnSurveyID.Value)
        End If
    End Sub

    Private Sub btntab2_ServerClick(sender As Object, e As EventArgs) Handles btntab2.ServerClick
        If hdnSurveyID.Value = 0 Then
            lblErrorMsg.Text = "Please Save survey first."
        Else
            Response.Redirect("SurveyDesign.aspx?SurveyID=" & hdnSurveyID.Value)
        End If
    End Sub

    Private Sub btntab4_ServerClick(sender As Object, e As EventArgs) Handles btntab4.ServerClick
        If hdnSurveyID.Value = 0 Then
            lblErrorMsg.Text = "Please Save survey first."
        Else
            Response.Redirect("SurveyFinalize.aspx?SurveyID=" & hdnSurveyID.Value)
        End If
    End Sub

    Private Sub lnkSurvey_ServerClick(sender As Object, e As EventArgs) Handles lnkSurvey.ServerClick
        Try
            If hdnSurveyID.Value = 0 Then
                lblErrorMsg.Text = "Please Save survey first."
            Else
                Dim url As String
                url = "http://localhost:54464/Surveypage.aspx?PMODE=P&CGUID=" & Session("CGUID") & "&SurveyID=" & hdnSurveyID.Value
                Dim s As String = "window.open('" & url + "', '_blank', 'width=screen.width,height=screen.height,fullscreen=yes');"
                ClientScript.RegisterStartupScript(Me.GetType(), "OPEN_WINDOW", s, True)
            End If
        Catch ex As Exception
            lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "lnkSurvey_ServerClick")
        End Try

    End Sub
End Class