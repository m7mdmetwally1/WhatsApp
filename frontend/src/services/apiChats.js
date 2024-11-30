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

export async function createIndividualChat(userId, number, customeName) {
  const response = await fetch(
    `http://localhost:5233/add-friend?SenderUserId=${userId}&FriendNumber=${number}&CustomName=${customeName}`,
    { method: "POST" }
  );

  return response.json();
}

export async function createGroupChat(members, userId) {
  const response = await fetch(
    `http://localhost:5233/api/Chat/CreateGroupChat`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        groupChatUsers: members.map((member) => member.friendId),
        name: `new Group`,
        groupChatCreator: userId,
      }),
    }
  );

  return response.json();
}
