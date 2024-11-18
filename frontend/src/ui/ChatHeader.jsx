import { FaPhone } from "react-icons/fa6";
import { IoVideocam } from "react-icons/io5";
import { VscAccount } from "react-icons/vsc";

function ChatHeader({ chatDetails }) {
  return (
    <div className="flex flex-row justify-between items-center p-1">
      <div className="flex flex-row space-x-4 items-center ml-4">
        <FaPhone size={20} className="text-green-600" />
        <IoVideocam size={20} className="text-green-600" />
      </div>
      <div className="mr-4 flex flex-row space-x-4  justify-between items-center">
        <p className="font-bold">{chatDetails?.customName}</p>
        <div className="">
          <img
            src={chatDetails.imageUrl}
            alt="User image"
            className="h-12 w-12 rounded-full"
          />
        </div>
      </div>
    </div>
  );
}

export default ChatHeader;
