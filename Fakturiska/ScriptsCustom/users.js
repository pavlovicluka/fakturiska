﻿$(document).ready(function () {
    $("#navbarLoggedIn_Users").addClass("active");

    setDataTables();
    setDataTablesWaiting();

    //$('.editEmail').editable();
    $('.editRole').editable({
        source: [{ value: 1, text: "Admin" }, { value: 2, text: "User" }],
    });
    
    var popover = $('#userPopover').popover({
        html: true,
        title: function () {
            return $("#popover-head").html();
        },
        content: function () {
            return $("#popover-content").html();
        }
    });
});

function setDataTables() {
    tableUsers = $('#tableUsers').DataTable({
        "dom": '<"pull-right"l>t<"pull-left"i><"pull-right"p>',
        language: { search: "" },
        responsive: true,
        "autoWidth": false,
        language: {
            "sProcessing": "Procesiranje u toku...",
            "sLengthMenu": "Prikaži _MENU_ elemenata",
            "sZeroRecords": "Nije pronađen nijedan rezultat",
            "sInfo": "Prikaz _START_ do _END_ od ukupno _TOTAL_ elemenata",
            "sInfoEmpty": "Prikaz 0 do 0 od ukupno 0 elemenata",
            "sInfoFiltered": "(filtrirano od ukupno _MAX_ elemenata)",
            "sInfoPostFix": "",
            "sSearch": "Pretraga:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Početna",
                "sPrevious": "Prethodna",
                "sNext": "Sledeća",
                "sLast": "Poslednja"
            }
        },
        "columnDefs": [
            {
                "targets": 0,
                "responsivePriority": 1,
            },
            {
                "targets": -1,
                "responsivePriority": 2,
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
        "autoWidth": false,
        language: {
            "sProcessing": "Procesiranje u toku...",
            "sLengthMenu": "Prikaži _MENU_ elemenata",
            "sZeroRecords": "Nije pronađen nijedan rezultat",
            "sInfo": "Prikaz _START_ do _END_ od ukupno _TOTAL_ elemenata",
            "sInfoEmpty": "Prikaz 0 do 0 od ukupno 0 elemenata",
            "sInfoFiltered": "(filtrirano od ukupno _MAX_ elemenata)",
            "sInfoPostFix": "",
            "sSearch": "Pretraga:",
            "sUrl": "",
            "oPaginate": {
                "sFirst": "Početna",
                "sPrevious": "Prethodna",
                "sNext": "Sledeća",
                "sLast": "Poslednja"
            }
        },
        "columnDefs": [
            {
                "targets": 0,
                "responsivePriority": 1,
            },
            {
                "targets": -1,
                "responsivePriority": 2,
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

function submitForm() {
    var userForm = $("#userForm");
    
    if (userForm.valid()) {
        $(".popover-content").html("<div class='center-block loader'></div>");
        $.ajax({
            url: "/User/CreateUserWithoutPassword",
            type: "POST",
            data: userForm.serialize(),
            success: function (result) {
                if (result.substring(1, 2) === "t") {
                    $('#userPopover').popover('hide');
                    $('#resultWaiting').html(result);
                    setDataTablesWaiting();
                } else {
                    $(".popover-content").html(result);
                }
            }
        });
    }
}

function getUsers() {
    $.ajax({
        url: "/User/TableUsers",
        type: "GET",
        success: function (result) {
            $('#result').html(result);
            setDataTables();
        }
    });
}

function getUsersWaiting() {
    $.ajax({
        url: "/User/TableUsersWaiting",
        type: "GET",
        success: function (result) {
            $('#resultWaiting').html(result);
            setDataTablesWaiting();
        }
    });
}


var popoverOpened = false;
function createUser() {
    if (!popoverOpened) {
        popoverOpened = true;
        $.ajax({
            url: "/User/CreateUserWithoutPassword",
            type: "GET",
            success: function (result) {
                $('#userPopover').popover('show');
                $(".popover-content").html(result);
            }
        });
    } else {
        popoverOpened = false;
    }
}

function deleteUser(userId, rowId, waiting) {
    $("#deleteModal").modal('toggle');
    $("#submitDelete").click(function () {
        $.ajax({
            url: "/User/DeleteUser",
            type: "POST",
            data: { id: userId },
            success: function (result) {
                if (waiting === "true") {
                    getUsersWaiting();
                } else {
                    getUsers();
                }
                $("#deleteModal").modal('hide');
                $('body').removeClass('modal-open');
                $('.modal-backdrop').remove();
            }
        });
    });
}

$(function () {
    var changes = $.connection.realTime;
    changes.client.NewUser = function (message) {
        $.notify(message, "success");
        getUsers();
        getUsersWaiting();
        $('.editRole').editable({
            source: [{ value: 1, text: "Admin" }, { value: 2, text: "User" }],
        });
    };

    $.connection.hub.start();
});