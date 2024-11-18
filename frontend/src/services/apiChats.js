export async function getChats() {
  const response = await fetch(
    `http://localhost:5233/api/Chat/MyChats?userId=54583280-6e93-4ea8-b1cf-4ad407fa7cc1`
  );

  return response.json();
}
