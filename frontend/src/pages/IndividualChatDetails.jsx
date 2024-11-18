import ChatHeader from "./../ui/ChatHeader";
import ChatManageMessages from "./../ui/ChatManageMessages";
import { UseChatDetailsProvider } from "./../context/selectedChatDetails";
import { useNavigate } from "react-router-dom";
import { useEffect } from "react";

function IndividualChatDetails() {
  const { selectedChat } = UseChatDetailsProvider();
  const navigate = useNavigate();

  useEffect(() => {
    if (!selectedChat) {
      navigate("/chats");
    }
  }, [selectedChat, navigate]);

  if (!selectedChat) {
    return null;
  }

  return (
    <div className="w-2/3">
      <ChatHeader chatDetails={selectedChat} />
      <img src="/ChatbackImage.jpg" alt="" />
      <ChatManageMessages />
    </div>
  );
}

export default IndividualChatDetails;
