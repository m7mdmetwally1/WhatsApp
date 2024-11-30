import { VscAccount } from "react-icons/vsc";
import { useQuery } from "./../context/SearchQuery";

function Friend({ data, members, setMembers }) {
  const { query } = useQuery();

  function handleAddMember(member) {
    if (
      member &&
      !members.some(
        (existingMember) => existingMember.friendId === member.friendId
      )
    ) {
      setMembers((prev) => [...prev, member]);
    }
  }

  const finalData = query
    ? [...(data || [])]
        .filter((chat) =>
          chat.friendCustomName.toLowerCase().includes(query.toLowerCase())
        )
        .sort((a, b) => a.id - b.id)
    : [...(data || [])].sort((a, b) => a.id - b.id);

  return (
    <div className="">
      {finalData.map((friend) => (
        <div
          className="flex flex-row justify-start items-center cursor-pointer border-b p-4"
          key={friend.friendId}
          onClick={() => handleAddMember(friend)}
        >
          <div className="ml-4 mr-5">
            <VscAccount size={45} />
          </div>
          <div className="flex flex-col justify-center items-center ">
            <p className="font-bold">{friend.friendCustomName}</p>
            <div className="text-gray-600">no status yet</div>
          </div>
        </div>
      ))}
    </div>
  );
}

export default Friend;
