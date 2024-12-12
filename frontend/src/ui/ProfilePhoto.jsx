import { useQuery } from "@tanstack/react-query";

function ProfilePhoto() {
  const { data: imageUrl } = useQuery({ queryKey: ["imageUrl"] });

  return (
    <div className="ml-3">
      <img
        src={imageUrl || "/default-image.webp"}
        alt="Profile"
        className="w-10 h-10 rounded-full border border-gray-300 shadow-md"
      />
    </div>
  );
}

export default ProfilePhoto;
