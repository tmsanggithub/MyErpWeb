<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="CfgUserInfo.aspx.cs" Inherits="WebRunDragon.Forms.CfgUserInfo" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/UserInfo.js" type="text/javascript"></script>
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
                        <td style="width: 81px">Từ khóa</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px" Theme="BlackGlass"></dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="BlackGlass" />
                        </td>
                        <td>
                            <div style="display: <%=GetRight(1)%>; float: right; margin-top: 10px; cursor: pointer; width: 45px;"
                                onclick="openAddForm()" title="Thêm mới">
                                <img src="../Images/icon-add.png" />&nbsp;
                            </div>
                        </td>
                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridUsers" ClientInstanceName="gridUsers" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" Theme="BlackGlass" OnCustomCallback="gridUsers_CustomCallback" OnPageIndexChanged="gridUsers_PageIndexChanged" OnDataBinding="gridUsers_DataBinding">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" Width="45px" VisibleIndex="0">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(0) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("UserName") %>','ReadOnly')" title="Xem thông tin">
                                                 <img src="../Images/icon-view.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" Width="45px" VisibleIndex="0">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(2) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("UserName") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                   <img src="../Images/icon-edit.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                    <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="BranchName" VisibleIndex="1" Width="170px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Phòng ban" FieldName="DepartmentName" VisibleIndex="2" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên đăng nhập" FieldName="UserName" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã nhân viên" FieldName="StaffNo" VisibleIndex="5" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên nhân viên" FieldName="StaffName" VisibleIndex="7" Width="200px" />
                                    <dx:GridViewDataTextColumn Caption="Email" FieldName="Email" VisibleIndex="9" Width="100%">
                                        <Settings AllowSort="False" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Nhóm quyền" FieldName="NhomQuyen" VisibleIndex="12" Width="250px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>


                                    <%--<dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="12">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("StaffName") %>')" title="Xóa">
                                                   <img src="../Images/icon-delete.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>--%>
                                </Columns>
                                <SettingsPager PageSize="20"></SettingsPager>
                                <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                <Settings ShowFilterRow="True" />

                                <Settings ShowHorizontalScrollBar="True" />
                            </dx:ASPxGridView>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="800px" Height="450px"
            ScrollBars="Auto" HeaderText="Phân quyền người dùng" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <dx:ASPxPageControl ID="ASPxTabControl1" runat="server" Theme="BlackGlass" ActiveTabIndex="0" Width="100%">
                        <TabPages>

                            <dx:TabPage Text="Thông tin người dùng">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table class="tblPopupUpdateForm" style="width: 100%">
                                            <tr>
                                                <td>Tên đăng nhập</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtUserName" ClientInstanceName="txtUserName" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                                <td style="width: 20px"></td>
                                                <td>Trạng thái</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtIsLocked" Enabled="false" ClientInstanceName="txtIsLocked" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 108px">Mã nhân viên</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtStaffCode" ClientInstanceName="txtStaffCode" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                                <td style="width: 20px"></td>
                                                <td>Số lần đăng nhập lỗi</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtFailedLoginCounter" ReadOnly="true" ClientInstanceName="txtFailedLoginCounter" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Tên nhân viên</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtStaffName" ClientInstanceName="txtStaffName" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                                <td style="width: 20px"></td>
                                                <td>TG cuối đăng nhập lỗi</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtLastLoginFailedTime" Enabled="false" ClientInstanceName="txtLastLoginFailedTime" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Email</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtEmail" ClientInstanceName="txtEmail" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                                <td style="width: 20px"></td>
                                                <td>IP cuối đăng nhập lỗi</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtLastLoginFailedFrom" Enabled="false" ClientInstanceName="txtLastLoginFailedFrom" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Phòng ban</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbDepartment" ClientInstanceName="cbDepartment" runat="server" ValueField="Code" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="Code" Name="Code" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="DeptName" Name="DeptName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td style="width: 20px"></td>
                                                <td>TG cuối đăng nhập thành công</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtLastLoginSuccessTime" Enabled="false" ClientInstanceName="txtLastLoginSuccessTime" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td></td>
                                                <td></td>
                                                <td style="width: 20px"></td>
                                                <td>IP cuối đăng nhập thành công</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtLastLoginSuccessFrom" Enabled="false" ClientInstanceName="txtLastLoginSuccessFrom" runat="server" Width="188px"></dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Chi nhánh</td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cbBranch" ClientInstanceName="cbBranch" runat="server" ValueField="Code" TextFormatString="{1}" Style="width: 100%">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="Code" Name="Code" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="BranchName" Name="BranchName" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="5"></td>
                                            </tr>
                                            <tr>
                                                <td colspan="2"></td>
                                                <td colspan="3">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                                    <ClientSideEvents Click="function(s,e){saveUserInfo();}" />
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
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>

                            <dx:TabPage Text="Phân quyền chức năng">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table class="tblPopupUpdateForm" style="width: 100%">
                                            <tr>
                                                <td colspan="3" style="font-weight: bolder; font-size: 16px; color: #596EB0">
                                                    <dx:ASPxLabel runat="server" ID="lblUserFullName" ClientInstanceName="lblUserFullName"></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Danh mục nhóm quyền đang cấp</td>
                                                <td style="width: 139px">&nbsp;</td>
                                                <td style="text-align: right">Danh mục nhóm quyền chưa cấp</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxListBox Theme="BlackGlass" ID="lsbRoles" ClientInstanceName="lsbRoles" runat="server" SelectionMode="CheckColumn" Width="290" Height="210"
                                                        ValueField="RoleId" TextField="RoleName" ValueType="System.Int32" OnCallback="lsbRoles_Callback">
                                                        <CaptionSettings Position="Top" />

                                                    </dx:ASPxListBox>
                                                </td>
                                                <td align="center">
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnAddRole" ClientInstanceName="btnAddRole" runat="server" AutoPostBack="False" Text="<< Thêm quyền" Width="129px">
                                                        <ClientSideEvents Click="function(s, e) {                                                            
                                                            addRoles(); 
                                                               e.processOnServer = false;
                                                            }" />
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnRemoveRole" ClientInstanceName="btnRemoveRole" runat="server" AutoPostBack="False" Text=">> Xóa quyền" Width="129px">
                                                        <ClientSideEvents Click="function(s,e){                                                            
                                                            removeRoles();
                                                            e.processOnServer = false;
                                                            }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="right">
                                                    <dx:ASPxListBox Theme="BlackGlass" ID="lsbUnRoles" ClientInstanceName="lsbUnRoles" runat="server" SelectionMode="CheckColumn" Width="290" Height="210"
                                                        ValueField="RoleId" TextField="RoleName" ValueType="System.Int32" OnCallback="lsbUnRoles_Callback">
                                                        <CaptionSettings Position="Top" />

                                                    </dx:ASPxListBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>

                            <dx:TabPage Text="Phân quyền dữ liệu">
                                <ContentCollection>
                                    <dx:ContentControl>
                                        <table class="tblPopupUpdateForm" style="width: 100%">
                                            <tr>
                                                <td colspan="3" style="font-weight: bolder; font-size: 16px; color: #596EB0">
                                                    <dx:ASPxLabel runat="server" ID="lblUserFullNameDataAccess" ClientInstanceName="lblUserFullNameDataAccess"></dx:ASPxLabel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Danh mục nhóm dữ liệu đang cấp</td>
                                                <td style="width: 139px">&nbsp;</td>
                                                <td style="text-align: right">Danh mục nhóm dữ liệu chưa cấp</td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxListBox Theme="BlackGlass" ID="lsbAccess" ClientInstanceName="lsbAccess" runat="server" SelectionMode="CheckColumn" Width="290" Height="210"
                                                        ValueField="GroupId" TextField="GroupName" ValueType="System.Int32" OnCallback="lsbAccess_Callback">
                                                        <CaptionSettings Position="Top" />

                                                    </dx:ASPxListBox>
                                                </td>
                                                <td align="center">
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnAddAccess" ClientInstanceName="btnAddAccess" runat="server" AutoPostBack="False" Text="<< Thêm quyền" Width="129px">
                                                        <ClientSideEvents Click="function(s, e) {
                                                             addAccessGroups();
                                                               e.processOnServer = false;
                                                             }" />
                                                    </dx:ASPxButton>
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnRemoveAccess" ClientInstanceName="btnRemoveAccess" runat="server" AutoPostBack="False" Text=">> Xóa quyền" Width="129px">
                                                        <ClientSideEvents Click="function(s,e){
                                                            removeAccessGroups();
                                                               e.processOnServer = false;
                                                            }" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="right">
                                                    <dx:ASPxListBox Theme="BlackGlass" ID="lsbUnAccess" ClientInstanceName="lsbUnAccess" runat="server" SelectionMode="CheckColumn" Width="290" Height="210"
                                                        ValueField="GroupId" TextField="GroupName" ValueType="System.Int32" OnCallback="lsbUnAccess_Callback">
                                                        <CaptionSettings Position="Top" />
                                                    </dx:ASPxListBox>
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
        <dx:ASPxPopupControl ClientInstanceName="popConfirmUnlockUser" ID="popConfirmUnlockUser" Width="400" Height="160px" HeaderText="Xác nhận mở khóa người dùng" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                    <table class="tblPopupUpdateForm" style="width: 100%">
                        <tr>
                            <td style="font-weight: 800; text-align: center">
                                <br />
                                Bạn có chắc chắn mở khóa cho người dùng <span id="spUnlockName"></span>?
                                <br />
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <table style="width: 100%">
                                    <tr>
                                        <td align="right">
                                            <dx:ASPxButton ID="btnConfirmUnlockUser" ClientInstanceName="btnConfirmUnlockUser" runat="server" AutoPostBack="False" Text="Mở khóa" Width="99px" Theme="BlackGlass">
                                                <ClientSideEvents Click="function(s,e){doUnlockUser();}" />
                                            </dx:ASPxButton>

                                        </td>
                                        <td style="width: 25px"></td>
                                        <td align="left">
                                            <dx:ASPxButton ID="btnCancelUnlockUser" ClientInstanceName="btnCancelUnlockUser" runat="server" AutoPostBack="False" Text="Đóng" Width="99px" Theme="BlackGlass">
                                                <ClientSideEvents Click="function(s,e){popConfirmUnlockUser.Hide();}" />
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
        <dx:ASPxPopupControl ID="popupConfirmDelete" ClientInstanceName="popupConfirmDelete" runat="server" Height="100px" Width="493px"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">

            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3">Bạn có chắc chắn xóa yêu cầu [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
                        </tr>
                        <tr>
                            <td colspan="3"></td>
                        </tr>
                        <tr>
                            <td align="right">
                                <dx:ASPxButton ID="btnConfirmDelete" ClientInstanceName="btnConfirmDelete" runat="server" Text="Có">
                                    <ClientSideEvents Click="function(s, e) {DoDelete(); }" />
                                </dx:ASPxButton>
                            </td>
                            <td></td>
                            <td>
                                <dx:ASPxButton ID="btnCancelDelete" runat="server" Text="Không">
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
        <dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField>

    </div>

</asp:Content>
