export async function getFriends(userId, token) {
  const response = await fetch(
    `http://localhost:5233/my-friends?userId=${userId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    throw new Error("Failed get friends data");
  }

  return response.json();
}

export async function createUserApi(
  firstName,
  lastName,
  password,
  phoneNumber,
  token
) {
  const response = await fetch(`http://localhost:5233/api/Authentication`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    },
    body: JSON.stringify({
      phoneNumber: phoneNumber,
      firstName: firstName,
      lastName: lastName,
      password: password,
    }),
  });
  const resonseData = await response.json();

  if (!response.ok) {
    throw new Error(resonseData.errors[0]);
  }

  return resonseData;
}

export async function loginUserApi(phoneNumber, password, token) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/Login`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        phoneNumber: phoneNumber,
        password: password,
      }),
    }
  );

  const responseData = await response.json();

  if (!response.ok || responseData.error) {
    throw new Error(responseData.error);
  }

  return responseData;
}

export async function verifyRefreshTokenApi(refreshToken, token) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/VerifyRefreshToken?refreshToken=${refreshToken}`,
    {
      method: "POST",
      header: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    throw new Error("Failed get verify refresh token");
  }

  return response.json();
}

export async function uploadImageApi(userId, formData, token) {
  const response = await fetch(
    `http://localhost:5233/Upload-Image?UserId=${userId}`,
    {
      method: "POST",
      header: {
        Authorization: `Bearer ${token}`,
      },
      body: formData,
    }
  );

  if (!response.ok) {
    throw new Error("Failed get verify refresh token");
  }

  return response.json();
}

export async function verifyTwoFactorAuthApi(userId, code, token) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/VerifyPhoneNumber?userId=${userId}&&code=${code}`,
    {
      method: "POST",
      header: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  const responseData = await response.json();

  if (!response.ok || responseData.error) {
    throw new Error(responseData.error);
  }

  return responseData;
}

export async function enableDisableTwoFactorAuthApi(userId, token) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/EnableDisableTwoFactor?userId=${userId}`,
    {
      method: "POST",
      header: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    throw new Error("Failed get verify refresh token");
  }

  return response.json();
}
