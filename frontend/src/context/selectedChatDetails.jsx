import { createContext, useState, useContext } from "react";

const SelectedChatContext = createContext();

function UseChatDetailsProvider() {
  return useContext(SelectedChatContext);
}

function SelectedChatProvider({ children }) {
  const [selectedChat, setSelectedChat] = useState(null);

  return (
    <SelectedChatContext.Provider value={{ selectedChat, setSelectedChat }}>
      {children}
    </SelectedChatContext.Provider>
  );
}

export { SelectedChatProvider, UseChatDetailsProvider };
