<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<!doctype html>
<!--[if lte IE 9]> <html class="lte-ie9" lang="en" dir="rtl"> <![endif]-->
<!--[if gt IE 9]><!-->
<html lang="en" dir="rtl">
<!--<![endif]-->
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="initial-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <!-- Remove Tap Highlight on Windows Phone IE -->
    <meta name="msapplication-tap-highlight" content="no" />

    <link rel="icon" type="image/png" href="assets/img/favicon-16x16.png" sizes="16x16">
    <link rel="icon" type="image/png" href="assets/img/favicon-32x32.png" sizes="32x32">

    <title>אפטר יו - מערכת לידים</title>

    <link href='//fonts.googleapis.com/css?family=Roboto:300,400,500' rel='stylesheet' type='text/css'>

    <!-- uikit rtl -->
    <link rel="stylesheet" href="assets/css/uikit.rtl.css" media="all">

    <!-- flag icons -->
    <link rel="stylesheet" href="assets/icons/flags/flags.min.css" media="all">

    <!-- style switcher -->
    <link rel="stylesheet" href="assets/css/style_switcher.min.css" media="all">

    <!-- altair admin -->
    <link rel="stylesheet" href="assets/css/main.min.css" media="all">

    <!-- themes -->
    <link rel="stylesheet" href="assets/css/themes/themes_combined.min.css" media="all">



    <!-- altair admin login page -->
    <link rel="stylesheet" href="assets/css/login_page.min.css" />


    <%-- <script src="assets/js/common.min.js"></script>--%>
</head>

