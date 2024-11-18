import { VscAccount } from "react-icons/vsc";
import { FiDelete } from "react-icons/fi";

function GroupMembers({ members, setMembers }) {
  function removeMember(member) {
    setMembers((prev) => prev.filter((m) => m !== member));
  }

  return (
    <div className="w-1/2 border p-2 text-center ">
      {members.length > 0 && (
        <button className="bg-green-800 rounded-md p-1 text-center mb-2  text-white">
          Create now
        </button>
      )}
      <div
        className=" overflow-y-scroll "
        style={{
          maxHeight: "65vh",
        }}
      >
        {members.map((friend) => (
          <div
            className="flex flex-row  justify-between items-center cursor-pointer border-b p-3"
            key={friend.id}
          >
            <div className="ml-4 mr-5">
              <VscAccount size={45} />
            </div>
            <div className="flex flex-col justify-center items-center ">
              <p className="font-bold">{friend.name}</p>
            </div>
            <div className="ml-4 ">
              <FiDelete size={25} onClick={() => removeMember(friend)} />
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}

export default GroupMembers;
