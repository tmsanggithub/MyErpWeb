<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="CfgFunctionRightInfo.aspx.cs" Inherits="WebRunDragon.Forms.CfgFunctionRightInfo" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/FunctionRightInfo.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel runat="server" ID="UpdatePanel1">
            <Triggers>
            </Triggers>
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridRoles" ClientInstanceName="gridRoles" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="BlackGlass" OnCustomCallback="gridRoles_CustomCallback" OnPageIndexChanged="gridRoles_PageIndexChanged" OnDataBinding="gridRoles_DataBinding">
                                <ClientSideEvents CustomButtonClick="function(s,e){
                                            if (e.buttonID == 'btnEdit'){
                                                s.GetRowValues(e.visibleIndex, 'Id;RoleName', openEditForm);
                                            }
                                            else if (e.buttonID == 'btnDelete'){
                                                s.GetRowValues(e.visibleIndex, 'Id;RoleName', openDeleteForm);
                                            }
                                        }" />

                                <SettingsPager PageSize="100"></SettingsPager>
                                <SettingsBehavior AllowSort="False" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Columns>
                                    <dx:GridViewCommandColumn Width="68px" VisibleIndex="0" ButtonType="Image" ShowNewButtonInHeader="True">
                                        <HeaderTemplate>
                                            <div align="center">
                                                <dx:ASPxImage ToolTip="Thêm mới nhóm quyền người dùng" ID="btnAddRole" runat="server" ImageUrl="../Images/icon-add.png">
                                                    <ClientSideEvents Click="function(s,e){openAddingForm();}" />
                                                </dx:ASPxImage>
                                            </div>
                                        </HeaderTemplate>
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="btnEdit">
                                                <Image ToolTip="Sửa thông tin nhóm quyền" Url="../Images/icon-edit.png"></Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                            <dx:GridViewCommandColumnCustomButton ID="btnDelete">
                                                <Image ToolTip="Xóa thông tin nhóm quyền" Url="../Images/icon-delete.png"></Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>

                                    </dx:GridViewCommandColumn>

                                    <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã nhóm" FieldName="Code" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên nhóm quyền" FieldName="RoleName" VisibleIndex="3">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="Description" VisibleIndex="4">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="880px" Height="379px" ScrollBars="Auto" HeaderText="Cập nhật nhóm quyền" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <dx:ASPxPageControl ID="ASPxTabControl1" runat="server" Theme="BlackGlass" ActiveTabIndex="0" Width="100%">
                        <TabPages>
                            <dx:TabPage Text="Thông tin nhóm quyền">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table class="tblPopupUpdateForm" style="width: 438px">
                                            <tr>
                                                <td style="width: 99px">Mã nhóm</td>
                                                <td>
                                                    <dx:ASPxTextBox Theme="BlackGlass" ID="txtCode" ClientInstanceName="txtCode" runat="server" Width="288px" MaxLength="10"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 99px">Tên nhóm</td>
                                                <td>
                                                    <dx:ASPxTextBox Theme="BlackGlass" ID="txtRoleName" ClientInstanceName="txtRoleName" runat="server" Width="288px" MaxLength="250"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Ghi chú</td>
                                                <td>
                                                    <dx:ASPxMemo Theme="BlackGlass" ID="txtDescription" ClientInstanceName="txtDescription" runat="server" Height="68px" Width="288px" MaxLength="500">
                                                    </dx:ASPxMemo>
                                                </td>
                                            </tr>
                                            <tfoot>
                                                <tr>
                                                    <td colspan="2">
                                                        <br />
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEditRole" ClientInstanceName="btnSaveEditRole" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                                        <ClientSideEvents Click="function(s,e){saveChangeForm();}" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                                <td style="width: 20px"></td>
                                                                <td>
                                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelEditRole" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
                                                                        <ClientSideEvents Click="function(s,e){popUpdateForm.Hide();}" />
                                                                    </dx:ASPxButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </tfoot>

                                        </table>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                            <dx:TabPage Text="Chi tiết quyền của nhóm">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%"
                                            AutoGenerateColumns="False" EnableTheming="True" Theme="BlackGlass"
                                            OnCustomCallback="gridDetails_CustomCallback"
                                            OnPageIndexChanged="gridDetails_PageIndexChanged">
                                            <ClientSideEvents CustomButtonClick="function(s,e){
                                                                if (e.buttonID == 'btnEditRoleDetail'){
                                                                    s.GetRowValues(e.visibleIndex, 'Id;FunctionName', openEditFormRoleDetail);
                                                                }
                                                                else if (e.buttonID == 'btnEDeleteRoleDetail'){
                                                                    s.GetRowValues(e.visibleIndex, 'Id;FunctionName', openDeleteFormRoleDetail);
                                                                }
                                                            }" />
                                            <Columns>
                                                <dx:GridViewCommandColumn Width="68px" VisibleIndex="0" ButtonType="Image" ShowNewButtonInHeader="True">
                                                    <HeaderTemplate>
                                                        <div align="center">
                                                            <dx:ASPxImage ToolTip="Thêm mới quyền người dùng" ID="ASPxImage1" runat="server" ImageUrl="../Images/icon-add.png">
                                                                <ClientSideEvents Click="function(s,e){openAddingFormRoleDetail();}" />
                                                            </dx:ASPxImage>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <CustomButtons>
                                                        <dx:GridViewCommandColumnCustomButton ID="btnEditRoleDetail">
                                                            <Image ToolTip="Sửa thông tin quyền" Url="../Images/icon-edit.png"></Image>
                                                        </dx:GridViewCommandColumnCustomButton>
                                                        <dx:GridViewCommandColumnCustomButton ID="btnEDeleteRoleDetail">
                                                            <Image ToolTip="Xóa thông tin quyền" Url="../Images/icon-delete.png"></Image>
                                                        </dx:GridViewCommandColumnCustomButton>
                                                    </CustomButtons>

                                                </dx:GridViewCommandColumn>
                                                <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" VisibleIndex="1" />
                                                <dx:GridViewDataTextColumn Caption="Mã quyền" FieldName="Code" VisibleIndex="3" />
                                                <dx:GridViewDataTextColumn Caption="Tên quyền" FieldName="FunctionName" VisibleIndex="4" />
                                                <dx:GridViewDataCheckColumn Caption="Đọc" FieldName="CanRead" VisibleIndex="5">
                                                    <PropertiesCheckEdit ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0">
                                                    </PropertiesCheckEdit>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dx:GridViewDataCheckColumn>
                                                <dx:GridViewDataCheckColumn Caption="Thêm" FieldName="CanCreate" VisibleIndex="6">
                                                    <PropertiesCheckEdit ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0">
                                                    </PropertiesCheckEdit>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dx:GridViewDataCheckColumn>
                                                <dx:GridViewDataCheckColumn Caption="Sửa" FieldName="CanEdit" VisibleIndex="7">
                                                    <PropertiesCheckEdit ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0">
                                                    </PropertiesCheckEdit>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dx:GridViewDataCheckColumn>
                                                <dx:GridViewDataCheckColumn Caption="Xóa" FieldName="CanDelete" VisibleIndex="8">
                                                    <PropertiesCheckEdit ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0">
                                                    </PropertiesCheckEdit>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dx:GridViewDataCheckColumn>
                                                <dx:GridViewDataCheckColumn Caption="In" FieldName="CanPrint" VisibleIndex="9">
                                                    <PropertiesCheckEdit ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0">
                                                    </PropertiesCheckEdit>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dx:GridViewDataCheckColumn>
                                                <dx:GridViewDataCheckColumn Caption="Duyệt" FieldName="CanApprove" VisibleIndex="10">
                                                    <PropertiesCheckEdit ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0">
                                                    </PropertiesCheckEdit>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dx:GridViewDataCheckColumn>
                                                <dx:GridViewDataCheckColumn Caption="Đặc biệt(TĐV Nhận/Trả lại TS)" FieldName="CanSpecial" VisibleIndex="11">
                                                    <PropertiesCheckEdit ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0">
                                                    </PropertiesCheckEdit>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                </dx:GridViewDataCheckColumn>
                                            </Columns>
                                            <SettingsPager PageSize="100"></SettingsPager>
                                            <Settings ShowFooter="False" />
                                            <SettingsBehavior AllowSort="False" />
                                            <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />


                                        </dx:ASPxGridView>
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
        <dx:ASPxPopupControl ClientInstanceName="popConfirmDelete" ID="popConfirmDelete" Width="488" Height="160px" HeaderText="Xác nhận xóa nhóm quyền người dùng" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td style="font-weight: 800; text-align: center">
                                <br />
                                Bạn có chắc chắn xóa nhóm quyền [<span style="font-weight: bold; color: red" id="spRoleName"></span>] ?
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnConfirm" ClientInstanceName="btnConfirmDeleteRole" runat="server" AutoPostBack="False" Text="Xóa" Width="99px">
                                                <ClientSideEvents Click="function(s,e){doDeleteAccessGroup();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td align="left">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelDelete" ClientInstanceName="btnCancelDeleteRole" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
                                                <ClientSideEvents Click="function(s,e){popConfirmDelete.Hide();}" />
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
        <dx:ASPxPopupControl ClientInstanceName="popUpdateRoleDetail" ID="popUpdateRoleDetail" Width="600px" Height="160px" HeaderText="Cập nhật chi tiết quyền của nhóm" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td style="width: 99px">Tên chức năng</td>
                            <td>
                                <dx:ASPxComboBox SelectedIndex="0" Width="238px" ID="cboFunctionId" ClientInstanceName="cboFunctionId" runat="server" ValueField="Id" TextFormatString="{2}" ValueType="System.Int32" DropDownRows="8" Theme="BlackGlass" OnCallback="cboFunctionId_Callback">
                                    <ClientSideEvents EndCallback="onEndCallbackFunctions" />
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Id" FieldName="Id" Width="68px" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="Code" Width="119px" />
                                        <dx:ListBoxColumn Caption="Tên chức năng" FieldName="FunctionName" Width="238px" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table class="tblPopupUpdateForm" style="width: 100%">
                                    <tr>
                                        <td>Đọc</td>
                                        <td>Thêm</td>
                                        <td>Sửa</td>
                                        <td>Xóa</td>
                                        <td>In</td>
                                        <td>Duyệt</td>
                                        <td>Đặc biệt</td>
                                    </tr>
                                    <tr>
                                        <td style="width: 14%">
                                            <dx:ASPxCheckBox runat="server" ID="chkRead" ClientInstanceName="chkRead" ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                                        </td>
                                        <td style="width: 14%">
                                            <dx:ASPxCheckBox runat="server" ID="chkCreate" ClientInstanceName="chkCreate" ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                                        </td>
                                        <td style="width: 14%">
                                            <dx:ASPxCheckBox runat="server" ID="chkEdit" ClientInstanceName="chkEdit" ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                                        </td>
                                        <td style="width: 14%">
                                            <dx:ASPxCheckBox runat="server" ID="chkDelete" ClientInstanceName="chkDelete" ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                                        </td>
                                        <td style="width: 14%">
                                            <dx:ASPxCheckBox runat="server" ID="chkPrint" ClientInstanceName="chkPrint" ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                                        </td>
                                        <td style="width: 14%">
                                            <dx:ASPxCheckBox runat="server" ID="chkApprove" ClientInstanceName="chkApprove" ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                                        </td>
                                        <td style="width: 14%">
                                            <dx:ASPxCheckBox runat="server" ID="chkSpecial" ClientInstanceName="chkSpecial" ValueChecked="1" ValueGrayed="0" ValueType="System.Int32" ValueUnchecked="0"></dx:ASPxCheckBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveRoleDetail" ClientInstanceName="btnSaveRoleDetail" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                <ClientSideEvents Click="function(s,e){saveChangeRoleDetail();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td align="left">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelSaveRoleDetail" ClientInstanceName="btnCancelSaveRoleDetail" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
                                                <ClientSideEvents Click="function(s,e){popUpdateRoleDetail.Hide();}" />
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
        <dx:ASPxPopupControl ClientInstanceName="popConfirmDeleteRoleDetail" ID="popConfirmDeleteRoleDetail" Width="400" Height="160px" HeaderText="Xác nhận xóa phân quyền" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td style="font-weight: 800; text-align: center">
                                <br />
                                Bạn có chắc chắn xóa phân quyền [<span style="font-weight: bold; color: red" id="spRoleDetail"></span>] ?
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnConfirmDeleteRoleDetail" ClientInstanceName="btnConfirmDeleteRoleDetail" runat="server" AutoPostBack="False" Text="Xóa" Width="99px">
                                                <ClientSideEvents Click="function(s,e){doDeleteRoleDetail();}" />
                                            </dx:ASPxButton>
                                        </td>
                                        <td style="width: 20px;">&nbsp;</td>
                                        <td align="left">
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelDeleteRoleDetail" ClientInstanceName="btnCancelDeleteRoleDetail" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
                                                <ClientSideEvents Click="function(s,e){popConfirmDeleteRoleDetail.Hide();}" />
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
