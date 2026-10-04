
function openAddForm() {
    clearForm();
    gridDetails.PerformCallback(-1);
    popUpdateForm.Show();
}

function clearForm() {
    txtObjectId.Set('hidden_value', "0");
    txtMaHangHoa.SetText("");
    txtMaHangHoa.SetEnabled(true);
    txtMaVach.SetText("");
    txtTenHangHoa.SetText("");
    cbNhomHangHoa.SetValue(0);
    cbThuongHieu.SetValue(0);
    txtDinhMucTonKhoThapNhat.SetValue(0);
    txtDinhMucTonCaoThapNhat.SetValue(0);
    txtTenViTri.SetValue(0);
    txtTrongLuong.SetValue(0);
    cbDonViTrongLuong.SetValue(0);
    txtKichThuotRong.SetValue(0);
    txtKichThuotDai.SetValue(0);
    cbDonViKichThuot.SetValue(0);
    txtMoTaChiTietHangHoa.SetText("");
}

function openEditForm(ID, readEditApproval) {

    var data = "mode=EDIT";
    data += "&id=" + ID;
    data += "&subMode=" + readEditApproval;

    clearForm();

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLHangHoaAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectId.Set('hidden_value', ID);
                txtMaHangHoa.SetText(data.entity.ma_hang_hoa);
                txtMaHangHoa.SetEnabled(false);
                txtMaVach.SetText(data.entity.ma_vach);
                txtTenHangHoa.SetText(data.entity.ten_hang_hoa);
                cbNhomHangHoa.SetValue(data.entity.id_nhom_hang_hoa);
                cbThuongHieu.SetValue(data.entity.id_thuong_hieu);
                txtDinhMucTonKhoThapNhat.SetValue(data.entity.dinh_muc_ton_kho_thap_nhat);
                txtDinhMucTonCaoThapNhat.SetValue(data.entity.dinh_muc_ton_cao_thap_nhat);
                txtTenViTri.SetValue(data.entity.ten_vi_tri);
                txtTrongLuong.SetValue(data.entity.trong_luong);
                cbDonViTrongLuong.SetValue(data.entity.id_don_vi_trong_luong);
                txtKichThuotRong.SetValue(data.entity.kich_thuot_rong);
                txtKichThuotDai.SetValue(data.entity.kich_thuot_dai);
                cbDonViKichThuot.SetValue(data.entity.id_don_vi_kich_thuot);
                txtMoTaChiTietHangHoa.SetText(data.entity.mo_ta_chi_tiet_hang_hoa);

                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                popUpdateForm.Show();
            }
            else {
                alert(data.message);
            }
        }
    });
}

function saveQLHangHoa() {

    if (txtTenHangHoa.GetText() + "" == "") {
        alert("Chưa nhập Tên hàng hóa");
        return;
    }

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");

    data += "&MaHangHoa=" + encodeURIComponent(txtMaHangHoa.GetText());
    data += "&MaVach=" + encodeURIComponent(txtMaVach.GetText());
    data += "&TenHangHoa=" + encodeURIComponent(txtTenHangHoa.GetText());
    data += "&IDNhomHangHoa=" + cbNhomHangHoa.GetValue();
    data += "&IDThuongHieu=" + cbThuongHieu.GetValue();
    data += "&DinhMucTonKhoThapNhat=" + txtDinhMucTonKhoThapNhat.GetValue();
    data += "&DinhMucTonCaoThapNhat=" + txtDinhMucTonCaoThapNhat.GetValue();
    data += "&TenViTri=" + txtTenViTri.GetValue();
    data += "&TrongLuong=" + txtTrongLuong.GetValue();
    data += "&IDDonViTrongLuong=" + cbDonViTrongLuong.GetValue();
    data += "&KichThuotRong=" + txtKichThuotRong.GetValue();
    data += "&KichThuotDai=" + txtKichThuotDai.GetValue();
    data += "&IDDonViKichThuot=" + cbDonViKichThuot.GetValue();
    data += "&MoTaChiTietHangHoa=" + encodeURIComponent(txtMoTaChiTietHangHoa.GetText());

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLHangHoaAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtMaHangHoa.SetEnabled(false);
                txtObjectId.Set('hidden_value', data.id);
                gridQLHangHoa.PerformCallback();
            }
            alert(data.message);
        }
    });
}

function OpenDeleteForm(ID, tenHangHoa) {
    txtObjectId.Set('hidden_value', ID);
    $("#spTopicName").html(tenHangHoa);
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
        url: "../Actions/QLHangHoaAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLHangHoa.PerformCallback();
            }
            alert(data.message);
        }
    });
}

// DETAIL (Đơn vị tính)

function openAddingFormDetail() {
    if (txtObjectId.Get("hidden_value") == "0") {
        alert("Bạn vui lòng lưu hàng hóa trước khi tạo đơn vị tính");
        return;
    }

    clearFormDetail();
    popUpdateDetail.Show();
}

function clearFormDetail() {
    txtObjectDetailId.Set("hidden_value", "0");
    txtTenDonViTinh.SetText("");
    txtGiaTriQuiDoi.SetValue(0);
    txtGiaBan.SetValue(0);
    chkIsDonViCoBan.SetValue(false);
}

function saveQLHangHoaChiTiet() {

    if (txtTenDonViTinh.GetText() + "" == "") {
        alert("Vui lòng nhập Tên đơn vị tính");
        return;
    }
    btnSaveDetail.SetEnabled(false);
    var data = "mode=AddOrUpdateDetail";
    data += "&ID=" + txtObjectDetailId.Get("hidden_value");
    data += "&IDMaster=" + txtObjectId.Get("hidden_value");
    data += "&TenDonViTinh=" + encodeURIComponent(txtTenDonViTinh.GetText());
    data += "&GiaTriQuiDoi=" + txtGiaTriQuiDoi.GetValue();
    data += "&GiaBan=" + txtGiaBan.GetValue();
    data += "&IsDonViCoBan=" + chkIsDonViCoBan.GetValue();

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLHangHoaAction.ashx",
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
        url: "../Actions/QLHangHoaAction.ashx",
        dataType: "json",
        data: "mode=EDITDETAIL&id=" + row[0],
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectDetailId.Set("hidden_value", data.entity.id);
                txtTenDonViTinh.SetText(data.entity.ten_don_vi_tinh);
                txtGiaTriQuiDoi.SetValue(data.entity.gia_tri_qui_doi);
                txtGiaBan.SetValue(data.entity.gia_ban);
                chkIsDonViCoBan.SetValue(data.entity.is_don_vi_co_ban == "1");

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
        url: "../Actions/QLHangHoaAction.ashx",
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
