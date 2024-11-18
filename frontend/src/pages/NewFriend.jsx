function NewFriend() {
  return (
    <div className="flex items-center justify-center w-full h-full">
      <div className=" text-center   bg-neutral-200 w-1/2 min-h-32 p-4">
        <p className="pt-5 font-bold text-3xl m-4">Start New Chat</p>
        <p className="mb-4">Intialize a conversation with a new contact</p>
        <input
          type="number"
          placeholder="WhatsApp Number"
          className="h-12 border rounded w-2/3 mb-8 outline-none p-2"
        />
        <button className="bg-green-300 w-60 h-12 text-center">Chat Now</button>
      </div>
    </div>
  );
}

export default NewFriend;
