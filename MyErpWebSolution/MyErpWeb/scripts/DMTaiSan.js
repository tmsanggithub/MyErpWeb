function openAddForm() {
    clearForm("NewOnly");



    gridDetails.PerformCallback(-1);
    gridLichSuKyDuyet.PerformCallback(-1);
    gridAttachments.PerformCallback(-1);


    popUpdateForm.Show();
}

function clearForm(readEdit, objStatus) {
    txtObjectId.Set('hidden_value', "0");
    txtMaTaiSan.SetText(""); txtMaTaiSanCopy.SetText("");
    txtMaTaiSan.SetEnabled(true);
    txtTenTaiSan.SetText("");
    cbDonViTinh.SetValue("");
    cbNhomTaiSan.SetValue("");
    cbNhaSanXuat.SetValue("");
    cbViTri.SetValue("");
    txtSoSerialNumber.SetText("");
    txtGiaTriTaiSan.SetValue(0);
    txtTrongLuong.SetValue(0);
    cbDonViTinhTrongLuong.SetValue("");
    txtKichThuotDai.SetValue(0);
    txtKichThuotRong.SetValue(0);
    cbDonViTinhKichThuot.SetValue("");

    txtGhiChu.SetText("");

    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
    btnThayDoiGia.SetEnabled(false);

    if (readEdit == "ReadOnly") {

    }
    else if (readEdit == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveEdit.SetEnabled(true);
        btnGuiDuyet.SetEnabled(true);

    }
    else if (readEdit == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED" || objStatus == "") {
            btnSaveEdit.SetEnabled(true);
            btnGuiDuyet.SetEnabled(true);

        }
        else { // các trạng thái còn lại ko enable

        }
    }
    else if (readEdit == "ApprovalOnly") {
        if (objStatus == "SENDAPPROVAL") {
            btnDuyet.SetEnabled(true);
            btnKhongDuyet.SetEnabled(true);
        }
        else if (objStatus == "APPROVED") {
            btnThayDoiGia.SetEnabled(true);
        }
    }

}

function openEditForm(ID, fullName, readEditApproval) {

    var data = "mode=EDIT";
    data += "&id=" + ID;
    data += "&subMode=" + readEditApproval;
    data += "&tableName=DMTaiSan";

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
                clearForm(readEditApproval, data.entity.TrangThai);

                txtObjectId.Set('hidden_value', ID);
                txtMaTaiSan.SetText(data.entity.MaTaiSan);
                txtTenTaiSan.SetText(data.entity.TenTaiSan);
                cbDonViTinh.SetValue(data.entity.IDDonViTinh);
                cbNhomTaiSan.SetValue(data.entity.IDNhomTaiSan);
                cbNhaSanXuat.SetValue(data.entity.IDNhaSanXuat);
                cbViTri.SetValue(data.entity.IDViTri);
                txtSoSerialNumber.SetText(data.entity.SoSerialNumber);
                txtGiaTriTaiSan.SetValue(data.entity.GiaTriTaiSan);
                txtTrongLuong.SetValue(data.entity.TrongLuong);
                cbDonViTinhTrongLuong.SetValue(data.entity.IdDonViTrongLuong);
                txtKichThuotDai.SetValue(data.entity.KichThuotDai);
                txtKichThuotRong.SetValue(data.entity.KichThuotRong);
                cbDonViTinhKichThuot.SetValue(data.entity.IdDonViKichThuot);
                txtGhiChu.SetText(data.entity.GhiChu);
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                gridAttachments.PerformCallback(txtObjectId.Get("hidden_value"));
                popUpdateForm.Show();
            }
        }
    });
}


