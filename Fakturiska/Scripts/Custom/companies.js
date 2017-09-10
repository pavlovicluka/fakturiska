$(document).ready(function () {
    $("#navbarLoggedIn_Companies").addClass("active");

    $('#tableCompanies').DataTable({
        "dom": '<"pull-left"f><"pull-right"l>t<"pull-left"i><"pull-right"p>',
        responsive: true,
        "columnDefs": [{
            "targets": 11,
            "searchable": false,
            "orderable": false,
        }],
    });
});