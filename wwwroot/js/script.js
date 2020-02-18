//skrypt obsługujący modal do usunięcia konta
$("#deleteAccountButton").click(function () {
    $("#deleteAccountModal").modal('hide');
    window.location.href = '@Url.Action("Delete","Account")'
    //$.ajax({
    //    url: '@Url.Action("Delete", "Account")',
    //    success: function () {
    //        location.reload();
    //    },
    //});
});

//skrypt obsługujący modal do wylogowania się
$("#logoutButton").click(function () {
    $("#logoutModal").modal('hide');
    window.location.href = '@Url.Action("Logout","Account")'
});

var passwordInput = $(".passwordInput");
var confirmPasswordInput = $(".confirmPasswordInput");

//skrypt do sprawdzenia czy hasła są identyczne
$('.registerPassword input[type="password"]').on("keyup", function () {
    if (passwordInput.val() == confirmPasswordInput.val() || (passwordInput.val() === "" && confirmPasswordInput.val() === "")) {
        $(".saveChangesButton").removeAttr("disabled");
        $(".confirmPasswordDiv").attr("hidden", true);
    }
    else {
        $(".saveChangesButton").attr("disabled", true);
        $(".confirmPasswordDiv").removeAttr("hidden");
    }
});

var warningInput = $("#capsLockWarningInput");

//skrypt do sprawdzenia czy Caps Lock jest wciśnięty przy logowaniu
warningInput.keypress(function (e) {

    var character = e.keyCode ? e.keyCode : e.which;
    var sftKey = e.shiftKey ? e.shiftKey : ((character == 16) ? true : false);

    if (((character >= 65 && character <= 90) && !sftKey) || ((character >= 97 && character <= 122) && sftKey)) {
        $("#capsLockWarningDiv").removeAttr("hidden");
    } else {
        $("#capsLockWarningDiv").attr("hidden", true);
    }
});