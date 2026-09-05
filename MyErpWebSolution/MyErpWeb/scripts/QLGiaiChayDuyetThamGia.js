
function openAddForm() {
    clearForm("NewOnly");

    gridLichSuKyDuyet.PerformCallback(-1);
    //gridAttachments.PerformCallback(-1);
    popUpdateForm.Show();
}

function clearForm(ReadAddEditApproval, objStatus) {
     
    txtObjectId.Set('hidden_value', "0");
    radLoaiUser.SetValue('');
    txtGiaiChayDangKy.SetText('');
    txt_staff_no.SetText('');
    txt_email.SetText('');
    txt_mobile_phone.SetText('');
    txtdescription.SetValue('');
    txt_full_name.SetText('');
    /*
    cbGiaiChay.SetValue('');
    txtObjectId.Set('hidden_value', "0");
    txt_code.SetText('')
    txt_subject.SetText('')
    txtdescription.SetText('')
    
    txt_content.SetHtml('');//"<p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Kính gửi anh/chị,</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'><br/></p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Vui lòng hỗ trợ:&nbsp;</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Điện thoại liên hệ: </p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Trân trọng,</p>");
    */

    //   btnSaveEdit.SetEnabled(false);
    //   btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);


    if (ReadAddEditApproval == "ReadOnly") {

    }
    else if (ReadAddEditApproval == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        // btnSaveEdit.SetEnabled(true);
        // btnGuiDuyet.SetEnabled(true);

    }
    else if (ReadAddEditApproval == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
            // btnSaveEdit.SetEnabled(true);
            // btnGuiDuyet.SetEnabled(true);

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

    var data = "mode=EDIT";
    data += "&id=" + ID;
    data += "&subMode=" + readEditApproval;
  
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLGiaiChayDuyetThamGiaAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearForm(readEditApproval, data.entity.status);

                txtObjectId.Set('hidden_value', ID);
                //cbGiaiChay.SetValue(data.entity.id_race_and_group);
                radLoaiUser.SetValue(data.entity.user_type);
                txtGiaiChayDangKy.SetText(data.entity.giai_chay_dang_ky);
                txt_staff_no.SetText(data.entity.staff_no_vdsc);
                txt_email.SetText(data.entity.email);
                txt_mobile_phone.SetText(data.entity.mobile_phone);
                txtdescription.SetValue(data.entity.description);
                txt_full_name.SetText(data.entity.full_name);


                /*
                select g.id, g.description, g.id_race_and_group, u.full_name, u.email, u.staff_no_vdsc
                             from ql_race_group_user_register g 
                             left join app_user_registed u on u.id=g.id_runner
                             where g.id=@Id and (g.IsDeleted=0 or g.IsDeleted is null)
                */

                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                //  gridAttachments.PerformCallback(txtObjectId.Get('hidden_value'));
                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}
/*
function saveQLGiaiChayDuyetThamGia() {

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
        url: "../Actions/QLGiaiChayDuyetThamGiaAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;

                txtObjectId.Set('hidden_value', data.id);
                gridQLGiaiChayDuyetThamGia.PerformCallback();
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
        url: "../Actions/QLGiaiChayDuyetThamGiaAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChayDuyetThamGia.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SendApprovalQLGiaiChayDuyetThamGia() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
  

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLGiaiChayDuyetThamGiaAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChayDuyetThamGia.PerformCallback();
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
*/
function ApprovalTinTuc() {
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
        url: "../Actions/QLGiaiChayDuyetThamGiaAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChayDuyetThamGia.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnDuyet.SetEnabled(false);             
                btnKhongDuyet.SetEnabled(false);
            }
            else {
                btnDuyet.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function RejectTinTuc() {
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
        url: "../Actions/QLGiaiChayDuyetThamGiaAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChayDuyetThamGia.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnKhongDuyet.SetEnabled(false);
                btnDuyet.SetEnabled(false); 
            }
            else {
                btnKhongDuyet.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}


