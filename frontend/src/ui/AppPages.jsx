import { IoLogoWhatsapp } from "react-icons/io";
import { IoHomeOutline } from "react-icons/io5";
import { FaUserFriends } from "react-icons/fa";
import { CiSettings } from "react-icons/ci";
import { useNavigate } from "react-router-dom";

function AppPages() {
  const navigate = useNavigate();

  return (
    <div className="my-8">
      <div className="text-lg text-white flex mb-20  justify-start items-center ">
        <span className="ml-6 text-3xl text-green-400 ">
          <IoLogoWhatsapp />
        </span>
        <span className="ml-6 "> Whats App</span>
      </div>
      <div className="text-lg text-gray-400 ">
        <div className=" flex mb-8  justify-start items-center hover:text-white cursor-pointer">
          <span className="ml-6 text-3xl ">
            <IoHomeOutline />
          </span>

          <span onClick={() => navigate("/Chats")} className="ml-6 ">
            {" "}
            Home{" "}
          </span>
        </div>
        <div className="flex mb-8  justify-start  items-center hover:text-white cursor-pointer">
          <span className="ml-6  text-3xl ">
            <FaUserFriends />
          </span>
          <span onClick={() => navigate("/NewFriend")} className="ml-6">
            {" "}
            Add Contact{" "}
          </span>
        </div>
        <div className="flex mb-8   justify-start items-center hover:text-white cursor-pointer">
          <span className="ml-6 text-3xl ">
            <CiSettings />
          </span>
          <span onClick={() => navigate("/settings")} className="ml-6">
            {" "}
            Settings{" "}
          </span>
        </div>
      </div>
    </div>
  );
}

export default AppPages;
