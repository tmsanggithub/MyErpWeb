<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMTuan.aspx.cs" Inherits="WebRunDragon.Forms.DMTuan" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                        <td style="width: 81px">Từ khóa</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px" Theme="PlasticBlue"></dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>

                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridTuan" ClientInstanceName="gridTuan" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnCustomCallback="gridTuan_CustomCallback"
                                OnPageIndexChanged="gridTuan_PageIndexChanged"
                                OnDataBinding="gridTuan_DataBinding">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(0)%>; cursor: pointer" onclick="openEditForm('<%#Eval("id") %>','<%#Eval("tuan") %>','ReadOnly')" title="Xem thông tin">
                                                    <img src="../Images/icon-view.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(2) %>; cursor: pointer" onclick="openEditForm('<%#Eval("id") %>','<%#Eval("tuan") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                    <img src="../Images/icon-edit.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="OpenDeleteForm('<%#Eval("id") %>','<%#Eval("tuan") %>')" title="Xóa">
                                                    <img src="../Images/icon-delete.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="0" Visible="false" />
                                    <dx:GridViewDataTextColumn Caption="Giải chạy" FieldName="race_name" VisibleIndex="2" Width="200px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tuần" FieldName="tuan" VisibleIndex="1" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Từ ngày" FieldName="from_date" VisibleIndex="3" Width="120px" />
                                    <dx:GridViewDataTextColumn Caption="Đến ngày" FieldName="to_date" VisibleIndex="4" Width="120px" />
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="ghi_chu" VisibleIndex="5" Width="300px">
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
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="550px" Height="300px"
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
                            <td style="width: 80px">Giải chạy</td>
                            <td colspan="3">
                                <dx:ASPxComboBox ID="cbNhanVien" ClientInstanceName="cbNhanVien" runat="server" ValueField="id" TextFormatString="{1}" Style="width: 100%">
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
                            <td style="width: 80px">Tuần</td>
                            <td style="width: 250px">
                                <dx:ASPxTextBox ID="txtTuan" ClientInstanceName="txtTuan" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 80px">Từ ngày</td>
                            <td style="width: 250px">
                                <dx:ASPxDateEdit ID="deNgayNghiTu" ClientInstanceName="deNgayNghiTu" runat="server" Width="100%" EditFormat="Date" EditFormatString="dd/MM/yyyy" DisplayFormatString="dd/MM/yyyy">
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 80px">Đến ngày</td>
                            <td style="width: 250px">
                                <dx:ASPxDateEdit ID="deNgayNghiDen" ClientInstanceName="deNgayNghiDen" runat="server" Width="100%" EditFormat="Date" EditFormatString="dd/MM/yyyy" DisplayFormatString="dd/MM/yyyy">
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxDateEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>Ghi chú</td>
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
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                <ClientSideEvents Click="function(s,e){saveTuan();}" />
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
                            <td colspan="3">Bạn có chắc chắn xóa tuần [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
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
            cbNhanVien.SetValue(null);
            cbNhanVien.SetEnabled(readEdit != 'ReadOnly');
            txtTuan.SetText("");
            txtTuan.SetEnabled(readEdit != 'ReadOnly');
            deNgayNghiTu.SetValue(null);
            deNgayNghiTu.SetEnabled(readEdit != 'ReadOnly');
            deNgayNghiDen.SetValue(null);
            deNgayNghiDen.SetEnabled(readEdit != 'ReadOnly');
            txtGhiChu.SetText("");
            txtGhiChu.SetEnabled(readEdit != 'ReadOnly');
            txtObjectId.Set('hidden_value', "0");
            btnSaveEdit.SetVisible((readEdit || '').toUpperCase() != 'READONLY');
        }

        function saveTuan() {
            if (cbNhanVien.GetValue() == null || cbNhanVien.GetValue() == "") {
                alert("Chưa chọn giải chạy");
                return;
            }
            if (txtTuan.GetText() + "" == "") {
                alert("Chưa nhập tuần");
                return;
            }
            if (deNgayNghiTu.GetValue() == null) {
                alert("Chưa nhập từ ngày");
                return;
            }
            if (deNgayNghiDen.GetValue() == null) {
                alert("Chưa nhập đến ngày");
                return;
            }

            var fromDate = deNgayNghiTu.GetDate();
            var toDate = deNgayNghiDen.GetDate();
            if (fromDate != null && toDate != null && fromDate > toDate) {
                alert("Từ ngày không được lớn hơn đến ngày");
                return;
            }

            var data = "mode=AddOrUpdate";
            data += "&id=" + txtObjectId.Get("hidden_value");
            data += "&id_race=" + cbNhanVien.GetValue();
            data += "&tuan=" + txtTuan.GetText();
            data += "&from_date=" + deNgayNghiTu.GetDate().toJSON();
            data += "&to_date=" + deNgayNghiDen.GetDate().toJSON();
            data += "&GhiChu=" + txtGhiChu.GetText();

            btnSaveEdit.SetEnabled(false);

            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLTuanAction.ashx",
                dataType: "json",
                data: data,
                complete: function () {
                    btnSaveEdit.SetEnabled(true);
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        txtObjectId.Set('hidden_value', data.id);
                        gridTuan.PerformCallback();
                        popUpdateForm.Hide();
                    }
                    alert(data.message);
                }
            });
        }

        function openEditForm(ID, fullName, readEditApproval) {
            var data = "mode=EDIT";
            data += "&id=" + ID;
            data += "&subMode=" + readEditApproval;

            clearForm(readEditApproval);

            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLTuanAction.ashx",
                dataType: "json",
                data: data,
                complete: function () { },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        txtObjectId.Set('hidden_value', ID);
                        cbNhanVien.SetValue(data.entity.id_race);
                        txtTuan.SetText(data.entity.tuan);
                        deNgayNghiTu.SetText(data.entity.from_date);
                        deNgayNghiDen.SetText(data.entity.to_date);
                        txtGhiChu.SetText(data.entity.ghi_chu);
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
                url: "../Actions/QLTuanAction.ashx",
                dataType: "json",
                data: "mode=DELETE&id=" + txtObjectId.Get('hidden_value'),
                complete: function () {
                    btnConfirmDelete.SetEnabled(true);
                    popupConfirmDelete.Hide();
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        gridTuan.PerformCallback();
                    }
                    alert(data.message);
                }
            });
        }
    </script>

</asp:Content>

