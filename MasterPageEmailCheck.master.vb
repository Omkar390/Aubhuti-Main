Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Exception
Imports System.Security.Cryptography

Partial Class MasterPageEmailCheck
    Inherits System.Web.UI.MasterPage
    Protected BindActivity As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("user_id") Is Nothing Then
            Response.Redirect("Login.aspx")
        End If

        litDateTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm")
        'pEmail.Text = Session("email")
        'pAddInf.Text = Request.Url.ToString

	End Sub

    Protected Sub UpdateTimer_Tick(ByVal sender As Object, ByVal e As System.EventArgs)
        litDateTime.Text = System.DateTime.Now.ToString("yyyy-MM-dd hh:mm")
    End Sub
    

    Protected Shared Function Decrypt(ByVal stringToDecrypt As String) As String
        Dim key() As Byte = {}
        Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

        Dim inputByteArray(stringToDecrypt.Length) As Byte

        key = System.Text.Encoding.UTF8.GetBytes("PVPSCRSTIPL".Substring(0, 8))
        Dim des As New DESCryptoServiceProvider()
        stringToDecrypt = stringToDecrypt.Replace(" ", "+")
        inputByteArray = Convert.FromBase64String(stringToDecrypt)
        Dim ms As New MemoryStream()
        Dim cs As New CryptoStream(ms, des.CreateDecryptor(key, IV), _
            CryptoStreamMode.Write)
        cs.Write(inputByteArray, 0, inputByteArray.Length)
        cs.FlushFinalBlock()
        Dim encoding As System.Text.Encoding = System.Text.Encoding.UTF8
        Return encoding.GetString(ms.ToArray())

    End Function

    'valueTxt = EmailManager.Decrypt(txt.Text)
    'valueTxt = EmailManager.Encrypt(txt.Text)

    Protected Shared Function Encrypt(ByVal stringToEncrypt As String) As String
        Try
            Dim key() As Byte = {}
            Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}

            key = System.Text.Encoding.UTF8.GetBytes(Left("PVPSCRSTIPL", 8))
            Dim des As New DESCryptoServiceProvider()
            Dim inputByteArray() As Byte = Encoding.UTF8.GetBytes( _
                stringToEncrypt)
            Dim ms As New MemoryStream()
            Dim cs As New CryptoStream(ms, des.CreateEncryptor(key, IV), _
                CryptoStreamMode.Write)
            cs.Write(inputByteArray, 0, inputByteArray.Length)
            cs.FlushFinalBlock()
            Return Convert.ToBase64String(ms.ToArray())
        Catch e As Exception
            Throw
        End Try
    End Function

   
End Class

'1394000101016903

'PUNB0139400
