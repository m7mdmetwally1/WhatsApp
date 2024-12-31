import { create } from "zustand";
import * as signalR from "@microsoft/signalr";

const useSignalRStore = create((set) => ({
  connection: null,
  isConnected: false,

  startConnection: async (url, token) => {
    const connection = new signalR.HubConnectionBuilder()
      .withUrl(url, {
        accessTokenFactory: () => token,
      })
      .withAutomaticReconnect()
      .build();

    connection.onclose(() => {
      set({ isConnected: false });
    });

    try {
      await connection.start();

      set({ connection, isConnected: true });
    } catch (err) {
      console.error("Error connecting to SignalR:", err);
    }
  },
}));

export default useSignalRStore;
