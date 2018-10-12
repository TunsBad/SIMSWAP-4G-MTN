"use strict";
//const Api = 'http://localhost:3704/' + 'api/simswap';
var popupNotification;

$(function () {

    prepareUi(id)
    popupNotification = $("#popupNotification").kendoNotification({
        stacking: "down",
        show: onShow,
        button: true
    }).data('kendoNotification');
});

var request = {};
var toBeSent = {};
var Id;

function prepareUi(id) {
    $.ajax({
        type: "GET",
        url: Api + "/GetRequestById?id=" + id,
        success: function (result) {
            request = result;
            console.log("request to be edited", request);

            Id = request[0].Id;

            $("#phonenumber").val(request[0].Msisdn);
            $("#serialnumber").val(request[0].SimSerial);
            $("#idnumber").val(request[0].IdNumber); 
            $("#idtype").val(request[0].IdType);
            $("#reason").val(request[0].Reason);
            $("#comment").val(request[0].Comment);
        }
    });
}


$("#submitChanges").click(function () {

    var pNumber = $("#phonenumber").val();
    var sNumber = $("#serialnumber").val();
    var idNumber = $("#idnumber").val();
    var idType = $("#idtype").val();
    var reason = $("#reason").val();
    var comment = $("#comment").val();

    toBeSent = {
        "Id": Id,
        "Msisdn": pNumber,
        "SimSerial": sNumber,
        "IdNumber": idNumber,
        "IdType":  idType,
        "Reason": reason,
        "Comment": comment
    };

    $.ajax({
        type: "POST",
        url: Api + "/EditRequest?editedrequest",
        data: toBeSent,
        success: function (result) {
            console.log("request to be edited", result);
            popupNotification.show('Request Successfully Updated\n', "success");
            //window.location = "/requests/allrequests";
        },
        error: function (result) {
            popupNotification.show('Updating Request Failed\n', "error");
        }
    });

    console.log("changes to be sent", toBeSent);
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
