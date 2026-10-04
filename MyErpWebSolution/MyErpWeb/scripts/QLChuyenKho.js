
function openAddForm() {

    clearForm("NewOnly");
    gridDetails.PerformCallback(-1);
    gridLichSuKyDuyet.PerformCallback(-1);
    
    // Load warehouse list based on current selected branch
    if (cbMaChiNhanh.GetValue() != null && cbMaChiNhanh.GetValue() != "") {
        cbKhoTraCuu.PerformCallback(cbMaChiNhanh.GetValue().toString());
    }
    
    popUpdateForm.Show();
}

function clearForm(ReadAddEditApproval, objStatus, IDTruongDonViChuyen, idTruongDonViNhan) {
    txtObjectIDKhoTraCuu.Set('hidden_value', "0");
    txtObjectId.Set('hidden_value', "0");
    txtSoPhieuChuyen.SetText("");
    cbTruongDonViChuyen.SetValue("");
  //  lbThongBaoImport.SetText("");
    //default----------------------------------------------------------------------
    cbMaChiNhanh.SetValue(branchID);
    cbPhongBanChuyen.SetValue(userLoginDept);
    cbTruongDonViChuyen.SetValue(userManagerId);// de load default TDV theo phong ban
    cbNhanVienChuyen.SetValue(userLoginID);
    //-------------------------------------------------------------------------------
    cbPhongBanNhan.SetValue("");
    cbNhanVienNhan.SetValue("");
    cbTruongDonViNhan.SetValue("");

    txtGhiChu.SetText("");

    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    //btnDuyet.SetEnabled(false);
    //btnKhongDuyet.SetEnabled(false);
    //btnDuyetNhanTaiSan.SetEnabled(false);
    //btnKhongDuyetNhanTaiSan.SetEnabled(false);
    //btnNhapKhoTSTraLai.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);

    if (ReadAddEditApproval == "ReadOnly") {

    }
    else if (ReadAddEditApproval == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveEdit.SetEnabled(true);
        btnGuiDuyet.SetEnabled(true);
        btnSaveDetail.SetEnabled(true);
    }
    else if (ReadAddEditApproval == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
            btnSaveEdit.SetEnabled(true);
            btnGuiDuyet.SetEnabled(true);
            btnSaveDetail.SetEnabled(true);
        }
        else { // các trạng thái còn lại ko enable

        }
    }
    //else if (ReadAddEditApproval == "ApprovalOnly") {
    //    if (objStatus == "SENDAPPROVAL" && userLoginID == IDTruongDonViChuyen) {
    //        btnDuyet.SetEnabled(true);  // xem có đúng trưởng đơn vị giao ts ko
    //        btnKhongDuyet.SetEnabled(true); // xem có đúng trưởng đơn vị giao tk ko
    //    }
    //    else if (objStatus == "WAITACCEPT" && userLoginID == idTruongDonViNhan) {
    //        btnDuyetNhanTaiSan.SetEnabled(true);  // xem có đúng trưởng đơn vị nhận ts ko
    //        btnKhongDuyetNhanTaiSan.SetEnabled(true); // xem có đúng trưởng đơn vị nhận ts ko
    //    }
    //    else if (objStatus == "NOTACCEPT" && userLoginID == IDTruongDonViChuyen) {
    //        btnNhapKhoTSTraLai.SetEnabled(true); // xem có đúng trưởng đơn vị giao ts ko

    //    }
    //}
}


