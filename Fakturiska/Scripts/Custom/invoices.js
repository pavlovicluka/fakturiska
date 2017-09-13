$(document).ready(function () {
    $("#navbarLoggedIn_Invoices").addClass("active");

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

    new Dropzone(document.body, {
        url: "/Invoice/Upload",
        dragenter: function () {
        },
        dragleave: function () {
        }
    });
});

$(function () {
    $('form').submit(function () {
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {
                    var modal;
                    var archived = false;
                    if (currentModalId.indexOf("createInvoiceModal") !== -1) {
                        modal = $('#createInvoiceModal');
                    } else if (currentModalId.indexOf("editInvoiceModal") !== -1) {
                        modal = $('#editInvoiceModal' + currentModalId);
                    } else if (currentModalId.indexOf("editArchivedInvoiceModal") !== -1) {
                        modal = $('#editArchivedInvoiceModal' + currentModalId);
                        archived = true;
                    }

                    if (result.substring(1, 2) === "t") {
                        modal.modal('hide');
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        if (archived)
                        {
                            $('#resultArchive').html(result);
                        } else {
                            $('#result').html(result);
                        }
                    } else {
                        modal.find(".modal-body").html(result);
                    }
                }
            });
        }
        return false;
    });
});

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