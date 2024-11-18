import Search from "./Search";
import Cambera from "./Camera";
import ProfilePhoto from "./ProfilePhoto";
import { useNavigate } from "react-router-dom";

function Header({ searchVisible }) {
  const navigate = useNavigate();

  return (
    <div className="col-start-2 flex flex-col bg-white">
      <div className="flex flex-row justify-between items-center">
        {searchVisible ? <Search /> : <div className="" />}
        <div className="flex flex-row justify-start items-center mr-8">
          <Cambera />
          <button
            onClick={() => navigate("/NewGroup")}
            className="border pr-5 pl-5 p-2 rounded-full"
          >
            New Group
          </button>
          <ProfilePhoto />
        </div>
      </div>
    </div>
  );
}

export default Header;
