import { MdKeyboardVoice } from "react-icons/md";
import { MdOutlineEmojiEmotions } from "react-icons/md";

function ChatManageMessages() {
  return (
    <div className="flex flex-row justify-between items-center pl-5 p-2">
      <MdKeyboardVoice
        size={25}
        className="text-white border rounded-full bg-green-600"
      />
      ;
      <input
        type="text"
        placeholder="send message"
        className="outline-none mr-6"
      />
      <MdOutlineEmojiEmotions size={25} />
    </div>
  );
}

export default ChatManageMessages;
