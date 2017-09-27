$(document).ready(function () {
    $("#navbarLoggedIn_Invoices").addClass("active");

    setDataTables();
    setDataTablesArchive();

    var dragCounter = 0;
    new Dropzone(document.body, {
        url: "/Invoice/Upload",
        acceptedFiles: "application/pdf, image/jpeg, image/png",
        previewsContainer: false,
        clickable: false,
        success: function (file, response) {
            $.notify("Invoice uploaded", "success");
            getInvoices();
        },
        dragenter: function () {
            dragCounter++;
            $("#dimmerHome").show();
        },
        dragleave: function () {
            dragCounter--;
            if (dragCounter === 0) {
                $("#dimmerHome").hide();
            }
        },
        drop: function () {
            dragCounter = 0;
            $("#dimmerHome").hide();
        }
    });
});
var archived = "false";

function setDataTables() {
    tableInvoices = $('#tableInvoices').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        responsive: true,
        "autoWidth": false,
        language: { search: "" },
        "columnDefs": [
            {
                "type": "date-eu", targets: 1
            },
            {
                "targets": 0,
                "responsivePriority": 1,
            },
            {
                "targets": -1,
                "responsivePriority": 2,
                "searchable": false,
                "orderable": false
            }],
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
    });

    $('#searchInvoices').on('keyup change', function () {
        if (tableInvoices.search() !== this.value || this.value === "") {
            tableInvoices
                .search(this.value)
                .draw();
        }
    });
}

function setDataTablesArchive() {
    var tableArchive = $('#tableArchive').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        responsive: true,
        "autoWidth": false,
        bFilter: true,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "columnDefs": [
            {
                "type": "date-eu", targets: 1
            },
            {
                "targets": 0,
                "responsivePriority": 1,
            },
            {
                "targets": 2,
                "render": function (data, type, row) {
                    if (type === "display") {
                        if (data === "True") {
                            return '<input checked="checked" class="check-box" disabled="disabled" type="checkbox">';
                        } else {
                            return '<input class="check- box" disabled="disabled" type="checkbox">';
                        }
                    }
                    return data;
                }
            },
            {
                "targets": 3,
                "render": function (data, type, row) {
                    if (type === "display") {
                        if (data === "True") {
                            return '<input checked="checked" class="check-box" disabled="disabled" type="checkbox">';
                        } else {
                            return '<input class="check- box" disabled="disabled" type="checkbox">';
                        }
                    }
                    return data;
                }
            },
            {
                "targets": 4,
                "render": function (data, type, row) {
                    if (type === "display") {
                        if (data === "True") {
                            return '<input checked="checked" class="check-box" disabled="disabled" type="checkbox">';
                        } else {
                            return '<input class="check- box" disabled="disabled" type="checkbox">';
                        }
                    }
                    return data;
                }
            },
            {
                "targets": 5,
                "render": function (data, type, row) {
                    if (type === "display") {
                        if (data === "True") {
                            return '<input checked="checked" class="check-box" disabled="disabled" type="checkbox">';
                        } else {
                            return '<input class="check- box" disabled="disabled" type="checkbox">';
                        }
                    }
                    return data;
                }
            },
            {
                "targets": 6,
                "render": function (data, type, row) {
                    if (type === "display") {
                        if (data === "True") {
                            return '<input checked="checked" class="check-box" disabled="disabled" type="checkbox">';
                        } else {
                            return '<input class="check- box" disabled="disabled" type="checkbox">';
                        }
                    }
                    return data;
                }
            },
            {
                "targets": -1,
                "responsivePriority": 2,
                "searchable": false,
                "orderable": false
            }],
    });

    $('#searchArchive').on('keyup change', function () {
        if (tableArchive.search() !== this.value || this.value === "") {
            tableArchive
                .columns($('#categorySelect').val())
                .search(this.value)
                .draw();
        }
    });

    $('#categorySelect').change(function () {
        var selected = $('#checkboxSelect').val();
        $('#searchArchive').val("");
        tableArchive
            .columns()
            .search("");
        for (var i = 0; i < selected.length; i++) {
            tableArchive
                .columns(selected[i])
                .search("True");
        }
        tableArchive.draw();
    });

    $('#checkboxSelect').change(function () {
        var selected = $('#checkboxSelect').val();
        tableArchive
            .columns()
            .search("");
        tableArchive
            .columns($('#categorySelect').val())
            .search($('#searchArchive').val());
        for (var i = 0; i < selected.length; i++) {
            tableArchive
                .columns(selected[i])
                .search("True");
        }
        tableArchive.draw();
    });
}

