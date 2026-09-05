<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLNhomChay.aspx.cs" Inherits="WebRunDragon.Forms.QLNhomChay" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script>
         var branchID = '<%=GetBranchID()%>'; 
    </script>--%>
    <script src="../Scripts/QLNhomChay.js" type="text/javascript"></script>
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
                        <%-- <td style="width: 50px">
                            <div style="display: <%=GetRight(1)%>; float: right; margin-top: 10px; cursor: pointer; width: 45px;"
                                onclick="openAddForm()" title="Thêm mới">
                                <img src="../Images/icon-add.png" />&nbsp;
                            </div>
                        </td>
                       <td style="width: 81px">Từ khóa</td>
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
                            <dx:ASPxGridView ID="gridQLNhomChay" ClientInstanceName="gridQLNhomChay" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnDataBinding="gridQLNhomChay_DataBinding"
                                OnCustomCallback="gridQLNhomChay_CustomCallback"
                                OnPageIndexChanged="gridQLNhomChay_PageIndexChanged">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(0) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ReadOnly')" title="Xem thông tin">
                                                    <img src="../Images/icon-view.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(2) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                    <img src="../Images/icon-edit.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>

                                    <%--<dx:GridViewDataTextColumn Caption="Duyệt" VisibleIndex="0" Width="40px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(5) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ApprovalOnly')" title="Duyệt">
                                                   <img src="../Images/icon-approval.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>--%>

                                    <%-- <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThai" VisibleIndex="0" Width="100px" FixedStyle="Left">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>--%>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Width="40px" />
                                    <%--  <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="BranchName" VisibleIndex="0" Width="40px" />--%>

                                    <dx:GridViewDataTextColumn Caption="Ký hiệu" FieldName="race_code" VisibleIndex="2" Width="200px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên giải chạy" FieldName="race_name" VisibleIndex="3" Width="350px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <%-- <dx:GridViewDataTextColumn Caption="Từ ngày" FieldName="from_date" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>    
                                    <dx:GridViewDataTextColumn Caption="Đến ngày" FieldName="to_date" VisibleIndex="3" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>--%>
                                    <dx:GridViewDataTextColumn Caption="Mã" FieldName="group_code" VisibleIndex="6" Width="100%">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên" FieldName="group_name" VisibleIndex="6" Width="100%">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                </Columns>
                                <SettingsPager PageSize="30"></SettingsPager>
                                <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />
                                <Settings ShowHorizontalScrollBar="True" />
                            </dx:ASPxGridView>
                            <%--<asp:SqlDataSource ID="SqlDataSourceThanhLy" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="SP_QLNhomChay_Search" SelectCommandType="StoredProcedure">
                                <SelectParameters>
                                    <asp:SessionParameter SessionField="ssKey4Search" Type="String" Name="Key4Search" />
                                    <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                </SelectParameters>
                            </asp:SqlDataSource>--%>

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

        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="1000px" Height="850px"
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
                            <dx:TabPage Text="Chi tiết giải chạy">
                                <ContentCollection>
                                    <dx:ContentControl>

                                        <table class="tblPopupUpdateForm" style="width: 100%" border="0">

                                            <tr>

                                                <td>Giải chạy</td>
                                                <td colspan="3">
                                                    <dx:ASPxComboBox ID="cbGiaiChay" ClientInstanceName="cbGiaiChay" runat="server" ValueField="id" TextFormatString="{2}" Style="width: 100%">
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
                                                <td>Nhóm chạy</td>
                                                <td colspan="3">
                                                    <dx:ASPxComboBox ID="cbNhomChay" ClientInstanceName="cbNhomChay" runat="server" ValueField="ID" TextFormatString="{2}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="ID" Name="ID" Visible="true" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="GroupCode" Name="GroupCode" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="GroupName" Name="GroupName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="4">
                                                    <div style="width: 679px;">
                                                        <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                                                            EnableTheming="True" Theme="PlasticBlue"
                                                            OnDataBinding="gridDetails_DataBinding"
                                                            OnCustomCallback="gridDetails_CustomCallback"
                                                            OnPageIndexChanged="gridDetails_PageIndexChanged">








                                                            <ClientSideEvents CustomButtonClick="function(s,e){
                                                                                            if (e.buttonID == 'btnEditDetail'){
                                                                                                s.GetRowValues(e.visibleIndex, 'id;full_name', openEditFormDetail);
                                                                                            }
                                                                                            else if (e.buttonID == 'btnEDeleteDetail'){
                                                                                                s.GetRowValues(e.visibleIndex, 'id;full_name', openDeleteFormDetail);
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
                                                                <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="0" Visible="true" Width="50px" />
                                                                <dx:GridViewDataTextColumn Caption="Ma nv" FieldName="staff_no_vdsc" Width="100px" VisibleIndex="3">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Tên " FieldName="full_name" Width="200px" VisibleIndex="3">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="Email	 " FieldName="email" Width="150px" VisibleIndex="3">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dx:GridViewDataTextColumn>
                                                                <dx:GridViewDataTextColumn Caption="id_strava	 " FieldName="id_strava" Width="150px" VisibleIndex="3">
                                                                    <Settings AutoFilterCondition="Contains" />
                                                                </dx:GridViewDataTextColumn>

                                                            </Columns>
                                                            <SettingsPager PageSize="15"></SettingsPager>
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
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 150px"></td>
                                                            <td style="width: 20px;">&nbsp;</td>

                                                            <td>
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyet" ClientInstanceName="btnGuiDuyet" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                                    <ClientSideEvents Click="function(s,e){SendApprovalQLNhomChay();}" />
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
                                                        <ClientSideEvents Click="function(s,e){ApprovalNhomChay();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="Từ chối">
                                                        <ClientSideEvents Click="function(s,e){RejectNhomChay();}" />
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
                        </TabPages>
                    </dx:ASPxPageControl>


                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>


    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateDetail" ID="popUpdateDetail" Width="850px" Height="500px" HeaderText="Cập nhật chi tiết " runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">

                        <tr>
                            <td colspan="2">
                                <dx:ASPxGridView ID="gridDanhSachRunner" ClientInstanceName="gridDanhSachRunner" runat="server" Width="100%" EnableTheming="True" Theme="PlasticBlue"
                                    AutoGenerateColumns="False"
                                    DataSourceID="MyDataSource"
                                    OnCustomCallback="gridDanhSachRunner_CustomCallback"
                                    OnPageIndexChanged="gridDanhSachRunner_PageIndexChanged">

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Chọn" Width="30px" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <dataitemtemplate>
                                                    <div style="cursor: pointer" onclick="openChonNhomChay('<%# Eval("ID") %>','<%# Eval("email") %>','<%# Eval("full_name") %>'  )" title="Chọn">
                                                        <img src="../Images/icon-check.png" />
                                                    </div>
                                                </dataitemtemplate>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="id" VisibleIndex="0" Visible="false" />
                                        <dx:GridViewDataTextColumn Caption="Mã " FieldName="staff_no_vdsc" VisibleIndex="2" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Id strava " FieldName="id_strava" VisibleIndex="2" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tên" FieldName="full_name" VisibleIndex="3" Width="250px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Email " FieldName="email" VisibleIndex="3" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="5"></SettingsPager>
                                    <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                    <Settings ShowFilterRow="True" />
                                    <Settings ShowHorizontalScrollBar="True" />
                                </dx:ASPxGridView>
                                <asp:SqlDataSource ID="MyDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                    SelectCommand="SP_QLNhomChay_DSRunner4Fillter" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="ssrace_group_id" Type="String" Name="race_group_id" />
                                        <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>

                        <tr>
                            <td>Người chạy</td>
                            <td>
                                <dx:ASPxComboBox ID="cbRunner" ClientInstanceName="cbRunner" runat="server" ValueField="id" TextFormatString="{1}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="false" />

                                        <dx:ListBoxColumn Caption="Tên" FieldName="full_name" Name="full_name" />
                                        <dx:ListBoxColumn Caption="Eamil" FieldName="email" Name="email" />
                                    </Columns>
                                    <ClearButton Visibility="Auto"></ClearButton>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="2">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="text-align: right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveDetail" ClientInstanceName="btnSaveDetail" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                <ClientSideEvents Click="function(s,e){saveQLNhomChayChiTiet();}" />
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

</asp:Content>

