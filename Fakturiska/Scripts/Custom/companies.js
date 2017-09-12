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

var currentModalId;
function setModal(id) {
    currentModalId = id;
}

$(function () {
    $('form').submit(function () {
        if ($(this).valid()) {
            $.ajax({
                url: this.action,
                type: this.method,
                data: $(this).serialize(),
                success: function (result) {  
                    var modal;
                    if (currentModalId === "createCompanyModal") {
                        modal = $('#createCompanyModal');
                    } else {
                        modal = $('#editCompanyModal' + currentModalId);
                    }

                    if (result.substring(1, 2) === "t") {
                        modal.modal('hide');
                        $('body').removeClass('modal-open');
                        $('.modal-backdrop').remove();
                        $('#result').html(result);
                    } else {
                        modal.find(".modal-body").html(result);
                    }
                }
            });
        }
        return false;
    });
});

function deleteCompany(companyId, modalId) {
    $.ajax({
        url: "/Company/DeleteCompany",
        type: "POST",
        data: { id: companyId },
        success: function (result) {
            $("#" + modalId).remove();
        }
    });
}