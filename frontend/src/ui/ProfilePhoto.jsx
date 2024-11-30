import { VscAccount } from "react-icons/vsc";

function ProfilePhoto() {
  return (
    <div className="ml-3">
      <img
        src="/default-image.webp"
        alt="Profile"
        className="w-10 h-10 rounded-full border border-gray-300 shadow-md"
      />
    </div>
  );
}

export default ProfilePhoto;
