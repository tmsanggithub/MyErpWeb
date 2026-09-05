
function openAddForm() {
    clearForm("NewOnly");
    gridLichSuKyDuyet.PerformCallback(-1);
    popUpdateForm.Show();
}

function clearForm(readEdit, objStatus) {
    txtObjectId.Set('hidden_value', "0");
    txtMaTaiSan.SetText(""); txtMaTaiSanCopy.SetText("");
    txtMaTaiSan.SetEnabled(true);
    txtTenTaiSan.SetText("");
    // cbMaChiNhanh.SetValue("");
    cbDonViTinh.SetValue("");
    cbLoaiTaiSan.SetValue("");
    cbNhomTaiSan.SetValue("");
    cbNhaCungCap.SetValue("");
    cbNhaSanXuat.SetValue("");
    txtSoSerialNumber.SetText("");
    txtGiaTriTaiSan.SetText(0);
    txtCauHinh.SetText("");
    //txtSoPhieuBaoHanh.SetText("");
    txtSoThangBaoHanh.SetText("");
    //  txtChungNhanCOCQ.SetText("");
    txtTyLeHaoMon.SetText("");
    //txtSoThangHanSuDung.SetText("");

    txtGhiChu.SetText("");
    cbLoaiBaoHiem.SetValue("");

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
                cbLoaiTaiSan.SetValue(data.entity.IDLoaiTaiSan);
                cbNhomTaiSan.SetValue(data.entity.IDNhomTaiSan);
                cbNhaCungCap.SetValue(data.entity.IDNhaCungCap);
                cbNhaSanXuat.SetValue(data.entity.IDNhaSanXuat);
                cbLoaiBaoHiem.SetValue(data.entity.IDLoaiBaoHiem);
                txtSoSerialNumber.SetText(data.entity.SoSerialNumber);
                txtGiaTriTaiSan.SetValue(data.entity.GiaTriTaiSan);
                txtCauHinh.SetText(data.entity.CauHinh);
                //txtSoPhieuBaoHanh.SetText(data.entity.SoPhieuBaoHanh);
                txtSoThangBaoHanh.SetText(data.entity.SoThangBaoHanh);
                //txtChungNhanCOCQ.SetText(data.entity.ChungNhanCOCQ);
                txtTyLeHaoMon.SetText(data.entity.TyLeHaoMon);
                //txtSoThangHanSuDung.SetText(data.entity.SoThangHanSuDung);
                //deNamSanXuat.SetText(data.entity.NamSanXuat);
                deNamDuaVaoSuDung.SetText(data.entity.NamDuaVaoSuDung);
                deNgayHetHan.SetText(data.entity.NgayHetHan);
                txtGhiChu.SetText(data.entity.GhiChu);
                gridLichSuKyDuyet.PerformCallback(txtObjectId.Get("hidden_value"));
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                popUpdateForm.Show();
            }
        }
    });
}


