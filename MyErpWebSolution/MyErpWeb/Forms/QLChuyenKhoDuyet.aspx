<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLChuyenKhoDuyet.aspx.cs" Inherits="WebRunDragon.Forms.QLChuyenKhoDuyet" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxSpellChecker.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxSpellChecker" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/QLChuyenKhoDuyet.js"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    </div>
    <div>
        <dx:ASPxPageControl ID="ASPxTabControl1" runat="server" Theme="PlasticBlue" ActiveTabIndex="0" Width="100%">
            <TabPages>
                <dx:TabPage Text="Chi tiết bàn giao tài sản">
                    <ContentCollection>
                        <dx:ContentControl>

                            <table class="tblPopupUpdateForm" style="width: 1000px" border="0">
                                <tr>
                                    <td>Chi nhánh</td>
                                    <td style="width: 100px" colspan="3">
                                        <dx:ASPxLabel ID="lbChiNhanh" runat="server" ClientInstanceName="lbChiNhanh" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 130px">Ngày chuyển</td>
                                    <td style="width: 270px">
                                        <dx:ASPxLabel ID="lbNgayGiaoDich" runat="server" ClientInstanceName="lbNgayGiaoDich" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                    <td style="width: 130px">Số phiếu</td>
                                    <td style="width: 270px">
                                        <dx:ASPxLabel ID="lbSoPhieuBanGiao" runat="server" ClientInstanceName="lbSoPhieuBanGiao" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Đơn vị xuất</td>
                                    <td>
                                        <dx:ASPxLabel ID="lbDonViGiao" runat="server" ClientInstanceName="lbDonViGiao" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                    <td>Đơn vị nhập</td>
                                    <td>
                                        <dx:ASPxLabel ID="lbDonViNhan" runat="server" ClientInstanceName="lbDonViNhan" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Người xuất</td>
                                    <td>
                                        <dx:ASPxLabel ID="lbNguoiGiao" runat="server" ClientInstanceName="lbNguoiGiao" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                    <td>Người nhập</td>
                                    <td>
                                        <dx:ASPxLabel ID="lbNguoiNhan" runat="server" ClientInstanceName="lbNguoiNhan" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td>Trưởng đơn vị xuất</td>
                                    <td>
                                        <dx:ASPxLabel ID="lbTruongDonViGiao" runat="server" ClientInstanceName="lbTruongDonViGiao" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                    <td>Trưởng đơn vị nhập</td>
                                    <td>
                                        <dx:ASPxLabel ID="lbTruongDonViNhan" runat="server" ClientInstanceName="lbTruongDonViNhan" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                </tr>

                                <tr>
                                    <td>Ghi chú</td>
                                    <td colspan="3">
                                        <dx:ASPxLabel ID="lbGhiChu" runat="server" ClientInstanceName="lbGhiChu" Text="" Font-Bold="true"></dx:ASPxLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4"><u>Danh sách tài sản</u> </td>

                                </tr>


                            </table>
                            <table style="width: 100%">

                                <tr>
                                    <td>
                                        <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                                            EnableTheming="True" Theme="PlasticBlue">
                                            <Columns>
                                                <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                                <dx:GridViewDataTextColumn Caption="Kho xuất" FieldName="TenKhoXuat" Width="150px" VisibleIndex="1">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Mã tài sản" FieldName="MaTaiSan" Width="150px" VisibleIndex="1">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Tài sản" FieldName="TenTaiSan" Width="300px" VisibleIndex="3">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" Width="50px" VisibleIndex="5">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="SL" FieldName="SoLuong" Width="50px" VisibleIndex="6" />
                                                <dx:GridViewDataTextColumn Caption="Trạng thái xuất" FieldName="TrangThaiTSXuatKho" Width="150px" VisibleIndex="7">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Số S/N" FieldName="SoSerialNumber" Width="150px" VisibleIndex="9">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Cấu hình" FieldName="CauHinh" Width="150px" VisibleIndex="10">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Kho nhập" FieldName="TenKhoNhap" Width="120px" VisibleIndex="9">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Trạng thái nhập" FieldName="TrangThaiTSNhapKho" Width="150px" VisibleIndex="10">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>
                                                <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChuChiTiet" Width="200px" VisibleIndex="11">
                                                    <Settings AutoFilterCondition="Contains" />
                                                </dx:GridViewDataTextColumn>

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
                            <table style="width: 900px">
                                <tr>
                                    <td style="width: 150px">Ghi chú duyệt(không duyệt)</td>
                                    <td style="width: 200px" colspan="2">
                                        <dx:ASPxMemo ID="txtGhiChuDuyetKhongDuyet" ClientInstanceName="txtGhiChuDuyetKhongDuyet" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                                    </td>

                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnDuyet" ClientInstanceName="btnDuyet" runat="server" AutoPostBack="False" Text="1.1 Duyệt chuyền và xuất kho">
                                            <ClientSideEvents Click="function(s,e){ApprovalChuyenKho();}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="1.2 Từ chối chuyển kho">
                                            <ClientSideEvents Click="function(s,e){RejectChuyenKho();}" />
                                        </dx:ASPxButton>

                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td colspan="3"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnDuyetNhanTaiSan" ClientInstanceName="btnDuyetNhanTaiSan" runat="server" AutoPostBack="False" Text="2.1 Đồng ý nhập tài sản">
                                            <ClientSideEvents Click="function(s,e){ApprovalNhanTaiSanChuyenKho();}" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyetNhanTaiSan" ClientInstanceName="btnKhongDuyetNhanTaiSan" runat="server" AutoPostBack="False" Text="2.2 Từ chối nhập tài sản">
                                            <ClientSideEvents Click="function(s,e){RejectNhanTaiSanChuyenKho();}" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                                <tr style="height: 20px">
                                    <td colspan="3"></td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td></td>
                                    <td>
                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnNhapKhoTSTraLai" ClientInstanceName="btnNhapKhoTSTraLai" runat="server" AutoPostBack="False" Text="3. Nhập kho tài sản ko nhận">
                                            <ClientSideEvents Click="function(s,e){NhapKhoTaiSanKhongNhanChuyenKho();}" />
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
                                                <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThai" Width="250px" VisibleIndex="3" />
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
            </TabPages>
        </dx:ASPxPageControl>

    </div>
    <dx:ASPxHiddenField runat="server" ID="hddId" ClientInstanceName="hddId" ViewStateMode="Enabled"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ID="hddCanDuyetBanGiao" ClientInstanceName="hddCanDuyetBanGiao" ViewStateMode="Enabled"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ID="hddCanDuyetNhan" ClientInstanceName="hddCanDuyetNhan" ViewStateMode="Enabled"></dx:ASPxHiddenField>
    <dx:ASPxHiddenField runat="server" ID="hddCanNhapKhoTSTraLai" ClientInstanceName="hddCanNhapKhoTSTraLai" ViewStateMode="Enabled"></dx:ASPxHiddenField>
</asp:Content>
