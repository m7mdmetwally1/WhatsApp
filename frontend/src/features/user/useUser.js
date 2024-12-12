import { getFriends, verifyRefreshTokenApi } from "../../services/apiUser";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import { toast } from "react-hot-toast";
import {
  createUserApi,
  loginUserApi,
  uploadImageApi,
  verifyTwoFactorAuthApi,
  enableDisableTwoFactorAuthApi,
} from "../../services/apiUser";
import { useNavigate } from "react-router-dom";
import { useAssignUserData } from "../../hooks/assignUserData";
export function useFriends() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { data: token } = useQuery({ queryKey: ["token"] });
  const { data, isLoading, error } = useQuery({
    queryKey: ["friends"],
    queryFn: () => getFriends(userId, token),
  });

  return { data, isLoading, error };
}

export function useCreateUser() {
  const queryClient = useQueryClient();
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { isLoading: isCreating, mutate: createUser } = useMutation({
    mutationFn: ({ firstName, lastName, password, phoneNumber }) =>
      createUserApi(firstName, lastName, password, phoneNumber, token),
    onSuccess: (data) => {
      toast.success(" user created uccessfully");
      queryClient.invalidateQueries({ queryKey: ["chats"] });
    },
    onError: (error) => {
      const errorMessage = error.message || "Failed to create user";
      toast.error(`Error: ${errorMessage}`);
    },
  });

  return { isCreating, createUser };
}

export function useLoginUser() {
  const queryClient = useQueryClient();
  const assignUserData = useAssignUserData();
  const navigate = useNavigate();
  const { data: token } = useQuery({ queryKey: ["token"] });

  const { isPending: isLogging, mutate: loginUser } = useMutation({
    mutationFn: ({ phoneNumber, password }) =>
      loginUserApi(phoneNumber, password, token),
    onSuccess: (data) => {
      if (data.isTwoFactorEnabled) {
        assignUserData(data);
        navigate("/checkTwoFactor");
      } else {
        assignUserData(data);
        toast.success("successfull login, welcome back");
        navigate("/Chats");
      }
    },
    onError: (error) => {
      const errorMessage = error.message || "Failed to login user";
      toast.error(`Error: ${errorMessage}`);
    },
  });

  return { isLogging, loginUser };
}

export function useVerifyRefreshToken() {
  const queryClient = useQueryClient();
  const navigate = useNavigate();
  const assignUserData = useAssignUserData();

  const { isPending: isLoading, mutate: verifyRefreshToken } = useMutation({
    mutationFn: ({ refreshToken }) => verifyRefreshTokenApi(refreshToken),

    onSuccess: (data) => {
      assignUserData(data);
    },
    onError: (error, variables) => {
      const { refreshToken } = variables;

      navigate("/login");
    },
  });

  return { isLoading, verifyRefreshToken };
}

export function useUploadImage() {
  const queryClient = useQueryClient();

  const { isPending: isLoading, mutate: uploadImage } = useMutation({
    mutationFn: ({ userId, formData, token }) =>
      uploadImageApi(userId, formData, token),

    onSuccess: (data) => {
      queryClient.setQueryData(["imageUrl"], data.imageUrl);
    },
    onError: (error, variables) => {},
  });

  return { isLoading, uploadImage };
}

export function useVerifyTwoFactorAuth() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { data: token } = useQuery({ queryKey: ["token"] });
  const navigate = useNavigate();

  const { isPending: isLoading, mutate: verifyTwoFactorAuth } = useMutation({
    mutationFn: ({ code }) => verifyTwoFactorAuthApi(userId, code, token),

    onSuccess: (data) => {
      toast.success("successfull login, welcome back");
      navigate("/chats");
    },
    onError: (error, variables) => {
      const errorMessage = error.message || "Failed to login user";
      toast.error(`Error: ${errorMessage}`);
    },
  });

  return { isLoading, verifyTwoFactorAuth };
}

export function useEnableDisableTwoFactorAuth() {
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { data: token } = useQuery({ queryKey: ["token"] });
  const queryClient = useQueryClient();

  const { isPending: isLoading, mutate: EnableDisableTwoFactorAuth } =
    useMutation({
      mutationFn: () => enableDisableTwoFactorAuthApi(userId, token),

      onSuccess: (data) => {
        queryClient.setQueryData(["isTwoFactorEnabled"], data.twoFactorEnabled);
      },
      onError: (error, variables) => {},
    });

  return { isLoading, EnableDisableTwoFactorAuth };
}
