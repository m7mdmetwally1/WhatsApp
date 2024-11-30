export async function getMessages(userId, chatId) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/IndividualChat?userId=${userId}&chatId=${chatId}`
  );

  return response.json();
}

export async function getGroupMessages(userId, chatId) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/GroupChat?userId=${userId}&chatId=${chatId}`
  );

  return response.json();
}

export async function openIndividualChat(userId, chatId) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/Open/IndividualChat?userId=${userId}&chatId=${chatId}`,
    {
      method: "POST",
    }
  );

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed to open individual chat");
  }

  return response.json();
}

export async function openGroupChat(userId, chatId) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/Open/GroupChat?userId=${userId}&chatId=${chatId}`,
    {
      method: "POST",
    }
  );

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed to open individual chat");
  }

  return response.json();
}

export async function insertIndividualChatMessage(userId, chatId, content) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/InsertMessage/IndividualChat`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        content: `${content}`,
        userId: `${userId}`,
        chatId: `${chatId}`,
      }),
    }
  );

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed to insert individual chat message");
  }

  return response.json();
}

export async function insertGroupChatMessage(userId, chatId, content) {
  const response = await fetch(
    `http://localhost:5233/api/Messages/InsertMessage/GroupChat`,
    {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        content: `${content}`,
        userId: `${userId}`,
        chatId: `${chatId}`,
      }),
    }
  );

  if (!response.ok) {
    console.log("not success");
    throw new Error("Failed to insert Group chat message");
  }

  return response.json();
}