function saveTaiSan() {

    if (txtMaTaiSan.GetText() + "" == "") {
        alert("Chưa nhập Mã TaiSan");
        return;
    }
    if (txtTenTaiSan.GetText() + "" == "") {
        alert("Chưa nhập Tên TaiSan");
        return;
    }
    if (txtGhiChu.GetText() + "" == "") {
        alert("Chưa nhập Ghi chú");
        return;
    }
    if (cbDonViTinh.GetValue() + "" == "null" || cbDonViTinh.GetValue() + "" == "") {
        alert("Chưa nhập Đơn vị tính");
        return;
    }
    if (cbNhomTaiSan.GetValue() + "" == "null" || cbNhomTaiSan.GetValue() + "" == "") {
        alert("Chưa nhập Nhóm tài sản");
        return;
    }
    if (cbViTri.GetValue() + "" == "null" || cbViTri.GetValue() + "" == "") {
        alert("Chưa nhập Vị trí");
        return;
    }
    if (txtSoSerialNumber.GetText() == "") {
        alert("Chưa nhập số serial (nếu không có nhập số 0)");
        return;
    }
    if (parseFloat(txtGiaTriTaiSan.GetValue()) < 0) {
        alert("Giá trị tài sản không hợp lệ");
        return;
    }

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&tableName=DMTaiSan";

    data += "&MaTaiSan=" + txtMaTaiSan.GetText();
    data += "&TenTaiSan=" + encodeURIComponent(txtTenTaiSan.GetText());
    data += "&IDDonViTinh=" + cbDonViTinh.GetValue();
    data += "&IDNhomTaiSan=" + cbNhomTaiSan.GetValue();
    data += "&IDNhaSanXuat=" + cbNhaSanXuat.GetValue();
    data += "&IDViTri=" + cbViTri.GetValue();
    data += "&SoSerialNumber=" + txtSoSerialNumber.GetText();
    data += "&GiaTriTaiSan=" + txtGiaTriTaiSan.GetValue();
    data += "&TrongLuong=" + txtTrongLuong.GetValue();
    data += "&IdDonViTrongLuong=" + cbDonViTinhTrongLuong.GetValue();
    data += "&KichThuotDai=" + txtKichThuotDai.GetValue();
    data += "&KichThuotRong=" + txtKichThuotRong.GetValue();
    data += "&IdDonViKichThuot=" + cbDonViTinhKichThuot.GetValue();
    data += "&GhiChu=" + encodeURIComponent(txtGhiChu.GetText());


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
            if (data.success) {
                txtMaTaiSan.SetEnabled(false);
                txtObjectId.Set('hidden_value', data.id);
                gridTaiSan.PerformCallback();

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
    btnConfirmDelete.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: "mode=delete&tableName=DMTaiSan&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridTaiSan.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SendApprovalDMTaiSan() {
    if (txtSoSerialNumber.GetText() == "" || txtSoSerialNumber.GetText() == "0") {
        if (!confirm('Số serial chưa đúng, bạn có muốn gửi duyệt không?')) {
            return;
        }
    }
    else if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }

    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: "mode=SendApprovalTaiSan&tableName=DMTaiSan&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {

                gridTaiSan.PerformCallback();
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

function ApprovalTaiSan() {
    if (!confirm('Bạn có muốn Duyệt không?')) {
        return;
    }


    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);

    var data = "mode=ApprovalTaiSan";
    data += "&tableName=DMTaiSan&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridTaiSan.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnDuyet.SetEnabled(false);
                btnKhongDuyet.SetEnabled(false);
            }
            else {
                btnDuyet.SetEnabled(true);
                btnKhongDuyet.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function RejectTaiSan() {
    if (!confirm('Bạn có muốn từ chối không?')) {
        return;
    }

    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
    var data = "mode=RejectTaiSan";
    data += "&tableName=DMTaiSan&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuKhongDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridTaiSan.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnDuyet.SetEnabled(false);
                btnKhongDuyet.SetEnabled(false);
            }
            else {
                btnDuyet.SetEnabled(true);
                btnKhongDuyet.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function UpdateGiaTriTaiSan() {
    if (!confirm('Bạn có muốn cập nhật thông tin (giá, nhóm TS, loại BH) không?')) {
        return;
    }
    btnThayDoiGia.SetEnabled(false);
    var data = "mode=CapNhatGiaTriTS";
    data += "&tableName=DMTaiSan&id=" + txtObjectId.Get("hidden_value");
    data += "&GiaTriTaiSan=" + txtGiaTriTaiSan.GetValue();
    data += "&IDNhomTaiSan=" + cbNhomTaiSan.GetValue();
    data += "&IDViTri=" + cbViTri.GetValue();

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnThayDoiGia.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
            }
            else {
            }
            alert(data.message);
        }
    });
}

