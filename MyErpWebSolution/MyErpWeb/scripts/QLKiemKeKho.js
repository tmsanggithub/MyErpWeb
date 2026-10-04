
function openAddForm() {
    clearForm("NewOnly");
    // Không cần load details và lịch sử khi form mới (ID = 0)
    // gridDetails.PerformCallback(-1);
    // gridLichSuKyDuyet.PerformCallback(-1);
    // gridAttachments.PerformCallback(-1);
    gridDetails.PerformCallback(-1);
    gridLichSuKyDuyet.PerformCallback(-1);
    gridAttachments.PerformCallback(-1);
    gridImportTaiSan.PerformCallback(-1);
    popUpdateForm.Show();
}

function clearForm(ReadAddEditApproval, objStatus) {

    txtObjectId.Set('hidden_value', "0");
    //cbMaChiNhanh.SetValue(branchID);

   
    cbMaNhanVienHCQT.SetValue(userStaffNo);
    //cbMaPhongBanNVHCQT.SetValue('');
    cbMaNhanVienKeToan.SetValue('');
    //cbMaPhongBanNVKeToan.SetValue('');
    cbMaPhongBanDuocKiemKe.SetValue('');
    txtGhiChu.SetText('');



    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);
    //  uplAttachment.SetEnabled(false);

    if (ReadAddEditApproval == "ReadOnly") {

    }
    else if (ReadAddEditApproval == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveEdit.SetEnabled(true);
        btnGuiDuyet.SetEnabled(true);
        btnSaveDetail.SetEnabled(true);
        // uplAttachment.SetEnabled(true);
    }
    else if (ReadAddEditApproval == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
            btnSaveEdit.SetEnabled(true);
            btnGuiDuyet.SetEnabled(true);
            btnSaveDetail.SetEnabled(true);
            // uplAttachment.SetEnabled(true);
        }
        else { // các trạng thái còn lại ko enable

        }
    }
    else if (ReadAddEditApproval == "ApprovalOnly") {
        if (objStatus == "SENDAPPROVAL") {
            btnDuyet.SetEnabled(true);
            btnKhongDuyet.SetEnabled(true);
            // uplAttachment.SetEnabled(true);// trường hợp xem duyệt cho upload file
        }

    }
    // gridImportTaiSan.PerformCallback();
}

function openEditForm(ID, xxxxx, readEditApproval) {

    var data = "mode=EDIT";
    data += "&id=" + ID;
    data += "&subMode=" + readEditApproval;

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearForm(readEditApproval, data.entity.TrangThai);
                
                txtObjectId.Set('hidden_value', ID);
                //cbMaChiNhanh.SetValue(data.entity.IDChiNhanh);
                deNgayChotDanhSach.SetText(data.entity.NgayChotDanhSach);
                cbMaNhanVienHCQT.SetValue(data.entity.MaNhanVienHCQT);
                //cbMaPhongBanNVHCQT.SetValue(data.entity.MaPhongBanNVHCQT);
                cbMaNhanVienKeToan.SetValue(data.entity.MaNhanVienKeToan);
                cbMaPhongBanDuocKiemKe.SetValue(data.entity.MaPhongBanDuocKiemKe);
                txtGhiChu.SetText(data.entity.GhiChu);
              
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                // gridAttachments.PerformCallback(txtObjectId.Get('hidden_value'));

                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}

function saveQLKiemKeKho() {


    if (cbMaNhanVienHCQT.GetValue() + "" == "") {
        alert("Chưa chọn nhân viên HCQT");
        return;
    }
    if (cbMaNhanVienKeToan.GetValue() + "" == "") {
        alert("Chưa chọn nhân viên Kế toán");
        return;
    }


    if (txtGhiChu.GetValue() + "" == "") {
        alert("Vui lòng nhập nội dung kiểm kê");
        return;
    }

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");

    data += "&NgayChotDanhSach=" + deNgayChotDanhSach.GetDate().toJSON();
    data += "&MaNhanVienHCQT=" + cbMaNhanVienHCQT.GetValue();
    // data += "&MaPhongBanNVHCQT=" + cbMaPhongBanNVHCQT.GetValue();
    data += "&MaNhanVienKeToan=" + cbMaNhanVienKeToan.GetValue();
    //  data += "&MaPhongBanNVKeToan=" + cbMaPhongBanNVKeToan.GetValue();
    data += "&MaPhongBanDuocKiemKe=" + cbMaPhongBanDuocKiemKe.GetValue();
    data += "&GhiChu=" + encodeURIComponent(txtGhiChu.GetText());


    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;

                txtObjectId.Set('hidden_value', data.id);
                gridQLKiemKeKho.PerformCallback();
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
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLKiemKeKho.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}


