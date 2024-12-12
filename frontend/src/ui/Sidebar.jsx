import AppPages from "./AppPages";
import UpdateApp from "./UpdateApp";

function Sidebar() {
  return (
    <div className="row-start-1 text-center row-span-2 h-full bg-emerald-700 dark:bg-cyan-950">
      <AppPages />
      <UpdateApp />
    </div>
  );
}

export default Sidebar;
