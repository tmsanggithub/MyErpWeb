<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="CfgDataRightInfo.aspx.cs" Inherits="WebRunDragon.Forms.CfgDataRightInfo" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="../Scripts/DataRightInfo.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridAccessGroups" runat="server" ClientInstanceName="gridAccessGroups" AutoGenerateColumns="False" EnableTheming="True" OnCustomCallback="gridAccessGroups_CustomCallback" OnDataBinding="gridAccessGroups_DataBinding" OnPageIndexChanged="gridAccessGroups_PageIndexChanged"   Theme="BlackGlass" Width="100%">

                                <ClientSideEvents CustomButtonClick="function(s, e) {
	                                                                                if (e.buttonID == 'btnEdit'){
                                                                                                                                s.GetRowValues(e.visibleIndex, 'Id;Code', openEditForm);
                                                                                                                            }
                                                                                                                            else if (e.buttonID == 'btnDelete'){
                                                                                                                                s.GetRowValues(e.visibleIndex, 'Id;Code;GroupName', openDeleteForm);
                                                                                                                            }
                                                                                }" />
                                <Columns>
                                    <dx:GridViewCommandColumn ButtonType="Image" ShowNewButtonInHeader="True" VisibleIndex="0" Width="60px">
                                        <HeaderTemplate>
                                            <div align="center">
                                                <dx:ASPxImage ToolTip="Thêm mới nhóm quyền dữ liệu" ID="btnAddRole" runat="server" ImageUrl="../Images/icon-add.png">
                                                    <ClientSideEvents Click="function(s,e){openAddingForm();}" />
                                                </dx:ASPxImage>
                                            </div>
                                        </HeaderTemplate>
                                        <CustomButtons>
                                            <dx:GridViewCommandColumnCustomButton ID="btnEdit">
                                                <Image ToolTip="Sửa thông tin dữ liệu" Url="../Images/icon-edit.png"></Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                            <dx:GridViewCommandColumnCustomButton ID="btnDelete">
                                                <Image ToolTip="Xóa thông tin dữ liệu" Url="../Images/icon-delete.png"></Image>
                                            </dx:GridViewCommandColumnCustomButton>
                                        </CustomButtons>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Mã nhóm" FieldName="Code" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên nhóm" FieldName="GroupName" VisibleIndex="3">
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
    <%--thong tin chi tiet--%>
    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="800px" Height="368px"  ScrollBars="Auto" HeaderText="Cập nhật nhóm quyền dữ liệu" runat="server" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True"   Theme="BlackGlass">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                    <dx:ASPxPageControl ID="ASPxTabControl1" runat="server"   Theme="BlackGlass" ActiveTabIndex="0" Width="100%">
                      <TabPages>
                          <dx:TabPage Text="Thông tin nhóm quyền truy cập">
                                     <ContentCollection>
                                    <dx:ContentControl>
                                        <table class="tblPopupUpdateForm" style="width:100%">
                                           <tr>
                                               <td style="width: 99px">Mã nhóm</td>
                                                <td><dx:ASPxTextBox ID="txtGroupCode" ClientInstanceName="txtGroupCode" runat="server" Width="338px" MaxLength="25"></dx:ASPxTextBox></td>
                                           </tr>
                                           <tr>
                                               <td>Tên nhóm</td>
                                               <td><dx:ASPxTextBox ID="txtGroupName" ClientInstanceName="txtGroupName" runat="server" Width="338px" MaxLength="250"></dx:ASPxTextBox></td>
                                           </tr>
                                           <tr>
                                               <td>Ghi chú</td>
                                               <td>
                                                   <dx:ASPxMemo ID="txtDescription" ClientInstanceName="txtDescription" runat="server" Height="68px" Width="338px" MaxLength="500"   Theme="BlackGlass">
                                                   </dx:ASPxMemo>
                                               </td>
                                           </tr>
                                           <tfoot>
                                                    <tr>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                            <br />
                                                            <table style="width:100%">
                                                                <tr>
                                                                    <td align="left" style="width:139px">
                                                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu" Width="99px">
                                                                            <ClientSideEvents Click="function(s,e){saveChangeForm();}" />
                                                                        </dx:ASPxButton>
                                                                    </td>
                                                                    
                                                                    <td align="left">
                                                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelEdit" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
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
                           <dx:TabPage Text="Chi tiết quyền truy cập của nhóm">
                                       <ContentCollection>
                                    <dx:ContentControl>
                                        <table class="tblPopupUpdateForm" style="width:100%">
                                            <tr>
                                                <td colspan="2" style="font-weight:bolder; font-size:16px;color:#596EB0">
                                                    <dx:ASPxLabel Font-Bold="true" Font-Size="16px" ForeColor="#596EB0" runat="server" ID="lblUserFullNameDataAccess" ClientInstanceName="lblUserFullNameDataAccess"></dx:ASPxLabel>
                                                </td>
                                                <td align="right">
                                                    Danh mục nhân viên chưa cấp    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>Danh mục nhân viên đang cấp quyền truy cập</td>
                                                <td style="width:139px">&nbsp;</td>
                                                <td align="right">
                                                    <dx:ASPxTextBox AutoPostBack="false" runat="server" ID="txtFilterName" MaxLength="25" ClientInstanceName="txtFilterName" Width="290px">
                                                        <ClientSideEvents KeyDown="function(){filterUnPriStaffList();}" />
                                                     </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <dx:ASPxListBox ID="lsbAccess" ClientInstanceName="lsbAccess" runat="server" SelectionMode="CheckColumn" Width="290" Height="210"
                                                        ValueField="UserName" TextField="StaffName" ValueType="System.String" OnCallback="lsbAccess_Callback" >
                                                        <CaptionSettings Position="Top" />
                                                        
                                                    </dx:ASPxListBox>
                                                </td>
                                                <td align="center">
                                                    <dx:ASPxButton ID="btnAddAccess" Theme="Office2003Blue" ClientInstanceName="btnAddAccess" runat="server" AutoPostBack="False" Text="< Thêm N.Viên" Width="119px">
                                                        <ClientSideEvents Click="function(s, e) { addAccessGroups(); }" />
                                                    </dx:ASPxButton>
                                                    <br/><br/><br/>
                                                    <dx:ASPxButton ID="btnRemoveAccess" Theme="Office2003Blue" ClientInstanceName="btnRemoveAccess" runat="server" AutoPostBack="False" Text="> Xóa N.Viên" Width="119px">
                                                        <ClientSideEvents Click="function(s,e){ removeAccessGroups();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="right">
                                                     <dx:ASPxListBox ID="lsbUnAccess" ClientInstanceName="lsbUnAccess" runat="server" SelectionMode="CheckColumn" Width="290" Height="210"
                                                        ValueField="UserName" TextField="StaffName" ValueType="System.String" OnCallback="lsbUnAccess_Callback" >
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
    <%--thong tin chi tiet cua chi tiet--%>
  <div>
        <dx:ASPxPopupControl ClientInstanceName="popConfirmDelete" ID="popConfirmDelete" Width="500" Height="160px" HeaderText="Xác nhận xóa nhóm quyền truy cập" runat="server" 
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True"   Theme="BlackGlass">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                    <table class="tblPopupUpdateForm" style="width:100%">
                        <tr>
                            <td style="font-weight:800;text-align:center">
                                <br/>
                                Bạn có chắc chắn xóa nhóm quyền truy cập [<span style="font-weight:bold;color:red" id="spGroupName"></span>] ?
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <br/><br/><br/>
                                <dx:ASPxButton Theme="Office2003Blue" ID="btnConfirmDeleteAccessGroup" ClientInstanceName="btnConfirmDeleteAccessGroup" runat="server" AutoPostBack="False" Text="Xóa" Width="99px">
                                    <ClientSideEvents Click="function(s,e){doDeleteAccessGroup();}" />
                                </dx:ASPxButton>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelDeleteAccessGroup" ClientInstanceName="btnCancelDeleteAccessGroup" runat="server" AutoPostBack="False" Text="Đóng" Width="99px">
                                    <ClientSideEvents Click="function(s,e){popConfirmDelete.Hide();}" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>
    <div><dx:ASPxHiddenField runat="server" ID="txtObjectId" ClientInstanceName="txtObjectId"></dx:ASPxHiddenField></div>
</asp:Content>