function SendApprovalQLKiemKeKho() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnGuiDuyet.SetEnabled(false);
    btnSaveEdit.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLKiemKeKho.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnGuiDuyet.SetEnabled(false);
                btnSaveEdit.SetEnabled(false);
                btnSaveDetail.SetEnabled(false);
            }
            else {

                btnGuiDuyet.SetEnabled(true);
                btnSaveEdit.SetEnabled(true);
                btnSaveDetail.SetEnabled(true);

            }
            alert(data.message);
        }
    });
}


function ApprovalKiemKeKho() {
    if (!confirm('Bạn có muốn Duyệt không?')) {
        return;
    }

    btnDuyet.SetEnabled(false);
    var data = "mode=Approval";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnDuyet.SetEnabled(true);
            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLKiemKeKho.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
            }
            else {

            }
            alert(data.message);
        }
    });
}

function RejectKiemKeKho() {
    if (!confirm('Bạn có muốn từ chối không?')) {
        return;
    }

    btnKhongDuyet.SetEnabled(false);
    var data = "mode=Reject";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuKhongDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnKhongDuyet.SetEnabled(true);
            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLKiemKeKho.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
            }
            else {

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

    cbKhoKiemKe.SetValue('');
    cbTaiSanChiTiet.SetValue('');
    cbTrangThaiTaiSan.SetValue('');
    deNgayMua.SetText('');
    txtDonGiaMua.SetText('');
    txtDonGiaConLai.SetText('');
    txtSoLuongSoSach.SetText('');
    txtSoLuongKiemKe.SetText('');
    txtGhiChuChiTiet.SetText('');

    // Chỉ gọi callback khi có giá trị kho để tìm kiếm
    var khoId = cbKhoKiemKe.GetValue();
    if (khoId != null && khoId != "") {
        gridDanhSachTaiSan.PerformCallback(khoId);
    }
}

function saveQLKiemKeKhoChiTiet() {

    //if (txtSoLuongMoi.GetValue() == "0") {
    //    alert("Vui lòng nhập số lượng");
    //    return;
    //}
    //if (txtSoLuongCu.GetValue() == "0" || txtSoLuongCu.GetValue() == "") {

    //}
    //else {
    //    if (cbTrangThaiCu.GetValue() == "0" || cbTrangThaiCu.GetValue() == "") {
    //        alert("Vui lòng chọn trạng thái tài sản");
    //        return;
    //    }
    //}

    if (cbTrangThaiTaiSan.GetValue() == "0" || cbTrangThaiTaiSan.GetValue() == "") {
        alert("Vui lòng chọn trạng thái tài sản");
        return;
    }

    btnSaveDetail.SetEnabled(false);
    var data = "mode=AddOrUpdateDetail";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    data += "&IDKho=" + cbKhoKiemKe.GetValue();
    data += "&IDTaiSanChiTiet=" + cbTaiSanChiTiet.GetValue();
    data += "&IDTrangThaiTaiSan=" + cbTrangThaiTaiSan.GetValue();
    data += "&NgayMua=" + deNgayMua.GetDate().toJSON();
    data += "&DonGiaMua=" + txtDonGiaMua.GetText();
    data += "&DonGiaConLai=" + txtDonGiaConLai.GetText();
    data += "&SoLuongSoSach=" + txtSoLuongSoSach.GetText();
    data += "&SoLuongKiemKe=" + txtSoLuongKiemKe.GetText();
    data += "&GhiChuChiTiet=" + encodeURIComponent(txtGhiChuChiTiet.GetText());


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
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
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: "mode=EDITDETAIL&id=" + row[0],
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectDetailId.Set("hidden_value", data.entity.ID);

                cbKhoKiemKe.SetValue(data.entity.IDKho);
                cbTaiSanChiTiet.SetValue(data.entity.IDTaiSanChiTiet);
                cbTrangThaiTaiSan.SetValue(data.entity.IDTrangThaiTaiSan);
                deNgayMua.SetText(data.entity.NgayMua);
                txtDonGiaMua.SetText(data.entity.DonGiaMua);
                txtDonGiaConLai.SetText(data.entity.DonGiaConLai);
                txtSoLuongSoSach.SetText(data.entity.SoLuongSoSach);
                txtSoLuongKiemKe.SetText(data.entity.SoLuongKiemKe);
                txtGhiChuChiTiet.SetText(data.entity.GhiChuChiTiet);


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
        url: "../Actions/QLKiemKeKhoAction.ashx",
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
/*
function openChonTaiSanKiemKeKho(id, maTaiSan, idTonKhoCu, cauHinh, tenNhaSanXuat, tenNhaCungCap, idTrangThaiCu, soLuongCu) {
    

    cbTaiSanKiemKeKho.SetValue(id);
    lbMaTaiSan.SetText(maTaiSan);
    lbIDTonKho.SetText(idTonKhoCu);
    cbTrangThaiCu.SetValue(idTrangThaiCu);
    txtSoLuongCu.SetText(soLuongCu);
    cbTrangThaiMoi.SetValue(idTrangThaiCu);
    txtSoLuongMoi.SetValue(soLuongCu);

}
*/

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

    var attId = hddAttachId.Get("hidden_value");
    var data = "mode=deleteAttachment";
    data += "&ID=" + attId;
    data += "&attId=" + attId;
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
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
        url: "../Actions/QLKiemKeKhoAction.ashx",
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


}

