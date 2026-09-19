// JavaScript moved from QLDonHangBanCT.aspx
// (client-only) we store newly added customer id in hidden field hddCustomerId

// debug + preserve typed value across callback
// no client-side cache
var _cbKhachDebounce = null;
var _cbDebounceMs = 5000; // debounce ms for PerformCallback
var _cbMinChars = 3; // minimum trimmed characters to trigger callback
window._cbKhachTyped = "";
window._cbKhachLastRequest = "";

// populateCbKhachFromArray removed (client-side AddItem breaks DevExpress multi-column)

// readComboItems removed (no client-side cache)

function cbKhachHang_KeyUp(s, e) {
    try {
        // read typed value robustly
        var typedRaw = ""; // raw value as user typed (preserve spaces)
        try {
            if (e && e.htmlEvent && e.htmlEvent.target && typeof e.htmlEvent.target.value !== 'undefined') {
                typedRaw = e.htmlEvent.target.value;
            } else {
                var inp = (s && s.GetInputElement) ? s.GetInputElement() : null;
                if (inp && typeof inp.value !== 'undefined') typedRaw = inp.value;
                else if (s && s.GetText) typedRaw = s.GetText();
            }
        } catch (ex) {
            typedRaw = (s && s.GetText && s.GetText()) || "";
        }

        var typed = (typedRaw || "").trim(); // use trimmed form for cache/key
        // preserve raw input for restoring cursor/visual state
        window._cbKhachTypedRaw = typedRaw || "";
        window._cbKhachTyped = typed;
        // no client-side cache logic

        // otherwise debounce and request server
        // Only perform server callback when trimmed typed is non-empty. This avoids
        // clearing the input when user deletes all text (no server round-trip needed).
        window._cbKhachLastRequest = typed;

        // do not call server for very short queries - reduces load and flicker
        if ((typed || '').length < _cbMinChars) {
            return;
        }

        if (_cbKhachDebounce) clearTimeout(_cbKhachDebounce);
        _cbKhachDebounce = setTimeout(function () {
            try {
                if (s && s.PerformCallback) s.PerformCallback(window._cbKhachLastRequest || '');
            } catch (err) {
                // ignore
            }
        }, _cbDebounceMs);
    } catch (err) {
        console.error('[cbKhachHang_KeyUp] error', err);
    }
}

function cbKhachHang_EndCallback(s, e) {
    try {
        // restore typed text (preserve raw input including spaces)
        var typedRaw = window._cbKhachTypedRaw !== undefined ? window._cbKhachTypedRaw : (window._cbKhachTyped || "");
        var inp = (s && s.GetInputElement) ? s.GetInputElement() : null;
        // If user cleared the input (raw is empty), avoid forcing SetText('') or
        // reopening the dropdown, because it can cause column-mapping/display issues
        // when client-side cache/items are incomplete. Leave control state as-is.
        if ((typedRaw || '').length > 0) {
            try { if (s && s.SetText) s.SetText(typedRaw); } catch (ex) { }
            // place caret at end of raw typed text
            if (inp && typeof inp.selectionStart !== "undefined") {
                inp.selectionStart = inp.selectionEnd = (typedRaw || "").length;
            } else {
                try { if (inp && inp.focus) inp.focus(); } catch (f) { }
            }
        } else {
            // keep focus on input but do not change text or selection
            try { if (inp && inp.focus) inp.focus(); } catch (f) { }
        }

        // restore text and show dropdown
        try {
            if ((typedRaw || '').length > 0) {
                try { if (s && s.SetText) s.SetText(typedRaw); } catch (ex) { }
                if (inp && typeof inp.selectionStart !== "undefined") {
                    inp.selectionStart = inp.selectionEnd = (typedRaw || "").length;
                } else {
                    try { if (inp && inp.focus) inp.focus(); } catch (f) { }
                }
                try { if (s && s.ShowDropDown) s.ShowDropDown(); } catch (ex) { }
            } else {
                try { if (s && s.ShowDropDown) s.ShowDropDown(); } catch (ex) { }
            }
        } catch (ex) { /* ignore */ }
    } catch (err) {
        console.error('[cbKhachHang_EndCallback] error', err);
    }
}

