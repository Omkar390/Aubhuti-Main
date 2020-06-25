Imports System.Data
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization
Imports System.Text
Imports System.Runtime.Serialization.Json
Imports Newtonsoft.Json

Partial Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Not (Request.Cookies("UserName")) Is Nothing Then
                Login1.UserName = Request.Cookies("UserName").Value
                'squaredFour.Checked = True
            End If
        End If
    End Sub


    Protected Sub _login_Authenticate(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.AuthenticateEventArgs) Handles Login1.Authenticate
        Try

            'Stpes performed for user login
            'First clear the old session!
            Session.Clear()
            Dim ipaddr As String = GetUserIP()
            Dim Client_ID As Integer = 0
            Dim ds As New DataSet()
            Login1.UserName = Login1.UserName.Trim
            ds = SSManager.Validate(Login1.UserName, Login1.Password, ipaddr)
            If (ds.Tables(0).Rows.Count > 0) Then
                'redirect to client selection screen
                e.Authenticated = True
                Session("user_id") = ds.Tables(0).Rows(0)("userid")
                Session("email") = ds.Tables(0).Rows(0)("email")
                Session("FirstName") = ds.Tables(0).Rows(0)("fname")
                Session("LastName") = ds.Tables(0).Rows(0)("lname")
                FormsAuthentication.SetAuthCookie(Login1.UserName, True)

                'SSManager.LogVisit(ipaddr, Request.UserAgent, Request.Browser.Browser + " " + Request.Browser.MajorVersion.ToString, ClientID, ds.Tables(1).Rows(0)(0))


                Response.Cookies("UserName").Value = Login1.UserName.Trim

                Dim db As New DBHelper
                Dim dt As New DataTable
                Dim strSQL As String = "Select CM.client_name, CM.client_id, SUBSTRING(database_name,8,4) AS dbname, '2' as ListOrder , 'SalesShark.aspx' as hrefPage, CM.GUID as CGUID, UC.userRole" &
                        " from tbluserclient UC Inner Join client_master CM on UC.client_id = CM.client_id " &
                        " where database_name='client_2002' AND CM.Active = 1 and UC.Active = 1 and userid = " & Session("user_id") & " Order by ListOrder, client_name "
                dt = db.DataAdapter(CommandType.Text, strSQL).Tables(0)
                Session("user_group") = "EmailCheck"
                Session("client_id") = dt.Rows(0)("client_id")
                Session("dbname") = dt.Rows(0)("dbname")
                Session("CGUID") = dt.Rows(0)("CGUID")
                Session("userRole") = dt.Rows(0)("userRole")
                Response.Redirect("assignlist.aspx", False)

            End If
            'lblError.Visible = True
            litErrorMsg.Visible = True
            litErrorMsg.Text = "Login failed. Invalid login and/or password."
            e.Authenticated = False
        Catch ex As Exception
            litErrorMsg.Text = "Login failed. Invalid login and/or password."
            e.Authenticated = False
        End Try
    End Sub

    Public Function GetUserIP() As String
        Dim strIP As String = [String].Empty
        Dim httpReq As HttpRequest = HttpContext.Current.Request

        'test for non-standard proxy server designations of client's IP
        If httpReq.ServerVariables("HTTP_CLIENT_IP") IsNot Nothing Then
            strIP = httpReq.ServerVariables("HTTP_CLIENT_IP").ToString()
        ElseIf httpReq.ServerVariables("HTTP_X_FORWARDED_FOR") IsNot Nothing Then
            strIP = httpReq.ServerVariables("HTTP_X_FORWARDED_FOR").ToString()
            'test for host address reported by the server
            'if exists
            'and if not localhost IPV6 or localhost name
        ElseIf (httpReq.UserHostAddress.Length <> 0) AndAlso ((httpReq.UserHostAddress <> "::1") AndAlso (httpReq.UserHostAddress <> "localhost")) Then
            strIP = httpReq.UserHostAddress
        Else
            'finally, if all else fails, get the IP from a web scrape of another server
            'Dim request As WebRequest = WebRequest.Create("http://checkip.dyndns.org/")
            'Using response As WebResponse = request.GetResponse()
            '    Using sr As New StreamReader(response.GetResponseStream())
            '        strIP = sr.ReadToEnd()
            '    End Using
            'End Using
            ''scrape ip from the html
            'Dim i1 As Integer = strIP.IndexOf("Address: ") + 9
            'Dim i2 As Integer = strIP.LastIndexOf("</body>")
            'strIP = strIP.Substring(i1, i2 - i1)
        End If
        Return strIP
    End Function
    Protected Sub createuserandredirect()
        '--------------- create new user    -----------------
        CreateUser(txtfname.Text.Trim, txtlname.Text.Trim, txtemail.Text.Trim, txtconame.Text.Trim)
        Dim MailObj As MailMessage = New MailMessage()
        'SendEmailToKarl(MailObj)
        Dim html As String = File.ReadAllText(Server.MapPath("Thanks2.html"))
        'SendMandrillHTTP(txtemail.Text.Trim, txtfname.Text.Trim + " " + txtlname.Text.Trim, ConfigurationManager.AppSettings("SSSupportEmail").ToString, ConfigurationManager.AppSettings("SSSupportName").ToString,
        '"Thanks for subscribing to VendorShark!", html, 1)
        'SendMandrillHTTP(txtEmail.Text.Trim, txtFirstName.Text.Trim + " " + txtLastName.Text.Trim, ConfigurationManager.AppSettings("VSSupportEmail").ToString, ConfigurationManager.AppSettings("VSSupportName").ToString, _
        '                 "Thanks for subscribing to VendorShark!", sBody, 1)
        Response.Redirect("Thanks2.html", True)
        '---------------------------------------------------------------------------------------
        ''check if user already exists!!!!!!!!!!!!
        'Dim strSQL As String
        'strSQL = "Insert into users (fname, lname, email, company, username, userLogin, userPassword, created_on ) " &
        '        " Values (@p_fname, @p_lname, @p_email, @p_company, @p_username, @p_userLogin, @p_userPassword, CURRENT_TIMESTAMP) "
        'Dim db As New DBHelper
        'Dim parms(6) As DBHelper.Parameters
        'parms(0) = New DBHelper.Parameters("p_fname", txtfname.Text)
        'parms(1) = New DBHelper.Parameters("p_lname", txtlname.Text)
        'parms(2) = New DBHelper.Parameters("p_email", txtemail.Text)
        'parms(3) = New DBHelper.Parameters("p_company", txtconame.Text)
        'parms(4) = New DBHelper.Parameters("p_username", txtfname.Text & " " & txtlname.Text)
        'parms(5) = New DBHelper.Parameters("p_userLogin", txtemail.Text)
        'parms(6) = New DBHelper.Parameters("p_userPassword", "Welcome1234!")
        'db.ExecuteNonQuery(CommandType.Text, strSQL, parms)
        ''ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "newuserconf", "setTimeout('newuserconf();',500);", True)
    End Sub
    Public Sub btnSubmit_ServerClick(sender As Object, e As EventArgs) Handles btnRegister.Click
        Try
            Dim isValidCaptcha As Boolean = ValidateReCaptcha()
            If isValidCaptcha Then
                lblCaptchaMessage.Text = ""
                createuserandredirect()
            Else
                lblCaptchaMessage.Text = "Invalid Captcha"
            End If
        Catch ex As Exception
            litErrorMsg.Text = "Login failed. Invalid login and/or password."
        End Try
    End Sub

    Public Sub CreateUser(ByVal UserEmail As String, ByVal FirstName As String, ByVal LastName As String, ByVal company As String)
        Try
            Dim db As New DBHelper
            Dim parms(3) As DBHelper.Parameters
            parms(0) = New DBHelper.Parameters("i_fname", FirstName)
            parms(1) = New DBHelper.Parameters("i_lname", LastName)
            parms(2) = New DBHelper.Parameters("i_email", UserEmail)
            parms(3) = New DBHelper.Parameters("i_company", company)
            db.ExecuteNonQuery(CommandType.StoredProcedure, "SP_RegisterUser", parms)
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Function SendMandrillHTTP(ByVal Receiver As String, ByVal ReceiverName As String, ByVal sender As String,
                                        ByVal sendername As String, ByVal sSubject As String, ByVal sBody As String, ByVal trackemail As Integer) As String

        Dim result As String = "ERR-"
        Try
            Dim emailaddressfrom As New MailAddress(sender, sendername)
            Dim Receiveremail As New MailAddress(Receiver, ReceiverName)

            '------------------------------------------------------------------------------------------------
            'sending using http post! --- START
            '------------------------------------------------------------------------------------------------
            Dim httpWebRequest = DirectCast(WebRequest.Create("https://api.sparkpost.com/api/v1/transmissions"), HttpWebRequest)
            httpWebRequest.ContentType = "text/json"
            httpWebRequest.Method = "POST"
            httpWebRequest.Headers.Add("Authorization", "1")

            Dim text As String = New JavaScriptSerializer().Serialize(sBody)

            Dim strTrackOpens As String = "false"
            Dim strTrackClicks As String = "false"

            Dim json2 As String = "{" +
                """options""" + ":" + "{" +
                        """open_tracking""" + ":" + "" + strTrackOpens + "," +
                        """click_tracking""" + ":" + "" + strTrackClicks + "" +
                    "}," +
                    """recipients""" + ":" + "[" +
                        "{" +
                            """address""" + ":{" +
                            """email""" + ":" + """" + Receiver + """," +
                            """name""" + ":" + """" + ReceiverName + """}" +
                        "}" +
                    "]," +
                """content""" + ":" + "{" +
                    """from""" + ":" + "{" +
                        """name""" + ":" + """" + sendername + """," +
                        """email""" + ":" + """" + "ITSupport@tic-us.com" + """" +
                        "}," +
                        """subject""" + ":" + """" + sSubject + """," +
                        """reply_to""" + ":" + """" + sender + """," +
                        """html""" + ":" + "" + text + ""

            'json2 = json2 + ",""attachments""" + ":" + "["
            'json2 = json2 + "{" + _
            '        """type""" + ":" + """" + "image/png" + """," + _
            '        """name""" + ":" + """" + "vslogo" + """," + _
            '        """data""" + ":" + """" + Convert.ToBase64String(File.ReadAllBytes(Server.MapPath("./imagesthanks/logo.png"))) + """" + _
            '    "}"
            'json2 = json2 + "]"
            json2 = json2 + "} }"


            Using streamWriter = New StreamWriter(httpWebRequest.GetRequestStream())
                'Dim json As String = New JavaScriptSerializer().Serialize(New With {json2})
                Dim json As String = json2
                streamWriter.Write(json)
                streamWriter.Flush()
                streamWriter.Close()
                Dim httpResponse = DirectCast(httpWebRequest.GetResponse(), HttpWebResponse)
                Using streamReader = New StreamReader(httpResponse.GetResponseStream())
                    result = streamReader.ReadToEnd()
                End Using
            End Using
            ''------------------------------------------------------------------------------------------------
            ''sending using http post! --- END
            ''------------------------------------------------------------------------------------------------
            'Dim listgeoip As New List(Of String)(result.Split(New Char() {","c}).Length)
            'Dim parts As String() = result.Split(New Char() {","c})
            'Dim part As String
            'For Each part In parts
            '    listgeoip.Add(part)
            'Next

            'strMandrillStatus = listgeoip(1).Split(":")(1).Substring(1, listgeoip(1).Split(":")(1).Length - 2)
            'result = listgeoip(2).Split(":")(1).Substring(1, listgeoip(2).Split(":")(1).Length - 2)

        Catch webex As WebException
            litErrorMsg.Text = webex.Message
        Catch ex As Exception
            litErrorMsg.Text = ex.Message
        End Try
        Return result
    End Function

    Public Sub SendEmailToKarl(ByVal MailObj As MailMessage)
        Try
            MailObj.Subject = "SalesShark New User Registration"
            MailObj.Priority = MailPriority.High
            MailObj.From = New MailAddress(ConfigurationManager.AppSettings("SSSupportEmail").ToString, ConfigurationManager.AppSettings("SSSupportName").ToString)
            MailObj.Body = "A new user has registered for SalesShark. " + "<br/>" + "Name: " + txtfname.Text.Trim + " " + txtlname.Text +
                "<br/>" + "Email: " + txtemail.Text.Trim + "<br/>" + "Company: " + txtconame.Text.Trim + "<br/>"
            Dim strCC As New System.Net.Mail.MailAddress(ConfigurationManager.AppSettings("karlemail").ToString, ConfigurationManager.AppSettings("karlname").ToString)
            MailObj.To.Add(strCC)
            MailObj.Bcc.Add("moak@tic-us.com")
            MailObj.IsBodyHtml = True
            'Dim smtp As New SmtpClient("smtp.sparkpostmail.com")
            'smtp.Port = "587"
            'smtp.Credentials = New System.Net.NetworkCredential("SMTP_Injection", "1")
            'smtp.Send(MailObj)

        Catch ex As Exception
            SSManager.LogAppError(ex, "CreateAndSendEmail")
        End Try

    End Sub

    Private Function GetIpAddress() As String
        Dim ipAddress = String.Empty
        If (Not (Request.ServerVariables("HTTP_X_FORWARDED_FOR")) Is Nothing) Then
            ipAddress = Request.ServerVariables("HTTP_X_FORWARDED_FOR")
        ElseIf Not String.IsNullOrEmpty(Request.UserHostAddress) Then
            ipAddress = Request.UserHostAddress
        End If

        Return ipAddress
    End Function

    Public Function ValidateReCaptcha() As Boolean

        Dim EncodedResponse As String = Request.Form("g-Recaptcha-Response")
        Dim IsCaptchaValid As Boolean = IIf(ReCaptchaClass.Validate(EncodedResponse) = "True", True, False)

        If IsCaptchaValid Then
            Return True
        Else
            Return False
        End If

        'Dim errorMessage As String
        'Dim gresponse = Request("g-recaptcha-response")
        'Dim secret As String = "6LcQp5EUAAAAALAs-UXk5gn8Hy0_qllBfWnry8K5"
        'Dim ipAddress As String = GetIpAddress
        'Dim client = New WebClient
        'Dim url As String = String.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}", secret, gresponse, ipAddress)
        'Dim response = client.DownloadString(url)
        'Dim captchaResponse = JsonConvert.DeserializeObject(Of ReCaptchaResponse)(response)

        'ScriptManager.RegisterStartupScript(Me.Page, Me.Page.[GetType](), "testetetstets", "alert('" + captchaResponse.Success.ToString + "');", True)

        'If captchaResponse.Success Then
        '    Return true
        'Else
        '    Dim error1 = captchaResponse.ErrorCodes(0).ToLower
        '    Select Case (error1)
        '        Case "missing-input-secret"
        '            errorMessage = "The secret key parameter is missing."
        '        Case "invalid-input-secret"
        '            errorMessage = "The given secret key parameter is invalid."
        '        Case "missing-input-response"
        '            errorMessage = "The g-recaptcha-response parameter is missing."
        '        Case "invalid-input-response"
        '            errorMessage = "The given g-recaptcha-response parameter is invalid."
        '        Case Else
        '            errorMessage = "reCAPTCHA Error. Please try again!"
        '    End Select

        '    Return false
        'End If

    End Function

End Class

Public Class ReCaptchaResponse

    <JsonProperty("success")>
    Public Property Success As Boolean
        Get
        End Get
        Set
        End Set
    End Property

    <JsonProperty("error-codes")>
    Public Property ErrorCodes As List(Of String)
        Get
        End Get
        Set
        End Set
    End Property
End Class
