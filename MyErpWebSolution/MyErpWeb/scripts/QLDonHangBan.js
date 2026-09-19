
// Open detail form in a new tab for add; if a tab already opened reuse and focus it
window.openAddForm = function () {
    var url = 'QLDonHangBanCT.aspx';
    var winName = 'QLDonHangBanCT_new';
    try {
        var w = window.open(url, winName);
        if (w) {
            try { w.focus(); } catch (e) { }
        } else {
            // fallback
            window.open(url, '_blank');
        }
    } catch (e) {
        window.open(url, '_blank');
    }
};

// Adjust grid page size to fit viewport more accurately
(function () {
    var resizeTimer = null;
    function adjustGridPageSize() {
        try {
            if (typeof gridQLDonHangBan === 'undefined' || !gridQLDonHangBan.GetMainElement) return;
            var mainEl = gridQLDonHangBan.GetMainElement();
            if (!mainEl) return;

            // compute available vertical space from grid top to viewport bottom
            var rect = mainEl.getBoundingClientRect();
            var viewportH = window.innerHeight || document.documentElement.clientHeight;
            var bottomMargin = 10; // allow space for footer/controls
            var available = viewportH - rect.top - bottomMargin;

            // fallback to element height when rect.top is weird
            if (!available || available < 120) available = mainEl.clientHeight || mainEl.offsetHeight || 400;

            // measure header/filter/pager and a data row inside the grid
            var headerEl = mainEl.querySelector('.dxgvHeader, .dxgvTableHeader');
            var filterEl = mainEl.querySelector('.dxgvFilterRow');
            var pagerEl = mainEl.querySelector('.dxgvPager');
            var rowEl = mainEl.querySelector('.dxgvDataRow, tr.dxgvDataRow');

            var headerH = headerEl ? headerEl.offsetHeight : 40;
            var filterH = filterEl ? filterEl.offsetHeight : 0;
            var pagerH = pagerEl ? pagerEl.offsetHeight : 40;
            var rowH = rowEl ? rowEl.offsetHeight : 36;

            var usable = available - headerH - filterH - pagerH - 12;
            if (usable <= 0) return;

            var rows = Math.floor(usable / rowH);
            if (rows < 5) rows = 5;
            if (rows > 200) rows = 200;

            // avoid repeated callbacks with same value
            if (window._gridQLDonHangBanLastPageSize === rows) return;
            window._gridQLDonHangBanLastPageSize = rows;

            try { gridQLDonHangBan.PerformCallback('PAGESIZE|' + rows); } catch (e) { console.error('PerformCallback error', e); }
        } catch (e) { console.error('adjustGridPageSize error', e); }
    }

    function scheduleAdjust() {
        if (resizeTimer) clearTimeout(resizeTimer);
        resizeTimer = setTimeout(adjustGridPageSize, 150);
    }

    // run on initial load and on resize
    if (document.readyState === 'complete' || document.readyState === 'interactive') {
        setTimeout(adjustGridPageSize, 300);
    } else {
        window.addEventListener('load', function () { setTimeout(adjustGridPageSize, 300); });
    }
    window.addEventListener('resize', scheduleAdjust);

    // also try to adjust after DevExpress callbacks finish (if available)
    try {
        if (typeof gridQLDonHangBan !== 'undefined' && gridQLDonHangBan) {
            try { gridQLDonHangBan.ClientSideEvents = gridQLDonHangBan.ClientSideEvents || {}; } catch (e) { }
            try { gridQLDonHangBan.EndCallback = function () { setTimeout(adjustGridPageSize, 50); }; } catch (e) { }
        }
    } catch (e) { }
})();



function clearForm(ReadAddEditApproval, objStatus) {

    //    txtObjectIDKhoTraCuu.Set('hidden_value', "0");
    cbGiaiChay.SetValue('');
    txtObjectId.Set('hidden_value', "0");
    txt_code.SetText('')
    txt_subject.SetText('')
    txtdescription.SetText('')

    txt_content.SetHtml('');//"<p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Kính gửi anh/chị,</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'><br/></p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Vui lòng hỗ trợ:&nbsp;</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>+</p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Điện thoại liên hệ: </p><p style='font-size: 14.6667px; font-family: Arial, sans-serif; margin: 0px 0px 10.6667px;'>Trân trọng,</p>");


    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
    btnDuyet.SetEnabled(false);
    btnKhongDuyet.SetEnabled(false);
  

    if (ReadAddEditApproval == "ReadOnly") {

    }
    else if (ReadAddEditApproval == "NewOnly") {
        // phụ thuộc vào Quyền: chỉ hiện nút save và sendApproval
        btnSaveEdit.SetEnabled(true);
        btnGuiDuyet.SetEnabled(true);
     
    }
    else if (ReadAddEditApproval == "EditOnly") {
        // phụ thuộc vào Trạng thái và Quyền
        if (objStatus == "NEW" || objStatus == "EDIT" || objStatus == "RETURNED") {
            btnSaveEdit.SetEnabled(true);
            btnGuiDuyet.SetEnabled(true);

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

window.openEditForm = function (ID, xxxxx, readEditApproval) {
    // Open edit form in a named tab so the user doesn't open duplicates; focus if exists
    var url = 'QLDonHangBanCT.aspx?id=' + encodeURIComponent(ID) + '&subMode=' + encodeURIComponent(readEditApproval);
    var winName = 'QLDonHangBanCT_' + (ID || 'new');
    try {
        var w = window.open(url, winName);
        if (w) {
            try { w.focus(); } catch (e) { }
        } else {
            window.open(url, '_blank');
        }
    } catch (e) {
        window.open(url, '_blank');
    }
};

function saveQLDonHangBan() {

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

    data += "&Code=" + txt_code.GetText();
    data += "&Subject=" + txt_subject.GetText();
    data += "&NewsDate=" + de_news_date.GetDate().toJSON();
    data += "&IdRace=" + cbGiaiChay.GetValue();

    data += "&Description=" + encodeURIComponent(txtdescription.GetText());
    data += "&Contents=" + encodeURIComponent(txt_content.GetHtml());

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;

                txtObjectId.Set('hidden_value', data.id);
                gridQLDonHangBan.PerformCallback();
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
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
            }
            else {

            }
            alert(data.message);
        }
    });
}

function SendApprovalQLDonHangBan() {
    if (!confirm('Bạn có muốn gửi duyệt không?')) {
        return;
    }
    btnSaveEdit.SetEnabled(false);
    btnGuiDuyet.SetEnabled(false);
  

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: "mode=SendApproval&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {



        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
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

function ApprovalDonHangBan() {
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
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
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

function RejectDonHangBan() {
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
        url: "../Actions/QLDonHangBanAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

            txtGhiChuDuyetKhongDuyet.SetText("");
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridQLDonHangBan.PerformCallback();
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

