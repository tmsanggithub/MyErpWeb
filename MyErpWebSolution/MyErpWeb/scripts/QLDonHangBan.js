function openAddForm() {
    // Open detail form in a new tab for add
    var url = 'QLDonHangBanCT.aspx';
    window.open(url, '_blank');
}



function clearForm(ReadAddEditApproval, objStatus) {

    //    txtObjectIDKhoTraCuu.Set('hidden_value', "0");
    cbGiaiChay.SetValue('');
    txtObjectId.Set('hidden_value', "0");
    txt_code.SetText('')
    txt_subject.SetText('')
    txtdescription.SetText('')

    txt_content.SetHtml('');//"<p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Kính gửi anh/chị,</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'><br/></p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Vui lòng hỗ trợ:&nbsp;</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Điện thoại liên hệ: </p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Trân trọng,</p>");


    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
  

    if (ReadAddEditApproval == "ReadOnly") {

    }
    else if (ReadAddEditApproval == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveEdit.SetEnabled(true);
        btnGuiDuyet.SetEnabled(true);
     
    }
    else if (ReadAddEditApproval == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
            btnSaveEdit.SetEnabled(true);
            btnGuiDuyet.SetEnabled(true);

        }
        else { // các trạng thái còn lại ko enable

        }
    }
    else if (ReadAddEditApproval == "ApprovalOnly") {
        if (objStatus == "SENDAPPROVAL") {
            btnDuyet.SetEnabled(true);
            btnKhongDuyet.SetEnabled(true);
        }
    }

}

function openEditForm(ID, xxxxx, readEditApproval) {
    // Open edit form in a new tab so the user can continue editing there
    var url = 'QLDonHangBanCT.aspx?id=' + encodeURIComponent(ID) + '&subMode=' + encodeURIComponent(readEditApproval);
    window.open(url, '_blank');
}

function saveQLDonHangBan() {

    //if (cbKhoNhap.GetValue() + "" == "") {
    //    alert("Chưa chọn Kho nhập");
    //    return;
    //}

    //if (cbMaChiNhanh.GetValue() + "" == "null" || cbMaChiNhanh.GetValue() + "" == "") {
    //    alert("Chưa nhập chi nhánh");
    //    return;
    //}

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");

    data += "&Code=" + txt_code.GetText();
    data += "&Subject=" + txt_subject.GetText();
    data += "&NewsDate=" + de_news_date.GetDate().toJSON();
    data += "&IdRace=" + cbGiaiChay.GetValue();

    data += "&Description=" + encodeURIComponent(txtdescription.GetText());
    data += "&Contents=" + encodeURIComponent(txt_content.GetHtml());

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;

                txtObjectId.Set('hidden_value', data.id);
                gridQLDonHangBan.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function OpenDeleteForm(ID, fullName) {
    txtObjectId.Set('hidden_value', ID);
    $("#spTopicName").html(fullName);
    popupConfirmDelete.Show();
}

function DoDelete() {
    if (!confirm('Bạn có muốn xóa không?')) {
        return;
    }

    btnConfirmDelete.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SendApprovalQLDonHangBan() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
  

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnSaveEdit.SetEnabled(false);
                btnGuiDuyet.SetEnabled(false);
              
            }
            else {
                btnSaveEdit.SetEnabled(true);
                btnGuiDuyet.SetEnabled(true);
             
            }
            alert(data.message);
        }
    });
}

function ApprovalDonHangBan() {
    if (!confirm('Bạn có muốn Duyệt không?')) {
        return;
    }

    btnDuyet.SetEnabled(false);
    var data = "mode=Approval";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuDuyet=" + txtGhiChuDuyetKhongDuyet.GetValue();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnDuyet.SetEnabled(false);
            }
            else {
                btnDuyet.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function RejectDonHangBan() {
    if (!confirm('Bạn có muốn từ chối không?')) {
        return;
    }

    btnKhongDuyet.SetEnabled(false);
    var data = "mode=Reject";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuKhongDuyet=" + txtGhiChuDuyetKhongDuyet.GetValue();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnKhongDuyet.SetEnabled(false);
            }
            else {
                btnKhongDuyet.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

