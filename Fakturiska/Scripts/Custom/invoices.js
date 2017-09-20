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
        language: { search: "" },
        "columnDefs": [{
            "targets": 2,
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
        ordering: false,
        bFilter: true,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
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
        $('#searchArchive').val("");
        tableArchive
            .columns()
            .search("")
            .draw();
    });
}

function submitForm() {
    var invoiceCompaniesModel = new FormData();
    var invoiceForm = $("#invoiceForm");

    if (invoiceForm.valid()) {

        var formArray = invoiceForm.serializeArray();
        var formObject = {};
        for (var i = 0; i < formArray.length; i++) {
            formObject[formArray[i]['name']] = formArray[i]['value'];
        }
        for (var key in formObject) {
            invoiceCompaniesModel.append(key, formObject[key]);
        }
        invoiceCompaniesModel.append("Invoice.File", dropzoneForm.getQueuedFiles()[0]);

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
                    prepareModal();
                }
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
            prepareModal();
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
            prepareModal();
            $("#invoiceModal").modal('toggle');
            archived = archive;
        }
    });
}

function deleteInvoice(invoiceId, rowId, archived) {
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
        }
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