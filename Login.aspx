<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Login.aspx.vb" Inherits="Login"  %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html class="no-js">
<!--<![endif]-->

<head>
    <meta charset="utf-8">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Anubhuti</title>
    <meta name="description" content="">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <!-- Place favicon.ico and apple-touch-icon.png in the root directory -->
    <link href="LandingPage/css/OpenSans.css" rel="stylesheet" />
    <link href='LandingPage/css/RobotoSlab.css' rel='stylesheet' type='text/css' />
    <link href='LandingPage/css/Montserrat.css' rel='stylesheet' type='text/css' />
    <link href='LandingPage/css/Slabo27px.css' rel='stylesheet' type='text/css' />
    <link href='LandingPage/css/Indie+Flower.css' rel='stylesheet' type='text/css' />
    <link href='LandingPage/css/DancingScript.css' rel='stylesheet' type='text/css' />
    <link href="LandingPage/css/font-awesome.css" rel="stylesheet" />
    <link rel="stylesheet" href="LandingPage/css/animate.css" />
    <link rel="stylesheet" href="LandingPage/css/normalize.css" />
    <link rel="stylesheet" href="LandingPage/css/bootstrap.css" />
    <link rel="stylesheet" href="LandingPage/css/style.css" />
    <link rel="stylesheet" href="LandingPage/css/responsive.css" />
    <script type="text/javascript" src="LandingPage/js/vendor/modernizr-2.6.2.min.js"></script>
    <script type="text/javascript" src="https://www.google.com/recaptcha/api.js"></script>

<style type="text/css">

    .mybutton2 {
        cursor: pointer;
        width: 70%;
        border: none;
        background: #F14517;
        color: #FFF;
        margin: 0 0 5px;
        padding: 15px;
        font-size: 15px;
        border-radius: 4px;
        font-size: 19px;
        font-weight: bold;
    }
.mybutton2:hover{
    background: #DB2420;
    -webkit-transition: background 0.3s ease-in-out;
    transition: background-color 0.3s ease-in-out;
}

    
.create_button{
	text-align:center;	
}
.create_button a.cancel{
	background:#28313c;
	padding:0 25px;
	display:inline-block;
	font:400 18px/36px 'Lato';
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	color:#fff;	
}
.create_button a.ok{
	background:#086541;
	padding:0 25px;
	display:inline-block;
	font:400 18px/36px 'Lato';
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	color:#fff;	
}	

.create_button .mybutton{
	background:#086541;
	padding:0 25px;
	display:inline-block;
	font:400 18px/36px 'Lato';
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	color:#fff;	
    margin-top: -8px;
}	

.ac_list th span {
    padding: 0 6px 0 0;
}
.create_button a.prev{
	background:#24c381;
	padding:0 25px;
	display:inline-block;
	font:400 18px/36px 'Lato';
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	color:#fff;
	margin: 0 0 0 7px;	
}





.clr{clear:both;}

    .e_detailpop{
	    position:absolute;
	    height:100%;
	    background:rgba(0, 0, 0, .85);
	    z-index:9999;	
	    top:0;
	    width:100%;
    }
    .create_edit{
	background:#fff;
	left:50%;
	top:50%;
	transform:translatex(-50%) translateY(-50%);
	width:1016px;
	position:absolute;
	margin:0 auto;
	-webkit-border-radius: 4px;
-moz-border-radius: 4px;
border-radius: 4px;	
}
    .close_btn1 {
    position: absolute;
    right: -13px;
    top: -11px;
    width: 25px;
}

.close_btn1 > img {
    width: 100%;
}


