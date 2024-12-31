import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  getChats,
  createIndividualChat,
  createGroupChat,
} from "../../services/apiChats";
import { toast } from "react-hot-toast";
import { useNavigate } from "react-router-dom";

export function useChats() {
  const queryClient = useQueryClient();
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { data, isLoading, error } = useQuery({
    queryKey: ["chats", userId, token],
    queryFn: () => getChats(userId, queryClient),
    enabled: !!userId,
  });

  return { data, isLoading, error };
}

export function useCreateIndividualChat() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const { isLoading: isCreating, mutate: createChat } = useMutation({
    mutationFn: ({ userId, number, customName }) =>
      createIndividualChat(userId, number, customName, queryClient),
    onSuccess: (data) => {
      toast.success(" chat created successfully");
      queryClient.invalidateQueries({ queryKey: ["chats"] });
      navigate("/chats");
    },
    onError: (error) => {
      toast.error(`Error:${error}`);
    },
  });

  return { isCreating, createChat };
}

export function useCreateGroupChat() {
  const queryClient = useQueryClient();
  const { data: userId } = useQuery({ queryKey: ["userId"] });

  const { isLoading: isCreating, mutate: createGroup } = useMutation({
    mutationFn: ({ members, name }) =>
      createGroupChat(members, userId, queryClient, name),
    onSuccess: (data) => {
      toast.success(" group chat created successfully");
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      toast.error(`Error:${error}`);
    },
  });

  return { isCreating, createGroup };
}
