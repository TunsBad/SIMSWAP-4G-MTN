"use strict";
//const Api = 'http://localhost:3704/' + 'api/simswap';
var popupNotification;

$(function () {
    var ui = new transferUI();
    ui.highValueSubscribersGrid();
});

var transferUI = (function () {
    function transferUI() { }
    transferUI.prototype.highValueSubscribersGrid = function () {

        var grid = $("#highvaluemsisdn").kendoGrid({
            editable: false,
            height: 610,
            toolbar: ["excel", "pdf", "create"],
            excel: {
                fileName: "HighValueMsisdn.xlsx",
                allPages: true,
                filterable: true
            },
            dataSource: {

                transport: {
                    read: {
                        url: Api + '/highvaluesubscribers',
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json"
                    },
                    update: {
                        url: Api + '/updatehighvaluesubscriber',
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json"
                    },
                    destroy: {
                        url: Api + "/deletehighvaluesubscriber",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json"
                    },
                    create: {
                        url: Api + "/createhighvaluesubscriber",
                        type: "POST",
                        dataType: "json",
                        contentType: "application/json"
                    },
                    parameterMap: function (data, type) {
                        return kendo.stringify(data);
                    }
                },
                pageSize: 10,
                schema: {
                    data: "Data",
                    total: "Count",
                    model: {
                        id: "Id",
                        fields: {
                            Msisdn: { editable: true, validation: { required: true } },
                            Id: { editable: false, validation: { required: true } }
                        }
                    }
                },
                serverPaging: true,
                serverFiltering: true,
                serverSorting: false
            },
            filterable: true,
            selectable: true,
            editable: true,
            columns: [
                { field: 'Msisdn', title: 'Subscriber Number', filterable: { cell: { operator: "contains" } }, width: "100px" },
                { command: [gridEditButton, gridDeleteButton], width: "100px" }
            ],
            edit: function (e) {
                if (!e.model.isNew()) {
                    e.preventDefault();
                    var id = e.model.Id;
                    console.log(id);
                }
                else {
                }
            },
            remove: function (e) {
                var id = e.model.Id;

                var retVal = confirm("Delete  " + e.model.Msisdn + "?");
                if (retVal == true) {
                    var req = {
                        Id: id
                    };

                    alert("Deleted");
                    return true;
                }
                else {
                    $('#highvaluemsisdn').data('kendoGrid').dataSource.cancelChanges();

                    return false;
                }

            },
            create: function (e) {
                if (!e.model.isNew()) {
                    e.preventDefault();
                    $('#highvaluemsisdn').data('kendoGrid').dataSource.read();
                    $('#highvaluemsisdn').data('kendoGrid').refresh();
                }
            },
            editable: { mode: 'inline' },
            sortable: { mode: 'multiple' },
            pageable: {
                pageSize: 15,
                pageSizes: [15, 30, 50, 100, 1000],
                previousNext: true,
                buttonCount: 5

            }

        });

    };

    return transferUI;
})();

var gridEditButton = {
    name: "edit",
    text: "",
    iconClass: "k-icon k-i-pencil",
    click: function (e) {

    }
};

var gridDeleteButton = {
    name: "destroy",
    text: "",
    iconClass: "k-icon k-i-delete",
    click: function (e) {
       
    }
};
