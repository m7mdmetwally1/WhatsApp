export async function getFriends(userId) {
  const response = await fetch(
    `http://localhost:5233/my-friends?userId=${userId}`,
    {
      method: "POST",
    }
  );

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed get friends data");
  }

  return response.json();
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

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed get friends data");
  }

  return response.json();
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

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed get friends data");
  }

  return response.json();
}

export async function verifyRefreshTokenApi(refreshToken) {
  const response = await fetch(
    `http://localhost:5233/api/Authentication/VerifyRefreshToken?refreshToken=${refreshToken}`,
    {
      method: "POST",
    }
  );

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed get verify refresh token");
  }

  return response.json();
}
