<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLGiaiChay.aspx.cs" Inherits="WebRunDragon.Forms.QLGiaiChay" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script>
         var branchID = '<%=GetBranchID()%>'; 
    </script>--%>
    <script src="../Scripts/QLGiaiChay.js" type="text/javascript"></script>
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
                            <dx:ASPxGridView ID="gridQLGiaiChay" ClientInstanceName="gridQLGiaiChay" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnDataBinding="gridQLGiaiChay_DataBinding"
                                OnCustomCallback="gridQLGiaiChay_CustomCallback"
                                OnPageIndexChanged="gridQLGiaiChay_PageIndexChanged">

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
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(3) %>; cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("race_name") %>')" title="Xóa">
                                                    <img src="../Images/icon-delete.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Duyệt" VisibleIndex="0" Width="40px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>
                                                <div style="display: <%#GetRight(5) %>; cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ApprovalOnly')" title="Duyệt">
                                                    <img src="../Images/icon-approval.png" />
                                                </div>
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Trạng thái" FieldName="TrangThai" VisibleIndex="0" Width="100px" FixedStyle="Left">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Width="40px" />
                                    <%--  <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="BranchName" VisibleIndex="0" Width="40px" />--%>

                                    <dx:GridViewDataTextColumn Caption="Ký hiệu" FieldName="race_code" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tên giải chạy" FieldName="race_name" VisibleIndex="3" Width="350px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Đang diễn ra" FieldName="dang_dien_ra" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Từ ngày" FieldName="from_date" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Đến ngày" FieldName="to_date" VisibleIndex="3" Width="150px">
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
                                SelectCommand="SP_QLGiaiChay_Search" SelectCommandType="StoredProcedure">
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

                                                <td style="width: 80px">Ký hiệu</td>
                                                <td style="width: 250px">
                                                    <dx:ASPxTextBox ID="txt_race_code" ClientInstanceName="txt_race_code" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                                <td style="width: 70px">Ngày tạo</td>
                                                <td style="width: 250px">
                                                    <dx:ASPxDateEdit ID="de_tran_date" ClientInstanceName="de_tran_date" runat="server" Width="100%">
                                                        <TimeSectionProperties>
                                                            <TimeEditProperties>
                                                                <ClearButton Visibility="Auto"></ClearButton>
                                                            </TimeEditProperties>
                                                        </TimeSectionProperties>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td>Tên giải chạy</td>
                                                <td colspan="3">
                                                    <dx:ASPxTextBox ID="txt_race_name" ClientInstanceName="txt_race_name" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>Từ ngày</td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="de_from_date" ClientInstanceName="de_from_date" runat="server" Width="100%">
                                                        <TimeSectionProperties>
                                                            <TimeEditProperties>
                                                                <ClearButton Visibility="Auto"></ClearButton>
                                                            </TimeEditProperties>
                                                        </TimeSectionProperties>

                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxDateEdit>
                                                </td>
                                                <td>Đến ngày</td>
                                                <td>
                                                    <dx:ASPxDateEdit ID="de_to_date" ClientInstanceName="de_to_date" runat="server" Width="100%">
                                                        <TimeSectionProperties>
                                                            <TimeEditProperties>
                                                                <ClearButton Visibility="Auto"></ClearButton>
                                                            </TimeEditProperties>
                                                        </TimeSectionProperties>

                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxDateEdit>
                                                </td>

                                            </tr>
                                            <tr>
                                                <td>Ghi chú</td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtdescription" ClientInstanceName="txtdescription" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                                <td>Đang diễn ra</td>
                                                <td>
                                                    <dx:ASPxCheckBox ID="checkDangDienRa" ClientInstanceName="checkDangDienRa" runat="server" Width="100%"></dx:ASPxCheckBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxPageControl ID="ASPxPageControl1" ClientInstanceName="tabControl" runat="server" ActiveTabIndex="0" EnableTheming="True" Theme="BlackGlass" Width="100%">
                                                        <TabPages>
                                                            <dx:TabPage Text="Nội dung chi tiết">
                                                                <ContentCollection>


                                                                    <dx:ContentControl runat="server">
                                                                        <dx:ASPxHtmlEditor ID="txt_race_info_html" runat="server" ClientInstanceName="txt_race_info_html" Width="100%" Height="500px">
                                                                            <ClientSideEvents Init="function(s, e) {
                                                                    s.core.setSpellCheckAttributeValue(false); 
                                                                }" />
                                                                            <Toolbars>
                                                                                <dx:HtmlEditorToolbar Name="StandardToolbar1">
                                                                                    <Items>
                                                                                        <dx:ToolbarCutButton>
                                                                                        </dx:ToolbarCutButton>
                                                                                        <dx:ToolbarCopyButton>
                                                                                        </dx:ToolbarCopyButton>
                                                                                        <dx:ToolbarPasteButton>
                                                                                        </dx:ToolbarPasteButton>
                                                                                        <dx:ToolbarPasteFromWordButton>
                                                                                        </dx:ToolbarPasteFromWordButton>
                                                                                        <dx:ToolbarUndoButton BeginGroup="True">
                                                                                        </dx:ToolbarUndoButton>
                                                                                        <dx:ToolbarRedoButton>
                                                                                        </dx:ToolbarRedoButton>
                                                                                        <dx:ToolbarRemoveFormatButton BeginGroup="True">
                                                                                        </dx:ToolbarRemoveFormatButton>
                                                                                        <dx:ToolbarSuperscriptButton BeginGroup="True">
                                                                                        </dx:ToolbarSuperscriptButton>
                                                                                        <dx:ToolbarSubscriptButton>
                                                                                        </dx:ToolbarSubscriptButton>
                                                                                        <dx:ToolbarInsertOrderedListButton BeginGroup="True">
                                                                                        </dx:ToolbarInsertOrderedListButton>
                                                                                        <dx:ToolbarInsertUnorderedListButton>
                                                                                        </dx:ToolbarInsertUnorderedListButton>
                                                                                        <dx:ToolbarIndentButton BeginGroup="True">
                                                                                        </dx:ToolbarIndentButton>
                                                                                        <dx:ToolbarOutdentButton>
                                                                                        </dx:ToolbarOutdentButton>
                                                                                        <dx:ToolbarInsertLinkDialogButton BeginGroup="True">
                                                                                        </dx:ToolbarInsertLinkDialogButton>
                                                                                        <dx:ToolbarUnlinkButton>
                                                                                        </dx:ToolbarUnlinkButton>
                                                                                        <dx:ToolbarInsertImageDialogButton>
                                                                                        </dx:ToolbarInsertImageDialogButton>
                                                                                        <dx:ToolbarTableOperationsDropDownButton BeginGroup="True">
                                                                                            <Items>
                                                                                                <dx:ToolbarInsertTableDialogButton BeginGroup="True" Text="Insert Table..." ToolTip="Insert Table...">
                                                                                                </dx:ToolbarInsertTableDialogButton>
                                                                                                <dx:ToolbarTablePropertiesDialogButton BeginGroup="True">
                                                                                                </dx:ToolbarTablePropertiesDialogButton>
                                                                                                <dx:ToolbarTableRowPropertiesDialogButton>
                                                                                                </dx:ToolbarTableRowPropertiesDialogButton>
                                                                                                <dx:ToolbarTableColumnPropertiesDialogButton>
                                                                                                </dx:ToolbarTableColumnPropertiesDialogButton>
                                                                                                <dx:ToolbarTableCellPropertiesDialogButton>
                                                                                                </dx:ToolbarTableCellPropertiesDialogButton>
                                                                                                <dx:ToolbarInsertTableRowAboveButton BeginGroup="True">
                                                                                                </dx:ToolbarInsertTableRowAboveButton>
                                                                                                <dx:ToolbarInsertTableRowBelowButton>
                                                                                                </dx:ToolbarInsertTableRowBelowButton>
                                                                                                <dx:ToolbarInsertTableColumnToLeftButton>
                                                                                                </dx:ToolbarInsertTableColumnToLeftButton>
                                                                                                <dx:ToolbarInsertTableColumnToRightButton>
                                                                                                </dx:ToolbarInsertTableColumnToRightButton>
                                                                                                <dx:ToolbarSplitTableCellHorizontallyButton BeginGroup="True">
                                                                                                </dx:ToolbarSplitTableCellHorizontallyButton>
                                                                                                <dx:ToolbarSplitTableCellVerticallyButton>
                                                                                                </dx:ToolbarSplitTableCellVerticallyButton>
                                                                                                <dx:ToolbarMergeTableCellRightButton>
                                                                                                </dx:ToolbarMergeTableCellRightButton>
                                                                                                <dx:ToolbarMergeTableCellDownButton>
                                                                                                </dx:ToolbarMergeTableCellDownButton>
                                                                                                <dx:ToolbarDeleteTableButton BeginGroup="True">
                                                                                                </dx:ToolbarDeleteTableButton>
                                                                                                <dx:ToolbarDeleteTableRowButton>
                                                                                                </dx:ToolbarDeleteTableRowButton>
                                                                                                <dx:ToolbarDeleteTableColumnButton>
                                                                                                </dx:ToolbarDeleteTableColumnButton>
                                                                                            </Items>
                                                                                        </dx:ToolbarTableOperationsDropDownButton>
                                                                                        <dx:ToolbarFullscreenButton BeginGroup="True">
                                                                                        </dx:ToolbarFullscreenButton>
                                                                                    </Items>
                                                                                </dx:HtmlEditorToolbar>
                                                                                <dx:HtmlEditorToolbar Name="StandardToolbar2">
                                                                                    <Items>
                                                                                        <dx:ToolbarFontSizeEdit>
                                                                                            <Items>
                                                                                                <dx:ToolbarListEditItem Text="1 (8pt)" Value="1" />
                                                                                                <dx:ToolbarListEditItem Text="2 (10pt)" Value="2" />
                                                                                                <dx:ToolbarListEditItem Text="3 (12pt)" Value="3" />
                                                                                                <dx:ToolbarListEditItem Text="4 (14pt)" Value="4" />
                                                                                                <dx:ToolbarListEditItem Text="5 (18pt)" Value="5" />
                                                                                                <dx:ToolbarListEditItem Text="6 (24pt)" Value="6" />
                                                                                                <dx:ToolbarListEditItem Text="7 (36pt)" Value="7" />
                                                                                            </Items>
                                                                                        </dx:ToolbarFontSizeEdit>
                                                                                        <dx:ToolbarBoldButton BeginGroup="True">
                                                                                        </dx:ToolbarBoldButton>
                                                                                        <dx:ToolbarItalicButton>
                                                                                        </dx:ToolbarItalicButton>
                                                                                        <dx:ToolbarUnderlineButton>
                                                                                        </dx:ToolbarUnderlineButton>
                                                                                        <dx:ToolbarStrikethroughButton>
                                                                                        </dx:ToolbarStrikethroughButton>
                                                                                        <dx:ToolbarJustifyLeftButton BeginGroup="True">
                                                                                        </dx:ToolbarJustifyLeftButton>
                                                                                        <dx:ToolbarJustifyCenterButton>
                                                                                        </dx:ToolbarJustifyCenterButton>
                                                                                        <dx:ToolbarJustifyRightButton>
                                                                                        </dx:ToolbarJustifyRightButton>
                                                                                        <dx:ToolbarBackColorButton BeginGroup="True">
                                                                                        </dx:ToolbarBackColorButton>
                                                                                        <dx:ToolbarFontColorButton>
                                                                                        </dx:ToolbarFontColorButton>
                                                                                    </Items>
                                                                                </dx:HtmlEditorToolbar>
                                                                            </Toolbars>
                                                                            <SettingsDialogs>
                                                                                <InsertImageDialog>
                                                                                    <SettingsImageUpload UploadFolder="~/Images/Upload">
                                                                                        <ValidationSettings AllowedFileExtensions=".jpe,.jpeg,.jpg,.gif,.png">
                                                                                        </ValidationSettings>
                                                                                    </SettingsImageUpload>
                                                                                </InsertImageDialog>
                                                                            </SettingsDialogs>
                                                                            <Settings AllowDesignView="true" AllowHtmlView="False" AllowPreview="true" AllowInsertDirectImageUrls="true" />
                                                                        </dx:ASPxHtmlEditor>
                                                                    </dx:ContentControl>



                                                                </ContentCollection>
                                                            </dx:TabPage>

                                                            <dx:TabPage Text="Ảnh đại diện">
                                                                <ContentCollection>
                                                                    <dx:ContentControl>
                                                                        <table cellpadding="3" style="width: 100%">
                                                                            <%-- dành cho design upload tap tin--%>
                                                                            <tr>
                                                                                <td>
                                                                                    <dx:ASPxGridView ID="gridAttachments" ClientInstanceName="gridAttachments" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True"
                                                                                        Theme="PlasticBlue"
                                                                                        OnCustomColumnDisplayText="gridAttachments_CustomColumnDisplayText"
                                                                                        OnCustomCallback="gridAttachments_CustomCallback"
                                                                                        OnDataBinding="gridAttachments_DataBinding">
                                                                                        <ClientSideEvents CustomButtonClick="function(s,e){
                                                                                        if (e.buttonID == 'btnDownload'){
                                                                                            s.GetRowValues(e.visibleIndex, 'Id', downloadAttachment);
                                                                                        }
                                                                                        else if (e.buttonID == 'btnDelete'){
                                                                                            s.GetRowValues(e.visibleIndex, 'Id;AttachName', openDeleteAttachment);
                                                                                        }
                                                                                    }" />
                                                                                        <Columns>
                                                                                            <dx:GridViewCommandColumn VisibleIndex="7" ButtonType="Image" ShowNewButtonInHeader="True">
                                                                                                <HeaderTemplate></HeaderTemplate>
                                                                                                <CustomButtons>
                                                                                                    <dx:GridViewCommandColumnCustomButton ID="btnDownload">
                                                                                                        <Image ToolTip="Tải tập tin" Url="../Images/icon-down.png"></Image>
                                                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                                                    <dx:GridViewCommandColumnCustomButton ID="btnDelete">
                                                                                                        <Image ToolTip="Xóa tập tin" Url="../Images/icon-delete.png"></Image>
                                                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                                                </CustomButtons>
                                                                                            </dx:GridViewCommandColumn>
                                                                                            <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" VisibleIndex="1"></dx:GridViewDataTextColumn>
                                                                                            <dx:GridViewDataTextColumn Caption="Tên tập tin" FieldName="AttachName" VisibleIndex="2"></dx:GridViewDataTextColumn>
                                                                                            <dx:GridViewDataTextColumn Caption="Kiểu" FieldName="Extension" VisibleIndex="3" />
                                                                                            <dx:GridViewDataTextColumn Caption="Size(KB)" FieldName="FileSize" VisibleIndex="4" />
                                                                                            <dx:GridViewDataTextColumn Caption="Người tạo" FieldName="CreatedBy" VisibleIndex="5" />
                                                                                            <dx:GridViewDataTextColumn Caption="Ngày tạo" FieldName="CreatedTime" VisibleIndex="6" />
                                                                                        </Columns>
                                                                                        <SettingsPager PageSize="10"></SettingsPager>
                                                                                        <SettingsBehavior AllowSort="True" />
                                                                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                                                                    </dx:ASPxGridView>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <div style="width: 679px;">

                                                                                        <dx:ASPxUploadControl ID="uplAttachment" runat="server" ClientInstanceName="uplAttachment" Width="330"
                                                                                            NullText="Nhấn vào đây để tải lên" UploadMode="Advanced" AutoStartUpload="True"
                                                                                            OnFilesUploadComplete="UploadControl_FilesUploadComplete">
                                                                                            <AdvancedModeSettings EnableMultiSelect="True" EnableDragAndDrop="True" />
                                                                                            <ValidationSettings
                                                                                                AllowedFileExtensions=".rtf, .pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .jpe, .jpeg, .jpg, .gif, .png, .rar, .zip">
                                                                                            </ValidationSettings>
                                                                                            <ClientSideEvents FileUploadStart="function(s, e) { UploadControl_OnFileUploadStart(); }"
                                                                                                FileUploadComplete="function(s, e) { UploadControl_OnFileUploadComplete(e); }"
                                                                                                FilesUploadComplete="function(s, e) { UploadControl_OnFilesUploadComplete(e); }"
                                                                                                UploadingProgressChanged="function(s, e) { UploadControl_OnUploadingProgressChanged(e); }" />
                                                                                        </dx:ASPxUploadControl>
                                                                                        <div style="padding-top: 5px; padding-bottom: 0px">
                                                                                            <b>Chú ý</b>: Kích thước không quá 10MB. Kiểu: .rtf, .pdf, .doc, .docx, .xls, .xlsx, .ppt, .pptx, .jpe, .jpeg, .jpg, .gif, .png, .rar, .zip
                                                                                        </div>

                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                            <%-- kết thúc design dành cho upload tap tin--%>
                                                                        </table>
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage>

                                                            <dx:TabPage Text="Danh sách nhóm chạy">
                                                                <ContentCollection>
                                                                    <dx:ContentControl>
                                                                        <table>

                                                                            <tr>
                                                                                <td>
                                                                                    <div style="width: 679px;">
                                                                                        <dx:ASPxGridView ID="gridDetails" ClientInstanceName="gridDetails" runat="server" Width="100%" AutoGenerateColumns="False"
                                                                                            EnableTheming="True" Theme="PlasticBlue" OnCustomCallback="gridDetails_CustomCallback" OnPageIndexChanged="gridDetails_PageIndexChanged">
                                                                                            <ClientSideEvents CustomButtonClick="function(s,e){
                                                                                            if (e.buttonID == 'btnEditDetail'){
                                                                                                s.GetRowValues(e.visibleIndex, 'ID;GroupName', openEditFormDetail);
                                                                                            }
                                                                                            else if (e.buttonID == 'btnEDeleteDetail'){
                                                                                                s.GetRowValues(e.visibleIndex, 'ID;GroupName', openDeleteFormDetail);
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
                                                                                                <dx:GridViewDataTextColumn Caption="Mã nhóm" FieldName="GroupCode" Width="150px" VisibleIndex="3" />
                                                                                                <dx:GridViewDataTextColumn Caption="Tên nhóm" FieldName="GroupName" Width="200px" VisibleIndex="3" />


                                                                                            </Columns>
                                                                                            <SettingsPager PageSize="10"></SettingsPager>
                                                                                            <Settings ShowFooter="False" />
                                                                                            <SettingsBehavior AllowSort="False" />
                                                                                            <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                                                                            <Settings ShowHorizontalScrollBar="True" />
                                                                                        </dx:ASPxGridView>
                                                                                    </div>

                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </dx:ContentControl>
                                                                </ContentCollection>
                                                            </dx:TabPage>

                                                        </TabPages>
                                                    </dx:ASPxPageControl>
                                                </td>
                                            </tr>


                                            <tr>
                                                <td></td>
                                                <td>
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td style="width: 150px">
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                                    <ClientSideEvents Click="function(s,e){saveQLGiaiChay();}" />
                                                                </dx:ASPxButton>

                                                            </td>
                                                            <td style="width: 20px;">&nbsp;</td>

                                                            <td>
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyet" ClientInstanceName="btnGuiDuyet" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                                    <ClientSideEvents Click="function(s,e){SendApprovalQLGiaiChay();}" />
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td style="width: 20px;">

                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnIn" ClientInstanceName="btnIn" runat="server" AutoPostBack="False" Text="In">
                                                                    <ClientSideEvents Click="function(s,e){inPhieuThanhLy();}" />
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
                                                        <ClientSideEvents Click="function(s,e){ApprovalGiaiChay();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="Từ chối">
                                                        <ClientSideEvents Click="function(s,e){RejectGiaiChay();}" />
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
                                            <tr>
                                                <td colspan="2"></td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnTaoGiayChungNhan" ClientInstanceName="btnTaoGiayChungNhan" runat="server" AutoPostBack="False" Text="Tạo giấy chứng nhận">
                                                        <ClientSideEvents Click="function(s,e){TaoGiayChungNhan();}" />
                                                    </dx:ASPxButton>

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

    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateDetail" ID="popUpdateDetail" Width="850px" Height="500px" HeaderText="Cập nhật chi tiết " runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">

                        <tr>
                            <td colspan="2">
                                <dx:ASPxGridView ID="gridDanhSachNhomChay" ClientInstanceName="gridDanhSachNhomChay" runat="server" Width="100%" EnableTheming="True" Theme="PlasticBlue"
                                    AutoGenerateColumns="False"
                                    DataSourceID="MyDataSource"
                                    OnCustomCallback="gridDanhSachNhomChay_CustomCallback"
                                    OnPageIndexChanged="gridDanhSachNhomChay_PageIndexChanged">

                                    <Columns>
                                        <dx:GridViewDataTextColumn Caption="Chọn" Width="30px" VisibleIndex="0">
                                            <DataItemTemplate>
                                                <dataitemtemplate>
                                                    <div style="cursor: pointer" onclick="openChonNhomChay('<%# Eval("ID") %>','<%# Eval("MaNhomChay") %>','<%# Eval("TenNhomChay") %>'  )" title="Chọn">
                                                        <img src="../Images/icon-check.png" />
                                                    </div>
                                                </dataitemtemplate>
                                            </DataItemTemplate>
                                        </dx:GridViewDataTextColumn>

                                        <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />
                                        <dx:GridViewDataTextColumn Caption="Mã " FieldName="MaNhomChay" VisibleIndex="2" Width="150px">
                                            <Settings AutoFilterCondition="Contains" />
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Tên" FieldName="TenNhomChay" VisibleIndex="3" Width="250px">
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
                                    SelectCommand="SP_QLGiaiChay_DSNhomChay4Fillter" SelectCommandType="StoredProcedure">
                                    <SelectParameters>
                                        <asp:SessionParameter SessionField="ssIDGiaiChay" Type="String" Name="IDGiaiChay" />
                                        <asp:SessionParameter SessionField="ssUserLogin" Type="String" Name="UserLogin" />
                                    </SelectParameters>
                                </asp:SqlDataSource>
                            </td>
                        </tr>

                        <tr>
                            <td>Nhóm chạy</td>
                            <td>
                                <dx:ASPxComboBox ID="cbNhomChay" ClientInstanceName="cbNhomChay" runat="server" ValueField="ID" TextFormatString="{1}" Style="width: 100%">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="ID" FieldName="Id" Name="Id" Visible="false" />
                                        <dx:ListBoxColumn Caption="Mã" FieldName="GroupCode" Name="GroupCode" />
                                        <dx:ListBoxColumn Caption="Tên" FieldName="GroupName" Name="GroupName" />
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
                                                <ClientSideEvents Click="function(s,e){saveQLGiaiChayChiTiet();}" />
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



    <!-- dành cho upload tập tin !-->
    <div>
        <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="pcProgress" Modal="True" HeaderText="Uploading"
            PopupAnimationType="None" CloseAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="460px"
            AllowDragging="true" ShowPageScrollbarWhenModal="True" ShowCloseButton="False" ShowFooter="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl21" runat="server" SupportsDisabledAttribute="True">
                    <table style="width: 100%;">
                        <tr>
                            <td style="width: 100%;">
                                <div style="overflow: hidden; width: 280px;">
                                    <dx:ASPxLabel ID="lblFileName" runat="server" ClientInstanceName="lblFileName" Text=""
                                        Wrap="False">
                                    </dx:ASPxLabel>
                                </div>
                            </td>
                            <td class="NoWrap" style="text-align: right">
                                <dx:ASPxLabel ID="lblCurrentUploadedFileLength" runat="server" ClientInstanceName="lblCurrentUploadedFileLength"
                                    Text="" Wrap="False">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="TopPadding">
                                <dx:ASPxProgressBar ID="ASPxProgressBar1" runat="server" Height="21px" Width="100%"
                                    ClientInstanceName="progress1">
                                </dx:ASPxProgressBar>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="Spacer" style="height: 12px;"></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 100%;">
                                <dx:ASPxLabel ID="lblUploadedFiles" runat="server" ClientInstanceName="lblUploadedFiles" Text=""
                                    Wrap="False">
                                </dx:ASPxLabel>
                            </td>
                            <td class="NoWrap" style="text-align: right">
                                <dx:ASPxLabel ID="lblUploadedFileLength" runat="server" ClientInstanceName="lblUploadedFileLength"
                                    Text="" Wrap="False">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="TopPadding">
                                <dx:ASPxProgressBar ID="ASPxProgressBar2" runat="server" CssClass="BottomMargin" Height="21px" Width="100%"
                                    ClientInstanceName="progress2">
                                </dx:ASPxProgressBar>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div class="Spacer" style="height: 12px;"></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <dx:ASPxLabel ID="lblProgressStatus" runat="server" ClientInstanceName="lblProgressStatus" Text=""
                                    Wrap="False">
                                </dx:ASPxLabel>
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
            <FooterTemplate>
                <div style="overflow: hidden;">
                    <dx:ASPxButton ID="btnCancel" runat="server" AutoPostBack="False" Text="Cancel" ClientInstanceName="btnCancel" Width="100px" Style="float: right">
                        <ClientSideEvents Click="function(s, e) { UploadControl.Cancel(); }" />
                    </dx:ASPxButton>
                </div>
            </FooterTemplate>
            <FooterStyle>
                <Paddings Padding="5px" PaddingRight="10px" />
            </FooterStyle>
        </dx:ASPxPopupControl>
    </div>

    <div>
        <dx:ASPxPopupControl ID="popConfirm4DeletingAttach" runat="server" ClientInstanceName="popConfirm4DeletingAttach" Width="500px" Height="150px" ScrollBars="Auto"
            HeaderText="Xác nhận xóa tập tin" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="PlasticBlue">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server" SupportsDisabledAttribute="True">
                    <div align="center">
                        <table style="width: 100%; height: 100%" border="0">
                            <tr>
                                <td style="text-align: center">Bạn có chắc chắn xóa tập tin <span id="lblDeletingAttachName" style="font-weight: bold"></span>?</td>
                            </tr>
                            <tr>
                                <td style="height: 30px"></td>
                            </tr>
                            <tr>
                                <td align="center" style="height: 30px; vertical-align: central">
                                    <dx:ASPxButton ID="btnDoDeleteAttach" ClientInstanceName="btnDoDeleteAttach" AutoPostBack="false" runat="server" Text="Xác nhận xóa" Style="margin-left: 0px" Theme="BlackGlass">
                                        <ClientSideEvents Click="function(s,e){doDeleteAttachment();}" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <dx:ASPxHiddenField runat="server" ID="hddAttachId" ClientInstanceName="hddAttachId"></dx:ASPxHiddenField>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

    </div>
    <!-- kết thúc dành cho upload tập tin !-->
</asp:Content>


