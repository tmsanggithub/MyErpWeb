<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="BCHoatDong.aspx.cs" Inherits="WebRunDragon.Forms.BCHoatDong" ResponseEncoding="utf-8" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .waiting-cursor {
            cursor: wait !important;
        }
    </style>
    <script type="text/javascript">
        function pageLoad() {
            var prm = Sys.WebForms.PageRequestManager.getInstance();
            // Khi bắt đầu PostBack: hiện waiting cursor, disable nút
            prm.add_beginRequest(function () {
                document.body.style.cursor = 'wait';

                btnSearchSum.SetEnabled(false);
            });
            // Khi PostBack xong: khôi phục
            prm.add_endRequest(function () {
                document.body.style.cursor = 'default';

                btnSearchSum.SetEnabled(true);
            });
        }

        function OpenAdd(id, ma, ten, price) {
            // perform callback to add item to right grid
            try {
                var param = 'ADD|' + id + '|' + ma + '|' + ten + '|' + price;
                if (gridHoatDongRight && gridHoatDongRight.PerformCallback) {
                    gridHoatDongRight.PerformCallback(param);
                }
            } catch (e) {
                console.error(e);
            }
        }

        function DeleteRightRow(index) {
            try {
                if (gridHoatDongRight && gridHoatDongRight.PerformCallback) {
                    gridHoatDongRight.PerformCallback('DEL|' + index);
                }
            } catch (e) { console.error(e); }
        }

        function UpdateRightCell(visibleIndex, field, value) {
            try {
                // get other value from DOM inputs
                var so = document.getElementById('so_' + visibleIndex);
                var dg = document.getElementById('dg_' + visibleIndex);
                var soVal = so ? so.value : '';
                var dgVal = dg ? dg.value : '';
                if (field === 'so_luong') soVal = value;
                if (field === 'don_gia') dgVal = value;

                // normalize numbers
                soVal = soVal.replace(/[^0-9\.\,]/g, '').replace(',', '.');
                dgVal = dgVal.replace(/[^0-9\.\,]/g, '').replace(',', '.');

                if (gridHoatDongRight && gridHoatDongRight.PerformCallback) {
                    var param = 'UPD|' + visibleIndex + '|' + soVal + '|' + dgVal;
                    gridHoatDongRight.PerformCallback(param);
                }
            } catch (e) { console.error(e); }
        }
    </script>
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
                            <dx:ASPxGridView ID="gridHoatDong" ClientInstanceName="gridHoatDong" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnCustomCallback="gridHoatDong_CustomCallback"
                                OnPageIndexChanged="gridHoatDong_PageIndexChanged"
                                OnDataBinding="gridHoatDong_DataBinding">

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
                                <dx:ASPxComboBox ID="cbKhachHang" ClientInstanceName="cbKhachHang" runat="server" ValueField="id" TextFormatString="{1}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="false" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </div>
                            <div style="margin-left: 8px;">
                                <dx:ASPxButton ID="btnAddCustomer" ClientInstanceName="btnAddCustomer" runat="server" Text="Thêm" Theme="Office2003Blue" />
                            </div>
                        </div>


                        <label style="font-size: 13px">Hàng hóa:</label>
                        <!-- detail grid for selected items -->
                        <div style="overflow: auto; border: 1px solid #ddd; padding: 6px; background: #fafafa;">
                            <dx:ASPxGridView ID="ASPxGridViewRight" ClientInstanceName="gridHoatDongRight" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
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
                                            <input id="so_<%# Container.VisibleIndex %>" type="text" value="<%# Eval("so_luong") %>" style="width: 90%; text-align: right;" onchange="UpdateRightCell('<%# Container.VisibleIndex %>','so_luong', this.value);" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Giá bán" VisibleIndex="5" Width="17.5%">
                                        <DataItemTemplate>
                                            <input id="dg_<%# Container.VisibleIndex %>" type="text" value="<%# Eval("don_gia") %>" style="width: 90%; text-align: right;" onchange="UpdateRightCell('<%# Container.VisibleIndex %>','don_gia', this.value);" />
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Thành tiền" FieldName="thanh_tien" VisibleIndex="6" Width="17.5%">
                                        <DataItemTemplate>
                                            <span id="tt_<%# Container.VisibleIndex %>"><%# Eval("thanh_tien") %></span>
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
                            <dx:ASPxButton ID="btnSaveTemp" ClientInstanceName="btnSaveTemp" runat="server" Text="Lưu tạm" Theme="Office2003Blue" OnClick="btnSaveTemp_Click" />
                            &nbsp;
                            <dx:ASPxButton ID="btnPay" ClientInstanceName="btnPay" runat="server" Text="Thanh toán" Theme="Office2003Blue" OnClick="btnPay_Click" />
                        </div>
                    </div>
                </div>

                <dx:ASPxGridViewExporter ID="gridExporter" runat="server" GridViewID="gridHoatDong">
                </dx:ASPxGridViewExporter>

                <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>



