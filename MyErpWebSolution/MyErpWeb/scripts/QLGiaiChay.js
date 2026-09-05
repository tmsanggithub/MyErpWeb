
function openAddForm() {
    clearForm("NewOnly");
    gridDetails.PerformCallback(-1);
    gridLichSuKyDuyet.PerformCallback(-1);
    gridAttachments.PerformCallback(-1);
    popUpdateForm.Show();
}

function clearForm(ReadAddEditApproval, objStatus) {

    //    txtObjectIDKhoTraCuu.Set('hidden_value', "0");
    cbNhomChay.SetValue('');
    txtObjectId.Set('hidden_value', "0");
    txt_race_code.SetText('')
    txt_race_name.SetText('')
    txtdescription.SetText('')
    checkDangDienRa.SetValue(false)

    txt_race_info_html.SetHtml('');//"<p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Kính gửi anh/chị,</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'><br/></p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Vui lòng hỗ trợ:&nbsp;</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Điện thoại liên hệ: </p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Trân trọng,</p>");


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
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearForm(readEditApproval, data.entity.status);

                txtObjectId.Set('hidden_value', ID);
                txt_race_code.SetText(data.entity.race_code);
                txt_race_name.SetText(data.entity.race_name);

                txtdescription.SetValue(data.entity.description);
                txt_race_info_html.SetHtml(data.entity.race_info_html);


                de_from_date.SetText(data.entity.from_date);
                de_to_date.SetText(data.entity.to_date);
                de_tran_date.SetText(data.entity.tran_date);

                checkDangDienRa.SetValue(data.entity.dang_dien_ra == 1)

                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                gridAttachments.PerformCallback(txtObjectId.Get('hidden_value'));
                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}

function saveQLGiaiChay() {

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

    data += "&DangDienRa=" + checkDangDienRa.GetValue();
    data += "&Description=" + encodeURIComponent(txtdescription.GetText());
    data += "&RaceInfoHtml=" + encodeURIComponent(txt_race_info_html.GetHtml());

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                ;

                txtObjectId.Set('hidden_value', data.id);
                gridQLGiaiChay.PerformCallback();
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
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChay.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SendApprovalQLGiaiChay() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChay.PerformCallback();
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
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChay.PerformCallback();
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
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLGiaiChay.PerformCallback();
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


function TaoGiayChungNhan() {
    if (!confirm('Bạn có muốn tạo lại giấy chứng nhận ?')) {
        return;
    }

    btnTaoGiayChungNhan.SetEnabled(false);
    var data = "mode=TaoGiayChungNhan";
    data += "&id=" + txtObjectId.Get("hidden_value");


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLGiaiChayAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            //  txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                // gridQLGiaiChay.PerformCallback();
                // gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnTaoGiayChungNhan.SetEnabled(false);
            }
            else {
                btnTaoGiayChungNhan.SetEnabled(true);
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

function saveQLGiaiChayChiTiet() {


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
        url: "../Actions/QLGiaiChayAction.ashx",
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
        url: "../Actions/QLGiaiChayAction.ashx",
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
        url: "../Actions/QLGiaiChayAction.ashx",
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
    cbNhomChay.SetValue(id);
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


