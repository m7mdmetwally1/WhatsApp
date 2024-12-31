import { setupAxiosInterceptors, axiosInstance } from "../utils/axiosInstance";

export async function getChats(userId, queryClient) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .get(`Chat/MyChats?userId=${userId}`)
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });
}

export async function createIndividualChat(
  userId,
  number,
  customeName,
  queryClient
) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`User/add-friend`, {
      SenderUserId: `${userId}`,
      FriendNumber: `${number}`,
      CustomName: `${customeName}`,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/add-friend?SenderUserId=${userId}&FriendNumber=${number}&CustomName=${customeName}`,
  //   {
  //     method: "POST",
  //     headers: {
  //       Authorization: `Bearer ${token}`,
  //     },
  //   }
  // );

  // return response.json();
}

export async function createGroupChat(members, userId, queryClient, name) {
  setupAxiosInterceptors(queryClient);

  return axiosInstance
    .post(`Chat/CreateGroupChat`, {
      groupChatUsers: members.map((member) => member.friendId),
      name: name,
      groupChatCreator: userId,
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.error(error);
      throw error;
    });

  // const response = await fetch(
  //   `http://localhost:5233/api/Chat/CreateGroupChat`,
  //   {
  //     method: "POST",
  //     headers: {
  //       "Content-Type": "application/json",
  //       Authorization: `Bearer ${token}`,
  //     },
  //     body: JSON.stringify({
  //       groupChatUsers: members.map((member) => member.friendId),
  //       name: name,
  //       groupChatCreator: userId,
  //     }),
  //   }
  // );

  // if (response.status === 401) {
  //   console.error("Token is invalid. Please login again.");
  //   return;
  // }

  // const responseData = await response.json();

  // if (!response.ok || responseData.error) {
  //   throw new Error(responseData.error);
  // }

  // return responseData;
}
