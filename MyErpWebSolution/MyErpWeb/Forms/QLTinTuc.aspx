<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="QLTinTuc.aspx.cs" Inherits="WebRunDragon.Forms.QLTinTuc" %>

<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script>
         var branchID = '<%=GetBranchID()%>'; 
    </script>--%>
    <script src="../Scripts/QLTinTuc.js" type="text/javascript"></script>
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
                            <dx:ASPxGridView ID="gridQLTinTuc" ClientInstanceName="gridQLTinTuc" runat="server" Width="100%"
                                AutoGenerateColumns="False" EnableTheming="True" Theme="PlasticBlue"
                                OnDataBinding="gridQLTinTuc_DataBinding"
                                OnCustomCallback="gridQLTinTuc_CustomCallback"
                                OnPageIndexChanged="gridQLTinTuc_PageIndexChanged">

                                <Columns>
                                    <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" FixedStyle="Left">
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(0) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TrangThai") %>','ReadOnly')" title="Xem thông tin">
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
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("subject") %>')" title="Xóa">
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
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Width="40px" />
                                    <%--  <dx:GridViewDataTextColumn Caption="Chi nhánh" FieldName="BranchName" VisibleIndex="0" Width="40px" />--%>

                                    <dx:GridViewDataTextColumn Caption="Giải chạy" FieldName="race_name" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Ký hiệu" FieldName="code" VisibleIndex="2" Width="150px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tiêu đề" FieldName="subject" VisibleIndex="3" Width="350px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Tin ngày" FieldName="news_date" VisibleIndex="3" Width="100px">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="description" VisibleIndex="6" Width="100%">
                                        <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>

                                    <dx:GridViewDataTextColumn Caption="Người tạo" FieldName="createdby" VisibleIndex="6" Width="100%">
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
                                SelectCommand="SP_QLTinTuc_Search" SelectCommandType="StoredProcedure">
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
                            <dx:TabPage Text="Chi tiết tin tức">
                                <ContentCollection>
                                    <dx:ContentControl>

                                        <table class="tblPopupUpdateForm" style="width: 100%" border="0">
                                            <tr>
                                                <td style="width: 80px">Giải chạy</td>
                                                <td colspan="3">
                                                    <dx:ASPxComboBox ID="cbGiaiChay" ClientInstanceName="cbGiaiChay" runat="server" ValueField="id" TextFormatString="{1}" Style="width: 100%">
                                                        <%--<ClientSideEvents ValueChanged="function(s, e) { cbGiaiChayValueChanged(); }" />--%>
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="ID" FieldName="id" Name="id" Visible="false" />
                                                            <dx:ListBoxColumn Caption="Mã" FieldName="code" Name="code" />
                                                            <dx:ListBoxColumn Caption="Tên" FieldName="name" Name="name" />
                                                        </Columns>
                                                        <ClearButton Visibility="Auto"></ClearButton>
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>

                                                <td style="width: 80px">Ký hiệu</td>
                                                <td style="width: 250px">
                                                    <dx:ASPxTextBox ID="txt_code" ClientInstanceName="txt_code" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                                <td style="width: 70px">Ngày tạo</td>
                                                <td style="width: 250px">
                                                    <dx:ASPxDateEdit ID="de_news_date" ClientInstanceName="de_news_date" runat="server" Width="100%">
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

                                                <td>Tên tin tức</td>
                                                <td colspan="3">
                                                    <dx:ASPxTextBox ID="txt_subject" ClientInstanceName="txt_subject" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>

                                            </tr>

                                            <tr>
                                                <td>Ghi chú</td>
                                                <td colspan="3">
                                                    <dx:ASPxTextBox ID="txtdescription" ClientInstanceName="txtdescription" runat="server" Width="100%"></dx:ASPxTextBox>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td colspan="4">
                                                    <dx:ASPxPageControl ID="ASPxPageControl1" ClientInstanceName="tabControl" runat="server" ActiveTabIndex="0" EnableTheming="True" Theme="BlackGlass" Width="100%">
                                                        <TabPages>
                                                            <dx:TabPage Text="Nội dung chi tiết">
                                                                <ContentCollection>


                                                                    <dx:ContentControl runat="server">
                                                                        <dx:ASPxHtmlEditor ID="txt_content" runat="server" ClientInstanceName="txt_content" Width="100%" Height="500px">
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

                                                            <dx:TabPage Text="Tập tin đính kèm">
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
                                                                    <ClientSideEvents Click="function(s,e){saveQLTinTuc();}" />
                                                                </dx:ASPxButton>

                                                            </td>
                                                            <td style="width: 20px;">&nbsp;</td>

                                                            <td>
                                                                <dx:ASPxButton Theme="Office2003Blue" ID="btnGuiDuyet" ClientInstanceName="btnGuiDuyet" runat="server" AutoPostBack="False" Text="Gửi duyệt" Width="99px">
                                                                    <ClientSideEvents Click="function(s,e){SendApprovalQLTinTuc();}" />
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
                                                        <ClientSideEvents Click="function(s,e){ApprovalTinTuc();}" />
                                                    </dx:ASPxButton>
                                                </td>
                                                <td>
                                                    <dx:ASPxButton Theme="Office2003Blue" ID="btnKhongDuyet" ClientInstanceName="btnKhongDuyet" runat="server" AutoPostBack="False" Text="Từ chối">
                                                        <ClientSideEvents Click="function(s,e){RejectTinTuc();}" />
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


