
var lastFunctionId = null;

$(document).ready(function () { })

function openDeleteFormRoleDetail(row) {
    txtObjectDetailId.Set("hidden_value", row[0]);
    $("#spRoleDetail").html(row[1]);
    popConfirmDeleteRoleDetail.Show();
}

function doDeleteRoleDetail() {
    btnConfirmDeleteRoleDetail.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgFunctionRightAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectDetailId.Get("hidden_value") + "&tblName=D",
        complete: function () {
            btnConfirmDeleteRoleDetail.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridDetails.PerformCallback(txtObjectId.Get("hidden_value"));
                popConfirmDeleteRoleDetail.Hide();
            }
            alert(data.message);
        }
    });
}

function saveChangeRoleDetail() {
    btnSaveRoleDetail.SetEnabled(false);
    var data = "mode=update&tblName=D";
    data += "&id=" + txtObjectDetailId.Get("hidden_value");
    data += "&roleId=" + txtObjectId.Get("hidden_value");
    data += "&functionId=" + cboFunctionId.GetValue();
    data += "&canRead=" + chkRead.GetValue();
    data += "&canCreate=" + chkCreate.GetValue();
    data += "&canEdit=" + chkEdit.GetValue();
    data += "&canDelete=" + chkDelete.GetValue();
    data += "&canPrint=" + chkPrint.GetValue();
    data += "&canApprove=" + chkApprove.GetValue();
    data += "&canSpecial=" + chkSpecial.GetValue();
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgFunctionRightAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnSaveRoleDetail.SetEnabled(true);
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


function onEndCallbackFunctions(s, e) {
    if (lastFunctionId != null) cboFunctionId.SetValue(lastFunctionId);
    else
        cboFunctionId.SetValue(null);
}

function openEditFormRoleDetail(row) {
    clearFormRoleDetail();
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgFunctionRightAction.ashx",
        dataType: "json",
        data: "mode=select&id=" + row[0] +  "&tblName=D",
        complete: function () {

        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                lastFunctionId = data.entity.FunctionId;
                cboFunctionId.PerformCallback("edit|" + txtObjectId.Get("hidden_value") + "|" + data.entity.FunctionId);
                txtObjectDetailId.Set("hidden_value", data.entity.Id);
                
                chkRead.SetChecked(data.entity.CanRead > 0);
                chkCreate.SetChecked(data.entity.CanCreate > 0);
                chkEdit.SetChecked(data.entity.CanEdit > 0);
                chkDelete.SetChecked(data.entity.CanDelete > 0);
                chkPrint.SetChecked(data.entity.CanPrint > 0);
                chkApprove.SetChecked(data.entity.CanApprove > 0);
                chkSpecial.SetChecked(data.entity.CanSpecial > 0);

                popUpdateRoleDetail.Show();
            }
            else
                alert(data.message);
        }
    });
}

function openAddingFormRoleDetail() {
    if (txtObjectId.Get("hidden_value") == "0") {
        alert("Bạn vui lòng tạo nhóm quyền trước khi tạo chi tiết");
        return;
    }

    lastFunctionId = null;;
    clearFormRoleDetail();
    cboFunctionId.PerformCallback("add|" + txtObjectId.Get("hidden_value"));
    popUpdateRoleDetail.Show();
}

function clearFormRoleDetail() {
    txtObjectDetailId.Set("hidden_value", "0");
    cboFunctionId.SetValue(null);
    chkRead.SetChecked(false);
    chkCreate.SetChecked(false);
    chkEdit.SetChecked(false);
    chkDelete.SetChecked(false);
    chkPrint.SetChecked(false);
    chkApprove.SetChecked(false);
    chkSpecial.SetChecked(false);
}

function doDeleteAccessGroup() {
    btnConfirmDeleteRole.SetEnabled(false);
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgFunctionRightAction.ashx",
        dataType: "json",
        data: "mode=delete&tblName=M&id=" + txtObjectId.Get('hidden_value'),
        complete: function () {
            btnConfirmDeleteRole.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridRoles.PerformCallback();
                popConfirmDelete.Hide();
            }
            alert(data.message);
        }
    });
}

function openDeleteForm(row) {
    txtObjectId.Set('hidden_value', row[0]);
    $("#spRoleName").html(row[1]);
    popConfirmDelete.Show();
}

function openEditForm(row) {
    clearForm();
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgFunctionRightAction.ashx",
        dataType: "json",
        data: "mode=select&tblName=M&id=" + row[0],
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectId.Set('hidden_value', data.entity.Id);
                txtCode.SetText(data.entity.Code);
                txtRoleName.SetText(data.entity.RoleName);
                txtDescription.SetText(data.entity.Description);

                txtCode.SetEnabled(false);
                gridDetails.PerformCallback(data.entity.Id);
                
                popUpdateForm.Show();
            }
        }
    });
}

function saveChangeForm() {
 
    btnSaveEditRole.SetEnabled(false);
    var data = "mode=update";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&tblName=M";
    data += "&code=" + txtCode.GetText();
    data += "&roleName=" + txtRoleName.GetText();
    data += "&description=" + txtDescription.GetText();
    var isAddNewRecord = (txtObjectId.Get("hidden_value") == "0");
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgFunctionRightAction.ashx",
        dataType: "json",
        data: data,
        complete: function () {
            btnSaveEditRole.SetEnabled(true);
        },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridRoles.PerformCallback();
                txtObjectId.Set("hidden_value", data.id);
                if (!isAddNewRecord) popUpdateForm.Hide();
                else {
                    
                }
            }
            alert(data.message)
        }
    });
}

function openAddingForm() {
    clearForm();
    gridDetails.PerformCallback(0);
    popUpdateForm.Show();
}

function clearForm() {
    txtObjectId.Set("hidden_value", 0)
    txtCode.SetText("");
    txtRoleName.SetText("");
    txtDescription.SetText("");
    txtCode.SetEnabled(true);
}




