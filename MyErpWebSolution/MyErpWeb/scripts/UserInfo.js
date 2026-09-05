function openAddForm() {
    clearForm("");
    lsbRoles.PerformCallback("");
    lsbUnRoles.PerformCallback("");
    lsbAccess.PerformCallback("");
    lsbUnAccess.PerformCallback("");
    popUpdateForm.Show();
}


function clearForm(readEdit) {
    txtObjectId.Set('hidden_value', "0");

    txtUserName.SetText("");
    txtStaffCode.SetText("");
    txtStaffName.SetValue("");
    txtEmail.SetText("");
    cbBranch.SetValue("");
    cbDepartment.SetValue("");

    lblUserFullName.SetText("");
    lblUserFullNameDataAccess.SetText("");

    // lsbRoles.PerformCallback("xxx");
    // lsbUnRoles.PerformCallback("xxx");
    // lsbAccess.PerformCallback("xxx");
    // lsbUnAccess.PerformCallback("xxx");

    txtUserName.SetEnabled(true);

    btnAddRole.SetEnabled(false);
    btnRemoveRole.SetEnabled(false);
    btnAddAccess.SetEnabled(false);
    btnRemoveAccess.SetEnabled(false);
}

function saveUserInfo() {

    if (txtUserName.GetText() + "" == "") {
        alert("Chưa nhập tên đăng nhập");
        return;
    }
    if (txtStaffName.GetText() + "" == "") {
        alert("Chưa nhập tên nhân viên");
        return;
    }
    if (cbBranch.GetValue() + "" == "null" || cbBranch.GetValue() + "" == "") {
        alert("Chưa nhập chi nhánh (cơ sở)");
        return;
    }
    if (cbDepartment.GetValue() + "" == "null" || cbDepartment.GetValue() + "" == "") {
        alert("Chưa nhập phòng ban");
        return;
    }

    var data = "mode=AddOrUpdate";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&UserName=" + txtUserName.GetText();
    data += "&StaffNo=" + txtStaffCode.GetText();
    data += "&StaffName=" + txtStaffName.GetText();
    data += "&Email=" + txtEmail.GetText();
    data += "&BranchId=" + cbBranch.GetValue();
    data += "&DepartmentId=" + cbDepartment.GetValue();
    //data += "&Description=" + txtDescription.GetText();

    btnSaveEdit.SetEnabled(false);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgUserInfoAction.ashx",
        dataType: "json",
        data: data,
        complete: function (xmlHttpRequest, status) {
            btnSaveEdit.SetEnabled(true);

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {;
                txtUserName.SetEnabled(false);
                txtObjectId.Set('hidden_value', data.id);
                gridUsers.PerformCallback();

                //lsbRoles.PerformCallback(data.UserName);
                //lsbUnRoles.PerformCallback(data.UserName);
                //lsbAccess.PerformCallback(data.UserName);
                //lsbUnAccess.PerformCallback(data.UserName);

                btnAddRole.SetEnabled(true);
                btnRemoveRole.SetEnabled(true);
                btnAddAccess.SetEnabled(true);
                btnRemoveAccess.SetEnabled(true);

            }
            else {

            }
            alert("Kết quả lưu: " + data.message);
        }
    });
}

function openEditForm(ID, UserName, readEdit) {
    clearForm(readEdit);

    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgUserInfoAction.ashx",
        dataType: "json",
        data: "mode=EDIT&id=" + ID,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {

                txtObjectId.Set('hidden_value', data.entity.Id);

                txtUserName.SetEnabled(false);

                txtUserName.SetText(data.entity.UserName);
                txtStaffCode.SetText(data.entity.StaffNo);
                txtStaffName.SetValue(data.entity.StaffName);
                txtEmail.SetText(data.entity.Email);
                cbDepartment.SetValue(data.entity.DepartmentId);
                cbBranch.SetValue(data.entity.BranchId);

                lsbRoles.PerformCallback(UserName);
                lsbUnRoles.PerformCallback(UserName);
                lsbAccess.PerformCallback(UserName);
                lsbUnAccess.PerformCallback(UserName);

                lblUserFullName.SetText(data.entity.StaffName);
                lblUserFullNameDataAccess.SetText(data.entity.StaffName);

                btnAddRole.SetEnabled(true);
                btnRemoveRole.SetEnabled(true);
                btnAddAccess.SetEnabled(true);
                btnRemoveAccess.SetEnabled(true);

                popUpdateForm.Show();
            }
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
        url: "../Actions/CfgUserInfoAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDelete.SetEnabled(true);
            popupConfirmDelete.Hide();
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridUsers.PerformCallback();
            }
            else {

            }
            alert("Kết thúc xóa: " + data.message);
        }
    });
}



