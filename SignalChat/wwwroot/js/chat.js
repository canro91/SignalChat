var token = '';
var connection;

register = function () {
    user = document.getElementById("user").value;
    pwd = document.getElementById("pwd").value;
    confirmPwd = document.getElementById("confirmPwd").value;

    fetch('http://localhost:5152/api/account/register', {
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
            return response.json().then(response => { throw new Error(JSON.stringify(response)) });
        }
    }).catch(function (error) {
        console.log(error);
    });
}

login = function () {
    user = document.getElementById("userLogin").value;
    pwd = document.getElementById("pwdLogin").value;

    fetch('http://localhost:5152/api/account/login', {
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
    }).then(response => response.text())
        .then(function (newToken) {

            token = newToken;

            connection = new signalR.HubConnectionBuilder()
                .withUrl("/chathub", { accessTokenFactory: () => token })
                .configureLogging(signalR.LogLevel.Information)
                .build();

            connection.start().then(function () {
                console.log('Successfully connected.');

                recents();
            }).catch(function (err) {
                return console.error(err.toString());
            });

            connection.on("updateUsers", function (username) {
                console.log('Connected : ' + username);

                var chatElement = document.getElementById("chat");
                var newNode = document.createTextNode(`${username} is connected\n`);
                chatElement.appendChild(newNode);
            });

            connection.on("broadcastMessage", function (message, sender) {
                console.log('A message from: ' + sender + ' ' + message);

                var chatElement = document.getElementById("chat");
                var newNode = document.createTextNode(`${sender}: ${message}\n`);
                chatElement.appendChild(newNode);
            });

        }).catch(function (error) {
            console.log('Something went wrong!');
            return console.error(error.toString());
        });
};

recents = function () {
    fetch('http://localhost:5152/api/message', {
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

                var chatElement = document.getElementById("chat");
                var newNode = document.createTextNode(`${m.username}: ${m.body}\n`);
                chatElement.appendChild(newNode);
            });
        }).catch(function (error) {
            console.log('Something went wrong!');
            console.error(error);
        });
};


sendMessage = function () {
    message = document.getElementById("msg").value

    connection.invoke("send", message).catch(function (err) {
        return console.error(err.toString());
    });

    document.getElementById("msg").value = '';
};