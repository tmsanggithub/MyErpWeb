<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLNhapKho.aspx.cs" Inherits="WebRunDragon.Forms.QLNhapKho" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var branchID = '<%=GetBranchID()%>';
        var userLogin = '<%=GetUserLogin()%>';
    </script>
    <script src="../Scripts/QLNhapKho.js" type="text/javascript"></script>
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
                        <%--<td style="width: 81px">Từ khóa</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px" Theme="PlasticBlue"></dx:ASPxTextBox>
                        </td>--%>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px"
                                OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>

                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridQLNhapKho" ClientInstanceName="gridQLNhapKho" runat="server" Width="100%"
                                AutoGenerateColumns="false" EnableTheming="True" Theme="PlasticBlue"
                                DataSourceID="SqlDataSourceNhapKho"
                                OnCustomCallback="gridQLNhapKho_CustomCallback"
                                OnPageIndexChanged="gridQLNhapKho_PageIndexChanged">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(0) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ReadOnly')" title="Xem thông tin">
                                                 <img src="../Images/icon-view.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(2) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                   <img src="../Images/icon-edit.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("SoPhieu") %>')" title="Xóa">
                                                   <img src="../Images/icon-delete.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Duyệt" VisibleIndex="0" Width="40px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(5) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ApprovalOnly')" title="Duyệt">
                                                   <img src="../Images/icon-approval.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThai"  Width="120px" VisibleIndex="0" FixedStyle="Left">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Width="40px" />
                                    <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="TenChiNhanh"  Width="150px" VisibleIndex="0">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Kho nhập" FieldName="TenKho"  Width="150px" VisibleIndex="2">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Số phiếu" FieldName="SoPhieu"  Width="150px" VisibleIndex="3">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ngày nhập" FieldName="NgayPhieu"  Width="100px" VisibleIndex="5">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhân sự lập" FieldName="TenNhanVien" Width="200px" VisibleIndex="6">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Người duyệt" FieldName="NguoiDuyet"  Width="150px" VisibleIndex="7">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu"  Width="500px" VisibleIndex="9">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>


                                </Columns>
                                <SettingsPager PageSize="20"></SettingsPager>
                                <SettingsBehavior AllowSort="True" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <SettingsBehavior ColumnResizeMode="Control" />
                            </dx:ASPxGridView>
                            <asp:SqlDataSource ID="SqlDataSourceNhapKho" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="SP_QLNhapKho_Search" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter SessionField="ssKey4Search" Type="String" Name="Key4Search" />
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

        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="900px" Height="650px"
            ScrollBars="Auto" HeaderText="Thông tin chi tiết chi" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">



                    <dx:ASPxPageControl ID="ASPxTabControl1" runat="server" Theme="PlasticBlue" ActiveTabIndex="0" Width="100%">
                        <TabPages>

                            <dx:TabPage Text="Chi tiết nhập kho">
                                <ContentCollection>
                                    <dx:ContentControl>

                                        <table class="tblPopupUpdateForm" style="width: 100%" border="0">
                                            <tr>
                                                <td style="width: 150px">Chi nhánh</td>
                                                <td style="width: 550px" colspan="3">
                                                    <dx:ASPxComboBox ID="cbMaChiNhanh" ClientInstanceName="cbMaChiNhanh" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" ReadOnly="true">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="Code" Name="Code" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="BranchName" Name="BranchName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px">Ngày nhập</td>
                                                <td style="width: 200px">
                                                    <dx:ASPxDateEdit ID="deNgayNhap" ClientInstanceName="deNgayNhap" runat="server"></dx:ASPxDateEdit>
                                                </td>
                                                <td style="width: 150px">Số phiếu nhập</td>
                                                <td style="width: 200px">
                                                    <dx:ASPxTextBox ID="txtSoPhieuNhap" ClientInstanceName="txtSoPhieuNhap" runat="server" Width="170px" ReadOnly="true"></dx:ASPxTextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td style="width: 150px">Kho nhập</td>
                                                <td style="width: 550px" colspan="3">
                                                    <dx:ASPxComboBox ID="cbKhoNhap" ClientInstanceName="cbKhoNhap" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="MaKho" Name="MaKho" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="TenKho" Name="TenKho" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px">Nhân viên</td>
                                                <td style="width: 550px" colspan="3">
                                                    <dx:ASPxComboBox ID="cbNhanVienPhieu" ClientInstanceName="cbNhanVienPhieu" runat="server" ValueField="UserName" ReadOnly="true" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="UserName" Name="UserName" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="StaffName" Name="StaffName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td>Ghi chú</td>
                                                <td colspan="3">
                                                    <dx:ASPxTextBox ID="txtGhiChu" ClientInstanceName="txtGhiChu" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%"
                                                        AutoGenerateColumns="False"
                                                        DataSourceID="SqlDataSourceTaiSan"
                                                        EnableTheming="True" Theme="PlasticBlue"
                                                        OnCustomCallback="gridDetails_CustomCallback"
                                                        OnPageIndexChanged="gridDetails_PageIndexChanged">
                                                        <ClientSideEvents CustomButtonClick="function(s,e){
                                                                if (e.buttonID == 'btnEditDetail'){
                                                                    s.GetRowValues(e.visibleIndex, 'ID;TenTaiSan', openEditFormDetail);
                                                                }
                                                                else if (e.buttonID == 'btnEDeleteDetail'){
                                                                    s.GetRowValues(e.visibleIndex, 'ID;TenTaiSan', openDeleteFormDetail);
                                                                }
                                                            }" />
                                                        <Columns>
                                                            <dx:GridViewCommandColumn Width="68px" VisibleIndex="0" ButtonType="Image" ShowNewButtonInHeader="True">
                                                                <HeaderTemplate>
                                                                    <div align="center">
                                                                        <dx:ASPxImage ToolTip="Thêm mới" ID="ASPxImage1" runat="server" ImageUrl="../Images/icon-add.png">
                                                                            <ClientSideEvents Click="function(s,e){openAddingFormDetail();}" />
                                                                        </dx:ASPxImage>
                                                                    </div>
                                                                </HeaderTemplate>
                                                                <CustomButtons>
                                                                    <dx:GridViewCommandColumnCustomButton ID="btnEditDetail">
                                                                        <Image ToolTip="Sửa thông tin quyền" Url="../Images/icon-edit.png"></Image>
                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                    <dx:GridViewCommandColumnCustomButton ID="btnEDeleteDetail">
                                                                        <Image ToolTip="Xóa thông tin quyền" Url="../Images/icon-delete.png"></Image>
                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                </CustomButtons>
                                                            </dx:GridViewCommandColumn>
                                                            <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                                            <dx:GridViewDataTextColumn Caption="Mã Tài sản" FieldName="MaTaiSan" Width="150px" VisibleIndex="1">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Tài sản" FieldName="TenTaiSan" Width="200px" VisibleIndex="2">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" Width="50px" VisibleIndex="3">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="SL" FieldName="SoLuong" Width="50px" VisibleIndex="4" />
                                                            <dx:GridViewDataTextColumn Caption="Đơn giá" FieldName="GiaTriTaiSan" VisibleIndex="5" Width="100px" />
                                                            <dx:GridViewDataTextColumn Caption="Thành tiền" FieldName="ThanhTien" Width="100px" VisibleIndex="6" />
                                                            <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThaiTS" Width="120px" VisibleIndex="7">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Thời gian mua" FieldName="NamDuaVaoSuDung" Width="100px" VisibleIndex="8" />
                                                            <dx:GridViewDataTextColumn Caption="Số S/N" FieldName="SoSerialNumber" Width="200px" VisibleIndex="9">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Cấu hình" FieldName="CauHinh" Width="200px" VisibleIndex="10" />
                                                            <dx:GridViewDataTextColumn Caption="Nhà cung cấp" FieldName="TenNhaCungCap" VisibleIndex="11" Width="300px">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Nhà sản xuất" FieldName="TenNhaSanXuat" VisibleIndex="11" Width="150px">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Số tháng BH" FieldName="SoThangBaoHanh" VisibleIndex="13" Width="50px" />
                                                            <dx:GridViewDataTextColumn Caption="Hạn sử dụng(tháng)" FieldName="SoThangHanSuDung" VisibleIndex="14" Width="50px" />
                                                            <%--    <dx:GridViewDataTextColumn Caption="Thời gian hết hạn sd" FieldName="ThoiGianHetHanSuDung" VisibleIndex="14" Width="110px" />--%>
                                                            <dx:GridViewDataTextColumn Caption="Loại bảo hiểm" FieldName="ViTri" VisibleIndex="14" Width="120px">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Ghi chú TS" FieldName="GhiChuTS" VisibleIndex="16" Width="200px">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChuChiTiet" Width="350px" VisibleIndex="16">
                                                                <Settings AutoFilterCondition="Contains" />
                                                            </dx:GridViewDataTextColumn>

                                                        </Columns>
                                                        <SettingsPager PageSize="10"></SettingsPager>
                                                        <Settings ShowFooter="False" />
                                                        <SettingsBehavior AllowSort="False" />
                                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                                        <Settings ShowHorizontalScrollBar="True" />
                                                        <Settings ShowFilterRow="True" />
                                                        <SettingsBehavior ColumnResizeMode="Control" />
                                                    </dx:ASPxGridView>
                                                    <asp:SqlDataSource ID="SqlDataSourceTaiSan" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                                        SelectCommand="SP_QLNhapKhoCT_SearchByIDMaster" SelectCommandType="StoredProcedure">
                                                        <SelectParameters>
                                                            <asp:SessionParameter SessionField="ssIDNhapKho" Type="String" Name="IDNhapKho" />
                                                            <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                                        </SelectParameters>

                                                    </asp:SqlDataSource>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                                    <ClientSideEvents Click="function(s,e){saveQLNhapKho();}" />
                                                                </dx:ASPxButton>

                                                            </td>
                                                            <td style="width: 20px;">&nbsp;</td>

                                                            <td>
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyet" ClientInstanceName="btnGuiDuyet" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                                    <ClientSideEvents Click="function(s,e){SendApprovalQLNhapKho();}" />
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td style="width: 20px;">&nbsp;</td>
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
                                                        <ClientSideEvents Click="function(s,e){ApprovalNhapKho();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="Từ chối">
                                                        <ClientSideEvents Click="function(s,e){RejectNhapKho();}" />
                                                    </dx:ASPxButton>

                                                </td>
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
                                                            <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" Width="200px" VisibleIndex="5" />
                                                            <dx:GridViewDataTextColumn Caption="Thời gian thực hiện" FieldName="ThoiGian" Width="150px" VisibleIndex="6">
                                                                <PropertiesTextEdit DisplayFormatString="dd/MM/yyyy HH:mm:ss" />
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn Caption="Nhân sự nhận" FieldName="UserNext" Width="200px" VisibleIndex="10" />
                                                        </Columns>
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>

                            <dx:TabPage Text="Import tài sản">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td colspan="4">
                                                    <asp:FileUpload ID="FileUpload1" runat="server" ClientIDMode="Static" />
                                                </td>

                                            </tr>

                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxLabel ID="lbThongBaoImport" ClientInstanceName="lbThongBaoImport" runat="server" Text="..." ForeColor="Red"></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>

                                                    <dx:ASPxButton ID="btnUploadImport" ClientInstanceName="btnUploadImport" runat="server" Text="Import" Style="margin-left: 0px"
                                                        OnClick="btnUploadImport_Click" Theme="Office2003Blue">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton ID="btnSaveTSImport" ClientInstanceName="btnSaveTSImport" runat="server" Text="Lưu tài sản import" Style="margin-left: 0px">
                                                        <ClientSideEvents Click="function(s,e){SaveTaiSanImport();}" />
                                                    </dx:ASPxButton>
                                                </td>

                                                <td>
                                                    <dx:ASPxButton ID="btnTraCuuImport" ClientInstanceName="btnTraCuuImport" runat="server" Text="Tra cứu import" Style="margin-left: 0px" Theme="Office2003Blue">
                                                        <ClientSideEvents Click="function(s,e){SearchImport();}" />
                                                    </dx:ASPxButton>
                                                </td>

                                                <td>
                                                    <dx:ASPxButton ID="btnXoaImport" ClientInstanceName="btnXoaImport" runat="server" Text="Xóa import" Style="margin-left: 0px" Theme="Office2003Blue">
                                                        <ClientSideEvents Click="function(s,e){XoaImport();}" />
                                                    </dx:ASPxButton>
                                                </td>

                                            </tr>
                                        </table>
                                        <table style="width: 100%;">
                                            <tr>
                                                <td>
                                                    <%--DataSourceID="dataSourceImportTaiSanQR"--%>
                                                    <dx:ASPxGridView ID="gridImportTaiSan" ClientInstanceName="gridImportTaiSan" runat="server" Width="100%"
                                                        OnDataBinding="gridImportTaiSan_DataBinding"
                                                        OnCustomCallback="gridImportTaiSan_CustomCallback"
                                                        OnPageIndexChanged="gridImportTaiSan_PageIndexChanged"
                                                        AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn Caption="MaTaiSan" FieldName="MaTaiSan" VisibleIndex="0" Width="150px" />
                                                            <dx:GridViewDataTextColumn Caption="TenTaiSan" FieldName="TenTaiSan" VisibleIndex="0" Width="250px" />
                                                            <dx:GridViewDataTextColumn Caption="TrangThaiTaiSan" FieldName="TrangThaiTaiSan" VisibleIndex="0" Width="80px" />
                                                            <dx:GridViewDataTextColumn Caption="SoLuong" FieldName="SoLuong" VisibleIndex="0" Width="60px" />
                                                            <%-- <dx:GridViewDataTextColumn Caption="DonGia" FieldName="DonGia" VisibleIndex="0" Width="100px" />
                                                            <dx:GridViewDataTextColumn Caption="ThanhTien" FieldName="ThanhTien" VisibleIndex="0" Width="100px" />--%>
                                                            <dx:GridViewDataTextColumn Caption="GhiChu" FieldName="GhiChu" VisibleIndex="0" Width="300px" />

                                                        </Columns>
                                                        <SettingsPager PageSize="100"></SettingsPager>
                                                        <SettingsBehavior AllowSort="True" />
                                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                                        <Settings ShowHorizontalScrollBar="True" />
                                                    </dx:ASPxGridView>
                                                    <%--<asp:SqlDataSource ID="dataSourceImportTaiSanQR" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                                        SelectCommand="SP_Import_TaoQRCode_Search" SelectCommandType="StoredProcedure">
                                                        <SelectParameters>
                                                            <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                                        </SelectParameters>
                                                    </asp:SqlDataSource>--%>
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
        <dx:ASPxPopupControl ID="popupConfirmDelete" ClientInstanceName="popupConfirmDelete" runat="server" Height="100px" Width="493px"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">

            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3">Bạn có chắc chắn xóa  [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
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
        <dx:ASPxPopupControl ClientInstanceName="popUpdateDetail" ID="popUpdateDetail" Width="750px" Height="500px" HeaderText="Cập nhật chi tiết " runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td>
                                <dx:ASPxGridView ID="gridDanhSachTaiSan" ClientInstanceName="gridDanhSachTaiSan" runat="server" Width="100%" EnableTheming="True" Theme="PlasticBlue"
                                    AutoGenerateColumns="False"
                                    OnDataBinding="gridDanhSachTaiSan_DataBinding"
                                    OnCustomCallback="gridDanhSachTaiSan_CustomCallback"
                                    OnPageIndexChanged="gridDanhSachTaiSan_PageIndexChanged">

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Chọn" Width="30px" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <dataitemtemplate>                    
                                                    <div style=" cursor: pointer" onclick="openChonTaiSanNhapKho('<%# Eval("ID") %>','<%# Eval("MaTaiSan") %>','<%# Eval("SoSerialNumber") %>','<%# Eval("CauHinh") %>','','')" title="Chọn">
                                                        <img src="../Images/icon-check.png" />
                                                    </div>                               
                                                 </dataitemtemplate>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                        <dx:GridViewDataTextColumn Caption="Mã tài sản" FieldName="MaTaiSan" VisibleIndex="2" Width="130px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tên tài sản" FieldName="TenTaiSan" VisibleIndex="3" Width="250px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" VisibleIndex="5" Width="50px" />
                                        <dx:GridViewDataTextColumn Caption="Số S/N" FieldName="SoSerialNumber" VisibleIndex="6" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Cấu hình" FieldName="CauHinh" VisibleIndex="7" Width="200px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <%--  <dx:GridViewDataTextColumn Caption="Nhóm tài sản" FieldName="TenNhomTaiSan" VisibleIndex="8" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Loại tài sản" FieldName="TenLoaiTaiSan" VisibleIndex="9" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="Nhà sản xuất" FieldName="TenNhaSanXuat" VisibleIndex="10" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Năm sản xuất" FieldName="NamSanXuat" VisibleIndex="11" Width="100px" />
                                        <dx:GridViewDataTextColumn Caption="Số phiếu bảo hành" FieldName="SoPhieuBaoHanh" VisibleIndex="12" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Số tháng BH" FieldName="SoThangBaoHanh" VisibleIndex="13" Width="50px" />
                                        <dx:GridViewDataTextColumn Caption="Hạn sử dụng(tháng)" FieldName="SoThangHanSuDung" VisibleIndex="14" Width="50px" />
                                        <dx:GridViewDataTextColumn Caption="Tỷ lệ hao mòn" FieldName="TyLeHaoMon" VisibleIndex="15" Width="50px" />
                                        <dx:GridViewDataTextColumn Caption="Nhà cung cấp" FieldName="TenNhaCungCap" VisibleIndex="16" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Năm sử dụng" FieldName="NamDuaVaoSuDung" VisibleIndex="17" Width="100px" />
                                        <dx:GridViewDataTextColumn Caption="Chứng nhận CO/CQ" FieldName="ChungNhanCOCQ" VisibleIndex="18" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" VisibleIndex="19" Width="350px" />--%>
                                    </Columns>
                                    <SettingsPager PageSize="5"></SettingsPager>
                                    <SettingsBehavior AllowSort="True" />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                    <Settings ShowFilterRow="True" />
                                    <Settings ShowHorizontalScrollBar="True" />
                                    <SettingsBehavior ColumnResizeMode="Control" />
                                </dx:ASPxGridView>
                                <%-- <asp:SqlDataSource ID="MyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                    SelectCommand="SP_QLNhapKhoCT_DSTaiSan4ThemNhapKho" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="ssIDNhapKho" Type="String" Name="IDNhapKho" />
                                        <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                    </SelectParameters>

                                </asp:SqlDataSource>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 100px; border: dotted; width: 100%">
                                <div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 100px">Tài sản</td>
                                            <td style="width: 700px" colspan="5">
                                                <dx:ASPxComboBox ID="cbTaiSanNhapKho" ClientInstanceName="cbTaiSanNhapKho" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true">
                                                    <ClientSideEvents ValueChanged="function(s, e) { cbTaiSanNhapKhoValueChanged(); }" />
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                        <dx:ListBoxColumn Caption="Mã" FieldName="MaTaiSan" Name="MaTaiSan" Width="140px" />
                                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenTaiSan" Name="TenTaiSan" Width="200px" />
                                                        <dx:ListBoxColumn Caption="Số S/N" FieldName="SoSerialNumber" Name="SoSerialNumber" Width="100px" />
                                                        <dx:ListBoxColumn Caption="Cấu hình" FieldName="CauHinh" Name="CauHinh" Width="150px" />
                                                        <%-- <dx:ListBoxColumn Caption="NSX" FieldName="TenNhaSanXuat" Name="TenNhaSanXuat" Width="50px" />
                                                        <dx:ListBoxColumn Caption="NCC" FieldName="TenNhaCungCap" Name="TenNhaCungCap" Width="50px" />--%>
                                                    </Columns>
                                                    <ClearButton Visibility="Auto"></ClearButton>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">Mã tài sản</td>
                                            <td style="width: 150px">
                                                <dx:ASPxLabel ID="lbMaTaiSan" ClientInstanceName="lbMaTaiSan" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>
                                            <td style="width: 100px">Số S/N</td>
                                            <td style="width: 150px">
                                                <dx:ASPxLabel ID="lbSoSerialNumber" ClientInstanceName="lbSoSerialNumber" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>
                                            <td style="width: 100px">Cấu hình</td>
                                            <td style="width: 150px">
                                                <dx:ASPxLabel ID="lbCauHinhTaiSan" ClientInstanceName="lbCauHinhTaiSan" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>
                                        </tr>
                                        <%-- <tr>

                                            <td>Nhà cung cấp</td>
                                            <td>
                                                <dx:ASPxLabel ID="lbNhaCungCap" ClientInstanceName="lbNhaCungCap" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>
                                            <td>Nhà sản xuất</td>
                                            <td>
                                                <dx:ASPxLabel ID="lbNhaSanXuat" ClientInstanceName="lbNhaSanXuat" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>
                                            <td>Năm sản xuất</td>
                                            <td>
                                                <dx:ASPxLabel ID="lbNamSanXuat" ClientInstanceName="lbNamSanXuat" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td colspan="6"></td>
                                        </tr>
                                        <tr>
                                            <td>Số lượng</td>
                                            <td colspan="5">
                                                <dx:ASPxSpinEdit ID="txtSoLuong" ClientInstanceName="txtSoLuong" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000" Width="100%" HorizontalAlign="Right" Font-Bold="true">
                                                    <%--  <ClientSideEvents NumberChanged="function(s,e) {OnSoLuongChanged(s); }" />--%>
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <%-- <td>Đơn giá</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txtDonGia" ClientInstanceName="txtDonGia" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000000" Width="100%" HorizontalAlign="Right" Font-Bold="true">
                                                    <ClientSideEvents NumberChanged="function(s,e) {OnDonGiaChanged(s); }" />
                                                </dx:ASPxSpinEdit>

                                            </td>
                                            <td>Thành tiền</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txtThanhTien" ClientInstanceName="txtThanhTien" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000000" Width="100%" HorizontalAlign="Right" Font-Bold="true">
                                                    <ClientSideEvents NumberChanged="function(s,e) {OnThanhTienChanged(s); }" />
                                                </dx:ASPxSpinEdit>
                                            </td>--%>
                                        </tr>
                                        <tr>
                                            <td style="width: 100px">Trạng thái tài sản</td>
                                            <td style="width: 700px" colspan="5">
                                                <dx:ASPxComboBox ID="cbTrangThaiTaiSan" ClientInstanceName="cbTrangThaiTaiSan" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Mã" FieldName="ID" Name="ID" />
                                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenTrangThaiTaiSan" Name="TenTrangThaiTaiSan" />
                                                    </Columns>
                                                    <ClearButton Visibility="Auto"></ClearButton>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Ghi chú</td>
                                            <td colspan="5">
                                                <dx:ASPxTextBox ID="txtGhiChuChiTiet" ClientInstanceName="txtGhiChuChiTiet" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveDetail" ClientInstanceName="btnSaveDetail" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                <ClientSideEvents Click="function(s,e){saveQLNhapKhoChiTiet();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td style="text-align: left">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelSaveDetail" ClientInstanceName="btnCancelSaveDetail" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
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
        <dx:ASPxPopupControl ClientInstanceName="popConfirmDeleteDetail" ID="popConfirmDeleteDetail" Width="400" Height="160px" HeaderText="Xác nhận xóa phân quyền" runat="server"
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
