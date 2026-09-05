<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HuongDanSuDung.aspx.cs" Inherits="WebRunDragon.Forms.HuongDanSuDung" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
  <form id="HelpDeskGuide" runat="server">
        <div>

            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkView" runat="server" Text="Hướng dẫn sử dụng Quản lý tài sản" ></asp:LinkButton>
                        <hr />
                        <asp:Literal ID="ltEmbed" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