<body class="login_page">

    <div class="login_page_wrapper">
        <div class="md-card" id="login_card">
            <div class="md-card-content large-padding" id="login_form">
                <div class="login_heading">
                    <div class="">
                        <img src="assets/img/logoAfterU.png" /></div>
                </div>
                <form id="form_validation" class="uk-form-stacked">

                    <div class="parsley-row">
                        <label for="login_username">שם משתמש</label>
                        <input class="md-input" type="text" required id="txtUserName" data-parsley-required-message="שדה שם משתמש הינו חובה" name="login_username" />
                    </div>
                    <div class="parsley-row">
                        <label for="login_password">סיסמה</label>
                        <input class="md-input" type="password" required id="txtPassword" data-parsley-required-message="שדה סיסמה הינו חובה" name="login_username" />
                    </div>


                    <div class="uk-margin-medium-top">
                    
                        <a class="md-btn md-btn-primary md-btn-block md-btn-large " onclick="LogMeIn()">הכנס/י</a>
                       
                    </div>

                     <div class="uk-margin-medium-top">
                      
                        <div id="dvEnterAlert" class="uk-notify-message uk-notify-message-danger" style="">
                            <div>שם משתמש או סיסמה שגויים!</div>
                        </div>
                    </div>



                   <%-- <div class="uk-grid uk-grid-width-1-3 uk-grid-small uk-margin-top">
                        <div><a href="#" class="md-btn md-btn-block md-btn-facebook" data-uk-tooltip="{pos:'bottom'}" title="Sign in with Facebook"><i class="uk-icon-facebook uk-margin-remove"></i></a></div>
                        <div><a href="#" class="md-btn md-btn-block md-btn-twitter" data-uk-tooltip="{pos:'bottom'}" title="Sign in with Twitter"><i class="uk-icon-twitter uk-margin-remove"></i></a></div>
                        <div><a href="#" class="md-btn md-btn-block md-btn-gplus" data-uk-tooltip="{pos:'bottom'}" title="Sign in with Google+"><i class="uk-icon-google-plus uk-margin-remove"></i></a></div>
                    </div>--%>
                    <div class="uk-margin-top">
                        <a href="#" id="login_help_show" class="uk-float-right">Need help?</a>
                        <span class="icheck-inline">
                            <input type="checkbox" name="login_page_stay_signed" id="login_page_stay_signed" data-md-icheck />
                            <label for="login_page_stay_signed" class="inline-label">השאר אותי מחובר</label>
                        </span>
                    </div>
                </form>




            </div>
            <div class="md-card-content large-padding uk-position-relative" id="login_help" style="display: none">
                <button type="button" class="uk-position-top-right uk-close uk-margin-right uk-margin-top back_to_login"></button>
                <h2 class="heading_b uk-text-success">Can't log in?</h2>
                <p>Here’s the info to get you back in to your account as quickly as possible.</p>
                <p>First, try the easiest thing: if you remember your password but it isn’t working, make sure that Caps Lock is turned off, and that your username is spelled correctly, and then try again.</p>
                <p>If your password still isn’t working, it’s time to <a href="#" id="password_reset_show">reset your password</a>.</p>
            </div>
            <div class="md-card-content large-padding" id="login_password_reset" style="display: none">
                <button type="button" class="uk-position-top-right uk-close uk-margin-right uk-margin-top back_to_login"></button>
                <h2 class="heading_a uk-margin-large-bottom">Reset password</h2>
                <form>
                    <div class="uk-form-row">
                        <label for="login_email_reset">Your email address</label>
                        <input class="md-input" type="text" id="login_email_reset" name="login_email_reset" />
                    </div>
                    <div class="uk-margin-medium-top">
                        <a href="index.html" class="md-btn md-btn-primary md-btn-block">Reset password</a>
                    </div>
                </form>
            </div>
            <div class="md-card-content large-padding" id="register_form" style="display: none">
                <button type="button" class="uk-position-top-right uk-close uk-margin-right uk-margin-top back_to_login"></button>
                <h2 class="heading_a uk-margin-medium-bottom">Create an account</h2>
                <form>
                    <div class="uk-form-row">
                        <label for="register_username">Username</label>
                        <input class="md-input" type="text" id="register_username" name="register_username" />
                    </div>
                    <div class="uk-form-row">
                        <label for="register_password">Password</label>
                        <input class="md-input" type="password" id="register_password" name="register_password" />
                    </div>
                    <div class="uk-form-row">
                        <label for="register_password_repeat">Repeat Password</label>
                        <input class="md-input" type="password" id="register_password_repeat" name="register_password_repeat" />
                    </div>
                    <div class="uk-form-row">
                        <label for="register_email">E-mail</label>
                        <input class="md-input" type="text" id="register_email" name="register_email" />
                    </div>
                    <div class="uk-margin-medium-top">
                        <a href="index.html" class="md-btn md-btn-primary md-btn-block md-btn-large">Sign Up</a>
                    </div>
                </form>
            </div>
        </div>
        <div class="uk-margin-top uk-text-center">
            <a href="#" id="signup_form_show">Create an account</a>
        </div>
    </div>

    <!-- common functions -->
    <script src="assets/js/common.min.js"></script>
    <!-- uikit functions -->
    <script src="assets/js/uikit_custom.min.js"></script>
    <!-- altair core functions -->
    <script src="assets/js/altair_admin_common.js"></script>

    <!-- altair login page functions -->
    <script src="assets/js/pages/login.min.js"></script>

    <!-- parsley (validation) -->
    <script>
        // load parsley config (altair_admin_common.js)
        altair_forms.parsley_validation_config();
    </script>
    <script src="bower_components/parsleyjs/dist/parsley.min.js"></script>

    <!--  forms validation functions -->
    <script src="assets/js/pages/forms_validation.js"></script>

    <script type="text/javascript">

        var OrgUnitCode = "";

        $(document).ready(function () {

            $("#dvEnterAlert").hide();
            $("#dvsms").hide();

        });






        function LogMeIn() {
            $("#dvEnterAlert").hide();
            
            var $form = $('#form_validation');
            if (!$form.parsley().validate()) return;


            var Password = $("#txtPassword").val();
            var UserName = $("#txtUserName").val();
            var data = Ajax("User_GetUserEnter", "UserName=" + UserName + "&Password=" + Password);


            if (data[0]) {

                location.href = "Leeds/LeedsMain.aspx";
            }
            else {

                $("#dvEnterAlert").show();

            }

        }



        function Ajax(sp, params) {
            var json = "";
            $.ajax({
                type: "POST",
                url: "WebService.asmx/" + sp,

                data: params,
                async: false,
                dataType: "json",
                success: function (data) {

                    json = data;
                },

                error: function (request, status, error) {

                    json = (error);
                }

            });
            return json;

        }







    </script>


</body>

</html>
