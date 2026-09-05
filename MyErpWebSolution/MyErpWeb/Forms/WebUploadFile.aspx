<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebUploadFile.aspx.cs" Inherits="WebRunDragon.Forms.WebUploadFile" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/WebUploadFile.js" type="text/javascript"></script>
</head>

<body>

    <form id="fromPDF" runat="server">

        <table style="width: 100%;">
            <tr>
                <td style="width: 100%; height: 10px"></td>
            </tr>
            <tr>
                <td>
                    <div style="width: 100%;">

                        <dx:ASPxUploadControl ID="uplAttachment" runat="server" ClientInstanceName="uplAttachment" Width="50%" Style="color: blue; font-size: 15px;"
                            NullText="Nhấn vào đây để tải lên" UploadMode="Advanced" AutoStartUpload="True"
                            OnFilesUploadComplete="UploadControl_FilesUploadComplete">
                            <AdvancedModeSettings EnableMultiSelect="True" EnableDragAndDrop="True" />
                            <ValidationSettings AllowedFileExtensions=".pdf, .doc, .docx, .xls, .xlsx">
                            </ValidationSettings>
                            <ClientSideEvents FileUploadStart="function(s, e) { UploadControl_OnFileUploadStart(); }"
                                FileUploadComplete="function(s, e) { UploadControl_OnFileUploadComplete(e); }"
                                FilesUploadComplete="function(s, e) { UploadControl_OnFilesUploadComplete(e); }"
                                UploadingProgressChanged="function(s, e) { UploadControl_OnUploadingProgressChanged(e); }" />
                        </dx:ASPxUploadControl>
                        <div style="padding-top: 15px; padding-bottom: 0px">
                            <b>Chú ý</b>: Kích thước không quá 10MB.
                        </div>

                    </div>
                </td>
            </tr>
            <tr>
                <td style="width: 100%; height: 20px"></td>
            </tr>

            <tr>
                <td>

                    <div>
                        <table style="width: 100%">
                            <tr>
                            </tr>

                            <tr>
                                <td>

                                    <dx:ASPxPageControl ID="ASPxPageControl1" runat="server" Theme="BlackGlass" ActiveTabIndex="0" Width="100%">
                                        <TabPages>
                                            <dx:TabPage Text="Tập tin PDF">
                                                <ContentCollection>
                                                    <dx:ContentControl>

                                                        <table style="width: 100%">


                                                            <tr>
                                                                <td>
                                                                    <dx:ASPxGridView ID="gridAttachments" ClientInstanceName="gridAttachments" runat="server" Width="100%" Style="color: blue; font-size:15px "
                                                                        AutoGenerateColumns="False" EnableTheming="True"
                                                                        Theme="BlackGlass"
                                                                        OnCustomCallback="gridAttachments_CustomCallback"
                                                                        OnDataBinding="gridAttachments_DataBinding"
                                                                        OnPageIndexChanged="gridAttachments_PageIndexChanged">
                                                                        <ClientSideEvents CustomButtonClick="function(s,e){
                                                                                        if (e.buttonID == 'btnDownload'){
                                                                                            s.GetRowValues(e.visibleIndex, 'Id', downloadAttachment);
                                                                                        }
                                                                                        else if (e.buttonID == 'btnDelete'){
                                                                                            s.GetRowValues(e.visibleIndex, 'Id;AttachName', openDeleteAttachment);
                                                                                        }
                                                                                    }" />
                                                                        <Columns>
                                                                            <dx:GridViewCommandColumn VisibleIndex="0" ButtonType="Image" ShowNewButtonInHeader="True" Width="150px">
                                                                                <HeaderTemplate></HeaderTemplate>
                                                                                <CustomButtons>
                                                                                    <dx:GridViewCommandColumnCustomButton ID="btnDownload">
                                                                                        <Image ToolTip="Tải tập tin" Url="../Images/icon-down.png"></Image>
                                                                                    </dx:GridViewCommandColumnCustomButton>
                                                                                    <%--  <dx:GridViewCommandColumnCustomButton ID="btnDelete">
                                                                                            <Image ToolTip="Xóa tập tin" Url="../Images/icon-delete.png"></Image>
                                                                                        </dx:GridViewCommandColumnCustomButton>--%>
                                                                                </CustomButtons>
                                                                            </dx:GridViewCommandColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Id" FieldName="Id" VisibleIndex="1" Width="100px">
                                                                                <Settings AutoFilterCondition="Contains" />
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Ngày tạo" FieldName="CreatedTime" VisibleIndex="1" Width="300px">
                                                                                <Settings AutoFilterCondition="Contains" />
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Tên tập tin" FieldName="AttachName" VisibleIndex="2">
                                                                                <Settings AutoFilterCondition="Contains" />
                                                                            </dx:GridViewDataTextColumn>
                                                                            <%-- <dx:GridViewDataTextColumn Caption="Kiểu" FieldName="Extension" VisibleIndex="3"  Width="100px">
                                                                                <Settings AutoFilterCondition="Contains" />
                                                                            </dx:GridViewDataTextColumn>
                                                                            <dx:GridViewDataTextColumn Caption="Size(KB)" FieldName="FileSize" VisibleIndex="4"  Width="200px" />--%>
                                                                        </Columns>
                                                                        <SettingsPager PageSize="20"></SettingsPager>
                                                                        <SettingsBehavior AllowSort="True" ColumnResizeMode="Control" />
                                                                        <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                                                                        <Settings ShowFilterRow="True" />
                                                                    </dx:ASPxGridView>
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
                        </table>
                    </div>


                    <!-- dành cho upload tập tin !-->
                    <div>
                        <dx:ASPxPopupControl ID="ASPxPopupControl1" runat="server" ClientInstanceName="pcProgress" Modal="True" HeaderText="Uploading"
                            PopupAnimationType="None" CloseAction="None" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="460px"
                            AllowDragging="true" ShowPageScrollbarWhenModal="True" ShowCloseButton="False" ShowFooter="True" Theme="BlackGlass">
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
                            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True" Theme="BlackGlass">
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

                </td>
            </tr>
        </table>

    </form>
</body>
</html>
