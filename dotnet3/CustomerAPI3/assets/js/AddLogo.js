var addLogoDo = function (e) {
    if (e) {
        e.insertAdjacentHTML('afterbegin', '<img src="/assets/images/favicon-32x32.png" class="logo">');
    }
};

var addLogo = function () {
    var e = document.querySelector('.title');
    if (e) {
        addLogoDo(e);
    } else {
        setTimeout(function () { addLogo(); }, 50);
    }
};

window.addEventListener('load', function () {
    setTimeout(function () { addLogo(); }, 50);
}, false);