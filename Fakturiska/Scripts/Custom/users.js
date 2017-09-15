$(document).ready(function () {
    $("#navbarLoggedIn_Users").addClass("active");

    setDataTables();
    setDataTablesWaiting();

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

       var role = $('.popover-content').find('#roleSelect').val();
       var email = $('.popover-content').find('#userEmail').val();
       $(".popover-content").html("<div class='center-block loader'></div>");

        $.post("CreateUserWithoutPassword", { email: email, role: role }, function (result) {
            $('.popover-content').find('#roleSelect').val("1");
            $('.popover-content').find('#userEmail').val("");
            $('#addUser').popover('hide');
            $(".popover-content").html($("#createUserForm").html());

            $('#resultWaiting').html(result);
        });
    });
});

function setDataTables() {
    tableUsers = $('#tableUsers').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        language: { search: "" },
        responsive: true,
        "columnDefs": [{
            "targets": 2,
            "searchable": false,
            "orderable": false
        }]
    });

    $('#searchUsers').on('keyup change', function () {
        if (tableUsers.search() !== this.value || this.value === "") {
            tableUsers
                .search(this.value)
                .draw();
        }
    });
}

function setDataTablesWaiting() {
    tableUsersWaiting = $('#tableUsersWaiting').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        language: { search: "" },
        responsive: true,
        "columnDefs": [{
            "targets": 2,
            "searchable": false,
            "orderable": false
        }]
    });

    $('#searchUsersWaiting').on('keyup change', function () {
        if (tableUsersWaiting.search() !== this.value || this.value === "") {
            tableUsersWaiting
                .search(this.value)
                .draw();
        }
    });
}

function deleteUser(userId, rowId, waiting) {
    $.ajax({
        url: "/User/DeleteUser",
        type: "POST",
        data: { id: userId },
        success: function (result) {
            if (waiting === "true") {
                $("#rowWaiting" + rowId).remove();
            } else {
                $("#row" + rowId).remove();
            }
        }
    });
}