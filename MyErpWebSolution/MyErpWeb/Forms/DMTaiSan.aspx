<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMTaiSan.aspx.cs" Inherits="WebRunDragon.Forms.DMTaiSan" %>



<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/DMTaiSan.js" type="text/javascript"></script>

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
                            <div style="display: <%=GetRight(1)%>; float: left; margin-top: 10px; cursor: pointer; width: 45px;"
                                onclick="openAddForm()" title="Thêm mới">
                                <img src="../Images/icon-add.png" />&nbsp;
                            </div>
                        </td>
                        <%--<td style="width: 81px">Từ khóa</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px" Theme="PlasticBlue"></dx:ASPxTextBox>
                        </td>--%>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>

                        <td>
                            <dx:ASPxButton ID="btnSearchThayDoi" ClientInstanceName="btnSearchThayDoi" runat="server" Text="Tìm kiếm TS thay đổi" Style="margin-left: 0px" OnClick="btnSearchThayDoi_Click" Theme="Office2003Blue" />
                        </td>

                        <td>
                            <dx:ASPxButton ID="btnSearchThayDoi_Duyet" ClientInstanceName="btnSearchThayDoi_Duyet" runat="server" Text="Tìm kiếm duyệt thay đổi" Style="margin-left: 0px" OnClick="btnSearchThayDoi_Duyet_Click" Theme="Office2003Blue" />
                        </td>
                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridTaiSan" ClientInstanceName="gridTaiSan" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue" AllowUserToResizeColumns="True"
                                OnCustomCallback="gridTaiSan_CustomCallback"
                                OnPageIndexChanged="gridTaiSan_PageIndexChanged"
                                OnDataBinding="gridTaiSan_DataBinding">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(0) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenTaiSan") %>','ReadOnly')" title="Xem thông tin">
                                                    <img src="../Images/icon-view.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(2) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenTaiSan") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                    <img src="../Images/icon-edit.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("TenTaiSan") %>')" title="Xóa">
                                                    <img src="../Images/icon-delete.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Duyệt" VisibleIndex="0" Width="40px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(5) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenTaiSan") %>','ApprovalOnly')" title="Duyệt">
                                                    <img src="../Images/icon-approval.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trạng thái TS" FieldName="TenTrangThai" Width="120px" FixedStyle="Left">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                    <%--<dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="TenChiNhanh" VisibleIndex="1" />--%>
                                    <dx:GridViewDataTextColumn Caption="Nhóm tài sản" FieldName="TenNhomTaiSan" VisibleIndex="1" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã tài sản" FieldName="MaTaiSan" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên tài sản" FieldName="TenTaiSan" VisibleIndex="3" Width="400px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" VisibleIndex="4" Width="50px" />
                                    <dx:GridViewDataTextColumn Caption="Giá trị" FieldName="GiaTriTaiSan" VisibleIndex="5" Width="150px">
                                        <PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Số S/N" FieldName="SoSerialNumber" VisibleIndex="6" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhà sản xuất" FieldName="TenNhaSanXuat" VisibleIndex="11" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Vị trí" FieldName="TenViTri" VisibleIndex="12" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trọng lượng" FieldName="TrongLuong" VisibleIndex="13" Width="100px">
                                        <PropertiesTextEdit DisplayFormatString="n2"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ĐV trọng lượng" FieldName="TenDonViTrongLuong" VisibleIndex="13" Width="100px" />
                                    <dx:GridViewDataTextColumn Caption="Kích thước rộng" FieldName="KichThuotRong" VisibleIndex="13" Width="100px">
                                        <PropertiesTextEdit DisplayFormatString="n2"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Kích thước dài" FieldName="KichThuotDai" VisibleIndex="13" Width="100px">
                                        <PropertiesTextEdit DisplayFormatString="n2"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ĐV kích thước" FieldName="TenDonViKichThuot" VisibleIndex="13" Width="100px" />
                                    <%--  <dx:GridViewDataTextColumn Caption="Năm sản xuất" FieldName="NamSanXuat" VisibleIndex="12" Width="100px" />
                                 <dx:GridViewDataTextColumn Caption="Số phiếu bảo hành" FieldName="SoPhieuBaoHanh" VisibleIndex="12" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>           
                                    <dx:GridViewDataTextColumn Caption="Tỷ lệ hao mòn" FieldName="TyLeHaoMon" VisibleIndex="15" Width="50px" />
                                    <dx:GridViewDataTextColumn Caption="Chứng nhận CO/CQ" FieldName="ChungNhanCOCQ" VisibleIndex="16" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>--%>

                                    <dx:GridViewDataTextColumn Caption="Loại bảo hiểm" FieldName="ViTri" VisibleIndex="14" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Loại tài sản" FieldName="TenLoaiTaiSan" VisibleIndex="15" Width="400px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" VisibleIndex="16" Width="500px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Thay đổi tài sản" FieldName="NoiDungThayDoi" VisibleIndex="17" Width="350px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Người tạo" FieldName="CreatedBy" VisibleIndex="19" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager PageSize="15"></SettingsPager>
                                <SettingsBehavior AllowSort="True" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <Settings ShowHorizontalScrollBar="True" />
                                <SettingsBehavior ColumnResizeMode="Control" />
                                <%--<SettingsBehavior AllowEllipsisInText="true" />--%>
                            </dx:ASPxGridView>
                            <%--<asp:SqlDataSource ID="MyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="SP_DMTaiSan_Search" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter SessionField="ssKey4Search" Type="String" Name="Key4Search" />
                                    <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                </SelectParameters>
                            </asp:SqlDataSource>--%>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>


    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="900px" Height="800px"
            ScrollBars="Auto" HeaderText="Thông tin TaiSan" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <dx:ASPxPageControl ID="ASPxTabControl1" runat="server" Theme="PlasticBlue" ActiveTabIndex="0" Width="100%">
                        <TabPages>
                            <dx:TabPage Text="Chi tiết tài sản">
                                <ContentCollection>
                                    <dx:ContentControl>

                                        <table class="tblPopupUpdateForm" style="width: 100%">
                                            <tr>
                                                <td>Mã tài sản sao chép</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtMaTaiSanCopy" ClientInstanceName="txtMaTaiSanCopy" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnSaoChepTaiSan" ClientInstanceName="btnSaoChepTaiSan" runat="server" AutoPostBack="False" Text="Sao chép">
                                                        <ClientSideEvents Click="function(s,e){SaoChepThongTinTaiSan();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td></td>
                                            </tr>
                                            <tr>
                                                <td>Nhóm tài sản</td>
                                                <td colspan="3">
                                                    <dx:ASPxComboBox ID="cbNhomTaiSan" ClientInstanceName="cbNhomTaiSan" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="MaNhomTaiSan" Name="MaNhomTaiSan" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="TenNhomTaiSan" Name="TenNhomTaiSan" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">Mã TaiSan</td>
                                                <td style="width: 200px">
                                                    <dx:ASPxTextBox ID="txtMaTaiSan" ClientInstanceName="txtMaTaiSan" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                                <td>Số S/N</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtSoSerialNumber" ClientInstanceName="txtSoSerialNumber" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tên TaiSan</td>
                                                <td colspan="3">
                                                    <dx:ASPxTextBox ID="txtTenTaiSan" ClientInstanceName="txtTenTaiSan" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Thương hiệu</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbNhaSanXuat" ClientInstanceName="cbNhaSanXuat" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="MaNhaSanXuat" Name="MaNhaSanXuat" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="TenNhaSanXuat" Name="TenNhaSanXuat" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>

                                                <td>Vị trí</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbViTri" ClientInstanceName="cbViTri" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="MaViTri" Name="MaViTri" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="TenViTri" Name="TenViTri" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Giá trị</td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="txtGiaTriTaiSan" ClientInstanceName="txtGiaTriTaiSan" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                </td>
                                                <td style="width: 100px">Trọng lượng</td>
                                                <td style="width: 200px">
                                                    <dx:ASPxSpinEdit ID="txtTrongLuong" ClientInstanceName="txtTrongLuong" runat="server" DisplayFormatString="#,###.##" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">Đơn vị trọng lượng</td>
                                                <td style="width: 200px">
                                                    <dx:ASPxComboBox ID="cbDonViTinhTrongLuong" ClientInstanceName="cbDonViTinhTrongLuong" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="MaDonViTinh" Name="MaDonViTinh" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="TenDonViTinh" Name="TenDonViTinh" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Kích thướt (Dài X Rộng)</td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="txtKichThuotDai" ClientInstanceName="txtKichThuotDai" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                </td>
                                                <td>
                                                    <dx:ASPxSpinEdit ID="txtKichThuotRong" ClientInstanceName="txtKichThuotRong" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>

                                                </td>
                                                <td style="width: 200px">
                                                    <dx:ASPxComboBox ID="cbDonViTinhKichThuot" ClientInstanceName="cbDonViTinhKichThuot" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="MaDonViTinh" Name="MaDonViTinh" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="TenDonViTinh" Name="TenDonViTinh" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Ghi chú</td>
                                                <td colspan="3">
                                                    <dx:ASPxMemo ID="txtGhiChu" ClientInstanceName="txtGhiChu" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100px">Đơn vị tính</td>
                                                <td style="width: 200px">
                                                    <dx:ASPxComboBox ID="cbDonViTinh" ClientInstanceName="cbDonViTinh" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="MaDonViTinh" Name="MaDonViTinh" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="TenDonViTinh" Name="TenDonViTinh" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td></td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                        <ClientSideEvents Click="function(s,e){saveTaiSan();}" />
                                                    </dx:ASPxButton>

                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyet" ClientInstanceName="btnGuiDuyet" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                        <ClientSideEvents Click="function(s,e){SendApprovalDMTaiSan();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnThayDoiGia" ClientInstanceName="btnThayDoiGia" runat="server" AutoPostBack="False" Text="Cập nhật thông tin" Width="99px" ToolTip="Cập nhật: Giá, Nhóm TS, Loại bảo hiểm">
                                                                    <ClientSideEvents Click="function(s,e){UpdateGiaTriTaiSan();}" />
                                                                </dx:ASPxButton>
                                                            </td>
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

                                        <table style="width: 100%">
                                            <tr>
                                                <td>Thông tin tài sản theo đơn vị</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div astyle="display: <%=GetRight(1)%>; float: left; margin-top: 10px; cursor: pointer; width: 45px;">
                                                        <dx:ASPxImage ToolTip="Thêm mới" ID="btnAddDetail" runat="server" ImageUrl="../Images/icon-add.png">
                                                            <ClientSideEvents Click="function(s,e){openAddingFormDetail();}" />
                                                        </dx:ASPxImage>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        EnableTheming="True" Theme="PlasticBlue" OnCustomCallback="gridDetails_CustomCallback" OnPageIndexChanged="gridDetails_PageIndexChanged">

                                                        <Columns>
                                                            <%--<dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                                                <DataItemTemplate>
                                                                    <dataitemtemplate>
                                                                        <div style="display: <%#GetRight(0) %>; cursor: pointer" onclick="openEditFormDetail('<%#Eval("ID") %>','<%#Eval("TenDonViTinh") %>','ReadOnly')" title="Xem thông tin">
                                                                            <img src="../Images/icon-view.png" />
                                                                        </div>
                                                                    </dataitemtemplate>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>--%>
                                                            <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                                                <DataItemTemplate>
                                                                    <dataitemtemplate>
                                                                        <div style="display: <%#GetRight(2) %>; cursor: pointer" onclick="openEditFormDetail('<%#Eval("ID") %>','<%#Eval("TenDonViTinh") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                                            <img src="../Images/icon-edit.png" />
                                                                        </div>
                                                                    </dataitemtemplate>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                                                <DataItemTemplate>
                                                                    <dataitemtemplate>
                                                                        <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="openDeleteFormDetail('<%#Eval("ID") %>','<%#Eval("TenDonViTinh") %>')" title="Xóa">
                                                                            <img src="../Images/icon-delete.png" />
                                                                        </div>
                                                                    </dataitemtemplate>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>
                                                            <%--<dx:GridViewDataTextColumn Caption="Duyệt" VisibleIndex="0" Width="40px" FixedStyle="Left">
                                                                <DataItemTemplate>
                                                                    <dataitemtemplate>
                                                                        <div style="display: <%#GetRight(5) %>; cursor: pointer" onclick="openEditFormDetail('<%#Eval("ID") %>','<%#Eval("TenDonViTinh") %>','ApprovalOnly')" title="Duyệt">
                                                                            <img src="../Images/icon-approval.png" />
                                                                        </div>
                                                                    </dataitemtemplate>
                                                                </DataItemTemplate>
                                                            </dx:GridViewDataTextColumn>--%>

                                                            <%--<dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Width="39px" />--%>
                                                            <dx:GridViewDataTextColumn Caption="Đơn vị" FieldName="TenDonViTinh" Width="200px" VisibleIndex="3" />
                                                            <dx:GridViewDataTextColumn Caption="Giá trị qui đổi" FieldName="GiaTriQuiDoiSoVoiDonViCoBan" Width="250px" VisibleIndex="4" />
                                                            <dx:GridViewDataTextColumn Caption="Đơn vị cơ bản" FieldName="TenDonViTinhCoBan" Width="100%" VisibleIndex="5" />
                                                        </Columns>
                                                        <SettingsPager PageSize="10"></SettingsPager>
                                                        <Settings ShowFooter="False" />
                                                        <SettingsBehavior AllowSort="False" />
                                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                                        <Settings ShowHorizontalScrollBar="True" />
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>

                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>


                            <dx:TabPage Text="Duyệt (không duyệt)">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 150px">Ghi chú duyệt(không duyệt)</td>
                                                <td style="width: 200px" colspan="2">
                                                    <dx:ASPxMemo ID="txtGhiChuDuyetKhongDuyet" ClientInstanceName="txtGhiChuDuyetKhongDuyet" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnDuyet" ClientInstanceName="btnDuyet" runat="server" AutoPostBack="False" Text="Duyệt">
                                                        <ClientSideEvents Click="function(s,e){ApprovalTaiSan();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="Từ chối">
                                                        <ClientSideEvents Click="function(s,e){RejectTaiSan();}" />
                                                    </dx:ASPxButton>

                                                </td>
                                            </tr>
                                            <tr style="height: 20px">
                                                <td colspan="4"></td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px" colspan="3"></td>
                                            </tr>
                                            <tr>
                                                <td>Lịch sử gửi và ký duyệt
                                                </td>
                                                <td colspan="2">
                                                    <dx:ASPxGridView ID="gridLichSuKyDuyet" ClientInstanceName="gridLichSuKyDuyet" runat="server" Width="100%"
                                                        AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                                        OnCustomCallback="gridLichSuKyDuyet_CustomCallback">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="Nhân sự thực hiện" FieldName="NhanSu" Width="100px" VisibleIndex="0" />
                                                            <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThai" Width="100px" VisibleIndex="3" />
                                                            <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" Width="250px" VisibleIndex="5" />
                                                            <dx:GridViewDataTextColumn Caption="Thời gian thực hiện" FieldName="ThoiGian" Width="250px" VisibleIndex="6">
                                                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Nhân sự nhận" FieldName="UserNext" Width="150px" VisibleIndex="10" />
                                                        </Columns>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                    </dx:ASPxPageControl>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>

    <div>
        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridTaiSan">
        </dx:ASPxGridViewExporter>

        <dx:ASPxButton ID="btnExcel" ClientInstanceName="btnExcel" runat="server" Text="Excel" Style="margin-left: 0px" OnClick="btnExcel_Click" Theme="Office2003Blue" />
    </div>


    <div>
        <dx:ASPxPopupControl ID="popupConfirmDelete" ClientInstanceName="popupConfirmDelete" runat="server" Height="100px" Width="493px"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">

            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3">Bạn có chắc chắn xóa TaiSan [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
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
        <dx:ASPxPopupControl ClientInstanceName="popUpdateDetail" ID="popUpdateDetail" Width="650px" Height="150px" HeaderText="Cập nhật chi tiết " runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td>Đơn vị tính </td>
                            <td>
                                <dx:ASPxComboBox ID="cbDonViTinhCT" ClientInstanceName="cbDonViTinhCT" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="MaDonViTinh" Name="MaDonViTinh" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenDonViTinh" Name="TenDonViTinh" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                            <td>Qui đổi theo đơn vị cơ bản</td>
                            <td>

                                <dx:ASPxSpinEdit ID="txtGiaTriQuiDoi" ClientInstanceName="txtGiaTriQuiDoi" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>

                            </td>
                            <td>
                                <dx:ASPxLabel ID="txtTenDonViCoBan" ClientInstanceName="txtTenDonViCoBan" runat="server" Width="100%" HorizontalAlign="Left"></dx:ASPxLabel>

                            </td>
                        </tr>
                        <tr style="height: 10px">
                        </tr>

                        <tr>
                            <td></td>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveThayDoi" ClientInstanceName="btnSaveThayDoi" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                <ClientSideEvents Click="function(s,e){saveQLTaiSanThayDoiCT();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <%--<td style="text-align: right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyetThayDoi" ClientInstanceName="btnGuiDuyetThayDoi" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                <ClientSideEvents Click="function(s,e){SendApprovalThayDoiTaiSan();}" />
                                            </dx:ASPxButton>
                                        </td>--%>
                                        <td>
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelThayDoi" ClientInstanceName="btnCancelSaveDetail" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
                                                <ClientSideEvents Click="function(s,e){popUpdateDetail.Hide();}" />
                                            </dx:ASPxButton>
                                        </td>

                                    </tr>
                                    <tr>

                                        <%-- <td style="text-align: right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnApprovalThayDoi" ClientInstanceName="btnApprovalThayDoi" runat="server" AutoPostBack="False" Text="Duyệt thay đổi" Width="99px">
                                                <ClientSideEvents Click="function(s,e){ApprovalThayDoiTaiSan();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="text-align: right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnRejectThayDoi" ClientInstanceName="btnRejectThayDoi" runat="server" AutoPostBack="False" Text="Từ chối thay đổi" Width="99px">
                                                <ClientSideEvents Click="function(s,e){RejectThayDoiTaiSan();}" />
                                            </dx:ASPxButton>
                                        </td>--%>

                                        <td></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <%-- <tr>
                            <td>Ghi chú không duyệt</td>
                            <td colspan="3">
                                <dx:ASPxMemo ID="txtGhiChuDuyetKhongDuyetThayDoi" ClientInstanceName="txtGhiChuDuyetKhongDuyetThayDoi" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="text-align: right">

                             

                            </td>

                        </tr>--%>
                    </table>

                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>

    <div>
        <dx:ASPxPopupControl ClientInstanceName="popConfirmDeleteDetail" ID="popConfirmDeleteDetail" Width="400" Height="160px" HeaderText="Xác nhận xóa" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td style="font-weight: 800; text-align: center">
                                <br />
                                Bạn có chắc chắn xóa  [<span style="font-weight: bold; color: red" id="spDetail"></span>] ?
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnConfirmDeleteDetail" ClientInstanceName="btnConfirmDeleteDetail" runat="server" AutoPostBack="False" Text="Xóa" Width="99px">
                                                <ClientSideEvents Click="function(s,e){doDeleteDetail();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td align="left">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelDeleteDetail" ClientInstanceName="btnCancelDeleteDetail" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
                                                <ClientSideEvents Click="function(s,e){popConfirmDeleteDetail.Hide();}" />
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
        <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>
        <dx:ASPxHiddenField runat="server" ID="txtObjectDetailId" ClientInstanceName="txtObjectDetailId"></dx:ASPxHiddenField>
    </div>


</asp:Content>

