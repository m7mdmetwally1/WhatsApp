import { VscAccount } from "react-icons/vsc";
import { FiDelete } from "react-icons/fi";
import { useCreateGroupChat } from "../features/chats/useChats";
import { useQuery } from "./../context/SearchQuery";

function GroupMembers({ members, setMembers }) {
  const { query } = useQuery();
  const { isCreating, createGroup } = useCreateGroupChat();

  function removeMember(member) {
    setMembers((prev) => prev.filter((m) => m !== member));
  }

  function handleCreateGroup() {
    console.log("working");
    console.log(members);
    if (members != null) {
      createGroup({ members: members });
      setMembers([]);
    }
  }

  return (
    <div className="w-1/2 border p-2 text-center ">
      {members.length > 0 && (
        <button
          onClick={handleCreateGroup}
          className="bg-green-800 rounded-md p-1 text-center mb-2  text-white"
        >
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
            key={friend.friendId}
          >
            <div className="ml-4 mr-5">
              <VscAccount size={45} />
            </div>
            <div className="flex flex-col justify-center items-center ">
              <p className="font-bold">{friend.friendCustomName}</p>
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
