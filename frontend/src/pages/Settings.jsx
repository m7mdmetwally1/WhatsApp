import { useNavigate } from "react-router-dom";
import { useQueryClient } from "@tanstack/react-query";
import { useQuery } from "@tanstack/react-query";
import {
  useUploadImage,
  useEnableDisableTwoFactorAuth,
} from "../features/user/useUser";
import { ClipLoader } from "react-spinners";

function Settings() {
  const navigate = useNavigate();
  const queryClient = useQueryClient();

  const { data: imageUrl } = useQuery({ queryKey: ["imageUrl"] });
  const { data: token } = useQuery({ queryKey: ["token"] });
  const { data: userId } = useQuery({ queryKey: ["userId"] });
  const { EnableDisableTwoFactorAuth } = useEnableDisableTwoFactorAuth();

  const { data: isTwoFactorEnabled } = useQuery({
    queryKey: ["isTwoFactorEnabled"],
  });

  const { uploadImage, isLoading } = useUploadImage();

  function changeTwoFactorAuth() {
    EnableDisableTwoFactorAuth();
  }

  function logout() {
    queryClient.removeQueries(["userId"]);
    queryClient.removeQueries(["token"]);
    localStorage.removeItem("refreshToken");

    navigate("/login");
  }

  function handleImageUpload(event) {
    const file = event.target.files[0];
    const formData = new FormData();
    formData.append("ImageUrl", file);

    if (!file) return;

    uploadImage({ userId, formData, token });
    event.target.value = "";
  }

  return (
    <div className="flex flex-row w-full h-full justify-between ">
      <div className="flex flex-row w-full h-full dark:text-white">
        <div className="relative  mr-10 ml-10 flex flex-col">
          <img
            src={imageUrl || "/default-image.webp"}
            alt=""
            className="w-[150px] h-[150px] rounded-full border border-gray-300 shadow-md"
          />
          {isLoading && (
            <div className="absolute left-[50px] top-[55px] -10 flex justify-center items-center">
              <ClipLoader color="#36d7b7" size={30} />
            </div>
          )}
          <div className="justify-center items-center flex flex-row">
            <label
              htmlFor="imageUpload"
              className="border pr-2 pl-2   rounded-full bg-green-500 mt-5 text-white cursor-pointer"
            >
              Update Photo
            </label>
            <input
              id="imageUpload"
              type="file"
              className="hidden mt-2"
              onChange={handleImageUpload}
            />
          </div>
        </div>

        <div className="flex flex-col">
          <div className="mb-5">
            <p className="font-bold"></p>
          </div>
          <div className="mb-5">
            <p className="text-gray-400">UserName</p>
            <p>Metwally</p>
          </div>
          <div className="mb-5">
            <p className="text-gray-400">Email</p>
            <p>
              {" "}
              [None]{" "}
              <span className=" ml-1 text-green-500 cursor-pointer">
                Add
              </span>{" "}
            </p>
          </div>
          <div className="mb-5 flex flex-col">
            <p className="text-gray-400">2Factor Auth</p>
            <p className="">
              <span className="ml-1 mr-2 ">
                {isTwoFactorEnabled ? "Enabled" : "Disabled"}
              </span>

              <span
                className="ml-1 text-green-500 cursor-pointer"
                onClick={() => changeTwoFactorAuth()}
              >
                {isTwoFactorEnabled ? "Disable" : "Enable"}
              </span>
            </p>
          </div>
          <div className="mb-5">
            <p className="text-gray-400">Role</p>
            <p className="">User</p>
          </div>
          <div className="mb-5"></div>
        </div>
      </div>

      <div>
        <div className="mr-20">
          <button
            className="border pr-5 pl-5 p-2 rounded-full bg-green-500 text-white"
            onClick={() => logout()}
          >
            Logout
          </button>
        </div>
      </div>
    </div>
  );
}

export default Settings;
