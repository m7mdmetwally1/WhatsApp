export async function getChats(userId, token) {
  const response = await fetch(
    `http://localhost:5233/api/Chat/MyChats?userId=${userId}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  return response.json();
}

export async function createIndividualChat(userId, number, customeName, token) {
  const response = await fetch(
    `http://localhost:5233/add-friend?SenderUserId=${userId}&FriendNumber=${number}&CustomName=${customeName}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  return response.json();
}

export async function createGroupChat(members, userId, token, name) {
  const response = await fetch(
    `http://localhost:5233/api/Chat/CreateGroupChat`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        groupChatUsers: members.map((member) => member.friendId),
        name: name,
        groupChatCreator: userId,
      }),
    }
  );

  const responseData = await response.json();

  if (!response.ok || responseData.error) {
    throw new Error(responseData.error);
  }

  return responseData;
}