.create_form{
	padding:10px;	
}
.create_form .create_row{
	padding:0 0 5px 0;	
}
.create_form .create_left{
	float:left;
	width:49%;	
}
.create_form .create_row{
	padding:0 0 10px 0;	
}
.create_form .create_left .field_name{
	width:25%;
	float:left;	
}
.create_form .create_left .field_name label{
	color: #3e3c3c;
    display: block;
    font: 400 14px "Lato";
    padding: 4px 0 0;	
}
.create_form .create_left .inp_f{
	width:74%;
	float:left;
	position:relative;	
}
.create_form .create_left .inp_f input{
	width:100%;
	border:solid 1px #bcbbbb;
	height:30px;
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	padding:0 5px;	
}
.create_form .create_left .inp_f select {
    background: #ffffff url("../images/drop-list.png") no-repeat scroll 97% center;
    border: 1px solid #bcbbbb;
    border-radius: 4px;
    height: 30px;
    margin:0;
    width: 100%;
}
.create_form .create_left .inp_f strong{
	font:700 14px 'Lato';	
}
.create_form .create_rt{
	float:right;
}	
.create_form .ct_left{
	width:12%;
	float:left;color: #3e3c3c;
    display: block;
    font: 400 14px "Lato";
    padding: 4px 0 0;	
}
.create_form .ct_rt{
	width:88%;
	float:right;	
}
.create_form .ct_rt textarea{
	width:100%;
	border:solid 1px #bcbbbb;
	height:80px;
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	padding:5px;	
	
}
.create_button{
	text-align:center;	
}
.create_button a.cancel{
	background:#28313c;
	padding:0 25px;
	display:inline-block;
	font:400 18px/36px 'Lato';
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	color:#fff;	
}
.create_button a.ok{
	background:#086541;
	padding:0 25px;
	display:inline-block;
	font:400 18px/36px 'Lato';
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	color:#fff;	
}	

.create_button .mybutton{
	background:#086541;
	padding:0 25px;
	display:inline-block;
	font:400 18px/36px 'Lato';
	-webkit-border-radius: 4px;
	-moz-border-radius: 4px;
	border-radius: 4px;
	color:#fff;	
    margin-top: -8px;
}	

.create_form .create_left .field_name label em{
	color:#f00;	
}
.create_form .create_left .field_name{
	float:none;
	width:100%;	
}


</style>



</head>

<body>
<form  id="form1" runat="server">
    <script type="text/javascript">
        function disablevals() {
            document.getElementById("Login1_Username").removeAttribute("required");
            document.getElementById("Login1_PassWord").removeAttribute("required");

            document.getElementById("txtfname").required = true;
            document.getElementById("txtlname").required = true;
            document.getElementById("txtemail").required = true;

            return true;
        }
        function disablevalidation() {
            //$('#Login1_Username').removeAttr('required');
            //$('#Login1_PassWord').removeAttr('required');
            return true;
        }
        function addvalidation() {
            document.getElementById("Login1_Username").required = true;
            document.getElementById("Login1_PassWord").required = true;

            document.getElementById("txtfname").removeAttribute("required");
            document.getElementById("txtlname").removeAttribute("required");
            document.getElementById("txtemail").removeAttribute("required");

            //$('#Login1_Username').addAttr('required');
            //$('#Login1_PassWord').addAttr('required');
            return true;
        }


        function newuserconf() {
            $("#newuserconf").show();
        }

        function sendEmail() {
            $.ajax({
                type: "POST",
                url: "WebMethods.aspx/GetPassword",
                data: '{ email: "' + document.getElementById("Login1_Username").value + '"}',
                contentType: "application/json; charset=utf-8",
                async: false,
                dataType: "json",
                success: function (data) {
                    alert(data.d[1]);
                },
                failure: function(response) {
                    alert(response.d);
                }
            }); 
        }

	</script>

    <div class="background-part">
        <div class="header-area">
            <div class="container">
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-5">
                            <div class="left-part">
                                <a href=""><img src="LandingPage/img/logo.png" class="img-responsive" style="height:60px;padding-top:10px;margin-top:10px;" /></a>
                            </div>
                        </div>
						<div class="col-md-6 col-md-offset-1 row " style="padding-top:15px;">
							<p class="beta"></p><p style="color: aqua;"><asp:Literal ID="litErrorMsg" runat="server"></asp:Literal></p>
							<asp:Login ID="Login1" runat="server" style="width:100%">
								<LayoutTemplate>
									<div class="col-md-5 for_padding" style="">
										<div class="form-group">
											<asp:TextBox TabIndex="1" runat="server" id="Username" CssClass="form-control custom_form" MaxLength="100" placeholder="User Name" required></asp:TextBox>
											<%--<input type="text" runat="server" id="Username" class="form-control custom_form" placeholder="User Name" required MaxLength="100" />--%>
										</div>
									</div>
									<div class="col-md-5 for_padding">
										<div class="form-group">
											<asp:TextBox TabIndex="2" TextMode="Password" AutoCompleteType="None" runat="server" id="PassWord" MaxLength="100" CssClass="form-control custom_form" placeholder="Password" required  ></asp:TextBox>
											<%--<input type="password" runat="server" id="PassWord" class="form-control custom_form" placeholder="Password" required  MaxLength="20" />--%>
										</div>
									</div>
									<div class="col-md-2 for_small">
										<asp:Button ID="btnLogin" OnClientClick="return addvalidation();" runat="server" Text="login" CssClass="login" CommandName="Login"/>
										<%--<button type="submit" class="login" name="submit">Login</button>--%>
									</div>
								</LayoutTemplate>
							</asp:Login>
							<div class="col-md-2 for_small" style="width:50%">
								<a ID="btnGetPassword" style="color:white" onclick='sendEmail();' href="#">Forgot Password</a>
							</div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="container">
            <div class="col-md-12 for_margin">
                <div class="col-md-7 " style="color:#fff;margin-top:5px;">
                    <div class="right-content-parts">
                        <h2><span style="color:#fff;font-size:50px;">Anubhuti <br /><br />Nurturing Young Minds</span></h2>
                    </div>
                </div>
                <div class="col-md-4 col-md-offset-1 ">
                    <div class="left-contact-form">
                        
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="story_part">
        <div class="container">
            <div class="col-md-12">
                <div class="col-md-10 col-md-offset-1">
                    <div class="left_story">
                        <h2>Our Story:</h2>
                        <p>A Team of Social Entrepreneurs</p>
                        <p>We at Anubhuti firmly believe that:
