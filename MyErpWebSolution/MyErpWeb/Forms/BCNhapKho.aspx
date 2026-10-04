<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="BCNhapKho.aspx.cs" Inherits="WebRunDragon.Forms.BCNhapKho" %>

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

                <table style="width: 800px;">
                    <tr>
                        <td style="width: 100px">Từ ngày</td>
                        <td style="width: 80px">
                            <dx:ASPxDateEdit ID="deNgayNhapTu" ClientInstanceName="deNgayNhapTu" runat="server"></dx:ASPxDateEdit>
                        </td>
                        <td style="width: 100px">Chi nhánh</td>
                        <td style="width: 200px">
                            <dx:ASPxComboBox ID="cbMaChiNhanh" ClientInstanceName="cbMaChiNhanh" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                <Columns>
                                    <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                    <dx:ListBoxColumn Caption="Mã" FieldName="Code" Name="Code" />
                                    <dx:ListBoxColumn Caption="Tên" FieldName="BranchName" Name="BranchName" />
                                </Columns>
                                <ClearButton Visibility="Auto"></ClearButton>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 100px">Tài sản</td>
                        <td style="width: 200px">
                            <dx:ASPxComboBox ID="cbTaiSanNhapKho" ClientInstanceName="cbTaiSanNhapKho" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true">
                                <Columns>
                                    <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                    <dx:ListBoxColumn Caption="Mã" FieldName="MaTaiSan" Name="MaTaiSan" Width="100px" />
                                    <dx:ListBoxColumn Caption="Tên" FieldName="TenTaiSan" Name="TenTaiSan" Width="200px" />
                                    <dx:ListBoxColumn Caption="Số S/N" FieldName="SoSerialNumber" Name="SoSerialNumber" Width="100px" />
                                    <dx:ListBoxColumn Caption="Cấu hình" FieldName="CauHinh" Name="CauHinh" Width="150px" />
                                    <dx:ListBoxColumn Caption="NSX" FieldName="TenNhaSanXuat" Name="TenNhaSanXuat" Width="50px" />
                                    <dx:ListBoxColumn Caption="NCC" FieldName="TenNhaCungCap" Name="TenNhaCungCap" Width="50px" />
                                </Columns>
                                <ClearButton Visibility="Auto"></ClearButton>
                            </dx:ASPxComboBox>
                        </td>
                        <td style="width: 100px"></td>
                    </tr>
                    <tr>
                        <td>Đến ngày</td>
                        <td>
                            <dx:ASPxDateEdit ID="deNgayNhapDen" ClientInstanceName="deNgayNhapDen" runat="server"></dx:ASPxDateEdit>
                        </td>
                        <td>Kho nhập</td>
                        <td>
                            <dx:ASPxComboBox ID="cbKhoNhap" ClientInstanceName="cbKhoNhap" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                <Columns>
                                    <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                    <dx:ListBoxColumn Caption="Mã" FieldName="MaKho" Name="MaKho" />
                                    <dx:ListBoxColumn Caption="Tên" FieldName="TenKho" Name="TenKho" />
                                </Columns>
                                <ClearButton Visibility="Auto"></ClearButton>
                            </dx:ASPxComboBox>
                        </td>
                        <td>Số phiếu</td>
                        <td>
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtSoPhieuNhapKho" ClientInstanceName="txtSoPhieuNhapKho" Theme="PlasticBlue" Width="100%"></dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>
                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>

                            <dx:ASPxGridView ID="gridNhapKho" ClientInstanceName="gridNhapKho" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                DataSourceID="MyDataSource"
                                OnCustomCallback="gridNhapKho_CustomCallback"
                                OnPageIndexChanged="gridNhapKho_PageIndexChanged">

                                <Columns>

                                    <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="TenChiNhanh" VisibleIndex="0" Width="120px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Kho nhập" FieldName="TenKho" VisibleIndex="1" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ngày nhập" FieldName="NgayPhieu" VisibleIndex="2" Width="100px" />
                                    <dx:GridViewDataTextColumn Caption="Số phiếu nhập" FieldName="SoPhieu" VisibleIndex="2" Width="130px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhóm tài sản" FieldName="TenNhomTaiSan" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã tài sản" FieldName="MaTaiSan" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên tài sản" FieldName="TenTaiSan" VisibleIndex="3" Width="200px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" VisibleIndex="5" Width="90px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="SL" FieldName="SoLuong" VisibleIndex="5" Width="60px" />
                                    <dx:GridViewDataTextColumn Caption="Giá trị" FieldName="GiaTriTaiSan" VisibleIndex="5" Width="150px" />
                                    <dx:GridViewDataTextColumn Caption="Thành tiền" FieldName="ThanhTien" VisibleIndex="5" Width="150px" />
                                    <dx:GridViewDataTextColumn Caption="Thời gian mua" FieldName="ThoiGianMua" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Số S/N" FieldName="SoSerialNumber" VisibleIndex="8" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Cấu hình" FieldName="CauHinh" VisibleIndex="9" Width="200px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhà cung cấp" FieldName="TenNhaCungCap" VisibleIndex="16" Width="190px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhà sản xuất" FieldName="TenNhaSanXuat" VisibleIndex="16" Width="190px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Số tháng BH" FieldName="SoThangBaoHanh" VisibleIndex="17" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>


                                    <%--
                                    <dx:GridViewDataTextColumn Caption="Tình trạng" FieldName="TenTrangThaiTaiSan" VisibleIndex="5" Width="150px" />
                                    <dx:GridViewDataTextColumn Caption="Loại tài sản" FieldName="TenLoaiTaiSan" VisibleIndex="7" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>                               
                                    <dx:GridViewDataTextColumn Caption="Năm sản xuất" FieldName="NamSanXuat" VisibleIndex="11" Width="100px" />
                                    <dx:GridViewDataTextColumn Caption="Số phiếu bảo hành" FieldName="SoPhieuBaoHanh" VisibleIndex="12" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                  
                                    <dx:GridViewDataTextColumn Caption="Hạn sử dụng(tháng)" FieldName="SoThangHanSuDung" VisibleIndex="14" Width="50px" />
                                    <dx:GridViewDataTextColumn Caption="Tỷ lệ hao mòn" FieldName="TyLeHaoMon" VisibleIndex="15" Width="50px" />
                                   
                                    <dx:GridViewDataTextColumn Caption="Năm sử dụng" FieldName="NamDuaVaoSuDung" VisibleIndex="17" Width="100px" />
                                    <dx:GridViewDataTextColumn Caption="Chứng nhận CO/CQ" FieldName="ChungNhanCOCQ" VisibleIndex="18" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    --%>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" VisibleIndex="19" Width="350px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                </Columns>
                                <SettingsPager PageSize="9"></SettingsPager>
                                <SettingsBehavior AllowSort="True" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <Settings ShowHorizontalScrollBar="True" />
                                <SettingsBehavior ColumnResizeMode="Control" />
                            </dx:ASPxGridView>
                            <asp:SqlDataSource ID="MyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="SP_BC_NhapKho" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter SessionField="ssTuNgay" Type="DateTime" Name="TuNgay" />
                                    <asp:SessionParameter SessionField="ssDenNgay" Type="DateTime" Name="DenNgay" />
                                    <asp:SessionParameter SessionField="ssIDChiNhanh" Type="Int32" Name="IDChiNhanh" />
                                    <asp:SessionParameter SessionField="ssIDKho" Type="Int32" Name="IDKho" />
                                    <asp:SessionParameter SessionField="ssIDTaiSan" Type="Int32" Name="IDTaiSan" />
                                    <asp:SessionParameter SessionField="ssSoPhieuNhapKho" Type="String" Name="SoPhieuNhapKho" />
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
        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridNhapKho">
        </dx:ASPxGridViewExporter>

        <dx:ASPxButton ID="btnExcel" ClientInstanceName="btnExcel" runat="server" Text="Excel" Style="margin-left: 0px" OnClick="btnExcel_Click" Theme="Office2003Blue" />
    </div>
</asp:Content>

