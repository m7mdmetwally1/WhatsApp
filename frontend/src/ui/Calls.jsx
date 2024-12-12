import { VscAccount } from "react-icons/vsc";
import { MdOutlineAddBox } from "react-icons/md";
import { FaPhone } from "react-icons/fa6";
import { IoVideocam } from "react-icons/io5";

function Calls() {
  return (
    <div className="flex flex-col items-center p-2 pt-6 pb-6 w-1/2 dark:text-white">
      <div className="flex flex-row justify-between w-full  items-center mb-10">
        <p className="text-3xl font-bold ml-4">Calls</p>
        <div className="mr-4">
          <MdOutlineAddBox size={35} />
        </div>
      </div>

      <div className="flex flex-row justify-between w-full items-center">
        <div className="flex flex-row justify-between items-center">
          <div className="ml-4 border rounded-full border-2">
            <VscAccount size={45} />
          </div>
          <div className="flex flex-col justify-start items-start ml-4">
            <div className="font-bold ">Ahmed Waled</div>
            <div className="text-gray-600"> Yesterday</div>
          </div>
        </div>
        <div className="flex flex-row space-x-4 items-center mr-4">
          <FaPhone size={20} className="text-green-600" />
          <IoVideocam size={20} className="text-green-600" />
        </div>
      </div>
    </div>
  );
}

export default Calls;
