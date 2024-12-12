import { useQueryClient } from "@tanstack/react-query";

export const useAssignUserData = () => {
  const queryClient = useQueryClient();

  const assignUserData = (data) => {
    if (data) {
      queryClient.setQueryData(["userId"], data.userId);
      queryClient.setQueryData(["imageUrl"], data.imageUrl);
      queryClient.setQueryData(["token"], data.token);
      queryClient.setQueryData(["isTwoFactorEnabled"], data.isTwoFactorEnabled);

      localStorage.setItem("refreshToken", data?.refreshToken);
    }
  };

  return assignUserData;
};
