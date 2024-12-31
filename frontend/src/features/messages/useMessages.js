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
  const queryClient = useQueryClient();

  const { data, isLoading, error } = useQuery({
    queryKey: ["Messages", userId, chatId],
    queryFn: () => getMessages(userId, chatId, queryClient),
    enabled: !!userId && !!chatId,
  });

  return { data, isLoading, error };
}

export function GroupChatMessages(userId, chatId) {
  const queryClient = useQueryClient();

  const { data, isLoading, error } = useQuery({
    queryKey: ["Messages", chatId],
    queryFn: () => getGroupMessages(userId, chatId, queryClient),
    enabled: !!userId && !!chatId,
  });

  return { data, isLoading, error };
}

export function useOpenIndividualChat(userId, chatId) {
  const queryClient = useQueryClient();

  const { isLoading: isOpening, mutate: openChat } = useMutation({
    mutationFn: ({ userId, chatId }) =>
      openIndividualChat(userId, chatId, queryClient),
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

  const { isLoading: isOpening, mutate: openChatGroup } = useMutation({
    mutationFn: ({ userId, chatId }) =>
      openGroupChat(userId, chatId, queryClient),
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

  const { isLoading: inserting, mutate: insertMessage } = useMutation({
    mutationFn: ({ userId, chatId, content }) =>
      insertIndividualChatMessage(userId, chatId, content, queryClient),
    onSuccess: () => {
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

  const { isLoading: inserting, mutate: insertGroupMessage } = useMutation({
    mutationFn: ({ userId, chatId, content }) =>
      insertGroupChatMessage(userId, chatId, content, queryClient),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      alert("Failed to insert message. Please try again later.");
    },
  });

  return { inserting, insertGroupMessage };
}
