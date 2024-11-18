import Status from "./../ui/Status";
import Calls from "./../ui/Calls";

function ChatFeatures() {
  return (
    <div className="w-2/3 justify-between items-center flex flex-row">
      <Status />
      <Calls />
    </div>
  );
}

export default ChatFeatures;
