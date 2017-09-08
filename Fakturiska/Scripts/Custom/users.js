$(document).ready(function () {
    $("#navbarLoggedIn_Users").addClass("active");

    $('#tableUsers').DataTable({
        responsive: true,
        "columnDefs": [{
            "targets": 2,
            "searchable": false,
            "orderable": false,
        }]
    });

    $('.editEmail').editable();
    var popover = $('#addUser').popover({
        html: true,
        title: function () {
            return $("#popover-head").html();
        },
        content: function () {
            return $("#popover-content").html();
        }
    });

    $(document).on('click', '#createUser', function () {
        console.log($('.popover-content').find('#roleSelect').val());
        console.log($('.popover-content').find('#userEmail').val());

        $.post("CreateUserWithoutPassword", { email: $('.popover-content').find('#userEmail').val(), role: $('.popover-content').find('#roleSelect').val() }, function (result) {
            $('.popover-content').find('#roleSelect').val("1");
            $('.popover-content').find('#userEmail').val("");
            $('#addUser').popover('hide');
        });
    });
});