<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMHoatDong.aspx.cs" Inherits="WebRunDragon.Forms.DMHoatDong" ResponseEncoding="utf-8" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .waiting-cursor {
            cursor: wait !important;
        }
    </style>
    <script type="text/javascript">
        function pageLoad() {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            // Khi bắt đầu PostBack: hiện waiting cursor, disable nút
            prm.add_beginRequest(function () {
                document.body.style.cursor = 'wait';
                btnSearch.SetEnabled(false);
            });
            // Khi PostBack xong: khôi phục
            prm.add_endRequest(function () {
                document.body.style.cursor = 'default';
                btnSearch.SetEnabled(true);
            });
        }
    </script>
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <ContentTemplate>
                <table>
                    <tr>
                        <tr>
                            <td style="width: 50px">
                                <div style="display: <%=GetRight(1)%>; float: right; margin-top: 10px; cursor: pointer; width: 45px;"
                                    onclick="openAddForm()" title="Thêm mới">
                                    <img src="../Images/icon-add.png" />&nbsp;
                                </div>
                            </td>
                            <td>Gải chạy</td>
                            <td>
                                <dx:ASPxComboBox ID="cbGiaiChay" ClientInstanceName="cbGiaiChay" runat="server" ValueField="id" TextFormatString="{1}" Style="width: 100%">
                                    <%--<ClientSideEvents ValueChanged="function(s, e) { cbGiaiChayValueChanged(); }" />--%>
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="false" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>

                            <td>
                                <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                            </td>
                        </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridHoatDong" ClientInstanceName="gridHoatDong" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnCustomCallback="gridHoatDong_CustomCallback"
                                OnPageIndexChanged="gridHoatDong_PageIndexChanged"
                                OnDataBinding="gridHoatDong_DataBinding">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(0) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("full_name") %>','ReadOnly')" title="Xem thông tin">
                                                    <img src="../Images/icon-view.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(2) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("full_name") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                    <img src="../Images/icon-edit.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("full_name") %>','<%#Eval("created_by") %>')" title="Xóa">
                                                    <img src="../Images/icon-delete.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                    <dx:GridViewDataTextColumn Caption="NV/KH" FieldName="user_type" VisibleIndex="1" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Đội" FieldName="group_name" VisibleIndex="1" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="ID hoạt động" FieldName="id_activities" VisibleIndex="1" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên hoạt động" FieldName="name" VisibleIndex="1" Width="120px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã Nhân viên" FieldName="staff_no_vdsc" VisibleIndex="1" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên nhân viên" FieldName="full_name" VisibleIndex="1" Width="180px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID Strava" FieldName="id_strava" VisibleIndex="1" Width="80px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Bắt đầu" FieldName="start_date_str" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <%--  <dx:GridViewDataTextColumn Caption="Time Zone" FieldName="time_zone" VisibleIndex="2" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Thời gian chạy" FieldName="total_moving_time" VisibleIndex="2" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>--%>
                                    <dx:GridViewDataTextColumn Caption="Thời gian chạy" FieldName="moving_time_str" VisibleIndex="2" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Quãng đường(m)" FieldName="total_distance_met" VisibleIndex="2" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Vận tốc trung bình" FieldName="average_speed_second_on_km" VisibleIndex="3" Width="120px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Q.đường hop le" FieldName="total_distance_met_valid" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Hợp lệ" FieldName="valid" VisibleIndex="3" Width="50px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Vi phạm tuần" FieldName="invalid_week" VisibleIndex="3" Width="90px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ly do" FieldName="reason" VisibleIndex="3" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Hien thi" FieldName="visibility" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="status" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Ghi chú thay đổi" FieldName="description_change" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nguon (thiet bi)" FieldName="external_id" VisibleIndex="3" Width="400px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="time_zone" FieldName="time_zone" VisibleIndex="3" Width="200px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="total_elapsed_time" FieldName="total_elapsed_time" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="end_time" FieldName="end_time" VisibleIndex="3" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="splits_pace" FieldName="splits_pace" VisibleIndex="3" Width="450px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager PageSize="25"></SettingsPager>
                                <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <Settings ShowHorizontalScrollBar="True" />
                            </dx:ASPxGridView>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="750px" Height="650px"
            ScrollBars="Auto" HeaderText="Thông tin loại chạy" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">

                        <tr>
                            <td>Giải chạy</td>
                            <td>
                                <dx:ASPxComboBox ID="cbGiaiChayPopup" ClientInstanceName="cbGiaiChayPopup" runat="server" ValueField="id" TextFormatString="{1}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="false" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Người dùng</td>
                            <td>
                                <dx:ASPxComboBox ID="cbNguoiDungDangKy" ClientInstanceName="cbNguoiDungDangKy" runat="server" ValueField="id_runner" TextField="name" Style="width: 100%">
                                    <ClientSideEvents ValueChanged="function(s, e) { updateThongTinNguoiDung(); }" />
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID Runner" FieldName="id_runner" Name="id_runner" />
                                        <dx:ListBoxColumn Caption="ID Strava" FieldName="id_strava" Name="id_strava" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>

                        <tr>
                            <td>ID Strava</td>
                            <td>
                                <dx:ASPxTextBox ID="txtIdStrava" ClientInstanceName="txtIdStrava" runat="server" Width="100%" ClientEnabled="false"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mã nhân viên</td>
                            <td>
                                <dx:ASPxTextBox ID="txtMaNhanVien" ClientInstanceName="txtMaNhanVien" runat="server" Width="50%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên nhân viên</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTenNhanVien" ClientInstanceName="txtTenNhanVien" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>


                        <tr>
                            <td>Ngày</td>
                            <td>
                                <dx:ASPxDateEdit ID="deNgayHoatDong" ClientInstanceName="deNgayHoatDong" runat="server" Width="100%">
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>ID hoạt động</td>
                            <td>
                                <dx:ASPxTextBox ID="txtIdActivities" ClientInstanceName="txtIdActivities" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Mã hoạt động</td>
                            <td>
                                <dx:ASPxTextBox ID="txtMaHoatDong" ClientInstanceName="txtMaHoatDong" runat="server" Width="50%"></dx:ASPxTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Tổng quãng đường (m)</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTongQuangDuong" ClientInstanceName="txtTongQuangDuong" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Hợp lệ</td>
                            <td>
                                <dx:ASPxCheckBox ID="ckHopLe" ClientInstanceName="ckHopLe" runat="server" Width="50%"></dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Quãng đường hợp lệ (m)</td>
                            <td>
                                <dx:ASPxTextBox ID="txtQuanDuongHopLe" ClientInstanceName="txtQuanDuongHopLe" runat="server" Width="50%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Ghi chú thay đổi</td>
                            <td>
                                <dx:ASPxMemo ID="txtGhiChuThayDoi" ClientInstanceName="txtGhiChuThayDoi" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                            </td>
                        </tr>


                        <tr>
                            <td>Created By</td>
                            <td>
                                <dx:ASPxTextBox ID="txtCreatedBy" ClientInstanceName="txtCreatedBy" runat="server" Width="100%" ClientEnabled="false"></dx:ASPxTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>Created Time</td>
                            <td>
                                <dx:ASPxTextBox ID="txtCreatedTime" ClientInstanceName="txtCreatedTime" runat="server" Width="100%" ClientEnabled="false"></dx:ASPxTextBox>
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
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                <ClientSideEvents Click="function(s,e){saveHoatDong();}" />
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
                            <td colspan="3">Bạn có chắc chắn xóa hoạt động [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
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
        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridHoatDong">
        </dx:ASPxGridViewExporter>

        <dx:ASPxButton ID="btnExcel" ClientInstanceName="btnExcel" runat="server" Text="Excel" Style="margin-left: 0px" OnClick="btnExcel_Click" Theme="Office2003Blue" />
    </div>
    <div>
        <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>
    </div>

    <script>

        function openAddForm() {
            clearForm("");
            setControlMode("");
            popUpdateForm.Show();
        }

        function setControlMode(readEditApproval) {
            var mode = (readEditApproval || "").toUpperCase();
            var isEditOnly = mode === "EDITONLY";
            var isReadOnly = mode === "READONLY";
            var isAddNew = !isEditOnly && !isReadOnly;

            cbGiaiChayPopup.SetEnabled(isAddNew);
            cbNguoiDungDangKy.SetEnabled(isAddNew);
            deNgayHoatDong.SetEnabled(isAddNew);
            txtTongQuangDuong.SetEnabled(isAddNew);
            txtIdActivities.SetEnabled(isAddNew);
            txtMaNhanVien.SetEnabled(isAddNew);
            txtTenNhanVien.SetEnabled(isAddNew);
            txtMaHoatDong.SetEnabled(isAddNew);

            txtQuanDuongHopLe.SetEnabled(isAddNew || isEditOnly);
            ckHopLe.SetEnabled(isAddNew || isEditOnly);
            txtGhiChuThayDoi.SetEnabled(isAddNew || isEditOnly);

            btnSaveEdit.SetVisible(!isReadOnly);
        }

        function clearForm(readEdit) {
            cbGiaiChayPopup.SetValue(cbGiaiChay.GetValue());
            cbNguoiDungDangKy.SetValue(null);
            txtIdStrava.SetText("");
            deNgayHoatDong.SetValue(null);
            txtTongQuangDuong.SetText("");
            txtIdActivities.SetText("");
            txtCreatedBy.SetText("admin_import");
            txtCreatedTime.SetText((new Date()).toLocaleString());
            txtMaNhanVien.SetText("");
            txtTenNhanVien.SetText("");
            //  txtMaHoatDong.SetEnabled(false);
            txtQuanDuongHopLe.SetText("");
            txtMaHoatDong.SetText("");
            ckHopLe.SetValue(false);
            txtGhiChuThayDoi.SetText("");

            txtObjectId.Set('hidden_value', "0");
        }

        function updateThongTinNguoiDung() {
            var item = cbNguoiDungDangKy.GetSelectedItem();
            if (!item) {
                txtIdStrava.SetText("");
                txtMaNhanVien.SetText("");
                txtTenNhanVien.SetText("");
                return;
            }

            txtIdStrava.SetText(item.GetColumnText(1));
            txtMaNhanVien.SetText(item.GetColumnText(2));
            txtTenNhanVien.SetText(item.GetColumnText(3));
        }

        function saveHoatDong() {
            if (cbGiaiChayPopup.GetValue() == null || cbGiaiChayPopup.GetValue() == "") {
                alert("Chưa chọn giải chạy");
                return;
            }
            if (cbNguoiDungDangKy.GetValue() == null || cbNguoiDungDangKy.GetValue() == "") {
                alert("Chưa chọn người dùng");
                return;
            }
            if (deNgayHoatDong.GetValue() == null) {
                alert("Chưa nhập ngày hoạt động");
                return;
            }
            if (txtIdActivities.GetText() + "" == "") {
                alert("Chưa nhập ID hoạt động");
                return;
            }
            if (txtGhiChuThayDoi.GetText() + "" == "") {
                alert("Chưa nhập ghi chú thay đổi");
                return;
            }

            var data = "mode=AddOrUpdate";
            data += "&id=" + txtObjectId.Get("hidden_value");
            data += "&tableName=ql_activities";
            data += "&IDRace=" + cbGiaiChayPopup.GetValue();
            data += "&IDRunner=" + cbNguoiDungDangKy.GetValue();
            data += "&IDStrava=" + txtIdStrava.GetText();
            data += "&NgayHoatDong=" + deNgayHoatDong.GetText();
            data += "&TongQuangDuong=" + txtTongQuangDuong.GetValue();
            data += "&QuangDuongHopLe=" + txtQuanDuongHopLe.GetValue();
            data += "&IDActivities=" + txtIdActivities.GetText();
            data += "&HopLe=" + ckHopLe.GetValue();
            data += "&GhiChuThayDoi=" + txtGhiChuThayDoi.GetText();

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
                        ;

                        txtObjectId.Set('hidden_value', data.id);
                        gridHoatDong.PerformCallback();
                        popUpdateForm.Hide();
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
            data += "&tableName=ql_activities";


            clearForm(readEditApproval);
            setControlMode(readEditApproval);



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
                        cbGiaiChayPopup.SetValue(data.entity.id_race);
                        cbNguoiDungDangKy.SetValue(data.entity.id_runner);
                        txtIdStrava.SetText(data.entity.id_strava);
                        deNgayHoatDong.SetText(data.entity.date);
                        txtTongQuangDuong.SetText(data.entity.total_distance_met);
                        txtIdActivities.SetText(data.entity.id_activities);
                        txtCreatedBy.SetText(data.entity.created_by);
                        txtCreatedTime.SetText(data.entity.created_time);
                        txtMaNhanVien.SetText(data.entity.staff_no_vdsc);
                        txtTenNhanVien.SetText(data.entity.full_name);
                        txtMaHoatDong.SetText(data.entity.id_activities);
                        ckHopLe.SetValue(data.entity.valid == 1);
                        txtGhiChuThayDoi.SetText(data.entity.description_change);
                        txtQuanDuongHopLe.SetText(data.entity.total_distance_met_valid);
                        popUpdateForm.Show();
                    }
                }
            });
        }

        function OpenDeleteForm(ID, fullName, createdBy) {
            if ((createdBy + '').toLowerCase() != 'admin_import') {
                alert('Chỉ cho phép xóa hoạt động có created_by = admin_import');
                return;
            }
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
                data: "mode=delete&tableName=DMHoatDong&id=" + txtObjectId.Get('hidden_value'),
                complete: function () {
                    btnConfirmDelete.SetEnabled(true);
                    popupConfirmDelete.Hide();
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        gridHoatDong.PerformCallback();
                    }
                    alert(data.message);
                }
            });
        }
    </script>

</asp:Content>



