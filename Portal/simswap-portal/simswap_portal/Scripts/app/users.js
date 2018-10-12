"use strict";
//const Api = 'http://localhost:3704/' + 'api/simswap';

$(function () {
    loadData();
});

function loadData() {
    var ui = new oUI;
    ui.prepareUI();
}

var oUI = (function () {
    function oUI() {
    }
    oUI.prototype.prepareUI = function () {
       //$('#tabs').kendoTabStrip();

        this.renderAsetGrid();
    }
    oUI.prototype.renderAsetGrid = function () {
        // use jQuery a selector to get the div with an id of statusGrid and call the kendoGrid function, passing in a configuration object to initialise the grid
        $("#usersGrid").kendoGrid({
            dataSource: {
                transport: {
                    read: {
                        url: Api + "/GetAllUsers",
                        type: 'POST',
                        contentType: "application/json",

                    },
                    parameterMap: function (data) {
                        return JSON.stringify(data);
                    }
                },
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "UserId",
                        fields: {
                            UserId: { editable: false, type: "number" },
                            Username: { editable: false },
                            Msisdn: { editable: false }
                        }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: false
            },
            columns: [
                { field: 'Username', title: 'User Name' },
                { field: 'Msisdn', title: 'Msisdn' },
                { command: [gridViewButton] }
            ],
            pdfExport: onPdfExport,
            toolbar: ["excel", "pdf"],
            excel: {
                fileName: "Sim Swap Users.xlsx",
                proxyUrl: "/Home/Excel_Export_Save",
                allPages: true
            },
            pdf: {
                allPages: true,
                avoidLinks: true,
                proxyURL: "/Home/Excel_Export_Save",
                fileName: "SimSwap Users.pdf",
                margin: {
                }
            },
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

    return oUI;
})();

var gridViewButton = {
    name: "ViewRequests",
    text: "View Requests",
    iconClass: "k-icon k-i-search",
    click: function (e) {
        var tr = $(e.target).closest("tr");
        var data = this.dataItem(tr);
        window.location = "/Requests/RequestsByUser?userid=" + data.UserId + "&username=" + data.Username;
    },

};

function onPdfExport(e) {
    debugger;
    current = e.sender;
}
