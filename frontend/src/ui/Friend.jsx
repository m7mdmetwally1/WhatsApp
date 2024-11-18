import { VscAccount } from "react-icons/vsc";
import { useQuery } from "./../context/SearchQuery";

function Friend({ data, members, setMembers }) {
  const { query } = useQuery();

  function handleAddMember(member) {
    if (
      member &&
      !members.some((existingMember) => existingMember.id === member.id)
    ) {
      setMembers((prev) => [...prev, member]);
    }
  }

  const finalData = query
    ? [...data]
        .filter((chat) => chat.name.toLowerCase().includes(query.toLowerCase()))
        .sort((a, b) => a.id - b.id)
    : [...data].sort((a, b) => a.id - b.id);

  return (
    <div className="">
      {finalData.map((friend) => (
        <div
          className="flex flex-row justify-start items-center cursor-pointer border-b p-4"
          key={friend.id}
          onClick={() => handleAddMember(friend)}
        >
          <div className="ml-4 mr-5">
            <VscAccount size={45} />
          </div>
          <div className="flex flex-col justify-center items-center ">
            <p className="font-bold">{friend.name}</p>
            <div className="text-gray-600">{friend.status}</div>
          </div>
        </div>
      ))}
    </div>
  );
}

export default Friend;
