<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMHoatDong.aspx.cs" Inherits="WebRunDragon.Forms.DMHoatDong" %>

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
                <table>
                    <tr>
                        <tr>
                            <td>Từ ngày</td>
                            <td>
                                <dx:ASPxDateEdit ID="deNgayDieuChinhTu" ClientInstanceName="deNgayDieuChinhTu" runat="server"></dx:ASPxDateEdit>
                            </td>

                            <td>
                                <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                            </td>
                        </tr>
                        <tr>
                            <td>Đến ngày</td>
                            <td>
                                <dx:ASPxDateEdit ID="deNgayDieuChinhDen" ClientInstanceName="deNgayDieuChinhDen" runat="server"></dx:ASPxDateEdit>
                            </td>

                            <td>
                                <dx:ASPxButton ID="btnSearchSum" ClientInstanceName="btnSearchSum" runat="server" Text="Tìm kiếm (Sum KM)" Style="margin-left: 0px" OnClick="btnSearchSum_Click" Theme="Office2003Blue" />
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
                                    <%-- <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("TenHoatDong") %>')" title="Xóa">
                                                   <img src="../Images/icon-delete.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>--%>

                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
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
                            <td>Mã hoạt động</td>
                            <td>
                                <dx:ASPxTextBox ID="txtMaHoatDong" ClientInstanceName="txtMaHoatDong" runat="server" Width="50%"></dx:ASPxTextBox>
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
                            <td colspan="3">Bạn có chắc chắn xóa nhóm chạy [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
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
            popUpdateForm.Show();
        }

        function clearForm(readEdit) {
            txtMaNhanVien.SetText("");
            txtTenNhanVien.SetText("");
            //  txtMaHoatDong.SetEnabled(false);
            txtQuanDuongHopLe.SetText("");
            txtMaHoatDong.SetText("");
            ckHopLe.SetValue(false);
            txtGhiChuThayDoi.SetText("");
            txtObjectId.Set('hidden_value', "0");
        }

        function saveHoatDong() {
            if (txtMaHoatDong.GetText() + "" == "") {
                alert("Chưa nhập Mã hoạt động");
                return;
            }
            if (txtGhiChuThayDoi.GetText() + "" == "") {
                alert("Chưa nhập ghi chú thay đổi");
                return;
            }

            var data = "mode=AddOrUpdate";
            data += "&id=" + txtObjectId.Get("hidden_value");
            data += "&tableName=ql_activities";
            data += "&QuangDuongHopLe=" + txtQuanDuongHopLe.GetValue();
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

        //function OpenDeleteForm(ID, fullName) {
        //    txtObjectId.Set('hidden_value', ID);
        //    $("#spTopicName").html(fullName);
        //    popupConfirmDelete.Show();
        //}

        //function DoDelete() {
        //    btnConfirmDelete.SetEnabled(false);
        //    $.ajax({
        //        type: "POST",
        //        async: true,
        //        url: "../Actions/QLDanhMucAction.ashx",
        //        dataType: "json",
        //        data: "mode=delete&tableName=dm_run_group&id=" + txtObjectId.Get('hidden_value'),
        //        complete: function () {
        //            btnConfirmDelete.SetEnabled(true);
        //            popupConfirmDelete.Hide();
        //        },
        //        timeout: 30000,
        //        success: function (data) {
        //            if (data.success) {
        //                gridHoatDong.PerformCallback();
        //            }
        //            else {

        //            }
        //            alert(data.message);
        //        }
        //    });
        //}
    </script>

</asp:Content>



