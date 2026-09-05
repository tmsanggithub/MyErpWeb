<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CfgLogin.aspx.cs" Inherits="WebRunDragon.Forms.CfgLogin" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>ĐĂNG NHẬP QUẢN LÝ TÀI SẢN</title>
    <link rel="stylesheet" href="../Css/style_login.css" />
    <script src="../Scripts/jquery-1.11.1.min.js" type="text/javascript"></script>
</head>

<body>

    <form id="frmLogin" runat="server">
        <header>ĐĂNG NHẬP QUẢN LÝ TÀI SẢN</header>
        <label>Tên đăng nhập <span class="sp1">*</span></label>
        <input id="txtUserId" name="userId" />
        <div class="help">Sử dụng tài khoản đăng nhập máy tính</div>
        <label>Mật khẩu <span>*</span></label>
        <input type="password" id="txtUserPwd" name="userPwd" />
        <div class="help">Lưu ý bộ gõ tiếng Việt, phân biệt chữ HOA/thường</div>

        <button type="button" id="btnLogin" onclick="doAuthen();return;">Đăng nhập</button>
        <input type="hidden" name="mode" value="login" />
    </form>

    <script>

        var flag4Submit = true;

        $(document).ready(function () {
            $.ajaxSetup({
                cache: false
            });

            $('#txtUserId').keydown(function (event) {
                if (event.which == 13) {
                    $('#txtUserPwd').focus();
                }
            });

            $('#txtUserPwd').keydown(function (event) {
                if (event.which == 13) {
                    doAuthen();
                }
            });

            //$("#btnLogin").click(function () { doAuthen(); });
        })

        function doAuthen() {            
            
            if (!flag4Submit) {
                return;
            }

            flag4Submit = false;

            var userId = $.trim($("#txtUserId").val());
            var userPwd = $("#txtUserPwd").val();

            var message = "";
            if (userId == "")
                message += "Bạn vui lòng cho biết tên đăng nhập. \n";

            if (userPwd == "")
                message += "Bạn vui lòng cho biết mật khẩu đăng nhập. \n";

            if (message == "") {
                $.ajax({
                    type: "POST",
                    async: true,
                    url: "../Actions/AuthenAction.ashx",
                    dataType: "json",
                    data: "mode=login&userId=" + userId + "&userPwd=" + userPwd,
                    data: $("#frmLogin").serialize(),
                    complete: function () { },
                    timeout: 30000,
                    success: function (data) {
                        if (data.success) {
                            var lastUrl = data.lastUrl;
                            lastUrl = lastUrl.substring(lastUrl.lastIndexOf("/") + 1);
                            location.href = lastUrl;
                        }
                        else
                            alert(data.message);
                    }
                });
            }
            else
                alert(message);

            flag4Submit = true;
            
        }


    </script>
</body>

</html>
