<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="CDHoatDongHopLe.aspx.cs" Inherits="WebRunDragon.Forms.CDHoatDongHopLe" %>

<%@ Register Assembly="DevExpress.Web.ASPxScheduler.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxScheduler" TagPrefix="dxwschs" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script>
         var branchID = '<%=GetBranchID()%>'; 
    </script>--%>
    <script src="../Scripts/CDHoatDongHopLe.js" type="text/javascript"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <asp:ScriptManager runat="server" ID="ScriptManager1"></asp:ScriptManager>
        <%--</div>--%>

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
                            <%--<td style="width: 81px">Từ khóa</td>
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
                                <dx:ASPxGridView ID="gridCDHoatDongHopLe" ClientInstanceName="gridCDHoatDongHopLe" runat="server" Width="100%"
                                    AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                    OnDataBinding="gridCDHoatDongHopLe_DataBinding"
                                    OnCustomCallback="gridCDHoatDongHopLe_CustomCallback"
                                    OnPageIndexChanged="gridCDHoatDongHopLe_PageIndexChanged">

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                            <DataItemTemplate>
                                                <dataitemtemplate>                    
                                                <div style="display: <%#GetRight(0) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ReadOnly')" title="Xem thông tin">
                                                 <img src="../Images/icon-view.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                            <DataItemTemplate>
                                                <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(2) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','EditOnly')" title="Chỉnh sửa thông tin">
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
                                                <div style=" display: <%#GetRight(5) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ApprovalOnly')" title="Duyệt">
                                                   <img src="../Images/icon-approval.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThai" VisibleIndex="0" Width="100px" FixedStyle="Left">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Width="40px" Visible="false" />
                                        <%--  <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="BranchName" VisibleIndex="0" Width="40px" />--%>

                                        <dx:GridViewDataTextColumn Caption="Số km tối thiểu" FieldName="distance_min" VisibleIndex="2" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Số km tối đa/ ngày" FieldName="distance_max_in_day" VisibleIndex="3" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Số ngày tối thiểu/ tuần" FieldName="activities_min_in_week" VisibleIndex="3" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tốc độ trung bình" FieldName="average_speed_from" VisibleIndex="3" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tốc độ/km" FieldName="average_speed_to" VisibleIndex="3" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="description" VisibleIndex="6" Width="100%">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                    <SettingsPager PageSize="12"></SettingsPager>
                                    <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                    <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                    <Settings ShowFilterRow="True" />
                                    <Settings ShowHorizontalScrollBar="True" />
                                </dx:ASPxGridView>
                                <%--<asp:SqlDataSource ID="SqlDataSourceThanhLy" runat="server" ConnectionString="<%$ ConnectionStrings:assetCnnString %>"
                                SelectCommand="SP_CDHoatDongHopLe_Search" SelectCommandType="StoredProcedure">
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

            <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="750px" Height="500px"
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
                                <dx:TabPage Text="Chi tiết cài đặt hoạt động hợp lệ">
                                    <ContentCollection>
                                        <dx:ContentControl>

                                            <table style="width: 100%" border="0">
                                                <tr>
                                                    <td>Chế độ (Run;Walk)</td>
                                                    <td colspan="4">
                                                        <dx:ASPxRadioButtonList ID="radCheDo" ClientInstanceName="radCheDo" runat="server" RepeatDirection="Horizontal" ValueType="System.String" Border-BorderStyle="None">
                                                            <Items>
                                                                <dx:ListEditItem Text="Run" Value="Run" Selected="true" />
                                                                <dx:ListEditItem Text="Walk" Value="Walk" />
                                                            </Items>
                                                        </dx:ASPxRadioButtonList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5">Thời gian làm việc:</td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 200px;">Sáng làm việc từ</td>
                                                    <td style="width: 100px;">

                                                        <dx:ASPxDateEdit ID="time_lam_viec_buoi_sang_tu" ClientInstanceName="time_lam_viec_buoi_sang_tu" runat="server" EditFormatString="HH:mm">
                                                            <TimeSectionProperties Visible="true">
                                                                <TimeEditProperties EditFormatString="HH:mm" />
                                                            </TimeSectionProperties>
                                                        </dx:ASPxDateEdit>

                                                    </td>
                                                    <td style="width: 20px;">&nbsp;</td>
                                                    <td>Chiều làm việc từ</td>
                                                    <td>
                                                        <dx:ASPxDateEdit ID="time_lam_viec_buoi_chieu_tu" ClientInstanceName="time_lam_viec_buoi_chieu_tu" runat="server" EditFormat="Custom" EditFormatString="HH:mm">
                                                            <TimeSectionProperties>
                                                                <TimeEditProperties DisplayFormatString="HH:mm" EditFormatString="HH:mm" />
                                                            </TimeSectionProperties>
                                                        </dx:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Sáng làm việc đến</td>
                                                    <td style="width: 100px;">
                                                        <dx:ASPxDateEdit ID="time_lam_viec_buoi_sang_den" ClientInstanceName="time_lam_viec_buoi_sang_den" runat="server" EditFormat="Custom" EditFormatString="HH:mm">
                                                            <TimeSectionProperties>
                                                                <TimeEditProperties DisplayFormatString="HH:mm" EditFormatString="HH:mm" />
                                                            </TimeSectionProperties>
                                                        </dx:ASPxDateEdit>

                                                    </td>

                                                    <td style="width: 20px;">&nbsp;</td>
                                                    <td>Chiều làm việc đến</td>
                                                    <td>
                                                        <dx:ASPxDateEdit ID="time_lam_viec_buoi_chieu_den" ClientInstanceName="time_lam_viec_buoi_chieu_den" runat="server" EditFormat="Custom" EditFormatString="HH:mm">
                                                            <TimeSectionProperties>
                                                                <TimeEditProperties DisplayFormatString="HH:mm" EditFormatString="HH:mm" />
                                                            </TimeSectionProperties>
                                                        </dx:ASPxDateEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" style="height: 10px;"></td>
                                                </tr>


                                                <tr>
                                                    <td>Nội dung cài đặt</td>
                                                    <td colspan="4">
                                                        <dx:ASPxTextBox ID="txtdescription" ClientInstanceName="txtdescription" runat="server" Width="100%"></dx:ASPxTextBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="5" style="height: 10px;"></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 200px">Distance (Số km) tối thiểu</td>
                                                    <td style="width: 100px">
                                                        <dx:ASPxSpinEdit ID="txt_distance_min" ClientInstanceName="txt_distance_min" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                    </td>
                                                    <td style="width: 20px;">&nbsp;</td>
                                                    <td>Tốc độ trung bình từ</td>
                                                    <td>
                                                        <dx:ASPxSpinEdit ID="txt_average_speed_from" ClientInstanceName="txt_average_speed_from" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td style="width: 200px">Số km tối đa/ ngày</td>
                                                    <td style="width: 100px">
                                                        <dx:ASPxSpinEdit ID="txt_distance_max_in_day" ClientInstanceName="txt_distance_max_in_day" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                    </td>

                                                    <td style="width: 20px;">&nbsp;</td>
                                                    <td>Tốc độ trung bình đến</td>
                                                    <td>
                                                        <dx:ASPxSpinEdit ID="txt_average_speed_to" ClientInstanceName="txt_average_speed_to" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                    </td>

                                                </tr>
                                                <tr>
                                                    <td colspan="5" style="height: 10px;"></td>
                                                </tr>
                                                <tr>
                                                    <td>Số ngày tối thiểu/ tuần</td>
                                                    <td>
                                                        <dx:ASPxSpinEdit ID="txt_activities_min_in_week" ClientInstanceName="txt_activities_min_in_week" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                    </td>
                                                    <td style="width: 20px;">&nbsp;</td>
                                                    <td>Số tiền/km</td>
                                                    <td>
                                                        <dx:ASPxSpinEdit ID="txt_money_per_kilomet" ClientInstanceName="txt_money_per_kilomet" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Số phút tối đa/ngày (NV lái xe)</td>
                                                    <td>
                                                        <dx:ASPxSpinEdit ID="txt_max_minute_in_day_of_laixe" ClientInstanceName="txt_max_minute_in_day_of_laixe" runat="server" DisplayFormatString="#,###" Number="0" MinValue="0" MaxValue="1000000000000" Width="100%" HorizontalAlign="Right"></dx:ASPxSpinEdit>
                                                    </td>
                                                    <td colspan="3"></td>
                                                </tr>


                                            </table>


                                            <table style="width: 100%" border="0">
                                                <tr>
                                                    <td colspan="4" style="height: 20px;"></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px">
                                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                            <ClientSideEvents Click="function(s,e){saveCDHoatDongHopLe();}" />
                                                        </dx:ASPxButton>

                                                    </td>
                                                    <td style="width: 20px;">&nbsp;</td>

                                                    <td>
                                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyet" ClientInstanceName="btnGuiDuyet" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                            <ClientSideEvents Click="function(s,e){SendApprovalCDHoatDongHopLe();}" />
                                                        </dx:ASPxButton>
                                                    </td>

                                                    <td>
                                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelEdit" runat="server" AutoPostBack="False" Text="Thoát">
                                                            <ClientSideEvents Click="function(s,e){popUpdateForm.Hide();}" />
                                                        </dx:ASPxButton>
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
                                                            <ClientSideEvents Click="function(s,e){ApprovalHoatDongHopLe();}" />
                                                        </dx:ASPxButton>
                                                    </td>
                                                    <td>
                                                        <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="Từ chối">
                                                            <ClientSideEvents Click="function(s,e){RejectHoatDongHopLe();}" />
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
                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnConfirmDelete" ClientInstanceName="btnConfirmDelete" runat="server" Text="Có">
                                        <ClientSideEvents Click="function(s, e) {DoDelete(); }" />
                                    </dx:ASPxButton>
                                </td>
                                <td></td>
                                <td>
                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnCancelDelete" runat="server" Text="Không">
                                        <ClientSideEvents Click="function(s, e) {	popupConfirmDelete.Hide();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>

            </dx:ASPxPopupControl>

        </div>
</asp:Content>
