import { useForm } from "react-hook-form";
import { useVerifyTwoFactorAuth } from "../features/user/useUser";
import { ClipLoader } from "react-spinners";

function CheckTwoFactor() {
  const { register, formState, handleSubmit, reset } = useForm();
  const { errors } = formState;
  const { isLoading, verifyTwoFactorAuth } = useVerifyTwoFactorAuth();

  function onSubmit({ code }) {
    verifyTwoFactorAuth(
      { code },
      {
        onSettled: () => reset(),
      }
    );
  }

  return (
    <div
      className="flex relative  bg-neutral-200 flex-row   h-screen  p-20 "
      style={{
        backgroundImage: `url('/photo9.jpg')`,
        backgroundSize: "cover",
        backgroundPosition: "center",
      }}
    >
      {isLoading ? (
        <div className="absolute bottom-30 left-[400px] pt-10 pr-10 pl-10 items-center justify-center bg-white flex flex-col border rounded-lg  h-1/5">
          <ClipLoader color="#36d7b7" size={50} />
        </div>
      ) : (
        <div className="absolute bottom-30 left-[200px] pt-10 pr-10 pl-10 bg-white flex flex-col border rounded-lg h-2/5">
          <form
            action=""
            className="flex flex-col items-center "
            onSubmit={handleSubmit(onSubmit)}
          >
            <input
              type="number"
              placeholder="code"
              id="code"
              className={`mt-10  w-full border rounded-lg p-5 h-10 outline-none ${
                errors.code ? "border-red-500" : ""
              }`}
              {...register("code", {
                required: "This field is required",
              })}
            />
            {errors.code && (
              <p className="text-red-500 test-start text-sm mt-2 w-full">
                {errors.number.message}
              </p>
            )}

            <button className="bg-green-600 w-full mt-5  border rounded-lg h-10  text-center text-white">
              Confirm
            </button>
          </form>
        </div>
      )}
    </div>
  );
}

export default CheckTwoFactor;
