import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  getChats,
  createIndividualChat,
  createGroupChat,
} from "../../services/apiChats";

export function useChats() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { data, isLoading, error } = useQuery({
    queryKey: ["chats", userId],
    queryFn: () => getChats(userId, token),
    enabled: !!userId,
  });

  return { data, isLoading, error };
}

export function useCreateIndividualChat(userId, number, customName) {
  const queryClient = useQueryClient();

  const { isLoading: isCreating, mutate: createChat } = useMutation({
    mutationFn: ({ userId, number, customName }) =>
      createIndividualChat(userId, number, customName),
    onSuccess: (data) => {
      console.log(data);
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      console.error("Error creating chat:", error);

      alert("Failed to create chat. Please try again later.");
    },
  });

  return { isCreating, createChat };
}

export function useCreateGroupChat(members) {
  const queryClient = useQueryClient();
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { isLoading: isCreating, mutate: createGroup } = useMutation({
    mutationFn: ({ members }) => createGroupChat(members, userId),
    onSuccess: (data) => {
      console.log(data);
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      console.error("Error creating group chat:", error);

      alert("Failed to create group chat. Please try again later.");
    },
  });

  return { isCreating, createGroup };
}
