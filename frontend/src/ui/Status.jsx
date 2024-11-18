import { VscAccount } from "react-icons/vsc";
import { MdOutlineAddBox } from "react-icons/md";

function Status() {
  return (
    <div className="flex flex-col items-center border-r p-2 pt-6 pb-6 w-1/2">
      <div className="flex flex-row justify-between w-full  items-center mb-10">
        <p className="text-3xl font-bold ml-4">Status</p>
        <div className="mr-4">
          <MdOutlineAddBox size={35} />
        </div>
      </div>

      <div className="flex flex-row justify-between w-full items-center">
        <div className="flex flex-row justify-between items-center">
          <div className="ml-4 border rounded-full border-2 border-green-600">
            <VscAccount size={45} />
          </div>
          <div className="flex flex-col justify-start items-start ml-4">
            <div className="font-bold ">Ahmed Waled</div>
            <div className="text-gray-600"> 3 hours ago</div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Status;
