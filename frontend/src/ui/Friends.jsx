import { useFriends } from "../features/user/useUser";
import Friend from "./Friend";

function Friends({ members, setMembers }) {
  let { data, isLoading, error } = useFriends();

  return (
    <div
      className="w-1/2 border p-5 overflow-y-scroll dark:text-white"
      style={{
        maxHeight: "70vh",
      }}
    >
      <Friend data={data?.data} members={members} setMembers={setMembers} />
    </div>
  );
}

export default Friends;
