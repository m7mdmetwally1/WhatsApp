import { VscAccount } from "react-icons/vsc";
import { FiDelete } from "react-icons/fi";
import { useCreateGroupChat } from "../features/chats/useChats";
import { useQuery } from "./../context/SearchQuery";
import { useForm } from "react-hook-form";

function GroupMembers({ members, setMembers }) {
  const { query } = useQuery();
  const { isCreating, createGroup } = useCreateGroupChat();
  const { register, formState, handleSubmit, reset } = useForm();

  function removeMember(member) {
    setMembers((prev) => prev.filter((m) => m !== member));
  }

  function onSubmit({ name }) {
    console.log(name);
    console.log("mohamed");
    if (members != null) {
      createGroup({ members, name });
      setMembers([]);
    }
  }

  return (
    <div className="w-1/2 border p-2 text-center dark:text-white">
      {members.length > 0 && (
        <form onSubmit={handleSubmit(onSubmit)} action="">
          <button className="bg-green-800 rounded-md p-1 text-center mb-2  text-white">
            Create now
          </button>
          <input
            type="text"
            id="name"
            name="name"
            placeholder="Group Name"
            className="h-10 ml-3 border rounded w-2/5 mb-2 outline-none p-2 dark:text-black"
            {...register("name")}
          />
        </form>
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
              <img
                src={friend.imageUrl || "/default-image.webp"}
                alt="Profile"
                className="w-10 h-10 rounded-full border border-gray-300 shadow-md"
              />
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
