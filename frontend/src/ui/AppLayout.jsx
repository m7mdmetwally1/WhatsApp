import Sidebar from "./sidebar";
import Header from "./Header";
import { Outlet, useLocation } from "react-router-dom";
import Login from "../pages/Login";
import SignUp from "../pages/SignUp";

function Container({ children }) {
  return (
    <div className="row-start-2 col-start-2 bg-white dark:bg-cyan-950">
      {children}
    </div>
  );
}

function Main({ children }) {
  return (
    <div className="p-10 bg-neutral-200 h-screen grid grid-cols-[1fr_4fr] grid-rows-[1fr_5fr] grid grid-cols-[1fr_4fr] grid-rows-[1fr_5fr] dark:bg-cyan-800">
      {children}
    </div>
  );
}

function AppLayout() {
  const location = useLocation();
  const loginVisible = location.pathname.toLowerCase() == "/login";
  const signUpVisible = location.pathname.toLowerCase() == "/signup";
  const searchVisible = location.pathname !== "/NewFriend";

  if (loginVisible) {
    return <Login />;
  }

  if (signUpVisible) {
    return <SignUp />;
  }

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
