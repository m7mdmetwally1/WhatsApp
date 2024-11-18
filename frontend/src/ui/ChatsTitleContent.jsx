import { MdOutlineAddBox } from "react-icons/md";
import IndividualChatTitleContent from "./IndividualChatTitleContent";
import { useQuery } from "./../context/SearchQuery";
import { useChats } from "./../features/chats/useChats";

function ChatsTitleContent() {
  const { query } = useQuery();

  let { data, isLoading, error } = useChats();

  console.log(data?.data);

  let finalData = data?.data;
  let sortedDataDesc = finalData?.sort(
    (a, b) => new Date(b.sentTime) - new Date(a.sentTime)
  );

  return (
    <div
      className="flex flex-col items-center border-r p-2 pt-6 pb-6 w-1/3 overflow-y-scroll "
      style={{
        maxHeight: "70vh",
      }}
    >
      <div className="flex flex-row justify-between w-full  items-center mb-10">
        <p className="text-3xl font-bold ml-4">Chats</p>
        <div className="mr-4">
          <MdOutlineAddBox size={35} />
        </div>
      </div>

      <div className="cursor-pointer w-full">
        <IndividualChatTitleContent data={sortedDataDesc} />
      </div>
    </div>
  );
}

export default ChatsTitleContent;
