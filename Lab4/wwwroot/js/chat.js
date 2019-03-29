"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

//Disable send button until connection is established
document.getElementById("sendButton").disabled = true;
document.getElementById("disconnectButton").disabled = false;
var connected = false;

connection.on("ReceiveMessage", function (user, message, dateTime) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " says " + msg + " at " + dateTime;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
connection.on("UniqueName", function () {
    var encodedMsg = "That user name is in use";
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("ConnectionMessage", function (user, message, dateTime) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " " + msg + " at " + dateTime;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});

connection.on("DisconnectionMessage", function (user, message, dateTime) {
    var msg = message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    var encodedMsg = user + " " + msg + " at " + dateTime;
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});
connection.on("Login", function () {
    var encodedMsg = "You must login before you can send a message";
    var li = document.createElement("li");
    li.textContent = encodedMsg;
    document.getElementById("messagesList").appendChild(li);
});




connection.start().then(function () {
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    var message = document.getElementById("messageInput").value;
    connection.invoke("SendMessage", user, message).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("connectButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    connection.invoke("OnConnect", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
document.getElementById("disconnectButton").addEventListener("click", function (event) {
    var user = document.getElementById("userInput").value;
    connection.invoke("OnDisconnect", user).catch(function (err) {
        return console.error(err.toString());
    });
    event.preventDefault();
});
