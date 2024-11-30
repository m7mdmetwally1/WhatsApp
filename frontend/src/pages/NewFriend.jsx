import { useState } from "react";
import { useCreateIndividualChat } from "../features/chats/useChats";
import { useQuery } from "@tanstack/react-query";

function NewFriend() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const [number, setNumber] = useState("");
  const [customName, setCustomName] = useState("");
  let { isCreating, createChat } = useCreateIndividualChat();

  function handleSubmit(e) {
    e.preventDefault();
    if (!number || !customName) return;

    createChat({ userId: userId, number: number, customName: customName });
    setCustomName("");
    setNumber("");
  }

  return (
    <div className="flex items-center justify-center w-full h-full">
      <div className=" text-center   bg-neutral-200 w-1/2 min-h-32 p-4">
        <p className="pt-5 font-bold text-3xl m-4">Start New Chat</p>
        <p className="mb-4">Intialize a conversation with a new contact</p>
        <form onSubmit={handleSubmit}>
          <input
            type="number"
            placeholder="WhatsApp Number"
            className="h-12 border rounded w-2/3 mb-8 outline-none p-2"
            value={number}
            onChange={(e) => setNumber(e.target.value)}
          />
          <input
            type="text"
            placeholder="Custom Name"
            className="h-12 border rounded w-2/3 mb-8 outline-none p-2"
            value={customName}
            onChange={(e) => setCustomName(e.target.value)}
          />
          <button className="bg-green-300 w-60 h-12 text-center" type="submit">
            Create Now
          </button>
        </form>
      </div>
    </div>
  );
}

export default NewFriend;
