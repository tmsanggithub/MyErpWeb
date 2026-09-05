
function openAddForm() {
    clearForm("NewOnly");
    gridDetails.PerformCallback(-1);
    gridLichSuKyDuyet.PerformCallback(-1);
    gridAttachments.PerformCallback(-1);
    popUpdateForm.Show();
}

function clearForm(ReadAddEditApproval, objStatus) {

    //    txtObjectIDKhoTraCuu.Set('hidden_value', "0");
    cbRunner.SetValue('');
    txtObjectId.Set('hidden_value', "0");
    cbGiaiChay.SetValue(0)
    cbNhomChay.SetValue(0)

   // btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
    //   btnSaveDetail.SetEnabled(false);

    if (ReadAddEditApproval == "ReadOnly") {

    }
    else if (ReadAddEditApproval == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
     //   btnSaveEdit.SetEnabled(true);
        btnGuiDuyet.SetEnabled(true);
        //   btnSaveDetail.SetEnabled(true);
    }
    else if (ReadAddEditApproval == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
          //  btnSaveEdit.SetEnabled(true);
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
        url: "../Actions/QLNhomChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearForm(readEditApproval, data.entity.status);

                txtObjectId.Set('hidden_value', ID);
                cbGiaiChay.SetValue(data.entity.race_id);
               
                cbNhomChay.SetValue(data.entity.run_group_id);
                 
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
             //   gridAttachments.PerformCallback(txtObjectId.Get('hidden_value'));
                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}


function SendApprovalQLNhomChay() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhomChayAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLNhomChay.PerformCallback();
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

function ApprovalNhomChay() {
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
        url: "../Actions/QLNhomChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLNhomChay.PerformCallback();
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

function RejectNhomChay() {
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
        url: "../Actions/QLNhomChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLNhomChay.PerformCallback();
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

    cbRunner.SetValue(0);

    gridDanhSachRunner.PerformCallback();
}

function saveQLNhomChayChiTiet() {


    if (cbRunner.GetValue() == "0" || cbRunner.GetValue() == "") {
        alert("Vui lòng chọn người chạy");
        return;
    }
    btnSaveDetail.SetEnabled(false);
    var data = "mode=AddOrUpdateDetail";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");
    data += "&IDRunner=" + cbRunner.GetValue();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhomChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnSaveDetail.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
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
        url: "../Actions/QLNhomChayAction.ashx",
        dataType: "json",
        data: "mode=EDITDETAIL&id=" + row[0],
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectDetailId.Set("hidden_value", data.entity.ID);

                cbRunner.SetValue(data.entity.user_id);

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
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhomChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnConfirmDeleteDetail.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                popConfirmDeleteDetail.Hide();
            }
            alert(data.message);
        }
    });
}

function openChonNhomChay(id, ma, ten) {
    cbRunner.SetValue(id);
}
