
function openAddForm() {
    clearForm("NewOnly");


    gridLichSuKyDuyet.PerformCallback(-1);


    popUpdateForm.Show();
}

function clearForm(ReadAddEditApproval, objStatus) {

    //    txtObjectIDKhoTraCuu.Set('hidden_value', "0");

    txtObjectId.Set('hidden_value', "0");
    txt_distance_min.SetText('0')

    txtdescription.SetText('')


    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
    //   btnSaveDetail.SetEnabled(false);

    if (ReadAddEditApproval == "ReadOnly") {

    }
    else if (ReadAddEditApproval == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveEdit.SetEnabled(true);
        btnGuiDuyet.SetEnabled(true);
        //   btnSaveDetail.SetEnabled(true);
    }
    else if (ReadAddEditApproval == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
            btnSaveEdit.SetEnabled(true);
            btnGuiDuyet.SetEnabled(true);
            //      btnSaveDetail.SetEnabled(true);
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
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearForm(readEditApproval, data.entity.status);
               
                txtObjectId.Set('hidden_value', ID);

                var date1 = convertVietnameseTimeToDate(data.entity.lam_viec_buoi_sang_tu);
                if (date1) {
                    var formattedTime = date1.getHours().toString().padStart(2, '0') + ':' + date1.getMinutes().toString().padStart(2, '0');
                    time_lam_viec_buoi_sang_tu.SetText(formattedTime);
                } 

                var date2 = convertVietnameseTimeToDate(data.entity.lam_viec_buoi_sang_den);
                if (date2) {
                    var formattedTime2 = date2.getHours().toString().padStart(2, '0') + ':' + date2.getMinutes().toString().padStart(2, '0');
                    time_lam_viec_buoi_sang_den.SetText(formattedTime2);
                }
                /*
                var date3 = convertVietnameseTimeToDate(data.entity.lam_viec_buoi_chieu_tu);
                if (date3) {
                    var formattedTime3 = date3.getHours().toString().padStart(2, '0') + ':' + date3.getMinutes().toString().padStart(2, '0');
                    time_lam_viec_buoi_chieu_tu.SetText(formattedTime3);
                }
                
                var date4 = convertVietnameseTimeToDate(data.entity.lam_viec_buoi_chieu_den);
                if (date4) {
                    var formattedTime4 = date4.getHours().toString().padStart(2, '0') + ':' + date4.getMinutes().toString().padStart(2, '0');
                    time_lam_viec_buoi_chieu_den.SetText(formattedTime4);
                }
                */
                datex= convertUSFormatTimeToDate(data.entity.lam_viec_buoi_sang_den)
                if (datex) {
                    var formattedTime3 = datex.getHours().toString().padStart(2, '0') + ':' + datex.getMinutes().toString().padStart(2, '0');
                    time_lam_viec_buoi_chieu_tu.SetText(formattedTime3);
                }


                time_lam_viec_buoi_chieu_den.SetText(data.entity.lam_viec_buoi_chieu_den);

                radCheDo.SetValue(data.entity.sport_type)

                txtdescription.SetText(data.entity.description);

                txt_distance_min.SetText(data.entity.distance_min);
                txt_distance_max_in_day.SetText(data.entity.distance_max_in_day);

                txt_average_speed_from.SetText(data.entity.average_speed_from);
                txt_average_speed_to.SetText(data.entity.average_speed_to);

                txt_activities_min_in_week.SetText(data.entity.activities_min_in_week);
                txt_money_per_kilomet.SetText(data.entity.money_per_kilomet);

                txt_max_minute_in_day_of_laixe.SetText(data.entity.max_minute_in_day_of_laixe);

                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));


                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}


function convertUSFormatTimeToDate(timeString) {
    if (!timeString) return null;
    // Chuyển đổi chuỗi sang Date
    var date = new Date(timeString);
    // Kiểm tra nếu hợp lệ
    if (!isNaN(date)) {
        return date;
    } else {
        return null;
    }
}


// Hàm chuyển đổi SA/CH -> AM/PM và định dạng HH:mm
function convertVietnameseTimeToDate(timeString) {
    if (!timeString) return null;

    // Thay thế 'SA' bằng 'AM' và 'CH' bằng 'PM'
    var normalizedString = timeString
        .replace('SA', 'AM')
        .replace('CH', 'PM');

    // Chuyển định dạng từ dd/MM/yyyy sang MM/dd/yyyy cho JavaScript
    var parts = normalizedString.match(/(\d{2})\/(\d{2})\/(\d{4}) (\d{1,2}):(\d{2}):(\d{2}) (AM|PM)/);
    if (parts) {
        var formattedDate = `${parts[2]}/${parts[1]}/${parts[3]} ${parts[4]}:${parts[5]} ${parts[7]}`;
        return new Date(formattedDate);
    }
    console.error("Định dạng không hợp lệ:", timeString);
    return null;
}


