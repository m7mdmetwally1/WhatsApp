import Search from "./Search";
import Cambera from "./Camera";
import ProfilePhoto from "./ProfilePhoto";
import { useNavigate } from "react-router-dom";
import { HiOutlineMoon, HiOutlineSun } from "react-icons/hi2";
import { useDarkMode } from "../context/DarkMode";

function Header({ searchVisible }) {
  const navigate = useNavigate();
  const { isDarkMode, toggleDarkMode } = useDarkMode();

  return (
    <div className="col-start-2 flex flex-col bg-white dark:bg-cyan-950">
      <div className="flex flex-row justify-between items-center">
        {searchVisible ? <Search /> : <div className="" />}
        <div className="flex flex-row justify-start items-center mr-8">
          <Cambera />
          <button
            onClick={() => navigate("/NewGroup")}
            className="border pr-5 pl-5 p-2 rounded-full dark:text-white"
          >
            New Group
          </button>

          <ProfilePhoto />
          <div
            onClick={toggleDarkMode}
            className="ml-3 cursor-pointer dark:text-white"
          >
            {isDarkMode ? (
              <HiOutlineSun size={30} />
            ) : (
              <HiOutlineMoon size={30} />
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default Header;
