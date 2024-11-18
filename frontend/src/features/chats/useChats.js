import { useQuery } from "@tanstack/react-query";
import { getChats } from "../../services/apiChats";

export function useChats() {
  const { data, isLoading, error } = useQuery({
    queryKey: ["chats"],
    queryFn: getChats,
  });

  return { data, isLoading, error };
}
