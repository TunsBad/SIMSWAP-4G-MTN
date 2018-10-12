"use strict";
//const Api = 'http://localhost:3704/' + 'api/simswap';

$(function () {
    console.log("id", Id);
    console.log("msisdn", Msisdn);
    getRequestById(Id)
});

var request = {};

function getRequestById(Id) {
    $.ajax({
        type: "GET",
        url: Api + "/GetRequestDetailsById?requestid=" + Id,
        success: function (result) {

            request = result;
            console.log("onerequest", request);
            console.log("longitude", request[0].Longitude);
            console.log("latitude", request[0].Latitude);
            
            $("#phonenumber").text(request[0].Msisdn);
            $("#serialnumber").text(request[0].SimSerial);
            $("#idtype").text(request[0].IdType);
            $("#idnumber").text(request[0].IdNumber);
            $("#datesubmitted").text(kendo.toString(new Date(request[0].DateSubmitted.substring(0, 10)), 'dd-MMM-yyyy'));
            $("#reason").text(request[0].Reason);
            $("#comment").text(request[0].Comment);
            $("#status").text(request[0].Status);
            $("#iphoto").attr("src", "/api/simswap/UserIdcardPhoto?requestid=" + request[0].Id);
            $("#dp").attr("src", "/api/simswap/UserProfilePhoto?requestid=" + request[0].Id);
            $("#uphoto").attr("src", "/api/simswap/UserProfilePhoto?requestid=" + request[0].Id);

            moveToLocation(request[0].Latitude, request[0].Longitude);
        }
    });
}