// Combo end-callback handler to clear reloading flag and ensure selection
// (removed fallback handler) leave empty placeholder if needed later

function getParameterByName(name) {
    name = name.replace(/[\[\]]/g, "\\$&");
    var regex = new RegExp("[?&]" + name + "(=([^&#]*)|&|#|$)"),
        results = regex.exec(window.location.href);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, " "));
}

function initLoadFromQuery() {
    var id = getParameterByName('id');
    if (!id) return;
    // clear controls first
    try { if (typeof cbKhachHang !== 'undefined' && cbKhachHang.SetValue) cbKhachHang.SetValue(null); } catch (e) { console.error(e); }
    try { if (typeof memoNotes !== 'undefined' && memoNotes.SetText) memoNotes.SetText(''); } catch (e) { console.error(e); }

    // request header via ASHX
    $.ajax({
        type: 'POST',
        url: '../Actions/QLDonHangBanAction.ashx',
        data: { mode: 'EDIT', id: id, subMode: 'EditOnly' },
        dataType: 'json',
        success: function (resp) {
            if (resp && resp.success) {
                var ent = resp.entity;
                try {
                    if (ent && (ent.id_khach_hang || ent.id_khachhang || ent.id_khach)) {
                        var v = ent.id_khach_hang || ent.id_khachhang || ent.id_khach;
                        cbKhachHang.SetValue(v);
                    } else if (ent && ent.id_khach_hang === 0) {
                        cbKhachHang.SetValue(null);
                    }
                } catch (e) { console.error('set cbKhachHang error', e); }
                try { if (ent && ent.ghi_chu) memoNotes.SetText(ent.ghi_chu); } catch (e) { console.error(e); }
                try { if (typeof txtObjectId !== 'undefined' && txtObjectId.Set) txtObjectId.Set('hidden_value', id); } catch (e) { console.error(e); }
                // load details into right grid via custom callback
                try {
                    if (typeof gridHangHoaRight !== 'undefined' && gridHangHoaRight.PerformCallback) gridHangHoaRight.PerformCallback('LOAD|' + id);
                    else if (typeof gridDonHangBanRight !== 'undefined' && gridDonHangBanRight.PerformCallback) gridDonHangBanRight.PerformCallback('LOAD|' + id);
                } catch (e) { console.error('grid load error', e); }
            }
            else {
                alert(resp ? (resp.message || 'Không thể tải dữ liệu') : 'Không thể tải dữ liệu');
            }
        },
        error: function (xhr, status, err) {
            console.error('Error loading header', err, xhr.responseText);
        }
    });
}

// ensure init runs on initial full load if pageLoad (MS AJAX) didn't fire
try {
    if (document.readyState === 'complete' || document.readyState === 'interactive') {
        setTimeout(function () { try { initLoadFromQuery(); } catch (e) { console.error(e); } }, 50);
    } else {
        window.addEventListener('load', function () { try { initLoadFromQuery(); } catch (e) { console.error(e); } });
    }
} catch (e) { console.error(e); }

