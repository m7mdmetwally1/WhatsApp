import axios from "axios";

export const axiosInstance = axios.create({
  baseURL: "http://localhost:5233/api/",
});

export const setupAxiosInterceptors = (queryClient) => {
  axiosInstance.interceptors.request.use(
    (config) => {
      const token = queryClient.getQueryData(["token"]);
      if (token) {
        config.headers.Authorization = `Bearer ${token}`;
      }
      return config;
    },
    (error) => Promise.reject(error)
  );

  axiosInstance.interceptors.response.use(
    (response) => response,
    async (error) => {
      const originalRequest = error.config;
      if (error.response.status === 401 && !originalRequest._retry) {
        originalRequest._retry = true;

        const refreshToken = localStorage.getItem("refreshToken");
        if (refreshToken) {
          const refreshToken = localStorage.getItem("refreshToken");
          try {
            const response = await axios.post(
              `http://localhost:5233/api/Authentication/VerifyRefreshToken?refreshToken=${refreshToken}`,
              {},
              {
                headers: { Authorization: `Bearer ${refreshToken}` },
              }
            );

            console.log(response);

            const responseRefreshToken = response.data.refreshToken;

            const newAccessToken = response.data.token;

            queryClient.setQueryData(["token"], newAccessToken);
            localStorage.setItem("refreshToken", responseRefreshToken);

            originalRequest.headers.Authorization = `Bearer ${newAccessToken}`;
            return axiosInstance(originalRequest);
          } catch (error) {
            queryClient.removeQueries(["userId"]);
            queryClient.removeQueries(["token"]);
            localStorage.removeItem("refreshToken");
            window.location("/login");
            console.log(error);
            console.log("Failed to refresh token");
          }
        }
      }
      return Promise.reject(error);
    }
  );
};
