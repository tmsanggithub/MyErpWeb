<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLHangHoa.aspx.cs" Inherits="WebRunDragon.Forms.QLHangHoa" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/QLHangHoa.js" type="text/javascript"></script>
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
                            <dx:ASPxTextBox MaxLength="50" runat="server" ID="txtKeyword" ClientInstanceName="txtKeyword" Width="118px" Height="22px" Theme="PlasticBlue"></dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px"
                                OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>

                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridQLHangHoa" ClientInstanceName="gridQLHangHoa" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnDataBinding="gridQLHangHoa_DataBinding"
                                OnCustomCallback="gridQLHangHoa_CustomCallback"
                                OnPageIndexChanged="gridQLHangHoa_PageIndexChanged">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(0) %>; cursor: pointer" onclick="openEditForm('<%#Eval("id") %>','ReadOnly')" title="Xem thông tin">
                                                    <img src="../Images/icon-view.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(2) %>; cursor: pointer" onclick="openEditForm('<%#Eval("id") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                    <img src="../Images/icon-edit.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="OpenDeleteForm('<%#Eval("id") %>','<%#Eval("ten_hang_hoa") %>')" title="Xóa">
                                                    <img src="../Images/icon-delete.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="0" Width="40px" />
                                    <dx:GridViewDataTextColumn Caption="Mã hàng hóa" FieldName="ma_hang_hoa" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã vạch" FieldName="ma_vach" VisibleIndex="3" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên hàng hóa" FieldName="ten_hang_hoa" VisibleIndex="4" Width="350px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Vị trí" FieldName="ten_vi_tri" VisibleIndex="5" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="mo_ta_chi_tiet_hang_hoa" VisibleIndex="6" Width="100%">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                </Columns>
                                <SettingsPager PageSize="12"></SettingsPager>
                                <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <Settings ShowHorizontalScrollBar="True" />
                            </dx:ASPxGridView>

                        </td>
                    </tr>
                </table>

                <div>
                    <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>
                    <dx:ASPxHiddenField runat="server" ID="txtObjectDetailId" ClientInstanceName="txtObjectDetailId"></dx:ASPxHiddenField>
                </div>

            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

    <div>

        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="1000px" Height="700px"
            ScrollBars="Auto" HeaderText="Thông tin hàng hóa" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%" border="0">

                        <tr>
                            <td style="width: 100px">Mã hàng hóa</td>
                            <td style="width: 250px">
                                <dx:ASPxTextBox ID="txtMaHangHoa" ClientInstanceName="txtMaHangHoa" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                            <td style="width: 100px">Mã vạch</td>
                            <td style="width: 250px">
                                <dx:ASPxTextBox ID="txtMaVach" ClientInstanceName="txtMaVach" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên hàng hóa</td>
                            <td colspan="3">
                                <dx:ASPxTextBox ID="txtTenHangHoa" ClientInstanceName="txtTenHangHoa" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Nhóm hàng hóa</td>

                            <td>
                                <dx:ASPxComboBox ID="cbNhomHangHoa" ClientInstanceName="cbNhomHangHoa" runat="server" ValueField="id" TextFormatString="{2}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="true" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                            <td>Thương hiệu</td>

                            <td>
                                <dx:ASPxComboBox ID="cbThuongHieu" ClientInstanceName="cbThuongHieu" runat="server" ValueField="id" TextFormatString="{2}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="true" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Định mức tồn kho thấp nhất</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtDinhMucTonKhoThapNhat" ClientInstanceName="txtDinhMucTonKhoThapNhat" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                            <td>Định mức tồn cao thấp nhất</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtDinhMucTonCaoThapNhat" ClientInstanceName="txtDinhMucTonCaoThapNhat" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>Tên vị trí</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtTenViTri" ClientInstanceName="txtTenViTri" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                            <td>Trọng lượng</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtTrongLuong" ClientInstanceName="txtTrongLuong" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>Đơn vị trọng lượng</td>
                            <td>
                                <dx:ASPxComboBox ID="cbDonViTrongLuong" ClientInstanceName="cbDonViTrongLuong" runat="server" ValueField="id" TextFormatString="{2}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="true" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                            <td>Kích thước rộng</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtKichThuotRong" ClientInstanceName="txtKichThuotRong" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>Kích thước dài</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtKichThuotDai" ClientInstanceName="txtKichThuotDai" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                            <td>Đơn vị kích thước</td>
                            <td>
                                <dx:ASPxComboBox ID="cbDonViKichThuot" ClientInstanceName="cbDonViKichThuot" runat="server" ValueField="id" TextFormatString="{2}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="true" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Mô tả chi tiết</td>
                            <td colspan="3">
                                <dx:ASPxMemo ID="txtMoTaChiTietHangHoa" ClientInstanceName="txtMoTaChiTietHangHoa" runat="server" TextMode="MultiLine" Width="100%" Height="60px"></dx:ASPxMemo>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="4">
                                <div style="width: 100%;">
                                    <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                                        EnableTheming="True" Theme="PlasticBlue"
                                        OnDataBinding="gridDetails_DataBinding"
                                        OnCustomCallback="gridDetails_CustomCallback"
                                        OnPageIndexChanged="gridDetails_PageIndexChanged">

                                        <ClientSideEvents CustomButtonClick="function(s,e){
                                            if (e.buttonID == 'btnEditDetail'){
                                                s.GetRowValues(e.visibleIndex, 'id;ten_don_vi_tinh', openEditFormDetail);
                                            }
                                            else if (e.buttonID == 'btnEDeleteDetail'){
                                                s.GetRowValues(e.visibleIndex, 'id;ten_don_vi_tinh', openDeleteFormDetail);
                                            }
                                        }" />
                                        <Columns>
                                            <dx:GridViewCommandColumn Width="68px" VisibleIndex="0" ButtonType="Image" ShowNewButtonInHeader="True">
                                                <HeaderTemplate>
                                                    <div align="center">
                                                        <dx:ASPxImage ToolTip="Thêm mới" ID="ASPxImage1" runat="server" ImageUrl="../Images/icon-add.png">
                                                            <ClientSideEvents Click="function(s,e){openAddingFormDetail();}" />
                                                        </dx:ASPxImage>
                                                    </div>
                                                </HeaderTemplate>
                                                <CustomButtons>
                                                    <dx:GridViewCommandColumnCustomButton ID="btnEditDetail">
                                                        <Image ToolTip="Sửa thông tin" Url="../Images/icon-edit.png"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                    <dx:GridViewCommandColumnCustomButton ID="btnEDeleteDetail">
                                                        <Image ToolTip="Xóa thông tin" Url="../Images/icon-delete.png"></Image>
                                                    </dx:GridViewCommandColumnCustomButton>
                                                </CustomButtons>

                                            </dx:GridViewCommandColumn>
                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="0" Visible="true" Width="50px" />
                                            <dx:GridViewDataTextColumn Caption="Tên đơn vị tính" FieldName="ten_don_vi_tinh" Width="200px" VisibleIndex="3">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Giá trị qui đổi" FieldName="gia_tri_qui_doi" Width="120px" VisibleIndex="4">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Giá bán" FieldName="gia_ban" Width="120px" VisibleIndex="5">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>
                                            <dx:GridViewDataTextColumn Caption="Đơn vị cơ bản" FieldName="is_don_vi_co_ban" Width="120px" VisibleIndex="6">
                                                <Settings AutoFilterCondition="Contains" />
                                            </dx:GridViewDataTextColumn>

                                        </Columns>
                                        <SettingsPager PageSize="10"></SettingsPager>
                                        <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                        <Settings ShowFilterRow="True" />
                                        <Settings ShowHorizontalScrollBar="True" />

                                    </dx:ASPxGridView>
                                </div>

                            </td>
                        </tr>

                        <tr>
                            <td></td>
                            <td colspan="3">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 150px"></td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td>
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                <ClientSideEvents Click="function(s,e){saveQLHangHoa();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 20px;"></td>
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
                            <td colspan="3">Bạn có chắc chắn xóa  [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <dx:ASPxButton Theme="Office2003Blue" ID="btnConfirmDelete" ClientInstanceName="btnConfirmDelete" runat="server" Text="Có">
                                    <ClientSideEvents Click="function(s, e) {DoDelete(); }" />
                                </dx:ASPxButton>
                            </td>
                            <td></td>
                            <td>
                                <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelDelete" runat="server" Text="Không">
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
        <dx:ASPxPopupControl ClientInstanceName="popUpdateDetail" ID="popUpdateDetail" Width="500px" Height="300px" HeaderText="Cập nhật đơn vị tính" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">

                        <tr>
                            <td style="width: 120px">Tên đơn vị tính</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTenDonViTinh" ClientInstanceName="txtTenDonViTinh" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>Giá trị qui đổi</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtGiaTriQuiDoi" ClientInstanceName="txtGiaTriQuiDoi" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>Giá bán</td>
                            <td>
                                <dx:ASPxSpinEdit ID="txtGiaBan" ClientInstanceName="txtGiaBan" runat="server" Width="100%"></dx:ASPxSpinEdit>
                            </td>
                        </tr>
                        <tr>
                            <td>Đơn vị cơ bản</td>
                            <td>
                                <dx:ASPxCheckBox ID="chkIsDonViCoBan" ClientInstanceName="chkIsDonViCoBan" runat="server"></dx:ASPxCheckBox>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 150px"></td>
                                        <td>
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveDetail" ClientInstanceName="btnSaveDetail" runat="server" AutoPostBack="False" Text="Lưu">
                                                <ClientSideEvents Click="function(s,e){saveQLHangHoaChiTiet();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 20px;"></td>
                                        <td>
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelEditDetail" runat="server" AutoPostBack="False" Text="Thoát">
                                                <ClientSideEvents Click="function(s,e){popUpdateDetail.Hide();}" />
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
        <dx:ASPxPopupControl ID="popConfirmDeleteDetail" ClientInstanceName="popConfirmDeleteDetail" runat="server" Height="100px" Width="493px"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">

            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3">Bạn có chắc chắn xóa  [<span id="spDetail" style="font-weight: bold; color: red"></span>] ?</td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <dx:ASPxButton Theme="Office2003Blue" ID="btnConfirmDeleteDetail" ClientInstanceName="btnConfirmDeleteDetail" runat="server" Text="Có">
                                    <ClientSideEvents Click="function(s, e) {doDeleteDetail(); }" />
                                </dx:ASPxButton>
                            </td>
                            <td></td>
                            <td>
                                <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelDeleteDetail" runat="server" Text="Không">
                                    <ClientSideEvents Click="function(s, e) {	popConfirmDeleteDetail.Hide();}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>

        </dx:ASPxPopupControl>

    </div>

</asp:Content>
