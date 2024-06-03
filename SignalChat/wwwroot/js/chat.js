var token = '';

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chathub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected.");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(async () => {
    await start();
});

// Start the connection.
start();

register = function () {
    user = document.getElementById("user").value;
    pwd = document.getElementById("pwd").value;
    confirmPwd = document.getElementById("confirmPwd").value;

    fetch('http://localhost:38655/api/account/register', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username: user, password: pwd, confirmPassword: confirmPwd })
    }).then(function (response) {
        if (response.ok) {
            console.log('Successfully registered.');
        } else {
            console.log('Something went wrong!');
        }
    }).catch(function (error) {
        console.log(error);
    });
}

login = function () {
    user = document.getElementById("userLogin").value;
    pwd = document.getElementById("pwdLogin").value;

    fetch('http://localhost:38655/api/account/login', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username: user, password: pwd })
    }).then(function (response) {
        if (response.ok) {
            return response;
        }

        throw Error(response.statusText);
    }).then(response => response.json())
        .then(function (newToken) {

            token = newToken;

            $.connection.hub.qs = `token=${token}`;
            $.connection.hub.start().done(function () {
                console.log('connected');

                recents();

            }).fail(function (result) {
                console.log(result);
            });

        }).catch(function (error) {
            console.log('Something went wrong!');
        });
};

recents = function () {
    fetch('http://localhost:38655/api/message/get', {
        method: 'get',
        headers: {
            'Authorization': `Bearer ${token}`
        },
    }).then(function (response) {
        if (response.ok) {
            return response;
        }

        throw Error(response.statusText);
    }).then(response => response.json())
        .then(function (data) {

            data.forEach(function (m) {
                console.log(`${m.username}: ${m.body}`);
                //$("#chat").append(`${m.username}: ${m.body}\n`);

                var chatElement = document.getElementById("chat");
                var newNode = document.createTextNode('A message from: ' + sender + ' ' + message + '\n');
                chatElement.appendChild(newNode);
            });
        }).catch(function (error) {
            console.log('Something went wrong!');
            console.error(error);
        });
};

connection.on("updateUsers", function (username) {
    console.log('Connected : ' + username);

    var chatElement = document.getElementById("chat");
    var newNode = document.createTextNode('Connected : ' + username + '\n');
    chatElement.appendChild(newNode);
});

connection.on("broadcastMessage", function (message, sender) {
    console.log('A message from: ' + sender + ' ' + message);

    var chatElement = document.getElementById("chat");
    var newNode = document.createTextNode('A message from: ' + sender + ' ' + message + '\n');
    chatElement.appendChild(newNode);
});

sendMessage = function () {
    message = document.getElementById("msg").value

    connection.invoke("send", message).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("msg").value = '';
};