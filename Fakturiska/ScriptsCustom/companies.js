$(document).ready(function () {
    $("#navbarLoggedIn_Companies").addClass("active");
    setDataTables(); 
});
var changes;

var tableCompanies;
function setDataTables() {
    tableCompanies = $('#tableCompanies').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        paging: true,
        language: {
            "sProcessing": "Procesiranje u toku...",
            "sLengthMenu": "Prikaži _MENU_ elemenata",
            "sZeroRecords": "Nije pronađen nijedan rezultat",
            "sInfo": "Prikaz _START_ do _END_ od ukupno _TOTAL_ elemenata",
            "sInfoEmpty": "Prikaz 0 do 0 od ukupno 0 elemenata",
            "sInfoFiltered": "(filtrirano od ukupno _MAX_ elemenata)",
            "sInfoPostFix": "",
            "sSearch": "Pretraga:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Početna",
                "sPrevious": "Prethodna",
                "sNext": "Sledeća",
                "sLast": "Poslednja"
            }
        },
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
        "autoWidth": false,
        "proccessing": true,
        "serverSide": true,
        "ajax": {
            url: "/Company/ServerSideSearchAction",
            type: 'POST'
        },
        "columnDefs": [
            {
                "targets": 0,
                "responsivePriority": 1,
            },
            {
                "targets": 1,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 2,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 3,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 4,
                "render": function (data, type, row) {
                    dataShort = (type === 'display' && data.length > 10 ?
                        data.substr(0, 10) + '…' :
                        data);
                    return '<a href="' + data + '">' + dataShort + '</a>';
                }
            },
            {
                "targets": 5,
                "render": function (data, type, row) {
                    dataShort = (type === 'display' && data.length > 10 ?
                        data.substr(0, 10) + '…' :
                        data);
                    return '<a href="mailto:' + data + '">' + dataShort + '</a>';
                }
            },
            {
                "targets": 6,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 7,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 6,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 9,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 10,
                "render": $.fn.dataTable.render.ellipsis()
            },
            {
                "targets": 11,
                "responsivePriority": 2,
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

$.fn.dataTable.render.ellipsis = function () {
    return function (data, type, row) {
        return type === 'display' && data.length > 8 ?
            data.substr(0, 8) + '…' :
            data;
    }
};

function submitForm() {
    var companyForm = $("#companyForm");

    if (companyForm.valid()) {
        $.ajax({
            url: "/Company/CreateCompany",
            type: "POST",
            data: companyForm.serialize(),
            success: function (result) {
                if (result === "success") {
                    $("#companyModal").modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();
                    tableCompanies.search("").draw();
                    changes.server.companiesChanged("refresh");
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
            $("#companyModalTitle").html("Kreiraj pravno lice");
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
            $("#companyModalTitle").html("Izmeni pravno lice");
            $("#companyModal").modal('toggle');
        }
    });
}

function deleteCompany(companyId, modalId) {
    $("#deleteModal").modal('toggle');
    $("#submitDelete").click(function () {
        $.ajax({
            url: "/Company/DeleteCompany",
            type: "POST",
            data: { id: companyId },
            success: function (result) {
                $("#" + modalId).parent().closest('tr').remove();
                $("#deleteModal").modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
                changes.server.companiesChanged("refresh");
            }
        });
    });
}

$(function () {
    changes = $.connection.realTime;

    changes.client.companiesChanged = function (message) {
        if (message === "refresh") {
            $.notify("Promene su ažurirane", "success");
            tableCompanies.search("").draw();
        }
    };

    $.connection.hub.start();
});