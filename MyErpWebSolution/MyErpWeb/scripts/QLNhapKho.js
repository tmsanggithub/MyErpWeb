
function openAddForm() {
    clearForm("NewOnly");
    gridDetails.PerformCallback(-1);
    gridLichSuKyDuyet.PerformCallback(-1);
    popUpdateForm.Show();
}

function clearForm(ReadAddEditApproval, objStatus) {
    txtObjectId.Set('hidden_value', "0");
    cbMaChiNhanh.SetValue(branchID);
    cbKhoNhap.SetValue("");
    cbNhanVienPhieu.SetValue(userLogin);
    txtGhiChu.SetText("");

    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
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
        url: "../Actions/QLNhapKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                clearForm(readEditApproval, data.entity.TrangThai);

                txtObjectId.Set('hidden_value', ID);
                cbMaChiNhanh.SetValue(data.entity.IDChiNhanh);
                cbKhoNhap.SetValue(data.entity.IDKhoNhap);
                cbNhanVienPhieu.SetValue(data.entity.NhanSuPhieu);
                deNgayNhap.SetText(data.entity.NgayPhieu);
                txtGhiChu.SetText(data.entity.GhiChu);

                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}

function saveQLNhapKho() {

    if (cbKhoNhap.GetValue() + "" == "") {
        alert("Chưa chọn Kho nhập");
        return;
    }

    if (cbMaChiNhanh.GetValue() + "" == "null" || cbMaChiNhanh.GetValue() + "" == "") {
        alert("Chưa nhập chi nhánh");
        return;
    }

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");

    data += "&IDChiNhanh=" + cbMaChiNhanh.GetValue();
    data += "&IDKhoNhap=" + cbKhoNhap.GetValue();
    data += "&NgayPhieu=" + deNgayNhap.GetDate().toJSON();
    data += "&NhanSuPhieu=" + cbNhanVienPhieu.GetValue();
    data += "&GhiChu=" + encodeURIComponent(txtGhiChu.GetText());

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhapKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;

                txtObjectId.Set('hidden_value', data.id);
                gridQLNhapKho.PerformCallback();
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
        url: "../Actions/QLNhapKhoAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLNhapKho.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SendApprovalQLNhapKho() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnSaveDetail.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhapKhoAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLNhapKho.PerformCallback();
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

function ApprovalNhapKho() {
    if (!confirm('Bạn có muốn Duyệt không?')) {
        return;
    }

    btnDuyet.SetEnabled(false);
    var data = "mode=Approval";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuDuyet=" + encodeURIComponent(txtGhiChuDuyetKhongDuyet.GetValue());


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhapKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnDuyet.SetEnabled(true);
            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLNhapKho.PerformCallback();
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
            }
            else {

            }
            alert(data.message);
        }
    });
}

function RejectNhapKho() {
    if (!confirm('Bạn có muốn từ chối không?')) {
        return;
    }

    btnKhongDuyet.SetEnabled(false);
    var data = "mode=Reject";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&GhiChuKhongDuyet=" +encodeURIComponent( txtGhiChuDuyetKhongDuyet.GetValue());


    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhapKhoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnKhongDuyet.SetEnabled(true);
            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLNhapKho.PerformCallback();
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
    cbTaiSanNhapKho.SetValue("");
    lbSoSerialNumber.SetText("");
    lbCauHinhTaiSan.SetText("");
    //lbDonViTinh.SetText("");
    //lbNhomTaiSan.SetText("");
    lbMaTaiSan.SetText("");
    //lbNhaSanXuat.SetText("");
    //lbNhaCungCap.SetText("");
    //lbSoThangBaoHanh.SetText("");
    //lbNamSanXuat.SetText("");
    txtSoLuong.SetText(1);
   // txtDonGia.SetText(0);
   // txtThanhTien.SetText(0);
    txtGhiChuChiTiet.SetText("");
    cbTrangThaiTaiSan.SetValue("");
    gridDanhSachTaiSan.PerformCallback(txtObjectId.Get("hidden_value"));
}

