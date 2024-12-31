import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";

const connection = new HubConnectionBuilder()
  .withUrl("http://localhost:5233/chatHub")
  .configureLogging(LogLevel.Information)
  .build();

connection
  .start()
  .then(() => {
    console.log("SignalR Connected");
  })
  .catch((err) => console.error("SignalR Connection Error:", err));

connection.on("ReceiveMessage", (chatId, userId, content) => {});

export default connection;