function OpenAdd(id, ma, ten, price) {
    try {
        // If we are editing an existing invoice (master id present), call server to persist detail
        var masterId = null;
        try {
            if (typeof txtObjectId !== 'undefined' && txtObjectId.Get) {
                masterId = txtObjectId.Get('hidden_value') || txtObjectId.Get('hidden_value');
            }
        } catch (e) { /* ignore */ }

        // fallback to querystring id
        if (!masterId) masterId = getParameterByName('id');

        // normalize
        masterId = masterId ? masterId + '' : '';

        if (masterId && masterId !== '0') {
            // persist detail via ASHX AddOrUpDateDetail then reload grid
            $.ajax({
                type: 'POST',
                url: '../Actions/QLDonHangBanAction.ashx',
                data: {
                    mode: 'AddOrUpDateDetail',
                    ID: 0,
                    IDMaster: masterId,
                    IDTaiSanChiTiet: id,
                    IDTrangTaiSan: 0,
                    SoLuong: 1,
                    DonGiaMua: price || 0,
                    GiaTriConLai: 0,
                    GiaTriThuHoi: 0,
                    IDKhoXuat: 0,
                    PhuongAnXuLy: '',
                    GhiChuChiTiet: ''
                },
                dataType: 'json',
                success: function (resp) {
                    if (resp && resp.success) {
                        try {
                            if (gridHangHoaRight && gridHangHoaRight.PerformCallback) gridHangHoaRight.PerformCallback('LOAD|' + masterId);
                            else if (gridDonHangBanRight && gridDonHangBanRight.PerformCallback) gridDonHangBanRight.PerformCallback('LOAD|' + masterId);
                        } catch (e) { console.error('reload grid after add detail error', e); }
                    } else {
                        alert(resp ? (resp.message || 'Thêm hàng hóa thất bại') : 'Thêm hàng hóa thất bại');
                    }
                },
                error: function (xhr, status, err) {
                    console.error('Add detail AJAX error', err, xhr.responseText);
                }
            });
        }
        else {
            // No master id yet: require user to save order (with customer) before selecting products
            try {
                var custVal = null;
                if (typeof cbKhachHang !== 'undefined' && cbKhachHang.GetValue) custVal = cbKhachHang.GetValue();
                alert('Vui lòng chọn khách hàng và nhấn "Lưu tạm" để tạo đơn hàng trước khi chọn hàng hóa.');
            } catch (e) {
                console.error(e);
                alert('Vui lòng lưu đơn hàng trước khi chọn hàng hóa.');
            }
            return;
        }
    } catch (e) {
        console.error(e);
    }
}

function DeleteRightRow(index) {
    try {
        // Determine master id
        var masterId = null;
        try {
            if (typeof txtObjectId !== 'undefined' && txtObjectId.Get) masterId = txtObjectId.Get('hidden_value');
        } catch (e) { }
        if (!masterId) masterId = getParameterByName('id');

        // Try get row id (key) from grid
        var rowId = null;
        try {
            if (gridHangHoaRight && gridHangHoaRight.GetRowKey) rowId = gridHangHoaRight.GetRowKey(parseInt(index));
            else if (gridDonHangBanRight && gridDonHangBanRight.GetRowKey) rowId = gridDonHangBanRight.GetRowKey(parseInt(index));
        } catch (e) { console.error('get row key error', e); }

        // If persisted row (has id > 0) and master persisted -> call server to delete then reload
        if (masterId && masterId !== '0' && rowId && parseInt(rowId) > 0) {
            $.ajax({
                type: 'POST',
                url: '../Actions/QLDonHangBanAction.ashx',
                data: { mode: 'DELETEDETAIL', ID: rowId, IDMaster: masterId },
                dataType: 'json',
                success: function (resp) {
                    if (resp && resp.success) {
                        try {
                            if (gridHangHoaRight && gridHangHoaRight.PerformCallback) gridHangHoaRight.PerformCallback('LOAD|' + masterId);
                            else if (gridDonHangBanRight && gridDonHangBanRight.PerformCallback) gridDonHangBanRight.PerformCallback('LOAD|' + masterId);
                        } catch (e) { console.error('reload grid after delete error', e); }
                    } else {
                        alert(resp ? (resp.message || 'Xóa chi tiết thất bại') : 'Xóa chi tiết thất bại');
                    }
                },
                error: function (xhr, status, err) {
                    console.error('Delete detail AJAX error', err, xhr.responseText);
                }
            });
        }
        else {
            // fallback: remove from session/UI (unsaved)
            if (gridHangHoaRight && gridHangHoaRight.PerformCallback) {
                gridHangHoaRight.PerformCallback('DEL|' + index);
            } else if (gridDonHangBanRight && gridDonHangBanRight.PerformCallback) {
                gridDonHangBanRight.PerformCallback('DEL|' + index);
            }
        }
    } catch (e) { console.error(e); }
}

