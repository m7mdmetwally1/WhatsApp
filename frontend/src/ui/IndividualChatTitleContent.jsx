import { VscAccount } from "react-icons/vsc";
import { useNavigate } from "react-router-dom";
import { UseChatDetailsProvider } from "./../context/selectedChatDetails";
import { formatTime } from "./../utils/helper";

function IndividualChatTitleContent({ data }) {
  const { setSelectedChat } = UseChatDetailsProvider();

  const navigate = useNavigate();

  function handleSelection(chat) {
    if (chat !== null) {
      setSelectedChat(chat);
      navigate("chat-details");
    }
  }

  return (
    <div>
      {data?.map((chat) => (
        <div
          className="w-full cursor-pointer"
          onClick={() => handleSelection(chat)}
          key={chat.id}
        >
          <div
            key={chat.id}
            className="flex flex-row justify-between w-full pb-4 items-start"
          >
            <div className="w-1/5 pr-2 pb-2">
              <img
                src={chat.imageUrl}
                alt="User image"
                className="h-12 w-12 rounded-full"
              />
            </div>
            <div className="flex flex-col justify-center pl-2 pr-3 w-3/5">
              <div className="font-bold">{chat.customName || chat.number}</div>
              <div className="text-gray-600 truncate max-w-full">
                {chat.lastMessage}
              </div>
            </div>
            <div className="flex flex-col w-1/5 gap-3 text-end ">
              <div className="text-green-600 text-xs">
                {chat.lastMessage && formatTime(chat.sentTime)}
              </div>
              {chat.numberOfUnSeenMessages > 0 && (
                <div className="text-xs">
                  <span className="bg-green-600 rounded-full border text-white inline-block text-xs w-5 text-center">
                    {chat.numberOfUnSeenMessages}
                  </span>
                </div>
              )}
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}

export default IndividualChatTitleContent;