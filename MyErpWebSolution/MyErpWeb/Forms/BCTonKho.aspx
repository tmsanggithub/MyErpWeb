<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="BCTonKho.aspx.cs" Inherits="WebRunDragon.Forms.BCTonKho" %>

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
                        <%-- <td style="width: 81px">Từ khóa</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px"   Theme="PlasticBlue"></dx:ASPxTextBox>
                        </td>--%>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>
                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridTonKho" ClientInstanceName="gridTonKho" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnCustomCallback="gridTonKho_CustomCallback"
                                OnPageIndexChanged="gridTonKho_PageIndexChanged"
                                OnDataBinding="gridTonKho_DataBinding">

                                <Columns>

                                    <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="TenChiNhanh" VisibleIndex="0" Width="150px" />
                                    <dx:GridViewDataTextColumn Caption="Kho" FieldName="TenKho" VisibleIndex="1" Width="120px" />
                                    <dx:GridViewDataTextColumn Caption="Mã tài sản" FieldName="MaTaiSan" VisibleIndex="2" Width="120px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên tài sản" FieldName="TenTaiSan" VisibleIndex="3" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="SL" FieldName="SoLuong" VisibleIndex="5" Width="50px" />
                                    <dx:GridViewDataTextColumn Caption="Giá trị TS" FieldName="DonGiaTaiSan" VisibleIndex="5" Width="80px">
                                        <PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tổng giá trị" FieldName="TongGiaTri" VisibleIndex="5" Width="150px">
                                        <PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" VisibleIndex="5" Width="50px" />
                                    <dx:GridViewDataTextColumn Caption="Tình trạng" FieldName="TrangThaiTS" VisibleIndex="5" Width="150px" />
                                    <dx:GridViewDataTextColumn Caption="Nhóm tài sản" FieldName="TenNhomTaiSan" VisibleIndex="6" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                 
                                    <dx:GridViewDataTextColumn Caption="Số S/N" FieldName="SoSerialNumber" VisibleIndex="8" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                   
                                    <dx:GridViewDataTextColumn Caption="Nhà sản xuất" FieldName="TenNhaSanXuat" VisibleIndex="10" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" VisibleIndex="19" Width="350px" />

                                </Columns>
                                <SettingsPager PageSize="15"></SettingsPager>
                                <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <Settings ShowHorizontalScrollBar="True" ShowGroupPanel="true" ShowFooter="true" ShowGroupFooter="VisibleIfExpanded" />

                                <TotalSummary>
                                    <%--  <dx:ASPxSummaryItem FieldName="TenKho" SummaryType="Count" />--%>
                                    <dx:ASPxSummaryItem FieldName="TongGiaTri" SummaryType="Sum" />
                                </TotalSummary>
                                <GroupSummary>
                                    <%--<dx:ASPxSummaryItem FieldName="TenKho" ShowInGroupFooterColumn="TenKho" SummaryType="Count" />--%>
                                    <dx:ASPxSummaryItem FieldName="TongGiaTri" ShowInGroupFooterColumn="TongGiaTri" SummaryType="Sum" />
                                </GroupSummary>
                            </dx:ASPxGridView>

                            <%--<asp:SqlDataSource ID="MyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="SP_TonKho_Search" SelectCommandType="StoredProcedure">
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
        <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridTonKho">
        </dx:ASPxGridViewExporter>

        <dx:ASPxButton ID="btnExcel" ClientInstanceName="btnExcel" runat="server" Text="Excel" Style="margin-left: 0px" OnClick="btnExcel_Click" Theme="Office2003Blue" />
    </div>
</asp:Content>