function SaoChepThongTinTaiSan() {

    if (txtMaTaiSanCopy.GetValue() + "" == "null" || txtMaTaiSanCopy.GetValue() + "" == "") {
        alert("Chưa nhập mã tài sản cần sao chép");
        return;
    }

    var data = "mode=COPYTAISAN";
    data += "&id=0";
    data += "&subMode=EditOnly";
    data += "&tableName=DMTaiSan";
    data += "&MaTaiSan=" + txtMaTaiSanCopy.GetValue() + "";

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

                clearForm("NewOnly", data.entity.TrangThai);

                txtMaTaiSan.SetText(data.entity.MaTaiSan);
                txtTenTaiSan.SetText(data.entity.TenTaiSan);
                cbDonViTinh.SetValue(data.entity.IDDonViTinh);
                cbNhomTaiSan.SetValue(data.entity.IDNhomTaiSan);
                cbNhaSanXuat.SetValue(data.entity.IDNhaSanXuat);
                cbViTri.SetValue(data.entity.IDViTri);

                txtSoSerialNumber.SetText(data.entity.SoSerialNumber);
                txtGiaTriTaiSan.SetValue(data.entity.GiaTriTaiSan);
                txtTrongLuong.SetValue(data.entity.TrongLuong);
                cbDonViTinhTrongLuong.SetValue(data.entity.IdDonViTrongLuong);
                txtKichThuotDai.SetValue(data.entity.KichThuotDai);
                txtKichThuotRong.SetValue(data.entity.KichThuotRong);
                cbDonViTinhKichThuot.SetValue(data.entity.IdDonViKichThuot);

                txtGhiChu.SetText(data.entity.GhiChu);

            }
            else {
                alert(data.message);
            }
        }
    });



}


// DETAIL

function openAddingFormDetail() {
    if (txtObjectId.Get("hidden_value") == "0") {
        alert("Bạn vui lòng lưu cài đặt trước khi tạo chi tiết");
        return;
    }

    // lastFunctionId = null;;
    clearFormDetail("NewOnly");
    // cbMaMonHoc.PerformCallback("add|" + txtObjectId.Get("hidden_value"));
    popUpdateDetail.Show();
}

function clearFormDetail(readEditDetail, objStatusDetail) {
    txtObjectDetailId.Set("hidden_value", "0");
    cbDonViTinhCT.SetValue("");
    txtGiaTriQuiDoi.SetText("0");
    txtTenDonViCoBan.SetText("");

    txtTenDonViCoBan.SetText(cbDonViTinh.GetText());
    gridDetails.PerformCallback(0);


    btnSaveThayDoi.SetEnabled(false);
    //btnGuiDuyetThayDoi.SetEnabled(false);
    //btnApprovalThayDoi.SetEnabled(false);
    //btnRejectThayDoi.SetEnabled(false);

    if (readEditDetail == "ReadOnly") {

    }
    else if (readEditDetail == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveThayDoi.SetEnabled(true);
        //  btnGuiDuyetThayDoi.SetEnabled(true);
    }
    else if (readEditDetail == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatusDetail == "NEW" || objStatusDetail == "EDIT" || objStatusDetail == "RETURNED" || objStatusDetail == "") {
            btnSaveThayDoi.SetEnabled(true);
            //   btnGuiDuyetThayDoi.SetEnabled(true);
        }
        else { // các trạng thái còn lại ko enable

        }
    }
    else if (readEditDetail == "ApprovalOnly") {
        if (objStatusDetail == "SENDAPPROVAL") {
            //  btnApprovalThayDoi.SetEnabled(true);
            //  btnRejectThayDoi.SetEnabled(true);
        }
    }


}

function saveQLTaiSanThayDoiCT() {


    btnSaveThayDoi.SetEnabled(false);
    var data = "mode=AddOrUpdateThayDoiTS";
    data += "&tableName=DMTaiSanCT";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");
    data += "&IdDonViTinh=" + cbDonViTinhCT.GetValue();
    data += "&GiaTriQuiDoiSoVoiDonViCoBan=" + txtGiaTriQuiDoi.GetText();

    //data += "&NamSanXuat=" + deNamSanXuat.GetDate().toJSON();

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            //  btnSaveThayDoi.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                alert("data.id:" + data.id);
                txtObjectDetailId.Set("hidden_value", data.id);

                popUpdateDetail.Hide();
            }
            alert(data.message);
        }
    });
}

function openEditFormDetail(ID, noiDungThayDoi, readEditApproval) {
    alert("aa:" + cbDonViTinh.GetText());
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: "mode=GetThayDoiTaiSanDetail&tableName=DMTaiSanCT&id=" + ID,
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearFormDetail(readEditApproval, data.entity.TrangThai);

                txtObjectDetailId.Set("hidden_value", data.entity.ID);

                cbDonViTinhCT.SetValue(data.entity.IdDonViTinh);
                txtGiaTriQuiDoi.SetValue(data.entity.GiaTriQuiDoiSoVoiDonViCoBan);

                txtTenDonViCoBan.SetText(cbDonViTinh.GetText());
                popUpdateDetail.Show();

            }
            else
                alert(data.message);
        }
    });
}

function openDeleteFormDetail(ID, noiDungThayDoi) {
    txtObjectDetailId.Set("hidden_value", ID);
    $("#spDetail").html(noiDungThayDoi);
    popConfirmDeleteDetail.Show();
}

