$(document).ready(function () {
        $.post("/Account/GetCurrentUser", function (user) {
                console.log(user.Email);
                console.log(user.RoleName);
                if (user.Email != null) {
                    $("#navbarLoggedIn_logOut").html("Odjavite se: " + user.Email);
                    if (user.RoleName === "Admin") {
                        $("#navbarLoggedIn_Users").removeClass("hide");
                    }
                    $("#navbarLoggedIn").removeClass("hide");
                }
            });
        });