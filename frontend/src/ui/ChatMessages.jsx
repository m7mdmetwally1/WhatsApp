import { useQuery } from "@tanstack/react-query";
import { useRef, useEffect } from "react";
import { ClipLoader } from "react-spinners";

function ChatMessages({ Messages, isLoading }) {
  const { data: userId } = useQuery({ queryKey: ["userId"] });

  const sortedMessages = [...(Messages || [])].sort(
    (a, b) => new Date(a.sentAt) - new Date(b.sentAt)
  );

  const chatEndRef = useRef(null);

  useEffect(() => {
    if (chatEndRef.current) {
      chatEndRef.current.scrollIntoView({ behavior: "smooth" });
    }
  }, [sortedMessages]);

  return (
    <div
      className="flex flex-col flex-grow overflow-y-scroll"
      style={{
        height: "60vh",
      }}
    >
      {isLoading ? (
        <div className="flex justify-center flex-row items-center">
          {" "}
          <ClipLoader color="#36d7b7" size={50} />
        </div>
      ) : (
        sortedMessages.map((message, index) => (
          <div key={index} className="flex flex-col pt-2">
            <div
              ref={chatEndRef}
              className={`${
                message.senderId === userId
                  ? "self-start ml-3 bg-green-500 text-white"
                  : "self-end mr-3 bg-white text-black"
              } p-2 rounded-lg`}
            >
              {message.content}
            </div>
          </div>
        ))
      )}
    </div>
  );
}

export default ChatMessages;
