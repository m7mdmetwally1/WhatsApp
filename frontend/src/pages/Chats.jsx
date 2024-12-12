import ChatsTitleContent from "../ui/ChatsTitleContent";
import { Outlet } from "react-router-dom";

function Chats() {
  return (
    <div className="flex items-start flex-row border-t dark:border-cyan-800 w-full h-full dark:bg-cyan-950">
      <ChatsTitleContent />
      <Outlet />
    </div>
  );
}

export default Chats;
