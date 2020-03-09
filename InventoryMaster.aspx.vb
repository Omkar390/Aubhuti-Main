Imports System.Data
Imports System.IO
Partial Class InventoryMaster
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
            strSQL = " SELECT InventoryID, InventoryShort, InventoryLong, CommType, InventorySKU, " & _ 
                    " CASE WHEN IFNULL(AttachmentFile, '') = '' THEN '' ELSE '<img src=""images/attach_ico.png"" style=""width: 18%""/>' END AS Attachment , " & _
                    " DATE_FORMAT(I.CreatedDate, '%m/%d/%Y') AS createdts, U1.UserName as createdby " & _
                    " FROM tblinventory I " & _
                    " LEFT OUTER JOIN client_master.users U ON I.CreatedBy = U.userID " & _
                    " LEFT OUTER JOIN client_master.users U1 ON I.CreatedBy = U1.userID " & _
                    " WHERE Coalesce(isdeleted, 0) = 0 "
            strSQL = strSQL & " ORDER BY InventoryShort "
            rptMR.DataSource = db.DataAdapter(Data.CommandType.Text, strSQL).Tables(0)
            rptMR.DataBind()

        Catch ex As Exception

        End Try

    End Sub

    Protected Sub rptMR_ItemCommand(ByVal source As Object, ByVal e As System.Web.UI.WebControls.RepeaterCommandEventArgs) Handles rptMR.ItemCommand
        Try
            If e.CommandName = "Edit" Then
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim dt As New DataTable
                Dim parms(0) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("p_InventoryID", e.CommandArgument)
                hdnSelTargetID.Value = e.CommandArgument.Split("|")(0)
                strSQL = " Select InventoryShort, CommType, InventorySKU, AttachmentFile from tblinventory where InventoryID = @p_InventoryID " 
                dt = db.DataAdapter(CommandType.Text, strSQL, parms).Tables(0)
                pName.Text = dt.Rows(0).Item("InventoryShort").ToString
                pType.ClearSelection()
                pType.Items.FindByText(dt.Rows(0).Item("CommType")).Selected = True
                pSKU.Text = dt.Rows(0).Item("InventorySKU").ToString
                If dt.Rows(0).Item("AttachmentFile").ToString = "" Then
                    divatt.Visible = False
                Else
                    divatt.Visible = True
                    pfile.Text = dt.Rows(0).Item("AttachmentFile").ToString
                End If

                ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "showeditpopup", "setTimeout('$(""#edittemplate"").show();',500);", True)

            ElseIf e.CommandName = "Del" Then
                Dim strSQL As String
                Dim db As New DBHelperClient
                Dim parms1(1) As DBHelperClient.Parameters
                parms1(0) = New DBHelperClient.Parameters("p_InventoryID", e.CommandArgument.Split("|")(0))
                parms1(1) = New DBHelperClient.Parameters("userid", Session("user_id"))
                strSQL = " Update tblinventory SET IsDeleted = '1' , ModifiedDate = CURRENT_TIMESTAMP " & _
                   " ,ModifiedBy = @userid WHERE InventoryID = @p_InventoryID "
                db.ExecuteNonQuery(CommandType.Text, strSQL,parms1)
                BindSelTargets()

            ElseIf e.CommandName = "Download" Then

                Response.Buffer = False
                Response.ContentType = "application/octet-stream"

                Dim db As New DBHelperClient
                Dim parms(0) As DBHelperClient.Parameters
                parms(0) = New DBHelperClient.Parameters("p_InventoryID", e.CommandArgument)
                Dim dt As New DataTable
                Dim strQuery As String = "Select AttachmentFile from tblinventory where InventoryID = @p_InventoryID " 
                dt = db.DataAdapter(CommandType.Text, strQuery, parms).Tables(0)

                Dim filename As String = dt.Rows(0).Item(0).ToString
                Dim path As String = ConfigurationManager.AppSettings("InventAttachPath") & Session("client_id")
                Dim file1 As New FileInfo(path & "\" & filename)
                Dim len As Integer = CInt(file1.Length), bytes As Integer

                Response.AppendHeader("content-length", len.ToString())
                Response.AddHeader("Content-Disposition", "attachment;filename=" + dt.Rows(0).Item(0).ToString)
                Dim buffer As Byte() = New Byte(1023) {}
                Dim outStream As Stream = Response.OutputStream
                Using stream As Stream = File.OpenRead(path & "\" & filename)
                    While len > 0 AndAlso (InlineAssignHelper(bytes, stream.Read(buffer, 0, buffer.Length))) > 0
                        outStream.Write(buffer, 0, bytes)
                        len -= bytes
                    End While
                End Using
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub btnMeetRefresh_ServerClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMeetRefresh.ServerClick
        BindSelTargets()
    End Sub

    Private Sub lnkSaveList_Click(sender As Object, e As EventArgs) Handles lnkSaveList.Click
        Try

            Dim AttachId As Integer 
            Dim db As New DBHelperClient
            Dim parms(6) As DBHelperClient.Parameters
            If hdnSelTargetID.Value = 0 Then
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "I")
            Else
                parms(0) = New DBHelperClient.Parameters("p_dFlag", "U")
            End If
            parms(1) = New DBHelperClient.Parameters("p_InventoryID", hdnSelTargetID.Value)
            parms(2) = New DBHelperClient.Parameters("p_Inventory", pName.Text)
            parms(3) = New DBHelperClient.Parameters("p_Type", pType.SelectedItem.Text)
            parms(4) = New DBHelperClient.Parameters("p_SKU", pSKU.Text)
            parms(5) = New DBHelperClient.Parameters("p_AttachmentFile", pfileUpload.PostedFile.FileName)
            parms(6) = New DBHelperClient.Parameters("p_user_id", Session("user_id"))
            If hdnSelTargetID.Value = 0 Then
                hdnSelTargetID.Value = db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IUDInventoryMstr", parms)
            Else
                db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_IUDInventoryMstr", parms)
            End If

            If pfileUpload.PostedFile.FileName  <> "" Then
                Dim dbTA As New DBHelperClient
                Dim parmsA(1) as DBHelperClient.Parameters
                parmsA(0) = New DBHelperClient.Parameters("p_InventoryID", hdnSelTargetID.Value)
                parmsA(1) = New DBHelperClient.Parameters("p_AttachmentFile", hdnSelTargetID.Value & "_" & pfileUpload.PostedFile.FileName)

                Dim strSQL As String
                strSQL = " Update tblinventory set AttachmentFile = @p_AttachmentFile where InventoryID = @p_InventoryID " 
                db.ExecuteNonQuery(CommandType.Text, strSQL, parmsA)

                If Not System.IO.Directory.Exists(ConfigurationManager.AppSettings("InventAttachPath") & Session("client_id")) Then
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings("InventAttachPath") & Session("client_id"))
                End If
                pfileUpload.PostedFile.SaveAs(ConfigurationManager.AppSettings("InventAttachPath") & Session("client_id") & "\" & hdnSelTargetID.Value & "_" & pfileUpload.PostedFile.FileName)
            End If

            BindSelTargets()

        Catch ex As Exception

        End Try
    End Sub

    Protected Sub showattachment(ByVal sender As Object, ByVal e As System.EventArgs)

        Try

            Response.Buffer = False
            Response.ContentType = "application/octet-stream"

            Dim db As New DBHelperClient
            Dim parms(0) As DBHelperClient.Parameters
            parms(0) = New DBHelperClient.Parameters("p_InventoryID", hdnSelTargetID.Value)
            Dim dt As New DataTable
            Dim strQuery As String = "Select AttachmentFile from tblinventory where InventoryID = @p_InventoryID " 
            dt = db.DataAdapter(CommandType.Text, strQuery, parms).Tables(0)

            Dim filename As String = DirectCast(sender, LinkButton).CommandArgument & dt.Rows(0).Item(0).ToString
            Dim path As String = ConfigurationManager.AppSettings("InventAttachPath") & Session("client_id")
            Dim file1 As New FileInfo(path & "\" & filename)
            Dim len As Integer = CInt(file1.Length), bytes As Integer

            Response.AppendHeader("content-length", len.ToString())
            Response.AddHeader("Content-Disposition", "attachment;filename=" + dt.Rows(0).Item(0).ToString)
            Dim buffer As Byte() = New Byte(1023) {}
            Dim outStream As Stream = Response.OutputStream
            Using stream As Stream = File.OpenRead(path & "\" & filename)
                While len > 0 AndAlso (InlineAssignHelper(bytes, stream.Read(buffer, 0, buffer.Length))) > 0
                    outStream.Write(buffer, 0, bytes)
                    len -= bytes
                End While
            End Using

        Catch ex As Exception
            'lblErrorMsg.Text = "Sorry ... this page does not appear to be working. The error has been logged and we are reviewing it."
            'lblErrorMsg.Visible = True
            SSManager.LogAppError(ex, "showattachment")
        End Try
    End Sub

    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

    Private Sub lnkdelAttach_Click(sender As Object, e As EventArgs) Handles lnkdelAttach.Click
        Try
            Dim dbTA As New DBHelperClient
            Dim parmsA(0) as DBHelperClient.Parameters
            parmsA(0) = New DBHelperClient.Parameters("p_InventoryID", hdnSelTargetID.Value)

            Dim strSQL As String
            strSQL = " Update tblinventory set AttachmentFile = '' where InventoryID = @p_InventoryID " 
            dbTA.ExecuteNonQuery(CommandType.Text, strSQL, parmsA)
            BindSelTargets()

        Catch ex As Exception

        End Try
    End Sub
End Class
