"use strict";
//const Api = 'http://localhost:3704/' + 'api/simswap';
var popupNotification;

$(function () {
    renderKendoTextBox();
    popupNotification = $("#popupNotification").kendoNotification({
        stacking: "down",
        show: onShow,
        button: true
    }).data('kendoNotification');
});

function renderKendoTextBox() {
    $("#thresholdBox").kendoMaskedTextBox();
}

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

$("#updatethreshold").click(
    function () {
        updateThreshold();
    }
);

function updateThreshold() {
    var threshold = $("#thresholdBox").val();
    $.ajax({
        url: Api + '/updatethreshold?threshold=' + threshold,
        type: 'POST',
        contentType: "application/json"
    }).done(function (data) {
        if (data.ThresholdValue == 1) {
            popupNotification.show('Threshold Successfully Updated\n', "success");
            window.location = "/Threshold/Index";
        } else if (data.ThresholdValue != 0) {
            popupNotification.show('Updating Threshold Failed\n', "error");
        }
    }).error(function (xhr, data, error) {
            popupNotification.show(error, "error");
    });
}
