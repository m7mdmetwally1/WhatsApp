import { jwtDecode } from "jwt-decode";

function isTokenExpired(token) {
  if (!token) return true;

  try {
    const { exp } = jwtDecode(token);

    const currentTime = Date.now() / 1000 - 60;

    return exp < currentTime;
  } catch (error) {
    console.error("Error decoding token:", error);
    return true;
  }
}

export default isTokenExpired;
