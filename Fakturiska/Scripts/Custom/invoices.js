$(document).ready(function () {
    $("#navbarLoggedIn_Invoices").addClass("active");

    tableInvoices = $('#tableInvoices').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
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
                    //$('#editInvoiceModal').modal('toggle');
                    //$('#createInvoiceModal').modal('toggle');
                    $('#tableInvoices').html($(result).find("#tableInvoices"));
                    $('#tableArchive').html($(result).find("#tableArchive"));
                }
            });
        }
        return false;
    });
});