$(document).ready(function () {
    $("#navbarLoggedIn_Invoices").addClass("active");

    var tableArchive = $('#tableArchive').DataTable({
        responsive: true,
        ordering: false,
        bFilter: true,
        dom: 'ltip',
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
    });

    $('#searchArchive').on('keyup change', function () {
        if (tableArchive.search() !== this.value || this.value == "") {
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

    tableInvoices = $('#tableInvoices').DataTable({
        "columnDefs": [{
            "targets": 2,
            "searchable": false,
            "orderable": false,
        }],
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
    });

    new Dropzone(document.body, {
        url: "/Invoice/Upload",
        dragenter: function () {
        },
        dragleave: function () {
        },
    });
});