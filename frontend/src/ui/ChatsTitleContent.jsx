import { MdOutlineAddBox } from "react-icons/md";
import IndividualChatTitleContent from "./IndividualChatTitleContent";
import { useQuery as useReactQuery } from "@tanstack/react-query";
import { useQuery } from "./../context/SearchQuery";
import { useChats } from "./../features/chats/useChats";
import { ClipLoader } from "react-spinners";
import { useEffect } from "react";
import useSignalRStore from "../store/useSignalRStore";

function ChatsTitleContent() {
  const { query } = useQuery();
  const { data: token } = useReactQuery({
    queryKey: ["token"],
  });
  let { data, isLoading, error } = useChats();
  const { startConnection } = useSignalRStore();

  useEffect(() => {
    startConnection("http://localhost:5233/chatHub", token);
  }, [startConnection, token]);

  let finalData = data?.data;

  let filteredData = finalData?.filter((item) => {
    if (!query && !item.customName) {
      return item.number;
    }

    return item.customName?.toLowerCase().includes(query?.toLowerCase());
  });

  let sortedDataDesc = filteredData?.sort(
    (a, b) => new Date(b.sentTime) - new Date(a.sentTime)
  );

  return (
    <div
      className="flex flex-col items-center border-r dark:border-cyan-800  p-2 pt-6 pb-6 w-1/3 overflow-y-scroll dark:text-white "
      style={{
        maxHeight: "70vh",
      }}
    >
      {isLoading ? (
        <ClipLoader color="#36d7b7" size={50} />
      ) : (
        <>
          <div className="flex flex-row justify-between w-full  items-center mb-10">
            <p className="text-3xl font-bold ml-4">Chats</p>
            <div className="mr-4">
              <MdOutlineAddBox size={35} />
            </div>
          </div>

          <div className="cursor-pointer w-full">
            <IndividualChatTitleContent
              data={sortedDataDesc}
              ErrorMessage={data?.errorMessage}
            />
          </div>
        </>
      )}
    </div>
  );
}

export default ChatsTitleContent;
