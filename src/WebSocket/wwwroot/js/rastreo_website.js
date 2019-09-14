$(document).ready(function () {

    var guid = '';
    var link = '';
    var page = window.location.href; // Actual page

    localStorage.removeItem("guid"); //TEMP

    getSocket().then(async function (server) {

        // Get and set session
        if (localStorage.getItem("guid")) {
            guid = localStorage.getItem("guid", guid);
        } else {
            guid = await getSession(server, page);
            localStorage.setItem("guid", guid);
        }

        // Register all link events
        $("a").click(async function (e) {
            link = $(this).attr("href");
            await setLink(server, link, guid);
        });

        // Check in page
        await initPage(server, page, guid);

        // Check out page
        window.onbeforeunload = async function (event) {
            await endedPage(server, page, guid);
        };
    })

});

// function getSocket() {
//     let webSocketProtocol = location.protocol == "https:" ? "wss:" : "ws:";
//     let webSocketURI = webSocketProtocol + "//" + location.host + "/session";

//     var server = new WebSocket(webSocketURI);

//     server.onopen = function () {
//         console.log("Connected.");
//     };
//     server.onclose = function (error) {
//         if (event.wasClean) {
//             console.log('Disconnected.');
//         } else {
//             console.log('Connection lost.'); // for example if server processes is killed
//         }
//         console.log('Code: ' + event.code + '. Reason: ' + event.reason);
//     };
//     server.onmessage = function (event) {
//         responde = event.data;
//         console.log("Data received: " + event.data);
//     };
//     server.onerror = function (error) {
//         console.log("Error: " + error.message);
//     };

//     return server;
// }

function getSocket() {
    let webSocketProtocol = location.protocol == "https:" ? "wss:" : "ws:";
    let webSocketURI = webSocketProtocol + "//" + location.host + "/session";

    return new Promise(function (resolve, reject) {
        var server = new WebSocket(webSocketURI);
        server.onopen = function () {
            console.log("Connected.");
            resolve(server);
        };
        server.onclose = function (error) {
            if (event.wasClean) {
                console.log('Disconnected.');
            } else {
                console.log('Connection lost.'); // for example if server processes is killed
            }
            console.log('Code: ' + event.code + '. Reason: ' + event.reason);
        };
        server.onmessage = function (event) {
            console.log("Data received: " + event.data);
        };
        server.onerror = function (error) {
            console.log("Error: " + error.message);
        };
    });
}

const getSession = async (socket, page) => {
    let json = {
        method: 'session',
        post: {
            uuid: '1',
            url: page
        }
    }
    let message = JSON.stringify(json);

    return await new Promise(function (resolve, reject) {
        socket.onmessage = function (event) {
            resolve(event.data);
            console.log("Session: " + event.data);
        };
        socket.send(message);
    });
}


const initPage = async (socket, page, guid) => {

    let json = {
        method: 'initpage',
        post: {
            uuid: guid,
            url: page
        }
    }
    let message = JSON.stringify(json);
    
    return await new Promise(function (resolve, reject) {
        
        socket.onmessage = function (event) {
            resolve(event.data);
            console.log("Page Initial: " + event.data);
        };
        socket.send(message);
    });
}

const endedPage = async (socket, page, guid) => {
    let json = {
        method: 'endedPage',
        post: {
            uuid: guid,
            url: page
        }
    }
    let message = JSON.stringify(json);

    return await new Promise(function (resolve, reject) {
        socket.onmessage = function (event) {
            resolve(event.data);
            console.log("Page End: " + event.data);
        };
        socket.send(message);
    });
}

async function setLink(socket, page, guid) {
    let json = {
        method: 'link',
        post: {
            uuid: guid,
            url: page
        }
    }
    let message = JSON.stringify(json);

    return await new Promise(function (resolve, reject) {
        socket.onmessage = function (event) {
            resolve(event.data);
            console.log("Link: " + event.data);
        };
        socket.send(message);
    });
}
