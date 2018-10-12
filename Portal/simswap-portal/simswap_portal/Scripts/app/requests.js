"use strict";
//const Api = 'http://localhost:3704/' + 'api/simswap';

$(function () {
    //displayLoadingDialog();
    loadData();

    $("#sDate").kendoDatePicker({
        value: new Date()
    });
    $("#eDate").kendoDatePicker({
        value: new Date()
    });

    $("#btnFilter").click(function () {
        var startDate = $("#sDate").data("kendoDatePicker").value();
        var endDate = $("#eDate").data("kendoDatePicker").value();

        var filteredRequestDataSource = {
            transport: {
                read: {
                    url: Api + "/ByDate",
                    type: 'Post',
                    contentType: "application/json",
                },
                parameterMap: function (data, type) {
                    data.startDate = startDate;
                    data.endDate = endDate;
                    return kendo.stringify(data);
                },
            },
            pageSize: 10,
            schema: {
                data: "Data",
                total: "Count",
                model: {
                    id: "id",
                    fields: {
                        id: { editable: false, type: "number" },
                        firstName: { editable: false },
                        lastName: { editable: false },
                        idNumber: { editable: false },
                        idType: { editable: false },
                        dob: { editable: false, type: 'date' },
                        registrationSyncDate: { editable: false, type: 'date' },
                        gender: { editable: false },
                        msisdn: { editable: false },
                        alternateNumber: { editable: false },
                        nationality: { editable: false },
                        registeredBy: { editable: false }
                    }
                }
            },
            serverPaging: true,
            serverFiltering: true,
            serverSorting: false,
        };

        $("#subscriberGrid").html('<div id="subscriberGrid"></div><br />');

        renderGrid(filteredSubscribersDataSource);
    });
});

function loadData() {
    var ui = new oUI;
    ui.prepareUI();
    //dismissLoadingDialog();
}

var oUI = (function () {
    function oUI() {}
    oUI.prototype.prepareUI = function () {
        $('#tabs').data("kendoTabStrip");
        //$("#tabStrip")
        this.renderAsetGrid();
    }
    oUI.prototype.renderAsetGrid = function () {
        // use jQuery a selector to get the div with an id of statusGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        renderGrid(AllRequestsDataSource);
    }

    return oUI;
})();

var AllRequestsDataSource = {
    transport: {
        read: {
            url: Api + "/GetAllSimSwapRequests",
            type: 'Post',
            contentType: "application/json",
        },
        parameterMap: function (data, type) {
            return kendo.stringify(data);
        },
    },
    pageSize: 10,
    schema: {
        data: "Data",
        total: "Count",
        model: {
            id: "Id",
            fields: {
                Id: { editable: false, type: "number" },
                Msisdn: { editable: false },
                NewSimSerial: { editable: false },
                IdType: { editable: false },
                IdNumber: { editable: false },
                Reason: { editable: false },
                Comments: { editable: false },
                LocationId: { editable: false },
                UserId: { editable: false },
                AttachmentId: { editable: false },
                DateSubmitted: { editable: false, type: 'date' },
                Status: { editable: false },
                Fullname: { editable: false }
            }
        }
    },
    sort: { field: "DateSubmitted", dir: "desc" },
    serverPaging: true,
    serverFiltering: true,
    serverSorting: false,
};

function renderGrid(dataSource) {
    $("#simswaprequestsGrid").kendoGrid({
        pdfExport: onPdfExport,
        dataSource: dataSource,
        toolbar: ["excel", "pdf"],
        excel: {
            fileName: "SimSwap Requests.xlsx",
            proxyUrl: "/Home/Excel_Export_Save",
            allPages: true
        },
        pdf: {
            allPages: true,
            avoidLinks: true,
            proxyURL: "/Home/Excel_Export_Save",
            fileName: "SimSwap Requests.pdf",
            margin: {
            },
        },
        columns: [
            { field: 'Msisdn', title: 'MSISDN' },
            { field: 'Fullname', title: 'FULL NAME' },
            { field: 'DateSubmitted', format: "{0:dd-MMM-yyyy}", title: 'DATE SUBMITTED', width: "200px" },
            { field: 'NewSimSerial', title: 'SERIAL NUMBER', width: "170px" },
            { field: 'IdType', title: 'ID TYPE' },
            { field: 'IdNumber', title: 'ID NUMBER' },
            { field: 'Status', title: 'STATUS' },
            { field: 'Reason', title: 'REASON', width: "220px" },
            { command: [gridViewButton, gridEditButton], width: "100px" }
        ],
        filterable: true,
        sortable: {
            mode: "multiple",
        },
        pageable: {
            pageSize: 10,
            pageSizes: [10, 25, 50, 100, 1000],
            previousNext: true,
            buttonCount: 5,
        },
        selectable: true,
    });
}

var gridEditButton = {
    name: "EditSubscriber",
    text: "",
    iconClass: "k-icon k-i-pencil",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/Requests/EditRequest?msisdn=" + data.Msisdn + "&id=" + data.Id + "&datesubmitted=" + data.DateSubmitted;
    },
};

var gridViewButton = {
    name: "ViewSubscriber",
    text: "",
    iconClass: "k-icon k-i-search",
    click: function (e) {
        var tr = $(e.target).closest("tr"); // get the current table row (tr)
        var data = this.dataItem(tr);
        window.location = "/Requests/RequestDetails?msisdn=" + data.Msisdn + "&id=" + data.Id;
    },
};

function onPdfExport(e) {
    debugger;
    current = e.sender;
}




