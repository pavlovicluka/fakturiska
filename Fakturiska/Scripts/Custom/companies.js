$(document).ready(function () {
    $("#navbarLoggedIn_Companies").addClass("active");

    tableCompanies = $('#tableCompanies').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        language: { search: "" },
        responsive: true,
        "columnDefs": [{
            "targets": 11,
            "searchable": false,
            "orderable": false
        }]
    });

    $('#searchCompanies').on('keyup change', function () {
        if (tableCompanies.search() !== this.value || this.value === "") {
            tableCompanies
                .search(this.value)
                .draw();
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
                    $('#createCompanyModal').modal('toggle');
                    $('#tableCompanies').html($(result).find("#tableCompanies"));
                }
            });
        }
        return false;
    });
});

function deleteCompany(id) {
    $.ajax({
        url: "/Company/DeleteCompany",
        type: "POST",
        data: { id: id },
        success: function (result) {
            $('#result').html(result);
        }
    });
}