function saveQLNhapKhoChiTiet() {

    if (txtSoLuong.GetValue() == "0") {
        alert("Vui lòng nhập số lượng");
        return;
    }
    if (cbTrangThaiTaiSan.GetValue() == "0" || cbTrangThaiTaiSan.GetValue() == "") {
        alert("Vui lòng chọn trạng thái tài sản");
        return;
    }

    btnSaveDetail.SetEnabled(false);
    var data = "mode=AddOrUpdateDetail";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");
    data += "&IDTaiSanChiTiet=" + cbTaiSanNhapKho.GetValue();
    data += "&SoLuong=" + txtSoLuong.GetValue();
    data += "&DonGia=0";// + txtDonGia.GetValue();
    data += "&ThanhTien=0";//+ txtThanhTien.GetValue();
    data += "&TrangThaiTaiSan=" + cbTrangThaiTaiSan.GetValue();
    data += "&GhiChuChiTiet=" + encodeURIComponent(txtGhiChuChiTiet.GetText());

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhapKhoAction.ashx",
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
        url: "../Actions/QLNhapKhoAction.ashx",
        dataType: "json",
        data: "mode=EDITDETAIL&id=" + row[0],
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectDetailId.Set("hidden_value", data.entity.ID);
                cbTaiSanNhapKho.SetValue(data.entity.IDTaiSanChiTiet);
                txtSoLuong.SetValue(data.entity.SoLuong);
               // txtDonGia.SetValue(data.entity.DonGia);
               // txtThanhTien.SetValue(data.entity.ThanhTien);
                txtGhiChuChiTiet.SetText(data.entity.GhiChuChiTiet);
                cbTrangThaiTaiSan.SetValue(data.entity.TrangThaiTaiSan);
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
        url: "../Actions/QLNhapKhoAction.ashx",
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

function openChonTaiSanNhapKho(id, maTaiSan, soSerialNumber, cauHinh, tenNhaSanXuat, tenNhaCungCap) {

    cbTaiSanNhapKho.SetValue(id);
    lbMaTaiSan.SetText(maTaiSan);
    //lbNhaSanXuat.SetText(tenNhaSanXuat);
    //lbNhaCungCap.SetText(tenNhaCungCap);
    lbSoSerialNumber.SetText(soSerialNumber);
    lbCauHinhTaiSan.SetText(cauHinh);

    // Eval("ID") %>','<%# Eval("MaTaiSan") %>','<%# Eval("SoSerialNumber") %>','<%# Eval("CauHinh") %>','<%# Eval("TenNhaSanXuat") %>','<%# Eval("TenNhaCungCap") %>')"

}

function cbTaiSanNhapKhoValueChanged() {

    lbMaTaiSan.SetText(cbTaiSanNhapKho.GetSelectedItem(1).GetColumnText("MaTaiSan"));
   // lbNhaSanXuat.SetText(cbTaiSanNhapKho.GetSelectedItem(1).GetColumnText("TenNhaSanXuat"));
   // lbNhaCungCap.SetText(cbTaiSanNhapKho.GetSelectedItem(1).GetColumnText("TenNhaCungCap"));
    lbSoSerialNumber.SetText(cbTaiSanNhapKho.GetSelectedItem(1).GetColumnText("SoSerialNumber"));
    lbCauHinhTaiSan.SetText(cbTaiSanNhapKho.GetSelectedItem(1).GetColumnText("CauHinh"));

}

//function OnThanhTienChanged(s) {
//    var thanhTien = s.GetValue();
//    var soLuong = parseFloat(txtSoLuong.GetValue());
//    var donGia = 0;
//    if (soLuong > 0)
//        donGia = thanhTien / soLuong;
//    txtDonGia.SetValue(donGia);
//}
//function OnDonGiaChanged(s) {
//    var soLuong = parseFloat(txtSoLuong.GetValue());
//    var donGia = s.GetValue();
//    var thanhTien = donGia * soLuong;
//    txtThanhTien.SetValue(thanhTien);
//}
//function OnSoLuongChanged(s) {

//    var soLuong = s.GetValue();
//    var donGia = parseFloat(txtDonGia.GetValue());
//    var thanhTien = donGia * soLuong;
//    txtThanhTien.SetValue(thanhTien);
//}
 
function SearchImport() {

     
    btnTraCuuImport.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLNhapKhoAction.ashx",
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
        url: "../Actions/QLNhapKhoAction.ashx",
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
        url: "../Actions/QLNhapKhoAction.ashx",
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

