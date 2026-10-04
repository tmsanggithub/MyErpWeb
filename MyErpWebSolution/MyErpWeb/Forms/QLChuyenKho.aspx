<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLChuyenKho.aspx.cs" Inherits="WebRunDragon.Forms.QLChuyenKho" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script>
        var branchID = '<%=GetBranchID()%>';
        var userLoginID = '<%=GetUserLogin()%>';
        var userLoginDept = '<%=GetDepartmentOfUserLogin()%>';
        var userManagerId = '<%=GetManagerOfUserLogin()%>';
    </script>
    <script src="../Scripts/QLChuyenKho.js" type="text/javascript"></script>
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
                        <td style="width: 281px">Trạng thái, hoặc Người xuất, hoặc Đơn vị nhập</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px" Theme="PlasticBlue" ToolTip="Trạng thái, hoặc Người nhập, hoặc Đơn vị nhập"></dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px"
                                OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>

                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridQLChuyenKho" ClientInstanceName="gridQLChuyenKho" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnDataBinding="gridTaiSan_DataBinding"
                                OnCustomCallback="gridQLChuyenKho_CustomCallback"
                                OnPageIndexChanged="gridQLChuyenKho_PageIndexChanged">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(0) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenTrangThai") %>','ReadOnly')" title="Xem thông tin">
                                                 <img src="../Images/icon-view.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(2) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenTrangThai") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                   <img src="../Images/icon-edit.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("ID") %>')" title="Xóa">
                                                   <img src="../Images/icon-delete.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Duyệt" VisibleIndex="0" Width="40px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style="display: <%#GetRight(5) %>; cursor: pointer" title="Duyệt">
                                                    <a target="_blank" href="QLChuyenKhoDuyet.aspx?id=<%#Eval("ID") %>">
                                                    <img src="../Images/icon-approval.png" />
                                                </div>                                    
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <%-- <dx:GridViewDataTextColumn Caption="" VisibleIndex="0">
                                        <DataItemTemplate>
                                             <div style=" display: <%#GetRight(5) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenTrangThai") %>','ApprovalOnly')" title="Duyệt">
                                                   <img src="../Images/icon-approval.png" />
                                                </div> 
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>--%>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Width="40px" />
                                    <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TenTrangThai" VisibleIndex="1" Width="120px" FixedStyle="Left">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ngày chuyển" FieldName="NgayChuyen" VisibleIndex="2" Width="100px" FixedStyle="Left">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="TenChiNhanh" VisibleIndex="3" Width="130px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Người xuất" FieldName="NguoiChuyen" VisibleIndex="5" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Đơn vị xuất" FieldName="DonViChuyen" VisibleIndex="6" Width="180px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trưởng đơn vị xuất" FieldName="TruongDonViChuyen" VisibleIndex="7" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Người nhập" FieldName="NguoiNhan" VisibleIndex="7" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Đơn vị nhập" FieldName="DonViNhan" VisibleIndex="8" Width="180px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trưởng đơn vị nhập" FieldName="TruongDonViNhan" VisibleIndex="9" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" VisibleIndex="9" Width="450px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>


                                </Columns>
                                <SettingsPager PageSize="20"></SettingsPager>
                                <SettingsBehavior AllowSort="True" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <SettingsBehavior ColumnResizeMode="Control" />
                                <Settings ShowHorizontalScrollBar="True" />

                            </dx:ASPxGridView>
                            <%--  <asp:SqlDataSource ID="SqlDataSourceChuyenKho" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="SP_QLChuyenKho_Search" SelectCommandType="StoredProcedure">
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

        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="1000px" Height="650px"
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
                            <dx:TabPage Text="Chi tiết bàn xuất tài sản">
                                <ContentCollection>
                                    <dx:ContentControl>

                                        <table class="tblPopupUpdateForm" style="width: 100%" border="0">
                                            <tr>
                                                <td>Chi nhánh</td>
                                                <td colspan="3">
                                                    <dx:ASPxComboBox ID="cbMaChiNhanh" ClientInstanceName="cbMaChiNhanh" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnChiNhanhChanged(s,e); }" />
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
                                                <td style="width: 130px">Ngày xuất</td>
                                                <td style="width: 270px">
                                                    <dx:ASPxDateEdit ID="deNgayChuyen" ClientInstanceName="deNgayChuyen" runat="server" Width="100%"></dx:ASPxDateEdit>
                                                </td>
                                                <td style="width: 130px">Số phiếu xuất</td>
                                                <td style="width: 270px">
                                                    <dx:ASPxTextBox ID="txtSoPhieuChuyen" ClientInstanceName="txtSoPhieuChuyen" runat="server" Width="100%" ReadOnly="true"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Đơn vị xuất</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbPhongBanChuyen" ClientInstanceName="cbPhongBanChuyen" runat="server" ValueField="ID" TextFormatString="{1}" ReadOnly="true" Style="width: 100%">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnPhongBanChuyenChanged(s,e); }" />
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="Code" Name="Code" Width="100px" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="DeptName" Name="DeptName" Width="200px" />
                                                            <dx:ListBoxColumn Caption="Trưởng đv" FieldName="ManagerId" Name="ManagerId" Width="100px" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>Đơn vị nhập</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbPhongBanNhan" ClientInstanceName="cbPhongBanNhan" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%"
                                                        OnCallback="cbPhongBanNhan_OnCallback">
                                                        <ClientSideEvents SelectedIndexChanged="function(s, e) { OnPhongBanNhanChanged(s,e); }" />
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="Code" Name="Code" Width="100px" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="DeptName" Name="DeptName" Width="200px" />
                                                            <dx:ListBoxColumn Caption="Trưởng đv" FieldName="ManagerId" Name="ManagerId" Width="100px" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Người xuất</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbNhanVienChuyen" ClientInstanceName="cbNhanVienChuyen" runat="server" ValueField="UserName" ReadOnly="true" TextFormatString="{1}" Style="width: 100%"
                                                        OnCallback="cbNhanVienChuyen_OnCallback">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="UserName" Name="UserName" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="StaffName" Name="StaffName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>Người nhập</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbNhanVienNhan" ClientInstanceName="cbNhanVienNhan" runat="server" ValueField="StaffNo" TextFormatString="{1}" Style="width: 100%"
                                                        OnCallback="cbNhanVienNhan_OnCallback">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="StaffNo" Name="StaffNo" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="StaffName" Name="StaffName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Trưởng đơn vị xuất</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbTruongDonViChuyen" ClientInstanceName="cbTruongDonViChuyen" runat="server" ValueField="UserName" ReadOnly="true" TextFormatString="{1}" Style="width: 100%"
                                                        OnCallback="cbTruongDonViChuyen_OnCallback">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="UserName" Name="UserName" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="StaffName" Name="StaffName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>Trưởng đơn vị nhập</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbTruongDonViNhan" ClientInstanceName="cbTruongDonViNhan" runat="server" ValueField="UserName" TextFormatString="{1}" Style="width: 100%"
                                                        OnCallback="cbTruongDonViNhan_OnCallback">
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
                                                    <dx:ASPxMemo ID="txtGhiChu" ClientInstanceName="txtGhiChu" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                                                        EnableTheming="True" Theme="PlasticBlue" OnCustomCallback="gridDetails_CustomCallback" OnPageIndexChanged="gridDetails_PageIndexChanged">
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
                                                            <dx:GridViewDataTextColumn Caption="Kho xuất" FieldName="TenKhoXuat" Width="120px" VisibleIndex="1" />
                                                            <dx:GridViewDataTextColumn Caption="Mã tài sản" FieldName="MaTaiSan" Width="120px" VisibleIndex="2" />
                                                            <dx:GridViewDataTextColumn Caption="Tài sản" FieldName="TenTaiSan" Width="300px" VisibleIndex="3" />
                                                            <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" Width="50px" VisibleIndex="5" />
                                                            <dx:GridViewDataTextColumn Caption="SL" FieldName="SoLuong" Width="50px" VisibleIndex="6" />
                                                            <dx:GridViewDataTextColumn Caption="Trạng thái xuất" FieldName="TrangThaiTSXuatKho" Width="150px" VisibleIndex="7" />
                                                            <dx:GridViewDataTextColumn Caption="Kho nhập" FieldName="TenKhoNhap" Width="120px" VisibleIndex="9" />
                                                            <dx:GridViewDataTextColumn Caption="Trạng thái nhập" FieldName="TrangThaiTSNhapKho" Width="150px" VisibleIndex="10" />
                                                            <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChuChiTiet" Width="200px" VisibleIndex="11" />

                                                        </Columns>
                                                        <SettingsPager PageSize="10"></SettingsPager>
                                                        <Settings ShowFooter="False" />
                                                        <SettingsBehavior AllowSort="False" />
                                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                                        <Settings ShowHorizontalScrollBar="True" />
                                                    </dx:ASPxGridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                                    <ClientSideEvents Click="function(s,e){saveQLChuyenKho();}" />
                                                                </dx:ASPxButton>

                                                            </td>
                                                            <td style="width: 20px;">&nbsp;</td>

                                                            <td>
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyet" ClientInstanceName="btnGuiDuyet" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                                    <ClientSideEvents Click="function(s,e){SendApprovalQLChuyenKho();}" />
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
                            <dx:TabPage Text="Lịch sử gửi và ký duyệt">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table style="width: 100%">
                                            <%-- <tr>
                                                <td style="width: 150px">Ghi chú duyệt(không duyệt)</td>
                                                <td style="width: 200px" colspan="2">
                                                    <dx:ASPxMemo ID="txtGhiChuDuyetKhongDuyet" ClientInstanceName="txtGhiChuDuyetKhongDuyet" runat="server" TextMode="MultiLine" Width="100%" Height="45px"></dx:ASPxMemo>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnDuyet" ClientInstanceName="btnDuyet" runat="server" AutoPostBack="False" Text="Duyệt bàn xuất và xuất kho">
                                                        <ClientSideEvents Click="function(s,e){ApprovalChuyenKho();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="Từ chối phiếu bàn xuất">
                                                        <ClientSideEvents Click="function(s,e){RejectChuyenKho();}" />
                                                    </dx:ASPxButton>

                                                </td>
                                            </tr>
                                            <tr style="height: 20px">
                                                <td colspan="4"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnDuyetNhanTaiSan" ClientInstanceName="btnDuyetNhanTaiSan" runat="server" AutoPostBack="False" Text="Đồng ý nhập tài sản">
                                                        <ClientSideEvents Click="function(s,e){ApprovalNhanTaiSanBanxuất();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyetNhanTaiSan" ClientInstanceName="btnKhongDuyetNhanTaiSan" runat="server" AutoPostBack="False" Text="Từ chối nhập tài sản">
                                                        <ClientSideEvents Click="function(s,e){RejectNhanTaiSanBanxuất();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr style="height: 20px">
                                                <td colspan="4"></td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnNhapKhoTSTraLai" ClientInstanceName="btnNhapKhoTSTraLai" runat="server" AutoPostBack="False" Text="Nhập kho tài sản trả lại">
                                                        <ClientSideEvents Click="function(s,e){NhapKhoTaiSanKhongNhanBanxuất();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 20px" colspan="3"></td>
                                            </tr>--%>
                                            <tr>
                                                <td>Lịch sử gửi và ký duyệt
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>
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
                                                            <dx:GridViewDataTextColumn Caption="Nhân sự nhập" FieldName="UserNext" Width="200px" VisibleIndex="10" />
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
        <dx:ASPxPopupControl ClientInstanceName="popUpdateDetail" ID="popUpdateDetail" Width="750px" Height="500px" HeaderText="Cập nhật chi tiết " runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td>Kho tra cứu</td>
                            <td>
                                <dx:ASPxComboBox ID="cbKhoTraCuu" ClientInstanceName="cbKhoTraCuu" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%"
                                    OnCallback="cbKhoTraCuu_OnCallback">
                                    <ClientSideEvents ValueChanged="function(s, e) { cbKhoTraCuuValueChanged(); }" />
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
                            <td colspan="2">
                                <%--DataSourceID="MyDataSource"--%>
                                <dx:ASPxGridView ID="gridDanhSachTaiSan" ClientInstanceName="gridDanhSachTaiSan" runat="server" Width="100%" EnableTheming="True" Theme="PlasticBlue"
                                    AutoGenerateColumns="False"
                                    OnDataBinding="gridDanhSachTaiSan_DataBinding"
                                    OnCustomCallback="gridDanhSachTaiSan_CustomCallback"
                                    OnPageIndexChanged="gridDanhSachTaiSan_PageIndexChanged">

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Chọn" Width="30px" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <dataitemtemplate>                    
                                                    <div style=" cursor: pointer" onclick="openChonTaiSanChuyenKho('<%# Eval("ID") %>','<%# Eval("MaTaiSan") %>','<%# Eval("SoSerialNumber") %>','','','','<%# Eval("IDKho") %>','<%# Eval("TrangThaiTaiSan") %>')" title="Chọn">
                                                        <img src="../Images/icon-check.png" />
                                                    </div>                               
                                                 </dataitemtemplate>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                        <dx:GridViewDataTextColumn Caption="Mã tài sản" FieldName="MaTaiSan" VisibleIndex="2" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tên tài sản" FieldName="TenTaiSan" VisibleIndex="3" Width="250px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="ĐVT" FieldName="TenDonViTinh" VisibleIndex="5" Width="50px" />
                                        <dx:GridViewDataTextColumn Caption="Trạng tái TS" FieldName="TrangThaiTSTonKho" VisibleIndex="6" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="SL tồn" FieldName="SoLuongTon" VisibleIndex="7" Width="80px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Số S/N" FieldName="SoSerialNumber" VisibleIndex="8" Width="100px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Giá trị tài sản" FieldName="GiaTriTaiSan" VisibleIndex="9" Width="100px">
                                            <Settings AutoFilterCondition="Contains" />
                                            <PropertiesTextEdit DisplayFormatString="n0"></PropertiesTextEdit>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="TrangThaiTaiSan" FieldName="TrangThaiTaiSan" VisibleIndex="10" Width="150px" Visible="false">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="IDKho" FieldName="IDKho" VisibleIndex="11" Width="100px" Visible="false" />
                                        <dx:GridViewDataTextColumn Caption="GiaTriTaiSan" FieldName="GiaTriTaiSan" VisibleIndex="12" Width="150px" Visible="false" />

                                    </Columns>
                                    <SettingsPager PageSize="5"></SettingsPager>
                                    <SettingsBehavior AllowSort="True" />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                    <Settings ShowFilterRow="True" />
                                    <Settings ShowHorizontalScrollBar="True" />
                                    <SettingsBehavior ColumnResizeMode="Control" />
                                </dx:ASPxGridView>
                                <%--<asp:SqlDataSource ID="MyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                    SelectCommand="SP_QLTonKho_TraCuu4Fillter" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="ssIDKhoTraCuu" Type="String" Name="IDKhoTraCuu" />
                                        <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                    </SelectParameters>

                                </asp:SqlDataSource>--%>
                            </td>
                        </tr>
                        <tr>
                            <td style="height: 100px; border: dotted; width: 100%" colspan="2">
                                <div>
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 150px">Kho xuất</td>
                                            <td style="width: 700px" colspan="3">
                                                <dx:ASPxComboBox ID="cbKhoXuat" ClientInstanceName="cbKhoXuat" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true" ReadOnly="true">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Mã" FieldName="ID" Name="ID" />
                                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenKho" Name="TenKho" />
                                                    </Columns>
                                                    <ClearButton Visibility="Auto"></ClearButton>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 150px">Tài sản</td>
                                            <td style="width: 700px" colspan="3">
                                                <dx:ASPxComboBox ID="cbTaiSanChuyenKho" ClientInstanceName="cbTaiSanChuyenKho" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true" ReadOnly="true">
                                                    <%--   <ClientSideEvents ValueChanged="function(s, e) { cbTaiSanChuyenKhoValueChanged(); }" />--%>
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="false" />
                                                        <dx:ListBoxColumn Caption="Mã" FieldName="MaTaiSan" Name="MaTaiSan" Width="100px" />
                                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenTaiSan" Name="TenTaiSan" Width="200px" />
                                                        <dx:ListBoxColumn Caption="Số S/N" FieldName="SoSerialNumber" Name="SoSerialNumber" Width="100px" />
                                                        <%-- <dx:ListBoxColumn Caption="Cấu hình" FieldName="CauHinh" Name="CauHinh" Width="150px" />
                                                        <dx:ListBoxColumn Caption="NSX" FieldName="TenNhaSanXuat" Name="TenNhaSanXuat" Width="50px" />
                                                        <dx:ListBoxColumn Caption="NCC" FieldName="TenNhaCungCap" Name="TenNhaCungCap" Width="50px" />
                                                        <dx:ListBoxColumn Caption="ID Kho" FieldName="IDKhoLuuHienTai" Name="IDKhoLuuHienTai" Width="10px" />
                                                        <dx:ListBoxColumn Caption="ID TT" FieldName="IDTrangThaiTaiSanKho" Name="IDTrangThaiTaiSanKho" Width="10px" />--%>
                                                    </Columns>
                                                    <ClearButton Visibility="Auto"></ClearButton>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>


                                        <tr>
                                            <td style="width: 150px">Mã tài sản</td>
                                            <td style="width: 200px">
                                                <dx:ASPxLabel ID="lbMaTaiSan" ClientInstanceName="lbMaTaiSan" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>

                                            <%--<td style="width: 100px">Số S/N</td>
                                            <td style="width: 150px">
                                                <dx:ASPxLabel ID="lbSoSerialNumber" ClientInstanceName="lbSoSerialNumber" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>
                                            <td style="width: 100px">Cấu hình</td>
                                            <td style="width: 150px">
                                                <dx:ASPxLabel ID="lbCauHinhTaiSan" ClientInstanceName="lbCauHinhTaiSan" runat="server" Text="..." Font-Bold="true"></dx:ASPxLabel>
                                            </td>--%>
                                            <td style="width: 150px">Trạng thái tài sản xuất kho</td>
                                            <td style="width: 200px">
                                                <dx:ASPxComboBox ID="cbTrangThaiTaiSanXuatKho" ClientInstanceName="cbTrangThaiTaiSanXuatKho" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true" ReadOnly="true">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Mã" FieldName="ID" Name="ID" />
                                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenTrangThaiTaiSan" Name="TenTrangThaiTaiSan" />
                                                    </Columns>
                                                    <ClearButton Visibility="Auto"></ClearButton>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <%--<tr>

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
                                            <td colspan="4"></td>
                                        </tr>
                                        <tr>
                                            <td>Số lượng</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txtSoLuong" ClientInstanceName="txtSoLuong" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="10000" Width="100%" HorizontalAlign="Right" Font-Bold="true">
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <%--<td>Đơn giá</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txtDonGia" ClientInstanceName="txtDonGia" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000" Width="100%" HorizontalAlign="Right" Font-Bold="true">
                                                </dx:ASPxSpinEdit>

                                            </td>
                                            <td>Thành tiền</td>
                                            <td>
                                                <dx:ASPxSpinEdit ID="txtThanhTien" ClientInstanceName="txtThanhTien" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000" Width="100%" HorizontalAlign="Right" Font-Bold="true">
                                                </dx:ASPxSpinEdit>
                                            </td>
                                                cbHinhThucDungTaiSan
                                            --%>
                                            <td>Trạng thái tài sản nhập kho</td>
                                            <td>
                                                <dx:ASPxComboBox ID="cbTrangThaiTaiSanNhapKho" ClientInstanceName="cbTrangThaiTaiSanNhapKho" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true" ReadOnly="true">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Mã" FieldName="ID" Name="ID" />
                                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenTrangThaiTaiSan" Name="TenTrangThaiTaiSan" />
                                                    </Columns>
                                                    <ClearButton Visibility="Auto"></ClearButton>
                                                </dx:ASPxComboBox>
                                            </td>


                                        </tr>


                                        <tr>
                                            <td style="width: 150px">Kho nhập</td>
                                            <td style="width: 700px" colspan="3">
                                                <dx:ASPxComboBox ID="cbKhoNhap" ClientInstanceName="cbKhoNhap" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%" Font-Bold="true">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Mã" FieldName="ID" Name="ID" />
                                                        <dx:ListBoxColumn Caption="Tên" FieldName="TenKho" Name="TenKho" />
                                                    </Columns>
                                                    <ClearButton Visibility="Auto"></ClearButton>
                                                </dx:ASPxComboBox>
                                            </td>


                                        </tr>

                                        <tr>
                                            <td>Ghi chú</td>
                                            <td colspan="3">
                                                <dx:ASPxTextBox ID="txtGhiChuChiTiet" ClientInstanceName="txtGhiChuChiTiet" runat="server" Width="100%"></dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveDetail" ClientInstanceName="btnSaveDetail" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                <ClientSideEvents Click="function(s,e){saveQLChuyenKhoChiTiet();}" />
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
        <dx:ASPxHiddenField runat="server" ID="txtObjectIDKhoTraCuu" ClientInstanceName="txtObjectIDKhoTraCuu"></dx:ASPxHiddenField>
    </div>

</asp:Content>
