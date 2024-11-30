import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useQuery } from "@tanstack/react-query";
import { useVerifyRefreshToken } from "../features/user/useUser";

function ProtectedRoute({ children }) {
  const navigate = useNavigate();
  const { data: token } = useQuery({ queryKey: ["token"] });
  const { isLoading, verifyRefreshToken } = useVerifyRefreshToken();

  const refreshToken = localStorage.getItem("refreshToken");

  if (refreshToken && !token && !isLoading) {
    verifyRefreshToken({ refreshToken });
  }

  useEffect(
    function () {
      if (!token) navigate("/login");
    },
    [token, navigate]
  );

  if (token) return children;
}

export default ProtectedRoute;
