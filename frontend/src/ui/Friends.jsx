import Friend from "./Friend";

function Friends({ members, setMembers }) {
  const data = [
    { id: 0, name: "mohamed ebrahim", status: "one day i will reach" },
    { id: 1, name: "ahmed kamel", status: "one day i will reach" },
    { id: 2, name: "doha metwally", status: "one day i will reach" },
    { id: 3, name: "heba metwally", status: "one day i will reach" },
    { id: 4, name: "ahmed metwally", status: "one day i will reach" },
    { id: 5, name: "m7md ra2ft", status: "one day i will reach" },
  ];

  return (
    <div
      className="w-1/2 border p-5 overflow-y-scroll"
      style={{
        maxHeight: "70vh",
      }}
    >
      <Friend data={data} members={members} setMembers={setMembers} />
    </div>
  );
}

export default Friends;
