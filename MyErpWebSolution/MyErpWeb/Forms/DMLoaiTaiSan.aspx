<%@ Page Title="" Language="C#" MasterPageFile="~/Forms/Main.Master" AutoEventWireup="true" CodeBehind="DMLoaiTaiSan.aspx.cs" Inherits="WebRunDragon.Forms.DMLoaiTaiSan" %>

<%@ Register Assembly="DevExpress.Web.v15.1, Version=15.1.8.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
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
                        <td style="width: 81px">Từ khóa</td>
                        <td style="width: 138px">
                            <dx:ASPxTextBox MaxLength="25" runat="server" ID="txtKeyword" Width="118px" Height="22px"   Theme="PlasticBlue"></dx:ASPxTextBox>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btnSearch" ClientInstanceName="btnSearch" runat="server" Text="Tìm kiếm" Style="margin-left: 0px" OnClick="btnSearch_Click" Theme="Office2003Blue" />
                        </td>
                        
                    </tr>
                </table>

                <table style="width: 100%;">
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="gridLoaiTaiSan" ClientInstanceName="gridLoaiTaiSan" runat="server" Width="100%" AutoGenerateColumns="False" EnableTheming="True" 
                                  Theme="PlasticBlue"
                                OnCustomCallback="gridLoaiTaiSan_CustomCallback"
                                OnPageIndexChanged="gridLoaiTaiSan_PageIndexChanged">

                                <Columns>
                                     <dx:GridViewDataTextColumn Caption="Xem" VisibleIndex="0" Width="35px" >
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(0) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenLoaiTaiSan") %>','ReadOnly')" title="Xem thông tin">
                                                 <img src="../Images/icon-view.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Sửa" VisibleIndex="0" Width="35px" >
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(2) %>;  cursor: pointer" onclick="openEditForm('<%#Eval("ID") %>','<%#Eval("TenLoaiTaiSan") %>','EditOnly')" title="Chỉnh sửa thông tin">
                                                   <img src="../Images/icon-edit.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Xóa" VisibleIndex="0" Width="35px" >
                                        <DataItemTemplate>
                                            <dataitemtemplate>                    
                                                <div style=" display: <%#GetRight(3) %>;  cursor: pointer" onclick="OpenDeleteForm('<%#Eval("ID") %>','<%#Eval("TenLoaiTaiSan") %>')" title="Xóa">
                                                   <img src="../Images/icon-delete.png" />
                                                </div>                                        
                                            </dataitemtemplate>
                                        </DataItemTemplate>
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="ID" FieldName="ID" VisibleIndex="0" Visible="false" />                                 
                                    <dx:GridViewDataTextColumn Caption="Mã loại tài sản" FieldName="MaLoaiTaiSan" VisibleIndex="2" />
                                    <dx:GridViewDataTextColumn Caption="Tên loại tài sản" FieldName="TenLoaiTaiSan" VisibleIndex="3" >
                                          <Settings AutoFilterCondition="Contains" />
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Ghi chú" FieldName="GhiChu" VisibleIndex="5" />                                     

                                   
                                </Columns>
                                <SettingsPager PageSize="15"></SettingsPager>
                                <SettingsBehavior AllowSort="True" />
                                <SettingsDataSecurity AllowDelete="False" AllowInsert="False" AllowEdit="False" />
                            </dx:ASPxGridView>

                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div>
        <dx:ASPxPopupControl ClientInstanceName="popUpdateForm" ID="popUpdateForm" Width="550px" Height="250px"
            ScrollBars="Auto" HeaderText="Thông tin loại tài sản" runat="server"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True"   Theme="PlasticBlue">
            <ContentStyle>
                <Paddings Padding="1px" />
            </ContentStyle>
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">

                    <table class="tblPopupUpdateForm" style="width: 100%">
                        
                        <tr>
                            <td>Mã loại tài sản</td>
                            <td>
                                <dx:ASPxTextBox ID="txtMaLoaiTaiSan" ClientInstanceName="txtMaLoaiTaiSan" runat="server" Width="50%"></dx:ASPxTextBox>
                            </td>
                            </tr>  
                        <tr>
                            <td>Tên loại tài sản</td>
                            <td>
                                <dx:ASPxTextBox ID="txtTenLoaiTaiSan" ClientInstanceName="txtTenLoaiTaiSan" runat="server" Width="100%"></dx:ASPxTextBox>
                            </td>
                        </tr>  
                        <tr>
                            <td>Ghi chú</td>
                            <td >
                                <dx:ASPxMemo ID="txtGhiChu" ClientInstanceName="txtGhiChu" runat="server" TextMode="MultiLine"  Width="100%" Height="45px"></dx:ASPxMemo>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td ></td>
                            <td >
                                <table>
                                    <tr>
                                        <td >
                                            <dx:ASPxButton Theme="Office2003Blue" ID="btnSaveEdit" ClientInstanceName="btnSaveEdit" runat="server" AutoPostBack="False" Text="Lưu">
                                                <ClientSideEvents Click="function(s,e){saveLoaiTaiSan();}" />
                                            </dx:ASPxButton>

                                        </td>
                                        <td style="width: 100px"></td>
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

                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
    </div>


    <div>
        <dx:ASPxPopupControl ID="popupConfirmDelete" ClientInstanceName="popupConfirmDelete" runat="server" Height="100px" Width="493px"
            PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            AllowDragging="True" CloseAction="CloseButton" CloseOnEscape="True" Modal="True"   Theme="PlasticBlue">

            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td colspan="3">Bạn có chắc chắn xóa loại tài sản [<span id="spTopicName" style="font-weight: bold; color: red"></span>] ?</td>
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

    <script>
      
        function openAddForm() {
            clearForm("");
            popUpdateForm.Show();
        }

        function clearForm(readEdit) {
            txtMaLoaiTaiSan.SetText("");
            txtMaLoaiTaiSan.SetEnabled(true);
            txtTenLoaiTaiSan.SetText("");               
            txtGhiChu.SetText("");
            txtObjectId.Set('hidden_value', "0");
            if (readEdit == "ReadOnly") {

            }
            else if (readEdit == "NewOnly") {
                // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval

                btnSaveEdit.SetEnabled(true);

            }
            else if (readEdit == "EditOnly") {
                // phụ thuộc vào Trạng thái và Quyền
                //if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
                btnSaveEdit.SetEnabled(true);

                //}
                //else { // các trạng thái còn lại ko enable

                //}
            }
            else if (readEdit == "ApprovalOnly") {
                //if (objStatus == "SENDAPPROVAL") {
                //    btnDuyet.SetEnabled(true);
                //    btnKhongDuyet.SetEnabled(true);
                //}
            }
        }

        function saveLoaiTaiSan() {

            if (txtMaLoaiTaiSan.GetText() + "" == "") {
                alert("Chưa nhập Mã loại tài sản");
                return;
            }
            if (txtTenLoaiTaiSan.GetText() + "" == "") {
                alert("Chưa nhập Tên loại tài sản");
                return;
            }
           
            var data = "mode=AddOrUpdate";
            data += "&id=" + txtObjectId.Get("hidden_value");
            data += "&tableName=DMLoaiTaiSan" ;
           
            data += "&MaLoaiTaiSan=" + txtMaLoaiTaiSan.GetText();
            data += "&TenLoaiTaiSan=" + txtTenLoaiTaiSan.GetText();
         
            data += "&GhiChu=" + txtGhiChu.GetText();

            btnSaveEdit.SetEnabled(false);

            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLDanhMucAction.ashx",
                dataType: "json",
                data: data,
                complete: function (xmlHttpRequest, status) {
                    btnSaveEdit.SetEnabled(true);
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {;
                        txtMaLoaiTaiSan.SetEnabled(false);
                        txtObjectId.Set('hidden_value', data.id);
                        gridLoaiTaiSan.PerformCallback();
                    }
                    else {

                    }
                    alert(data.message);
                }
            });
        }

        function openEditForm(ID, fullName, readEditApproval) {

      
            var data = "mode=EDIT";
            data += "&id=" + ID;
            data += "&subMode=" + readEditApproval;
            data += "&tableName=DMLoaiTaiSan";

            clearForm(readEditApproval);

            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLDanhMucAction.ashx",
                dataType: "json",
                data: data,
                complete: function () { },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        txtObjectId.Set('hidden_value', ID);
                                  
                        txtMaLoaiTaiSan.SetText(data.entity.MaLoaiTaiSan);
                        txtTenLoaiTaiSan.SetText(data.entity.TenLoaiTaiSan);                      
                        txtGhiChu.SetText(data.entity.GhiChu);
                        popUpdateForm.Show();
                    }
                }
            });
        }

        function OpenDeleteForm(ID, fullName) {
            txtObjectId.Set('hidden_value', ID);
            $("#spTopicName").html(fullName);
            popupConfirmDelete.Show();
        }

        function DoDelete() {
            btnConfirmDelete.SetEnabled(false);
            $.ajax({
                type: "POST",
                async: true,
                url: "../Actions/QLDanhMucAction.ashx",
                dataType: "json",
                data: "mode=delete&tableName=DMLoaiTaiSan&id=" + txtObjectId.Get('hidden_value'),
                complete: function () {
                    btnConfirmDelete.SetEnabled(true);
                    popupConfirmDelete.Hide();
                },
                timeout: 30000,
                success: function (data) {
                    if (data.success) {
                        gridLoaiTaiSan.PerformCallback();
                    }
                    else {

                    }
                    alert(data.message);
                }
            });
        }
    </script>

</asp:Content>
