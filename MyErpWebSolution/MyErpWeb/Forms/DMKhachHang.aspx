<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMKhachHang.aspx.cs" Inherits="WebRunDragon.Forms.DMKhachHang" Culture="vi-VN" UICulture="vi-VN" %>
<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta charset="utf-8" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td style="width: 50px">
                            <div style="display: <%=GetRight(1)%>; float: right; margin-top: 10px; cursor: pointer; width: 45px;"
                                onclick="openAddForm()" title="Thêm mới">
                                <img src="../Images/icon-add.png" />&nbsp;
                            </div>
                        </td>
                        <td style="width: 81px">Từ khóa</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px" Theme="PlasticBlue"></dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm ki?m" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>

                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridKhachHang" ClientInstanceName="gridKhachHang" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnCustomCallback="gridKhachHang_CustomCallback"
                                OnPageIndexChanged="gridKhachHang_PageIndexChanged">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(0) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenKhachHang") %>','ReadOnly')" title="Xem thông tin">
                                                    <img src="../Images/icon-view.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(2) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenKhachHang") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                    <img src="../Images/icon-edit.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("TenKhachHang") %>')" title="Xóa">
                                                    <img src="../Images/icon-delete.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                    <dx:GridViewDataTextColumn Caption="Số điện thoại" FieldName="SoDienThoai" VisibleIndex="2">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên khách hàng" FieldName="TenKhachHang" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" VisibleIndex="4">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Địa chỉ" FieldName="DiaChi" VisibleIndex="5">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Phường xã" FieldName="PhuongXa" VisibleIndex="6">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tỉnh thành phố" FieldName="TinhThanhPho" VisibleIndex="7">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" VisibleIndex="8">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                </Columns>
                                <SettingsPager PageSize="15"></SettingsPager>
                                <SettingsBehavior AllowSort="True" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                            </dx:ASPxGridView>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="550px" Height="400px"
            ScrollBars="Auto" HeaderText="Thông tin khách hàng" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">

                        <tr>
                            <td>Số điện thoại</td>
                            <td>
                                <dx:ASPxTextBox ID="txtSoDienThoai" ClientInstanceName="txtSoDienThoai" runat="server" Width="50%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên khách hàng</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTenKhachHang" ClientInstanceName="txtTenKhachHang" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Email</td>
                            <td>
                                <dx:ASPxTextBox ID="txtEmail" ClientInstanceName="txtEmail" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Địa chỉ</td>
                            <td>
                                <dx:ASPxTextBox ID="txtDiaChi" ClientInstanceName="txtDiaChi" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Phường xã</td>
                            <td>
                                <dx:ASPxTextBox ID="txtPhuongXa" ClientInstanceName="txtPhuongXa" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Tỉnh thành phố</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTinhThanhPho" ClientInstanceName="txtTinhThanhPho" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Ghi chú</td>
                            <td>
                                <dx:ASPxMemo ID="txtGhiChu" ClientInstanceName="txtGhiChu" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="L?u">
                                                <ClientSideEvents Click="function(s,e){saveKhachHang();}" />
                                            </dx:ASPxButton>

                                        </td>
                                        <td style="width: 100px"></td>
                                        <td>
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelEdit" runat="server" AutoPostBack="False" Text="Thoát">
                                                <ClientSideEvents Click="function(s,e){popUpdateForm.Hide();}" />
                                            </dx:ASPxButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>

                    </table>

                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>


    <div>
        <dx:ASPxPopupControl ID="popupConfirmDelete" ClientInstanceName="popupConfirmDelete" runat="server" Height="100px" Width="493px"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">

            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3">B?n có ch?c ch?n xóa khách hàng [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <dx:ASPxButton ID="btnConfirmDelete" ClientInstanceName="btnConfirmDelete" runat="server" Text="Có" Theme="Office2003Blue">
                                    <ClientSideEvents Click="function(s, e) {DoDelete(); }" />
                                </dx:ASPxButton>
                            </td>
                            <td></td>
                            <td>
                                <dx:ASPxButton ID="btnCancelDelete" runat="server" Text="Không" Theme="Office2003Blue">
                                    <ClientSideEvents Click="function(s, e) {	popupConfirmDelete.Hide();}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>

        </dx:ASPxPopupControl>

    </div>



    <div>
        <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>
    </div>

    <script>

        function openAddForm() {
            clearForm("");
            popUpdateForm.Show();
        }

        function clearForm(readEdit) {
            txtSoDienThoai.SetText("");
            txtSoDienThoai.SetEnabled(true);
            txtTenKhachHang.SetText("");
            txtEmail.SetText("");
            txtDiaChi.SetText("");
            txtPhuongXa.SetText("");
            txtTinhThanhPho.SetText("");
            txtGhiChu.SetText("");
            txtObjectId.Set('hidden_value', "0");
            btnSaveEdit.SetEnabled(false);
            if (readEdit == "ReadOnly") {

            }
            else if (readEdit == "NewOnly") {
                // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval

                btnSaveEdit.SetEnabled(true);

            }
            else if (readEdit == "EditOnly") {
                // phụ thuộc vào Trạng thái và Quyền
                //if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
                btnSaveEdit.SetEnabled(true);

                //}
                //else { // các trạng thái còn lại ko enable

                //}
            }
            else if (readEdit == "ApprovalOnly") {
                //if (objStatus == "SENDAPPROVAL") {
                //    btnDuyet.SetEnabled(true);
                //    btnKhongDuyet.SetEnabled(true);
                //}
            }
        }

        function saveKhachHang() {

            if (txtSoDienThoai.GetText() + "" == "") {
                alert("Ch?a nh?p S? ?i?n tho?i");
                return;
            }
            if (txtTenKhachHang.GetText() + "" == "") {
                alert("Ch?a nh?p Tên khách hàng");
                return;
            }

            var data = "mode=AddOrUpdate";
            data += "&id=" + txtObjectId.Get("hidden_value");
            data += "&tableName=DM_KHACH_HANG";

            data += "&SoDienThoai=" + txtSoDienThoai.GetText();
            data += "&TenKhachHang=" + txtTenKhachHang.GetText().replace("&", "{sang_va}");
            data += "&Email=" + txtEmail.GetText();
            data += "&DiaChi=" + txtDiaChi.GetText();
            data += "&PhuongXa=" + txtPhuongXa.GetText();
            data += "&TinhThanhPho=" + txtTinhThanhPho.GetText();
            data += "&GhiChu=" + txtGhiChu.GetText();

            btnSaveEdit.SetEnabled(false);

            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLDanhMucAction.ashx",
                dataType: "json",
                data: data,
                complete: function (xmlHttpRequest, status) {
                    btnSaveEdit.SetEnabled(true);
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        txtSoDienThoai.SetEnabled(false);
                        txtObjectId.Set('hidden_value', data.id);
                        gridKhachHang.PerformCallback();
                    }
                    else {

                    }
                    alert(data.message);
                }
            });
        }

        function openEditForm(ID, fullName, readEditApproval) {

            var data = "mode=EDIT";
            data += "&id=" + ID;
            data += "&subMode=" + readEditApproval;
            data += "&tableName=DM_KHACH_HANG";


            clearForm(readEditApproval);

            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLDanhMucAction.ashx",
                dataType: "json",
                data: data,
                complete: function () { },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        txtObjectId.Set('hidden_value', ID);

                        txtSoDienThoai.SetText(data.entity.SoDienThoai);
                        txtTenKhachHang.SetText(data.entity.TenKhachHang);
                        txtEmail.SetText(data.entity.Email);
                        txtDiaChi.SetText(data.entity.DiaChi);
                        txtPhuongXa.SetText(data.entity.PhuongXa);
                        txtTinhThanhPho.SetText(data.entity.TinhThanhPho);
                        txtGhiChu.SetText(data.entity.GhiChu);
                        popUpdateForm.Show();
                    }
                }
            });
        }

        function OpenDeleteForm(ID, fullName) {
            txtObjectId.Set('hidden_value', ID);
            $("#spTopicName").html(fullName);
            popupConfirmDelete.Show();
        }

        function DoDelete() {
            btnConfirmDelete.SetEnabled(false);
            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLDanhMucAction.ashx",
                dataType: "json",
                data: "mode=delete&tableName=DM_KHACH_HANG&id=" + txtObjectId.Get('hidden_value'),
                complete: function () {
                    btnConfirmDelete.SetEnabled(true);
                    popupConfirmDelete.Hide();
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        gridKhachHang.PerformCallback();
                    }
                    else {

                    }
                    alert(data.message);
                }
            });
        }
    </script>

</asp:Content>
