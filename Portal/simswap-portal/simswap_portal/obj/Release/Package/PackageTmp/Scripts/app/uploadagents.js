"use strict";
var popupNotification;

$(function () {
    renderKendoTextBox();
    renderInvisibleGridForAgents();
    popupNotification = $("#popupNotification").kendoNotification({
        stacking: "down",
        show: onShow,
        button: true
    }).data('kendoNotification');
});

function onShow(e) {
    if (e.sender.getNotifications().length == 1) {
        var element = e.element.parent(),
            eWidth = element.width(),
            eHeight = element.height(),
            wWidth = $(window).width(),
            wHeight = $(window).height(),
            newTop, newLeft;

        newLeft = Math.floor(wWidth / 2 - eWidth / 2);
        newTop = Math.floor(wHeight / 2 - eHeight / 2);

        e.element.parent().css({ top: newTop, left: newLeft });
    }
}

function onUpload(e) {
    var upload = this;
    var dropdown = upload.wrapper.find(".k-file[data-uid='" + e.files[0].uid + "'] select")
        .data("kendoDropDownList");
}

function onSuccess(e) {
    popupNotification.show("File Successfully Uploaded\n", "success");
}

function onError(e) {
    popupNotification.show("File Upload Failed, Check for empty Fields in Excel Sheet\n", "error");
}

function renderInvisibleGridForAgents() {
    $("#uploadBatchAgents").kendoUpload({
        async: {
            saveUrl: '/Users/UploadFile',
            removeUrl: "remove",
            autoUpload: false
        },
        upload: onUpload,
        success: onSuccess,
        error: onError
    });
}

function renderKendoTextBox() {
    $("#msisdnBox").kendoMaskedTextBox();
}

function checkAgent() {
    var cellid = $("#msisdnBox").val();
    $.ajax({
        url: Api + '/?=' + cellid,
        type: 'GET',
        contentType: "application/json"
    }).done(function (data) {
        if (data == 1) {
            popupNotification.show('Cell Id Already Exists\n', "success");
        } else if (data == -1) {
            popupNotification.show('Cell Id Does Not Exist\n', "error");
        }
    }).error(function (xhr, data, error) {
        popupNotification.show(error, "error");
    });
}

$("#checkAgent").click(
    function () {
        checkAgent();
    }
);