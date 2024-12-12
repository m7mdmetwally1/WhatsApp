import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  getMessages,
  getGroupMessages,
  openIndividualChat,
  insertIndividualChatMessage,
  insertGroupChatMessage,
  openGroupChat,
} from "../../services/apiMessages";

export function IndividualChatMessages(userId, chatId) {
  const { data: token } = useQuery({ queryKey: ["token"] });
  const { data, isLoading, error } = useQuery({
    queryKey: ["Messages", userId, chatId],
    queryFn: () => getMessages(userId, chatId, token),
    enabled: !!userId && !!chatId,
  });

  return { data, isLoading, error };
}

export function GroupChatMessages(userId, chatId) {
  const { data: token } = useQuery({ queryKey: ["token"] });
  const { data, isLoading, error } = useQuery({
    queryKey: ["Messages", chatId],
    queryFn: () => getGroupMessages(userId, chatId, token),
    enabled: !!userId && !!chatId,
  });

  return { data, isLoading, error };
}

export function useOpenIndividualChat(userId, chatId) {
  const queryClient = useQueryClient();
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { isLoading: isOpening, mutate: openChat } = useMutation({
    mutationFn: ({ userId, chatId }) =>
      openIndividualChat(userId, chatId, token),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["Messages"] });
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      alert("Failed to open chat. Please try again later.");
    },
  });

  return { isOpening, openChat };
}

export function useOpenGroupChat() {
  const queryClient = useQueryClient();
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { isLoading: isOpening, mutate: openChatGroup } = useMutation({
    mutationFn: ({ userId, chatId }) => openGroupChat(userId, chatId, token),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["Messages"] });
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      alert("Failed to open chat. Please try again later.");
    },
  });

  return { isOpening, openChatGroup };
}

export function useInsertIndividualChatMessage() {
  const queryClient = useQueryClient();
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { isLoading: inserting, mutate: insertMessage } = useMutation({
    mutationFn: ({ userId, chatId, content }) =>
      insertIndividualChatMessage(userId, chatId, content, token),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["Messages"] });
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      console.error("Error inserting message :", error);

      alert("Failed to insert message. Please try again later.");
    },
  });

  return { inserting, insertMessage };
}

export function useInsertGroupChatMessage() {
  const queryClient = useQueryClient();
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { isLoading: inserting, mutate: insertGroupMessage } = useMutation({
    mutationFn: ({ userId, chatId, content }) =>
      insertGroupChatMessage(userId, chatId, content, token),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["Messages"] });
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      alert("Failed to insert message. Please try again later.");
    },
  });

  return { inserting, insertGroupMessage };
}
