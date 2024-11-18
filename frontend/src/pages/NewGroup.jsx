import { useState } from "react";
import Friends from "./../ui/Friends";
import GroupMembers from "./../ui/GroupMembers";

function NewGroup() {
  const [members, setMembers] = useState([]);

  return (
    <div className="flex flex-row items-start border ">
      <Friends members={members} setMembers={setMembers} />
      <GroupMembers members={members} setMembers={setMembers} />
    </div>
  );
}

export default NewGroup;
