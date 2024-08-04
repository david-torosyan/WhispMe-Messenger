
const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7001/chathub")
    .build();

connection.on("ReceiveMessage", (message) => {
    const msg = `User says ${message}`;
    const li = document.createElement("li");
    li.textContent = msg;
    document.getElementById("messagesList").appendChild(li);
});

connection.start().then(function () {
    aler("Connection started");
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("messageForm").addEventListener("submit", async (event) => {
    event.preventDefault();
    const message = document.getElementById("message").value;

    // Make an API call to PushMessage endpoint
    try {
        const response = await fetch(`https://localhost:7001/api/Messages/PushMessage?message=${encodeURIComponent(message)}`, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json'
            }
        });

        if (response.ok) {
            console.log("Message sent successfully");
        } else {
            console.error("Failed to send message");
        }
    } catch (error) {
        console.error("Error:", error);
    }

    document.getElementById("message").value = '';
});
