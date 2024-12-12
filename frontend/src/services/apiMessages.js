export async function getMessages(userId, chatId, token) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/IndividualChat?userId=${userId}&chatId=${chatId}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  return response.json();
}

export async function getGroupMessages(userId, chatId, token) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/GroupChat?userId=${userId}&chatId=${chatId}`,
    {
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  return response.json();
}

export async function openIndividualChat(userId, chatId, token) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/Open/IndividualChat?userId=${userId}&chatId=${chatId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    throw new Error("Failed to open individual chat");
  }

  return response.json();
}

export async function openGroupChat(userId, chatId, token) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/Open/GroupChat?userId=${userId}&chatId=${chatId}`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${token}`,
      },
    }
  );

  if (!response.ok) {
    throw new Error("Failed to open individual chat");
  }

  return response.json();
}

export async function insertIndividualChatMessage(
  userId,
  chatId,
  content,
  token
) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/InsertMessage/IndividualChat`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        content: `${content}`,
        userId: `${userId}`,
        chatId: `${chatId}`,
      }),
    }
  );

  if (!response.ok) {
    throw new Error("Failed to insert individual chat message");
  }

  return response.json();
}

export async function insertGroupChatMessage(userId, chatId, content, token) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/InsertMessage/GroupChat`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify({
        content: `${content}`,
        userId: `${userId}`,
        chatId: `${chatId}`,
      }),
    }
  );

  if (!response.ok) {
    throw new Error("Failed to insert Group chat message");
  }

  return response.json();
}
