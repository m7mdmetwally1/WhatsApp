import { setupAxiosInterceptors, axiosInstance } from "../utils/axiosInstance";

export async function getMessages(userId, chatId, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .get(`Messages/IndividualChat?userId=${userId}&chatId=${chatId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Messages/IndividualChat?userId=${userId}&chatId=${chatId}`,
  //   {
  //     headers: {
  //       Authorization: `Bearer ${token}`,
  //     },
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // return response.json();
}

export async function getGroupMessages(userId, chatId, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .get(`Messages/GroupChat?userId=${userId}&chatId=${chatId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Messages/GroupChat?userId=${userId}&chatId=${chatId}`,
  //   {
  //     headers: {
  //       Authorization: `Bearer ${token}`,
  //     },
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // console.log(response.json());

  // return response.json();
}

export async function openIndividualChat(userId, chatId, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`Messages/Open/IndividualChat?userId=${userId}&chatId=${chatId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Messages/Open/IndividualChat?userId=${userId}&chatId=${chatId}`,
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
  //   throw new Error("Failed to open individual chat");
  // }

  // return response.json();
}

export async function openGroupChat(userId, chatId, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`Messages/Open/GroupChat?userId=${userId}&chatId=${chatId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Messages/Open/GroupChat?userId=${userId}&chatId=${chatId}`,
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
  //   throw new Error("Failed to open individual chat");
  // }

  // return response.json();
}

export async function insertIndividualChatMessage(
  userId,
  chatId,
  content,
  queryClient
) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`Messages/InsertMessage/IndividualChat`, {
      content: `${content}`,
      userId: `${userId}`,
      chatId: `${chatId}`,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Messages/InsertMessage/IndividualChat`,
  //   {
  //     method: "POST",
  //     headers: {
  //       "Content-Type": "application/json",
  //       Authorization: `Bearer ${token}`,
  //     },
  //     body: JSON.stringify({
  //       content: `${content}`,
  //       userId: `${userId}`,
  //       chatId: `${chatId}`,
  //     }),
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // if (!response.ok) {
  //   throw new Error("Failed to insert individual chat message");
  // }

  // return response.json();
}

export async function insertGroupChatMessage(
  userId,
  chatId,
  content,
  queryClient
) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`Messages/InsertMessage/GroupChat`, {
      content: `${content}`,
      userId: `${userId}`,
      chatId: `${chatId}`,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Messages/InsertMessage/GroupChat`,
  //   {
  //     method: "POST",
  //     headers: {
  //       "Content-Type": "application/json",
  //       Authorization: `Bearer ${token}`,
  //     },
  //     body: JSON.stringify({
  //       content: `${content}`,
  //       userId: `${userId}`,
  //       chatId: `${chatId}`,
  //     }),
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // if (!response.ok) {
  //   throw new Error("Failed to insert Group chat message");
  // }

  // return response.json();
}