function doDeleteDetail() {
    btnConfirmDeleteDetail.SetEnabled(false);

    var data = "mode=deleteThayDoiTaiSan";
    data += "&tableName=DMTaiSanCT";
    data += "&id=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
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

function SendApprovalThayDoiTaiSan() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveThayDoi.SetEnabled(false);
    btnGuiDuyetThayDoi.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: "mode=SendApprovalThayDoiTaiSan&tableName=DMTaiSanCT&id=" + txtObjectDetailId.Get('hidden_value'),
        complete: function () {


        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {

                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                btnSaveThayDoi.SetEnabled(false);
                btnGuiDuyetThayDoi.SetEnabled(false);

            }
            else {
                // btnSaveThayDoi.SetEnabled(true);
                btnGuiDuyetThayDoi.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function ApprovalThayDoiTaiSan() {
    if (!confirm('Bạn có muốn Duyệt thay đổi thông tin tài sản không?')) {
        return;
    }

    btnApprovalThayDoi.SetEnabled(false);
    btnRejectThayDoi.SetEnabled(false);

    var data = "mode=ApprovalThayDoiTaiSan";
    data += "&tableName=DMTaiSan&id=" + txtObjectDetailId.Get("hidden_value");
    data += "&GhiChuDuyetThayDoi=" + txtGhiChuDuyetKhongDuyetThayDoi.GetValue();

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            // txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                // gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnApprovalThayDoi.SetEnabled(false);
                btnRejectThayDoi.SetEnabled(false);
            }
            else {
                btnApprovalThayDoi.SetEnabled(true);
                btnRejectThayDoi.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function RejectThayDoiTaiSan() {
    if (!confirm('Bạn có muốn từ chối không?')) {
        return;
    }
    btnApprovalThayDoi.SetEnabled(false);
    btnRejectThayDoi.SetEnabled(false);

    var data = "mode=RejectThayDoiTaiSan";
    data += "&tableName=DMTaiSan&id=" + txtObjectDetailId.Get("hidden_value");
    data += "&GhiChuKhongDuyetThayDoi=" + txtGhiChuDuyetKhongDuyetThayDoi.GetValue();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDanhMucAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            //txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                // gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnApprovalThayDoi.SetEnabled(false);
                btnRejectThayDoi.SetEnabled(false);
            }
            else {
                btnApprovalThayDoi.SetEnabled(true);
                btnRejectThayDoi.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}







/* sau đây là phần dành cho upload control-------------------------------------------------------------------------------------------------- */
/*
    Cần có 1 UploadControl có setting giống như trong apsx
    Cần có 1 popup control thể hiện progressbar như pcProgress
    Trong file CS thì cần có method UploadControl_FilesUploadComplete
*/

var fileNumber = 0;
var fileName = "";
var startDate = null;

function UploadControl_OnFileUploadStart() {
    startDate = new Date();
    ClearProgressInfo();
    pcProgress.Show();
}

function UploadControl_OnFilesUploadComplete(e) {
    pcProgress.Hide();
    var result = jQuery.parseJSON(e.callbackData);
    if (result.success) {
        gridAttachments.PerformCallback(txtObjectId.Get('hidden_value'));
    }
    else
        alert(result.message);
}

function UploadControl_OnFileUploadComplete(e) {
    alert("javaScript UploadControl_OnFileUploadComplete ");
}

function ShowMessage(message) {
    window.setTimeout(function () { window.alert(message); }, 0);
}

function UploadControl_OnUploadingProgressChanged(args) {
    if (!pcProgress.IsVisible())
        return;
    if (args.currentFileName != fileName) {
        fileName = args.currentFileName;
        fileNumber++;
    }
    SetCurrentFileUploadingProgress(args.currentFileName, args.currentFileUploadedContentLength, args.currentFileContentLength);
    progress1.SetPosition(args.currentFileProgress);
    SetTotalUploadingProgress(fileNumber, args.fileCount, args.uploadedContentLength, args.totalContentLength);
    progress2.SetPosition(args.progress);
    UpdateProgressStatus(args.uploadedContentLength, args.totalContentLength);
}

function SetCurrentFileUploadingProgress(fileName, uploadedLength, fileLength) {
    lblFileName.SetText("Current File Progress: " + fileName);
    lblFileName.GetMainElement().title = fileName;
    lblCurrentUploadedFileLength.SetText(GetContentLengthString(uploadedLength) + " / " + GetContentLengthString(fileLength));
}

function SetTotalUploadingProgress(number, count, uploadedLength, totalLength) {
    lblUploadedFiles.SetText("Total Progress: " + number + ' of ' + count + " file(s)");
    lblUploadedFileLength.SetText(GetContentLengthString(uploadedLength) + " / " + GetContentLengthString(totalLength));
}

function ClearProgressInfo() {
    SetCurrentFileUploadingProgress("", 0, 0);
    progress1.SetPosition(0);
    SetTotalUploadingProgress(0, 0, 0, 0);
    progress2.SetPosition(0);
    lblProgressStatus.SetText('Elapsed time: 00:00:00 &ensp; Estimated time: 00:00:00 &ensp; Speed: ' + GetContentLengthString(0) + '/s');
    fileNumber = 0;
    fileName = "";
}

function UpdateProgressStatus(uploadedLength, totalLength) {
    var currentDate = new Date();
    var elapsedDateMilliseconds = currentDate - startDate;
    var speed = uploadedLength / (elapsedDateMilliseconds / 1000);
    var elapsedDate = new Date(elapsedDateMilliseconds);
    var elapsedTime = GetTimeString(elapsedDate);
    var estimatedMilliseconds = Math.floor((totalLength - uploadedLength) / speed) * 1000;
    var estimatedDate = new Date(estimatedMilliseconds);
    var estimatedTime = GetTimeString(estimatedDate);
    var speed = uploadedLength / (elapsedDateMilliseconds / 1000);
    lblProgressStatus.SetText('Elapsed time: ' + elapsedTime + ' &ensp; Estimated time: ' + estimatedTime + ' &ensp; Speed: ' + GetContentLengthString(speed) + '/s');
}

function GetContentLengthString(contentLength) {
    var sizeDimensions = ['bytes', 'KB', 'MB', 'GB', 'TB'];
    var index = 0;
    var length = contentLength;
    var postfix = sizeDimensions[index];
    while (length > 1024) {
        length = length / 1024;
        postfix = sizeDimensions[++index];
    }
    var numberRegExpPattern = /[-+]?[0-9]*(?:\.|\,)[0-9]{0,2}|[0-9]{0,2}/;
    var results = numberRegExpPattern.exec(length);
    length = results ? results[0] : Math.floor(length);
    return length.toString() + ' ' + postfix;
}

function GetTimeString(date) {
    var timeRegExpPattern = /\d{1,2}:\d{1,2}:\d{1,2}/;
    var results = timeRegExpPattern.exec(date.toUTCString());
    return results ? results[0] : "00:00:00";
}


function openDeleteAttachment(row) {
    hddAttachId.Set("hidden_value", row[0]);
    $("#lblDeletingAttachName").html(row[1]);
    popConfirm4DeletingAttach.Show();
}

function doDeleteAttachment() {
    //var attId = hddAttachId.Get("hidden_value");
    //$.ajax({
    //    type: "POST",
    //    async: true,
    //    url: "../Actions/IssueListAction.ashx?mode=delete&action=attachment&attId=" + attId,
    //    dataType: "json",
    //    data: "",
    //    complete: function () { },
    //    timeout: 30000,
    //    success: function (data) {
    //        if (data.success) {
    //            gridAttachments.PerformCallback(hddIssueId.Get('hidden_value'));
    //            popConfirm4DeletingAttach.Hide();
    //        }
    //        else
    //            alert(data.message);
    //    }
    //});

    var attId = hddAttachId.Get("hidden_value");
    var data = "mode=deleteAttachment";
    data += "&ID=" + attId;
    data += "&attId=" + attId;
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridAttachments.PerformCallback(txtObjectId.Get('hidden_value'));
                popConfirm4DeletingAttach.Hide();
            }
            alert(data.message);
        }
    });


}

function downloadAttachment(attId) {


    var data = "mode=checkAtt";
    data += "&ID=" + attId;
    data += "&attId=" + attId;
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {

                window.open("../Actions/FileDownloadAction.ashx?Id=" + attId, '_blank');

            }
            else {
                alert("Thành bại")
            }
        }
    });



    //$.ajax({
    //    type: "POST",
    //    async: true,
    //    url: "../Actions/QLDieuChinhKhoAction.ashx?mode=checkAtt&Id=" + attId,
    //    dataType: "json",
    //    data: "",
    //    complete: function () { },
    //    timeout: 30000,
    //    success: function (data) {
    //        if (data.success) {

    //        }
    //        else
    //            alert(data.message);
    //    }
    //});
}

/* Kết thúc dành cho upload control-------------------------------------------------------------------------------------------------- */


