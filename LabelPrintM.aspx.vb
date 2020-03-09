Imports System.Net
Imports System.Data
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf

Partial Class LabelPrintM
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim rptSavePath, rptSavePathFile As String
        rptSavePath = Request.ServerVariables("APPL_PHYSICAL_PATH")  & Session("user_id")
        rptSavePathFile = Request.ServerVariables("APPL_PHYSICAL_PATH") & Session("user_id") & "\PrintFile" & Request.QueryString("emailblastprofileid") & ".pdf"

        If Not Directory.Exists(rptSavePath) Then
            Directory.CreateDirectory(rptSavePath)
        End If
        Dim dt As DataTable

        Dim db As New DBHelperClient
        Dim parms(1) As DBHelperClient.Parameters
        parms(0) = New DBHelperClient.Parameters("p_ListId", Request.QueryString("ListId"))
        parms(1) = New DBHelperClient.Parameters("p_ListType", Request.QueryString("ListType"))
        dt = db.DataAdapter(CommandType.StoredProcedure, "SP_GetLabels", parms).Tables(0)

        Dim pdfDoc As New Document(New Rectangle(252.0F, 90.0F))
        If File.Exists(rptSavePathFile) Then
            File.Delete(rptSavePathFile)
        End If
        Dim writer As PdfWriter = PdfWriter.GetInstance(pdfDoc, New FileStream(rptSavePathFile, FileMode.OpenOrCreate))
        Dim f_cb As BaseFont = BaseFont.CreateFont("c:\windows\fonts\calibri.ttf", BaseFont.CP1257, BaseFont.NOT_EMBEDDED)
        Dim f_cn As BaseFont = BaseFont.CreateFont("c:\windows\fonts\calibri.ttf", BaseFont.CP1257, BaseFont.NOT_EMBEDDED)

        pdfDoc.Open()

        Dim cb As PdfContentByte = writer.DirectContent

        For i = 0 To dt.Rows.Count - 1
            cb.BeginText()
            cb.SetFontAndSize(f_cn, 9)
            writeText(cb, dt.Rows(i).Item(0).ToString, 14, 71, f_cn, 12)
            cb.EndText()

            cb.BeginText()
            cb.SetFontAndSize(f_cn, 9)
            writeText(cb, dt.Rows(i).Item(1).ToString, 14, 56, f_cn, 12)
            cb.EndText()

            cb.BeginText()
            cb.SetFontAndSize(f_cn, 9)
            writeText(cb, dt.Rows(i).Item(2).ToString, 14, 41, f_cn, 12)
            cb.EndText()

            cb.BeginText()
            cb.SetFontAndSize(f_cn, 9)
            writeText(cb, dt.Rows(i).Item(3).ToString, 14, 26, f_cn, 12)
            cb.EndText()

            cb.BeginText()
            cb.SetFontAndSize(f_cn, 9)
            writeText(cb, dt.Rows(i).Item(4).ToString, 14, 11, f_cn, 12)
            cb.EndText()

            pdfDoc.NewPage()
        Next

        pdfDoc.Close()

        'Response.Buffer = False
        'Response.ContentType = "application/pdf"
        'Dim file__1 As New FileInfo(rptSavePathFile)
        'Dim len As Integer = CInt(file__1.Length), bytes As Integer
        'Response.AppendHeader("content-length", len.ToString())
        'Response.AddHeader("Content-Disposition", "attachment;filename=" + rptSavePathFile)
        'Dim buffer As Byte() = New Byte(1023) {}
        'Dim outStream As Stream = Response.OutputStream
        'Using stream As Stream = File.OpenRead(rptSavePathFile)
        '    While len > 0 AndAlso (InlineAssignHelper(bytes, stream.Read(buffer, 0, buffer.Length))) > 0
        '        outStream.Write(buffer, 0, bytes)
        '        len -= bytes
        '    End While
        'End Using

        Dim client As New WebClient()
        Dim buffer2 As [Byte]() = client.DownloadData(rptSavePathFile)
        Response.ContentType = "application/pdf"
        Response.AddHeader("content-length", buffer2.Length.ToString())
        Response.BinaryWrite(buffer2)
		
    End Sub
    Private Sub writeText(ByVal cb As PdfContentByte, ByVal Text As String, ByVal X As Integer, ByVal Y As Integer, ByVal font As BaseFont, ByVal Size As Integer)
        cb.SetFontAndSize(font, Size)
        cb.ShowTextAligned(PdfContentByte.ALIGN_LEFT, Text, X, Y, 0)
    End Sub


    Private Shared Function InlineAssignHelper(Of T)(ByRef target As T, ByVal value As T) As T
        target = value
        Return value
    End Function

End Class