function saveTaiSan() {

    if (txtMaTaiSan.GetText() + "" == "") {
        alert("Chưa nhập Mã TaiSan");
        return;
    }
    if (txtTenTaiSan.GetText() + "" == "") {
        alert("Chưa nhập Tên TaiSan");
        return;
    }
    if (txtGhiChu.GetText() + "" == "") {
        alert("Chưa nhập Ghi chú");
        return;
    }
    if (cbDonViTinh.GetValue() + "" == "null" || cbDonViTinh.GetValue() + "" == "") {
        alert("Chưa nhập Đơn vị tính");
        return;
    }
    if (cbLoaiTaiSan.GetValue() + "" == "null" || cbLoaiTaiSan.GetValue() + "" == "") {
        alert("Chưa nhập Loại tài sản");
        return;
    }
    if (cbNhomTaiSan.GetValue() + "" == "null" || cbNhomTaiSan.GetValue() + "" == "") {
        alert("Chưa nhập Nhóm tài sản");
        return;
    }
    if (cbLoaiBaoHiem.GetValue() + "" == "null" || cbLoaiBaoHiem.GetValue() + "" == "") {
        alert("Chưa nhập Loại bảo hiểm");
        return;
    }
    if (deNamDuaVaoSuDung.GetText() + "" == "null" || deNamDuaVaoSuDung.GetText() + "" == "") {
        alert("Chưa nhập năm sử dụng");
        return;
    }
    if (txtSoSerialNumber.GetText() == "") {
        alert("Chưa nhập số serial (nếu không có nhập số 0)");
        return;
    }
    if (parseFloat(txtGiaTriTaiSan.GetText()) < 0) {
        alert("Giá trị tài sản không hợp lệ");
        return;
    }

   // if (parseFloat(txtGiaTriTaiSan.GetValue()) >= 30000000 && cbNhomTaiSan.GetValue() + "" != "1") {
   //     if (confirm("Giá trị tài sản >=30.000.000, bạn có muốn chuyển TS qua nhóm TSCĐ không?")) {
   //         cbNhomTaiSan.SetValue(1);
   //     }
   // }

    if (parseFloat(txtSoThangBaoHanh.GetText()) < 0) {
        alert("Số tháng bảo hành không hợp lệ");
        return;
    }

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&tableName=DMTaiSan";

    data += "&MaTaiSan=" + txtMaTaiSan.GetText();
    data += "&TenTaiSan=" + encodeURIComponent(txtTenTaiSan.GetText());
    data += "&IDDonViTinh=" + cbDonViTinh.GetValue();
    data += "&IDLoaiTaiSan=" + cbLoaiTaiSan.GetValue();
    data += "&IDNhomTaiSan=" + cbNhomTaiSan.GetValue();
    data += "&IDNhaSanXuat=" + cbNhaSanXuat.GetValue();
    data += "&IDNhaCungCap=" + cbNhaCungCap.GetValue();
    data += "&IDLoaiBaoHiem=" + cbLoaiBaoHiem.GetValue();
    data += "&SoSerialNumber=" + txtSoSerialNumber.GetText();
    data += "&GiaTriTaiSan=" + txtGiaTriTaiSan.GetValue();
    data += "&CauHinh=" + encodeURIComponent(txtCauHinh.GetText());
    // data += "&SoPhieuBaoHanh=" + txtSoPhieuBaoHanh.GetValue();
    data += "&SoThangBaoHanh=" + txtSoThangBaoHanh.GetValue();
    //data += "&SoThangHanSuDung=" + txtSoThangHanSuDung.GetValue();
    data += "&TyLeHaoMon=" + txtTyLeHaoMon.GetText();
    // data += "&ChungNhanCOCQ=" + txtChungNhanCOCQ.GetText();
    // data += "&NamSanXuat=" + deNamSanXuat.GetDate().toJSON();
    data += "&NamDuaVaoSuDung=" + deNamDuaVaoSuDung.GetDate().toJSON();
    if (deNgayHetHan.GetText() + "" != "null" && deNgayHetHan.GetText() + "" != "") {
        data += "&NgayHetHan=" + deNgayHetHan.GetDate().toJSON();
    }
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
            if (data.success) {;
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
    data += "&IDLoaiBaoHiem=" + cbLoaiBaoHiem.GetValue();

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

                //txtObjectId.Set('hidden_value', "0");

                txtMaTaiSan.SetText(data.entity.MaTaiSan);
                txtTenTaiSan.SetText(data.entity.TenTaiSan);
                cbDonViTinh.SetValue(data.entity.IDDonViTinh);

                cbLoaiTaiSan.SetValue(data.entity.IDLoaiTaiSan);
                cbNhomTaiSan.SetValue(data.entity.IDNhomTaiSan);
                cbNhaCungCap.SetValue(data.entity.IDNhaCungCap);
                cbNhaSanXuat.SetValue(data.entity.IDNhaSanXuat);
                cbLoaiBaoHiem.SetValue(data.entity.IDLoaiBaoHiem);

                txtSoSerialNumber.SetText(data.entity.SoSerialNumber);
                txtGiaTriTaiSan.SetValue(data.entity.GiaTriTaiSan);
                txtCauHinh.SetText(data.entity.CauHinh);
                //txtSoPhieuBaoHanh.SetText(data.entity.SoPhieuBaoHanh);
                txtSoThangBaoHanh.SetText(data.entity.SoThangBaoHanh);
                // txtChungNhanCOCQ.SetText(data.entity.ChungNhanCOCQ);
                txtTyLeHaoMon.SetText(data.entity.TyLeHaoMon);
                //txtSoThangHanSuDung.SetText(data.entity.SoThangHanSuDung);

                // deNamSanXuat.SetText(data.entity.NamSanXuat);
                deNamDuaVaoSuDung.SetText(data.entity.NamDuaVaoSuDung);
                deNgayHetHan.SetText(data.entity.NgayHetHan);

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
        alert("Bạn vui lòng lưu cài đặt trước khi tạo chi tiết");
        return;
    }

    // lastFunctionId = null;;
    clearFormDetail("NewOnly");
    // cbMaMonHoc.PerformCallback("add|" + txtObjectId.Get("hidden_value"));
    popUpdateDetail.Show();
}

