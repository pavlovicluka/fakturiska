$(document).ready(function () {
    $("#navbarLoggedIn_Companies").addClass("active");

    $('#tableCompanies').DataTable({
        responsive: true,
        "columnDefs": [{
            "targets": 11,
            "searchable": false,
            "orderable": false,
        }],
    });
});