function saveQLChuyenKho() {

    if (cbPhongBanChuyen.GetValue() + "" == "") { alert("Chưa chọn Đơn vị giao"); return; }
    if (cbNhanVienChuyen.GetValue() + "" == "") { alert("Chưa chọn Người giao"); return; }
    if (cbTruongDonViChuyen.GetValue() + "" == "") { alert("Chưa chọn Trưởng đơn vị giao"); return; }
    if (cbPhongBanNhan.GetValue() + "" == "") { alert("Chưa chọn Đơn vị nhận"); return; }
    if (cbNhanVienNhan.GetValue() + "" == "") { alert("Chưa chọn Người nhận"); return; }
    if (cbTruongDonViNhan.GetValue() + "" == "") { alert("Chưa chọn Trưởng đơn vị nhận"); return; }


    if (cbMaChiNhanh.GetValue() + "" == "null" || cbMaChiNhanh.GetValue() + "" == "") {
        alert("Chưa nhập chi nhánh");
        return;
    }

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&IDChiNhanh=" + cbMaChiNhanh.GetValue();
    data += "&NgayChuyen=" + deNgayChuyen.GetDate().toJSON();

    data += "&IDDonViChuyen=" + cbPhongBanChuyen.GetValue();
    data += "&IDNguoiChuyen=" + cbNhanVienChuyen.GetValue();
    data += "&IDTruongDonViChuyen=" + cbTruongDonViChuyen.GetValue();
    data += "&IDDonViNhan=" + cbPhongBanNhan.GetValue();
    data += "&IDNguoiNhan=" + cbNhanVienNhan.GetValue();
    data += "&IDTruongDonViNhan=" + cbTruongDonViNhan.GetValue();
 
    data += "&GhiChu=" + encodeURIComponent(txtGhiChu.GetText());

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;

                txtObjectId.Set('hidden_value', data.id);
                gridQLChuyenKho.PerformCallback();
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
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLChuyenKho.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function openEditForm(ID, xxxxx, readEditApproval) {

    var data = "mode=EDIT";
    data += "&id=" + ID;
    data += "&subMode=" + readEditApproval;

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearForm(readEditApproval, data.entity.TrangThai, data.entity.IDTruongDonViChuyen, data.entity.IDTruongDonViNhan);
                txtSoPhieuChuyen.SetText(data.entity.SoPhieu);
                txtObjectId.Set('hidden_value', ID);
                cbMaChiNhanh.SetValue(data.entity.IDChiNhanh);

                cbPhongBanChuyen.SetValue(data.entity.IDDonViChuyen);
                cbNhanVienChuyen.SetValue(data.entity.IDNguoiChuyen);
                cbTruongDonViChuyen.SetValue(data.entity.IDTruongDonViChuyen);

                cbPhongBanNhan.SetValue(data.entity.IDDonViNhan);
                cbNhanVienNhan.SetValue(data.entity.IDNguoiNhan);
                cbTruongDonViNhan.SetValue(data.entity.IDTruongDonViNhan);

                deNgayChuyen.SetText(data.entity.NgayChuyen);
                txtGhiChu.SetText(data.entity.GhiChu);

                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                
                // Load warehouse list based on the record's branch
                if (data.entity.IDChiNhanh != null && data.entity.IDChiNhanh != "") {
                    cbKhoTraCuu.PerformCallback(data.entity.IDChiNhanh.toString());
                }
                
                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}


function SendApprovalQLChuyenKho() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnGuiDuyet.SetEnabled(false);
    btnSaveEdit.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {


        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLChuyenKho.PerformCallback();
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

/*
function ApprovalChuyenKho() {
    if (!confirm('Bạn có muốn Duyệt không?')) {
        return;
    }

    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
    var data = "mode=Approval";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLChuyenKho.PerformCallback();
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

function RejectChuyenKho() {
    if (!confirm('Bạn có muốn từ chối không?')) {
        return;
    }

    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
    var data = "mode=Reject";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuKhongDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLChuyenKho.PerformCallback();
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



function ApprovalNhanTaiSanBanGiao() {
    if (!confirm('Bạn có muốn đồng ý nhận tài sản không?')) {
        return;
    }

    btnDuyetNhanTaiSan.SetEnabled(false);
    btnKhongDuyetNhanTaiSan.SetEnabled(false);
    var data = "mode=ApprovalAcceptAsset";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLChuyenKho.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnDuyetNhanTaiSan.SetEnabled(false);
                btnKhongDuyetNhanTaiSan.SetEnabled(false);
            }
            else {
                btnDuyetNhanTaiSan.SetEnabled(true);
                btnKhongDuyetNhanTaiSan.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function RejectNhanTaiSanBanGiao() {
    if (!confirm('Bạn có muốn từ chối nhận tài sản không?')) {
        return;
    }

    btnDuyetNhanTaiSan.SetEnabled(false);
    btnKhongDuyetNhanTaiSan.SetEnabled(false);
    var data = "mode=RejectAcceptAsset";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuKhongDuyet=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLChuyenKho.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnDuyetNhanTaiSan.SetEnabled(false);
                btnKhongDuyetNhanTaiSan.SetEnabled(false);
            }
            else {
                btnDuyetNhanTaiSan.SetEnabled(true);
                btnKhongDuyetNhanTaiSan.SetEnabled(true);
            }
            alert(data.message);
        }
    });
}

function NhapKhoTaiSanKhongNhanBanGiao() {
    if (!confirm('Bạn có muốn nhập kho tài sản trả lại không ?')) {
        return;
    }
    btnNhapKhoTSTraLai.SetEnabled(false);

    var data = "mode=NhapKhoTaiSanTraLai";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuNhapKhoTSTraLai=" + txtGhiChuDuyetKhongDuyet.GetText();


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLChuyenKho.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                btnNhapKhoTSTraLai.SetEnabled(false);

            }
            else {
                btnNhapKhoTSTraLai.SetEnabled(true);

            }
            alert(data.message);
        }
    });
}
*/
// DETAIL



function openAddingFormDetail() {
    if (txtObjectId.Get("hidden_value") == "0") {
        alert("Bạn vui lòng lưu cài đặt trước khi tạo chi tiết");
        return;
    }
    clearFormDetail();
    
    // Load warehouse list based on current selected branch
    if (cbMaChiNhanh.GetValue() != null && cbMaChiNhanh.GetValue() != "") {
        cbKhoTraCuu.PerformCallback(cbMaChiNhanh.GetValue().toString());
    }
    
    popUpdateDetail.Show();
}

function clearFormDetail() {
    txtObjectDetailId.Set("hidden_value", "0");
    cbTaiSanChuyenKho.SetValue("");
    lbMaTaiSan.SetText("");
    cbKhoXuat.SetValue("");
    cbKhoNhap.SetValue("");
    cbTrangThaiTaiSanXuatKho.SetValue("");
    cbTrangThaiTaiSanNhapKho.SetValue("");
    txtSoLuong.SetText(0);
    cbKhoTraCuu.SetValue("");

    txtGhiChuChiTiet.SetText("");
    gridDanhSachTaiSan.PerformCallback(txtObjectId.Get("hidden_value"));
}

function saveQLChuyenKhoChiTiet() {

    if (txtSoLuong.GetValue() == "0") {
        alert("Vui lòng nhập số lượng");
        return;
    }
    if (cbTrangThaiTaiSanXuatKho.GetValue() == "0" || cbTrangThaiTaiSanXuatKho.GetValue() == "") {
        alert("Vui lòng chọn trạng thái tài sản Xuất kho");
        return;
    }
    if (cbTrangThaiTaiSanNhapKho.GetValue() == "0" || cbTrangThaiTaiSanNhapKho.GetValue() == "") {
        alert("Vui lòng chọn trạng thái tài sản Nhân viên");
        return;
    }

    btnSaveDetail.SetEnabled(false);
    var data = "mode=AddOrUpdateDetail";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");
    data += "&IDKhoXuat=" + cbKhoXuat.GetValue();
    data += "&IDTaiSanChiTiet=" + cbTaiSanChuyenKho.GetValue();
    data += "&SoLuong=" + txtSoLuong.GetValue();
    data += "&IDKhoNhap=" + cbKhoNhap.GetValue();// + txtThanhTien.GetValue();
    data += "&TrangThaiCu=" + cbTrangThaiTaiSanXuatKho.GetValue();
    data += "&TrangThaiMoi=" + cbTrangThaiTaiSanNhapKho.GetValue();
    data += "&GhiChuChiTiet=" + encodeURIComponent(txtGhiChuChiTiet.GetText());

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
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
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: "mode=EDITDETAIL&id=" + row[0],
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectDetailId.Set("hidden_value", data.entity.ID);
                cbTaiSanChuyenKho.SetValue(data.entity.IDTaiSanChiTiet);
                txtSoLuong.SetText(data.entity.SoLuong);
                txtGhiChuChiTiet.SetText(data.entity.GhiChuChiTiet);
                cbKhoXuat.SetValue(data.entity.IDKhoXuat);
                cbKhoNhap.SetValue(data.entity.IDKhoNhap);
                cbTrangThaiTaiSanXuatKho.SetValue(data.entity.TrangThaiCu);
                cbTrangThaiTaiSanNhapKho.SetValue(data.entity.TrangThaiMoi);
            
                // Load warehouse list based on current selected branch
                if (cbMaChiNhanh.GetValue() != null && cbMaChiNhanh.GetValue() != "") {
                    cbKhoTraCuu.PerformCallback(cbMaChiNhanh.GetValue().toString());
                }
            
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

    var data = "mode=deleteDetail";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");

    btnConfirmDeleteDetail.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
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
// chọn trên lươí
function openChonTaiSanChuyenKho(id, maTaiSan, soSerialNumber, cauHinh, tenNhaSanXuat, tenNhaCungCap, idKhoLuuHienTai, idTrangThaiTaiSanKho) {

    cbTaiSanChuyenKho.SetValue(id);
    lbMaTaiSan.SetText(maTaiSan);
    cbKhoXuat.SetValue(idKhoLuuHienTai);
    cbTrangThaiTaiSanXuatKho.SetValue(idTrangThaiTaiSanKho);
    cbTrangThaiTaiSanNhapKho.SetValue(idTrangThaiTaiSanKho);
    cbKhoTraCuu.SetValue("")
}
// khi chọn tài sản từ combo
//function cbTaiSanChuyenKhoValueChanged() {

//    lbMaTaiSan.SetText(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("MaTaiSan"));
//    //  lbNhaSanXuat.SetText(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("TenNhaSanXuat"));
//    //    lbNhaCungCap.SetText(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("TenNhaCungCap"));
//    //  lbSoSerialNumber.SetText(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("SoSerialNumber"));
//    //    lbCauHinhTaiSan.SetText(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("CauHinh"));
//    cbKhoXuat.SetValue(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("IDKhoLuuHienTai"));
//    cbTrangThaiTaiSanXuatKho.SetValue(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("IDTrangThaiTaiSanKho"));
//    cbTrangThaiTaiSanNV.SetValue(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("IDTrangThaiTaiSanKho"));

//    //    cbKhoXuat.SetValue(cbTaiSanChuyenKho.GetSelectedItem(1).GetColumnText("IDKhoLuuHienTai"));

//}

function OnPhongBanGiaoChanged(s, e) {
    cbNhanVienGiao.PerformCallback(s.GetSelectedItem().value.toString());
    cbTruongDonViGiao.PerformCallback(s.GetSelectedItem().value.toString());
    // cbTruongDonViGiao.SetValue(cbPhongBanGiao.GetSelectedItem(1).GetColumnText("ManagerId"));
    // alert(cbPhongBanGiao.GetSelectedItem(0).GetColumnText("ManagerId"));
}

function OnPhongBanNhanChanged(s, e) {
    cbNhanVienNhan.PerformCallback(s.GetSelectedItem().value.toString());
    cbTruongDonViNhan.PerformCallback(s.GetSelectedItem().value.toString());
    // cbTruongDonViNhan.SetValue(cbPhongBanNhan.GetSelectedItem(1).GetColumnText("ManagerId"));
}

function OnChiNhanhChanged(s, e) {
    cbPhongBanNhan.PerformCallback(s.GetSelectedItem().value.toString());
    cbKhoTraCuu.PerformCallback(s.GetSelectedItem().value.toString());
}

function cbKhoTraCuuValueChanged() {

    //lbMaTaiSan.SetText(cbTaiSanThanhLy.GetSelectedItem(1).GetColumnText("MaTaiSan"));
    //lbNhaSanXuat.SetText(cbTaiSanThanhLy.GetSelectedItem(1).GetColumnText("TenNhaSanXuat"));
    //lbNhaCungCap.SetText(cbTaiSanThanhLy.GetSelectedItem(1).GetColumnText("TenNhaCungCap"));
    //lbSoSerialNumber.SetText(cbTaiSanThanhLy.GetSelectedItem(1).GetColumnText("SoSerialNumber"));
    //lbCauHinhTaiSan.SetText(cbTaiSanThanhLy.GetSelectedItem(1).GetColumnText("CauHinh"));

    txtObjectIDKhoTraCuu.Set("hidden_value", cbKhoTraCuu.GetValue());
    gridDanhSachTaiSan.PerformCallback();
}

// danh cho import ts ban giao----------------------------------------------------------------------------------------------
function SearchImport() {
    btnTraCuuImport.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: "mode=TraCuuImport",
        complete: function () {
            btnTraCuuImport.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridImportTaiSan.PerformCallback();
            }
            else {

            }
        }
    });
}


function XoaImport() {
    if (!confirm('Bạn có muốn xóa tất cả dữ liệu import không?')) {
        return;
    }
    btnXoaImport.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: "mode=deleteimport",
        complete: function () {
            btnXoaImport.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridImportTaiSan.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SaveTaiSanImport() {

    btnSaveTSImport.SetEnabled(false);

    var data = "mode=SaveTaiSanImport";
    data += "&id=" + txtObjectId.Get("hidden_value");

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLChuyenKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnSaveTSImport.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
            }
            alert(data.message);
        }
    });
}
// ket thuc cho import ------------------------------------------------------------------------------------------------------