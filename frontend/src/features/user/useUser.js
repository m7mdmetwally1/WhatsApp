import { getFriends, verifyRefreshTokenApi } from "../../services/apiUser";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { createUserApi, loginUserApi } from "../../services/apiUser";
import { useNavigate } from "react-router-dom";

export function useFriends() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { data, isLoading, error } = useQuery({
    queryKey: ["friends"],
    queryFn: () => getFriends(userId),
  });

  return { data, isLoading, error };
}

export function useCreateUser(firstName, lastName, password, phoneNumber) {
  const queryClient = useQueryClient();

  const { isLoading: isCreating, mutate: createUser } = useMutation({
    mutationFn: ({ firstName, lastName, password, phoneNumber }) =>
      createUserApi(firstName, lastName, password, phoneNumber),
    onSuccess: (data) => {
      console.log(data);
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      console.error("Error creating user :", error);

      alert("Failed to create user . Please try again later.");
    },
  });

  return { isCreating, createUser };
}

export function useLoginUser() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const { isPending: isLogging, mutate: loginUser } = useMutation({
    mutationFn: ({ phoneNumber, password }) =>
      loginUserApi(phoneNumber, password),
    onSuccess: (data) => {
      queryClient.setQueryData(["userId"], data.userId);
      queryClient.setQueryData(["token"], data.token);
      localStorage.setItem("refreshToken", data.refreshToken);
      navigate("/Chats");
    },
    onError: (error) => {
      console.error("Error loging user :", error);

      alert("Failed to login user . Please try again later.");
    },
  });

  return { isLogging, loginUser };
}

export function useVerifyRefreshToken() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();

  const { isPending: isLoading, mutate: verifyRefreshToken } = useMutation({
    mutationFn: ({ refreshToken }) => verifyRefreshTokenApi(refreshToken),

    onSuccess: (data) => {
      queryClient.setQueryData(["userId"], data.userId);
      queryClient.setQueryData(["token"], data.token);
      localStorage.setItem("refreshToken", data.refreshToken);
      navigate("/Chats");
    },
    onError: (error, variables) => {
      const { refreshToken } = variables;
      navigate("/login");
      console.log("Refresh token:", refreshToken);
    },
  });

  return { isLoading, verifyRefreshToken };
}
