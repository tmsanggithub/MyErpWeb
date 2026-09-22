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


// ensure init runs on initial full load if pageLoad (MS AJAX) didn't fire
try {
    if (document.readyState === 'complete' || document.readyState === 'interactive') {
        setTimeout(function () { try { initLoadFromQuery(); } catch (e) { console.error(e); } }, 50);
    } else {
        window.addEventListener('load', function () { try { initLoadFromQuery(); } catch (e) { console.error(e); } });
    }
} catch (e) { console.error(e); }


function initLoadFromQuery() {
    console.log("initLoadFromQuery begin");
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

                setBtnStateFromStatus(ent.trang_thai);
                console.log("initLoadFromQuery end" + ent.trang_thai);
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

function setBtnStateFromStatus(status) {
        // enable/disable add/delete/add-customer UI elements based on allow
        try {
            var s = (status || '').toString().trim().toUpperCase();
            var allow = (s === '' || s === 'NEW' || s === 'REJECT');

            try { if (typeof btnPay !== 'undefined' && btnPay.SetEnabled) btnPay.SetEnabled(allow); } catch (e) { }
            try { if (typeof btnSaveTemp !== 'undefined' && btnSaveTemp.SetEnabled) btnSaveTemp.SetEnabled(allow); } catch (e) { }
            
        var addItemEls = document.querySelectorAll('.btn-add-item');
            for (var i = 0; i < addItemEls.length; i++) {
                try {
                    if (allow) {
                        addItemEls[i].style.pointerEvents = '';
                        addItemEls[i].style.opacity = '';
                        addItemEls[i].style.cursor = '';
                    } else {
                        addItemEls[i].style.pointerEvents = 'none';
                        addItemEls[i].style.opacity = '0.4';
                        addItemEls[i].style.cursor = 'default';
                    }
                } catch (ie) { console.error('setBtnPayStateFromStatus addItemEls loop error', ie); }
            }
            
            //var delEls = document.querySelectorAll('.btn-delete-item');
            //for (var j = 0; j < delEls.length; j++) {
            //    try {
            //        if (allow) {
            //            delEls[j].style.pointerEvents = '';
            //            delEls[j].style.opacity = '';
            //            delEls[j].style.cursor = '';
            //        } else {
            //            delEls[j].style.pointerEvents = 'none';
            //            delEls[j].style.opacity = '0.4';
            //            delEls[j].style.cursor = 'default';
            //        }
            //    } catch (de) { console.error('setBtnPayStateFromStatus delEls loop error', de); }
            //}
            
            var custEls = document.querySelectorAll('.btn-add-customer');
            for (var k = 0; k < custEls.length; k++) {
                try {
                    if (allow) {
                        custEls[k].style.pointerEvents = '';
                        custEls[k].style.opacity = '';
                        custEls[k].style.cursor = '';
                    } else {
                        custEls[k].style.pointerEvents = 'none';
                        custEls[k].style.opacity = '0.4';
                        custEls[k].style.cursor = 'default';
                    }
                } catch (ce) { console.error('setBtnPayStateFromStatus custEls loop error', ce); }
            }
            // also disable/enable quantity and price inputs (so_*, dg_*) in right grid
            try {
                var qtys = [];
                var usedGridForQty = false;
                var rightMain = null;
                if (typeof gridHangHoaRight !== 'undefined' && gridHangHoaRight.GetMainElement) {
                    rightMain = gridHangHoaRight.GetMainElement();
                    if (rightMain) {
                        // try several selectors to find the inputs inside DevExpress grid structure
                        qtys = rightMain.querySelectorAll('input.so-input, input[id^="so_"], input[onchange*="so_luong"]');
                        usedGridForQty = true;
                    }
                }
                if (!qtys || qtys.length === 0) {
                    // fallback to global selectors
                    qtys = document.querySelectorAll('input.so-input, input[id^="so_"], input[onchange*="so_luong"]');
                }
                // if grid main was present but no qty inputs found yet, retry shortly (grid may render async)
                if (usedGridForQty && (!qtys || qtys.length === 0)) {
                    window._setBtnRetryCount = (window._setBtnRetryCount || 0) + 1;
                    if (window._setBtnRetryCount < 6) {
                        setTimeout(function () { try { setBtnStateFromStatus(window._currentOrderStatus || status); } catch (e) { } }, 150);
                        return;
                    } else {
                        // exhausted retries
                        window._setBtnRetryCount = 0;
                    }
                }
                // reset retry counter when we have results or not using gridMain
                window._setBtnRetryCount = 0;

                for (var qi = 0; qi < qtys.length; qi++) {
                    try {
                        var el = qtys[qi];
                        el.disabled = !allow;
                        try { el.readOnly = !allow; } catch (r) { }
                        try { el.setAttribute('aria-disabled', (!allow).toString()); } catch (a) { }
                        try { el.tabIndex = allow ? 0 : -1; } catch (t) { }
                        el.style.opacity = allow ? '' : '0.6';
                        el.style.pointerEvents = allow ? '' : 'none';

                        if (!allow) {
                            try { el.setAttribute('disabled','disabled'); } catch (e) { }
                            // attach simple blocking handlers (assign to properties so we can remove later)
                            el._old_onkeydown = el.onkeydown;
                            el._old_onkeypress = el.onkeypress;
                            el._old_onpaste = el.onpaste;
                            el._old_onfocus = el.onfocus;
                            el.onkeydown = function (ev) { ev.preventDefault(); ev.stopPropagation(); return false; };
                            el.onkeypress = function (ev) { ev.preventDefault(); ev.stopPropagation(); return false; };
                            el.onpaste = function (ev) { ev.preventDefault(); ev.stopPropagation(); return false; };
                            el.onfocus = function (ev) { try { ev.target.blur(); } catch (e) { } };
                        }
                        else {
                            try { el.removeAttribute('disabled'); } catch (e) { }
                            try { el.onkeydown = el._old_onkeydown || null; } catch (e) { }
                            try { el.onkeypress = el._old_onkeypress || null; } catch (e) { }
                            try { el.onpaste = el._old_onpaste || null; } catch (e) { }
                            try { el.onfocus = el._old_onfocus || null; } catch (e) { }
                        }
                    } catch (qex) { console.error('setBtnStateFromStatus qty update error', qex); }
                }
            } catch (qee) { }
            // also disable/enable price (đơn giá) inputs (dg_*) in right grid
            try {
                var dgs = [];
                var usedGridForDg = false;
                var rightMainDg = null;
                if (typeof gridHangHoaRight !== 'undefined' && gridHangHoaRight.GetMainElement) {
                    rightMainDg = gridHangHoaRight.GetMainElement();
                    if (rightMainDg) {
                        dgs = rightMainDg.querySelectorAll('input.dg-input, input[id^="dg_"], input[onchange*="don_gia"]');
                        usedGridForDg = true;
                    }
                }
                if (!dgs || dgs.length === 0) {
                    // fallback to global selectors
                    dgs = document.querySelectorAll('input.dg-input, input[id^="dg_"], input[onchange*="don_gia"]');
                }
                // if grid main was present but no dg inputs found yet, retry shortly (grid may render async)
                if (usedGridForDg && (!dgs || dgs.length === 0)) {
                    window._setBtnRetryCountDg = (window._setBtnRetryCountDg || 0) + 1;
                    if (window._setBtnRetryCountDg < 6) {
                        setTimeout(function () { try { setBtnStateFromStatus(window._currentOrderStatus || status); } catch (e) { } }, 150);
                        return;
                    } else {
                        // exhausted retries
                        window._setBtnRetryCountDg = 0;
                    }
                }
                // reset retry counter when we have results or not using gridMain
                window._setBtnRetryCountDg = 0;

                for (var di = 0; di < dgs.length; di++) {
                    try {
                        var elDg = dgs[di];
                        elDg.disabled = !allow;
                        try { elDg.readOnly = !allow; } catch (r) { }
                        try { elDg.setAttribute('aria-disabled', (!allow).toString()); } catch (a) { }
                        try { elDg.tabIndex = allow ? 0 : -1; } catch (t) { }
                        elDg.style.opacity = allow ? '' : '0.6';
                        elDg.style.pointerEvents = allow ? '' : 'none';

                        if (!allow) {
                            try { elDg.setAttribute('disabled', 'disabled'); } catch (e) { }
                            elDg._old_onkeydown = elDg.onkeydown;
                            elDg._old_onkeypress = elDg.onkeypress;
                            elDg._old_onpaste = elDg.onpaste;
                            elDg._old_onfocus = elDg.onfocus;
                            elDg.onkeydown = function (ev) { ev.preventDefault(); ev.stopPropagation(); return false; };
                            elDg.onkeypress = function (ev) { ev.preventDefault(); ev.stopPropagation(); return false; };
                            elDg.onpaste = function (ev) { ev.preventDefault(); ev.stopPropagation(); return false; };
                            elDg.onfocus = function (ev) { try { ev.target.blur(); } catch (e) { } };
                        }
                        else {
                            try { elDg.removeAttribute('disabled'); } catch (e) { }
                            try { elDg.onkeydown = elDg._old_onkeydown || null; } catch (e) { }
                            try { elDg.onkeypress = elDg._old_onkeypress || null; } catch (e) { }
                            try { elDg.onpaste = elDg._old_onpaste || null; } catch (e) { }
                            try { elDg.onfocus = elDg._old_onfocus || null; } catch (e) { }
                        }
                    } catch (dex) { console.error('setBtnPayStateFromStatus dg update error', dex); }
                }
            } catch (dee) { console.error('setBtnPayStateFromStatus dgs selector error', dee); }

            /*
            // show divDuyetTuChoi only when status === 'SENDAPPROVAL'
            try {
                var div = document.getElementById('divDuyetTuChoi');
                try { console.log('[QLDonHangBanCT] divDuyetTuChoi found:', !!div); } catch (e) { }
                if (div) {
                    try { console.log('[QLDonHangBanCT] applying visibility for', s); } catch (e) { }
                    if (s === 'SENDAPPROVAL') div.style.display = '';
                    else div.style.display = 'none';
                }
            } catch (err) { console.error('setBtnPayStateFromStatus divDuyetTuChoi error', err); }

            */
        } catch (e) {
            console.error('setBtnPayStateFromStatus error', e);

        }
    }

