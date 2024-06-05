var token = '';
var connection;

register = function () {
    var user = document.getElementById("user");
    var pwd = document.getElementById("pwd");
    var confirmPwd = document.getElementById("confirmPwd");

    fetch('http://localhost:5152/api/account/register', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            username: user.value,
            password: pwd.value,
            confirmPassword: confirmPwd.value
        })
    }).then(function (response) {
        if (response.ok) {
            console.log('Successfully registered.');
            showRegisterMessage('Successfully registered.');

            user.value = '';
            pwd.value = '';
            confirmPwd.value = '';
        } else {
            console.error('Something went wrong!');
            return response.json().then(response => { throw new Error(response.error) });
        }
    }).catch(function (error) {
        console.error(error);
        showRegisterMessage(error);
    });
}

function showRegisterMessage(message) {
    var messageBox = document.getElementById('registerMessage');
    messageBox.innerHTML = message;
    messageBox.style.display = 'block';
    setTimeout(function () { messageBox.style.display = 'none'; }, 3000);
}

login = function () {
    var user = document.getElementById("userLogin");
    var pwd = document.getElementById("pwdLogin");

    fetch('http://localhost:5152/api/account/login', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ username: user.value, password: pwd.value })
    }).then(function (response) {
        if (response.ok) {
            return response;
        }
        if (response.status == 401) {
            throw new Error('User or password are invalid!');
        }

        return response.json().then(response => { throw new Error(response.error) });
    }).then(response => response.text())
        .then(function (newToken) {
            showLoginMessage('You\'re in. Messages coming in...');
            login.value = '';
            pwd.value = '';

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
            console.error('Something went wrong!');
            showLoginMessage(error);
        });
};

function showLoginMessage(message) {
    var messageBox = document.getElementById('loginMessage');
    messageBox.innerHTML = message;
    messageBox.style.display = 'block';
    setTimeout(function () { messageBox.style.display = 'none'; }, 3000);
}

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
            console.error('Something went wrong!');
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