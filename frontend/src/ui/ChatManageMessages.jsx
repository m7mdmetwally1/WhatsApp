import { MdKeyboardVoice } from "react-icons/md";
import { MdOutlineEmojiEmotions } from "react-icons/md";
import { useState } from "react";
import {
  useInsertIndividualChatMessage,
  useInsertGroupChatMessage,
  useOpenIndividualChat,
  useOpenGroupChat,
} from "../features/messages/useMessages";
import { useQuery } from "@tanstack/react-query";

function ChatManageMessages({ chatDetails }) {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const [message, setMessage] = useState("");
  const [isFocused, setIsFocused] = useState(false);
  const { inserting, insertMessage } = useInsertIndividualChatMessage();
  const { GroupMessageInserting, insertGroupMessage } =
    useInsertGroupChatMessage();
  const { isOpening, openChat } = useOpenIndividualChat();
  const { isOpeningGroupChat, openChatGroup } = useOpenGroupChat();

  const handleFocus = () => {
    console.log(isFocused);
    setIsFocused(true);
    console.log(isFocused);
    if (isFocused) {
      if (!chatDetails.chatType) {
        console.log("inside is focused and group chat ");

        openChatGroup({ userId: userId, chatId: chatDetails.chatId });
      }
      if (chatDetails.chatType) {
        console.log("inside is focused and open chat");

        openChat({ userId: userId, chatId: chatDetails.chatId });
      }
    }
  };

  const handleBlur = () => {
    setIsFocused(false);
    console.log(isFocused);
  };

  const handleKeyPress = (event) => {
    if (event.key === "Enter" && message.trim() !== "") {
      if (chatDetails.chatType) {
        insertMessage({
          userId: userId,
          chatId: chatDetails?.chatId,
          content: message,
        });
      }
      if (!chatDetails.chatType) {
        insertGroupMessage({
          userId: userId,
          chatId: chatDetails?.chatId,
          content: message,
        });
      }

      setMessage("");
    }
  };

  return (
    <div className="flex flex-row justify-between items-start pl-5 p-2 ">
      <MdKeyboardVoice
        size={25}
        className="text-white border rounded-full bg-green-600 mr-2"
      />

      <input
        type="text"
        placeholder="send message"
        className="outline-none mr-2 w-full rounded-full pl-3 pt-1 pb-1"
        value={message}
        onChange={(e) => setMessage(e.target.value)}
        onFocus={handleFocus}
        onBlur={handleBlur}
        onKeyDown={handleKeyPress}
      />
      <MdOutlineEmojiEmotions size={25} />
    </div>
  );
}

export default ChatManageMessages;
