﻿$(document).ready(function () {
    $("#navbarLoggedIn_Companies").addClass("active");
    setDataTables(); 
});

function setDataTables() {
    tableCompanies = $('#tableCompanies').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        paging: true,
        language: { search: "" },
        aoColumns: [
            { mData: 'Name' },
            { mData: 'PhoneNumber' },
            { mData: 'FaxNumber' },
            { mData: 'Address' },
            { mData: 'Website' },
            { mData: 'Email' },
            { mData: 'PersonalNumber' },
            { mData: 'PIB' },
            { mData: 'MIB' },
            { mData: 'AccountNumber' },
            { mData: 'BankCode' },
            { mData: 'CompanyGuid' },
        ],
        responsive: true,
        "proccessing": true,
        "serverSide": true,
        "ajax": {
            url: "/Company/ServerSideSearchAction",
            type: 'POST'
        },
        "columnDefs": [
            {
                "targets": 11,
                "searchable": false,
                "orderable": false,
                "render": function (data, type, row) {
                    return '<div class="row" id="'+row.CompanyId+'"> <a class="btn btn-info" onclick="editCompany(\''
                        + data + '\')"><span class="glyphicon glyphicon-edit"></span></a> <a class="btn btn-danger" onclick="deleteCompany(\''
                        + data + '\' , \'' + row.CompanyId + '\')"><span class="glyphicon glyphicon-remove"></span></a> </div> ';
                }
            }]
    });

    $('#searchCompanies').on('keyup change', function () {
        if (tableCompanies.search() !== this.value || this.value === "") {
            tableCompanies
                .search(this.value)
                .draw();
        }
    });
}

function submitForm() {
    var companyForm = $("#companyForm");

    if (companyForm.valid()) {
        $.ajax({
            url: "/Company/CreateCompany",
            type: "POST",
            data: companyForm.serialize(),
            success: function (result) {

                if (result.substring(1, 2) === "t") {
                    $("#companyModal").modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                    $('#result').html(result);
                    setDataTables();
                } else {
                    $("#companyModal").find(".modal-body").html(result);
                }
            }
        });
    }
}

function createCompany() {
    $.ajax({
        url: "/Company/CreateCompany",
        type: "GET",
        success: function (result) {
            $("#companyModalBody").html(result);
            $("#companyModal").modal('toggle');
        }
    });
}

function editCompany(companyId) {
    $.ajax({
        url: "/Company/EditCompany",
        type: "POST",
        data: { id: companyId },
        success: function (result) {
            $("#companyModalBody").html(result);
            $("#companyModal").modal('toggle');
        }
    });
}

function deleteCompany(companyId, modalId) {
    $.ajax({
        url: "/Company/DeleteCompany",
        type: "POST",
        data: { id: companyId },
        success: function (result) {
            $("#" + modalId).parent().closest('tr').remove();
        }
    });
}

$(function () {
    var changes = $.connection.realTime;

    changes.client.CompaniesChange = function (message) {
        $.notify(message, "success");
        if (message === "refresh") {
            $.ajax({
                url: "/Company/TableCompanies",
                type: "GET",
                success: function (result) {
                    $('#result').html(result);
                    setDataTables();
                }
            });
        }
    };

    $.connection.hub.start();
});