const connection = new signalR.HubConnectionBuilder()
    .withUrl("https://localhost:7001/chathub")
    .build();

connection.on("ReceiveMessage", (user, message) => {
    const msg = `${user} says ${message}`;

    const div = document.createElement("div");
    div.className = "message text-only";
    const div1 = document.createElement("div");
    div1.className = "response";
    const p = document.createElement("p");
    p.className = "text";
    p.textContent = msg;

    div1.appendChild(p);
    div.appendChild(div1);

    const parrentDiv = document.getElementById("messages-chat");
    parrentDiv.appendChild(div);
});

connection.start().then(function () {
    console.log("Connection started");
}).catch(function (err) {
    return console.error(err.toString());
});

// Function to get cookie by name
function getCookie(name) {
    const value = `; ${document.cookie}`;
    const parts = value.split(`; ${name}=`);
    if (parts.length === 2) return parts.pop().split(';').shift();
    return null; // Ensure it returns null if the cookie is not found
}

document.getElementById("messageButton").addEventListener("click", async (event) => {
    event.preventDefault();
    const username = getCookie("userFullName");
    const userEmail = getCookie("userEmail");

    const messageDto = {
        content: document.getElementById("message").value,
        senderEmail: userEmail,
        senderFullName: username,
        room: document.getElementById("header-chat").getAttribute("tag")
    };

    // Make an API call to PushMessage endpoint
    try {
        const response = fetch(`https://localhost:7001/api/Messages`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(messageDto),
        });

        if (response.ok) {
            console.log("Message sent successfully");
        } else {
            console.error("Failed to send message");
        }
    } catch (error) {
        console.error("Error:", error);

        document.getElementById("message").value = '';
    };
});

document.addEventListener("DOMContentLoaded", () => {
    console.log("DOM fully loaded and parsed");

    const discussions = document.getElementsByClassName("disc");

    Array.from(discussions).forEach(discussion => {
        discussion.addEventListener("click", async (event) => {
            const roomName = event.currentTarget.getAttribute("tag");

            try {
                const response = await fetch(`https://localhost:7001/api/Messages/Room/${roomName}`, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                });

                if (response.ok) {
                    console.log("Messages loaded successfully");

                    const messages = await response.json();
                    const chat = document.getElementById("messages-chat");

                    // Clear existing messages if needed
                    chat.innerHTML = '';

                    messages.forEach(message => {
                        console.log(getCookie("userEmail"));
                        console.log(getCookie("userFullName"));
                        if (message.senderEmail === getCookie("userEmail")) {
                            chat.appendChild(createResoinseElement(message));
                            chat.appendChild(createResponseTimeElement());
                        }
                        else {
                            chat.appendChild(createMessageElement(message));
                            chat.appendChild(createTimeElement());
                        }
                    });
                } else {
                    console.error("Failed to load messages");
                }
            } catch (error) {
                console.error("Error:", error);
            }
        });
    });
});

function createMessageElement(message) {
    const div = document.createElement("div");
    div.className = "message";

    const p = document.createElement("p");
    p.className = "text";
    p.textContent = message.content;

    div.appendChild(p);
    return div;
}

function createResoinseElement(message) {
    const div = document.createElement("div");
    div.className = "response";

    const p = document.createElement("p");
    p.className = "text";
    p.textContent = message.content;

    div.appendChild(p);
    return div;
}

function createTimeElement() {
    const p1 = document.createElement("p");
    p1.className = "time";
    p1.textContent = "12:00";

    return p1;
}

function createResponseTimeElement() {
    const p1 = document.createElement("p");
    p1.className = "response-time time";
    p1.textContent = "12:00";

    return p1;
}