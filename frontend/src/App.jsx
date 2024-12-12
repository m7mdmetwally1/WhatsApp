import { BrowserRouter, Routes, Route, Navigate } from "react-router-dom";
import AppLayout from "./ui/AppLayout";
import Chats from "./pages/Chats";
import NewGroup from "./pages/NewGroup";
import NewFriend from "./pages/NewFriend";
import IndividualChatDetails from "./pages/IndividualChatDetails";
import ChatFeatures from "./pages/ChatFeatures";
import { SelectedChatProvider } from "./context/selectedChatDetails";
import { SelectedQueryProvider } from "./context/SearchQuery";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { ReactQueryDevtools } from "@tanstack/react-query-devtools";
import ProtectedRoute from "./ui/ProtectedRoute";
import SignUp from "./pages/SignUp";
import Login from "./pages/Login";
import Settings from "./pages/Settings";
import CheckTwoFactor from "./pages/CheckTwoFactor";
import ProtectCheckAuth from "./ui/ProtectCheckAuth";
import { Toaster } from "react-hot-toast";
import { DarkModeProvider } from "./context/DarkMode";

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 0,
    },
  },
});

function App() {
  return (
    <DarkModeProvider>
      <SelectedChatProvider>
        <SelectedQueryProvider>
          <QueryClientProvider client={queryClient}>
            <ReactQueryDevtools initialIsOpen={false} />
            <BrowserRouter>
              <Routes>
                <Route
                  element={
                    <ProtectedRoute>
                      <AppLayout />
                    </ProtectedRoute>
                  }
                >
                  <Route path="/" element={<Navigate replace to="/Login" />} />
                  <Route path="Chats" element={<Chats />}>
                    <Route
                      path="chat-details"
                      element={<IndividualChatDetails />}
                    />
                    <Route path="chat-features" element={<ChatFeatures />} />

                    <Route
                      index
                      element={<Navigate replace to="chat-features" />}
                    />
                  </Route>
                  <Route path="settings" element={<Settings />} />
                  <Route path="NewGroup" element={<NewGroup />} />
                  <Route path="NewFriend" element={<NewFriend />} />
                  <Route path="IndividualChat" element={<NewFriend />} />
                </Route>
                <Route path="Signup" element={<SignUp />} />
                <Route index path="Login" element={<Login />} />
                <Route
                  path="checkTwoFactor"
                  element={
                    <ProtectCheckAuth>
                      <CheckTwoFactor />
                    </ProtectCheckAuth>
                  }
                />
              </Routes>
            </BrowserRouter>
          </QueryClientProvider>
          <Toaster
            position="top-center"
            gutter={12}
            containerStyle={{ margin: "8px" }}
            toastOptions={{
              success: {
                duration: 3000,
              },
              error: {
                duration: 5000,
              },
              style: {
                fontSize: "16px",
                maxWidth: "500px",
                padding: "16px 24px",
                backgroundColor: "#4CAF50",
                color: "var(--color-grey-700)",
              },
            }}
          />
        </SelectedQueryProvider>
      </SelectedChatProvider>
    </DarkModeProvider>
  );
}

export default App;