function clearFormDetail(readEditDetail, objStatusDetail) {
    txtObjectDetailId.Set("hidden_value", "0");
    txtGhiChuThayDoi.SetText("");
    txtNoiDungThayDoi.SetText("");
    txtGhiChuDuyetKhongDuyetThayDoi.SetText("");
    deNgayThayDoi.SetDate(new Date());
    gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));


    btnSaveThayDoi.SetEnabled(false);
    btnGuiDuyetThayDoi.SetEnabled(false);
    btnApprovalThayDoi.SetEnabled(false);
    btnRejectThayDoi.SetEnabled(false);

    if (readEditDetail == "ReadOnly") {

    }
    else if (readEditDetail == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveThayDoi.SetEnabled(true);
        btnGuiDuyetThayDoi.SetEnabled(true);
    }
    else if (readEditDetail == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatusDetail == "NEW" || objStatusDetail == "EDIT" || objStatusDetail == "RETURNED" || objStatusDetail == "") {
            btnSaveThayDoi.SetEnabled(true);
            btnGuiDuyetThayDoi.SetEnabled(true);
        }
        else { // các trạng thái còn lại ko enable

        }
    }
    else if (readEditDetail == "ApprovalOnly") {
        if (objStatusDetail == "SENDAPPROVAL") {
            btnApprovalThayDoi.SetEnabled(true);
            btnRejectThayDoi.SetEnabled(true);
        }
    }


}

function saveQLTaiSanThayDoiCT() {

    if (txtNoiDungThayDoi.GetText() == "") {
        alert("Vui lòng nhập nội dung thay đổi");
        return;
    }
    if (deNgayThayDoi.GetText() + "" == "null" || deNgayThayDoi.GetText() + "" == "") {
        alert("Chưa nhập ngày thay đổi");
        return;
    }
    btnSaveThayDoi.SetEnabled(false);
    var data = "mode=AddOrUpdateThayDoiTS";
    data += "&tableName=DMTaiSanCT";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");
    data += "&NoiDungThayDoi=" + txtNoiDungThayDoi.GetText();
    data += "&GhiChuThayDoi=" + txtGhiChuThayDoi.GetText();
    data += "&NgayThayDoi=" + deNgayThayDoi.GetDate().toJSON();
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

                // popUpdateDetail.Hide();
            }
            alert(data.message);
        }
    });
}

function openEditFormDetail(ID, noiDungThayDoi, readEditApproval) {

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

                txtNoiDungThayDoi.SetValue(data.entity.NoiDungThayDoi);
                txtGhiChuThayDoi.SetValue(data.entity.GhiChu);
                deNgayThayDoi.SetText(data.entity.NgayThayDoi);
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


