import { setupAxiosInterceptors, axiosInstance } from "../utils/axiosInstance";

export async function getFriends(userId, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .get(`User/my-friends?userId=${userId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });
  // const response = await fetch(
  //   `http://localhost:5233/my-friends?userId=${userId}`,
  //   {
  //     method: "POST",
  //     headers: {
  //       Authorization: `Bearer ${token}`,
  //     },
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // if (!response.ok) {
  //   throw new Error("Failed get friends data");
  // }

  // return response.json();
}

export async function createUserApi(
  firstName,
  lastName,
  password,
  phoneNumber
) {
  const response = await fetch(`http://localhost:5233/api/Authentication`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({
      phoneNumber: phoneNumber,
      firstName: firstName,
      lastName: lastName,
      password: password,
    }),
  });

  if (response.status === 401) {
    console.error("Token is invalid. Please login again.");
    return;
  }

  const resonseData = await response.json();

  if (!response.ok) {
    throw new Error(resonseData.errors[0]);
  }

  return resonseData;
}

export async function loginUserApi(phoneNumber, password) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/Login`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        phoneNumber: phoneNumber,
        password: password,
      }),
    }
  );
  if (response.status === 401) {
    console.error("Token is invalid. Please login again.");
    return;
  }
  const responseData = await response.json();
  if (!response.ok || responseData.error) {
    throw new Error(responseData.error);
  }
  return responseData;
}

export async function verifyRefreshTokenApi(refreshToken) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/VerifyRefreshToken?refreshToken=${refreshToken}`,
    {
      method: "POST",
    }
  );

  if (response.status === 401) {
    console.error("Token is invalid. Please login again.");
    return;
  }

  if (!response.ok) {
    throw new Error("Failed get verify refresh token");
  }

  return response.json();
}

export async function uploadImageApi(userId, formData, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`User/Upload-Image`, formData)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/Upload-Image?UserId=${userId}`,
  //   {
  //     method: "POST",
  //     header: {
  //       Authorization: `Bearer ${token}`,
  //     },
  //     body: formData,
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // if (!response.ok) {
  //   throw new Error("Failed get verify refresh token");
  // }

  // return response.json();
}

export async function verifyTwoFactorAuthApi(userId, code) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/VerifyPhoneNumber?userId=${userId}&&code=${code}`,
    {
      method: "POST",
    }
  );

  if (response.status === 401) {
    console.error("Token is invalid. Please login again.");
    return;
  }

  const responseData = await response.json();

  if (!response.ok || responseData.error) {
    throw new Error(responseData.error);
  }

  return responseData;
}

export async function enableDisableTwoFactorAuthApi(userId, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`Authentication/EnableDisableTwoFactor?userId=${userId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Authentication/EnableDisableTwoFactor?userId=${userId}`,
  //   {
  //     method: "POST",
  //     header: {
  //       Authorization: `Bearer ${token}`,
  //     },
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // if (!response.ok) {
  //   throw new Error("Failed get verify refresh token");
  // }

  // return response.json();
}
