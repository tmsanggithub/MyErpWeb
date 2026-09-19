<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLDonHangBanCT.aspx.cs" Inherits="WebRunDragon.Forms.QLDonHangBanCT" ResponseEncoding="utf-8" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/QLDonHangBan.js" type="text/javascript"></script>
    <script src="../Scripts/QLDonHangBanCT.js" type="text/javascript"></script>
    <style>
        .waiting-cursor {
            cursor: wait !important;
        }
    </style>

</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style>
        html, body, form {
            height: 100%;
            margin: 0;
            padding: 0;
        }

        .page-root {
            display: flex;
            flex-direction: column;
            height: 100vh; /* fill viewport */
            box-sizing: border-box;
        }

        .page-main {
            display: flex;
            flex: 1 1 auto;
            overflow: hidden;
        }

        .left-pane {
            flex: 0 0 40%;
            padding: 10px;
            box-sizing: border-box;
            overflow: auto;
        }

        .right-pane {
            flex: 0 0 60%;
            padding: 10px;
            box-sizing: border-box;
            border-left: 1px solid #e0e0e0;
            overflow: auto;
            background: #fff;
        }

        .toolbar {
            padding: 8px 10px;
            border-bottom: 1px solid #eaeaea;
            background: #f9f9f9;
        }

        .grid-full-height {
            height: calc(100% - 10px);
            width: 100%;
            box-sizing: border-box;
        }
    </style>

    <div>
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    </div>

    <!-- Add Customer Popup -->
    <div id="popupAddCustomer" style="display: none; position: fixed; left: 0; top: 0; right: 0; bottom: 0; background: rgba(0,0,0,0.4); z-index: 9999;">
        <div style="width: 420px; margin: 80px auto; background: #fff; padding: 16px; border-radius: 4px; box-shadow: 0 2px 8px rgba(0,0,0,0.2);">
            <h3 style="margin-top: 0;">Thêm khách hàng</h3>
            <div style="margin-bottom: 8px;">
                <label>Số điện thoại</label><br />
                <input id="cust_so_dien_thoai" type="text" style="width: 100%" />
            </div>
            <div style="margin-bottom: 8px;">
                <label>Tên khách hàng</label><br />
                <input id="cust_ten" type="text" style="width: 100%" />
            </div>
            <div style="margin-bottom: 8px;">
                <label>Email</label><br />
                <input id="cust_email" type="text" style="width: 100%" />
            </div>
            <div style="margin-bottom: 8px;">
                <label>Địa chỉ</label><br />
                <input id="cust_dia_chi" type="text" style="width: 100%" />
            </div>
            <div style="margin-bottom: 8px;">
                <label>Phường/Xã</label><br />
                <input id="cust_phuong_xa" type="text" style="width: 100%" />
            </div>
            <div style="margin-bottom: 8px;">
                <label>Tỉnh/Thành phố</label><br />
                <input id="cust_tinh_thanh" type="text" style="width: 100%" />
            </div>
            <div style="margin-bottom: 12px;">
                <label>Ghi chú</label><br />
                <input id="cust_ghi_chu" type="text" style="width: 100%" />
            </div>
            <div style="text-align: right;">
                <button type="button" onclick="closeAddCustomer();">Hủy</button>
                &nbsp;
                                    <button type="button" onclick="saveAddCustomer();">Lưu</button>
            </div>
        </div>
    </div>

    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
        <ContentTemplate>
            <div class="page-root">


                <div class="page-main">
                    <div class="left-pane">
                        <div style="margin-bottom: 6px;">



                            <label style="font-size: 13px">Chọn Hàng hóa:</label>
                        </div>

                        <!-- small header grid -->
                        <div style="overflow: auto; border: 1px solid #ddd; padding: 6px; background: #fafafa;">
                            <dx:ASPxGridView ID="gridHangHoa" ClientInstanceName="gridHangHoa" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnCustomCallback="gridHangHoa_CustomCallback"
                                OnPageIndexChanged="gridHangHoa_PageIndexChanged"
                                OnDataBinding="gridHangHoa_DataBinding">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="1" Visible="false" Width="120px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã" FieldName="ma_hang_hoa" VisibleIndex="2" Width="120px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên" FieldName="ten_hang_hoa" VisibleIndex="3" Width="200px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên" FieldName="don_gia" VisibleIndex="3" Visible="false" Width="200px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Hình ảnh" FieldName="image_url" VisibleIndex="4" Width="120px">
                                        <DataItemTemplate>
                                            <img src='<%# Eval("image_url") %>' style="max-width: 100px; max-height: 60px;" alt="Hình" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Chọn" VisibleIndex="5" Width="35px">
                                        <DataItemTemplate>
                                            <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="OpenAdd('<%# Eval("id") %>','<%# Eval("ma_hang_hoa") %>','<%# Eval("ten_hang_hoa") %>','<%# Eval("don_gia") %>');" title="Chọn">
                                                <img src="../Images/icon-add.png" />
                                            </div>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager PageSize="10"></SettingsPager>
                                <Settings ShowFilterRow="True" />
                            </dx:ASPxGridView>
                        </div>

                    </div>

                    <div class="right-pane">
                        <div style="display: flex; align-items: center; margin-bottom: 6px;">
                            <div style="flex: 1">


                                <label style="font-size: 13px">Khách hàng:</label>
                                <dx:ASPxComboBox ID="cbKhachHang" ClientInstanceName="cbKhachHang" runat="server" ValueField="id" TextField="TenKhachHang" TextFormatString="{1}" Style="width: 100%" OnCallback="cbKhachHang_Callback" EnableCallbackMode="True" IncrementalFilteringMode="Contains">
                                   
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="false" />
                                        <dx:ListBoxColumn Caption="Số điện thoại" FieldName="SoDienThoai" Name="SoDienThoai" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenKhachHang" Name="TenKhachHang" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </div>

                            <div style="display: <%=GetRight(1)%>; float: right; margin-top: 10px; cursor: pointer; width: 45px;"
                                onclick="openAddCustomer()" title="Thêm mới">
                                <img src="../Images/icon-add.png" />&nbsp;
                            </div>
                        </div>


                        <label style="font-size: 13px">Hàng hóa:</label>
                        <!-- detail grid for selected items -->
                        <div style="overflow: auto; border: 1px solid #ddd; padding: 6px; background: #fafafa;">
                            <dx:ASPxGridView ID="ASPxGridViewRight" ClientInstanceName="gridHangHoaRight" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue" KeyFieldName="id"
                                OnCustomCallback="ASPxGridViewRight_CustomCallback"
                                OnPageIndexChanged="ASPxGridViewRight_PageIndexChanged"
                                OnDataBinding="ASPxGridViewRight_DataBinding">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="50px">
                                        <DataItemTemplate>
                                            <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="DeleteRightRow('<%# Container.VisibleIndex %>');" title="Xóa">
                                                <img src="../Images/icon-delete.png" />
                                            </div>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="1" Visible="false">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã" FieldName="ma_hang_hoa" VisibleIndex="2" Width="15%">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên" FieldName="ten_hang_hoa" VisibleIndex="3" Width="40%">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Số lượng" VisibleIndex="4" Width="10%">
                                        <DataItemTemplate>
                                            <input id="so_<%# Container.VisibleIndex %>" type="text" value="<%# Eval("so_luong", "{0:N0}") %>" style="width: 90%; text-align: right;" onchange="UpdateRightCell('<%# Container.VisibleIndex %>','so_luong', this.value);" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Giá bán" VisibleIndex="5" Width="17.5%">
                                        <DataItemTemplate>
                                            <input id="dg_<%# Container.VisibleIndex %>" type="text" value="<%# Eval("don_gia", "{0:N0}") %>" style="width: 90%; text-align: right;" onchange="UpdateRightCell('<%# Container.VisibleIndex %>','don_gia', this.value);" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Thành tiền" FieldName="thanh_tien" VisibleIndex="6" Width="17.5%">
                                        <DataItemTemplate>
                                            <span id="tt_<%# Container.VisibleIndex %>"><%# Eval("thanh_tien", "{0:N0}") %></span>
                                        </DataItemTemplate>
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager PageSize="10"></SettingsPager>
                                <Settings ShowFilterRow="false" ShowFooter="True" ShowHorizontalScrollBar="True" />
                                <TotalSummary>
                                    <dx:ASPxSummaryItem FieldName="thanh_tien" SummaryType="Sum" DisplayFormat="{0:N0}" />
                                </TotalSummary>
                            </dx:ASPxGridView>
                        </div>

                        <div style="margin-top: 10px;">
                            <label style="font-size: 13px">Ghi chú:</label>
                            <dx:ASPxMemo ID="memoNotes" ClientInstanceName="memoNotes" runat="server" Width="100%" Height="30px"></dx:ASPxMemo>
                        </div>

                        <div style="text-align: center; margin-top: 18px;">
                            <dx:ASPxButton ID="btnPrint" ClientInstanceName="btnPrint" runat="server" Text="In" Theme="Office2003Blue" />
                            &nbsp;
                            <dx:ASPxButton ID="btnSaveTemp" ClientInstanceName="btnSaveTemp" runat="server" Text="Lưu" Theme="Office2003Blue">
                                <ClientSideEvents Click="function(s,e){ SaveTempCall(); }" />
                            </dx:ASPxButton>
                            &nbsp;
                            <dx:ASPxButton ID="btnPay" ClientInstanceName="btnPay" runat="server" Text="Lưu và gửi duyệt" Theme="Office2003Blue" OnClick="btnPay_Click" />
                        </div>
                    </div>
                </div>

                <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridHangHoa">
                </dx:ASPxGridViewExporter>

                <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>
                <dx:ASPxHiddenField runat="server" ID="hddCustomerId" ClientInstanceName="hddCustomerId"></dx:ASPxHiddenField>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>



