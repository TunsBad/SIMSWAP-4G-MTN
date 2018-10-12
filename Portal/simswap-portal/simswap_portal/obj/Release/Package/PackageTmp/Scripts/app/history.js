"use strict";

$(function () {
    loadData();
});

function loadData() {
    var ui = new oUI;
    ui.prepareUI();
}

var oUI = (function () {
    function oUI() { }
    oUI.prototype.prepareUI = function () {
        $('#tabs').data("kendoTabStrip");
        //$("#tabStrip")
        this.renderAsetGrid();
    }
    oUI.prototype.renderAsetGrid = function () {
        // use jQuery a selector to get the div with an id of statusGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        renderGrid(HistoryDataSource);
    }

    return oUI;
})();

var HistoryDataSource = {
    transport: {
        read: {
            url: Api + "/RequesterHistory?msisdn=" + Msisdn,
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
                SimSerial: { editable: false },
                IdType: { editable: false },
                IdNumber: { editable: false },
                Reason: { editable: false },
                Comment: { editable: false },
                DateSubmitted: { editable: false, type: 'date' },
                Status: { editable: false }
            }
        }
    },
    sort: { field: "DateSubmitted", dir: "desc" },
    serverPaging: true,
    serverFiltering: true,
    serverSorting: false,
};

function renderGrid(dataSource) {
    $("#historyGrid").kendoGrid({
        pdfExport: onPdfExport,
        dataSource: dataSource,
        toolbar: ["excel", "pdf"],
        excel: {
            fileName: "Requester History.xlsx",
            proxyUrl: "/Home/Excel_Export_Save",
            allPages: true
        },
        pdf: {
            allPages: true,
            avoidLinks: true,
            proxyURL: "/Home/Excel_Export_Save",
            fileName: "Requester History.pdf",
            margin: {
            },
        },
        columns: [
            { field: 'Msisdn', title: 'MSISDN' },
            { field: 'DateSubmitted', format: "{0:dd-MMM-yyyy}", title: 'DATE SUBMITTED' },
            { field: 'SimSerial', title: 'SERIAL NUMBER' },
            { field: 'IdType', title: 'ID TYPE' },
            { field: 'IdNumber', title: 'ID NUMBER' },
            { field: 'Status', title: 'STATUS' },
            { field: 'Reason', title: 'REASON', width: "220px" }
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

function onPdfExport(e) {
    debugger;
    current = e.sender;
}




