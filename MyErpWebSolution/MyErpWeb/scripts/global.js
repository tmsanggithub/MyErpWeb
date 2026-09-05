

$(document).ready(function () {
    ping();
})

function ping() {
    $.ajax({
        type: "POST",
        async: true,
        url: "../Actions/Timer.ashx",
        dataType: "json",
        data: null,
        complete: function () {
            setTimeout(function () { ping();}, 30000);
        },
        timeout: 30000,
        success: function (data) {}
    });
}