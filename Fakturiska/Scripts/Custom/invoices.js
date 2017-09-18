$(document).ready(function () {
    $("#navbarLoggedIn_Invoices").addClass("active");

    setDataTables();
    setDataTablesArchive();

    new Dropzone(document.body, {
        url: "/Invoice/Upload",
        dragenter: function () {
        },
        dragleave: function () {
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
    var invoiceForm = $("#invoiceForm");

    if (invoiceForm.valid()) {
        $.ajax({
            url: "/Invoice/CreateInvoice",
            type: "POST",
            data: invoiceForm.serialize(),
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
                }
            }
        });
    }
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
        }
    });
}

$(function () {
    var chat = $.connection.realTime;

    chat.client.Send = function (message) {
        $.notify(message, "success");
    };

    $.connection.hub.start();
});