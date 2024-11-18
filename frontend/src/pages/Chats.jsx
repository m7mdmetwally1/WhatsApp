import ChatsTitleContent from "../ui/ChatsTitleContent";
import { Outlet } from "react-router-dom";

function Chats() {
  return (
    <div className="flex items-start flex-row border w-full h-full">
      <ChatsTitleContent />
      <Outlet />
    </div>
  );
}

export default Chats;
