<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMNhaCungCap.aspx.cs" Inherits="WebRunDragon.Forms.DMNhaCungCap" %>



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
                            <dx:ASPxGridView ID="gridNhaCungCap" ClientInstanceName="gridNhaCungCap" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True"
                                Theme="PlasticBlue"
                                DataSourceID="MyDataSource"
                                OnCustomCallback="gridNhaCungCap_CustomCallback"
                                OnPageIndexChanged="gridNhaCungCap_PageIndexChanged">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(0) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenNhaCungCap") %>','ReadOnly')" title="Xem thông tin">
                                                 <img src="../Images/icon-view.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(2) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenNhaCungCap") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                   <img src="../Images/icon-edit.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("TenNhaCungCap") %>')" title="Xóa">
                                                   <img src="../Images/icon-delete.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                    <dx:GridViewDataTextColumn Caption="Mã Nhà cung cấp" FieldName="MaNhaCungCap" Width="100px" VisibleIndex="2">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên Nhà cung cấp" FieldName="TenNhaCungCap" Width="300px" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Địa chỉ" FieldName="DiaChi" Width="300px" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Điện thoại công ty" FieldName="DienThoaiCty" Width="150px" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Website & Fax" FieldName="WebsiteFax" Width="200px" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhóm Nhà cung cấp" FieldName="TenNhomNhaCungCap" Width="150px" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Đại diện" FieldName="GhiChu" Width="300px" VisibleIndex="5">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Điện thoại đại diện" FieldName="DienThoaiNguoiDaiDien" Width="200px" VisibleIndex="5">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Email đại diện" FieldName="EmailNguoiDaiDien" Width="200px" VisibleIndex="5">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                </Columns>
                                <SettingsPager PageSize="15"></SettingsPager>
                                <SettingsBehavior AllowSort="True"  ColumnResizeMode="Control" />  
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <Settings ShowHorizontalScrollBar="True" />
                            </dx:ASPxGridView>
                            <asp:SqlDataSource ID="MyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="sp_Utils_DanhMucTraCuu" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter SessionField="ssKeySearch" Type="String" Name="KeySearch" />
                                    <asp:SessionParameter SessionField="ssTableName" Type="String" Name="TableName" />
                                    <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                </SelectParameters>
                            </asp:SqlDataSource>


                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="550px" Height="490px"
            ScrollBars="Auto" HeaderText="Thông tin nhà cung cấp" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">

                        <tr>
                            <td>Mã Nhà cung cấp</td>
                            <td>
                                <dx:ASPxTextBox ID="txtMaNhaCungCap" ClientInstanceName="txtMaNhaCungCap" runat="server" Width="50%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên Nhà cung cấp</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTenNhaCungCap" ClientInstanceName="txtTenNhaCungCap" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Địa chỉ</td>
                            <td>
                                <dx:ASPxTextBox ID="txtDiaChi" ClientInstanceName="txtDiaChi" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Điện thoại</td>
                            <td>
                                <dx:ASPxTextBox ID="txtDienThoaiCty" ClientInstanceName="txtDienThoaiCty" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Fax</td>
                            <td>
                                <dx:ASPxTextBox ID="txtFax" ClientInstanceName="txtFax" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Website</td>
                            <td>
                                <dx:ASPxTextBox ID="txtWebsite" ClientInstanceName="txtWebsite" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 150px">Lĩnh vực cung cấp</td>
                            <td style="width: 550px" colspan="3">
                                <dx:ASPxComboBox ID="cbNhomNhaCungCap" ClientInstanceName="cbNhomNhaCungCap" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="MaNhomNhaCungCap" Name="Code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenNhomNhaCungCap" Name="BranchName" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Đại diện</td>
                            <td>
                                <dx:ASPxMemo ID="txtGhiChu" ClientInstanceName="txtGhiChu" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                            </td>
                        </tr>
                        <tr>
                            <td>Điện thoại đại diện</td>
                            <td>
                                <dx:ASPxMemo ID="txtDienThoaiNguoiDaiDien" ClientInstanceName="txtDienThoaiNguoiDaiDien" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                            </td>
                        </tr>
                        <tr>
                            <td>Email đại diện</td>
                            <td>
                                <dx:ASPxMemo ID="txtEmailNguoiDaiDien" ClientInstanceName="txtEmailNguoiDaiDien" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
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
                                                <ClientSideEvents Click="function(s,e){saveNhaCungCap();}" />
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
                            <td colspan="3">Bạn có chắc chắn xóa loại tài sản [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
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
            txtMaNhaCungCap.SetText("");
            txtMaNhaCungCap.SetEnabled(true);
            txtTenNhaCungCap.SetText("");
            txtDiaChi.SetText("");
            txtDienThoaiCty.SetText("");
            txtWebsite.SetText("");
            txtFax.SetText("");
            txtDienThoaiNguoiDaiDien.SetText("");
            txtEmailNguoiDaiDien.SetText("");

            txtGhiChu.SetText("");
            txtObjectId.Set('hidden_value', "0");
        }

        function saveNhaCungCap() {

            if (txtMaNhaCungCap.GetText() + "" == "") {
                alert("Chưa nhập Mã loại tài sản");
                return;
            }
            if (txtTenNhaCungCap.GetText() + "" == "") {
                alert("Chưa nhập Tên loại tài sản");
                return;
            }
            
            if (txtGhiChu.GetText() + "" == "") {
                alert("Chưa nhập người đại diện");
                return;
            }
           
            if (txtDienThoaiNguoiDaiDien.GetText() + "" == "") {
                alert("Chưa nhập điện thoại người đại diện");
                return;
            }

            var data = "mode=AddOrUpdate";
            data += "&id=" + txtObjectId.Get("hidden_value");
            data += "&tableName=DMNhaCungCap";

            data += "&MaNhaCungCap=" + txtMaNhaCungCap.GetText();
            data += "&TenNhaCungCap=" + txtTenNhaCungCap.GetText().replace("&","{sang_va}");
            data += "&DiaChi=" + txtDiaChi.GetText();
            data += "&DienThoaiCty=" + txtDienThoaiCty.GetText();
            data += "&Website=" + txtWebsite.GetText();
            data += "&Fax=" + txtFax.GetText();
            data += "&IDNhomNhaCungCap=" + cbNhomNhaCungCap.GetValue();

            data += "&GhiChu=" + txtGhiChu.GetText();
            data += "&DienThoaiNguoiDaiDien=" + txtDienThoaiNguoiDaiDien.GetText();
            data += "&EmailNguoiDaiDien=" + txtEmailNguoiDaiDien.GetText();

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
                    if (data.success) {;
                        txtMaNhaCungCap.SetEnabled(false);
                        txtObjectId.Set('hidden_value', data.id);
                        gridNhaCungCap.PerformCallback();
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
            data += "&tableName=DMNhaCungCap";


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

                        txtMaNhaCungCap.SetText(data.entity.MaNhaCungCap);
                        txtTenNhaCungCap.SetText(data.entity.TenNhaCungCap);
                        txtDiaChi.SetText(data.entity.DiaChi);
                        txtDienThoaiCty.SetText(data.entity.DienThoaiCty);
                        txtWebsite.SetText(data.entity.Website);
                        txtFax.SetText(data.entity.Fax);
                        txtGhiChu.SetText(data.entity.GhiChu);
                        txtDienThoaiNguoiDaiDien.SetText(data.entity.DienThoaiNguoiDaiDien);
                        txtEmailNguoiDaiDien.SetText(data.entity.EmailNguoiDaiDien);
                        cbNhomNhaCungCap.SetValue(data.entity.IDNhomNhaCungCap);
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
                data: "mode=delete&tableName=DMNhaCungCap&id=" + txtObjectId.Get('hidden_value'),
                complete: function () {
                    btnConfirmDelete.SetEnabled(true);
                    popupConfirmDelete.Hide();
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        gridNhaCungCap.PerformCallback();
                    }
                    else {

                    }
                    alert(data.message);
                }
            });
        }
    </script>

</asp:Content>

