import { useQuery } from "@tanstack/react-query";
import { useEffect } from "react";
import { useNavigate } from "react-router-dom";

function ProtectCheckAuth({ children }) {
  const navigate = useNavigate();
  const { data: twoFactorAuth } = useQuery({
    queryKey: ["isTwoFactorEnabled"],
  });

  useEffect(
    function () {
      if (!twoFactorAuth) navigate("/login");
    },
    [twoFactorAuth, navigate]
  );

  if (twoFactorAuth) return children;
}

export default ProtectCheckAuth;