//function openUnlockUserDialog(id) {
//    $.ajax({
//        type: "POST",
//        async: true,
//        url: "../Actions/CfgUserInfoAction.ashx",
//        dataType: "json",
//        data: "mode=select&id=0&userId=" + id,
//        complete: function () { },
//        timeout: 30000,
//        success: function (data) {
//            if (data.success) {
//                if (data.entity.IsLocked > 0) {
//                    txtObjectId.Set("hidden_value", id);
//                    $("#spUnlockName").html(data.entity.StaffName);
//                    popConfirmUnlockUser.Show();
//                }
//                else
//                    alert("Người dùng này không bị khóa nên không cần mở khóa.");
//            }
//        }
//    });
//}

//function doUnlockUser() {
//    var id = txtObjectId.Get("hidden_value");
//    var data = "&mode=update";
//    data += "&action=unlock";
//    data += "&tblName=S";
//    data += "&id=0";
//    data += "&userId=" + id;
//    $.ajax({
//        type: "POST",
//        async: true,
//        url: "../Actions/UserInfoAction.ashx",
//        dataType: "json",
//        data: data,
//        complete: function () {

//        },
//        timeout: 10000,
//        success: function (data) {
//            if (data.success) {
//                btnSearch.DoClick();
//                popConfirmUnlockUser.Hide();
//            }
//        }
//    });
//}

function removeAccessGroups() {
    btnRemoveAccess.SetEnabled(false);
    moveSelectedAccessGroups(lsbAccess, lsbUnAccess, false);
    btnRemoveAccess.SetEnabled(true);
}

function addAccessGroups() {
    btnAddAccess.SetEnabled(false);
    moveSelectedAccessGroups(lsbUnAccess, lsbAccess, true);
    btnAddAccess.SetEnabled(true);
}

function addOrRemoveAccessGroup(id, isAdding) {
    var flag4Update = false;
    var action = isAdding ? "AddAccessGroup" : "RemoveAccessGroup";
    var data = "action=" + action;
    data += "&mode=AddRemoveAccessGroup";
    data += "&groupId=" + id;
    data += "&userName=" + txtUserName.GetText();
    data += "&id=0";

    $.ajax({
        type: "POST",
        async: false, // xử lý đồng bộ
        url: "../Actions/CfgUserInfoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

        },
        timeout: 10000,
        success: function (data) {
            flag4Update = data.success;
        }
    });

    return flag4Update;
}

function moveSelectedAccessGroups(srcListBox, dstListBox, isAdding) {

    var items = srcListBox.GetSelectedItems();
    if (items != null && items.length > 0) {
        srcListBox.BeginUpdate();
        dstListBox.BeginUpdate();
        for (var i = items.length - 1; i >= 0; i = i - 1) {
            if (addOrRemoveAccessGroup(items[i].value, isAdding)) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
        }
        srcListBox.EndUpdate();
        dstListBox.EndUpdate();
    }


}

function removeRoles() {
    btnRemoveRole.SetEnabled(false);
    moveSelectedItems(lsbRoles, lsbUnRoles, false);
    btnRemoveRole.SetEnabled(true);
}

function addRoles() {
    btnAddRole.SetEnabled(false);
    moveSelectedItems(lsbUnRoles, lsbRoles, true);
    btnAddRole.SetEnabled(true);
}

function moveSelectedItems(srcListBox, dstListBox, isAdding) {

    var items = srcListBox.GetSelectedItems();
    if (items != null && items.length >0) {
        srcListBox.BeginUpdate();
        dstListBox.BeginUpdate();
        for (var i = items.length - 1; i >= 0; i = i - 1) {
            if (addOrRemoveRole(items[i].value, isAdding)) {
                dstListBox.AddItem(items[i].text, items[i].value);
                srcListBox.RemoveItem(items[i].index);
            }
        }
        srcListBox.EndUpdate();
        dstListBox.EndUpdate();
    }

}

function addOrRemoveRole(id, isAdding) {
    var flag4Update = false;
    var action = isAdding ? "AddRole" : "RemoveRole";
    var data = "action=" + action;
    data += "&mode=AddRemoveRole";
    data += "&roleId=" + id;
    data += "&userName=" + txtUserName.GetText();
    data += "&id=0";
    $.ajax({
        type: "POST",
        async: false, // xử lý đồng bộ
        url: "../Actions/CfgUserInfoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {

        },
        timeout: 10000,
        success: function (data) {
            flag4Update = data.success;
        }
    });

    return flag4Update;
}

/*
function saveChangeUser() {
    var data = "mode=update&action=USER&tblName=U";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&state=" + hddEntityState.Get("hidden_value");
    data += "&email=" + txtEmail.GetText();
    data += "&userId=" + txtUserName.GetText();
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgUserInfoAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {

            }
            alert(data.message)
        }
    });

}
*/