/* Kết thúc dành cho upload control-------------------------------------------------------------------------------------------------- */

function inKiemKeTaiSanNhanVien() {
    window.open("InBaoCao.aspx?IDKiemKeKho=" + txtObjectId.Get("hidden_value") + "&Mode=QLKiemKeKho", '_blank');
    //   location.target = "_blank";
    //  location.href = "InBaoCao.aspx?IDQLTamUng=" + txtObjectId.Get("hidden_value");
}
function inKiemKeTaiSanPhongBan() {
    window.open("InBaoCao.aspx?IDKiemKeKho=" + txtObjectId.Get("hidden_value") + "&Mode=QLKiemKeKho", '_blank');
    //   location.target = "_blank";
    //  location.href = "InBaoCao.aspx?IDQLTamUng=" + txtObjectId.Get("hidden_value");
}


// danh cho import tai san-------------------------


function SearchImport() {
    btnTraCuuImport.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: "mode=xxxx",
        complete: function () {
            btnTraCuuImport.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            gridImportTaiSan.PerformCallback(txtObjectId.Get("hidden_value"));
        }
    });
}

function TraCuuTaiSanPhongBan() {
    btnTraCuuTaiSanPhongBan.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: "mode=xxxx",
        complete: function () {
            btnTraCuuTaiSanPhongBan.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            gridImportTaiSan.PerformCallback(txtObjectId.Get("hidden_value"));
        }
    });
}

function SaveTaiSanImport() {

    if (!confirm('Bạn có muốn Lưu tài sản import ?')) {
        return;
    }
    btnLuuChiTietImport.SetEnabled(false);
    var data = "mode=SaveTaiSanImport";
    data += "&ID=" + txtObjectId.Get("hidden_value");
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: data,

        complete: function () {
            btnLuuChiTietImport.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                //gridImportTaiSan.PerformCallback();
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
            }
            else {
            }
            alert(data.message);
        }
    });
}
function DeleteImport() {

    if (!confirm('Bạn có muốn xóa tất cả dữ liệu import không?')) {
        return;
    }
    btnXoaDSImport.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLKiemKeKhoAction.ashx",
        dataType: "json",
        data: "mode=DeleteImport",
        complete: function () {
            btnXoaDSImport.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridImportTaiSan.PerformCallback(txtObjectId.Get("hidden_value"));
            }
            else {

            }
            alert(data.message);
        }
    });
}

function btnAddTaiSan_Click() {
    // Mở form thêm chi tiết tài sản
    openAddingFormDetail();
}

//-------------------------------------------------