function saveCDHoatDongHopLe() {

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

    data += "&RaceCode=" + txt_race_code.GetText();
    data += "&RaceName=" + txt_race_name.GetText();
    data += "&FromDate=" + de_from_date.GetDate().toJSON();
    data += "&ToDate=" + de_to_date.GetDate().toJSON();
    data += "&TranDate=" + de_tran_date.GetDate().toJSON();


    data += "&Description=" + encodeURIComponent(txtdescription.GetText());
    data += "&RaceInfoHtml=" + encodeURIComponent(txt_race_info_html.GetHtml());

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;

                txtObjectId.Set('hidden_value', data.id);
                gridCDHoatDongHopLe.PerformCallback();
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
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridCDHoatDongHopLe.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SendApprovalCDHoatDongHopLe() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridCDHoatDongHopLe.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnSaveEdit.SetEnabled(false);
                btnGuiDuyet.SetEnabled(false);
                btnSaveDetail.SetEnabled(false);
            }
            else {
                btnSaveEdit.SetEnabled(true);
                btnGuiDuyet.SetEnabled(true);
                btnSaveDetail.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function ApprovalGiaiChay() {
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
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridCDHoatDongHopLe.PerformCallback();
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

function RejectGiaiChay() {
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
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridCDHoatDongHopLe.PerformCallback();
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
// DETAIL

function openAddingFormDetail() {
    if (txtObjectId.Get("hidden_value") == "0") {
        alert("Bạn vui lòng lưu cài đặt trước khi tạo chi tiết");
        return;
    }

    // lastFunctionId = null;;
    clearFormDetail();
    // cbMaMonHoc.PerformCallback("add|" + txtObjectId.Get("hidden_value"));
    popUpdateDetail.Show();
}

function clearFormDetail() {
    txtObjectDetailId.Set("hidden_value", "0");

    cbNhomChay.SetValue("");
    //  lbSoSerialNumber.SetText("");
    // lbCauHinhTaiSan.SetText("");
    //lbDonViTinh.SetText("");
    //lbNhomTaiSan.SetText("");
    // lbMaTaiSan.SetText("");
    // lbNhaSanXuat.SetText("");
    //  lbNhaCungCap.SetText("");
    //lbSoThangBaoHanh.SetText("");
    // lbNamSanXuat.SetText("");
    // txtSoLuong.SetText(1);
    //  txtDonGia.SetText(0);
    //  txtThanhTien.SetText(0);
    //  txtGhiChuChiTiet.SetText("");

    // cbTrangThaiTaiSan.SetValue("");
    gridDanhSachNhomChay.PerformCallback();
}

function saveCDHoatDongHopLeChiTiet() {


    if (cbNhomChay.GetValue() == "0" || cbNhomChay.GetValue() == "") {
        alert("Vui lòng chọn nhóm chạy");
        return;
    }
    btnSaveDetail.SetEnabled(false);
    var data = "mode=AddOrUpdateDetail";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");
    data += "&IDNhomChay=" + cbNhomChay.GetValue();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnSaveDetail.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {

                popUpdateDetail.Hide();
            }
            alert(data.message);
        }
    });
}

function openEditFormDetail(row) {
    clearFormDetail();
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: "mode=EDITDETAIL&id=" + row[0],
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectDetailId.Set("hidden_value", data.entity.ID);

                cbNhomChay.SetValue(data.entity.run_group_id);

                popUpdateDetail.Show();

            }
            else
                alert(data.message);
        }
    });
}

function openDeleteFormDetail(row) {
    txtObjectDetailId.Set("hidden_value", row[0]);
    $("#spDetail").html(row[1]);
    popConfirmDeleteDetail.Show();
}

function doDeleteDetail() {
    btnConfirmDeleteDetail.SetEnabled(false);

    var data = "mode=deleteDetail";
    data += "&id=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CDHoatDongHopLeAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnConfirmDeleteDetail.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {


                popConfirmDeleteDetail.Hide();
            }
            alert(data.message);
        }
    });
}





