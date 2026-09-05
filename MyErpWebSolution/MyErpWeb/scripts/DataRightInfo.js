

$(document).ready(function () { })

function filterUnPriStaffList() {
    var filterVal = txtFilterName.GetText();
    if (filterVal.length >= 3) {
        lsbUnAccess.PerformCallback(txtObjectId.Get('hidden_value') + '|' + filterVal);
    }
}

function doDeleteAccessGroup() {
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgDataRightAction.ashx",
        dataType: "json",
        data: "mode=delete&id=" + txtObjectId.Get('hidden_value'),
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridAccessGroups.PerformCallback();
                popConfirmDelete.Hide();
            }
            alert(data.message);
        }
    });
}

function openDeleteForm(row) {
    txtObjectId.Set('hidden_value', row[0]);
    $("#spGroupName").html(row[2]);
    popConfirmDelete.Show();
}

function openEditForm(row) {
    clearForm();
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgDataRightAction.ashx",
        dataType: "json",
        data: "mode=select&id=" + row[0],
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                txtObjectId.Set('hidden_value', data.entity.Id);
                txtGroupCode.SetText(data.entity.Code);
                txtGroupName.SetText(data.entity.GroupName);
                txtDescription.SetText(data.entity.Description);
                lblUserFullNameDataAccess.SetText(data.entity.GroupName);
                btnRemoveAccess.SetEnabled(true);
                btnAddAccess.SetEnabled(true);
                lsbAccess.PerformCallback(data.entity.Id);
                lsbUnAccess.PerformCallback(data.entity.Id + "|");
                popUpdateForm.Show();
            }
        }
    });
}

function saveChangeForm() {
    var data = "mode=update";
    data += "&id=" + txtObjectId.Get("hidden_value");
    data += "&tblName=M";
    data += "&code=" + txtGroupCode.GetText();
    data += "&groupName=" + txtGroupName.GetText();
    data += "&description=" + txtDescription.GetText();
    var isAddNewRecord = (txtObjectId.Get("hidden_value") == "0");
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/CfgDataRightAction.ashx",
        dataType: "json",
        data: data,
        complete: function () { },
        timeout: 30000,
        success: function (data) {
            if (data.success) {
                gridAccessGroups.PerformCallback();
                txtObjectId.Set("hidden_value", data.id);
                if (!isAddNewRecord) popUpdateForm.Hide();
                else {
                    btnRemoveAccess.SetEnabled(true);
                    btnAddAccess.SetEnabled(true);
                    lblUserFullNameDataAccess.SetText(txtGroupName.GetText());
                }
            }
            alert(data.message)
        }
    });
}


function openAddingForm() {
    lsbAccess.PerformCallback(0);
    lsbUnAccess.PerformCallback("0|");
    clearForm();
    popUpdateForm.Show();
}

function clearForm() {
    lblUserFullNameDataAccess.SetText("");
    txtFilterName.SetText("");
    btnRemoveAccess.SetEnabled(false);
    btnAddAccess.SetEnabled(false);
    txtObjectId.Set("hidden_value", 0)
    txtGroupCode.SetText("");
    txtGroupName.SetText("");
    txtDescription.SetText("");
}


function addOrRemoveAccessGroup(id, isAdding) {
    var flag4Update = false;
    var action = isAdding ? "Add" : "Remove";
    var data = "action=" + action;
    data += "&mode=update";
    data += "&tblName=D";
    data += "&domainUser=" + id;
    data += "&id=" + txtObjectId.Get("hidden_value");
    $.ajax({
        type: "POST",
        async: false, // xử lý đồng bộ
        url: "../Actions/CfgDataRightAction.ashx",
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

function removeAccessGroups() {
    btnRemoveAccess.SetEnabled(false);
    moveSelectedItems(lsbAccess, lsbUnAccess, false);
    btnRemoveAccess.SetEnabled(true);
}

function addAccessGroups() {
    btnAddAccess.SetEnabled(false);
    moveSelectedItems(lsbUnAccess, lsbAccess, true);
    btnAddAccess.SetEnabled(true);
}

function moveSelectedItems(srcListBox, dstListBox, isAdding) {
    srcListBox.BeginUpdate();
    dstListBox.BeginUpdate();
    var items = srcListBox.GetSelectedItems();
    for (var i = items.length - 1; i >= 0; i = i - 1) {
        if (addOrRemoveAccessGroup(items[i].value, isAdding)) {
            dstListBox.AddItem(items[i].text, items[i].value);
            srcListBox.RemoveItem(items[i].index);
        }
    }
    srcListBox.EndUpdate();
    dstListBox.EndUpdate();
}
