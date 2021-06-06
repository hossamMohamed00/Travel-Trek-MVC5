var loginIsRun = false;
var registerIsRun = false;

/* This method will be called after success of the ajax request for register*/
function RegisterUser(data) {
    if (data.success) {
        var dialog = bootbox.dialog({
            title: "Register You In Our Website 🐱‍🏍",
            message: '<p><i class="fa fa-spin fa-spinner"></i> Processing 👀...</p>',
            backdrop: true,
            buttons: {
                Ok: {
                    label: "Ok",
                    className: 'btn-success',
                    callback: function () {
                        //* Hide register and show login popup
                        loginIsRun = document.getElementById('register').classList.toggle('block'); // remove register 
                        registerIsRun = document.getElementById('login').classList.toggle('block');
                    }
                }
            }
        });

        dialog.init(function () {
            setTimeout(function () {
                    //* Target the dialog body and put the success msg
                    dialog.find('.bootbox-body').html(data.message);
                    dialog.closeButton = true;
                },
                1000);
        });

    } else {
        bootbox.alert(data.message);
    }
};

/* This method will be called after success of the ajax request for login */
function LoginUser(data) {
    if (data.success) {
        //* Redirect the user
        window.location.replace(data.url);
    } else {
        //* Error happened
        bootbox.alert(data.message);
    }
}

/* This method will be called if the user click login of the nav bar  */
function showLogin() {
    //* Check if the register modal is already appear or not, if yes, remove it first
    if (registerIsRun) {
        registerIsRun = document.getElementById('register').classList.toggle('block');
    }

    loginIsRun = document.getElementById('login').classList.toggle('block');
}

/* This method will be called if the user click register of the nav bar  */
function showRegister() {
    //* Check if the login modal is already appear or not, if yes, remove it first

    if (loginIsRun) {
        loginIsRun = document.getElementById('login').classList.toggle('block');
    }

    registerIsRun = document.getElementById('register').classList.toggle('block');
}

/*----------------------*/

/* In Partial Login */

function togglePassword() {
    var x = document.getElementById("password");
    if (x.type === "password") {
        x.type = "text";
    } else {
        x.type = "password";
    }
}