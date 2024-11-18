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

const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 0,
    },
  },
});

function App() {
  return (
    <SelectedChatProvider>
      <SelectedQueryProvider>
        <QueryClientProvider client={queryClient}>
          <ReactQueryDevtools initialIsOpen={false} />
          <BrowserRouter>
            <Routes>
              <Route element={<AppLayout />}>
                <Route index element={<Navigate replace to="Chats" />} />
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
                <Route path="NewGroup" element={<NewGroup />} />
                <Route path="NewFriend" element={<NewFriend />} />
                <Route path="IndividualChat" element={<NewFriend />} />
              </Route>
            </Routes>
          </BrowserRouter>
        </QueryClientProvider>
      </SelectedQueryProvider>
    </SelectedChatProvider>
  );
}

export default App;
