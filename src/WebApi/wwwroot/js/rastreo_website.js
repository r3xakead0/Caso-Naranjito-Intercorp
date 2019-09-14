$(document).ready(function () {

    var guid, page, link;

    // Actual page
    page = window.location.href;

    // Get and set session
    if (localStorage.getItem("guid")) {
        guid = localStorage.getItem("guid", guid);  
    } else {
        guid = getSession(page);
        console.log(guid);
        localStorage.setItem("guid", guid);
    }

    // Register all link events
    link = '';
    $("a").click(function (e) {
        link = $(this).attr("href");
        setLink(guid, link);
    });

    // Check in page
    initPage(guid, page);

    window.onbeforeunload = function (event) {
        // Check out page
        endedPage(guid, page);
    };
});

function getSession(page) {
    var rpta = '';
    $.ajax({
        url: '/analytics/session',
        type: 'POST',
        data: JSON.stringify({ url: page }),
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Accept", "application/json");
            xhr.setRequestHeader("Content-Type", "application/json");
        },
        success: function (resposeJsonObject) {
            rpta = resposeJsonObject;
        }
    });
    return rpta.uuid
}

function setLink(guid, page) {
    var rpta = false;
    $.ajax({
        url: '/analytics/link',
        type: 'POST',
        data: JSON.stringify({ url: page, uuid: guid }),
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Accept", "application/json");
            xhr.setRequestHeader("Content-Type", "application/json");
        },
        success: function (resposeJsonObject) {
            rpta = true;
        },
    });
    return rpta;
}

function initPage(guid, page) {
    var rpta = false;
    $.ajax({
        url: '/analytics/page/init',
        type: 'POST',
        data: JSON.stringify({ url: page, uuid: guid }),
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Accept", "application/json");
            xhr.setRequestHeader("Content-Type", "application/json");
        },
        success: function (resposeJsonObject) {
            rpta = true;
        },
    });
    return rpta;
}

function endedPage(guid, page) {
    var rpta = false;
    $.ajax({
        url: '/analytics/page/ended',
        type: 'POST',
        data: JSON.stringify({ url: page, uuid: guid }),
        async: false,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("Accept", "application/json");
            xhr.setRequestHeader("Content-Type", "application/json");
        },
        success: function (resposeJsonObject) {
            rpta = true;
        },
    });
    return rpta;
}