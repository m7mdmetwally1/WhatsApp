import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import {
  getChats,
  createIndividualChat,
  createGroupChat,
} from "../../services/apiChats";
import { toast } from "react-hot-toast";
import { useNavigate } from "react-router-dom";

export function useChats() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { data, isLoading, error, success } = useQuery({
    queryKey: ["chats", userId],
    queryFn: () => getChats(userId, token),
    enabled: !!userId,
  });

  return { data, isLoading, error, success };
}

export function useCreateIndividualChat() {
  const queryClient = useQueryClient();
  const { data: token } = useQuery({ queryKey: ["token"] });
  const navigate = useNavigate();

  const { isLoading: isCreating, mutate: createChat } = useMutation({
    mutationFn: ({ userId, number, customName }) =>
      createIndividualChat(userId, number, customName, token),
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
  const { data: token } = useQuery({ queryKey: ["token"] });
  const { isLoading: isCreating, mutate: createGroup } = useMutation({
    mutationFn: ({ members, name }) =>
      createGroupChat(members, userId, token, name),
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