var invoiceFile = null; 
function submitForm() {
    var invoiceCompaniesModel = new FormData();
    var invoiceForm = $("#invoiceForm");

    if (invoiceForm.valid()) {

        disableEnableFields("CompanyReceiver", false);
        disableEnableFields("CompanyPayer", false);
        var formArray = invoiceForm.serializeArray();
        
        var formObject = {};
        for (var i = 0; i < formArray.length; i++) {
            if (formObject[formArray[i]['name']] == null) {
                formObject[formArray[i]['name']] = formArray[i]['value'];
            }
        }

        for (var key in formObject) {
            invoiceCompaniesModel.append(key, formObject[key]);
        }

        if (dropzoneForm !== null) {
            if (invoiceFile === null) {
                if (dropzoneForm.getQueuedFiles().length === 1) {
                    invoiceFile = dropzoneForm.getQueuedFiles()[0];
                }
            }
            invoiceCompaniesModel.append("Invoice.File", invoiceFile);
        }

        //console.log(formObject);

        $.ajax({
            url: "/Invoice/CreateInvoice",
            data: invoiceCompaniesModel,
            type: "POST",
            processData: false,
            contentType: false,
            success: function (result) {

                if (result.substring(1, 2) === "t") {
                    $("#invoiceModal").modal('hide');
                    $('body').removeClass('modal-open');
                    $('.modal-backdrop').remove();

                    if (archived === "true") {
                        $('#resultArchive').html(result);
                        setDataTablesArchive();
                    } else {
                        $('#result').html(result);
                        setDataTables();
                    }
                } else {
                    $("#invoiceModal").find(".modal-body").html(result);

                    if (invoiceCompaniesModel.get("Invoice.InvoiceGuid") !== null && invoiceCompaniesModel.get("Invoice.InvoiceGuid") !== "") {
                        prepareEditModal();

                        if (invoiceCompaniesModel.get("CompanyReceiver.CompanyGuid") === null || invoiceCompaniesModel.get("CompanyReceiver.CompanyGuid") === "") {
                            disableEnableFields("CompanyReceiver", false);
                        }
                        if (invoiceCompaniesModel.get("CompanyPayer.CompanyGuid") === null || invoiceCompaniesModel.get("CompanyPayer.CompanyGuid") === "") {
                            disableEnableFields("CompanyPayer", false);
                        }
                    } else {
                        prepareCreateModal();

                        if (invoiceCompaniesModel.get("CompanyReceiver.CompanyGuid") === null || invoiceCompaniesModel.get("CompanyReceiver.CompanyGuid") === "") {
                            disableEnableFields("CompanyReceiver", false);
                        }
                        if (invoiceCompaniesModel.get("CompanyPayer.CompanyGuid") === null || invoiceCompaniesModel.get("CompanyPayer.CompanyGuid") === "") {
                            disableEnableFields("CompanyPayer", false);
                        }

                        if (invoiceFile !== null) {
                            dropzoneForm.emit("addedfile", invoiceFile);
                            dropzoneForm.files.push(invoiceFile);
                        }
                    }    
                }
            },
            error: function (result) {
                console.log(result);
            }
        });
    }
}

function getInvoices() {
    $.ajax({
        url: "/Invoice/TableInvoices",
        type: "GET",
        success: function (result) {
            $('#result').html(result);
            setDataTables();
        }
    });
}

function createInvoice() {
    $.ajax({
        url: "/Invoice/CreateInvoice",
        type: "GET",
        success: function (result) {
            $("#invoiceModalBody").html(result);
            $("#invoiceModalTitle").html("Dodaj novu fakturu");
            invoiceFile = null;
            prepareCreateModal();
            $("#invoiceModal").modal('toggle');
        }
    });
}

function editInvoice(invoiceId, archive) {
    $.ajax({
        url: "/Invoice/EditInvoice",
        type: "POST",
        data: { id: invoiceId },
        success: function (result) {
            $("#invoiceModalBody").html(result);
            $("#invoiceModalTitle").html("Izmeni fakturu");
            invoiceFile = null;
            prepareEditModal();
            $("#invoiceModal").modal('toggle');
            archived = archive;
        }
    });
}

function deleteInvoice(invoiceId, rowId, archived) {
    $("#deleteModal").modal('toggle');
    $("#submitDelete").click(function () {
        $.ajax({
            url: "/Invoice/DeleteInvoice",
            type: "POST",
            data: { id: invoiceId },
            success: function (result) {
                if (archived === "true") {
                    $("#rowArchive" + rowId).remove();
                } else {
                    $("#row" + rowId).remove();
                }
                $("#deleteModal").modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
        });
    });
}

function archiveInvoice(invoiceId, rowId) {
    $.ajax({
        url: "/Invoice/ArchiveInvoice",
        type: "POST",
        data: { id: invoiceId },
        success: function (result) {
            $("#row" + rowId).remove();
            $('#resultArchive').html(result);
            setDataTablesArchive();
        }
    });
}

function printInvoice(guid) {   
    var w = window.open("/Invoice/PrintInvoice?guid=" + guid);
    w.print();
}

$(function () {
    var changes = $.connection.realTime;
    changes.client.MailReceived = function (message) {
        $.notify(message, "success");
        getInvoices();
    };

    $.connection.hub.start();
});