function UpdateRightCell(visibleIndex, field, value) {
    try {
        // get other value from DOM inputs
        var so = document.getElementById('so_' + visibleIndex);
        var dg = document.getElementById('dg_' + visibleIndex);
        var soVal = so ? so.value : '';
        var dgVal = dg ? dg.value : '';
        if (field === 'so_luong') soVal = value;
        if (field === 'don_gia') dgVal = value;

        // normalize numbers: remove non-numeric, replace all commas with dot
        soVal = soVal.replace(/[^0-9\.\,]/g, '').replace(/,/g, '.');
        dgVal = dgVal.replace(/[^0-9\.\,]/g, '').replace(/,/g, '.');

        if (gridHangHoaRight && gridHangHoaRight.PerformCallback) {
            var param = 'UPD|' + visibleIndex + '|' + soVal + '|' + dgVal;
            gridHangHoaRight.PerformCallback(param);
        } else if (gridDonHangBanRight && gridDonHangBanRight.PerformCallback) {
            var param = 'UPD|' + visibleIndex + '|' + soVal + '|' + dgVal;
            gridDonHangBanRight.PerformCallback(param);
        }
    } catch (e) { console.error(e); }
}

// customer popup helpers
function openAddCustomer() {
    try {
        document.getElementById('cust_so_dien_thoai').value = '';
        document.getElementById('cust_ten').value = '';
        document.getElementById('cust_email').value = '';
        document.getElementById('cust_dia_chi').value = '';
        document.getElementById('cust_phuong_xa').value = '';
        document.getElementById('cust_tinh_thanh').value = '';
        document.getElementById('cust_ghi_chu').value = '';
        document.getElementById('popupAddCustomer').style.display = 'block';
    } catch (e) { console.error(e); }
}

function closeAddCustomer() {
    try { document.getElementById('popupAddCustomer').style.display = 'none'; } catch (e) { console.error(e); }
}

function saveAddCustomer() {
    try {
        var soDienThoai = document.getElementById('cust_so_dien_thoai').value.trim();
        var ten = document.getElementById('cust_ten').value.trim();
        var email = document.getElementById('cust_email').value.trim();
        var diachi = document.getElementById('cust_dia_chi').value.trim();
        var phuong = document.getElementById('cust_phuong_xa').value.trim();
        var tinh = document.getElementById('cust_tinh_thanh').value.trim();
        var ghichu = document.getElementById('cust_ghi_chu').value.trim();

        if (!soDienThoai) { alert('Chưa nhập Số điện thoại'); return; }

        var data = {
            mode: 'AddOrUpdate',
            tableName: 'dm_khach_hang',
            id: 0,
            SoDienThoai: soDienThoai,
            TenKhachHang: ten,
            Email: email,
            DiaChi: diachi,
            PhuongXa: phuong,
            TinhThanhPho: tinh,
            GhiChu: ghichu
        };

        $.ajax({
            type: 'POST',
            url: '../Actions/QLDanhMucAction.ashx',
            data: data,
            dataType: 'json',
            success: function (resp) {
                if (resp && resp.success) {
                    var newId = resp.id;
                    try {
                        // store id in hidden field so SaveTempCall uses it; avoid PerformCallback
                        try { if (typeof hddCustomerId !== 'undefined' && hddCustomerId.Set) hddCustomerId.Set('hidden_value', newId + ''); } catch (e) { }
                        // set displayed text on combo
                        try { if (cbKhachHang && cbKhachHang.SetText) cbKhachHang.SetText(ten || soDienThoai); } catch (e) { }
                    } catch (e) { console.error('set cbKhachHang after add error', e); }
                    closeAddCustomer();
                } else {
                    alert(resp ? (resp.message || 'Lưu khách hàng thất bại') : 'Lưu khách hàng thất bại');
                }
            },
            error: function (xhr, status, err) {
                console.error('Add customer AJAX error', err, xhr.responseText);
                alert('Lỗi khi gọi server');
            }
        });

    } catch (e) { console.error(e); alert('Lỗi'); }
}