• Thoughts and Attitude largely define a personality
• Both can be moulded as a mind never really grows old

We are a group of young minds working collectively to help ourselves become better human beings.
We also assist willing individuals to expand reform or refurbish their horizons of thoughts and choose the optimistic route anywhere and everywhere.
We believe in Anubhuti: Learning here is experiential. Anubhuti occurs through discussions, brain storming sessions and a variety of other enlightening activities.</p>
                    </div>
                </div>
               
            </div>
        </div>
    </div>
    <div class="footers" style="color:#fff;padding-top:20px;padding-bottom:15px;">
        <div class="container">
            <div class="col-md-12">
                <div class="inner_footer text-center">
                    <p>© 2020 Anubhuti, All Rights Reserved </p>
                </div>
            </div>
        </div>
    </div>

    <section class='e_detailpop' id='newuserconf' style="display:none;height:100%"><!--Email Detail Pop up!-->
	    <div class="create_edit">
    	    <div class="close_btn1"><img src="images/close.png" style="cursor:pointer" onclick='$("#newuserconf").hide();' /></div>
            <div class="create_form"><!--Create Edit form!-->
                <div class="create_row"><!--row !-->
       		        <div class="create_left" style="width:100%"><!--left section!-->
            	        <div class="field_name"><label><center><h1>We have logged your request and will respond soon. Thanks!</h1></center></label></div>
                        <div class="clr"></div>
                    </div><!--left section!-->
                    <div class="clr"></div>
                    <div class="create_button">
                        <center>
                            <a href="#" class="cancel"  onclick='$("#newuserconf").hide();'  >Close</a>
                        </center>
                    </div>
                    <div class="clr"></div>
                </div><!--row !-->
            </div><!--Create Edit form!-->
        </div>
    </section><!--Email Detail Pop up!-->




    <script type="text/javascript" src="https://ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery-easing/1.3/jquery.easing.min.js"></script>
    <script  type="text/javascript">
        window.jQuery || document.write('<script src="js/vendor/jquery-1.10.2.min.js"><\/script>')
    </script>
    <script type="text/javascript" src="LangindPage/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="LangindPage/js/plugins.js"></script>
    <script type="text/javascript" src="LangindPage/js/main.js"></script>
    <script type="text/javascript" src="LangindPage/js/wow.min.js"></script>

</form>
</body>

</html>