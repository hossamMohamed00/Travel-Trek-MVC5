/* In WallLayout */

var loginIsRun = false;
var registerIsRun = false;

function showLogin() {
    //* Check if the register modal is already appear or not, if yes, remove it first
    if (registerIsRun) {
        registerIsRun = document.getElementById('register').classList.toggle('block');
    }

    loginIsRun = document.getElementById('login').classList.toggle('block');
}
function showRegister() {
    //* Check if the login modal is already appear or not, if yes, remove it first

    if (loginIsRun) {
        loginIsRun = document.getElementById('login').classList.toggle('block');
    }

    registerIsRun = document.getElementById('register').classList.toggle('block');
}

/*----------------------*/