import ChatHeader from "./../ui/ChatHeader";
import ChatManageMessages from "./../ui/ChatManageMessages";
import { UseChatDetailsProvider } from "./../context/selectedChatDetails";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";
import ChatMessages from "./../ui/ChatMessages";
import {
  IndividualChatMessages,
  useOpenIndividualChat,
  GroupChatMessages,
} from "../features/messages/useMessages";
import { useQuery } from "@tanstack/react-query";

function IndividualChatDetails() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { selectedChat } = UseChatDetailsProvider();
  const navigate = useNavigate();

  let { data, isLoading, error } =
    selectedChat?.chatType == true
      ? IndividualChatMessages(userId, selectedChat?.chatId)
      : GroupChatMessages(userId, selectedChat?.chatId);

  useEffect(() => {
    if (!selectedChat) {
      navigate("/chats");
    }
  }, [selectedChat, navigate]);

  if (!selectedChat) {
    return null;
  }

  return (
    <div
      className="w-2/3 h-full flex flex-col"
      style={{
        backgroundImage: `url('/ChatbackImage.jpg')`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      <div>
        <ChatHeader chatDetails={selectedChat} />
      </div>

      <ChatMessages Messages={data?.data} isLoading={isLoading} />

      <div>
        <ChatManageMessages chatDetails={selectedChat} />
      </div>
    </div>
  );
}

export default IndividualChatDetails;
