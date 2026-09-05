<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InBaoCao.aspx.cs" Inherits="WebRunDragon.Forms.InBaoCao" %>

<%@ Register Assembly="DevExpress.XtraReports.v15.1.Web, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.XtraReports.Web" TagPrefix="dx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <table style="width: 100%; height: 100%">
            <tr>
                <td>
                    <dx:aspxdocumentviewer id="ASPxDocumentViewer1" runat="server" width="100%"></dx:aspxdocumentviewer>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