// EndCallback removed: using hidden-field client approach and server callback when needed

// Save header + use session-stored details to persist via ASHX (moved from QLDonHangBan.js)
function SaveTempCall() {
    try {
        var id = (typeof txtObjectId !== 'undefined' && txtObjectId.Get) ? txtObjectId.Get('hidden_value') : '0';
        var idKh = '';
        try { if (typeof hddCustomerId !== 'undefined' && hddCustomerId.Get) idKh = hddCustomerId.Get('hidden_value') || ''; } catch (e) { }
        if (!idKh) {
            try { if (typeof cbKhachHang !== 'undefined' && cbKhachHang.GetValue) idKh = cbKhachHang.GetValue(); } catch (e) { }
        }
        var ghiChu = (typeof memoNotes !== 'undefined' && memoNotes.GetText) ? memoNotes.GetText() : '';

        var data = 'mode=SaveTemp';
        data += '&id=' + encodeURIComponent(id);
        data += '&idKhachHang=' + encodeURIComponent(idKh);
        data += '&ghiChu=' + encodeURIComponent(ghiChu);

        if (typeof btnSaveTemp !== 'undefined' && btnSaveTemp.SetEnabled) btnSaveTemp.SetEnabled(false);

        $.ajax({
            type: 'POST',
            url: '../Actions/QLDonHangBanAction.ashx',
            data: data,
            dataType: 'json',
            success: function (resp) {
                alert(resp.message);
                if (resp.success) {
                    // set hidden id so current tab becomes edit mode, keep current UI data
                    try {
                        if (typeof txtObjectId !== 'undefined' && txtObjectId.Set) txtObjectId.Set('hidden_value', resp.id + '');
                    } catch (e) { }
                    // update browser URL to include id without reloading the page
                    try {
                        var params = new URLSearchParams(window.location.search);
                        var subMode = params.get('subMode');
                        var newUrl = window.location.pathname + '?id=' + encodeURIComponent(resp.id);
                        if (subMode) newUrl += '&subMode=' + encodeURIComponent(subMode);
                        if (window.history && window.history.replaceState) {
                            window.history.replaceState(null, null, newUrl);
                        } else {
                            window.location.hash = '#id=' + resp.id; // fallback
                        }
                    } catch (e) { console.error('update url after save error', e); }
                    // Optionally enable/disable buttons for edit mode
                    try {
                        if (typeof btnPay !== 'undefined' && btnPay.SetEnabled) btnPay.SetEnabled(true);
                    } catch (e) { }
                    // refresh right grid from server session
                    try {
                        if (typeof gridDonHangBanRight !== 'undefined' && gridDonHangBanRight.PerformCallback) gridDonHangBanRight.PerformCallback('REFRESH');
                        if (typeof gridHangHoaRight !== 'undefined' && gridHangHoaRight.PerformCallback) gridHangHoaRight.PerformCallback('REFRESH');
                    } catch (e) { }
                    // do not reload the page; user can continue editing in current tab
                }
            },
            error: function (xhr, status, err) {
                alert('Lỗi khi gọi server: ' + err);
            },
            complete: function () {
                if (typeof btnSaveTemp !== 'undefined' && btnSaveTemp.SetEnabled) btnSaveTemp.SetEnabled(true);
            }
        });
    } catch (e) { console.error(e); }
}
