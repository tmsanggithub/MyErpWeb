<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMVanDongVien.aspx.cs" Inherits="WebRunDragon.Forms.DMVanDongVien" %>

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
                            <dx:ASPxGridView ID="gridVanDongVien" ClientInstanceName="gridVanDongVien" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnCustomCallback="gridVanDongVien_CustomCallback"
                                OnPageIndexChanged="gridVanDongVien_PageIndexChanged"
                                OnDataBinding="gridVanDongVien_DataBinding">

                                <Columns>

                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(0)%>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("full_name") %>','ReadOnly')" title="Xem thông tin">
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
                                    <%--
                                   <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("TenVanDongVien") %>')" title="Xóa">
                                                   <img src="../Images/icon-delete.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>--%>

                                    <dx:GridViewDataTextColumn Caption="ID NV" FieldName="ID" VisibleIndex="0" Width="50px" />
                                    <dx:GridViewDataTextColumn Caption="Giai chay" FieldName="race_name_list" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhom chay" FieldName="group_name_list" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã Strava" FieldName="id_strava" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Mã NV/Tài khoản CK" FieldName="staff_no_vdsc" VisibleIndex="2" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Loại User" FieldName="user_type" VisibleIndex="2" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Tên đầy đủ" FieldName="full_name" VisibleIndex="2" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Giới tính" FieldName="gender" VisibleIndex="2" Width="50px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Email" FieldName="email" VisibleIndex="3" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Điện thoại" FieldName="mobile_phone" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên hiển thị" FieldName="display_name" VisibleIndex="2" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tôi lái xe" FieldName="is_lai_xe" VisibleIndex="5" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="last time activities" FieldName="last_time_activities" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="strava_token" FieldName="strava_token" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="strava_token_refresh" FieldName="strava_token_refresh" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="strava_expire_at" FieldName="strava_expire_at" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Need authen" FieldName="need_authen" VisibleIndex="5" Width="50px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chu tinh activities" FieldName="get_activities_description" VisibleIndex="5" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="get_activities_lasted" FieldName="get_activities_lasted" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="created_time" FieldName="created_time" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="register_time" FieldName="register_time" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="register_approved_time" FieldName="register_approved_time" VisibleIndex="5" Width="150px">
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
        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridVanDongVien">
        </dx:ASPxGridViewExporter>

        <dx:ASPxButton ID="btnExcel" ClientInstanceName="btnExcel" runat="server" Text="Excel" Style="margin-left: 0px" OnClick="btnExcel_Click" Theme="Office2003Blue" />
    </div>

    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="550px" Height="350px"
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
                            <td>Loại user</td>
                            <td colspan="4">
                                <dx:ASPxRadioButtonList ID="radUserType" ClientInstanceName="radUserType" runat="server" RepeatDirection="Horizontal" ValueType="System.String" Border-BorderStyle="None">
                                    <Items>
                                        <dx:ListEditItem Text="Nhân viên" Value="staff" Selected="true" />
                                        <dx:ListEditItem Text="Khách hàng" Value="customer" />
                                    </Items>
                                </dx:ASPxRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>Mã nhân viên</td>
                            <td>
                                <dx:ASPxTextBox ID="txtMaVanDongVien" ClientInstanceName="txtMaVanDongVien" runat="server" Width="50%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên hiển thị</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTenHienThi" ClientInstanceName="txtTenHienThi" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên đầy đủ</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTenDayDu" ClientInstanceName="txtTenDayDu" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Giới tính</td>
                            <td colspan="4">
                                <dx:ASPxRadioButtonList ID="radNamNu" ClientInstanceName="radNamNu" runat="server" RepeatDirection="Horizontal" ValueType="System.String" Border-BorderStyle="None">
                                    <Items>
                                        <dx:ListEditItem Text="Nam" Value="Nam" Selected="true" />
                                        <dx:ListEditItem Text="Nữ" Value="Nu" />
                                    </Items>
                                </dx:ASPxRadioButtonList>
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
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                <ClientSideEvents Click="function(s,e){saveVanDongVien();}" />
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
    <%--
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
    --%>
    <div>
        <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>
    </div>

    <script>

        function openAddForm() {
            clearForm("");
            popUpdateForm.Show();
        }

        function clearForm(readEdit) {
            txtMaVanDongVien.SetText("");
            txtTenHienThi.SetText("");
            txtTenDayDu.SetText("");
            txtGhiChu.SetText("");
            txtObjectId.Set('hidden_value', "0");
        }


        function saveVanDongVien() {

            if (txtMaVanDongVien.GetText() + "" == "") {
                alert("Chưa nhập Mã nhân viên");
                return;
            }
            if (txtTenHienThi.GetText() + "" == "") {
                alert("Chưa nhập Tên hiển thị");
                return;
            }
            if (txtTenDayDu.GetText() + "" == "") {
                alert("Chưa nhập Tên đầy đủ");
                return;
            }
            var data = "mode=AddOrUpdate";
            data += "&id=" + txtObjectId.Get("hidden_value");
            data += "&tableName=app_user_registed";
            data += "&MaVanDongVien=" + txtMaVanDongVien.GetText();
            data += "&TenHienThi=" + txtTenHienThi.GetText();
            data += "&TenDayDu=" + txtTenDayDu.GetText();
            data += "&GioiTinh=" + radNamNu.GetValue();
            data += "&UserType=" + radUserType.GetValue();
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
                        ;

                        txtObjectId.Set('hidden_value', data.id);
                        gridVanDongVien.PerformCallback();
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
            data += "&tableName=app_user_registed";


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

                        txtMaVanDongVien.SetText(data.entity.staff_no_vdsc);
                        txtTenHienThi.SetText(data.entity.display_name);
                        txtTenDayDu.SetText(data.entity.full_name);
                        radNamNu.SetValue(data.entity.gender);
                        radUserType.SetValue(data.entity.user_type);
                        txtGhiChu.SetText(data.entity.admin_description);
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
                data: "mode=delete&tableName=dm_run_group&id=" + txtObjectId.Get('hidden_value'),
                complete: function () {
                    btnConfirmDelete.SetEnabled(true);
                    popupConfirmDelete.Hide();
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        gridVanDongVien.PerformCallback();
                    }
                    else {

                    }
                    alert(data.message);
                }
            });
        }
    </script>

</asp:Content>
