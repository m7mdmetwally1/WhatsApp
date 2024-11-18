import Sidebar from "./sidebar";
import Header from "./Header";
import { Outlet, useLocation } from "react-router-dom";

function Container({ children }) {
  return <div className="row-start-2 col-start-2 bg-white">{children}</div>;
}

function Main({ children }) {
  return (
    <div className="p-10 bg-neutral-200 h-screen grid grid-cols-[1fr_4fr] grid-rows-[1fr_5fr]">
      {children}
    </div>
  );
}

function AppLayout() {
  const location = useLocation();

  const searchVisible = location.pathname !== "/NewFriend";

  return (
    <Main>
      <Sidebar />
      <Header searchVisible={searchVisible} />
      <Container>
        <Outlet />
      </Container>
    </Main>
  );
}

export default AppLayout;
