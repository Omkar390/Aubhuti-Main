﻿Imports System.Data
Partial Class surveytypemaster
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                Dim db As New DBHelper
                BindSelTargets()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Protected Sub BindSelTargets()
        Try
            Dim db As New DBHelperClient
            Dim strSQL As String
            strSQL = " SELECT surveytypeid, surveytype, U.UserName , DATE_FORMAT(ST.createdts, '%m/%d/%Y') AS createdts, U1.UserName as createdby " & _
                    " FROM tblsurveytype ST " & _
                    " LEFT OUTER JOIN client_master.users U ON ST.CreatedBy = U.userID " & _
                    " LEFT OUTER JOIN client_master.users U1 ON ST.CreatedBy = U1.userID " & _
                    " WHERE Coalesce(isdeleted, 0) = 0 "
            strSQL = strSQL & " ORDER BY surveytype "
            rptMR.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
            rptMR.DataBind()

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub rptMR_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptMR.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                hdnSelTargetID.Value = e.CommandArgument.Split("|")(0)
                pMRName.Text = e.CommandArgument.Split("|")(1)
                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "showeditpopup", "setTimeout('$(""#edittemplate"").show();',500);", True)

            ElseIf e.CommandName = "Del" Then
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms1(1) As DBHelperClient.Parameters
                parms1(0) = New DBHelperClient.Parameters("surveytypeid", e.CommandArgument.Split("|")(0))
                parms1(1) = New DBHelperClient.Parameters("userid", Session("user_id"))
                strSQL = " Update tblsurveytype SET isdeleted = '1' , modifiedts = CURRENT_TIMESTAMP " & _
                   " ,modifiedby = @userid WHERE surveytypeid = @surveytypeid "
                db.ExecuteNonQuery(CommandType.Text, strSQL,parms1)
                BindSelTargets()

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnMeetRefresh_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMeetRefresh.ServerClick
        BindSelTargets()
    End Sub

    Private Sub lnkSaveList_Click(sender As Object, e As EventArgs) Handles lnkSaveList.Click
        Try

            Dim db As New DBHelperClient
            Dim parms(3) As DBHelperClient.Parameters
            If hdnSelTargetID.Value = 0 Then
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            Else
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "U")
            End If
            parms(1) = New DBHelperClient.Parameters("p_surveytypeid", hdnSelTargetID.Value)
            parms(2) = New DBHelperClient.Parameters("p_surveytype", pMRName.Text)
            parms(3) = New DBHelperClient.Parameters("p_user_id", Session("user_id"))
            If hdnSelTargetID.Value = 0 Then
                hdnSelTargetID.Value = db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IUDSurveyType", parms)
            Else
                db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IUDSurveyType", parms)
            End If

            BindSelTargets()

        Catch ex As Exception

        End Try
    End Sub

End Class
